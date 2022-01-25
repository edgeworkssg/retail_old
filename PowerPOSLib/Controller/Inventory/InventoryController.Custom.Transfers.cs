using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
namespace PowerPOS
{
    public partial class InventoryController
    {
        #region "Inventory Transfer"
        /*
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static ViewInventoryTransferCollection FetchPendingTransferInList(int ToLocationID)
        {
            try
            {
                ViewInventoryTransferCollection vTC = new ViewInventoryTransferCollection();
                vTC.Where(ViewInventoryTransfer.Columns.ToInventoryLocationID, ToLocationID).Where(ViewInventoryTransfer.Columns.IsReceived, false).Load();

                return vTC;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        */
        /*
        public bool TransferOut
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string status)
        {
            try
            {
                //Check if order is valid
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }
                if (!IsQuantitySufficient(FromInventoryLocationID, out status))
                {                 
                    return false;
                }
                //Set the header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = FromInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferOut;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(FromInventoryLocationID);
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer

                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd = InvHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                int i = 0;

                //For every inventory detail.. Calculate cost of goods                
                while (i < InvDet.Count)
                {
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();

                    DistributeInventoryDetQuantity(ref cmd, ref i, out status);

                }

                //Create Into Location Transfer Table
                LocationTransfer lt = new LocationTransfer();

                lt.FromInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.FromInventoryLocationID = FromInventoryLocationID;
                lt.ToInventoryLocationID = ToInventoryLocationID;
                lt.TransferFromBy = username;
                cmd.Add(lt.GetInsertCommand(username));

                SubSonic.DataService.ExecuteTransaction(cmd);
                status = "";
                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;

                return false;
            }
        }
        
        public bool TransferOutAutoReceive
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string status)
        {
            try
            {
                //Check if order is valid
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }
                if (!IsQuantitySufficient(FromInventoryLocationID, out status))
                {                    
                    return false;
                }
                //Set the header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = FromInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferOut;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(FromInventoryLocationID);
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer

                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd = InvHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                int i = 0;                

                //For every inventory detail.. Calculate cost of goods                
                while (i < InvDet.Count)
                {
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();

                    DistributeInventoryDetQuantity(ref cmd, ref i, out status);

                }

                //Create Into Location Transfer Table
                LocationTransfer lt = new LocationTransfer();
                
                lt.FromInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.FromInventoryLocationID = FromInventoryLocationID;
                lt.ToInventoryLocationID = ToInventoryLocationID;
                lt.TransferFromBy = username;
                lt.IsReceived = true;
                cmd.Add(lt.GetInsertCommand(username));

                InventoryHdr invHdrReceive = new InventoryHdr();
                invHdrReceive.UserName = username;
                invHdrReceive.InventoryDate = InvHdr.InventoryDate;
                invHdrReceive.InventoryLocationID = ToInventoryLocationID;
                invHdrReceive.MovementType = InventoryController.InventoryMovementType_TransferIn;
                invHdrReceive.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(ToInventoryLocationID);
                cmd.Add(invHdrReceive.GetInsertCommand(username));

                InventoryDetCollection invDevReceive = new InventoryDetCollection();
                InvDet.CopyTo(invDevReceive);
                //Loop through inventory detail and add into transaction
                for (int j = 0; j < invDevReceive.Count; j++)
                {
                    invDevReceive[j].InventoryHdrRefNo = invHdrReceive.InventoryHdrRefNo;
                    invDevReceive[j].InventoryDetRefNo = invHdrReceive.InventoryHdrRefNo + "." + (j + 1).ToString();
                    invDevReceive[j].RemainingQty = invDevReceive[j].Quantity;
                    mycmd = invDevReceive[j].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }                
                                                
                SubSonic.DataService.ExecuteTransaction(cmd);

                status = "";
                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;

                return false;
            }
        }
        */
        /*
        public bool ReceiveTransfer(string username, int LocationTransferID, out string status)
        {
            try
            {
                //Fetch the Initial Transfer Out Info using LocationTransferID
                LocationTransfer lt = new LocationTransfer(LocationTransferID);

                //Check if header and detail is available
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }

                //Set inventory header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = lt.ToInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferIn;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo((int)lt.ToInventoryLocationID);

                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd = InvHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);

                //Mark Location Transfer as Received
                lt.ToInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.TransferReceivedBy = username;
                lt.IsReceived = true;
                cmd.Add(lt.GetUpdateCommand(username));

                //Record discrepancy quantities
                InventoryTransferDiscrepancy discr;

                InventoryDetCollection oldDet = new InventoryDetCollection();
                oldDet.Where(InventoryDet.Columns.InventoryHdrRefNo, lt.FromInventoryHdrRefNo).Load();
                DistributeNewItemNo(); //Distribute value from hash table into the actual structure
                //Mark Quantity differnce
                for (int i = 0; i < oldDet.Count; i++)
                {
                    if (oldDet[i].ItemNo != InvDet[i].ItemNo) //Check if the items match
                    {
                        //Item no doest match... something is wrong here
                        status = "The old Inventory and the new Inventory Line detail does not match.";
                        return false;

                    }

                    //If quantity mismatch, record it down
                    if (oldDet[i].Quantity != InvDet[i].Quantity)
                    {
                        if (InvDet[i].Remark == "") //Reject if no remark has not been metioned
                        {
                            status = "No remark has been stated for discrepancy quantity.";
                            return false;
                        }

                        //Create Discrepancy info
                        discr = new InventoryTransferDiscrepancy();
                        discr.ItemNo = oldDet[i].ItemNo;
                        discr.LocationTransferID = LocationTransferID;
                        discr.Remark = InvDet[i].Remark;
                        discr.DiscrepancyQuantity = InvDet[i].Quantity - oldDet[i].Quantity;
                        discr.IsClosed = false;
                        discr.CostOfGoods = oldDet[i].CostOfGoods;
                        //Check if discrepancy quantity is bigger or lesser
                        if (discr.DiscrepancyQuantity > 0)
                        {
                            //if more quantity - set the extra quantity to new Inventory Line, 
                            //added at the end of inventory line
                            InvDet[i].Quantity = oldDet[i].Quantity;

                            //Create temporary detail
                            InventoryDet tmpDet = new InventoryDet();

                            tmpDet.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                            tmpDet.InventoryDetRefNo = InvDet[i].InventoryDetRefNo + GetInvDetMaxID(out status);
                            tmpDet.Quantity = discr.DiscrepancyQuantity;
                            tmpDet.ItemNo = oldDet[i].ItemNo;
                            //tmpDet.RemainingQty = discr.DiscrepancyQuantity;
                            tmpDet.IsDiscrepancy = true;
                            tmpDet.CostOfGoods = 0; //Set cost of goods to be open for editing...
                            InvDet.Add(tmpDet);
                        }
                        else
                        {
                            //Else Do nothing...                            
                        }
                        cmd.Add(discr.GetInsertCommand(username)); //add discrepancy into the transactions
                    }
                }

                //Loop through inventory detail and add into transaction
                for (int i = 0; i < InvDet.Count; i++)
                {
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    InvDet[i].RemainingQty = InvDet[i].Quantity;
                    mycmd = InvDet[i].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }


                SubSonic.DataService.ExecuteTransaction(cmd);
                status = "";

                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;

                return false;
            }
        }
        */
        /*
        public bool TransferOut
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string status)
        {
            InventoryDetCollection tmpDet;
            tmpDet = InvDet;
            try
            {
                //Check if order is valid
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }
                InventoryDetCollection mergedInvDet;
                if (!IsQuantitySufficient(FromInventoryLocationID, out mergedInvDet, out status))
                {
                    return false;
                }
                InvDet = mergedInvDet;
                //Set the header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = FromInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferOut;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(FromInventoryLocationID);
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer

                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd = InvHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);


                //For every inventory detail.. Calculate cost of goods                
                int index = 0;
                //while (i < InvDet.Count)
                for (int i = 0; i < InvDet.Count; i++)
                {
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    DistributeInventoryDetQuantity(ref cmd, InvDet[i], ref index, out status);
                }

                //Create Into Location Transfer Table
                LocationTransfer lt = new LocationTransfer();

                lt.FromInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.FromInventoryLocationID = FromInventoryLocationID;
                lt.ToInventoryLocationID = ToInventoryLocationID;
                lt.TransferFromBy = username;
                cmd.Add(lt.GetInsertCommand(username));

                SubSonic.DataService.ExecuteTransaction(cmd);
                status = "";
                InvHdr.IsNew = false;
                return true;
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;
                return false;
            }
        }
        */
        //transfer stock out from one location and automatically receive the stock in the other location
        public bool TransferOutAutoReceive
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string status)
        {
            string InventoryRefNoFrom = "";
            string InventoryRefNoTo = "";
            if (ActiveCostingType == CostingTypes.FIFO)
                return TransferOutAutoReceive_FIFO(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
            else if (ActiveCostingType == CostingTypes.FixedAvg)
                return TransferOutAutoReceive_FixedAvg(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
            else if (ActiveCostingType == CostingTypes.WeightedAvg)
                return TransferOutAutoReceive_WeightedAvg(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
            else
            {
                Logger.writeLog("ActiveCostingType " + ActiveCostingType + " is not recognized. Will use FIFO");
                return TransferOutAutoReceive_FIFO(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
            }
        }

        public bool TransferOutAutoReceive
           (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom, out string InventoryRefNoTo, out string status)
        {
            if (ActiveCostingType == CostingTypes.FIFO)
                return TransferOutAutoReceive_FIFO(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
            else if (ActiveCostingType == CostingTypes.FixedAvg)
                return TransferOutAutoReceive_FixedAvg(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
            else if (ActiveCostingType == CostingTypes.WeightedAvg)
                return TransferOutAutoReceive_WeightedAvg(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
            else
            {
                Logger.writeLog("ActiveCostingType " + ActiveCostingType + " is not recognized. Will use FIFO");
                return TransferOutAutoReceive_FIFO(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
            }
        }

        
        public bool TransferOutAutoReceive_FIFO
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom, out string InventoryRefNoTo, out string status)
        {
            //Logger.writeLog("Start Transfer");
            InventoryDetCollection tmpDet;
            tmpDet = InvDet;

            InventoryRefNoFrom = "";
            InventoryRefNoTo = "";

            try
            {
                //Check if order is valid
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }

                InventoryDetCollection mergedInvDet = new InventoryDetCollection();
                //perform check if quantity is sufficient

                /*if (!IsQuantitySufficient(FromInventoryLocationID, out mergedInvDet, out status))
                {

                    return false;
                }
                InvDet = mergedInvDet;*/

                //Set the header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = FromInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferOut;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(FromInventoryLocationID);
                InventoryRefNoFrom = InvHdr.InventoryHdrRefNo;
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer

                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd = InvHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                QueryCommandCollection transferDet = new QueryCommandCollection();
                int index = 0;
                for (int i = 0; i < InvDet.Count; i++)
                {

                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    //set the stock balance
                    //InvDet[i].BalanceBefore = GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, DateTime.Now, out status);

                    if (InvDet[i].Quantity > 0)
                    {
                        //Find the inventory det for the same item that contains balance quantity to be deducted (FIFO)
                        DistributeInventoryDetQuantity(ref transferDet, InvDet[i], ref index, out status);
                        if (status != "")
                        {
                            Logger.writeLog("Error preforming distributeinventorydetquantity on transfer autoreceive. Item no = " + InvDet[i].ItemNo + ": " + status);
                            status = "Error preforming distributeinventorydetquantity on transfer autoreceive: " + status;
                            return false;
                        }
                    }
                    else
                    {
                        //Logger.writeLog("Qty is zero: " + InvDet[i].ItemNo + " Qty: " + InvDet[i].Quantity + " ID: " + InvDet[i].InventoryDetRefNo);
                        //i += 1;
                    }
                }

                cmd.AddRange(transferDet);

                #region *) Update Item Summary

                foreach (var id in InvDet)
                {
                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                        InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                        id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                        id.InventoryDetRefNo, InvHdr.InventoryDate);
                    cmd.AddRange(itemSummaryCmd);
                }

                #endregion
                
                //Create Into Location Transfer Table
                LocationTransfer lt = new LocationTransfer();

                lt.FromInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.FromInventoryLocationID = FromInventoryLocationID;
                lt.ToInventoryLocationID = ToInventoryLocationID;
                lt.TransferFromBy = username;
                lt.IsReceived = true;
                cmd.Add(lt.GetInsertCommand(username));

                //Auto Receive - This portion is for the InventoryHdr of the receiving part
                
                InventoryHdr invHdrReceive = new InventoryHdr();
                invHdrReceive.IsNew = true;
                invHdrReceive.UserName = username;
                invHdrReceive.InventoryDate = InvHdr.InventoryDate.AddSeconds(1);
                invHdrReceive.InventoryLocationID = ToInventoryLocationID;
                invHdrReceive.MovementType = InventoryController.InventoryMovementType_TransferIn;
                invHdrReceive.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(ToInventoryLocationID);
                invHdrReceive.PurchaseOrderNo = InvHdr.PurchaseOrderNo;
                InventoryRefNoTo = invHdrReceive.InventoryHdrRefNo;
                cmd.Add(invHdrReceive.GetInsertCommand(username));

                //QueryCommand[] tmpcmdArray = new QueryCommand[transferDet.Count];
                //transferDet.CopyTo(tmpcmdArray);
                for (int detCount = 0; detCount < transferDet.Count; detCount++)
                {
                    if (transferDet[detCount].CommandSql.Contains("Insert"))
                    {
                        InventoryDet tmp = new InventoryDet();
                        tmp.IsNew = true;
                        for (int paramCount = 0; paramCount < transferDet[detCount].Parameters.Count; paramCount++)
                        {
                            if (transferDet[detCount].Parameters[paramCount].ParameterName.ToLower() != "@createdon" 
                                && transferDet[detCount].Parameters[paramCount].ParameterName.ToLower() != "@createdby" 
                                && transferDet[detCount].Parameters[paramCount].ParameterName.ToLower() != "@modifiedon" 
                                && transferDet[detCount].Parameters[paramCount].ParameterName.ToLower() != "@modifiedby")
                            {
                                tmp.SetColumnValue(transferDet[detCount].Parameters[paramCount].ParameterName.Replace("@",""),
                                    transferDet[detCount].Parameters[paramCount].ParameterValue);
                            }
                        }
                        tmp.InventoryDetRefNo = invHdrReceive.InventoryHdrRefNo + "." + (detCount + 1).ToString();
                        tmp.InventoryHdrRefNo = invHdrReceive.InventoryHdrRefNo;
                        tmp.StockInRefNo = transferDet[detCount].Parameters.GetParameter("@InventoryDetRefNo").ParameterValue.ToString();
                        //tmpcmdArray[detCount].Parameters.GetParameter("@InventoryDetRefNo").ParameterValue = invHdrReceive.InventoryHdrRefNo + "." + (detCount + 1).ToString();
                        //tmpcmdArray[detCount].Parameters.GetParameter("@InventoryHdrRefNo").ParameterValue = invHdrReceive.InventoryHdrRefNo;
                        //tmpcmdArray[detCount].Parameters.GetParameter("@StockInRefNo").ParameterValue = transferDet[detCount].Parameters.GetParameter("@InventoryHdrRefNo").ParameterValue;
                        cmd.Add(tmp.GetInsertCommand(UserInfo.username));

                        var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(tmp.ItemNo,
                            invHdrReceive.InventoryLocationID.GetValueOrDefault(0), invHdrReceive.MovementType,
                            tmp.FactoryPrice, Convert.ToDouble(tmp.Quantity.GetValueOrDefault(0)),
                            tmp.InventoryDetRefNo, invHdrReceive.InventoryDate);
                        cmd.AddRange(itemSummaryCmd);
                    }
                }

                #region *) Update Item Tag Summary

                var itemTagOut = ItemTagController.FetchItemTagUpdate(InvHdr, InvDet);
                if (itemTagOut.Count > 0)
                    cmd.AddRange(itemTagOut);

                var itemTagIn = ItemTagController.FetchItemTagUpdate(invHdrReceive, InvDet);
                if (itemTagIn.Count > 0)
                    cmd.AddRange(itemTagIn);

                #endregion

                SubSonic.DataService.ExecuteTransaction(cmd);
                
                
                status = "";
                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;

                return false;
            }
        }
        public bool TransferOutAutoReceive_FixedAvg
           (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom, out string InventoryRefNoTo, out string status)
        {
            //Logger.writeLog("Start Transfer");
            InventoryDetCollection tmpDet;
            tmpDet = InvDet;
            List<string> ListInventoryHdr = new List<string>();

            InventoryRefNoFrom = "";
            InventoryRefNoTo = "";
            try
            {
                #region *) Validation: Check whether it is a valid order
                if (InvHdr == null | InvDet == null) //Valid Order?
                    throw new Exception("(error)Invalid Inventory data. Unable to receive Inventory.");
                #endregion

                #region *) Validation: Quantity must be larger than zero
                for (int i = 0; i < InvDet.Count; i++)
                    if (InvDet[i].Quantity < 0)
                        throw new Exception("(error)Quantity for Item No [" + InvDet[i].ItemNo + "] must be larger than zero");
                    else if (InvDet[i].Quantity == 0)
                        InvDet.RemoveAt(i--);
                #endregion

                #region *) Validation: check if record already exists
                if (IsRecordExisted(InventoryController.InventoryMovementType_TransferOut))
                    throw new Exception("(error) Record already exists.");
                #endregion

                #region *) Conditioning: Merge all InventoryDet with same name
                InventoryDetCollection mergedInvDet = new InventoryDetCollection();

                MergeInventoryDet(ref mergedInvDet);

                InvDet = mergedInvDet;
                #endregion

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowStockTransferEvenStockIsZero), false))
                {
                    for (int i = 0; i < mergedInvDet.Count; i++)
                    {
                        #region *) Validation: Have enough balance in the warehouse
                        decimal bal = InventoryController.GetStockBalanceQtyByItemSummaryByDate
                            (mergedInvDet[i].ItemNo, FromInventoryLocationID, InvHdr.InventoryDate, out status);

                        if (mergedInvDet[i].Quantity.GetValueOrDefault(0) > bal)
                            throw new Exception("(error)Insufficient quantity to perform stock out for item " +
                                mergedInvDet[i].ItemNo + " (" + mergedInvDet[i].Item.ItemName +
                                "). Onhand quantity is " + bal + " while stock out quantity is " +
                                mergedInvDet[i].Quantity.GetValueOrDefault(0) + ".");
                        #endregion
                    }
                }

                QueryCommandCollection cmd = new QueryCommandCollection();

                #region *) Save: Generate Save Script for Transfer Out InventoryHdr
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = FromInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferOut;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(FromInventoryLocationID);
                InventoryRefNoFrom = InvHdr.InventoryHdrRefNo;
                ListInventoryHdr.Add(InvHdr.InventoryHdrRefNo);
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer

                cmd.Add(InvHdr.GetInsertCommand(username));
                #endregion

                for (int i = 0; i < InvDet.Count; i++)
                {
                    #region *) Save: Generate Save Script for Transfer Out InventoryDet
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();

                    // FactoryPrice should have been declared before, so don't overwrite it anymore
                    //InvDet[i].FactoryPrice = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                    //InvDet[i].CostOfGoods = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));

                    cmd.Add(InvDet[i].GetInsertCommand(username));
                    #endregion
                }
                #region *) Update Item Summary

                foreach (var id in InvDet)
                {
                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                        InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                        id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                        id.InventoryDetRefNo, InvHdr.InventoryDate);
                    cmd.AddRange(itemSummaryCmd);
                }

                #endregion


                InventoryHdr invHdrReceive = new InventoryHdr();
                #region *) Save: Generate Save Script for Transfer In InventoryHdr
                invHdrReceive.IsNew = true;
                invHdrReceive.UserName = username;
                invHdrReceive.InventoryDate = InvHdr.InventoryDate.AddSeconds(1);
                invHdrReceive.InventoryLocationID = ToInventoryLocationID;
                invHdrReceive.MovementType = InventoryController.InventoryMovementType_TransferIn;
                invHdrReceive.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(ToInventoryLocationID);
                invHdrReceive.PurchaseOrderNo = InvHdr.PurchaseOrderNo;
                invHdrReceive.Remark = InvHdr.Remark;
                InventoryRefNoTo = invHdrReceive.InventoryHdrRefNo;
                invHdrReceive.UniqueID = Guid.NewGuid();
                invHdrReceive.Deleted = false;
                ListInventoryHdr.Add(invHdrReceive.InventoryHdrRefNo);
                cmd.Add(invHdrReceive.GetInsertCommand(username));
                #endregion

                for (int i = 0; i < InvDet.Count; i++)
                {
                    #region *) Save: Generate Save Script for Transfer In InventoryDet
                    InventoryDet tmpInvDet = InvDet[i].Clone();
                    tmpInvDet.InventoryHdrRefNo = invHdrReceive.InventoryHdrRefNo;
                    tmpInvDet.InventoryDetRefNo = invHdrReceive.InventoryHdrRefNo + "." + (i + 1).ToString();

                    // FactoryPrice should have been declared before, so don't overwrite it anymore
                    //tmpInvDet.FactoryPrice = ItemSummaryController.GetAvgCostPrice(tmpInvDet.ItemNo, invHdrReceive.InventoryLocationID.GetValueOrDefault(0));
                    //tmpInvDet.CostOfGoods = ItemSummaryController.GetAvgCostPrice(tmpInvDet.ItemNo, invHdrReceive.InventoryLocationID.GetValueOrDefault(0));

                    cmd.Add(tmpInvDet.GetInsertCommand(username));

                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(tmpInvDet.ItemNo,
                        invHdrReceive.InventoryLocationID.GetValueOrDefault(0), invHdrReceive.MovementType,
                        tmpInvDet.FactoryPrice, Convert.ToDouble(tmpInvDet.Quantity.GetValueOrDefault(0)),
                        tmpInvDet.InventoryDetRefNo, invHdrReceive.InventoryDate);
                    cmd.AddRange(itemSummaryCmd);
                    #endregion
                }

                #region *) Save: Generate Save Script for LocationTransfer
                /* Just for a Log. And will be needed for Join purpose */
                LocationTransfer lt = new LocationTransfer();
                lt.FromInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.FromInventoryLocationID = FromInventoryLocationID;
                lt.ToInventoryHdrRefNo = invHdrReceive.InventoryHdrRefNo;
                lt.ToInventoryLocationID = ToInventoryLocationID;
                lt.TransferFromBy = username;
                lt.TransferReceivedBy = username;
                lt.IsReceived = true;
                cmd.Add(lt.GetInsertCommand(username));
                #endregion

                #region *) Update Item Tag Summary

                var itemTagOut = ItemTagController.FetchItemTagUpdate(InvHdr, InvDet);
                if (itemTagOut.Count > 0)
                    cmd.AddRange(itemTagOut);

                var itemTagIn = ItemTagController.FetchItemTagUpdate(invHdrReceive, InvDet);
                if (itemTagIn.Count > 0)
                    cmd.AddRange(itemTagIn);

                #endregion

                SubSonic.DataService.ExecuteTransaction(cmd);

                // check if there are any stock take after the transfer out
                string itemlist = "";
                if (gotStockTakeAfter(out itemlist))
                {
                    StockInAdjustment(username, FromInventoryLocationID, true, true, itemlist, out status);
                }

                // check if there are any stock take after the transfer in
                InventoryHdr tmpHdr = new InventoryHdr();
                InvHdr.CopyTo(tmpHdr); // create copy of InvHdr so that we can restore it later
                InvHdr = invHdrReceive;
                itemlist = "";
                if (gotStockTakeAfter(out itemlist))
                {
                    StockOutAdjustment(username, 1, ToInventoryLocationID, true, true, itemlist, out status);
                }

                //Restore the InvHdr in case we need it again after this
                InvHdr.CopyFrom(tmpHdr);

                status = "";
                return true;
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message.Replace("(error)", "").Replace("(warning)", "");
                InvDet = tmpDet;

                return false;
            }
        }
        public bool TransferOutAutoReceive_WeightedAvg
          (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom, out string InventoryRefNoTo, out string status)
        {
            /* Doing same thing */
            return TransferOutAutoReceive_FixedAvg(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out  status);
        }


        #region transfer with commandcollection

        public bool TransferOutAutoReceiveWithQueryCommand
           (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom, out string InventoryRefNoTo, out QueryCommandCollection cmd, out string status)
        {
            if (ActiveCostingType == CostingTypes.FIFO)
                return TransferOutAutoReceive_FIFO_Command(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out cmd, out status);
            else if (ActiveCostingType == CostingTypes.FixedAvg)
                return TransferOutAutoReceive_FixedAvg_Command(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out cmd, out  status);
            else if (ActiveCostingType == CostingTypes.WeightedAvg)
                return TransferOutAutoReceive_WeightedAvg_Command(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out cmd, out  status);
            else
            {
                Logger.writeLog("ActiveCostingType " + ActiveCostingType + " is not recognized. Will use FIFO");
                return TransferOutAutoReceive_FIFO_Command(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo, out cmd, out  status);
            }
        }

        public bool TransferOutAutoReceive_FIFO_Command
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom, out string InventoryRefNoTo, out QueryCommandCollection cmd, out string status)
        {
            //Logger.writeLog("Start Transfer");
            InventoryDetCollection tmpDet;
            tmpDet = InvDet;

            InventoryRefNoFrom = "";
            InventoryRefNoTo = "";

            cmd = new QueryCommandCollection();

            try
            {
                //Check if order is valid
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }

                InventoryDetCollection mergedInvDet = new InventoryDetCollection();
                //perform check if quantity is sufficient

                /*if (!IsQuantitySufficient(FromInventoryLocationID, out mergedInvDet, out status))
                {

                    return false;
                }
                InvDet = mergedInvDet;*/

                //Set the header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = FromInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferOut;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(FromInventoryLocationID);
                InventoryRefNoFrom = InvHdr.InventoryHdrRefNo;
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer

                
                QueryCommand mycmd = InvHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                QueryCommandCollection transferDet = new QueryCommandCollection();
                int index = 0;
                for (int i = 0; i < InvDet.Count; i++)
                {

                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    //set the stock balance
                    //InvDet[i].BalanceBefore = GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, DateTime.Now, out status);

                    if (InvDet[i].Quantity > 0)
                    {
                        //Find the inventory det for the same item that contains balance quantity to be deducted (FIFO)
                        DistributeInventoryDetQuantity(ref transferDet, InvDet[i], ref index, out status);
                        if (status != "")
                        {
                            Logger.writeLog("Error preforming distributeinventorydetquantity on transfer autoreceive. Item no = " + InvDet[i].ItemNo + ": " + status);
                            status = "Error preforming distributeinventorydetquantity on transfer autoreceive: " + status;
                            return false;
                        }
                    }
                    else
                    {
                        //Logger.writeLog("Qty is zero: " + InvDet[i].ItemNo + " Qty: " + InvDet[i].Quantity + " ID: " + InvDet[i].InventoryDetRefNo);
                        //i += 1;
                    }
                }

                cmd.AddRange(transferDet);

                #region *) Update Item Summary

                foreach (var id in InvDet)
                {
                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                        InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                        id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                        id.InventoryDetRefNo, InvHdr.InventoryDate);
                    cmd.AddRange(itemSummaryCmd);
                }

