using SubSonic;
using PowerPOS;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerInventory.Support
{
    public partial class InventoryMage
    {
        public string ItemNo;
        public int InventoryLocationID;

        SortedList<DateTime, StockInElement> MasterData;
        List<StockOutElement> IgnoredStockOut;
        List<StockOutElement> UndeductedSalesData;

        public InventoryMage(string itemNo, int inventoryLocationID)
        {
            ItemNo = itemNo;
            InventoryLocationID = inventoryLocationID;
            MasterData = new SortedList<DateTime, StockInElement>();
            IgnoredStockOut = new List<StockOutElement>();
            UndeductedSalesData = new List<StockOutElement>();
        }

        public void GenerateAllMovement()
        {
            MasterData = new SortedList<DateTime, StockInElement>();
            
            string sqlString;
            IDataReader Rdr;

            #region *) Generate all Manual Stock In and Adjustment In and Negative Sales
            sqlString =
                "DECLARE @ItemNo NVARCHAR(MAX); " +
                "DECLARE @InventoryLocationID INT; " +
                "SET @ItemNo = N'" + ItemNo + "'; " +
                "SET @InventoryLocationID = " + InventoryLocationID.ToString("N0") + "; " +
                "SELECT MovementType, InventoryDetRefNo, InventoryDate, Quantity " +
                "FROM InventoryHdr IH " +
                    "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                "WHERE MovementType IN ('Stock In', 'Adjustment In') " +
                    "AND ItemNo = @ItemNo AND InventoryLocationID = @InventoryLocationID " +
                    "AND (ID.Remark IS NULL OR ID.Remark NOT LIKE 'Stock Take Adj. Id:%') " + // Exclude the Stock Take Adjustment
                    //"AND (IH.Remark IS NULL OR IH.Remark NOT LIKE 'Stock in for order %') " + // Exclude the Negative Sales
                "ORDER BY InventoryDate ASC ";
            
            Rdr = DataService.GetReader(new QueryCommand(sqlString));

            while (Rdr.Read())
            {
                DateTime CurrDate = (DateTime)Rdr[2];
                while (MasterData.ContainsKey(CurrDate))
                    CurrDate = CurrDate.AddSeconds(1);

                MasterData.Add(CurrDate, new StockInElement(Rdr[0].ToString(), Rdr[1].ToString(), (DateTime)Rdr[2], (int)Rdr[3]));
            }

            Rdr.Close();
            #endregion

            //if (MasterData.Count == 0)
            //    throw new Exception("(warning)No Stock In / Adjustment In has been done for ItemNo " + ItemNo + " in InventoryLocationID " + InventoryLocationID.ToString("N0"));

            Queue<StockOutElement> tmpStockTake = new Queue<StockOutElement>();
            #region *) Generate all Stock Take Info
            sqlString =
                "DECLARE @ItemNo NVARCHAR(MAX); " +
                "DECLARE @InventoryLocationID INT; " +
                "SET @ItemNo = N'" + ItemNo + "'; " +
                "SET @InventoryLocationID = " + InventoryLocationID.ToString("N0") + "; " +
                "SELECT 'Stock Take', StockTakeID, StockTakeDate, StockTakeQty " +
                "FROM StockTake ST " +
                "WHERE ItemNo = @ItemNo AND InventoryLocationID = @InventoryLocationID " +
                "ORDER BY StockTakeDate, StockTakeID ";

            Rdr = DataService.GetReader(new QueryCommand(sqlString));

            while (Rdr.Read())
            {
                tmpStockTake.Enqueue(new StockOutElement(Rdr[0].ToString(), Rdr[1].ToString(), (DateTime)Rdr[2], (int)Rdr[3]));
            }

            Rdr.Close();
            #endregion

            Queue<StockOutElement> tmpStockOut = new Queue<StockOutElement>();
            #region *) Generate all Stock Out, Adjustment Out, and Sales
            sqlString =
                "DECLARE @ItemNo NVARCHAR(MAX); " +
                "DECLARE @InventoryLocationID INT; " +
                "SET @ItemNo = N'" + ItemNo + "'; " +
                "SET @InventoryLocationID = " + InventoryLocationID.ToString("N0") + "; " +
                "SELECT MovementType, InventoryDetRefNo AS RefNo, InventoryDate, Quantity " +
                "FROM InventoryHdr IH " +
                    "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                "WHERE ((MovementType = 'Stock Out' AND StockOutReasonID <> 0) OR MovementType = 'Adjustment Out') " +
                    "AND ItemNo = @ItemNo AND InventoryLocationID = @InventoryLocationID " +
                    "AND (ID.Remark IS NULL OR ID.Remark NOT LIKE 'Stock Take Adj. Id:%') " +
                "UNION ALL "+
                "SELECT 'Sales' AS MovementType, OrderDetID AS RefNo, OrderDate AS InventoryDate, Quantity " +
                "FROM OrderHdr OH " +
                    "INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
                    "INNER JOIN PointOfSale LP ON OH.PointOfSaleID = LP.PointOfSaleID " +
                    "INNER JOIN Outlet LO ON LP.OutletName = LO.OutletName " +
                "WHERE OH.IsVoided = 0 AND OD.IsVoided = 0 " +
                    "AND ItemNo = @ItemNo AND InventoryLocationID = @InventoryLocationID " +
                "ORDER BY InventoryDate ";

            Rdr = DataService.GetReader(new QueryCommand(sqlString));

            while (Rdr.Read())
            {
                tmpStockOut.Enqueue(new StockOutElement(Rdr[0].ToString(), Rdr[1].ToString(), (DateTime)Rdr[2], (int)Rdr[3]));
            }

            Rdr.Close();
            #endregion

            while (tmpStockTake.Count > 0 || tmpStockOut.Count > 0)
            {
                if (tmpStockTake.Count > 0 && tmpStockOut.Count == 0) AssignStockTake(tmpStockTake.Dequeue());
                else if (tmpStockOut.Count > 0 && tmpStockTake.Count == 0) AssignStockOutOrSales(tmpStockOut.Dequeue());
                else if (tmpStockOut.Peek().InventoryDate <= tmpStockTake.Peek().InventoryDate) AssignStockOutOrSales(tmpStockOut.Dequeue());
                else AssignStockTake(tmpStockTake.Dequeue());
            }
        }

        private void AssignStockTake(StockOutElement Inst)
        {
            int LeftOverQuantity = 0;
            #region *) Generate Left Over Quantity
            for (int Counter = 0; Counter < MasterData.Count && MasterData.Values[Counter].InventoryDate < Inst.InventoryDate; Counter++)
            {
                if (MasterData.Values[Counter].QuantityLeft > 0)
                    LeftOverQuantity += MasterData.Values[Counter].QuantityLeft;
            }
            #endregion
            #region *) Generate Undeducted Quantity to be Wiped Out
            for (int Counter = 0; Counter < UndeductedSalesData.Count; Counter++)
            {
                LeftOverQuantity -= UndeductedSalesData[Counter].Quantity;
            }
            #endregion

            if (LeftOverQuantity == Inst.Quantity) return;

            if (LeftOverQuantity < Inst.Quantity)
            {
                #region *) Add new Adjustment In
                MasterData.Add(Inst.InventoryDate, new StockInElement("Stock Take", Inst.ReferenceNo, Inst.InventoryDate, Inst.Quantity - LeftOverQuantity));
                #endregion

                Queue<StockOutElement> tmpStockOut = new Queue<StockOutElement>();
                for (int Counter = 0; Counter < UndeductedSalesData.Count; Counter++)
                    tmpStockOut.Enqueue(UndeductedSalesData[Counter]);

                UndeductedSalesData = new List<StockOutElement>();

                while (tmpStockOut.Count>0)
                    AssignSales(tmpStockOut.Dequeue());
            }
            else
            {
                #region *) Add new Adjustment Out - Based on FIFO
                int UndeductedQuantity = LeftOverQuantity - Inst.Quantity;
                for (int Counter = 0; Counter < MasterData.Count && UndeductedQuantity > 0; Counter++)
                {
                    if (MasterData.Values[Counter].QuantityLeft <= 0) continue;

                    if (MasterData.Values[Counter].QuantityLeft >= UndeductedQuantity)
                    {
                        MasterData.Values[Counter].StockOutList.Add(new StockOutElement(Inst.MovementType, Inst.ReferenceNo, Inst.InventoryDate, UndeductedQuantity));
                        UndeductedQuantity = 0;
                    }
                    else
                    {
                        MasterData.Values[Counter].StockOutList.Add(new StockOutElement(Inst.MovementType, Inst.ReferenceNo, Inst.InventoryDate, MasterData.Values[Counter].QuantityLeft));
                        UndeductedQuantity = UndeductedQuantity - MasterData.Values[Counter].QuantityLeft;
                    }
                }
                #endregion
            }
        }
        private void AssignStockOutOrSales(StockOutElement Inst)
        {
            if (Inst.MovementType == "Sales")
                AssignSales(Inst);
            else
                AssignStockOut(Inst);
        }
        private void AssignSales(StockOutElement Inst)
        {
            int UndeductedQuantity = Inst.Quantity;
            for (int Counter = 0; Counter < MasterData.Count && UndeductedQuantity > 0; Counter++)
            {
                if (MasterData.Values[Counter].QuantityLeft <= 0) continue;

                DateTime InventoryDate = Inst.InventoryDate;
                if (MasterData.Values[Counter].InventoryDate > InventoryDate)
                    InventoryDate = MasterData.Values[Counter].InventoryDate;

                if (MasterData.Values[Counter].QuantityLeft >= UndeductedQuantity)
                {
                    MasterData.Values[Counter].StockOutList.Add(new StockOutElement(Inst.MovementType, Inst.ReferenceNo, InventoryDate, UndeductedQuantity));
                    UndeductedQuantity = 0;
                }
                else
                {
                    MasterData.Values[Counter].StockOutList.Add(new StockOutElement(Inst.MovementType, Inst.ReferenceNo, InventoryDate, MasterData.Values[Counter].QuantityLeft));
                    UndeductedQuantity = UndeductedQuantity - MasterData.Values[Counter].QuantityLeft;
                }
            }

            if (UndeductedQuantity > 0)
                UndeductedSalesData.Add(new StockOutElement(Inst.MovementType, Inst.ReferenceNo, Inst.InventoryDate, UndeductedQuantity));
        }
        private void AssignStockOut(StockOutElement Inst)
        {
            int UndeductedQuantity = Inst.Quantity;
            for (int Counter = 0; Counter < MasterData.Count && UndeductedQuantity > 0; Counter++)
            {
                if (MasterData.Values[Counter].QuantityLeft <= 0) continue;
                if (MasterData.Values[Counter].InventoryDate > Inst.InventoryDate) break;

                DateTime InventoryDate = Inst.InventoryDate;
                if (MasterData.Values[Counter].InventoryDate > InventoryDate)
                    InventoryDate = MasterData.Values[Counter].InventoryDate;

                if (MasterData.Values[Counter].QuantityLeft >= UndeductedQuantity)
                {
                    MasterData.Values[Counter].StockOutList.Add(new StockOutElement(Inst.MovementType, Inst.ReferenceNo, InventoryDate, UndeductedQuantity));
                    UndeductedQuantity = 0;
                }
                else
                {
                    MasterData.Values[Counter].StockOutList.Add(new StockOutElement(Inst.MovementType, Inst.ReferenceNo, InventoryDate, MasterData.Values[Counter].QuantityLeft));
                    UndeductedQuantity = UndeductedQuantity - MasterData.Values[Counter].QuantityLeft;
                }
            }

            if (UndeductedQuantity > 0)
                IgnoredStockOut.Add(new StockOutElement(Inst.MovementType, Inst.ReferenceNo, Inst.InventoryDate, UndeductedQuantity));
        }

        public DataTable ToDataTable()
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("ItemNo", Type.GetType("System.String"));
            Dt.Columns.Add("ItemName", Type.GetType("System.String"));
            Dt.Columns.Add("InventoryLocationID", Type.GetType("System.Int32"));
            Dt.Columns.Add("InventoryLocationName", Type.GetType("System.String"));

            Dt.Columns.Add("StockInMovementType", Type.GetType("System.String"));
            Dt.Columns.Add("StockInReferenceNo", Type.GetType("System.String"));
            Dt.Columns.Add("StockInInventoryDate", Type.GetType("System.DateTime"));
            Dt.Columns.Add("StockInQuantity", Type.GetType("System.Int32"));
            Dt.Columns.Add("StockOutMovementType", Type.GetType("System.String"));
            Dt.Columns.Add("StockOutReferenceNo", Type.GetType("System.String"));
            Dt.Columns.Add("StockOutInventoryDate", Type.GetType("System.DateTime"));
            Dt.Columns.Add("StockOutQuantity", Type.GetType("System.Int32"));

            Item myItem = new Item(ItemNo);
            InventoryLocation myILoc = new InventoryLocation(InventoryLocationID);

            for (int Counter = 0; Counter < MasterData.Count; Counter++)
            {
                for (int cOut = 0; cOut < MasterData.Values[Counter].StockOutList.Count; cOut++)
                {
                    Dt.Rows.Add(new object[]{
                        ItemNo
                        , myItem.IsNew?"": myItem.ItemName
                        ,InventoryLocationID
                        ,myILoc.IsNew ?"":myILoc.InventoryLocationName 
                        , MasterData.Values[Counter].MovementType  
                        , MasterData.Values[Counter].ReferenceNo 
                        , MasterData.Values[Counter].InventoryDate
                        , MasterData.Values[Counter].Quantity 
                        , MasterData.Values[Counter].StockOutList[cOut].MovementType 
                        , MasterData.Values[Counter].StockOutList[cOut].ReferenceNo
                        , MasterData.Values[Counter].StockOutList[cOut].InventoryDate
                        , MasterData.Values[Counter].StockOutList[cOut].Quantity 
                    });
                }
            }

            for (int Counter = 0; Counter < IgnoredStockOut.Count; Counter++)
            {
                Dt.Rows.Add(new object[]{
                    ItemNo
                    , myItem.IsNew?"": myItem.ItemName
                    ,InventoryLocationID
                    ,myILoc.IsNew ?"":myILoc.InventoryLocationName 
                    , "Ignored Stock Out"  
                    , "Ignored Stock Out" 
                    , new DateTime (2000,1,1)
                    , 0 
                    , UndeductedSalesData[Counter].MovementType 
                    , UndeductedSalesData[Counter].ReferenceNo
                    , UndeductedSalesData[Counter].InventoryDate
                    , UndeductedSalesData[Counter].Quantity 
                });
            }

            for (int Counter = 0; Counter < UndeductedSalesData.Count; Counter++)
            {
                Dt.Rows.Add(new object[]{
                    ItemNo
                    , myItem.IsNew?"": myItem.ItemName
                    ,InventoryLocationID
                    ,myILoc.IsNew ?"":myILoc.InventoryLocationName 
                    , "Undeducted"  
                    , "Undeducted" 
                    , new DateTime (2000,1,1)
                    , 0 
                    , UndeductedSalesData[Counter].MovementType 
                    , UndeductedSalesData[Counter].ReferenceNo
                    , UndeductedSalesData[Counter].InventoryDate
                    , UndeductedSalesData[Counter].Quantity 
                });
            }

            return Dt;
        }
        public DataTable GetMagixTemplate()
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("ItemNo", Type.GetType("System.String"));
            Dt.Columns.Add("ItemName", Type.GetType("System.String"));
            Dt.Columns.Add("InventoryLocationID", Type.GetType("System.Int32"));
            Dt.Columns.Add("InventoryLocationName", Type.GetType("System.String"));

            Dt.Columns.Add("StockInMovementType", Type.GetType("System.String"));
            Dt.Columns.Add("StockInReferenceNo", Type.GetType("System.String"));
            Dt.Columns.Add("StockInInventoryDate", Type.GetType("System.DateTime"));
            Dt.Columns.Add("StockInQuantity", Type.GetType("System.Int32"));
            Dt.Columns.Add("StockOutMovementType", Type.GetType("System.String"));
            Dt.Columns.Add("StockOutReferenceNo", Type.GetType("System.String"));
            Dt.Columns.Add("StockOutInventoryDate", Type.GetType("System.DateTime"));
            Dt.Columns.Add("StockOutQuantity", Type.GetType("System.Int32"));

            return Dt;
        }
        public DataTable CastMagix(bool ShowNormalTransaction, bool ShowUndeductedSales, bool ShowUnassignedError)
        {
            DataTable Dt = GetMagixTemplate();

            Item myItem = new Item(ItemNo);
            InventoryLocation myILoc = new InventoryLocation(InventoryLocationID);

            if (ShowNormalTransaction)
            {
                for (int Counter = 0; Counter < MasterData.Count; Counter++)
                {
                    for (int cOut = 0; cOut < MasterData.Values[Counter].StockOutList.Count; cOut++)
                    {
                        Dt.Rows.Add(new object[]{
                        ItemNo
                        , myItem.IsNew?"": myItem.ItemName
                        ,InventoryLocationID
                        ,myILoc.IsNew ?"":myILoc.InventoryLocationName 
                        , MasterData.Values[Counter].MovementType  
                        , MasterData.Values[Counter].ReferenceNo 
                        , MasterData.Values[Counter].InventoryDate
                        , MasterData.Values[Counter].Quantity 
                        , MasterData.Values[Counter].StockOutList[cOut].MovementType 
                        , MasterData.Values[Counter].StockOutList[cOut].ReferenceNo
                        , MasterData.Values[Counter].StockOutList[cOut].InventoryDate
                        , MasterData.Values[Counter].StockOutList[cOut].Quantity 
                    });
                    }
                }
            }

            if (ShowUnassignedError)
            {
                for (int Counter = 0; Counter < IgnoredStockOut.Count; Counter++)
                {
                    Dt.Rows.Add(new object[]{
                    ItemNo
                    , myItem.IsNew?"": myItem.ItemName
                    ,InventoryLocationID
                    ,myILoc.IsNew ?"":myILoc.InventoryLocationName 
                    , "Ignored Stock Out"  
                    , "Ignored Stock Out" 
                    , new DateTime (2000,1,1)
                    , 0 
                    , IgnoredStockOut[Counter].MovementType 
                    , IgnoredStockOut[Counter].ReferenceNo
                    , IgnoredStockOut[Counter].InventoryDate
                    , IgnoredStockOut[Counter].Quantity 
                });
                }
            }

            if (ShowUndeductedSales)
            {
                for (int Counter = 0; Counter < UndeductedSalesData.Count; Counter++)
                {
                    Dt.Rows.Add(new object[]{
                    ItemNo
                    , myItem.IsNew?"": myItem.ItemName
                    ,InventoryLocationID
                    ,myILoc.IsNew ?"":myILoc.InventoryLocationName 
                    , "Undeducted"  
                    , "Undeducted" 
                    , new DateTime (2000,1,1)
                    , 0 
                    , UndeductedSalesData[Counter].MovementType 
                    , UndeductedSalesData[Counter].ReferenceNo
                    , UndeductedSalesData[Counter].InventoryDate
                    , UndeductedSalesData[Counter].Quantity 
                });
                }
            }

            return Dt;
        }
    }
}