                #endregion

                //Create Into Location Transfer Table
                LocationTransfer lt = new LocationTransfer();

                lt.FromInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.FromInventoryLocationID = FromInventoryLocationID;
                lt.ToInventoryLocationID = ToInventoryLocationID;
                lt.TransferFromBy = username;
                lt.IsReceived = true;
                cmd.Add(lt.GetInsertCommand(username));

                //Auto Receive - This portion is for the InventoryHdr of the receiving part

                InventoryHdr invHdrReceive = new InventoryHdr();
                invHdrReceive.IsNew = true;
                invHdrReceive.UserName = username;
                invHdrReceive.InventoryDate = InvHdr.InventoryDate.AddSeconds(1);
                invHdrReceive.InventoryLocationID = ToInventoryLocationID;
                invHdrReceive.MovementType = InventoryController.InventoryMovementType_TransferIn;
                invHdrReceive.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(ToInventoryLocationID);
                invHdrReceive.PurchaseOrderNo = InvHdr.PurchaseOrderNo;
                InventoryRefNoTo = invHdrReceive.InventoryHdrRefNo;
                cmd.Add(invHdrReceive.GetInsertCommand(username));

                //QueryCommand[] tmpcmdArray = new QueryCommand[transferDet.Count];
                //transferDet.CopyTo(tmpcmdArray);
                for (int detCount = 0; detCount < transferDet.Count; detCount++)
                {
                    if (transferDet[detCount].CommandSql.Contains("Insert"))
                    {
                        InventoryDet tmp = new InventoryDet();
                        tmp.IsNew = true;
                        for (int paramCount = 0; paramCount < transferDet[detCount].Parameters.Count; paramCount++)
                        {
                            if (transferDet[detCount].Parameters[paramCount].ParameterName.ToLower() != "@createdon"
                                && transferDet[detCount].Parameters[paramCount].ParameterName.ToLower() != "@createdby"
                                && transferDet[detCount].Parameters[paramCount].ParameterName.ToLower() != "@modifiedon"
                                && transferDet[detCount].Parameters[paramCount].ParameterName.ToLower() != "@modifiedby")
                            {
                                tmp.SetColumnValue(transferDet[detCount].Parameters[paramCount].ParameterName.Replace("@", ""),
                                    transferDet[detCount].Parameters[paramCount].ParameterValue);
                            }
                        }
                        tmp.InventoryDetRefNo = invHdrReceive.InventoryHdrRefNo + "." + (detCount + 1).ToString();
                        tmp.InventoryHdrRefNo = invHdrReceive.InventoryHdrRefNo;
                        tmp.StockInRefNo = transferDet[detCount].Parameters.GetParameter("@InventoryDetRefNo").ParameterValue.ToString();
                        //tmpcmdArray[detCount].Parameters.GetParameter("@InventoryDetRefNo").ParameterValue = invHdrReceive.InventoryHdrRefNo + "." + (detCount + 1).ToString();
                        //tmpcmdArray[detCount].Parameters.GetParameter("@InventoryHdrRefNo").ParameterValue = invHdrReceive.InventoryHdrRefNo;
                        //tmpcmdArray[detCount].Parameters.GetParameter("@StockInRefNo").ParameterValue = transferDet[detCount].Parameters.GetParameter("@InventoryHdrRefNo").ParameterValue;
                        cmd.Add(tmp.GetInsertCommand(UserInfo.username));

                        var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(tmp.ItemNo,
                            invHdrReceive.InventoryLocationID.GetValueOrDefault(0), invHdrReceive.MovementType,
                            tmp.FactoryPrice, Convert.ToDouble(tmp.Quantity.GetValueOrDefault(0)),
                            tmp.InventoryDetRefNo, invHdrReceive.InventoryDate);
                        cmd.AddRange(itemSummaryCmd);
                    }
                }

                #region *) Update Item Tag Summary

                var itemTagOut = ItemTagController.FetchItemTagUpdate(InvHdr, InvDet);
                if (itemTagOut.Count > 0)
                    cmd.AddRange(itemTagOut);

                var itemTagIn = ItemTagController.FetchItemTagUpdate(invHdrReceive, InvDet);
                if (itemTagIn.Count > 0)
                    cmd.AddRange(itemTagIn);

                #endregion

                //SubSonic.DataService.ExecuteTransaction(cmd);


                status = "";
                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;

                return false;
            }
        }
        public bool TransferOutAutoReceive_FixedAvg_Command
           (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom, out string InventoryRefNoTo, out QueryCommandCollection cmd, out string status)
        {
            //Logger.writeLog("Start Transfer");
            InventoryDetCollection tmpDet;
            tmpDet = InvDet;
            List<string> ListInventoryHdr = new List<string>();

            cmd = new QueryCommandCollection();

            InventoryRefNoFrom = "";
            InventoryRefNoTo = "";
            try
            {
                #region *) Validation: Check whether it is a valid order
                if (InvHdr == null | InvDet == null) //Valid Order?
                    throw new Exception("(error)Invalid Inventory data. Unable to receive Inventory.");
                #endregion

                #region *) Validation: Quantity must be larger than zero
                for (int i = 0; i < InvDet.Count; i++)
                    if (InvDet[i].Quantity < 0)
                        throw new Exception("(error)Quantity for Item No [" + InvDet[i].ItemNo + "] must be larger than zero");
                    else if (InvDet[i].Quantity == 0)
                        InvDet.RemoveAt(i--);
                #endregion

                #region *) Validation: check if record already exists
                if (IsRecordExisted(InventoryController.InventoryMovementType_TransferOut))
                    throw new Exception("(error) Record already exists.");
                #endregion

                #region *) Conditioning: Merge all InventoryDet with same name
                InventoryDetCollection mergedInvDet = new InventoryDetCollection();

                MergeInventoryDet(ref mergedInvDet);

                InvDet = mergedInvDet;
                #endregion

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowStockTransferEvenStockIsZero), false))
                {
                    for (int i = 0; i < mergedInvDet.Count; i++)
                    {
                        #region *) Validation: Have enough balance in the warehouse
                        decimal bal = InventoryController.GetStockBalanceQtyByItemSummaryByDate
                            (mergedInvDet[i].ItemNo, FromInventoryLocationID, InvHdr.InventoryDate, out status);

                        if (mergedInvDet[i].Quantity.GetValueOrDefault(0) > bal)
                            throw new Exception("(error)Insufficient quantity to perform stock out for item " +
                                mergedInvDet[i].ItemNo + " (" + mergedInvDet[i].Item.ItemName +
                                "). Onhand quantity is " + bal + " while stock out quantity is " +
                                mergedInvDet[i].Quantity.GetValueOrDefault(0) + ".");
                        #endregion
                    }
                }

                
                #region *) Save: Generate Save Script for Transfer Out InventoryHdr
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = FromInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferOut;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(FromInventoryLocationID);
                InventoryRefNoFrom = InvHdr.InventoryHdrRefNo;
                ListInventoryHdr.Add(InvHdr.InventoryHdrRefNo);
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer

                cmd.Add(InvHdr.GetInsertCommand(username));
                #endregion

                for (int i = 0; i < InvDet.Count; i++)
                {
                    #region *) Save: Generate Save Script for Transfer Out InventoryDet
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();

                    // FactoryPrice should have been declared before, so don't overwrite it anymore
                    //InvDet[i].FactoryPrice = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                    //InvDet[i].CostOfGoods = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));

                    cmd.Add(InvDet[i].GetInsertCommand(username));
                    #endregion
                }
                #region *) Update Item Summary

                foreach (var id in InvDet)
                {
                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                        InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                        id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                        id.InventoryDetRefNo, InvHdr.InventoryDate);
                    cmd.AddRange(itemSummaryCmd);
                }

                #endregion


                InventoryHdr invHdrReceive = new InventoryHdr();
                #region *) Save: Generate Save Script for Transfer In InventoryHdr
                invHdrReceive.IsNew = true;
                invHdrReceive.UserName = username;
                invHdrReceive.InventoryDate = InvHdr.InventoryDate.AddSeconds(1);
                invHdrReceive.InventoryLocationID = ToInventoryLocationID;
                invHdrReceive.MovementType = InventoryController.InventoryMovementType_TransferIn;
                invHdrReceive.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(ToInventoryLocationID);
                invHdrReceive.PurchaseOrderNo = InvHdr.PurchaseOrderNo;
                invHdrReceive.Remark = InvHdr.Remark;
                InventoryRefNoTo = invHdrReceive.InventoryHdrRefNo;
                invHdrReceive.UniqueID = Guid.NewGuid();
                invHdrReceive.Deleted = false;
                ListInventoryHdr.Add(invHdrReceive.InventoryHdrRefNo);
                cmd.Add(invHdrReceive.GetInsertCommand(username));
                #endregion

                for (int i = 0; i < InvDet.Count; i++)
                {
                    #region *) Save: Generate Save Script for Transfer In InventoryDet
                    InventoryDet tmpInvDet = InvDet[i].Clone();
                    tmpInvDet.InventoryHdrRefNo = invHdrReceive.InventoryHdrRefNo;
                    tmpInvDet.InventoryDetRefNo = invHdrReceive.InventoryHdrRefNo + "." + (i + 1).ToString();

                    // FactoryPrice should have been declared before, so don't overwrite it anymore
                    //tmpInvDet.FactoryPrice = ItemSummaryController.GetAvgCostPrice(tmpInvDet.ItemNo, invHdrReceive.InventoryLocationID.GetValueOrDefault(0));
                    //tmpInvDet.CostOfGoods = ItemSummaryController.GetAvgCostPrice(tmpInvDet.ItemNo, invHdrReceive.InventoryLocationID.GetValueOrDefault(0));

                    cmd.Add(tmpInvDet.GetInsertCommand(username));

                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(tmpInvDet.ItemNo,
                        invHdrReceive.InventoryLocationID.GetValueOrDefault(0), invHdrReceive.MovementType,
                        tmpInvDet.FactoryPrice, Convert.ToDouble(tmpInvDet.Quantity.GetValueOrDefault(0)),
                        tmpInvDet.InventoryDetRefNo, invHdrReceive.InventoryDate);
                    cmd.AddRange(itemSummaryCmd);
                    #endregion
                }

                #region *) Save: Generate Save Script for LocationTransfer
                /* Just for a Log. And will be needed for Join purpose */
                LocationTransfer lt = new LocationTransfer();
                lt.FromInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.FromInventoryLocationID = FromInventoryLocationID;
                lt.ToInventoryHdrRefNo = invHdrReceive.InventoryHdrRefNo;
                lt.ToInventoryLocationID = ToInventoryLocationID;
                lt.TransferFromBy = username;
                lt.TransferReceivedBy = username;
                lt.IsReceived = true;
                cmd.Add(lt.GetInsertCommand(username));
                #endregion

                #region *) Update Item Tag Summary

                var itemTagOut = ItemTagController.FetchItemTagUpdate(InvHdr, InvDet);
                if (itemTagOut.Count > 0)
                    cmd.AddRange(itemTagOut);

                var itemTagIn = ItemTagController.FetchItemTagUpdate(invHdrReceive, InvDet);
                if (itemTagIn.Count > 0)
                    cmd.AddRange(itemTagIn);

                #endregion

                // check if there are any stock take after the transfer out
                string itemlist = "";
                
                if (gotStockTakeAfter(out itemlist))
                {
                    QueryCommandCollection cmdStockIn = new QueryCommandCollection();

                    if (!StockInAdjustmentCommand(username, FromInventoryLocationID, true, true, itemlist, out cmdStockIn, out status))
                        throw new Exception(status);

                    cmd.AddRange(cmdStockIn);
                }

                // check if there are any stock take after the transfer in
                InventoryHdr tmpHdr = new InventoryHdr();
                InvHdr.CopyTo(tmpHdr); // create copy of InvHdr so that we can restore it later
                InvHdr = invHdrReceive;
                itemlist = "";
                if (gotStockTakeAfter(out itemlist))
                {
                    QueryCommandCollection cmdStockOut = new QueryCommandCollection();

                    if (!StockOutAdjustmentCommand(username, 1, ToInventoryLocationID, true, true, itemlist, out cmdStockOut, out status))
                        throw new Exception(status);

                    cmd.AddRange(cmdStockOut);
                }

                //Restore the InvHdr in case we need it again after this
                InvHdr.CopyFrom(tmpHdr);

                status = "";
                return true;
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message.Replace("(error)", "").Replace("(warning)", "");
                InvDet = tmpDet;

                return false;
            }
        }
        public bool TransferOutAutoReceive_WeightedAvg_Command
          (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom, out string InventoryRefNoTo, out QueryCommandCollection cmd, out string status)
        {
            /* Doing same thing */
            return TransferOutAutoReceive_FixedAvg_Command(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom, out InventoryRefNoTo,out cmd, out  status);
        }


        #endregion
        //transfer stock out from one location and automatically receive the stock in the other location
        public bool TransferOut
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom ,out string status)
        {
           
            return TransferOut_FIFO(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoFrom,  out  status);
            
        }

        public bool TransferOut_FIFO
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoFrom,  out string status)
        {
            //Logger.writeLog("Start Transfer");
            InventoryDetCollection tmpDet;
            tmpDet = InvDet;

            InventoryRefNoFrom = "";
           

            try
            {
                //Check if order is valid
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }

                InventoryDetCollection mergedInvDet = new InventoryDetCollection();
                //perform check if quantity is sufficient

                //Set the header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = FromInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferOut;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(FromInventoryLocationID);
                InventoryRefNoFrom = InvHdr.InventoryHdrRefNo;
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer

                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd = InvHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                QueryCommandCollection transferDet = new QueryCommandCollection();
                
                for (int i = 0; i < InvDet.Count; i++)
                {

                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    InvDet[i].RemainingQty = InvDet[i].Quantity.GetValueOrDefault(0);

                    InvDet[i].Gst = GetGST();

                    #region *) Save: Generate Save Script for InventoryDet
                    if (InvDet[i].IsNew)
                    {
                        mycmd = InvDet[i].GetInsertCommand(UserInfo.username);
                        cmd.Add(mycmd);
                    }
                    else
                    {
                        mycmd = InvDet[i].GetUpdateCommand(UserInfo.username);
                        cmd.Add(mycmd);
                    }
                    #endregion
                }

                cmd.AddRange(transferDet);

                #region *) Update Item Summary

                foreach (var id in InvDet)
                {
                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                        InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                        id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                        id.InventoryDetRefNo, InvHdr.InventoryDate);
                    cmd.AddRange(itemSummaryCmd);
                }

                #endregion

                SubSonic.DataService.ExecuteTransaction(cmd);


                status = "";
                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;

                return false;
            }
        }

        public bool TransferIn
            (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoTo, out string status)
        {
            return TransferIn_FIFO(username, FromInventoryLocationID, ToInventoryLocationID, out InventoryRefNoTo, out  status);
        }

        public bool TransferIn_FIFO
           (string username, int FromInventoryLocationID, int ToInventoryLocationID, out string InventoryRefNoTo, out string status)
        {
            //Logger.writeLog("Start Transfer");
            InventoryDetCollection tmpDet;
            tmpDet = InvDet;

            InventoryRefNoTo = "";

            QueryCommandCollection cmd = new QueryCommandCollection();

            try
            {
                //Check if order is valid
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }

                InventoryDetCollection mergedInvDet = new InventoryDetCollection();


                InvHdr.IsNew = true;
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = ToInventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_TransferIn;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(ToInventoryLocationID);
                InventoryRefNoTo = InvHdr.InventoryHdrRefNo;
                InvHdr.StockOutReasonID = 1; //0 & 1 reserved..... 0= sales and 1=Transfer                
                QueryCommand mycmd = InvHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);

                for (int i = 0; i < InvDet.Count; i++)
                {
                    #region *) Conditioning: Set detail information
                    //decimal weight = (InvDet[i].FactoryPrice * InvDet[i].Quantity.GetValueOrDefault(0)) / SumOfFactoryPriceAndQty;

                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    InvDet[i].RemainingQty = InvDet[i].Quantity.GetValueOrDefault(0);

                    InvDet[i].Gst = GetGST();
                    #endregion

                    #region *) Save: Generate Save Script for InventoryDet
                    if (InvDet[i].IsNew)
                    {
                        mycmd = InvDet[i].GetInsertCommand(UserInfo.username);
                        cmd.Add(mycmd);
                    }
                    else
                    {
                        mycmd = InvDet[i].GetUpdateCommand(UserInfo.username);
                        cmd.Add(mycmd);
                    }
                    #endregion

                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(InvDet[i].ItemNo,
                        InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                        InvDet[i].FactoryPrice, Convert.ToDouble(InvDet[i].Quantity.GetValueOrDefault(0)),
                        InvDet[i].InventoryDetRefNo, InvHdr.InventoryDate);
                    cmd.AddRange(itemSummaryCmd);
                    
                }

                //Create Into Location Transfer Table
                LocationTransfer lt = new LocationTransfer();

                lt.FromInventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                lt.FromInventoryLocationID = FromInventoryLocationID;
                lt.ToInventoryLocationID = ToInventoryLocationID;
                lt.TransferFromBy = username;
                lt.IsReceived = true;
                cmd.Add(lt.GetInsertCommand(username));

                SubSonic.DataService.ExecuteTransaction(cmd);


                status = "";
                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;

                return false;
            }
        }
       
       
        /*
          public DataTable FetchUnSavedInventoryGroupByItemNo(out string status)
          {
              status = "";
              try
              {
                  //create and return a datatable.....
                  DataTable dTable = new DataTable();
                  DataRow dr;
                  DataRow[] tmpDrArray;

                  dTable.Columns.Add("ItemNo");
                  dTable.Columns.Add("ItemName");
                  dTable.Columns.Add("Quantity");
                  dTable.Columns.Add("FactoryPrice");
                  dTable.Columns.Add("Remark");

                  Item myItem;
                  //map OrderDet
                  if (RollUpItemQty == null)
                  {
                      RollUpItemQty = new Hashtable();
                  }

                  for (int i = 0; i < InvDet.Count; i++)
                  {

                      //already existed?
                      tmpDrArray = dTable.Select("ItemNo = '" + InvDet[i].ItemNo + "'");

                      if (tmpDrArray == null | tmpDrArray.Length < 1)
                      {

                          //No create a new one
                          dr = dTable.NewRow();
                          myItem = new Item(InvDet[i].ItemNo);

                          if (myItem.IsInInventory == true)
                          {
                              InvDet[i].FactoryPrice = myItem.FactoryPrice;
                              dr["ItemNo"] = InvDet[i].ItemNo;
                              dr["ItemName"] = myItem.ItemName;

                              if (RollUpItemQty.ContainsKey(InvDet[i].ItemNo))
                              {
                                  dr["Quantity"] = int.Parse(RollUpItemQty[InvDet[i].ItemNo].ToString());
                              }
                              else
                              {
                                  dr["Quantity"] = 0;
                              }
                              dr["FactoryPrice"] = InvDet[i].FactoryPrice;
                              dr["Remark"] = InvDet[i].Remark;
                              dTable.Rows.Add(dr);
                          }
                      }
                      else
                      {
                          //Skip this line as it has already existed
                      }
                  }
                  return dTable;
              }
              catch (Exception ex)
              {
                  Logger.writeLog(ex);
                  status = ex.Message;
                  return null;
              }
          }
          */ 

        #endregion

        #region "Transfer Discrepancy"
        /*
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ViewInventoryTransferDiscrepancyCollection FetchTransferDiscrepancy()
        {
            ViewInventoryTransferDiscrepancyCollection discr = new ViewInventoryTransferDiscrepancyCollection();
            return discr.Where(ViewInventoryTransferDiscrepancy.Columns.IsClosed, false).Load();
        }
        */
        /*
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateTransferDiscrepancyInformation(int DiscrepancyID, decimal CostOfGoods, string DiscrepancyReason, string DiscrepancyRemark)
        {
            if (DiscrepancyReason == null) DiscrepancyReason = "";
            if (DiscrepancyRemark == null) DiscrepancyRemark = "";
            Query invDet = InventoryTransferDiscrepancy.CreateQuery();
            invDet.QueryType = QueryType.Update;
            invDet.AddUpdateSetting(InventoryTransferDiscrepancy.Columns.DiscrepancyReason, DiscrepancyReason.ToString());
            invDet.AddUpdateSetting(InventoryTransferDiscrepancy.Columns.CostOfGoods, CostOfGoods);
            invDet.AddUpdateSetting(InventoryTransferDiscrepancy.Columns.Remark, DiscrepancyRemark.ToString());
            invDet.AddWhere("DiscrepancyID", DiscrepancyID);
            invDet.Execute();
        }
        */
        /*
        public bool CorrectTransferDiscrepancy(string username)
        {
            try
            {
                string status;

                ViewInventoryTransferDiscrepancyCollection discr = new ViewInventoryTransferDiscrepancyCollection();
                discr.Where(ViewInventoryTransferDiscrepancy.Columns.IsClosed, false).Load();

                for (int i = 0; i < discr.Count; i++)
                {
                    //for every row, perform stock in or stock out
                    if (discr[i].DiscrepancyQuantity < 0)
                    {
                        //do stock out
                        discr[i].DiscrepancyQuantity = -discr[i].DiscrepancyQuantity;
                        InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        ctrl.AddItemIntoInventory(discr[i].ItemNo, discr[i].DiscrepancyQuantity, discr[i].CostOfGoods.Value, out status);
                        ctrl.ChangeRemarkByItemNo(discr[i].ItemNo, "Adjustment for" + discr[i].InventoryHdrRefNo, out status);
                        ctrl.ChangeStockInRefNoByItemNo(discr[i].ItemNo, discr[i].InventoryHdrRefNo, out status);
                        ctrl.StockOut(username, 2, discr[i].ToInventoryLocationID.Value, true, false, out status);


                    }
                    else if (discr[i].DiscrepancyQuantity > 0)
                    {
                        //do stock in
                        InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        //Set COGS.....
                        ctrl.AddItemIntoInventory(discr[i].ItemNo, discr[i].DiscrepancyQuantity, discr[i].CostOfGoods.Value, out status);
                        ctrl.ChangeRemarkByItemNo(discr[i].ItemNo, "Adjustment for" + discr[i].InventoryHdrRefNo, out status);
                        ctrl.ChangeStockInRefNoByItemNo(discr[i].ItemNo, discr[i].InventoryHdrRefNo, out status);
                        ctrl.StockIn(username, discr[i].ToInventoryLocationID.Value, true, false, out status);
                    }

                    //Update IsClose as true 
                    Query qr = InventoryTransferDiscrepancy.CreateQuery();
                    qr.AddUpdateSetting("IsClosed", true);
                    qr.AddWhere("DiscrepancyID", discr[i].DiscrepancyID);
                    qr.Execute();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        */
        #endregion



    }
}
