using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.ComponentModel;
using System.Collections;
using PowerPOSLib.Reports;
using System.Drawing;

namespace PowerPOS
{
    [System.ComponentModel.DataObject]
    public partial class ReportController
    {
        public const string YEARLY = "y";
        public const string MONTHLY = "m";
        public const string DAILY = "d";
        public const string QUATERLY = "q";
        public const string HOURLY = "h";

        public static DataTable Pivot(IDataReader dataValues, string keyColumn, string pivotNameColumn, string pivotValueColumn)
        {

            DataTable tmp = new DataTable();

            DataRow r;

            string LastKey = "//dummy//";

            int i, pValIndex, pNameIndex;

            string s;

            bool FirstRow = true;



            // Add non-pivot columns to the data table:



            pValIndex = dataValues.GetOrdinal(pivotValueColumn);

            pNameIndex = dataValues.GetOrdinal(pivotNameColumn);



            for (i = 0; i <= dataValues.FieldCount - 1; i++)

                if (i != pValIndex && i != pNameIndex)

                    tmp.Columns.Add(dataValues.GetName(i), dataValues.GetFieldType(i));



            r = tmp.NewRow();



            // now, fill up the table with the data:

            while (dataValues.Read())
            {

                // see if we need to start a new row

                if (dataValues[keyColumn].ToString() != LastKey)
                {

                    // if this isn't the very first row, we need to add the last one to the table

                    if (!FirstRow)

                        tmp.Rows.Add(r);

                    r = tmp.NewRow();

                    FirstRow = false;

                    // Add all non-pivot column values to the new row:

                    for (i = 0; i <= dataValues.FieldCount - 3; i++)

                        r[i] = dataValues[tmp.Columns[i].ColumnName];

                    LastKey = dataValues[keyColumn].ToString();

                }

                // assign the pivot values to the proper column; add new columns if needed:

                s = dataValues[pNameIndex].ToString();

                if (!tmp.Columns.Contains(s))
                {
                    DataColumn c = tmp.Columns.Add(s, dataValues.GetFieldType(pValIndex));
                    // set the index so that it is sorted properly:
                    int newOrdinal = c.Ordinal;
                    for (i = newOrdinal - 1; i >= dataValues.FieldCount - 2; i--)
                        if (c.ColumnName.CompareTo(tmp.Columns[i].ColumnName) < 0)
                            newOrdinal = i;
                    c.SetOrdinal(newOrdinal);
                }

                r[s] = dataValues[pValIndex];

            }



            // add that final row to the datatable:

            tmp.Rows.Add(r);


            // Close the DataReader

            dataValues.Close();


            // and that's it!

            return tmp;

        }

        public DataTable FetchDynamicSalesReport
            (DateTime startDate, DateTime endDate, ArrayList ExtraField, string TimeGrouping, Hashtable whereLists)
        {

            string StartDateSQL, EndDateSQL, extraFieldSQL, timeGroupingSQL, whereListSQL;

            //convert human readable form to SQL readable form
            StartDateSQL = startDate.ToString("yyyyMMdd");
            EndDateSQL = endDate.ToString("yyyyMMdd");

            //Extra Fields
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ExtraField.Count; i++)
            {
                sb.Append(ExtraField[i] + ",");
            }
            extraFieldSQL = sb.ToString(0, sb.Length - 1);

            timeGroupingSQL = TimeGrouping;

            //Where Clause            
            sb = new StringBuilder();
            foreach (object key in whereLists.Keys)
            {
                //String? how about integer or date?
                sb.Append(key + " like '%" + whereLists[key] + "%' AND");
            }
            whereListSQL = sb.ToString(0, sb.Length - 3);

            /*Execute*/
            StoredProcedure sp =
                SPs.FetchDynamicSalesReport(StartDateSQL, EndDateSQL, extraFieldSQL, timeGroupingSQL, whereListSQL);
            sp.CommandTimeout = 3000;
            DataSet ds = sp.GetDataSet();
            if (ds == null || ds.Tables.Count <= 0)
            {
                return null;
            }
            DataTable dt = ds.Tables[0];


            //Reformat Table to human readable form
            DataTable result = new DataTable("SalesViewer");

            //Add columns

            //Add Grouping Columns
            for (int i = 0; i < ExtraField.Count; i++)
            {
                result.Columns.Add(ExtraField[i].ToString());
            }

            //Add date specifier - yyyy etc....
            switch (TimeGrouping)
            {
                case YEARLY:
                    int StartYear = startDate.Year;
                    int EndYear = endDate.Year;
                    for (int i = StartYear; i <= EndYear; i++)
                    {
                        result.Columns.Add(i.ToString()); // all the years
                    }
                    break;
                case QUATERLY:
                    //all the years and the quaters
                    StartYear = startDate.Year;
                    int indexQuarter = UtilityController.GetQuarterFromDate(startDate);
                    EndYear = endDate.Year;
                    int EndQuater = UtilityController.GetQuarterFromDate(endDate);

                    for (int i = StartYear; i <= EndYear; i++)
                    {
                        while (indexQuarter <= 4)
                        {
                            if (i == EndYear && indexQuarter > EndQuater)
                            {
                                break;
                            }
                            result.Columns.Add("Q" + indexQuarter.ToString() + " " + i.ToString()); // all the years
                            indexQuarter += 1;
                        }
                        indexQuarter = 1;
                    }
                    break;
                case MONTHLY:
                    //all years and the month
                    StartYear = startDate.Year;
                    int indexMonth = startDate.Month;
                    EndYear = endDate.Year;
                    int EndMonth = endDate.Month;

                    for (int i = StartYear; i <= EndYear; i++)
                    {
                        while (indexMonth <= 12)
                        {
                            if (i == EndYear && indexMonth > EndMonth)
                            {
                                break;
                            }
                            result.Columns.Add(indexMonth + "/" + i.ToString()); // all the years
                            indexMonth += 1;
                        }
                        indexMonth = 1;
                    }
                    break;
                case DAILY:
                    //all the dates
                    DateTime indexDate = startDate.Date;
                    while (indexDate <= endDate.Date)
                    {
                        result.Columns.Add(indexDate.ToString("dd/MM/yy"));
                        indexDate = indexDate.AddDays(1);
                    }
                    break;
                case HOURLY:
                    //all the hour
                    //all the dates
                    indexDate = startDate.Date;
                    while (indexDate <= endDate.Date)
                    {
                        result.Columns.Add(indexDate.ToString("dd/MM/yy HH:00"));
                        indexDate = indexDate.AddHours(1);
                    }
                    break;
            }

            int lastIndex = dt.Columns.Count - 3;
            int colIndex = 0; int k = 0, rowIndex = -1;
            bool IsTheSame = false;
            DataRow dr;
            //Populate rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //for every distinct value of 
                //compare with the column name
                //copy value with column name...
                if (i > 0)
                {
                    //for every expected list
                    //compare value with previous row.... If the last index is the same, then it will still be the same due to the sorting
                    if (dt.Rows[i][lastIndex].ToString() == dt.Rows[i - 1][lastIndex].ToString())
                    {
                        IsTheSame = true;
                    }
                    else
                    {
                        IsTheSame = false;
                    }
                }

                //if different or if i=0
                if (!IsTheSame || i == 0)
                {
                    //create new row
                    dr = result.NewRow();
                    //reset column index
                    colIndex = ExtraField.Count;
                    k++;
                    result.Rows.Add(dr);
                    rowIndex += 1;
                    for (int m = 0; m < ExtraField.Count; m++)
                    {
                        result.Rows[rowIndex][ExtraField[m].ToString()] =
                            dt.Rows[i][ExtraField[m].ToString()].ToString();
                    }
                }
                //check column header until we found a match
                //move column index
                //compare columns until found/end
                //result.Rows[rowIndex][ExtraField.Count + 1] = k; 
                //Add date specifier - yyyy etc....
                switch (TimeGrouping)
                {
                    case YEARLY:
                        //check if column exist....
                        result.Rows[i][dt.Rows[i]["ReportYear"].ToString()] = "$" + dt.Rows[i]["TotalAmount"].ToString();
                        /*
                        while (colIndex < result.Columns.Count)
                        {
                            if (dt.Rows[i]["ReportYear"].ToString() == result.Columns[colIndex].ColumnName)
                            {
                                result.Rows[i][colIndex] 
                            }
                            else
                            {
                                colIndex += 1;
                            }
                        }*/
                        break;
                    case QUATERLY:
                        break;
                    case MONTHLY:
                        break;
                    case DAILY:
                        break;
                    case HOURLY:
                        break;
                }

            }

            return result;
        }

        public static DataTable FetchQuotationReport
          (DateTime StartDate, DateTime EndDate, string RefNo,
          string Cashier, string paymenttype, string outletName, string remark, string isVoided, string nametoappear)
        {
            string SQL = "select X.* from " +
                        "(select   a.Userfld5 as orderrefno,a.nettamount as amount,orderdate,a.cashierid, " +
                        "a.orderhdrid,a.isvoided," +
                        "isnull(a.remark,'') as remark, " +
                        "isnull(a.membershipno, '') as membershipno,isnull(d.nametoappear,'') as nametoappear,a.pointofsaleid,e.pointofsalename,e.outletname " +
                        "from quotationhdr a " +
                        "left outer join membership d " +
                        "on d.membershipno=a.membershipno " +
                        "inner join Pointofsale e " +
                        "on a.pointofsaleid = e.pointofsaleid " +
                        "group by a.Userfld5,a.nettamount,orderdate,a.cashierid,a.orderhdrid,a.isvoided, " +
                        "a.remark,isnull(a.membershipno,''),isnull(d.nametoappear,''),a.pointofsaleid,e.pointofsalename,e.outletname) X  " +
                        "where X.orderdate >= ' " + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND X.orderdate <= ' " + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  " +
                        " AND ISNULL(X.orderrefno,'') like '%" + RefNo + "%'" +
                        " AND X.CashierID like '%" + Cashier + "%'  " +
                        " AND ISNULL(X.PointOfSaleName,'') like '%%' " +
                        " AND ISNULL(X.OutletName,'') like '%" + outletName + "%' " +
                        " AND ISNULL(X.Remark,'') like '%" + remark + "%' " +
                        " AND X.isVoided like '%" + isVoided + "%' " +
                        " AND ISNULL(X.Nametoappear,'') like N'%" + nametoappear + "%' " +
                        "order by x.orderdate desc; ";


            QueryCommand qr = new QueryCommand(SQL, "PowerPOS");

            DataTable dt = DataService.GetDataSet(qr).Tables[0];

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if ((int)dt.Rows[i]["paymentcount"] > 1)
            //    {
            //        dt.Rows[i]["paymentType"] = dt.Rows[i]["paymentType"] + " (COMBINED)";
            //    }
            //}
            return dt;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DataTable FetchProductSalesReport(
                DateTime startdate,
                DateTime enddate,
                string searchQry,
                string PointOfSaleName,
                string OutletName,
                string CategoryName,
                string DeptID,
                int supplierID,
                bool IsVoided,
                string SortColumn,
                string SortDir
            )
        {

            if (CategoryName == "") { CategoryName = "ALL"; }
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortDir == "") { SortDir = "ASC"; }
            //if (SortColumn == "") { SortColumn = "CategoryName"; }
            if (DeptID == "" || DeptID == "0") { DeptID = "ALL"; }

            string sortExp = "";
            if (SortColumn == "")
                sortExp = "OutletName, TotalAmount DESC";
            else
                sortExp = string.Format("{0} {1}", SortColumn, SortDir);

            DataTable dt = new DataTable();
            try
            {
                string sql = @"DECLARE @CategoryName NVARCHAR(500); 
                            DECLARE @StartDate DATETIME; 
                            DECLARE @EndDate DATETIME; 
                            DECLARE @PointOfSaleName NVARCHAR(500); 
                            DECLARE @OutletName NVARCHAR(500); 
                            DECLARE @DeptID NVARCHAR(500); 
                            DECLARE @IsVoided BIT; 
                            DECLARE @Search NVARCHAR(MAX);
                            DECLARE @SupplierID INT;

                            SET @StartDate = '{0}'; 
                            SET @EndDate = '{1}'; 
                            SET @PointOfSaleName = N'{2}'; 
                            SET @OutletName = N'{3}'; 
                            SET @DeptID = N'{4}'; 
                            SET @CategoryName = N'{5}';
                            SET @Search = N'{6}';
                            SET @IsVoided = 0; 
                            SET @OutletName = LTRIM(RTRIM(@OutletName)); 
                            SET @SupplierID ={7};

                            SELECT   ID.DepartmentName
                                    ,C.CategoryName
		                            ,I.ItemNo
		                            ,I.ItemName
                                    ,I.Barcode 
		                            ,CAST(SUM(OD.Quantity) as Decimal(18,2)) TotalQuantity
		                            ,SUM(OD.Amount) TotalAmount
		                            ,SUM(OD.GSTAmount) GSTAmount
		                            ,(SUM(OD.Amount)-SUM(OD.GSTAmount)) TotalAmountWithoutGST
	                                ,SUM(OD.CostOfGoodSold) TotalCostOfGoodsSold	
	                                ,SUM(OD.Amount)-SUM(OD.CostOfGoodSold) ProfitLoss	
                                    ,CASE WHEN SUM(OD.Amount) = 0 THEN 0
			                              ELSE (SUM(OD.Amount)-SUM(OD.CostOfGoodSold)) / SUM(OD.Amount) END ProfitLossPercentage	    		
		                            ,I.Attributes1
		                            ,I.Attributes2
		                            ,I.Attributes3
		                            ,I.Attributes4
		                            ,I.Attributes5
		                            ,I.Attributes6
		                            ,I.Attributes7
                                    ,CASE WHEN @PointOfSaleName = 'ALL' THEN 'ALL' ELSE POS.PointOfSaleName END PointOfSaleName
                                    ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE POS.OutletName END OutletName  
                                    ,SUPP.SupplierName      
                            FROM	OrderHdr OH
                                    LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                                    LEFT JOIN Item I ON I.ItemNo = OD.ItemNo
                                    LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                    LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
		                            LEFT JOIN (
			                            SELECT  ISM.ItemNo
					                            ,ISM.SupplierID
                                                ,SP.SupplierName
					                            ,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
			                            FROM	ItemSupplierMap ISM
                                        LEFT JOIN Supplier SP on ISM.SupplierID = SP.SupplierID
			                            WHERE	ISNULL(ISM.Deleted,0) = 0
		                            ) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
                            WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
                                    AND OH.OrderDate BETWEEN @StartDate AND @EndDate
                                    AND (POS.PointOfSaleName = @PointOfSaleName OR @PointOfSaleName = 'ALL' OR @PointOfSaleName = 'ALL - BreakDown')
                                    AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                                    AND (ID.ItemDepartmentID = @DeptID OR @DeptID = 'ALL')
                                    AND (C.CategoryName = @CategoryName OR @CategoryName = 'ALL') 
                            		AND (@SupplierID = 0 OR ISNULL(SUPP.SupplierID,0) = @SupplierID)
                                    AND (I.ItemNo LIKE '%'+@Search+'%'
		                            OR I.ItemName LIKE '%'+@Search+'%'		
		                            OR ISNULL(I.Attributes1,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes2,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes3,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes4,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes5,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes6,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes7,'') LIKE '%'+@Search+'%')
                            GROUP BY  ID.DepartmentName
                                    ,C.CategoryName
		                            ,I.ItemNo
		                            ,I.ItemName	  
                                    ,I.Barcode  		
		                            ,I.Attributes1
		                            ,I.Attributes2
		                            ,I.Attributes3
		                            ,I.Attributes4
		                            ,I.Attributes5
		                            ,I.Attributes6
		                            ,I.Attributes7
                                    ,CASE WHEN @PointOfSaleName = 'ALL' THEN 'ALL' ELSE POS.PointOfSaleName END 
                                    ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE POS.OutletName END 
                                    ,SUPP.SupplierName    
                            ORDER BY {8}";

                sql = string.Format(sql, startdate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , enddate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , PointOfSaleName
                                       , OutletName
                                       , DeptID
                                       , CategoryName
                                       , searchQry
                                       , supplierID
                                       , sortExp);

                QueryCommand qc = new QueryCommand(sql);
                qc.CommandTimeout = 1000;
                dt.Load(DataService.GetReader(qc));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DataTable FetchProductSalesReport(
                DateTime startdate,
                DateTime enddate,
                string searchQry,
                string PointOfSaleName,
                string OutletName,
                string CategoryName,
                string DeptID,
                bool IsVoided,
                string SortColumn,
                string SortDir
            )
        {

            if (CategoryName == "") { CategoryName = "ALL"; }
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortDir == "") { SortDir = "ASC"; }
            //if (SortColumn == "") { SortColumn = "CategoryName"; }
            if (DeptID == "" || DeptID == "0") { DeptID = "ALL"; }

            string sortExp = "";
            if (SortColumn == "")
                sortExp = "OutletName, TotalAmount DESC";
            else
                sortExp = string.Format("{0} {1}", SortColumn, SortDir);

            DataTable dt = new DataTable();
            try
            {
                string sql = @"DECLARE @CategoryName NVARCHAR(500); 
                            DECLARE @StartDate DATETIME; 
                            DECLARE @EndDate DATETIME; 
                            DECLARE @PointOfSaleName NVARCHAR(500); 
                            DECLARE @OutletName NVARCHAR(500); 
                            DECLARE @DeptID NVARCHAR(500); 
                            DECLARE @IsVoided BIT; 
                            DECLARE @Search NVARCHAR(MAX);

                            SET @StartDate = '{0}'; 
                            SET @EndDate = '{1}'; 
                            SET @PointOfSaleName = N'{2}'; 
                            SET @OutletName = N'{3}'; 
                            SET @DeptID = N'{4}'; 
                            SET @CategoryName = N'{5}';
                            SET @Search = N'{6}';
                            SET @IsVoided = 0; 
                            SET @OutletName = LTRIM(RTRIM(@OutletName)); 

                            SELECT   ID.DepartmentName
                                    ,C.CategoryName
		                            ,I.ItemNo
		                            ,I.ItemName
                                    ,I.Barcode 
		                            ,CAST(SUM(OD.Quantity) as Decimal(18,2)) TotalQuantity
		                            ,SUM(OD.Amount) TotalAmount
		                            ,SUM(OD.GSTAmount) GSTAmount
		                            ,(SUM(OD.Amount)-SUM(OD.GSTAmount)) TotalAmountWithoutGST
	                                ,SUM(OD.CostOfGoodSold) TotalCostOfGoodsSold	
	                                ,SUM(OD.Amount)-SUM(OD.CostOfGoodSold) ProfitLoss	
                                    ,CASE WHEN SUM(OD.Amount) = 0 THEN 0
			                              ELSE (SUM(OD.Amount)-SUM(OD.CostOfGoodSold)) / SUM(OD.Amount) END ProfitLossPercentage	    		
		                            ,I.Attributes1
		                            ,I.Attributes2
		                            ,I.Attributes3
		                            ,I.Attributes4
		                            ,I.Attributes5
		                            ,I.Attributes6
		                            ,I.Attributes7
                                    ,CASE WHEN @PointOfSaleName = 'ALL' THEN 'ALL' ELSE POS.PointOfSaleName END PointOfSaleName
                                    ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE POS.OutletName END OutletName        
                            FROM	OrderHdr OH
                                    LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                                    LEFT JOIN Item I ON I.ItemNo = OD.ItemNo
                                    LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                    LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                            WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
                                    AND OH.OrderDate BETWEEN @StartDate AND @EndDate
                                    AND (POS.PointOfSaleName = @PointOfSaleName OR @PointOfSaleName = 'ALL' OR @PointOfSaleName = 'ALL - BreakDown')
                                    AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                                    AND (ID.ItemDepartmentID = @DeptID OR @DeptID = 'ALL')
                                    AND (C.CategoryName = @CategoryName OR @CategoryName = 'ALL') 
                                    AND (I.ItemNo LIKE '%'+@Search+'%'
		                            OR I.ItemName LIKE '%'+@Search+'%'		
		                            OR ISNULL(I.Attributes1,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes2,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes3,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes4,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes5,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes6,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes7,'') LIKE '%'+@Search+'%')
                            GROUP BY  ID.DepartmentName
                                    ,C.CategoryName
		                            ,I.ItemNo
		                            ,I.ItemName	  
                                    ,I.Barcode  		
		                            ,I.Attributes1
		                            ,I.Attributes2
		                            ,I.Attributes3
		                            ,I.Attributes4
		                            ,I.Attributes5
		                            ,I.Attributes6
		                            ,I.Attributes7
                                    ,CASE WHEN @PointOfSaleName = 'ALL' THEN 'ALL' ELSE POS.PointOfSaleName END 
                                    ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE POS.OutletName END    
                            ORDER BY {7}";

                sql = string.Format(sql, startdate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , enddate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , PointOfSaleName
                                       , OutletName
                                       , DeptID
                                       , CategoryName
                                       , searchQry
                                       , sortExp);

                QueryCommand qc = new QueryCommand(sql);
                qc.CommandTimeout = 1000;
                dt.Load(DataService.GetReader(qc));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;

            #region *) Legacy Code

            //string SQL = 
            //    "DECLARE @StartDate DATETIME; " +
            //    "DECLARE @EndDate DATETIME; " +
            //    "DECLARE @ItemName VARCHAR(50); " +
            //    "DECLARE @PointOfSaleName VARCHAR(50); " +
            //    "DECLARE @OutletName VARCHAR(50); " +
            //    "DECLARE @CategoryName VARCHAR(50); " +
            //    "DECLARE @DeptID VARCHAR(50); " +
            //    "DECLARE @IsVoided BIT; " +
            //    "DECLARE @sortby VARCHAR(50); " +
            //    "DECLARE @sortdir VARCHAR(5); " +
            //    " " +
            //    "SET @StartDate = '" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
            //    "SET @EndDate = '" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
            //    "SET @ItemName = '%" + searchQry + "%'; " +
            //    "SET @PointOfSaleName = '" + PointOfSaleName + "'; " +
            //    "SET @OutletName = '" + OutletName + "'; " +
            //    "SET @CategoryName = '%" + CategoryName + "%'; " +
            //    "SET @DeptID = '" + DeptID + "'; " +
            //    "SET @IsVoided = 0; " +
            //    "SET @sortby = '" + SortColumn + "'; " +
            //    "SET @sortdir = '" + SortDir + "'; " +
            //    " " +
            //    "SET NOCOUNT ON; " +
            //    "set @sortby = LTRIM(RTRIM(@sortby)); " +
            //    "set @sortdir = LTRIM(RTRIM(@sortdir)); " +
            //    " "+
            //    "IF ((SELECT MAX(AppSettingValue) FROM AppSetting WHERE AppSettingKey = 'Inventory_CostingMethod') = 'fixed avg') " +
            //    "BEGIN " +
            //        "UPDATE InventoryDet " +
            //        "SET CostOfGoods = COG.COG " +
            //        "FROM InventoryHdr IH " +
            //            "INNER JOIN " +
            //            "( " +
            //                "SELECT InventoryLocationID, ItemNo, case when sum(quantity) = 0 then 0 else SUM(Quantity * CostOfGoods) / SUM(Quantity) end AS COG " +
            //                "FROM InventoryHdr IH " +
            //                    "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
            //                "WHERE IH.MovementType LIKE '% In' " +
            //                "GROUP BY InventoryLocationID, ItemNo " +
            //            ") COG ON IH.InventoryLocationID = COG.InventoryLocationID " +
            //        "WHERE IH.MovementType LIKE '% Out' " +
            //            "AND IH.InventoryHdrRefNo = InventoryDet.InventoryHdrRefNo " +
            //            "AND InventoryDet.ItemNo = COG.ItemNo " +
            //    "END " +
            //    " "+
            //    "if (@OutletName = 'ALL' AND @PointOfSaleName = 'ALL') " +
            //    "Begin " +
            //    "SELECT ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, SUM(OrderDet.Quantity) AS TotalQuantity  " +
            //        ", ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, ISNULL(Attributes3,'') Attributes3 " +
            //        ", ISNULL(Attributes4,'') Attributes4, ISNULL(Attributes5,'') Attributes5, ISNULL(Attributes6,'') Attributes6 " +
            //        ", ISNULL(Attributes7,'') Attributes7 " +
            //        ",Sum(OrderDet.Amount) AS TotalAmount, " +
            //        "Sum(OrderDet.GSTAmount) AS GSTAmount,Sum(OrderDet.Discount) AS Discount,  " +
            //        "(Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) AS TotalAmountWithoutGST,  " +
            //        "OrderDet.IsVoided, 'ALL' AS PointOfSaleName, 'ALL' AS OutletName, " +
            //        "Item.CategoryName, Item.ProductLine, isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            //        "isnull((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            //        "isnull((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount))+0.001)*100,0) as ProfitLossPercentage " +
            //    "FROM ItemDepartment Inner Join Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID inner join Item on Category.CategoryName = Item.CategoryName INNER JOIN " +
            //    "  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            //    "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            //    "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
            //    "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
            //    //"  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            //    "  LEFT outer jOIN " +
            //    "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            //    "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            //    "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
            //        "AND Item.ItemName + '%' + Item.CategoryName + '%' + Item.Barcode + '%'+ ItemDepartment.DepartmentName + '%' + isnull(Item.Attributes1,'') + '%' + isnull(Item.Attributes2,'') + '%' + isnull(Item.Attributes3,'') + '%' + isnull(Item.Attributes4,'') + '%' + isnull(Item.Attributes5,'') Like @itemname " +
            //        "AND Item.CategoryName Like @CategoryName " +
            //        "AND OrderDet.IsVoided = @IsVoided " +
            //        "AND OrderHdr.IsVoided = 0 " +
            //        "AND orderdet.itemno <> 'inst_payment' "+
            //        "AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            //    "GROUP BY ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, OrderDet.IsVoided, Item.CategoryName, Item.ProductLine " +
            //        ", ISNULL(Attributes1,''), ISNULL(Attributes2,''), ISNULL(Attributes3,'') " +
            //        ", ISNULL(Attributes4,''), ISNULL(Attributes5,''), ISNULL(Attributes6,'') " +
            //        ", ISNULL(Attributes7,'') " +
            //    "ORDER BY " +
            //    "CASE    WHEN @sortby = 'ItemNo' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.ItemNo desc) " +
            //    "WHEN @sortby = 'ItemNo' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.ItemNo asc) " +
            //    "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +            
            //    "WHEN @sortby = 'ItemName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.ItemName desc) " +
            //    "WHEN @sortby = 'ItemName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.ItemName asc) " +
            //    "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by OrderDet.IsVoided desc) " +
            //    "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by OrderDet.IsVoided asc) " +
            //    "WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.CategoryName desc) " +
            //    "WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.CategoryName asc) " +
            //    "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            //    "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            //    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            //    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            //    "    WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)) desc) " +
            //    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)) asc) " +
            //    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100 desc) " +
            //    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100 asc) " +
            //    "ELSE rank() over (order by Item.ProductLine,Item.CategoryName,Item.ItemName asc) " +
            //    "END  " +
            //    "End " +


            //    "Else if (@PointOfSaleName = 'ALL' and @OutletName != 'ALL')  " +
            //    "Begin " +
            //    "SELECT ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, SUM(OrderDet.Quantity) AS TotalQuantity,  " +
            //    "Sum(OrderDet.Amount) AS TotalAmount, " +
            //    "Sum(OrderDet.GSTAmount) AS GSTAmount,  " +
            //    "   (Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) AS TotalAmountWithoutGST,  " +
            //    "   OrderDet.IsVoided, 'ALL' AS PointOfSaleName, Outlet.OutletName, " +
            //    "   Item.CategoryName, Item.ProductLine " +
            //        ", ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, ISNULL(Attributes3,'') Attributes3 " +
            //        ", ISNULL(Attributes4,'') Attributes4, ISNULL(Attributes5,'') Attributes5, ISNULL(Attributes6,'') Attributes6 " +
            //        ", ISNULL(Attributes7,'') Attributes7 " +
            //    ", isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold,Sum(OrderDet.Discount) AS Discount, " +
            //    "isnull((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            //    "isnull((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount))+0.001)*100,0) as ProfitLossPercentage " +
            //    "FROM ItemDepartment Inner Join Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID inner join Item on Category.CategoryName = Item.CategoryName INNER JOIN " +
            //    "  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            //    "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            //    "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
            //    "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
            //    //" INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            //    "LEFT outer jOIN " +
            //    "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            //    "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            //    "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
            //    "   AND Item.ItemName + '%' + Item.CategoryName + '%' + Item.Barcode + '%' + ItemDepartment.DepartmentName + '%'+ isnull(Item.Attributes1,'') + '%' + isnull(Item.Attributes2,'') + '%' + isnull(Item.Attributes3,'') + '%' + isnull(Item.Attributes4,'') + '%'+ isnull(Item.Attributes5,'') Like @itemname " +
            //    "   AND Outlet.OutletName Like @OutletName " +
            //    "   AND Item.CategoryName Like @CategoryName " +
            //    "   AND OrderDet.IsVoided = @IsVoided " +
            //    "   AND OrderHdr.IsVoided = 0 " +
            //        "AND orderdet.itemno <> 'inst_payment' "+
            //    "   AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            //    "GROUP BY ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, OrderDet.IsVoided, Item.CategoryName, Outlet.OutletName, Item.ProductLine " +
            //        ", ISNULL(Attributes1,''), ISNULL(Attributes2,''), ISNULL(Attributes3,'') " +
            //        ", ISNULL(Attributes4,''), ISNULL(Attributes5,''), ISNULL(Attributes6,'') " +
            //        ", ISNULL(Attributes7,'') " +
            //    "ORDER BY " +
            //    "CASE    WHEN @sortby = 'ItemNo' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.ItemNo desc) " +
            //    "WHEN @sortby = 'ItemNo' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.ItemNo asc) " +
            //    "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +            
            //    "WHEN @sortby = 'ItemName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.ItemName desc) " +
            //    "WHEN @sortby = 'ItemName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.ItemName asc) " +
            //    "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by OrderDet.IsVoided desc) " +
            //    "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by OrderDet.IsVoided asc) " +
            //    "WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.CategoryName desc) " +
            //    "WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.CategoryName asc) " +
            //    "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            //    "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            //    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            //    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            //    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)) desc) " +
            //    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)) asc) " +
            //    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100 desc) " +
            //    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100 asc) " +
            //    "ELSE rank() over (order by Item.ProductLine,Item.CategoryName,Item.ItemName asc) " +
            //    "END  " +
            //    "End " +


            //    "Else if (@PointOfSaleName != 'ALL' and @OutletName = 'ALL')   " +
            //    "begin " +
            //    "set @OutletName = '%'; " +
            //    "SELECT ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, SUM(OrderDet.Quantity) AS TotalQuantity " +
            //        ", ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, ISNULL(Attributes3,'') Attributes3 " +
            //        ", ISNULL(Attributes4,'') Attributes4, ISNULL(Attributes5,'') Attributes5, ISNULL(Attributes6,'') Attributes6 " +
            //        ", ISNULL(Attributes7,'') Attributes7 " +
            //    ", Sum(OrderDet.Amount) AS TotalAmount, " +
            //    "Sum(OrderDet.GSTAmount) AS GSTAmount,  " +
            //    "   (Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) AS TotalAmountWithoutGST,  " +
            //    "   OrderDet.IsVoided, PointOfSale.PointOfSaleID, PointOfSale.PointOfSaleName, Outlet.OutletName, " +
            //    "   Item.CategoryName, Item.ProductLine, " +
            //    "isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold,Sum(OrderDet.Discount) AS Discount, " +
            //    "isnull(((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            //    "isnull(((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount))+0.001)*100,0) as ProfitLossPercentage " +
            //    "FROM ItemDepartment Inner Join Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID inner join Item on Category.CategoryName = Item.CategoryName INNER JOIN " +
            //    "  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            //    "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            //    "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
            //    "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
            //    //"  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            //    "  LEFT outer jOIN " +
            //    "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            //    "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            //    "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
            //    "   AND Item.ItemName + '%' + Item.CategoryName + '%' + Item.Barcode + '%' + ItemDepartment.DepartmentName + '%' + isnull(Item.Attributes1,'') + '%' + isnull(Item.Attributes2,'') + '%' + isnull(Item.Attributes3,'') + '%' + isnull(Item.Attributes4,'') + '%' + isnull(Item.Attributes5,'') Like @itemname AND PointOfSale.PointOfSaleName Like @PointOfSaleName " +
            //    "   AND Outlet.OutletName Like @OutletName " +
            //    "   AND Item.CategoryName Like @CategoryName " +
            //    "   AND OrderDet.IsVoided = @IsVoided " +
            //    "   AND OrderHdr.IsVoided = 0 " +
            //        "AND orderdet.itemno <> 'inst_payment' "+
            //    "   AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            //    "GROUP BY ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, PointOfSale.PointOfSaleID,  " +
            //    " PointOfSale.PointOfSaleName, OrderDet.IsVoided, Item.CategoryName, Outlet.OutletName, item.ProductLine " +
            //        ", ISNULL(Attributes1,''), ISNULL(Attributes2,''), ISNULL(Attributes3,'') " +
            //        ", ISNULL(Attributes4,''), ISNULL(Attributes5,''), ISNULL(Attributes6,'') " +
            //        ", ISNULL(Attributes7,'') " +

            //    "ORDER BY " +
            //    "CASE    WHEN @sortby = 'ItemNo' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.ItemNo desc) " +
            //    "WHEN @sortby = 'ItemNo' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.ItemNo asc) " +
            //    "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +            
            //    "WHEN @sortby = 'ItemName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.ItemName desc) " +
            //    "WHEN @sortby = 'ItemName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.ItemName asc) " +
            //    "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by PointOfSale.PointOfSaleName desc) " +
            //    "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by PointOfSale.PointOfSaleName asc) " +
            //    "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by OrderDet.IsVoided desc) " +
            //    "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by OrderDet.IsVoided asc) " +
            //    "WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Outlet.OutletName desc) " +
            //    "WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Outlet.OutletName asc) " +
            //    "WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.CategoryName desc) " +
            //    "WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.CategoryName asc) " +
            //    "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            //    "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            //    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            //    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            //    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)) desc) " +
            //    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)) asc) " +
            //    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100 desc) " +
            //    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100 asc) " +
            //    "ELSE rank() over (order by  Item.ProductLine,Item.CategoryName,Item.ItemName asc) " +
            //    "END  " +
            //    "end " +


            //    "Else " +
            //    "Begin " +
            //    "SELECT ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, SUM(OrderDet.Quantity) AS TotalQuantity,  " +
            //    "   Sum(OrderDet.Amount) AS TotalAmount, " +
            //    "Sum(OrderDet.GSTAmount) AS GSTAmount,  " +
            //    "   (Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) AS TotalAmountWithoutGST,  " +
            //    "   OrderDet.IsVoided, PointOfSale.PointOfSaleID, PointOfSale.PointOfSaleName, Outlet.OutletName, Sum(OrderDet.Discount) AS Discount," +
            //    "   Item.CategoryName, Item.ProductLine " +
            //        ", ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, ISNULL(Attributes3,'') Attributes3 " +
            //        ", ISNULL(Attributes4,'') Attributes4, ISNULL(Attributes5,'') Attributes5, ISNULL(Attributes6,'') Attributes6 " +
            //        ", ISNULL(Attributes7,'') Attributes7 " +
            //    ", isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            //    "isnull(((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            //    "isnull(((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount))+0.001)*100,0) as ProfitLossPercentage " +
            //    "FROM ItemDepartment Inner Join Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID inner join Item on Category.CategoryName = Item.CategoryName INNER JOIN " +
            //    "  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            //    "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            //    "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
            //    "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
            //    //"  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            //    "LEFT outer jOIN " +
            //    "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            //    "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            //    "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
            //    "   AND Item.ItemName + '%' + Item.CategoryName + '%' + Item.Barcode + '%'+ ItemDepartment.DepartmentName + '%' + isnull(Item.Attributes1,'') + '%' + isnull(Item.Attributes2,'') + '%' + isnull(Item.Attributes3,'') + '%' + isnull(Item.Attributes4,'') + '%' + isnull(Item.Attributes5,'') Like @itemname AND PointOfSale.PointOfSaleName Like @PointOfSaleName " +
            //    "   AND Outlet.OutletName Like @OutletName " +
            //    "   AND Item.CategoryName Like @CategoryName " +
            //    "   AND OrderDet.IsVoided = @IsVoided " +
            //    "   AND OrderHdr.IsVoided = 0 " +
            //        "AND orderdet.itemno <> 'inst_payment' "+
            //    "   AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            //    "GROUP BY ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, PointOfSale.PointOfSaleID,  " +
            //    " PointOfSale.PointOfSaleName, OrderDet.IsVoided, Item.CategoryName, Outlet.OutletName, Item.ProductLine " +
            //        ", ISNULL(Attributes1,''), ISNULL(Attributes2,''), ISNULL(Attributes3,'') " +
            //        ", ISNULL(Attributes4,''), ISNULL(Attributes5,''), ISNULL(Attributes6,'') " +
            //        ", ISNULL(Attributes7,'') " +
            //    "ORDER BY " +
            //    "CASE    WHEN @sortby = 'ItemNo' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.ItemNo desc) " +
            //    "WHEN @sortby = 'ItemNo' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.ItemNo asc) " +
            //    "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +            
            //    "WHEN @sortby = 'ItemName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.ItemName desc) " +
            //    "WHEN @sortby = 'ItemName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.ItemName asc) " +
            //    "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by PointOfSale.PointOfSaleName desc) " +
            //    "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by PointOfSale.PointOfSaleName asc) " +
            //    "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by OrderDet.IsVoided desc) " +
            //    "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by OrderDet.IsVoided asc) " +
            //    "WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by Item.CategoryName desc) " +
            //    "WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by Item.CategoryName asc) " +
            //    "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            //    "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            //    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            //    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            //    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)) desc) " +
            //    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)) asc) " +
            //    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100 desc) " +
            //    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            //    "THEN rank() over (order by (Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100 asc) " +
            //    "ELSE rank() over (order by  Item.ProductLine,Item.CategoryName,Item.ItemName asc) " +
            //    "END  " +
            //    "End ";

            //QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            //cmd.CommandTimeout = 600;
            //DataSet ds = DataService.GetDataSet(cmd);
            //if (ds != null && ds.Tables.Count > 0)
            //    return ds.Tables[0];
            //else
            //    return null;   

            #endregion
        }

        public static DataTable FetchProductSalesReport(
                DateTime startdate,
                DateTime enddate,
                string searchQry,
                string PointOfSaleName,
                string OutletName,
                string[] CategoryName,
                string DeptID,
                bool IsVoided,
                string SortColumn,
                string SortDir
            )
        {
            string categoryFilter = "";

            if (CategoryName.Length > 0)
            {
                for (var i = 0; i < CategoryName.Length; i++)
                {
                    if (CategoryName[i].Trim() == "")
                        CategoryName[i] = null;
                    else
                        CategoryName[i] = "N'" + CategoryName[i].Trim().Replace("'", "''") + "'";
                }
                categoryFilter = string.Join(",", CategoryName).Trim(',');
            }

            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortDir == "") { SortDir = "ASC"; }
            //if (SortColumn == "") { SortColumn = "CategoryName"; }
            if (DeptID == "" || DeptID == "0") { DeptID = "ALL"; }

            string sortExp = "";
            if (SortColumn == "")
                sortExp = "OutletName, TotalAmount DESC";
            else
                sortExp = string.Format("{0} {1}", SortColumn, SortDir);

            DataTable dt = new DataTable();
            try
            {
                string sql = @"
                            DECLARE @StartDate DATETIME; 
                            DECLARE @EndDate DATETIME; 
                            DECLARE @PointOfSaleName NVARCHAR(500); 
                            DECLARE @OutletName NVARCHAR(500); 
                            DECLARE @DeptID NVARCHAR(500); 
                            DECLARE @IsVoided BIT; 
                            DECLARE @Search NVARCHAR(MAX);
                            
                            SET @StartDate = '{0}'; 
                            SET @EndDate = '{1}'; 
                            SET @PointOfSaleName = N'{2}'; 
                            SET @OutletName = N'{3}'; 
                            SET @DeptID = N'{4}'; 
                            SET @Search = N'{6}';
                            SET @IsVoided = 0; 
                            SET @OutletName = LTRIM(RTRIM(@OutletName)); 

                            SELECT   ID.DepartmentName
                                    ,C.CategoryName
		                            ,I.ItemNo
		                            ,I.ItemName
                                    ,I.Barcode 
		                            ,CAST(SUM(OD.Quantity) as Decimal(18,2)) TotalQuantity
		                            ,SUM(OD.Amount) TotalAmount
		                            ,SUM(OD.GSTAmount) GSTAmount
		                            ,(SUM(OD.Amount)-SUM(OD.GSTAmount)) TotalAmountWithoutGST
	                                ,SUM(OD.CostOfGoodSold) TotalCostOfGoodsSold	
	                                ,SUM(OD.Amount)-SUM(OD.CostOfGoodSold) ProfitLoss	
                                    ,CASE WHEN SUM(OD.Amount) = 0 THEN 0
			                              ELSE (SUM(OD.Amount)-SUM(OD.CostOfGoodSold)) / SUM(OD.Amount) END ProfitLossPercentage	    		
		                            ,I.Attributes1
		                            ,I.Attributes2
		                            ,I.Attributes3
		                            ,I.Attributes4
		                            ,I.Attributes5
		                            ,I.Attributes6
		                            ,I.Attributes7
                                    ,CASE WHEN @PointOfSaleName = 'ALL' THEN 'ALL' ELSE POS.PointOfSaleName END PointOfSaleName
                                    ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE POS.OutletName END OutletName        
                            FROM	OrderHdr OH
                                    LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                                    LEFT JOIN Item I ON I.ItemNo = OD.ItemNo
                                    LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                    LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                            WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
                                    AND OH.OrderDate BETWEEN @StartDate AND @EndDate
                                    AND (POS.PointOfSaleName = @PointOfSaleName OR @PointOfSaleName = 'ALL' OR @PointOfSaleName = 'ALL - BreakDown')
                                    AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                                    AND (ID.ItemDepartmentID = @DeptID OR @DeptID = 'ALL')
                                    {8} 
                                    AND (I.ItemNo LIKE '%'+@Search+'%'
		                            OR I.ItemName LIKE '%'+@Search+'%'		
		                            OR ISNULL(I.Attributes1,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes2,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes3,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes4,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes5,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes6,'') LIKE '%'+@Search+'%'
		                            OR ISNULL(I.Attributes7,'') LIKE '%'+@Search+'%')
                            GROUP BY  ID.DepartmentName
                                    ,C.CategoryName
		                            ,I.ItemNo
		                            ,I.ItemName	  
                                    ,I.Barcode  		
		                            ,I.Attributes1
		                            ,I.Attributes2
		                            ,I.Attributes3
		                            ,I.Attributes4
		                            ,I.Attributes5
		                            ,I.Attributes6
		                            ,I.Attributes7
                                    ,CASE WHEN @PointOfSaleName = 'ALL' THEN 'ALL' ELSE POS.PointOfSaleName END 
                                    ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE POS.OutletName END    
                            ORDER BY {7}";

                sql = string.Format(sql, startdate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , enddate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , PointOfSaleName
                                       , OutletName
                                       , DeptID
                                       , CategoryName
                                       , searchQry
                                       , sortExp
                                       , categoryFilter == "" ? "" : "AND C.CategoryName IN (" + categoryFilter + ") ");

                QueryCommand qc = new QueryCommand(sql);
                qc.CommandTimeout = 1000;
                dt.Load(DataService.GetReader(qc));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DataTable FetchOverallSalesReport(
                DateTime startdate,
                DateTime enddate,
                string searchQry,
                string PointOfSaleName,
                string OutletName,
                string CategoryName,
                string DeptID,
                string MembershipNo,
                string MembershipName,
                string SalesPerson,
                bool IsVoided,
                string SortColumn,
                string SortDir
            )
        {
            if (DeptID.ToUpper() == "ALL") { DeptID = ""; }
            if (CategoryName.ToUpper() == "ALL") { CategoryName = ""; }
            if (PointOfSaleName.ToUpper() == "ALL") { PointOfSaleName = ""; }
            if (OutletName.ToUpper() == "ALL") { OutletName = ""; }
            if (SortDir == "") { SortDir = "ASC"; }

            string SQL =
@"DECLARE @StartDate DATETIME; 
DECLARE @EndDate DATETIME; 
DECLARE @Search VARCHAR(500); 
DECLARE @PointOfSaleName VARCHAR(50); 
DECLARE @OutletName VARCHAR(50); 
DECLARE @CategoryName VARCHAR(50); 
DECLARE @DeptID VARCHAR(50); 
DECLARE @MembershipNo VARCHAR(500); 
DECLARE @MembershipName VARCHAR(500); 
DECLARE @SalesPerson VARCHAR(500); 

SET @StartDate = '{0}';
SET @EndDate = '{1}';
SET @Search = '{2}';
SET @PointOfSaleName = '{3}';
SET @OutletName = '{4}';
SET @CategoryName = '{5}';
SET @DeptID = '{6}';
SET @MembershipNo = '{7}';
SET @MembershipName = '{8}';
SET @SalesPerson = '{9}';

SELECT   TheOrder.PointOfSale
		,TheOrder.Outlet
		,TheOrder.InvoiceNo
		,TheOrder.OrderDate
		,TheOrder.SalesPerson
		,TheOrder.MembershipNo
		,TheOrder.MemberName
		,TheOrder.Department
		,TheOrder.Category
		,TheOrder.ItemNo
		,TheOrder.ItemName
		,TheOrder.Quantity
		,TheOrder.SalesPrice 
		,ISNULL(TheItem.FactoryPrice,0) FactoryPrice
		,TheOrder.SalesPrice - (TheOrder.Quantity * ISNULL(TheItem.FactoryPrice,0)) Profit
		,TheOrder.Remark
		,QtyAfter = dbo.StockBalanceOnDate(TheOrder.ItemNo, TheOrder.OrderDate)
FROM	(
SELECT   POS.PointOfSaleName PointOfSale
		,OU.OutletName Outlet
		,OU.InventoryLocationID InventoryLocationID
		,CAST(OH.OrderDate AS DATE) OrderDate
		,UM.DisplayName SalesPerson
		,M.MembershipNo MembershipNo
		,M.NameToAppear MemberName
		,ID.DepartmentName Department
		,C.CategoryName Category
		,I.ItemNo ItemNo
		,I.ItemName ItemName
		,SUM(OD.Quantity) Quantity
		,SUM(OD.Amount) SalesPrice
		,OD.Remark Remark
		, InvoiceNo = OH.userfld5
FROM	OrderHdr OH
		INNER JOIN PointOfSale POS ON OH.PointOfSaleID = POS.PointOfSaleID
		INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
		INNER JOIN Membership M ON M.MembershipNo = OH.MembershipNo
		INNER JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
		INNER JOIN Item I ON I.ItemNo = OD.ItemNo
		INNER JOIN Category C ON C.CategoryName = I.CategoryName
		INNER JOIN ItemDepartment ID ON ID.ItemDepartmentId = C.ItemDepartmentId
		INNER JOIN SalesCommissionRecord SC ON SC.OrderHdrID = OH.OrderHdrID 
		INNER JOIN UserMst UM ON UM.UserName = SC.SalesPersonID
WHERE	OH.IsVoided = 0
GROUP BY POS.PointOfSaleName
		,OU.OutletName
		,OU.InventoryLocationID
		,CAST(OH.OrderDate AS DATE)
		,UM.DisplayName
		,M.MembershipNo
		,M.NameToAppear
		,ID.DepartmentName
		,C.CategoryName
		,I.ItemNo
		,I.ItemName
		,OD.Remark
        , OH.OrderHdrID
		, OH.userfld5
) TheOrder LEFT OUTER JOIN (
SELECT  IH.InventoryLocationID, ID.ItemNo, AVG(ID.FactoryPrice) FactoryPrice, AVG(ID.CostOfGoods) CostOfGoods
FROM	InventoryHdr IH
		INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo
WHERE	IH.MovementType LIKE '% In%'
GROUP BY IH.InventoryLocationID, ID.ItemNo
) TheItem ON TheItem.ItemNo = TheOrder.ItemNo AND TheItem.InventoryLocationID = TheOrder.InventoryLocationID
WHERE	TheOrder.OrderDate BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
		AND ((TheOrder.PointOfSale LIKE '%'+@Search+'%')
			 OR (TheOrder.Outlet LIKE '%'+@Search+'%')
			 OR (TheOrder.SalesPerson LIKE '%'+@Search+'%')
			 OR (TheOrder.MembershipNo LIKE '%'+@Search+'%')
			 OR (TheOrder.MemberName LIKE '%'+@Search+'%')
			 OR (TheOrder.Department LIKE '%'+@Search+'%')
			 OR (TheOrder.Category LIKE '%'+@Search+'%')
			 OR (TheOrder.ItemNo LIKE '%'+@Search+'%')
			 OR (TheOrder.ItemName LIKE '%'+@Search+'%')
			 OR (TheOrder.Remark LIKE '%'+@Search+'%'))
		AND (TheOrder.PointOfSale LIKE '%'+@PointOfSaleName+'%')
		AND (TheOrder.Outlet LIKE '%'+@OutletName+'%')		
		AND (TheOrder.Category LIKE '%'+@CategoryName+'%')
		AND (TheOrder.Department LIKE '%'+@DeptID+'%')		
		AND (TheOrder.MembershipNo LIKE '%'+@MembershipNo+'%')		
		AND (TheOrder.MemberName LIKE '%'+@MembershipName+'%')		
		AND (TheOrder.SalesPrice LIKE '%'+@SalesPerson+'%')		
";

            SQL = string.Format(SQL, startdate.ToString("yyyy-MM-dd HH:mm:ss"), enddate.ToString("yyyy-MM-dd HH:mm:ss")
                , searchQry, PointOfSaleName, OutletName, CategoryName, DeptID
                , MembershipNo, MembershipName, SalesPerson);

            if (string.IsNullOrEmpty(SortColumn))
                SQL += string.Format(" ORDER BY TheOrder.OrderDate desc ");
            else
                SQL += string.Format(" ORDER BY TheOrder.{0} {1} ", SortColumn, SortDir);
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            cmd.CommandTimeout = 600;
            DataSet ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        Outlet ou = new Outlet(Outlet.Columns.OutletName, dt.Rows[i]["Outlet"].ToString());
                        int InventoryLocationID = ou.InventoryLocationID.GetValueOrDefault(0);
                        decimal cost = FetchCostOfGoodsAvg(dt.Rows[i]["ItemNo"].ToString(), InventoryLocationID);
                        decimal salesPrice = (decimal)dt.Rows[i]["SalesPrice"];
                        int quantity = (int)dt.Rows[i]["Quantity"];
                        //dt.Rows[i]["FactoryPrice"] = cost;
                        cost = decimal.Parse(dt.Rows[i]["FactoryPrice"].ToString());
                        dt.Rows[i]["Profit"] = salesPrice - (quantity * cost);
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                    }
                }
                return dt;
            }
            else
                return null;


        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DataTable FetchProfitLossReport(
                 DateTime startdate,
                 DateTime enddate,
                 string OutletName,
                 string DeptID,
                 string SortColumn,
                 string SortDir
             )
        {
            if (OutletName == "") { OutletName = "ALL"; }
            if (OutletName == "ALL - BreakDown") { OutletName = "%"; }
            if (SortColumn == "") { SortColumn = "ProfitLoss"; }
            if (SortDir == "") { SortDir = "DESC"; }
            if (DeptID == "0") { DeptID = ""; }

            /*Where clause
            Query qry = new Query("OrderHdr");
            Where whr = new Where();
            whr.ColumnName = "";
            qry.GetSum(OrderHdr.Columns.GrossAmount, whr);                        
            //use sum*/
            DataSet dsAll = new DataSet();
            DataTable dtAll = new DataTable();

            DataSet ds = SPs.FetchProfitLossReport(startdate, enddate, OutletName, DeptID, SortColumn, SortDir).GetDataSet();

            return ds.Tables[0];
        }

        public static bool IsProfitAndLossShowIncomplete()
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            bool Rst = false;
            Rst = Rst || CostingMethod == InventoryController.CostingTypes.FIFO;

            return Rst;
        }

        public static DataTable FetchProfitLossReportOnTransactionLevel(
                 DateTime startdate, DateTime enddate, string OutletName, string DeptID,
                 string SortColumn, string SortDir
             )
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            startdate = startdate.Date;
            enddate = enddate.Date.AddDays(1);

            if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return ReportController.FetchProfitLossReportOnTransactionLevel_FIFO(startdate, enddate, OutletName, DeptID, SortColumn, SortDir);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)
                return ReportController.FetchProfitLossReportOnTransactionLevel_FixedAvg(startdate, enddate, OutletName, DeptID, SortColumn, SortDir);
            else
                return ReportController.FetchProfitLossReportOnTransactionLevel_FIFO(startdate, enddate, OutletName, DeptID, SortColumn, SortDir);

        }
        private static DataTable FetchProfitLossReportOnTransactionLevel_FIFO(
                 DateTime startdate, DateTime enddate, string OutletName, string DeptID,
                 string SortColumn, string SortDir
             )
        {
            if (OutletName == "") { OutletName = "ALL"; }
            if (OutletName == "ALL - BreakDown") { OutletName = "%"; }
            if (SortColumn == "") { SortColumn = "OrderDate"; }
            if (SortDir == "") { SortDir = "DESC"; }
            if (DeptID == "0") { DeptID = ""; }

            string SQL = "declare @startdate as datetime; " +
                            "declare @enddate  as datetime; " +
                            "declare @OutletName as varchar(50); " +
                            "declare @DeptID as varchar(5); " +
                            "declare @sortby as varchar(50); " +
                            "declare @sortdir as varchar(5); " +
                            "set @startdate = '" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                            "set @enddate = '" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                            "set @OutletName = '" + OutletName + "'; " +
                            "set @DeptID = '" + DeptID + "'; " +
                            "set @sortby = '" + SortColumn + "'; " +
                            "set @sortdir = '" + SortDir + "'; " +
                            "set @sortby = ltrim(rtrim(@sortby)); " +
                            "if (@Outletname = 'ALL') " +
                            "begin " +
                            "select OrderHdr.OrderHdrid as orderhdrid,OrderHdr.Userfld5  as InVoiceNo,orderrefno,orderdate,grossAmount as GrossSales, " +
                            " DiscountAmount as DiscountSales, " +
                            " (DiscountAmount / (GrossAmount + 0.001)) as DiscountPercentage, " +
                            " NettAmount as NettSalesBeforeGST, " +
                            " OrderHdr.GSTAmount as GSTAmount,  " +
                            " NettAmount - OrderHdr.GSTAmount as NettSalesAfterGST, " +
                            " Orderhdr.GST,  " +
                            "sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)) as CostOfGoodsSold , " +
                            "(NettAmount - OrderHdr.GSTAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0))) as ProfitLoss, " +
                            "(NettAmount - OrderHdr.GSTAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)))/(NettAmount + 0.0001) as ProfitLossPercentage, " +
                            "'ALL' as OutletName " +
                            "from " +
                            "orderhdr inner join orderdet on OrderHdr.orderhdrid=orderdet.orderhdrid " +
                            "inner join pointofsale on pointofsale.pointofsaleid= orderhdr.pointofsaleid " +
                            "left join viewinventorysalesstockout " +
                            "on orderdet.inventoryhdrrefno = viewinventorysalesstockout.inventoryhdrrefno  " +
                            "and orderdet.itemno=viewinventorysalesstockout.itemno " +
                            "where " +
                            "orderdate >= @startdate  " +
                            "and " +
                            "orderdate <= @enddate " +
                            "AND PointOfSale.DepartmentID like '%' + @DeptID " +
                            "and orderhdr.isvoided=0 and orderdet.isvoided=0 " +
                            "group by OrderHdr.OrderHdrid,OrderHdr.Userfld5,OrderHdr.Userfld5,orderrefno,grossAmount,NettAmount,DiscountAmount, orderhdr.GST,  " +
                            " orderdate,orderhdr.pointofsaleid,OrderHdr.GstAmount " +
                            "order by " +
                            "case WHEN @sortby = 'GrossSales' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by GrossAmount desc) " +
                            "WHEN @sortby = 'GrossSales' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by GrossAmount asc) " +
                            "WHEN @sortby = 'DiscountSales' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by DiscountAmount desc) " +
                            "WHEN @sortby = 'DiscountSales' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by DiscountAmount asc) " +
                            "WHEN @sortby = 'DiscountPercentage' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by (DiscountAmount / (GrossAmount + 0.001)) desc) " +
                            "WHEN @sortby = 'DiscountPercentage' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by (DiscountAmount / (GrossAmount + 0.001)) asc) " +
                            "WHEN @sortby = 'NettSalesBeforeGST' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by (NettAmount * (1 + orderhdr.GST/100)) desc) " +
                            "WHEN @sortby = 'NettSalesBeforeGST' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by (NettAmount * (1 + orderhdr.GST/100)) asc) " +
                            "WHEN @sortby = 'GSTAmount' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by ((NettAmount * (OrderHdr.GST/100)) / (1+(OrderHDR.GST/100))) desc) " +
                            "WHEN @sortby = 'GSTAmount' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by ((NettAmount * (OrderHdr.GST/100)) / (1+(OrderHDR.GST/100))) asc) " +
                            "WHEN @sortby = 'CostOfGoodsSold' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)) desc) " +
                            "WHEN @sortby = 'CostOfGoodsSold' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)) asc) " +
                            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0))) desc) " +
                            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0))) asc) " +
                            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
                            "THEN rank() over " +
                            "(order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)))/(NettAmount + 0.0001) desc) " +
                            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
                            "THEN rank() over   " +
                            "(order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)))/(NettAmount + 0.0001) asc) " +
                            "ELSE  " +
                            "rank() over   " +
                            "(order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)))/(NettAmount + 0.0001) desc) " +
                            "End " +
                            "end " +
                            "else " +
                            "begin " +
                            "select  OrderHdr.OrderHdrId  as orderhdrid,OrderHdr.Userfld5 as InVoiceNo, orderrefno,orderdate,grossAmount as GrossSales, " +
                            " DiscountAmount as DiscountSales, " +
                            " (DiscountAmount / (GrossAmount + 0.001)) as DiscountPercentage, " +
                            " (NettAmount - OrderHdr.GSTAmount) as NettSalesAfterGST, " +
                            " OrderHdr.GSTAmount, " +
                            " NettAmount as NettSalesBeforeGST, " +
                            "orderhdr.GST,  " +
                            "sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)) as CostOfGoodsSold , " +
                            "(NettAmount - OrderHdr.GSTAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0))) as ProfitLoss, " +
                            "(NettAmount - OrderHdr.GSTAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)))/(NettAmount + 0.0001) as ProfitLossPercentage, " +
                            "Outlet.Outletname as OutletName " +
                            "from  " +
                            "orderhdr " +
                            "inner join orderdet on orderhdr.orderhdrid = orderdet.orderhdrid " +
                            "INNER JOIN PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  " +
                            "INNER JOIN Outlet ON PointOfSale.OutletName = Outlet.OutletName " +
                            "left join viewinventorysalesstockout " +
                            "on orderdet.inventoryhdrrefno = viewinventorysalesstockout.inventoryhdrrefno  " +
                            "and orderdet.itemno=viewinventorysalesstockout.itemno " +
                            "where " +
                            "orderdate >= @startdate  " +
                            "and " +
                            "orderdate <= @enddate " +
                            "and Outlet.Outletname like @outletname " +
                            "AND PointOfSale.DepartmentID like '%' + @DeptID " +
                            "and orderhdr.isvoided=0 and orderdet.isvoided=0 " +
                            "group by OrderHdr.OrderHdrId,OrderHdr.Userfld5,orderrefno,grossAmount,NettAmount, " +
                            "DiscountAmount, orderhdr.GST, orderdate,Outlet.Outletname, OrdeRHdr.GSTAmount " +
                            "order by " +
                            "case WHEN @sortby = 'GrossSales' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by GrossAmount desc) " +
                            "WHEN @sortby = 'GrossSales' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by GrossAmount asc) " +
                            "WHEN @sortby = 'DiscountSales' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by DiscountAmount desc) " +
                            "WHEN @sortby = 'DiscountSales' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by DiscountAmount asc) " +
                            "WHEN @sortby = 'DiscountPercentage' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by (DiscountAmount / (GrossAmount + 0.001)) desc) " +
                            "WHEN @sortby = 'DiscountPercentage' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by (DiscountAmount / (GrossAmount + 0.001)) asc) " +
                            "WHEN @sortby = 'NettSalesBeforeGST' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by (NettAmount * (1 + orderhdr.GST/100)) desc) " +
                            "WHEN @sortby = 'NettSalesBeforeGST' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by (NettAmount * (1 + orderhdr.GST/100)) asc) " +
                            "WHEN @sortby = 'GSTAmount' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by ((NettAmount * (OrderHdr.GST/100)) / (1+(OrderHDR.GST/100))) desc) " +
                            "WHEN @sortby = 'GSTAmount' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by ((NettAmount * (OrderHdr.GST/100)) / (1+(OrderHDR.GST/100))) asc) " +
                            "WHEN @sortby = 'CostOfGoodsSold' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)) desc) " +
                            "WHEN @sortby = 'CostOfGoodsSold' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)) asc) " +
                            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
                            "THEN rank() over (order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0))) desc) " +
                            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
                            "THEN rank() over (order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0))) asc) " +
                            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
                            "THEN rank() over " +
                            "(order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)))/(NettAmount + 0.0001) desc) " +
                            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
                            "THEN rank() over   " +
                            "(order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)))/(NettAmount + 0.0001) asc) " +
                            "ELSE rank() over   " +
                            "(order by (NettAmount - sum(isnull(costofgoods,0) * isnull(ViewInventorySalesStockOut.Quantity,0)))/(NettAmount + 0.0001) desc) " +
                            "end " +
                            "end ";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            return DataService.GetDataSet(cmd).Tables[0];
            /*
            DataSet dsAll = new DataSet();
            DataTable dtAll = new DataTable();

            DataSet ds = SPs.FetchProfitAndLossOnTransactionLevel(startdate, enddate,
                            OutletName, DeptID, SortColumn, SortDir).GetDataSet();

            return ds.Tables[0];
             */
        }

        public static DataTable FetchProfitLossReportOnTransactionLevel_NEW(DateTime startDate,
            DateTime endDate, string outletName, string deptId, bool includePreOrder, string sortColumn, string sortDir)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(outletName)) outletName = "ALL";
            if (outletName == "ALL-BreakDown" || outletName == "ALL - BreakDown") outletName = "ALL";
            if (string.IsNullOrEmpty(sortColumn)) sortColumn = "OrderDate";
            if (string.IsNullOrEmpty(sortDir)) sortDir = "DESC";
            try
            {
                string sql = @"DECLARE @StartDate as DATETIME; 
                            DECLARE @EndDate  as DATETIME; 
                            DECLARE @OutletName as VARCHAR(50); 
                            DECLARE @IsPreOrder as bit;

                            SET @StartDate = '{0}'; 
                            SET @EndDate = '{1}'; 
                            SET @IsPreOrder = {5}
                            SET @OutletName = N'{2}'; 

                            SELECT  OH.OrderHdrID
		                            ,OH.userfld5 InVoiceNo
		                            ,OH.OrderRefNo
		                            ,OH.OrderDate
		                            ,SUM(ISNULL(OD.GrossSales,0)) GrossSales
		                            ,SUM(ISNULL(OD.DiscountSales,0)) DiscountSales
		                            ,SUM(ISNULL(OD.DiscountPercentage,0)) DiscountPercentage 
		                            ,SUM(ISNULL(OD.NettSalesBeforeGST,0)) NettSalesBeforeGST
		                            ,SUM(ISNULL(OD.GSTAmount,0)) AS GSTAmount
		                            ,SUM(ISNULL(OD.NettSalesAfterGST,0)) AS NettSalesAfterGST
		                            ,OH.GST 		
		                            ,SUM(ISNULL(OD.CostOfGoodSold,0)) CostOfGoodsSold
		                            ,SUM(ISNULL(OD.ProfitLoss,0)) AS ProfitLoss 
		                            ,CASE WHEN SUM(ISNULL(OD.NettSalesAfterGST,0)) = 0 THEN 0 ELSE  SUM(ISNULL(OD.ProfitLoss,0)) / SUM(ISNULL(OD.NettSalesAfterGST,0)) END AS ProfitLossPercentage 		
		                            ,POS.PointOfSaleName
		                            ,OU.OutletName 		
                            FROM	OrderHdr OH
		                            INNER JOIN (
			                            SELECT  d.GrossSales
											    ,d.GrossSales- d.Amount DiscountSales
                                                ,CASE WHEN d.GrossSales = 0 THEN 0
                                                    ELSE (d.GrossSales-d.Amount)/d.GrossSales END DiscountPercentage
                                                ,d.Amount as NettSalesBeforeGST
                                                ,ROUND(ISNULL(d.GSTAmount,0),2) AS GSTAmount
                                                ,d.Amount - ROUND(ISNULL(d.GSTAmount,0),2) AS NettSalesAfterGST
												,CASE WHEN ISNULL(d.IsPreOrder,0) = 1 and ISNULL(DH.IsDelivered,0) = 0 then IT.FactoryPrice else ISNULL(d.CostOfGoodSold,0) END CostOfGoodSold  
                                                ,d.Amount - ISNULL(d.GSTAmount,0)  - (CASE WHEN ISNULL(d.IsPreOrder,0) = 1 and ISNULL(DH.IsDelivered,0) = 0 then IT.FactoryPrice else ISNULL(d.CostOfGoodSold,0) END) AS ProfitLoss
												,CASE WHEN d.Amount-ISNULL(d.GSTAmount,0) = 0 THEN 0
                                                      ELSE  ((d.Amount) - ISNULL(d.GSTAmount,0) - (CASE WHEN ISNULL(d.IsPreOrder,0) = 1 and ISNULL(DH.IsDelivered,0) = 0 then IT.FactoryPrice else ISNULL(d.CostOfGoodSold,0) END))
                                                            / (d.Amount -ISNULL(d.GSTAmount,0))
                                                      END ProfitLossPercentage                                              
					                            ,d.OrderHdrID
			                            FROM	OrderDet d
                                        LEFT JOIN Item IT on IT.ItemNo = d.ItemNo
                                        LEFT JOIN DeliveryOrderDetails DD on DD.OrderDetID = d.OrderDetID
                                        LEFT JOIN DeliveryOrder DH on DH.OrderNumber = DD.DOHDRID 
			                            WHERE	d.IsVoided = 0 
                                            AND ISNULL(IT.Userint5,0) = 0
                                            AND (ISNULL(d.IsPreOrder,0) = 0 OR (ISNULL(d.IsPreOrder,0) = 1 and ISNULL(DH.IsDelivered,0) = 1) 
                                           OR (ISNULL(d.IsPreOrder,0) = CASE WHEN @IsPreOrder = 1 AND ISNULL(DH.IsDelivered,0) = 0 THEN 1 ELSE 0 END)) 
			                            --GROUP BY OrderHdrID		
		                            ) OD ON OD.OrderHdrID = OH.OrderHdrID
		                            LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
		                            LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                            WHERE	OH.IsVoided = 0
		                            AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE) 
		                            AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
		                            AND (POS.OutletName = @OutletName OR @OutletName = 'ALL')
							GROUP BY OH.OrderHdrID
		                            ,OH.userfld5 
		                            ,OH.OrderRefNo
		                            ,OH.OrderDate
									,OH.GST 
									,POS.PointOfSaleName
		                            ,OU.OutletName 
                            ORDER BY {3} {4}";

                sql = string.Format(sql, startDate.ToString("yyyy-MM-dd")
                                       , endDate.ToString("yyyy-MM-dd")
                                       , outletName
                                       , sortColumn
                                       , sortDir
                                       , includePreOrder ? "1" : "0"
                                       );

                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return dt;
        }

        private static DataTable FetchProfitLossReportOnTransactionLevel_FixedAvg(
                 DateTime startdate, DateTime enddate, string OutletName, string DeptID,
                 string SortColumn, string SortDir
             )
        {
            if (OutletName == "") { OutletName = "ALL"; }
            //if (OutletName == "ALL - BreakDown") { OutletName = "%"; }
            if (SortColumn == "") { SortColumn = "OrderDate"; }
            if (SortDir == "") { SortDir = "DESC"; }
            if (DeptID == "0") { DeptID = ""; }

            string SQL =
                "declare @startdate as DATETIME; " +
                "declare @enddate  as DATETIME; " +
                "declare @OutletName as VARCHAR(50); " +
                "set @startdate = '" + startdate.ToString("yyyy-MM-dd") + "'; " +
                "set @enddate = '" + enddate.ToString("yyyy-MM-dd") + "'; " +
                "set @OutletName = '" + OutletName + "'; " +
                "SELECT  OH.OrderhdrId  as orderhdrid,OH.Userfld5 as InVoiceNo, OrderRefNo, OrderDate, GrossAmount AS GrossSales, DiscountAmount AS DiscountSales " +
                    ", CASE WHEN GrossAmount = 0 THEN 0 ELSE DiscountAmount / GrossAmount END AS DiscountPercentage " +
                    ", NettAmount AS NettSalesBeforeGST, ISNULL(OH.GSTAmount,0) AS GSTAmount, NettAmount - ISNULL(OH.GSTAmount,0) AS NettSalesAfterGST, OH.GST " +
                    ", SUM(ISNULL(IC.COG,0) * OD.Quantity) AS CostOfGoodsSold " +
                    ", (NettAmount - ISNULL(OH.GSTAmount,0) - SUM(ISNULL(IC.COG,0) * OD.Quantity)) AS ProfitLoss " +
                    ", CASE WHEN (NettAmount - ISNULL(OH.GSTAmount,0)) = 0 THEN 0 ELSE (NettAmount - ISNULL(OH.GSTAmount,0) - SUM(ISNULL(IC.COG,0) * OD.Quantity)) / (NettAmount - ISNULL(OH.GSTAmount,0)) END AS ProfitLossPercentage " +
                    ", LP.PointOfSaleName, LO.OutletName " +
                "FROM OrderHdr OH " +
                    "INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
                    "INNER JOIN PointOfSale LP ON LP.PointOfSaleID = OH.PointOfSaleID " +
                    "INNER JOIN Outlet LO ON LP.OutletName = LO.OutletName " +
                    "LEFT JOIN " +
                    "( " +
                        "SELECT InventoryLocationID, ItemNo, SUM(Quantity) AS Quantity" +
                            ", CASE WHEN SUM(Quantity) = 0 THEN 0 ELSE SUM(Quantity * CostOfGoods) / SUM(Quantity) END AS COG " +
                        "FROM InventoryHdr IH " +
                            "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                        "WHERE MovementType LIKE '% In' " +
                        "GROUP BY InventoryLocationID, ItemNo " +
                    ") IC ON LO.InventoryLocationID = IC.InventoryLocationID AND OD.ItemNo = IC.ItemNo " +
                "WHERE OrderDate BETWEEN @startdate AND @enddate " +
                    "AND OH.IsVoided = 0 AND OD.IsVoided = 0 " +
                    "AND LO.OutletName LIKE CASE WHEN LOWER(@OutletName) = 'all' THEN '%' ELSE @OutletName END " +
                "GROUP BY OH.OrderHdrId,OH.Userfld5, OrderRefNo, OrderDate, GrossAmount, DiscountAmount, NettAmount, OH.GSTAmount, OH.GST, PointOfSaleName, LO.OutletName " +
                "ORDER BY " + SortColumn + " " + SortDir;

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            return DataService.GetDataSet(cmd).Tables[0];
            /*
            DataSet dsAll = new DataSet();
            DataTable dtAll = new DataTable();

            DataSet ds = SPs.FetchProfitAndLossOnTransactionLevel(startdate, enddate,
                            OutletName, DeptID, SortColumn, SortDir).GetDataSet();

            return ds.Tables[0];
             */
        }

        public static DataTable FetchProfitLossReportGroupByDay
            (DateTime startdate, DateTime enddate, string OutletName, string DeptID
            , string SortColumn, string SortDir)
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            startdate = startdate.Date;
            enddate = enddate.Date.AddDays(1);

            if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return ReportController.FetchProfitLossReportGroupByDay_FIFO(startdate, enddate, OutletName, DeptID, SortColumn, SortDir);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)
                return ReportController.FetchProfitLossReportGroupByDay_FixedAvg(startdate, enddate, OutletName, DeptID, SortColumn, SortDir);
            else
                return ReportController.FetchProfitLossReportGroupByDay_FIFO(startdate, enddate, OutletName, DeptID, SortColumn, SortDir);
        }
        public static DataTable FetchProfitLossReportGroupByDay_FIFO
            (DateTime startdate, DateTime enddate, string OutletName, string DeptID
            , string SortColumn, string SortDir)
        {
            if (OutletName == "") { OutletName = "ALL"; }
            if (OutletName == "ALL - BreakDown") { OutletName = "%"; }
            if (SortColumn == "") { SortColumn = "ProfitLoss"; }
            if (SortDir == "") { SortDir = "DESC"; }
            if (DeptID == "0") { DeptID = ""; }

            /*Where clause
            Query qry = new Query("OrderHdr");
            Where whr = new Where();
            whr.ColumnName = "";
            qry.GetSum(OrderHdr.Columns.GrossAmount, whr);                        
            //use sum*/
            int DaySpan = 1;

            DataTable dtAll = new DataTable();
            dtAll.Columns.Add("PLDate", System.Type.GetType("System.DateTime"));
            dtAll.Columns.Add("GrossSales", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("DiscountSales", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("DiscountPercentage", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("NettSalesBeforeGST", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("GSTAmount", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("NettSalesAfterGST", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("CostOfGoodsSold", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("ProfitLoss", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("ProfitLossPercentage", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("OutletName");
            DateTime beginingdate = startdate;

            DataRow dr;
            DataTable dtTemp;

            while (startdate < enddate)
            {
                dtTemp = (SPs.FetchProfitLossReport(startdate, startdate.AddMinutes(1439),
                                OutletName, DeptID, SortColumn, SortDir).GetDataSet()).Tables[0];
                //copy table
                for (int t = 0; t < dtTemp.Rows.Count; t++)
                {
                    if ((decimal)dtTemp.Rows[0]["GrossSales"] > 0)
                    {
                        dr = dtAll.NewRow();
                        dr["PLDate"] = startdate.ToString("dd MMM yyyy");
                        dr["GrossSales"] = dtTemp.Rows[t]["GrossSales"];
                        dr["DiscountSales"] = dtTemp.Rows[t]["DiscountSales"];
                        dr["DiscountPercentage"] = dtTemp.Rows[t]["DiscountPercentage"];
                        dr["NettSalesBeforeGST"] = dtTemp.Rows[t]["NettSalesBeforeGST"];
                        dr["GSTAmount"] = dtTemp.Rows[t]["GSTAmount"];
                        dr["NettSalesAfterGST"] = dtTemp.Rows[t]["NettSalesAfterGST"];
                        dr["CostOfGoodsSold"] = dtTemp.Rows[t]["CostOfGoodsSold"];
                        dr["ProfitLoss"] = dtTemp.Rows[t]["ProfitLoss"];
                        dr["ProfitLossPercentage"] = dtTemp.Rows[t]["ProfitLossPercentage"];
                        dr["OutletName"] = dtTemp.Rows[t]["OutletName"];
                        dtAll.Rows.Add(dr);
                    }
                }
                startdate = startdate.AddDays(DaySpan);
            }

            return dtAll;
        }

        public static DataTable FetchProfitLossReportGroupByDay_NEW
    (DateTime startdate, DateTime enddate, string outletName, string deptID, bool includePreOrder
    , string sortColumn, string sortDir)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(outletName)) outletName = "ALL";
            if (outletName == "ALL-BreakDown") outletName = "ALL";
            if (string.IsNullOrEmpty(sortColumn)) sortColumn = "OrderDate";
            if (string.IsNullOrEmpty(sortDir)) sortDir = "DESC";

            try
            {
                string sql = @"DECLARE @StartDate as DATETIME; 
                                DECLARE @EndDate  as DATETIME; 
                                DECLARE @OutletName as VARCHAR(50); 
                                DECLARE @IsPreOrder as bit;

                                SET @StartDate = '{0}'; 
                                SET @EndDate = '{1}'; 
                                SET @IsPreOrder = {5}

                                SET @OutletName = N'{2}'; 
                                SELECT   CAST(OH.OrderDate AS DATE) PLDate
		                                ,COUNT(DISTINCT OH.OrderHdrID) NoOfTransaction
		                                ,SUM(OD.GrossSales)  GrossSales
		                                ,SUM(OD.GrossSales)-SUM(OD.Amount) DiscountSales
                                        ,CASE WHEN SUM(OD.GrossSales) = 0 THEN 0
                                              ELSE (SUM(OD.GrossSales)-SUM(OD.Amount))/SUM(OD.GrossSales) END DiscountPercentage		
		                                ,SUM(OD.Amount) NettSalesBeforeGST
		                                ,SUM(ROUND(ISNULL(OD.GSTAmount,0),2)) AS GSTAmount
                                        ,SUM(OD.Amount)-SUM(ROUND(ISNULL(OD.GSTAmount,0),2)) AS NettSalesAfterGST
		                                ,SUM(CASE WHEN ISNULL(OD.IsPreOrder,0) = 1 and ISNULL(DH.IsDelivered,0) = 0 then IT.FactoryPrice else ISNULL(OD.CostOfGoodSold,0) END) CostOfGoodsSold
		                                ,SUM(OD.Amount) - SUM(ISNULL(OD.GSTAmount,0)) - SUM(CASE WHEN ISNULL(OD.IsPreOrder,0) = 1 and ISNULL(DH.IsDelivered,0) = 0 then IT.FactoryPrice else ISNULL(OD.CostOfGoodSold,0) END) AS ProfitLoss
		                                ,CASE WHEN (SUM(OD.Amount)-SUM(ISNULL(OD.GSTAmount,0))) = 0 THEN 0
			                                  ELSE  (SUM(OD.Amount) - SUM(ISNULL(OD.GSTAmount,0)) - SUM(CASE WHEN ISNULL(OD.IsPreOrder,0) = 1 and ISNULL(DH.IsDelivered,0) = 0 then IT.FactoryPrice else ISNULL(OD.CostOfGoodSold,0) END)) 
					                                / (SUM(OD.Amount)-SUM(ISNULL(OD.GSTAmount,0)))
			                                  END ProfitLossPercentage
                                        ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE OU.OutletName END OutletName
                                        ,{5} as IncludePreOrder
                                FROM	OrderHdr OH
		                                LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                                        LEFT JOIN Item IT on IT.ItemNo = OD.ItemNo
                                        LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                                        LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                        LEFT JOIN DeliveryOrderDetails DD on dd.OrderDetID = OD.OrderDetID
                                        LEFT JOIN DeliveryOrder DH on DH.OrderNumber = DD.DOHDRID 
                                WHERE	OH.IsVoided = 0
                                        AND ISNULL(IT.Userint5,0) = 0 
                                        AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE) 
                                        AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
                                        AND (POS.OutletName = @OutletName OR @OutletName = 'ALL'OR @OutletName = 'ALL - BreakDown')
                                        AND (ISNULL(OD.IsPreOrder,0) = 0 OR (ISNULL(OD.IsPreOrder,0) = 1 and ISNULL(DH.IsDelivered,0) = 1) 
                                           OR (ISNULL(OD.IsPreOrder,0) = CASE WHEN @IsPreOrder = 1 AND ISNULL(DH.IsDelivered,0) = 0 THEN 1 ELSE 0 END))                                        
                                GROUP BY CAST(OH.OrderDate AS DATE)
                                        ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE OU.OutletName END
                                ORDER BY {3} {4}";

                sql = string.Format(sql, startdate.ToString("yyyy-MM-dd")
                                       , enddate.ToString("yyyy-MM-dd")
                                       , outletName
                                       , sortColumn
                                       , sortDir
                                       , includePreOrder ? "1" : "0");
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }


        public static DataTable FetchProfitLossReportGroupByDay_FixedAvg
            (DateTime startdate, DateTime enddate, string OutletName, string DeptID
            , string SortColumn, string SortDir)
        {
            if (OutletName == "") { OutletName = "ALL"; }
            if (OutletName == "ALL - BreakDown") { OutletName = "%"; }
            if (SortColumn == "") { SortColumn = "ProfitLoss"; }
            if (SortDir == "") { SortDir = "DESC"; }
            //if (DeptID == "0") { DeptID = ""; }

            /*Where clause
            Query qry = new Query("OrderHdr");
            Where whr = new Where();
            whr.ColumnName = "";
            qry.GetSum(OrderHdr.Columns.GrossAmount, whr);                        
            //use sum*/
            int DaySpan = 1;

            DataTable dtAll = new DataTable();
            dtAll.Columns.Add("PLDate", System.Type.GetType("System.DateTime"));
            dtAll.Columns.Add("GrossSales", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("DiscountSales", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("DiscountPercentage", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("NettSalesBeforeGST", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("GSTAmount", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("NettSalesAfterGST", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("CostOfGoodsSold", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("ProfitLoss", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("ProfitLossPercentage", System.Type.GetType("System.Decimal"));
            dtAll.Columns.Add("OutletName");
            DateTime beginingdate = startdate;

            DataRow dr;
            DataTable dtTemp;

            while (startdate < enddate)
            {
                string SQL =
                    "declare @startdate as DATETIME; " +
                    "declare @enddate  as DATETIME; " +
                    "declare @OutletName as VARCHAR(50); " +
                    "set @startdate = '" + startdate.ToString("yyyy-MM-dd") + "'; " +
                    "set @enddate = '" + startdate.AddDays(1).ToString("yyyy-MM-dd") + "'; " +
                    "set @OutletName = '" + OutletName + "'; " +
                    "SELECT ISNULL(SUM(GrossSales),0) AS GrossSales, ISNULL(SUM(DiscountSales),0) AS DiscountSales " +
                        ", CASE WHEN ISNULL(SUM(GrossSales),0) = 0 THEN 0 ELSE ISNULL(SUM(DiscountSales),0) / ISNULL(SUM(GrossSales),0) END AS DiscountPercentage " +
                        ", ISNULL(SUM(NettSalesBeforeGST),0) AS NettSalesBeforeGST, ISNULL(SUM(GSTAmount),0) AS GSTAmount" +
                        ", ISNULL(SUM(NettSalesBeforeGST),0) - ISNULL(SUM(GSTAmount),0) AS NettSalesAfterGST " +
                        ", ISNULL(SUM(CostOfGoodsSold),0) AS CostOfGoodsSold " +
                        ", (ISNULL(SUM(NettSalesBeforeGST),0) - ISNULL(SUM(GSTAmount),0) - ISNULL(SUM(CostOfGoodsSold),0)) AS ProfitLoss " +
                        ", CASE WHEN ISNULL(SUM(NettSalesBeforeGST),0) - ISNULL(SUM(GSTAmount),0) = 0 THEN 0 ELSE (ISNULL(SUM(NettSalesBeforeGST),0) - ISNULL(SUM(GSTAmount),0) - ISNULL(SUM(CostOfGoodsSold),0)) / (ISNULL(SUM(NettSalesBeforeGST),0) - ISNULL(SUM(GSTAmount),0)) END AS ProfitLossPercentage " +
                        (OutletName == "ALL" ? ", 'ALL' PointOfSaleName, 'ALL' OutletName " : ", PointOfSaleName, OutletName ") +
                    "FROM " +
                    "( " +
                        "SELECT GrossAmount AS GrossSales, DiscountAmount AS DiscountSales " +
                            ", CASE WHEN GrossAmount = 0 THEN 0 ELSE DiscountAmount / GrossAmount END AS DiscountPercentage " +
                            ", NettAmount AS NettSalesBeforeGST, OH.GSTAmount AS GSTAmount, NettAmount - OH.GSTAmount AS NettSalesAfterGST, OH.GST " +
                            ", SUM(ISNULL(IC.COG,0) * OD.Quantity) AS CostOfGoodsSold " +
                            ", (NettAmount - OH.GSTAmount - SUM(ISNULL(IC.COG,0) * OD.Quantity)) AS ProfitLoss " +
                            ", CASE WHEN (NettAmount - OH.GSTAmount) = 0 THEN 0 ELSE (NettAmount - OH.GSTAmount - SUM(ISNULL(IC.COG,0) * OD.Quantity)) / (NettAmount - OH.GSTAmount) END AS ProfitLossPercentage " +
                            ", LP.PointOfSaleName, LO.OutletName " +
                        "FROM OrderHdr OH " +
                            "INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
                            "INNER JOIN PointOfSale LP ON LP.PointOfSaleID = OH.PointOfSaleID " +
                            "INNER JOIN Outlet LO ON LP.OutletName = LO.OutletName " +
                            "LEFT JOIN " +
                            "( " +
                                "SELECT InventoryLocationID, ItemNo, SUM(Quantity) AS Quantity" +
                                    ", CASE WHEN SUM(Quantity) = 0 THEN 0 ELSE SUM(Quantity * CostOfGoods) / SUM(Quantity) END AS COG " +
                                "FROM InventoryHdr IH " +
                                    "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                                "WHERE MovementType LIKE '% In' " +
                                "GROUP BY InventoryLocationID, ItemNo " +
                            ") IC ON LO.InventoryLocationID = IC.InventoryLocationID AND OD.ItemNo = IC.ItemNo " +
                        "WHERE OrderDate BETWEEN @startdate AND @enddate " +
                            "AND OH.IsVoided = 0 AND OD.IsVoided = 0 AND OD.ItemNo <> 'INST_PAYMENT' " +
                            "AND LO.OutletName LIKE CASE WHEN LOWER(@OutletName) = 'all' THEN '%' ELSE @OutletName END " +
                        "GROUP BY OH.OrderHdrID, OrderRefNo, OrderDate, GrossAmount, DiscountAmount, NettAmount, OH.GSTAmount, OH.GST, PointOfSaleName, LO.OutletName " +
                    ") SP " +
                    (OutletName == "ALL" ? "" : "GROUP BY PointOfSaleName, OutletName ");
                //"ORDER BY " + SortColumn + " " + SortDir;

                dtTemp = new DataTable();
                dtTemp.Load(DataService.GetReader(new QueryCommand(SQL)));
                //dtTemp = (SPs.FetchProfitLossReport(startdate, startdate.AddMinutes(1439),
                //                OutletName, DeptID, SortColumn, SortDir).GetDataSet()).Tables[0];
                //copy table
                for (int t = 0; t < dtTemp.Rows.Count; t++)
                {
                    if ((decimal)dtTemp.Rows[0]["GrossSales"] > 0)
                    {
                        dr = dtAll.NewRow();
                        dr["PLDate"] = startdate.ToString("dd MMM yyyy");
                        dr["GrossSales"] = dtTemp.Rows[t]["GrossSales"];
                        dr["DiscountSales"] = dtTemp.Rows[t]["DiscountSales"];
                        dr["DiscountPercentage"] = dtTemp.Rows[t]["DiscountPercentage"];
                        dr["NettSalesBeforeGST"] = dtTemp.Rows[t]["NettSalesBeforeGST"];
                        dr["GSTAmount"] = dtTemp.Rows[t]["GSTAmount"];
                        dr["NettSalesAfterGST"] = dtTemp.Rows[t]["NettSalesAfterGST"];
                        dr["CostOfGoodsSold"] = dtTemp.Rows[t]["CostOfGoodsSold"];
                        dr["ProfitLoss"] = dtTemp.Rows[t]["ProfitLoss"];
                        dr["ProfitLossPercentage"] = dtTemp.Rows[t]["ProfitLossPercentage"];
                        dr["OutletName"] = dtTemp.Rows[t]["OutletName"];
                        dtAll.Rows.Add(dr);
                    }
                }
                startdate = startdate.AddDays(DaySpan);
            }

            return dtAll;
        }



        public static DataTable FetchCollectionReport(
              DateTime startdate,
              DateTime enddate,
              string PointOfSaleName,
              string OutletName,
              string SortColumn,
              string SortDir
          )
        {
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortColumn == "") { SortColumn = "TotalCollected"; }
            if (SortDir == "") { SortDir = "ASC"; }
            DataSet ds = SPs.FetchCollectionReport(startdate, enddate,
                        PointOfSaleName, OutletName, SortColumn, SortDir).GetDataSet();

            return ds.Tables[0];
        }

        public static DataTable FetchProductSumarizedSalesReport(
            DateTime startdate,
            DateTime enddate,
            string PointOfSaleName,
            string OutletName,
            string Search,
            string SumarizeField,
            bool IsVoided,
            string SortColumn,
            string SortDir)
        {
            if (Search == "") { Search = "%"; }
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortColumn == "") { SortColumn = "ItemNo"; }
            if (SortDir == "") { SortDir = "ASC"; }
            if (SumarizeField == "") { SumarizeField = "ItemNo"; }

            string SQL =
                "declare @Search varchar(50); " +
                "declare @startdate datetime; " +
                "declare @enddate datetime; " +
                "declare @PointOfSaleName varchar(50); " +
                "declare @OutletName varchar(50); " +
                "declare @DeptID varchar(50); " +
                "declare @IsVoided bit; " +
                "declare @sortby varchar(50); " +
                "declare @sortdir varchar(5); " +
                "set @Search = '" + Search + "'; " +
                "set @startdate = '" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                "set @enddate = '" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                "set @PointOfSaleName = '" + PointOfSaleName + "'; " +
                "set @OutletName = '" + OutletName + "'; " +
                "set @DeptID = ''; " +
                "set @IsVoided = 0; " +
                "set @sortby = '" + SortColumn + "'; " +
                "set @sortdir = '" + SortDir + "'; " +
                "set @sortby = LTRIM(RTRIM(@sortby)); " +
                "set @sortdir = LTRIM(RTRIM(@sortdir)); " +
                "set @OutletName = LTRIM(RTRIM(@OutletName)); " +
                "SELECT CASE WHEN (CategoryName = 'SYSTEM') THEN 'SYSTEM' ELSE ISNULL(" + SumarizeField + ",'') END AS RefID " +
                    ", CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity " +
                    ", SUM(OrderDet.Amount) AS TotalAmount " +
                    ", ISNULL(SUM(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold " +
                    ", ISNULL((SUM(OrderDet.Amount) - SUM(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss " +
                    ", ISNULL((SUM(OrderDet.Amount) - SUM(CostofGoods * ViewInventorySalesStockOut.Quantity))/(SUM(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage " +
                    ", ISNULL(SUM(OrderDet.GSTAmount),0) as GSTAmount " +
                    ", OrderDet.IsVoided " +
                    ", CASE WHEN (@OutletName = 'ALL' AND @PointOfSaleName = 'ALL') THEN 'ALL' ELSE Outlet.OutletName END as OutletName " +
                    ", CASE WHEN (@PointOfSaleName = 'ALL') THEN 'ALL' ELSE PointOfSale.PointOfSaleName END as PointOfSaleName " +
                "FROM Item " +
                    "INNER JOIN OrderDet ON Item.ItemNo = OrderDet.ItemNo " +
                    "INNER JOIN OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID " +
                    "INNER JOIN PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID " +
                    "INNER JOIN Outlet ON PointOfSale.OutletName = Outlet.OutletName " +
                    "LEFT OUTER jOIN ViewInventorySalesStockOut ON " +
                        "(ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
                        "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
                "WHERE (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)   " +
                    "AND ISNULL(" + SumarizeField + ",'') LIKE '%' + @Search + '%' " +
                    "AND OrderDet.IsVoided = @IsVoided " +
                    "AND OrderHdr.IsVoided = 0 " +
                "GROUP BY CASE WHEN (CategoryName = 'SYSTEM') THEN 'SYSTEM' ELSE ISNULL(" + SumarizeField + ",'') END " +
                    ", OrderDet.IsVoided " +
                    ", CASE WHEN (@OutletName = 'ALL' AND @PointOfSaleName = 'ALL') THEN 'ALL' ELSE Outlet.OutletName END " +
                    ", CASE WHEN (@PointOfSaleName = 'ALL') THEN 'ALL' ELSE PointOfSale.PointOfSaleName END " +
                "ORDER BY " +
                "CASE     " +
                    "WHEN @sortby = 'RefID' and @sortdir = 'DESC'  " +
                    "THEN rank() over (order by CASE WHEN (CategoryName = 'SYSTEM') THEN 'SYSTEM' ELSE ISNULL(" + SumarizeField + ",'') END desc) " +
                    "WHEN @sortby = 'RefID' and @sortdir = 'ASC'  " +
                    "THEN rank() over (order by CASE WHEN (CategoryName = 'SYSTEM') THEN 'SYSTEM' ELSE ISNULL(" + SumarizeField + ",'') END Asc) " +
                    "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
                    "THEN rank() over (order by OrderDet.IsVoided desc) " +
                    "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
                    "THEN rank() over (order by OrderDet.IsVoided asc) " +
                    "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
                    "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
                    "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
                    "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
                    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
                    "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
                    "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
                    "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
                    "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
                    "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
                    "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
                    "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
                    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
                    "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
                    "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
                    "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
                    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
                    "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
                    "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
                    "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
                    "ELSE rank() over (order by CASE WHEN (CategoryName = 'SYSTEM') THEN 'SYSTEM' ELSE ISNULL(" + SumarizeField + ",'') END asc) " +
                "END ";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            DataSet ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }


        public static DataTable FetchProductCategorySalesReport(
         DateTime startdate,
         DateTime enddate,
         string PointOfSaleName,
         string OutletName,
         string CategoryName,
         string DeptID,
         bool IsVoided,
         string SortColumn,
         string SortDir)
        {
            if (CategoryName == "") { CategoryName = "%"; }
            if (CategoryName.IndexOf("'") > 0)
            {
                CategoryName = CategoryName.Insert(CategoryName.IndexOf("'"), "'");
            }
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortColumn == "") { SortColumn = "ItemNo"; }
            if (SortDir == "") { SortDir = "ASC"; }
            if (DeptID == "0") { DeptID = ""; }

            string SQL = "declare @categoryname varchar(50); " +
            "declare @startdate datetime; " +
            "declare @enddate datetime; " +
            "declare @PointOfSaleName varchar(50); " +
            "declare @OutletName varchar(50); " +
            "declare @DeptID varchar(50); " +
            "declare @IsVoided bit; " +
            "declare @sortby varchar(50); " +
            "declare @sortdir varchar(5); " +
            "set @categoryname = '%" + CategoryName + "%'; " +
            "set @startdate = '" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
            "set @enddate = '" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
            "set @PointOfSaleName = '" + PointOfSaleName + "'; " +
            "set @OutletName = '" + OutletName + "'; " +
            "set @DeptID = '" + DeptID + "'; " +
            "set @IsVoided = 0; " +
            "set @sortby = '" + SortColumn + "'; " +
            "set @sortdir = '" + SortDir + "'; " +
            "set @sortby = LTRIM(RTRIM(@sortby)); " +
            "set @sortdir = LTRIM(RTRIM(@sortdir)); " +
            "set @OutletName = LTRIM(RTRIM(@OutletName)); " +
            "if (@OutletName = 'ALL' AND @PointOfSaleName = 'ALL')  " +
            "Begin " +
            "SELECT ItemDepartment.DepartmentName,Item.CategoryName, CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,  " +
            "SUM(OrderDet.Amount) AS TotalAmount,  " +
            "isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            "isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            "isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, " +
            "isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, " +
            "OrderDet.IsVoided, @OutletName as OutletName, @PointOfSaleName as PointOfSaleName   " +
            "FROM  ItemDepartment INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID INNER JOIN Item on Item.CategoryName = Category.CategoryName INNER JOIN " +
            " OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            " OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            " PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  INNER JOIN " +
            "  Outlet ON PointOfSale.OutletName = Outlet.OutletName " +
            " INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            " LEFT outer jOIN " +
            "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)   " +
            "  AND Item.CategoryName Like @CategoryName " +
            "  AND OrderDet.IsVoided = @IsVoided " +
            "  AND OrderHdr.IsVoided = 0 " +
            "  AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            "GROUP BY ItemDepartment.DepartmentName,Item.CategoryName, " +
            "OrderDet.IsVoided " +
            "ORDER BY " +
            "CASE     " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName Asc) " +
            "WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Item.CategoryName desc) " +
            "WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Item.CategoryName asc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by OrderDet.IsVoided desc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by OrderDet.IsVoided asc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
            "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
            "ELSE rank() over (order by Item.CategoryName asc) " +
            "END " +
            "End " +
            "Else if (@PointOfSaleName = 'ALL' and @OutletName != 'ALL')   " +
            "Begin " +
            "SELECT ItemDepartment.DepartmentName,Item.CategoryName, CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,  " +
            "  SUM(OrderDet.Amount) AS TotalAmount,  " +
            "isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            "isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            "isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, " +
            "isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, " +
            "  OrderDet.IsVoided, 'ALL' as PointOfSaleName, Outlet.OutletName   " +
            "FROM  ItemDepartment INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID INNER JOIN Item on Item.CategoryName = Category.CategoryName INNER JOIN " +
            " OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            " OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            " PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
            "  Outlet ON PointOfSale.OutletName = Outlet.OutletName " +
            " INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            " LEFT outer jOIN " +
            "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)   " +
            "  AND Item.CategoryName Like @CategoryName " +
            "  AND OrderDet.IsVoided = @IsVoided " +
            "  AND Outlet.OutletName Like @OutletName " +
            "               AND OrderHdr.IsVoided = 0 " +
            "  AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            "GROUP BY ItemDepartment.DepartmentName,Item.CategoryName, " +
            "Outlet.OutletName, OrderDet.IsVoided " +
            "ORDER BY " +
            "CASE     " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName Asc) " +
            "WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Item.CategoryName desc) " +
            "WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Item.CategoryName asc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Outlet.OutletName desc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Outlet.OutletName asc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by OrderDet.IsVoided desc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by OrderDet.IsVoided asc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
            "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
            "ELSE rank() over (order by Item.CategoryName asc) " +
            "END " +
            "End " +
            "Else if (@PointOfSaleName != 'ALL' and @OutletName = 'ALL')   " +
            "begin " +
            "SELECT ItemDepartment.DepartmentName,Item.CategoryName, CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,  " +
            "  SUM(OrderDet.Amount) AS TotalAmount,  " +
            "isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            "isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            "isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, " +
            "isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, " +
            "  OrderDet.IsVoided, PointOfSale.PointOfSaleName, Outlet.OutletName   " +
            "FROM  ItemDepartment INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID INNER JOIN Item on Item.CategoryName = Category.CategoryName INNER JOIN " +
            " OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            " OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            " PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
            "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
            " INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            " LEFT outer jOIN " +
            "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
            "  AND PointOfSale.PointOfSaleName Like @PointOfSaleName " +
            "  AND Item.CategoryName Like @CategoryName " +
            "  AND OrderDet.IsVoided = @IsVoided " +
            "  AND OrderHdr.IsVoided = 0 " +
            "  AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            "GROUP BY ItemDepartment.DepartmentName,Item.CategoryName, " +
            "PointOfSale.PointOfSaleName, Outlet.OutletName, OrderDet.IsVoided " +
            "ORDER BY " +
            "CASE     " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName Asc) " +
            "WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Item.CategoryName desc) " +
            "WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Item.CategoryName asc) " +
            "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by PointOfSale.PointOfSaleName desc) " +
            "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by PointOfSale.PointOfSaleName asc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Outlet.OutletName desc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Outlet.OutletName asc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by OrderDet.IsVoided desc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by OrderDet.IsVoided asc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
            "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
            "ELSE rank() over (order by Item.CategoryName asc) " +
            "END " +
            "end " +
            "Else " +
            "Begin        " +
            "SELECT ItemDepartment.DepartmentName,Item.CategoryName, CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,  " +
            "  SUM(OrderDet.Amount) AS TotalAmount,  " +
            "isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            "isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            "isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, " +
            "isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, " +
            "  OrderDet.IsVoided, PointOfSale.PointOfSaleName, Outlet.OutletName   " +
            "FROM  ItemDepartment INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID INNER JOIN Item on Item.CategoryName = Category.CategoryName INNER JOIN " +
            " OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            " OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            " PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  INNER JOIN " +
            "  Outlet ON PointOfSale.OutletName = Outlet.OutletName " +
            " INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            " LEFT outer jOIN " +
            "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
            "  AND PointOfSale.PointOfSaleName Like @PointOfSaleName " +
            "  AND Outlet.OutletName Like @OutletName " +
            "  AND Item.CategoryName Like @CategoryName " +
            "  AND OrderDet.IsVoided = @IsVoided " +
            "  AND OrderHdr.IsVoided = 0 " +
            "  AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            "GROUP BY ItemDepartment.DepartmentName,Item.CategoryName, Outlet.OutletName, " +
            "PointOfSale.PointOfSaleName, OrderDet.IsVoided " +
            "ORDER BY " +
            "CASE     " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName Asc) " +
            "WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Item.CategoryName desc) " +
            "WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Item.CategoryName asc) " +
            "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by PointOfSale.PointOfSaleName desc) " +
            "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by PointOfSale.PointOfSaleName asc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Outlet.OutletName desc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Outlet.OutletName asc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by OrderDet.IsVoided desc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by OrderDet.IsVoided asc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
            "WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
            "WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
            "WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
            "ELSE rank() over (order by Item.CategoryName asc) " +
            "END " +
            "End ";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            DataSet ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }

        public static DataTable FetchProductCategorySalesReport(
         DateTime startdate,
         DateTime enddate,
         string PointOfSaleName,
         string OutletName,
         string[] CategoryName,
         string DeptID,
         bool IsVoided,
         string SortColumn,
         string SortDir)
        {
            string categoryFilter = "";

            if (CategoryName.Length > 0)
            {
                for (var i = 0; i < CategoryName.Length; i++)
                {
                    if (CategoryName[i].Trim() == "")
                        CategoryName[i] = null;
                    else
                        CategoryName[i] = "N'" + CategoryName[i].Trim().Replace("'", "''") + "'";
                }
                categoryFilter = string.Join(",", CategoryName).Trim(',');
            }

            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortColumn == "") { SortColumn = "ItemNo"; }
            if (SortDir == "") { SortDir = "ASC"; }
            if (DeptID == "0") { DeptID = ""; }

            string SQL = @"
            declare @startdate datetime; 
            declare @enddate datetime; 
            declare @PointOfSaleName varchar(50); 
            declare @OutletName varchar(50); 
            declare @DeptID varchar(50); 
            declare @IsVoided bit; 
            declare @sortby varchar(50); 
            declare @sortdir varchar(5); 
            set @startdate = '" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + @"'; 
            set @enddate = '" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + @"'; 
            set @PointOfSaleName = '" + PointOfSaleName + @"'; 
            set @OutletName = '" + OutletName + @"'; 
            set @DeptID = '" + DeptID + @"'; 
            set @IsVoided = 0; 
            set @sortby = '" + SortColumn + @"'; 
            set @sortdir = '" + SortDir + @"'; 
            set @sortby = LTRIM(RTRIM(@sortby)); 
            set @sortdir = LTRIM(RTRIM(@sortdir)); 
            set @OutletName = LTRIM(RTRIM(@OutletName)); 
            
            if (@OutletName = 'ALL' AND @PointOfSaleName = 'ALL')  
            Begin 
                SELECT ItemDepartment.DepartmentName,
                    Item.CategoryName, 
                    CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,
                    SUM(OrderDet.Amount) AS TotalAmount,
                    isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, 
                    isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, 
                    isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, 
                    isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, 
                    OrderDet.IsVoided, 
                    @OutletName as OutletName, 
                    @PointOfSaleName as PointOfSaleName   
                FROM  ItemDepartment 
                    INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID 
                    INNER JOIN Item on Item.CategoryName = Category.CategoryName 
                    INNER JOIN  OrderDet ON Item.ItemNo = OrderDet.ItemNo 
                    INNER JOIN  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID 
                    INNER JOIN  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  
                    INNER JOIN   Outlet ON PointOfSale.OutletName = Outlet.OutletName  
                    INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID  
                    LEFT outer jOIN ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) 
                WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)     
                    {0} 
                    AND OrderDet.IsVoided = @IsVoided   
                    AND OrderHdr.IsVoided = 0   
                    AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' 
                GROUP BY ItemDepartment.DepartmentName,Item.CategoryName, OrderDet.IsVoided 
                ORDER BY CASE     
                    WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  THEN rank() over (order by ItemDepartment.DepartmentName desc) 
                    WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  THEN rank() over (order by ItemDepartment.DepartmentName Asc) 
                    WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  THEN rank() over (order by Item.CategoryName desc) 
                    WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  THEN rank() over (order by Item.CategoryName asc) 
                    WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  THEN rank() over (order by OrderDet.IsVoided desc) 
                    WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  THEN rank() over (order by OrderDet.IsVoided asc) 
                    WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  THEN rank() over (order by sum(OrderDet.Amount) desc) 
                    WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  THEN rank() over (order by sum(OrderDet.Amount) asc) 
                    WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  THEN rank() over (order by sum(OrderDet.Quantity) desc) 
                    WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  THEN rank() over (order by sum(OrderDet.Quantity) asc) 
                    WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) 
                    WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) 
                    WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) 
                    WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) 
                    WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) 
                    WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) 
                    ELSE rank() over (order by Item.CategoryName asc) 
                END 
            End 
            Else if (@PointOfSaleName = 'ALL' and @OutletName != 'ALL')   
            Begin   
                SELECT ItemDepartment.DepartmentName,
                    Item.CategoryName, 
                    CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,    
                    SUM(OrderDet.Amount) AS TotalAmount,  
                    isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, 
                    isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, 
                    isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, 
                    isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount,   
                    OrderDet.IsVoided, 
                    'ALL' as PointOfSaleName, 
                    Outlet.OutletName   
                FROM  ItemDepartment 
                    INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID 
                    INNER JOIN Item on Item.CategoryName = Category.CategoryName 
                    INNER JOIN  OrderDet ON Item.ItemNo = OrderDet.ItemNo 
                    INNER JOIN  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID 
                    INNER JOIN  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID 
                    INNER JOIN   Outlet ON PointOfSale.OutletName = Outlet.OutletName  
                    INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID  
                    LEFT outer jOIN ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) 
                WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)     
                    {0} 
                    AND OrderDet.IsVoided = @IsVoided   
                    AND Outlet.OutletName Like @OutletName                
                    AND OrderHdr.IsVoided = 0   
                    AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' 
                GROUP BY ItemDepartment.DepartmentName,Item.CategoryName, Outlet.OutletName, OrderDet.IsVoided 
                ORDER BY CASE    
                    WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  THEN rank() over (order by ItemDepartment.DepartmentName desc) 
                    WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  THEN rank() over (order by ItemDepartment.DepartmentName Asc) 
                    WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  THEN rank() over (order by Item.CategoryName desc) 
                    WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  THEN rank() over (order by Item.CategoryName asc) 
                    WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  THEN rank() over (order by Outlet.OutletName desc) 
                    WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  THEN rank() over (order by Outlet.OutletName asc)
                    WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  THEN rank() over (order by OrderDet.IsVoided desc) 
                    WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  THEN rank() over (order by OrderDet.IsVoided asc) 
                    WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  THEN rank() over (order by sum(OrderDet.Amount) desc) 
                    WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  THEN rank() over (order by sum(OrderDet.Amount) asc) 
                    WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  THEN rank() over (order by sum(OrderDet.Quantity) desc) 
                    WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  THEN rank() over (order by sum(OrderDet.Quantity) asc) 
                    WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) 
                    WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) 
                    WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) 
                    WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) 
                    WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) 
                    WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) 
                    ELSE rank() over (order by Item.CategoryName asc) 
                END 
            End 
            Else if (@PointOfSaleName != 'ALL' and @OutletName = 'ALL')   
            begin 
                SELECT ItemDepartment.DepartmentName,
                    Item.CategoryName, 
                    CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,    
                    SUM(OrderDet.Amount) AS TotalAmount,  
                    isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, 
                    isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, 
                    isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, 
                    isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount,   
                    OrderDet.IsVoided, 
                    PointOfSale.PointOfSaleName, 
                    Outlet.OutletName   
                FROM  ItemDepartment 
                    INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID 
                    INNER JOIN Item on Item.CategoryName = Category.CategoryName 
                    INNER JOIN  OrderDet ON Item.ItemNo = OrderDet.ItemNo 
                    INNER JOIN  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID 
                    INNER JOIN  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID 
                    INNER JOIN   Outlet ON PointOfSale.OutletName = Outlet.OutletName   
                    INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID  
                    LEFT outer jOIN ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) 
                WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)   
                    AND PointOfSale.PointOfSaleName Like @PointOfSaleName   
                    {0} 
                    AND OrderDet.IsVoided = @IsVoided   
                    AND OrderHdr.IsVoided = 0   
                    AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' 
                GROUP BY ItemDepartment.DepartmentName,Item.CategoryName, PointOfSale.PointOfSaleName, Outlet.OutletName, OrderDet.IsVoided 
                ORDER BY CASE     
                    WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  THEN rank() over (order by ItemDepartment.DepartmentName desc) 
                    WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  THEN rank() over (order by ItemDepartment.DepartmentName Asc) 
                    WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  THEN rank() over (order by Item.CategoryName desc) 
                    WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  THEN rank() over (order by Item.CategoryName asc) 
                    WHEN @sortby = 'PointOfSaleName' and @sortdir = 'DESC'  THEN rank() over (order by PointOfSale.PointOfSaleName desc)
                    WHEN @sortby = 'PointOfSaleName' and @sortdir = 'ASC'  THEN rank() over (order by PointOfSale.PointOfSaleName asc) 
                    WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  THEN rank() over (order by Outlet.OutletName desc) 
                    WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  THEN rank() over (order by Outlet.OutletName asc) 
                    WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  THEN rank() over (order by OrderDet.IsVoided desc) 
                    WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  THEN rank() over (order by OrderDet.IsVoided asc) 
                    WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  THEN rank() over (order by sum(OrderDet.Amount) desc) 
                    WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  THEN rank() over (order by sum(OrderDet.Amount) asc) 
                    WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  THEN rank() over (order by sum(OrderDet.Quantity) desc) 
                    WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  THEN rank() over (order by sum(OrderDet.Quantity) asc) 
                    WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) 
                    WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) 
                    WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) 
                    WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) 
                    WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) 
                    WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) 
                    ELSE rank() over (order by Item.CategoryName asc) 
                END 
            end 
            Else 
            Begin        
                SELECT ItemDepartment.DepartmentName,
                    Item.CategoryName, 
                    CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,    
                    SUM(OrderDet.Amount) AS TotalAmount,  
                    isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, 
                    isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, 
                    isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, 
                    isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount,   
                    OrderDet.IsVoided, 
                    PointOfSale.PointOfSaleName, 
                    Outlet.OutletName   
                FROM  ItemDepartment 
                    INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID 
                    INNER JOIN Item on Item.CategoryName = Category.CategoryName 
                    INNER JOIN  OrderDet ON Item.ItemNo = OrderDet.ItemNo 
                    INNER JOIN  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID 
                    INNER JOIN  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  
                    INNER JOIN   Outlet ON PointOfSale.OutletName = Outlet.OutletName  
                    INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID  
                    LEFT outer jOIN ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) 
                WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)   
                    AND PointOfSale.PointOfSaleName Like @PointOfSaleName   
                    AND Outlet.OutletName Like @OutletName   
                    {0} 
                    AND OrderDet.IsVoided = @IsVoided   
                    AND OrderHdr.IsVoided = 0   
                    AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' 
                GROUP BY ItemDepartment.DepartmentName,Item.CategoryName, Outlet.OutletName, PointOfSale.PointOfSaleName, OrderDet.IsVoided 
                ORDER BY CASE     
                    WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  THEN rank() over (order by ItemDepartment.DepartmentName desc) 
                    WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  THEN rank() over (order by ItemDepartment.DepartmentName Asc) 
                    WHEN @sortby = 'CategoryName' and @sortdir = 'DESC'  THEN rank() over (order by Item.CategoryName desc) 
                    WHEN @sortby = 'CategoryName' and @sortdir = 'ASC'  THEN rank() over (order by Item.CategoryName asc) 
                    WHEN @sortby = 'PointOfSaleName' and @sortdir = 'DESC'  THEN rank() over (order by PointOfSale.PointOfSaleName desc) 
                    WHEN @sortby = 'PointOfSaleName' and @sortdir = 'ASC'  THEN rank() over (order by PointOfSale.PointOfSaleName asc) 
                    WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  THEN rank() over (order by Outlet.OutletName desc) 
                    WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  THEN rank() over (order by Outlet.OutletName asc) 
                    WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  THEN rank() over (order by OrderDet.IsVoided desc) 
                    WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  THEN rank() over (order by OrderDet.IsVoided asc) 
                    WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  THEN rank() over (order by sum(OrderDet.Amount) desc) 
                    WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  THEN rank() over (order by sum(OrderDet.Amount) asc) 
                    WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  THEN rank() over (order by sum(OrderDet.Quantity) desc) 
                    WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  THEN rank() over (order by sum(OrderDet.Quantity) asc) 
                    WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) 
                    WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) 
                    WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) 
                    WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) 
                    WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) 
                    WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) 
                    ELSE rank() over (order by Item.CategoryName asc) 
                END 
            End 
            ";

            SQL = string.Format(SQL,
                categoryFilter == "" ? "" : "AND Item.CategoryName IN (" + categoryFilter + ") ");
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            DataSet ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }

        public static DataTable FetchProductCategorySalesReportMultipleCategory(
         DateTime startdate,
         DateTime enddate,
         string PointOfSaleName,
         string OutletName,
         string CategoryName,
         string DeptID,
         bool IsVoided,
         string SortColumn,
         string SortDir)
        {
            DataTable dt = new DataTable();
            try
            {
                if (CategoryName == "") { CategoryName = "''"; }
                if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
                if (OutletName == "") { OutletName = "ALL"; }
                if (SortDir == "") { SortDir = "ASC"; }
                //if (SortColumn == "") { SortColumn = "CategoryName"; }
                if (DeptID == "" || DeptID == "0") { DeptID = "ALL"; }

                string sortExp = "";
                if (SortColumn == "")
                    sortExp = "OutletName, TotalAmount DESC";
                else
                    sortExp = string.Format("{0} {1}", SortColumn, SortDir);

                string sql = @"DECLARE @CategoryName VARCHAR(50); 
                            DECLARE @StartDate DATETIME; 
                            DECLARE @EndDate DATETIME; 
                            DECLARE @PointOfSaleName VARCHAR(50); 
                            DECLARE @OutletName VARCHAR(50); 
                            DECLARE @DeptID VARCHAR(50); 
                            DECLARE @IsVoided BIT; 
                            SET @StartDate = '{0}'; 
                            SET @EndDate = '{1}'; 
                            SET @PointOfSaleName = '{2}'; 
                            SET @OutletName = '{3}'; 
                            SET @DeptID = '{4}'; 
                            SET @IsVoided = 0; 
                            SET @OutletName = LTRIM(RTRIM(@OutletName)); 

                            SELECT   ID.DepartmentName
		                            ,C.CategoryName
		                            ,CAST(SUM(OD.Quantity) as decimal(18,2)) TotalQuantity
		                            ,CAST(SUM(OD.Amount) as decimal(18,2)) TotalAmount
		                            ,SUM(OD.CostOfGoodSold) TotalCostOfGoodsSold
		                            ,SUM(OD.Amount)-SUM(OD.CostOfGoodSold) ProfitLoss
		                            ,CASE WHEN SUM(OD.Amount) = 0 THEN 0
										  ELSE (SUM(OD.Amount)-SUM(OD.CostOfGoodSold)) / SUM(OD.Amount) END ProfitLossPercentage
                                    ,CASE WHEN @PointOfSaleName = 'ALL' THEN 'ALL' ELSE POS.PointOfSaleName END PointOfSaleName
                                    ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE POS.OutletName END OutletName        
                            FROM	OrderHdr OH
		                            LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
		                            LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
		                            LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
		                            LEFT JOIN Item I ON I.ItemNo = OD.ItemNo
		                            LEFT JOIN Category C ON C.CategoryName = I.CategoryName
		                            LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                            WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
		                            AND OH.OrderDate BETWEEN @StartDate AND @EndDate
		                            AND (POS.PointOfSaleName = @PointOfSaleName OR @PointOfSaleName = 'ALL' OR @PointOfSaleName = 'ALL - BreakDown')
		                            AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
		                            AND (ID.ItemDepartmentID = @DeptID OR @DeptID = 'ALL')
                                    AND C.CategoryName IN ( {5} )
                            GROUP BY ID.DepartmentName
		                            ,C.CategoryName
		                            ,CASE WHEN @PointOfSaleName = 'ALL' THEN 'ALL' ELSE POS.PointOfSaleName END 
                                    ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' ELSE POS.OutletName END   
                            ORDER BY {6}";
                sql = string.Format(sql, startdate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , enddate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , PointOfSaleName
                                       , OutletName
                                       , DeptID
                                       , CategoryName
                                       , sortExp);

                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchProductCategorySalesReportWithCashOut(
         DateTime startdate,
         DateTime enddate,
         string PointOfSaleName,
         string OutletName,
         string CategoryName,
         string DeptID,
         bool IsVoided,
         string SortColumn,
         string SortDir)
        {
            if (CategoryName == "") { CategoryName = "%"; }
            if (CategoryName.IndexOf("'") > 0)
            {
                CategoryName = CategoryName.Insert(CategoryName.IndexOf("'"), "'");
            }
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortColumn == "") { SortColumn = "ItemNo"; }
            if (SortDir == "") { SortDir = "ASC"; }
            if (DeptID == "0") { DeptID = ""; }

            DataSet ds = SPs.FetchProductCategorySalesReportWithCashOut(CategoryName, startdate, enddate, PointOfSaleName, OutletName, DeptID, IsVoided, SortColumn, SortDir).GetDataSet();

            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }

        public static DataTable FetchCashOutReport(
         DateTime startdate,
         DateTime enddate,
         string PointOfSaleName,
         string OutletName,
         string DeptID)
        {
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (DeptID == "0") { DeptID = ""; }

            DataSet ds = SPs.FetchCashOutReport(startdate, enddate, PointOfSaleName, OutletName, DeptID).GetDataSet();

            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }

        public static DataTable FetchDiscountReport(DateTime startDate, DateTime endDate,
            int posID)
        {
            DataTable dt = new DataTable();

            string query = @"
                SELECT   ISNULL(SUM((CASE WHEN OD.Discount >= 20 THEN (OD.OriginalRetailPrice-(OD.Amount/OD.Quantity)) ELSE 0 END)),0) AS DiscPercentage
		                ,ISNULL(SUM((CASE WHEN OD.Discount < 20 THEN (OD.OriginalRetailPrice-(OD.Amount/OD.Quantity)) ELSE 0 END)),0) AS DiscPrice
                FROM	OrderHdr OH
		                INNER JOIN OrderDet OD ON OH.OrderHdrID  = OD.OrderHdrID
                WHERE	OH.IsVoided <> 1
                        AND OD.Quantity <> 0 
                        AND (OD.OriginalRetailPrice-(OD.Amount/OD.Quantity)) > 0 ";

            query += string.Format(" AND OH.OrderDate BETWEEN '{0}' AND '{1}' ", startDate.ToString("yyyy-MM-dd HH:mm:ss"), endDate.ToString("yyyy-MM-dd HH:mm:ss"));
            query += string.Format(" AND OH.PointOfSaleID = {0} ", posID);

            QueryCommand cmd = new QueryCommand(query, "PowerPOS");
            DataSet ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        public static DataTable FetchDepartmentSalesReport(
                 DateTime startdate,
                 DateTime enddate,
                 string PointOfSaleName,
                 string OutletName,
                 string DeptID,
                 bool IsVoided,
                 string SortColumn,
                 string SortDir)
        {
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortColumn == "") { SortColumn = "DepartmentName"; }
            if (SortDir == "") { SortDir = "ASC"; }
            if (DeptID == "0") { DeptID = ""; }

            string SQL = "declare @startdate datetime; " +
            "declare @enddate datetime; " +
            "declare @PointOfSaleName varchar(50); " +
            "declare @OutletName varchar(50); " +
            "declare @DeptID varchar(50); " +
            "declare @IsVoided bit; " +
            "declare @sortby varchar(50); " +
            "declare @sortdir varchar(5); " +
            "set @startdate = '" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
            "set @enddate = '" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
            "set @PointOfSaleName = '" + PointOfSaleName + "'; " +
            "set @OutletName = '" + OutletName + "'; " +
            "set @DeptID = '" + DeptID + "'; " +
            "set @IsVoided = 0; " +
            "set @sortby = '" + SortColumn + "'; " +
            "set @sortdir = '" + SortDir + "'; " +
            "set @sortby = LTRIM(RTRIM(@sortby)); " +
            "set @sortdir = LTRIM(RTRIM(@sortdir)); " +
            "set @OutletName = LTRIM(RTRIM(@OutletName)); " +
            "if (@OutletName = 'ALL' AND @PointOfSaleName = 'ALL')  " +
            "Begin " +
            "SELECT ItemDepartment.DepartmentName,CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,  " +
            "SUM(OrderDet.Amount) AS TotalAmount,  " +
            //"isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            //"isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            //"isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, " +
            "isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, " +
            "OrderDet.IsVoided, @OutletName as OutletName, @PointOfSaleName as PointOfSaleName   " +
            "FROM  ItemDepartment INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID INNER JOIN Item on Item.CategoryName = Category.CategoryName INNER JOIN " +
            " OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            " OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            " PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  INNER JOIN " +
            "  Outlet ON PointOfSale.OutletName = Outlet.OutletName " +
            " INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            //" LEFT outer jOIN " +
            //"ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            //"AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)   " +
            "  AND OrderDet.IsVoided = @IsVoided " +
            "  AND OrderHdr.IsVoided = 0 " +
            "  AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            "GROUP BY ItemDepartment.DepartmentName, " +
            "OrderDet.IsVoided " +
            "ORDER BY " +
            "CASE     " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName Asc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by OrderDet.IsVoided desc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by OrderDet.IsVoided asc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            //"WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
            //"WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
            //"WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
            //"WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
            //"WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
            //"WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
            "ELSE rank() over (order by ItemDepartment.DepartmentName asc) " +
            "END " +
            "End " +
            "Else if (@PointOfSaleName = 'ALL' and @OutletName != 'ALL')   " +
            "Begin " +
            "SELECT ItemDepartment.DepartmentName, CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,  " +
            "  SUM(OrderDet.Amount) AS TotalAmount,  " +
            //"isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            //"isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            //"isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, " +
            "isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, " +
            "  OrderDet.IsVoided, 'ALL' as PointOfSaleName, Outlet.OutletName   " +
            "FROM  ItemDepartment INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID INNER JOIN Item on Item.CategoryName = Category.CategoryName INNER JOIN " +
            " OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            " OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            " PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
            "  Outlet ON PointOfSale.OutletName = Outlet.OutletName " +
            " INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            //" LEFT outer jOIN " +
            //"ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            //"AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)   " +
            "  AND OrderDet.IsVoided = @IsVoided " +
            "  AND Outlet.OutletName Like @OutletName " +
            "               AND OrderHdr.IsVoided = 0 " +
            "  AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            "GROUP BY ItemDepartment.DepartmentName, " +
            "Outlet.OutletName, OrderDet.IsVoided " +
            "ORDER BY " +
            "CASE     " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName Asc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Outlet.OutletName desc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Outlet.OutletName asc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by OrderDet.IsVoided desc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by OrderDet.IsVoided asc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            //"WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
            //"WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
            //"WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
            //"WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
            //"WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
            //"WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
            "ELSE rank() over (order by ItemDepartment.DepartmentName asc) " +
            "END " +
            "End " +
            "Else if (@PointOfSaleName != 'ALL' and @OutletName = 'ALL')   " +
            "begin " +
            "SELECT ItemDepartment.DepartmentName, CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,  " +
            "  SUM(OrderDet.Amount) AS TotalAmount,  " +
            //"isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            //"isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            //"isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, " +
            "isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, " +
            "  OrderDet.IsVoided, PointOfSale.PointOfSaleName, Outlet.OutletName   " +
            "FROM  ItemDepartment INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID INNER JOIN Item on Item.CategoryName = Category.CategoryName INNER JOIN " +
            " OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            " OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            " PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
            "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
            " INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            //" LEFT outer jOIN " +
            //"ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            //"AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
            "  AND PointOfSale.PointOfSaleName Like @PointOfSaleName " +
            "  AND OrderDet.IsVoided = @IsVoided " +
            "  AND OrderHdr.IsVoided = 0 " +
            "  AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            "GROUP BY ItemDepartment.DepartmentName, " +
            "PointOfSale.PointOfSaleName, Outlet.OutletName, OrderDet.IsVoided " +
            "ORDER BY " +
            "CASE     " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName Asc) " +
            "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by PointOfSale.PointOfSaleName desc) " +
            "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by PointOfSale.PointOfSaleName asc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Outlet.OutletName desc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Outlet.OutletName asc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by OrderDet.IsVoided desc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by OrderDet.IsVoided asc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            //"WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
            //"WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
            //"WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
            //"WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
            //"WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
            //"WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
            "ELSE rank() over (order by ItemDepartment.DepartmentName asc) " +
            "END " +
            "end " +
            "Else " +
            "Begin        " +
            "SELECT ItemDepartment.DepartmentName, CAST(SUM(OrderDet.Quantity) as decimal(18,2)) AS TotalQuantity,  " +
            "  SUM(OrderDet.Amount) AS TotalAmount,  " +
            //"isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " +
            //"isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
            //"isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage, " +
            "isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount, " +
            "  OrderDet.IsVoided, PointOfSale.PointOfSaleName, Outlet.OutletName   " +
            "FROM  ItemDepartment INNER JOIN Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID INNER JOIN Item on Item.CategoryName = Category.CategoryName INNER JOIN " +
            " OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
            " OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
            " PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  INNER JOIN " +
            "  Outlet ON PointOfSale.OutletName = Outlet.OutletName " +
            " INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
            //" LEFT outer jOIN " +
            //"ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
            //"AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
            "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
            "  AND PointOfSale.PointOfSaleName Like @PointOfSaleName " +
            "  AND Outlet.OutletName Like @OutletName " +
            "  AND OrderDet.IsVoided = @IsVoided " +
            "  AND OrderHdr.IsVoided = 0 " +
            "  AND ItemDepartment.DepartmentName like '%' + @DeptID + '%' " +
            "GROUP BY ItemDepartment.DepartmentName, Outlet.OutletName, " +
            "PointOfSale.PointOfSaleName, OrderDet.IsVoided " +
            "ORDER BY " +
            "CASE     " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName desc) " +
            "WHEN @sortby = 'DepartmentName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by ItemDepartment.DepartmentName Asc) " +
            "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by PointOfSale.PointOfSaleName desc) " +
            "WHEN @sortby = 'PointOfSaleName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by PointOfSale.PointOfSaleName asc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by Outlet.OutletName desc) " +
            "WHEN @sortby = 'OutletName' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by Outlet.OutletName asc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by OrderDet.IsVoided desc) " +
            "WHEN @sortby = 'IsVoided' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by OrderDet.IsVoided asc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) desc) " +
            "WHEN @sortby = 'TotalAmount' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Amount) asc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'DESC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) desc) " +
            "WHEN @sortby = 'TotalQuantity' and @sortdir = 'ASC'  " +
            "THEN rank() over (order by sum(OrderDet.Quantity) asc) " +
            //"WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) desc) " +
            //"WHEN @sortby = 'TotalCostOfGoodsSold' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) asc) " +
            //"WHEN @sortby = 'ProfitLoss' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) desc) " +
            //"WHEN @sortby = 'ProfitLoss' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) asc) " +
            //"WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'DESC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) desc) " +
            //"WHEN @sortby = 'ProfitLossPercentage' and @sortdir = 'ASC'  " +
            //"THEN rank() over (order by isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) asc) " +
            "ELSE rank() over (order by ItemDepartment.DepartmentName asc) " +
            "END " +
            "End ";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            DataSet ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }

        public static DataTable FetchRefundProductCategorySalesReport(
         DateTime startdate,
         DateTime enddate,
         string PointOfSaleName,
         string OutletName,
         string CategoryName,
         string DeptID,
         bool IsVoided,
         string SortColumn,
         string SortDir)
        {
            if (CategoryName == "") { CategoryName = "%"; }
            if (PointOfSaleName == "") { PointOfSaleName = "ALL"; }
            if (OutletName == "") { OutletName = "ALL"; }
            if (SortColumn == "") { SortColumn = "ItemNo"; }
            if (SortDir == "") { SortDir = "ASC"; }
            if (DeptID == "0") { DeptID = ""; }

            DataSet ds = SPs.FetchNegativeProductCategorySalesReport
                (CategoryName, startdate, enddate,
                 PointOfSaleName, OutletName, DeptID, IsVoided, SortColumn, SortDir).GetDataSet();

            return ds.Tables[0];
        }
        public static DataTable FetchSalesReport
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string RefNo, string Cashier, string myCategoryName,
            string myItemName)
        {
            ViewSalesDetailCollection mySales = new ViewSalesDetailCollection();
            if (RefNo != "")
            {
                mySales.Where(ViewSalesDetail.Columns.OrderRefNo, RefNo);
            }
            else
            {
                if (useStartDate & useEndDate)
                {
                    mySales.BetweenAnd(ViewSalesDetail.Columns.OrderDate, StartDate, EndDate);
                }
                else if (useStartDate)
                {
                    mySales.Where(ViewSalesDetail.Columns.OrderDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
                }
                else if (useEndDate)
                {
                    mySales.Where(ViewSalesDetail.Columns.OrderDate, SubSonic.Comparison.LessOrEquals, EndDate);
                }
                if (Cashier != "")
                {
                    mySales.Where(ViewSalesDetail.Columns.CashierID, SubSonic.Comparison.Equals, Cashier);
                }
                if (myCategoryName != "")
                {
                    mySales.Where(ViewSalesDetail.Columns.CategoryName, SubSonic.Comparison.Equals, myCategoryName);
                }
                if (myItemName != "")
                {
                    mySales.Where(ViewSalesDetail.Columns.ItemName, SubSonic.Comparison.Equals, myItemName);
                }
            }
            mySales.OrderByDesc(ViewSalesDetail.Columns.OrderDate);
            return mySales.Load().ToDataTable();
        }

        public static DataTable FetchCashRecordingReport(
            bool useStartDate,
            bool useEndDate,
            DateTime StartDate,
            DateTime EndDate,
            string RefNo,
            string myCashierName,
            string mySupervisorName,
            string myCashRecordingType,
            int PointOfSaleID,
            string PointOfSaleName,
            string outletName,
            string deptID,
            string SortColumn,
            string SortDir
            )
        {
            ViewCashRecordingCollection myCashRecordings = new ViewCashRecordingCollection();
            if (RefNo != "")
            {
                myCashRecordings.Where(ViewCashRecording.Columns.CashRecRefNo, RefNo);
            }

            if (useStartDate & useEndDate)
            {
                myCashRecordings.BetweenAnd(ViewCashRecording.Columns.CashRecordingTime, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myCashRecordings.Where(ViewCashRecording.Columns.CashRecordingTime, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myCashRecordings.Where(ViewCashRecording.Columns.CashRecordingTime, SubSonic.Comparison.LessOrEquals, EndDate);
            }
            if (myCashierName != "")
            {
                myCashRecordings.Where(ViewCashRecording.Columns.CashierName, SubSonic.Comparison.Like, myCashierName);
            }
            if (mySupervisorName != "")
            {
                myCashRecordings.Where(ViewCashRecording.Columns.SupervisorName, SubSonic.Comparison.Like, mySupervisorName);
            }
            if (myCashRecordingType != "")
            {
                myCashRecordings.Where(ViewCashRecording.Columns.CashRecordingTypeName, SubSonic.Comparison.Equals, myCashRecordingType);
            }

            if (outletName != "")
            {
                myCashRecordings.Where(ViewCashRecording.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                myCashRecordings.Where(ViewCashRecording.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    myCashRecordings.Where(ViewCashRecording.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }
            if (deptID != "0")
            {
                myCashRecordings.Where(ViewCashRecording.Columns.DepartmentId, int.Parse(deptID));
            }
            SubSonic.TableSchema.TableColumn t = ViewCashRecording.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myCashRecordings.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myCashRecordings.OrderByDesc(SortColumn);
                }

            }
            return myCashRecordings.Load().ToDataTable();
        }

        public static DataTable FetchPreviousMonthsCommissionReport(
            DateTime? monthYear,
            string SalesPersonName,
            string GroupName,
            string SortColumn,
            string SortDir
         )
        {
            SalesCommissionHistoryCollection myCommissions = new SalesCommissionHistoryCollection();


            if (monthYear.HasValue)
            {
                myCommissions.Where(SalesCommissionHistory.Columns.MonthYear, monthYear.Value);
            }

            if (SalesPersonName != "")
            {
                myCommissions.Where(SalesCommissionHistory.Columns.SalesPersonName, SubSonic.Comparison.Like, SalesPersonName);
            }

            if (GroupName != "")
            {
                myCommissions.Where(SalesCommissionHistory.Columns.GroupName, SubSonic.Comparison.Like, GroupName);
            }

            SubSonic.TableSchema.TableColumn t = SalesCommissionHistory.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myCommissions.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myCommissions.OrderByDesc(SortColumn);
                }

            }
            return myCommissions.Load().ToDataTable();
        }

        public static DataTable FetchCurrentMonthCommissionReport(
            string SalesPersonName,
            string GroupName,
            string SortColumn,
            string SortDir
            )
        {
            ViewSalesPersonCurrentMonthCommissionCollection myCommissions = new ViewSalesPersonCurrentMonthCommissionCollection();


            if (SalesPersonName != "")
            {
                myCommissions.Where(ViewSalesPersonCurrentMonthCommission.Columns.SalesPersonName, SubSonic.Comparison.Like, SalesPersonName);
            }

            if (GroupName != "")
            {
                myCommissions.Where(ViewSalesPersonCurrentMonthCommission.Columns.GroupName, SubSonic.Comparison.Like, GroupName);
            }

            SubSonic.TableSchema.TableColumn t = ViewSalesPersonCurrentMonthCommission.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myCommissions.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myCommissions.OrderByDesc(SortColumn);
                }

            }
            return myCommissions.Load().ToDataTable();
        }

        public static DataTable FetchSalesPersonSalesByDate(DateTime startdate, DateTime enddate, string outletname, string pointofsalename, string salespersonname)
        {
            if (outletname == "") outletname = "%";
            if (pointofsalename == "") pointofsalename = "%";
            if (salespersonname == "") salespersonname = "%";

            DataSet ds = SPs.FetchSalesPersonSalesByDate
                (startdate, enddate,
                outletname, pointofsalename, salespersonname)
                .GetDataSet();
            return ds.Tables[0];
        }

        public static DataTable FetchTransactoionReport(DateTime startDate, DateTime endDate, bool useStartDate, bool useEndDate,
            string refNo, string cashierID, int pointOfSaleID, string outletName, string paymentMode, string remarks, bool showVoidedTransaction)
        {
            DataTable dt = new DataTable();

            try
            {
                //DataSet ds = SPs.FetchTransactionReport(startDate, endDate, useStartDate, useEndDate, refNo, cashierID,
                //    pointOfSaleID, outletName, paymentMode, remarks, showVoidedTransaction).GetDataSet();
                //dt = ds.Tables[0];
                var newSql = @"EXEC FetchTransactionReport
		                               @StartDate = @StartDate_,
		                               @EndDate = @EndDate_,
                                       @UseStartDate = @UseStartDate_,
                                        @UseEndDate = @UseEndDate_,
                                       @RefNo = @RefNo_,
                                        @CashierID = @CashierID_,
                                        @PointOfSaleID = @PointOfSaleID_,
                                        @Outlet = @Outlet_,
                                        @PaymentType = @PaymentType_,
                                        @Remarks = @Remarks_,
                                        @ShowVoidedTransaction = @ShowVoidedTransaction_";
                var cmd = new QueryCommand(newSql, "PowerPOS");
                cmd.AddParameter("@StartDate_", startDate);
                cmd.AddParameter("@EndDate_", endDate);
                cmd.AddParameter("@UseStartDate_", useStartDate);
                cmd.AddParameter("@UseEndDate_", useEndDate);
                cmd.AddParameter("@RefNo_", refNo);
                cmd.AddParameter("@CashierID_", cashierID);
                cmd.AddParameter("@PointOfSaleID_", pointOfSaleID);
                cmd.AddParameter("@Outlet_", outletName);
                cmd.AddParameter("@PaymentType_", paymentMode);
                cmd.AddParameter("@Remarks_", remarks);
                cmd.AddParameter("@ShowVoidedTransaction_", showVoidedTransaction);

                cmd.CommandTimeout = 0;

                dt.Load(DataService.GetReader(cmd));
                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchTransactionReportWithGST
               (DateTime StartDate, DateTime EndDate, string RefNo,
               string Cashier, string paymenttype, string outletName, string remark, string isVoided, string nametoappear)
        {
            string SQL = " Declare @temp table(ID varchar(14),Ptype  varchar(150)) " +
                         " Declare @ID varchar(30);Declare @r  nvarchar(max); " +
                         " Declare cr cursor for select distinct ReceipthdrID from receipthdr where receiptdate between '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                         " " +
                         " open cr fetch next from cr into  @id  while (@@FETCH_STATUS =0) " +
                         " Begin set @r='' select @r=isnull(@r,'') +(ISNULL(paymenttype,'NO PAYMENT')) + ': ' + CONVERT(varchar(50), ReceiptDet.Amount) + ','   " +
                         " from ReceiptHdr left join ReceiptDet on ReceiptHdr.ReceiptHdrID = ReceiptDet.ReceiptHdrID  " +
                         " where receipthdr.ReceiptHdrID=@id  " +
                          " " +
                         " insert into @temp select @id,left(@r,LEN(@r)-1)  " +
                          " " +
                         " fetch next from cr into  @id End close cr Deallocate cr  " +
                          " " +
                         " select X.*, Y.paymenttype from  " +
                          " " +
                         " (select CASE WHEN b.amount = 0 THEN 0 ELSE isnull(a.discountamount,0) END as discountamount, b.receipthdrid, case when isnull(a.userfld5,'Y') = 'Y' or a.userfld5 ='' then orderrefno else a.userfld5 end as orderrefno, " +
                         " b.amount,CASE WHEN b.amount = 0 THEN 0 ELSE isnull(a.gstamount,0) END as gst, " +
                         " b.amount - CASE WHEN b.amount = 0 THEN 0 ELSE isnull(a.gstamount,0) END as amountBefGST, " +
                         " orderdate,a.cashierid, a.orderhdrid, ISNULL(a.Userint1,0) TransactionCount, a.isvoided,  " +
                         " count(c.receiptdetid) as paymentcount, isnull(a.remark,'') as remark,  " +
                         " isnull(a.membershipno, '') as membershipno, " +
                         " isnull(d.nametoappear,'') as nametoappear, " +
                         " a.pointofsaleid,e.pointofsalename, " +
                         " e.outletname from orderhdr a  " +
                         "  " +
                         " left join receipthdr b on a.orderhdrid = b.orderhdrid   " +
                         " left join receiptdet c on c.receipthdrid=b.receipthdrid   " +
                         " left outer join membership d on d.membershipno=a.membershipno  " +
                         " inner join Pointofsale e on a.pointofsaleid = e.pointofsaleid  " +
                         " Where a.OrderDate between '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                         " group by a.discountamount, b.receipthdrid,a.userfld5, orderrefno,b.amount,a.gstamount, " +
                         " orderdate,a.cashierid,a.orderhdrid,ISNULL(a.Userint1,0),a.isvoided, a.remark,isnull(a.membershipno,''), " +
                         " isnull(d.nametoappear,''),a.pointofsaleid,e.pointofsalename,e.outletname " +
                         "  " +
                         " ) X   " +
                         "  " +
                         " left join  " +
                         " ( " +
                          " " +
                         " select id, Max(rtrim(Ptype + ' ' + ISNULL(Item.ItemName, " +
                         " isnull(receiptdet.Userfld1,'')) + ' ' + isnull(receiptdet.userfld2,'')))  as paymenttype " +
                          " " +
                          " from receiptdet LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
                          " right join @temp Temp on Temp.ID=receiptdet.ReceiptHdrID " +
                          "  " +
                          " group by id " +
                          " ) Y  on X.receiptHDRID = Y.id " +
                        "where X.orderdate >= ' " + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND X.orderdate <= ' " + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  " +
                        " AND X.OrderRefNo like '%" + RefNo + "%'" +
                        " AND X.CashierID like '%" + Cashier + "%'  " +
                        " AND Y.PaymentType like '%" + paymenttype + "%'  " + " AND X.PointOfSaleName like '%%' " +
                        " AND X.OutletName like '%" + outletName + "%' " +
                        " AND X.Remark like '%" + remark + "%' " +
                        " AND (X.isVoided = 0 OR '" + isVoided + "'='1') " +
                        " AND X.Nametoappear like N'%" + nametoappear + "%' " +
                        "order by x.orderdate desc; ";


            QueryCommand qr = new QueryCommand(SQL, "PowerPOS");
            qr.CommandTimeout = 1000000;

            DataTable dt = DataService.GetDataSet(qr).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((int)dt.Rows[i]["paymentcount"] > 1)
                {
                    // dt.Rows[i]["paymentType"] = dt.Rows[i]["paymentType"] + " (COMBINED)";
                }
            }
            return dt;
        }
        public static DataTable FetchTransactionReportWithProjectAndGST
        (DateTime StartDate, DateTime EndDate, string RefNo,
        string Cashier, string MemberID, string outletName, string ProjectName, string isVoided, string nametoappear)
        {
            string SQL = " Declare @temp table(ID varchar(14),Ptype  varchar(30)) " +
                         " Declare @ID numeric;Declare @r  nvarchar(max); " +
                         " Declare cr cursor for select distinct ReceipthdrID from receipthdr; " +
                         " " +
                         " open cr fetch next from cr into  @id  while (@@FETCH_STATUS =0) " +
                         " Begin set @r='' select @r=isnull(@r,'') +(ISNULL(paymenttype,'NO PAYMENT')) +','   " +
                         " from ReceiptHdr left join ReceiptDet on ReceiptHdr.ReceiptHdrID = ReceiptDet.ReceiptHdrID  " +
                         " where receipthdr.ReceiptHdrID=@id  " +
                          " " +
                         " insert into @temp select @id,left(@r,LEN(@r)-1)  " +
                          " " +
                         " fetch next from cr into  @id End close cr Deallocate cr  " +
                          " " +
                         " select X.*, Y.paymenttype from  " +
                          " " +
                         " (select CASE WHEN b.amount = 0 THEN 0 ELSE isnull(a.discountamount,0) END as discountamount, b.receipthdrid,orderrefno, " +
                         " b.amount,CASE WHEN b.amount = 0 THEN 0 ELSE isnull(a.gstamount,0) END as gst, " +
                         " b.amount - CASE WHEN b.amount = 0 THEN 0 ELSE isnull(a.gstamount,0) END as amountBefGST, " +
                         " orderdate,a.cashierid, a.orderhdrid,a.isvoided,  " +
                         " count(c.receiptdetid) as paymentcount, isnull(a.remark,'') as remark,  " +
                         " isnull(a.membershipno, '') as membershipno, " +
                         " isnull(d.nametoappear,'') as nametoappear, " +
                         " a.pointofsaleid,e.pointofsalename, a.userfld1 as ProjectName ," + //added
                         " e.outletname from orderhdr a  " +
                         "  " +
                         " left join receipthdr b on a.orderhdrid = b.orderhdrid   " +
                         " left join receiptdet c on c.receipthdrid=b.receipthdrid   " +
                         " left outer join membership d on d.membershipno=a.membershipno  " +
                         " inner join Pointofsale e on a.pointofsaleid = e.pointofsaleid  " +
                         "  " +
                         " group by a.discountamount, b.receipthdrid,orderrefno,b.amount,a.gstamount, " +
                         " orderdate,a.cashierid,a.orderhdrid,a.isvoided, a.remark,isnull(a.membershipno,''), " +
                         " isnull(d.nametoappear,''),a.pointofsaleid,e.pointofsalename, a.userfld1 ,e.outletname " +
                         "  " +
                         " ) X   " +
                         "  " +
                         " left join  " +
                         " ( " +
                          " " +
                         " select id, Max(rtrim(Ptype + ' ' + ISNULL(Item.ItemName, " +
                         " isnull(receiptdet.Userfld1,'')) + ' ' + isnull(receiptdet.userfld2,'')))  as paymenttype " +
                          " " +
                          " from receiptdet LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
                          " right join @temp Temp on Temp.ID=receiptdet.ReceiptHdrID " +
                          "  " +
                          " group by id " +
                          " ) Y  on X.receiptHDRID = Y.id " +
                        "where X.orderdate >= ' " + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND X.orderdate <= ' " + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  " +
                        " AND X.OrderHdrID like '%" + RefNo + "%'" +
                        " AND X.CashierID like '%" + Cashier + "%'  " +
                        " AND Y.PaymentType like '%" + MemberID + "%'  " + " AND X.PointOfSaleName like '%%' " +
                        " AND X.OutletName like '%" + outletName + "%' " +
                        " AND X.Projectname like '%" + ProjectName + "%' " +
                        " AND X.isVoided like '%" + isVoided + "%' " +
                        " AND X.Nametoappear like N'%" + nametoappear + "%' " +
                        " AND ISNULL(X.ProjectName,'') <> '' " +
                        "order by x.orderdate desc; ";


            QueryCommand qr = new QueryCommand(SQL, "PowerPOS");
            qr.CommandTimeout = 1000000;

            DataTable dt = DataService.GetDataSet(qr).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((int)dt.Rows[i]["paymentcount"] > 1)
                {
                    // dt.Rows[i]["paymentType"] = dt.Rows[i]["paymentType"] + " (COMBINED)";
                }
            }
            return dt;
        }
        public static DataTable FetchTransactionReport
                (DateTime StartDate, DateTime EndDate, string RefNo,
                string Cashier, string paymenttype, string outletName, string remark, string isVoided, string nametoappear)
        {
            string SQL = "select X.*, Y.paymenttype from " +
                        "(select   b.receipthdrid,a.Userfld5 as orderrefno,od.amount,orderdate,a.cashierid, " +
                        "a.orderhdrid,ISNULL(a.userint1,0) TransactionCount,a.isvoided, count(c.receiptdetid) as paymentcount, isnull(a.remark,'') as remark, " +
                        "isnull(a.membershipno, '') as membershipno,isnull(d.nametoappear,'') as nametoappear,a.pointofsaleid,e.pointofsalename,e.outletname " +
                        "from orderhdr a " +
                        "left join (select orderhdrid, sum(amount)as amount from OrderDet group by OrderHdrID ) od on a.orderhdrid = od.orderhdrid " +
                        "inner join receipthdr b on a.orderhdrid = b.orderhdrid  " +
                        "left join receiptdet c on c.receipthdrid=b.receipthdrid  " +
                        "left outer join membership d " +
                        "on d.membershipno=a.membershipno " +
                        "inner join Pointofsale e " +
                        "on a.pointofsaleid = e.pointofsaleid " +
                        "group by b.receipthdrid,a.Userfld5,od.amount, b.amount,orderdate,a.cashierid,a.orderhdrid, ISNULL(a.userint1,0),a.isvoided, " +
                        "a.remark,isnull(a.membershipno,''),isnull(d.nametoappear,''),a.pointofsaleid,e.pointofsalename,e.outletname) X  " +
                        "left join " +
                        "(select receipthdrid, Max(rtrim(paymenttype + ' ' + ISNULL(Item.ItemName,isnull(receiptdet." + ReceiptDet.UserColumns.PointItemNo + ",'')) + ' ' + isnull(receiptdet.userfld2,'')))  " +
                        "as paymenttype from receiptdet LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
                        "group by receipthdrid) Y  " +
                        "on X.receiptHDRID = Y.receipthdrID  " +
                        "where X.orderdate >= ' " + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND X.orderdate <= ' " + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  " +
                        " AND X.orderrefno like '%" + RefNo + "%'" +
                        " AND X.CashierID like '%" + Cashier + "%'  " +
                        " AND ISNULL(Y.PaymentType, '') like '%" + paymenttype + "%'  " + " AND X.PointOfSaleName like '%%' " +
                        " AND X.OutletName like '%" + outletName + "%' " +
                        " AND X.Remark like '%" + remark + "%' " +
                        " AND X.isVoided like '%" + isVoided + "%' " +
                        " AND X.Nametoappear like N'%" + nametoappear + "%' " +
                        "order by x.orderdate desc; ";


            QueryCommand qr = new QueryCommand(SQL, "PowerPOS");

            DataTable dt = DataService.GetDataSet(qr).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((int)dt.Rows[i]["paymentcount"] > 1)
                {
                    dt.Rows[i]["paymentType"] = dt.Rows[i]["paymentType"] + " (COMBINED)";
                }
            }
            return dt;
        }

        public static DataTable FetchTransactionReportForViewSales
          (DateTime StartDate, DateTime EndDate, string RefNo,
          string Cashier, string paymenttype, string outletName, string remark, string isVoided, string nametoappear, string lineInfo)
        {
            //string SQL = "select X.*, Y.PaymentType from " +
            //            "(select   b.receipthdrid,a.Userfld5 as orderrefno,b.amount,orderdate,a.cashierid, " +
            //            "a.orderhdrid,a.isvoided," +
            //            "count(c.receiptdetid) as paymentcount, " +
            //            "isnull(a.remark,'') as remark, " +
            //            "isnull(a.membershipno, '') as membershipno,isnull(d.nametoappear,'') as nametoappear,a.pointofsaleid,e.pointofsalename,e.outletname, " +
            //            "STUFF(( " +
            //            "SELECT DISTINCT ', ' + od.userfld4 AS LineInfo  " +
            //            "FROM OrderDet od  " +
            //            "WHERE od.OrderHdrID = a.OrderHdrID  " +
            //            "ORDER BY LineInfo  " +
            //            "FOR XML PATH(''), TYPE).value('.','varchar(max)'),1,2, '') AS LineInfo " +
            //            "from orderhdr a inner join receipthdr b on a.orderhdrid = b.orderhdrid  " +
            //            "LEFT JOIN receiptdet c on c.receipthdrid=b.receipthdrid  " +
            //            "left outer join membership d " +
            //            "on d.membershipno=a.membershipno " +
            //            "inner join Pointofsale e " +
            //            "on a.pointofsaleid = e.pointofsaleid " +
            //            "group by b.receipthdrid,a.Userfld5,b.amount,orderdate,a.cashierid,a.orderhdrid,a.isvoided, " +
            //            "a.remark,isnull(a.membershipno,''),isnull(d.nametoappear,''),a.pointofsaleid,e.pointofsalename,e.outletname) X  " +

            //            "LEFT JOIN " +
            //            "(Select distinct ReceiptHdrId," +
            //            "STUFF((select ',' + rtrim(paymenttype + ': ' + CONVERT(varchar(100), amount) + ' ' + ISNULL(Item.ItemName,isnull(receiptdet." + ReceiptDet.UserColumns.PointItemNo + ",'')) + ' ' + isnull(receiptdet.userfld2,''))  as paymenttype " +
            //            "from receiptdet LEFT OUTER JOIN Item ON receiptdet.userfld1 = Item.ItemNo " +
            //            "where ReceiptHdrID = det.ReceiptHdrID " +
            //            "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'),1,1,'')as PaymentType " +
            //            "from ReceiptDet det) Y " +

            //            "on X.receiptHDRID = Y.receipthdrID  " +
            //            "where X.orderdate >= ' " + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND X.orderdate <= ' " + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  " +
            //    //"where X.orderdate >= '2013-01-10' AND X.orderdate <= '2014-01-01'  " +
            //            " AND ISNULL(X.orderrefno,'') like '%" + RefNo + "%'" +
            //            " AND X.CashierID like '%" + Cashier + "%'  " +
            //            " AND ISNULL(Y.PaymentType,'') like '%" + paymenttype + "%'  " +
            //            " AND ISNULL(X.PointOfSaleName,'') like '%%' " +
            //    //" AND ISNULL(X.OutletName,'') like '%" + outletName + "%' " +
            //            " AND ISNULL(X.Remark,'') like '%" + remark + "%' " +
            //            " AND ISNULL(X.isVoided,'0') like '%" + isVoided + "%' " +
            //            " AND ISNULL(X.Nametoappear,'') like '%" + nametoappear + "%' " +
            //            " AND ISNULL(X.LineInfo,'') like '%" + lineInfo + "%' " +
            //            "order by x.orderdate desc; ";

            string sql = @"
SELECT
  X.*,
  Y.PaymentType
FROM (SELECT
  b.receipthdrid,
  a.Userfld5 AS orderrefno,
  b.amount,
  od.quantity,
  orderdate,
  a.cashierid,
  a.orderhdrid,
  a.isvoided,
  COUNT(c.receiptdetid) AS paymentcount,
  ISNULL(a.remark, '') AS remark,
  ISNULL(a.membershipno, '') AS membershipno,
  ISNULL(d.nametoappear, '') AS nametoappear,
  a.pointofsaleid,
  e.pointofsalename,
  e.outletname,
  STUFF((SELECT DISTINCT
    ', ' + od.userfld4 AS LineInfo
  FROM OrderDet od
  WHERE od.OrderHdrID = a.OrderHdrID
  ORDER BY LineInfo
  FOR xml PATH (''), TYPE)
  .value('.', 'varchar(max)'), 1, 2, '') AS LineInfo
FROM orderhdr a
LEFT JOIN (
SELECT   OD.OrderHdrID
		,SUM(OD.Quantity) Quantity
FROM	OrderDet OD 
WHERE	OD.ISVoided = 0
GROUP BY OD.OrderHdrID
) od ON od.OrderHdrID = a.OrderHdrID
INNER JOIN receipthdr b
  ON a.orderhdrid = b.orderhdrid
LEFT JOIN receiptdet c
  ON c.receipthdrid = b.receipthdrid
LEFT OUTER JOIN membership d
  ON d.membershipno = a.membershipno
INNER JOIN Pointofsale e
  ON a.pointofsaleid = e.pointofsaleid
GROUP BY b.receipthdrid,
         a.Userfld5,
         b.amount,
		 od.quantity,
         orderdate,
         a.cashierid,
         a.orderhdrid,
         a.isvoided,
         a.remark,
         ISNULL(a.membershipno, ''),
         ISNULL(d.nametoappear, ''),
         a.pointofsaleid,
         e.pointofsalename,
         e.outletname) X
LEFT JOIN (SELECT DISTINCT
  ReceiptHdrId,
  STUFF((SELECT
    ',' + RTRIM(paymenttype + ': ' + CONVERT(varchar(100), amount) + ' ' + ISNULL(Item.ItemName, ISNULL(receiptdet.Userfld1, '')) + ' ' + ISNULL(receiptdet.userfld2, '')) AS paymenttype
  FROM receiptdet
  LEFT OUTER JOIN Item
    ON receiptdet.userfld1 = Item.ItemNo
  WHERE ReceiptHdrID = det.ReceiptHdrID
  FOR xml PATH (''), TYPE)
  .value('.', 'varchar(max)'), 1, 1, '') AS PaymentType
FROM ReceiptDet det) Y
  ON X.receiptHDRID = Y.receipthdrID
WHERE X.orderdate >= '{0}'
AND X.orderdate <= '{1}'
AND ISNULL(X.orderrefno, '') LIKE '%{2}%'
AND X.CashierID LIKE '%{3}%'
AND ISNULL(Y.PaymentType, '') LIKE '%{4}%'
AND ISNULL(X.PointOfSaleName, '') LIKE '%{5}%'
AND ISNULL(X.Remark, '') LIKE '%{6}%'
AND ISNULL(X.isVoided, '0') LIKE '%{7}%'
AND ISNULL(X.Nametoappear, '') LIKE N'%{8}%'
AND ISNULL(X.LineInfo, '') LIKE '%{9}%'
ORDER BY x.orderdate DESC;";

            sql = string.Format(sql, StartDate.ToString("yyyy-MM-dd HH:mm:ss")
                                   , EndDate.ToString("yyyy-MM-dd HH:mm:ss")
                                   , RefNo
                                   , Cashier
                                   , paymenttype
                                   , ""
                                   , remark
                                   , isVoided
                                   , nametoappear
                                   , lineInfo);

            QueryCommand qr = new QueryCommand(sql, "PowerPOS");

            DataTable dt = DataService.GetDataSet(qr).Tables[0];

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if ((int)dt.Rows[i]["paymentcount"] > 1)
            //    {
            //        dt.Rows[i]["paymentType"] = dt.Rows[i]["paymentType"] + " (COMBINED)";
            //    }
            //}
            return dt;
        }

        public static DataTable FetchTransactionReportForViewSalesWeb
          (string StartDate, string EndDate, string RefNo,
          string Cashier, string paymenttype, string outletName, string remark, string isVoided, string nametoappear)
        {
            string SQL = "select X.*, Y.paymenttype from " +
                        "(select   b.receipthdrid,a.Userfld5 as orderrefno,b.amount,convert(date, orderdate) as orderdate,a.cashierid, " +
                        "a.orderhdrid,a.isvoided," +
                        "count(c.receiptdetid) as paymentcount, " +
                        "isnull(a.remark,'') as remark, " +
                        "isnull(a.membershipno, '') as membershipno,isnull(d.nametoappear,'') as nametoappear,a.pointofsaleid,e.pointofsalename,e.outletname " +
                        "from orderhdr a inner join receipthdr b on a.orderhdrid = b.orderhdrid  " +
                        "LEFT JOIN receiptdet c on c.receipthdrid=b.receipthdrid  " +
                        "left outer join membership d " +
                        "on d.membershipno=a.membershipno " +
                        "inner join Pointofsale e " +
                        "on a.pointofsaleid = e.pointofsaleid " +
                        "group by b.receipthdrid,a.Userfld5,b.amount,orderdate,a.cashierid,a.orderhdrid,a.isvoided, " +
                        "a.remark,isnull(a.membershipno,''),isnull(d.nametoappear,''),a.pointofsaleid,e.pointofsalename,e.outletname) X  " +

                        "LEFT JOIN " +
                        "(select receipthdrid, Max(rtrim(paymenttype + ' ' + ISNULL(Item.ItemName,isnull(receiptdet." + ReceiptDet.UserColumns.PointItemNo + ",'')) + ' ' + isnull(receiptdet.userfld2,'')))  " +
                        "as paymenttype from receiptdet LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
                        "group by receipthdrid) Y  " +

                        "on X.receiptHDRID = Y.receipthdrID  " +
                        "where X.orderdate between '" + StartDate + "' AND '" + EndDate + "'  " +
                        " AND ISNULL(X.orderrefno,'') like '%" + RefNo + "%'" +
                        " AND X.CashierID like '%" + Cashier + "%'  " +
                        " AND ISNULL(Y.PaymentType,'') like '%" + paymenttype + "%'  " +
                        " AND ISNULL(X.PointOfSaleName,'') like '%%' " +
                        " AND ISNULL(X.OutletName,'') like '%" + outletName + "%' " +
                        " AND ISNULL(X.Remark,'') like '%" + remark + "%' " +
                        " AND X.isVoided like '%" + isVoided + "%' " +
                        " AND ISNULL(X.Nametoappear,'') like N'%" + nametoappear + "%' " +
                        "order by x.orderdate desc; ";


            QueryCommand qr = new QueryCommand(SQL, "PowerPOS");

            DataTable dt = DataService.GetDataSet(qr).Tables[0];

            return dt;
        }


        public static DataTable FetchTransactionReport(bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ViewTransactionRefNo, string Cashier, int PointOfSaleID, string PointOfSaleName, string outletName, string deptID,
            string SortColumn, string SortDir)
        {

            ViewTransactionCollection myViewTransaction = new ViewTransactionCollection();
            if (useStartDate & useEndDate)
            {
                myViewTransaction.BetweenAnd(ViewTransaction.Columns.OrderDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewTransaction.Where(ViewTransaction.Columns.OrderDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewTransaction.Where(ViewTransaction.Columns.OrderDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }
            if (ViewTransactionRefNo != "")
            {
                myViewTransaction.Where(ViewTransaction.Columns.OrderRefNo, SubSonic.Comparison.Like, ViewTransactionRefNo);
            }

            if (Cashier != "")
            {
                myViewTransaction.Where(ViewTransaction.Columns.CashierID, SubSonic.Comparison.Like, Cashier);
            }
            if (outletName != "")
            {
                myViewTransaction.Where(ViewTransaction.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }
            if (deptID != "0")
            {
                myViewTransaction.Where(ViewTransaction.Columns.DepartmentID, SubSonic.Comparison.Like, deptID);
            }




            if (PointOfSaleID > 0) //<0 for all
            {
                myViewTransaction.Where(ViewTransaction.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    myViewTransaction.Where(ViewTransaction.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }
            SubSonic.TableSchema.TableColumn t = ViewTransaction.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewTransaction.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewTransaction.OrderByDesc(SortColumn);
                }
                else
                {
                    myViewTransaction.OrderByDesc("OrderDate");
                }
            }
            else
            {
                myViewTransaction.OrderByDesc("OrderDate");
            }
            return myViewTransaction.Load().ToDataTable();
        }
        public static DataTable TransactionDetailReportWithRemark(DateTime StartDate, DateTime EndDate, int PosID, string OutletName, string DeptID, string SortColumn, string SortDir, object SearchValue)
        {
            string strQuery =

                "SELECT OrderDet.ItemNo, OrderDet.OrderDetDate, OrderDet.Quantity, OrderDet.UnitPrice, OrderDet.Discount" +
                    ", OrderDet.Amount, OrderDet.IsPromo, OrderDet.PromoDiscount, OrderDet.PromoAmount" +
                    ", OrderDet.IsPromoFreeOfCharge, OrderDet.IsFreeOfCharge, PointOfSale.PointOfSaleName" +
                    ", ISNULL(OrderHdr.Userfld5,OrderHdr.OrderRefNo) OrderRefNo, Item.ItemName, OrderHdr.NettAmount, OrderHdr.IsVoided" +
                    ", OrderDet.IsVoided AS IsLineVoided, Category.ItemDepartmentID, Item.CategoryName" +
                    ", PointOfSale.OutletName, PointOfSale.DepartmentID, PointOfSale.PointOfSaleID" +
                    ", OrderDet.InventoryHdrRefNo ,OrderHdr.Remark, OrderDet.Remark " +
                " FROM OrderDet " +
                    "INNER JOIN OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID " +
                    "INNER JOIN PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID " +
                    "INNER JOIN Item ON OrderDet.ItemNo = Item.ItemNo " +
                    "INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                "WHERE OrderDet.OrderDetDate BETWEEN @StartDate AND @EndDate " +
                    "AND PointOfSale.OutletName LIKE @OutletName " +
                    "AND PointOfSale.DepartmentID LIKE @DeptID " +
                    "AND (" +
                        "OrderHdr.Remark LIKE @Search " +
                        "OR OrderDet.Remark LIKE @Search " +
                        "OR OrderDet.ItemNo LIKE @Search " +
                        "OR Item.ItemName LIKE @Search " +
                        "OR Item.CategoryName LIKE @Search " +
                        "OR Category.ItemDepartmentID LIKE @Search " +
                        ") ";

            if (PosID != 0) strQuery += " AND PointOfSale.PointOfSaleID = " + PosID.ToString("N0");

            QueryCommand Cmd = new QueryCommand(strQuery);
            Cmd.AddParameter("@StartDate", StartDate, DbType.DateTime);
            Cmd.AddParameter("@EndDate", EndDate, DbType.String);
            Cmd.AddParameter("@OutletName", "%" + OutletName + "%", DbType.String);
            Cmd.AddParameter("@DeptID", "%" + DeptID + "%", DbType.String);
            Cmd.AddParameter("@Search", "%" + SearchValue.ToString() + "%", DbType.String);

            DataTable Output = new DataTable();
            Output.Load(DataService.GetReader(Cmd));

            return Output;
        }

        public static DataTable FetchTransactionDetailWithRemarkReport(bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
                    string ItemName,
                    string CategoryName, int PointOfSaleID, bool remarkOnly, string PointOfSaleName,
                    string outletName, string deptID,
                    string SortColumn, string SortDir)
        {


            ViewTransactionDetailWithRemarkCollection myViewTransactionDetailWithRemark = new ViewTransactionDetailWithRemarkCollection();
            if (useStartDate & useEndDate)
            {
                myViewTransactionDetailWithRemark.BetweenAnd(ViewTransactionDetailWithRemark.Columns.OrderDetDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewTransactionDetailWithRemark.Where(ViewTransactionDetailWithRemark.Columns.OrderDetDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewTransactionDetailWithRemark.Where(ViewTransactionDetailWithRemark.Columns.OrderDetDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }
            if (ItemName != "")
            {
                myViewTransactionDetailWithRemark.Where(ViewTransactionDetailWithRemark.Columns.ItemNo, SubSonic.Comparison.Like, "%" + ItemName + "%");
            }
            if (CategoryName != "")
            {
                myViewTransactionDetailWithRemark.Where(ViewTransactionDetailWithRemark.Columns.CategoryName, SubSonic.Comparison.Like, CategoryName);
            }

            if (outletName != "")
            {
                myViewTransactionDetailWithRemark.Where(ViewTransactionDetailWithRemark.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (deptID != "0")
            {
                myViewTransactionDetailWithRemark.Where(ViewTransactionDetailWithRemark.Columns.DepartmentID, SubSonic.Comparison.Like, deptID);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                myViewTransactionDetailWithRemark.Where(ViewTransactionDetailWithRemark.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    myViewTransactionDetailWithRemark.Where(ViewTransactionDetailWithRemark.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }
            SubSonic.TableSchema.TableColumn t = ViewTransactionDetailWithRemark.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewTransactionDetailWithRemark.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewTransactionDetailWithRemark.OrderByDesc(SortColumn);
                }
                else
                {
                    myViewTransactionDetailWithRemark.OrderByDesc("OrderDetDate");
                }
            }
            else
            {
                myViewTransactionDetailWithRemark.OrderByDesc("OrderDetDate");
            }
            return myViewTransactionDetailWithRemark.Load().ToDataTable();
        }
        public static DataTable FetchTransactionDetailReportForWeb
                    (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
                    string ItemName,
                    string CategoryName, int PointOfSaleID, string PointOfSaleName,
                    string outletName, string deptID,
                    string SortColumn, string SortDir)
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            StartDate = StartDate.Date;
            EndDate = EndDate.Date.AddDays(1);

            if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return FetchTransactionDetailReportForWeb_FIFO(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, CategoryName, PointOfSaleID, PointOfSaleName, outletName, deptID, SortColumn, SortDir);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)
                return FetchTransactionDetailReportForWeb_FixedAvg(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, CategoryName, PointOfSaleID, PointOfSaleName, outletName, deptID, SortColumn, SortDir);
            else
                return FetchTransactionDetailReportForWeb_FIFO(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, CategoryName, PointOfSaleID, PointOfSaleName, outletName, deptID, SortColumn, SortDir);
        }
        public static DataTable FetchTransactionDetailReportForWeb_FIFO
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName,
            string CategoryName, int PointOfSaleID, string PointOfSaleName,
            string outletName, string deptID,
            string SortColumn, string SortDir)
        {
            string SQL = "select b.GSTAmount as GSTAmountPerItem" +
                            ", ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, ISNULL(Attributes3,'') Attributes3 " +
                            ", ISNULL(Attributes4,'') Attributes4, ISNULL(Attributes5,'') Attributes5, ISNULL(Attributes6,'') Attributes6 " +
                            ", ISNULL(Attributes7,'') Attributes7 " +
                            ",b.Discount*b.unitprice*b.quantity/100 as Disc, b.Discount as DiscPercent,a.Userfld5 as Customize, * from OrderHdr a inner join OrderDet b " +
                            "on a.OrderHdrID = b.OrderHdrID inner join item c " +
                            "on b.itemno = c.itemno " +
                            "inner join pointofsale d on a.pointofsaleid = d.pointofsaleid " +
                            "inner join category e on c.categoryname = e.categoryname " +
                         "where a.isvoided=0 and b.isvoided=0 ";
            if (useStartDate)
            {
                SQL += " AND " + ViewTransactionDetail.Columns.OrderDetDate + " >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }
            if (useEndDate)
            {
                SQL += " AND " + ViewTransactionDetail.Columns.OrderDetDate + " <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }

            if (ItemName != "")
            {
                SQL += " AND c.ItemName like '%" + ItemName + "%' ";

            }
            if (CategoryName != "")
            {
                SQL += " AND  e.CategoryName like '%" + CategoryName + "%' ";
            }

            if (outletName != "")
            {
                SQL += " AND  OutletName like '%" + outletName + "%' ";
            }

            if (deptID != "0")
            {
                SQL += " AND  ItemDepartmentID = '" + deptID + "' ";
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                SQL += " AND  PointOfSaleID = " + PointOfSaleID.ToString() + " ";
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    SQL += " AND  PointOfSaleName like '%" + PointOfSaleName + "%' ";
                }
            }
            if (SortColumn != "")
            {
                SQL += "Order by " + SortColumn + " " + SortDir;
            }
            else
            {
                SQL += "Order by OrderDetDate desc ";
            }
            DataTable dt = DataService.GetDataSet(new QueryCommand(SQL, "PowerPOS")).Tables[0];

            DataTable remaining = ReportController.FetchStockReport("", 0, true, "", "", "");
            //Add average cost price
            dt.Columns.Add("AverageCost", System.Type.GetType("System.Decimal"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal avgCost = 0.00M;
                DataRow[] dr = remaining.Select("ItemNo = '" + dt.Rows[i]["ItemNo"].ToString() + "'");
                if (dr.Length > 0 && dr[0]["CostOfGoods"] != null)
                {
                    decimal.TryParse(dr[0]["CostOfGoods"].ToString(), out avgCost);
                }
                dt.Rows[i]["AverageCost"] = avgCost;
            }

            //add price before GST
            dt.Columns.Add("amountBefGST", System.Type.GetType("System.Decimal"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal amount = 0.00M;
                decimal GSTamount = 0.00M;

                decimal.TryParse(dt.Rows[i]["Amount"].ToString(), out amount);
                decimal.TryParse(dt.Rows[i]["GSTAmountPerItem"].ToString(), out GSTamount);

                dt.Rows[i]["amountBefGST"] = amount - GSTamount;
            }
            return dt;
        }
        public static DataTable FetchTransactionDetailReportForWeb_FixedAvg
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName,
            string CategoryName, int PointOfSaleID, string PointOfSaleName,
            string outletName, string deptID,
            string SortColumn, string SortDir)
        {
            string SQL = "select b.GSTAmount as GSTAmountPerItem" +
                            ", ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, ISNULL(Attributes3,'') Attributes3 " +
                            ", ISNULL(Attributes4,'') Attributes4, ISNULL(Attributes5,'') Attributes5, ISNULL(Attributes6,'') Attributes6 " +
                            ", ISNULL(Attributes7,'') Attributes7 " +
                            ",b.Discount*b.unitprice*b.quantity/100 as Disc, b.Discount as DiscPercent, * from OrderHdr a inner join OrderDet b " +
                            "on a.OrderHdrID = b.OrderHdrID inner join item c " +
                            "on b.itemno = c.itemno " +
                            "inner join pointofsale d on a.pointofsaleid = d.pointofsaleid " +
                            "inner join category e on c.categoryname = e.categoryname " +
                         "where a.isvoided=0 and b.isvoided=0 ";
            if (useStartDate)
            {
                SQL += " AND " + ViewTransactionDetail.Columns.OrderDetDate + " >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }
            if (useEndDate)
            {
                SQL += " AND " + ViewTransactionDetail.Columns.OrderDetDate + " <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }

            if (ItemName != "")
            {
                SQL += " AND c.ItemName like '%" + ItemName + "%' ";

            }
            if (CategoryName != "")
            {
                SQL += " AND  e.CategoryName like '%" + CategoryName + "%' ";
            }

            if (outletName != "")
            {
                SQL += " AND  OutletName like '%" + outletName + "%' ";
            }

            if (deptID != "0")
            {
                SQL += " AND  ItemDepartmentID = '" + deptID + "' ";
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                SQL += " AND  PointOfSaleID = " + PointOfSaleID.ToString() + " ";
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    SQL += " AND  PointOfSaleName like '%" + PointOfSaleName + "%' ";
                }
            }
            if (SortColumn != "")
            {
                SQL += "Order by " + SortColumn + " " + SortDir;
            }
            else
            {
                SQL += "Order by OrderDetDate desc ";
            }
            DataTable dt = DataService.GetDataSet(new QueryCommand(SQL, "PowerPOS")).Tables[0];

            DataTable remaining = ReportController.FetchStockReport("", 0, true, "", "", "");

            //Add average cost price
            dt.Columns.Add("AverageCost", System.Type.GetType("System.Decimal"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal avgCost = 0.00M;
                DataRow[] dr = remaining.Select("ItemNo = '" + dt.Rows[i]["ItemNo"].ToString() + "'");
                if (dr.Length > 0 && dr[0]["CostOfGoods"] != null)
                {
                    decimal.TryParse(dr[0]["CostOfGoods"].ToString(), out avgCost);
                }
                dt.Rows[i]["AverageCost"] = avgCost;
            }

            //add price before GST
            dt.Columns.Add("amountBefGST", System.Type.GetType("System.Decimal"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal amount = 0.00M;
                decimal GSTamount = 0.00M;

                decimal.TryParse(dt.Rows[i]["Amount"].ToString(), out amount);
                decimal.TryParse(dt.Rows[i]["GSTAmountPerItem"].ToString(), out GSTamount);

                dt.Rows[i]["amountBefGST"] = amount - GSTamount;
            }
            return dt;
        }
        public static DataTable FetchTransactionDetailReport
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemNo,
            string CategoryName, int PointOfSaleID, string PointOfSaleName,
            string outletName, string deptID,
            string SortColumn, string SortDir)
        {
            ViewTransactionDetailCollection myViewTransactionDetail = new ViewTransactionDetailCollection();
            if (useStartDate & useEndDate)
            {
                myViewTransactionDetail.BetweenAnd(ViewTransactionDetail.Columns.OrderDetDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewTransactionDetail.Where(ViewTransactionDetail.Columns.OrderDetDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewTransactionDetail.Where(ViewTransactionDetail.Columns.OrderDetDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }

            if (ItemNo != "")
            {
                myViewTransactionDetail.Where(ViewTransactionDetail.Columns.ItemNo, SubSonic.Comparison.Like, ItemNo);
            }
            if (CategoryName != "")
            {
                myViewTransactionDetail.Where(ViewTransactionDetail.Columns.CategoryName, SubSonic.Comparison.Like, CategoryName);
            }

            if (outletName != "")
            {
                myViewTransactionDetail.Where(ViewTransactionDetail.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (deptID != "0")
            {
                myViewTransactionDetail.Where(ViewTransactionDetail.Columns.DepartmentID, SubSonic.Comparison.Like, deptID);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                myViewTransactionDetail.Where(ViewTransactionDetail.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    myViewTransactionDetail.Where(ViewTransactionDetail.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }

            SubSonic.TableSchema.TableColumn t = ViewTransactionDetail.Schema.GetColumn(SortColumn);
            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewTransactionDetail.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewTransactionDetail.OrderByDesc(SortColumn);
                }
                else
                {
                    myViewTransactionDetail.OrderByDesc("OrderDetDate");
                }
            }
            else
            {
                myViewTransactionDetail.OrderByDesc("OrderDetDate");
            }

            return myViewTransactionDetail.Load().ToDataTable();
        }

        public static DataTable FetchTransactionWithSalesPersonByDaily
                (DateTime StartDate, DateTime EndDate, int type,
                    int PointOfSaleID)
        {
            try
            {

                Query qr = ViewDailyTransactionBySalesPerson.CreateQuery();
                qr.QueryType = QueryType.Select;
                string pivotcol;
                if (type == 0)
                {
                    pivotcol = ViewDailyTransactionBySalesPerson.Columns.Amount;
                }
                else if (type == 1)
                {
                    pivotcol = ViewDailyTransactionBySalesPerson.Columns.Serviceamount;
                }
                else
                {
                    pivotcol = ViewDailyTransactionBySalesPerson.Columns.Productamount;
                }
                //PointOfSaleID = 45;
                PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                qr.SelectList = "Orderdate, Displayname," + pivotcol;
                qr.AddWhere(ViewDailyTransactionBySalesPerson.Columns.Orderdate, Comparison.GreaterOrEquals, StartDate);
                qr.AddWhere(ViewDailyTransactionBySalesPerson.Columns.Orderdate, Comparison.LessOrEquals, EndDate);
                //qr.AddWhere(ViewDailyTransactionBySalesPerson.Columns.PointOfSaleID, PointOfSaleID);

                DataTable dt;

                dt = Pivot(qr.ORDER_BY(ViewDailyTransactionBySalesPerson.Columns.Orderdate).ExecuteReader(), "OrderDate", "displayname", pivotcol);

                //add total column
                decimal total;
                dt.Columns.Add("TOTAL", typeof(decimal));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    total = 0;
                    for (int k = 1; k < dt.Columns.Count - 1; k++)
                    {
                        if (dt.Rows[i][k] != null && dt.Rows[i][k] is decimal)
                            total += (decimal)dt.Rows[i][k];
                    }
                    dt.Rows[i]["Total"] = total;
                }
                DataRow drFooter = dt.NewRow();
                for (int k = 1; k < dt.Columns.Count; k++)
                {
                    total = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][k] != null && dt.Rows[i][k] is decimal)
                            total += (decimal)dt.Rows[i][k];
                    }
                    drFooter[k] = total;
                }
                dt.Rows.Add(drFooter);
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        public static DataTable FetchTransactionWithSalesPersonByReceiptNo
            (DateTime StartDate, DateTime EndDate,
             bool IsServiceItem, bool IsInventoryItem)
        {
            try
            {
                string productOrServiceFilter = "";

                if (IsServiceItem & IsInventoryItem)
                {
                    productOrServiceFilter = " 1=1 ";

                }
                else if (IsServiceItem & !IsInventoryItem)
                {
                    productOrServiceFilter = " IsServiceItem = 1 and IsInInventory=0 ";
                }
                else if (!IsServiceItem & IsInventoryItem)
                {
                    productOrServiceFilter = " IsServiceItem = 0 and IsInInventory=1 ";
                }
                else
                {
                    productOrServiceFilter = " IsServiceItem = 0 and IsInInventory=0 ";
                }
                string SQL = "select orderdate,b.salespersonid,  " +
                            "b.OrderHdrID as orderhdrid, sum(b.amount) as amount " +
                            "from orderhdr a " +
                            "inner join ( " +
                            "        SELECT od.OrderHdrID, ISNULL(NULLIF(od.userfld1,''), scr.SalesPersonID) SalesPersonID, od.ItemNo,  " +
                            "            CASE WHEN ISNULL(od.userfld20,'') = '' THEN od.Amount ELSE od.Amount/2 END Amount " +
                            "        FROM OrderDet od  " +
                            "            INNER JOIN SalesCommissionRecord scr ON scr.OrderHdrID = od.OrderHdrID " +
                            "        WHERE od.isvoided = 0 " +
                            "        UNION ALL " +
                            "        SELECT od.OrderHdrID, od.userfld20 SalesPersonID, od.ItemNo, od.Amount/2 Amount " +
                            "        FROM OrderDet od  " +
                            "            INNER JOIN SalesCommissionRecord scr ON scr.OrderHdrID = od.OrderHdrID " +
                            "        WHERE od.isvoided = 0 AND ISNULL(od.userfld20,'') <> '' " +
                            "    ) b ON a.orderhdrid = b.orderhdrid  " +
                            "inner join SalesCommissionRecord c " +
                            "on a.OrderHdrID = c.OrderHdrID " +
                            "inner join item d " +
                            "on b.itemno = d.itemno " +
                            "where OrderDate > '" + StartDate.ToString("yyyy-MM-dd") + "' and orderdate < '" + EndDate.AddDays(1).ToString("yyyy-MM-dd") + "' " +
                            "and a.isvoided=0 and isCommission=1 " +
                            "and " + productOrServiceFilter +
                            "group by orderdate,b.salespersonid,b.orderhdrid order by orderdate asc";
                DataTable dt = ReportController.Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "OrderHdrId", "SalesPersonID", "Amount");
                return dt;
                /*
                Query qr = ViewProductAndServiceBySalesPerson.CreateQuery();
                qr.QueryType = QueryType.Select;
                qr.SelectList = "OrderHdrID,OrderDate,Displayname,ProductAmount, ServiceAmount";
                qr.AddWhere(ViewProductAndServiceBySalesPerson.Columns.OrderDate, Comparison.GreaterOrEquals, StartDate);
                qr.AddWhere(ViewProductAndServiceBySalesPerson.Columns.OrderDate, Comparison.LessOrEquals, EndDate);
                qr.AddWhere(ViewProductAndServiceBySalesPerson.Columns.PointOfSaleID, PointOfSaleID);
                qr.ORDER_BY("OrderHdrID");

                DataTable dt = Pivot(qr.ExecuteReader(), "OrderHdrID", "displayname", "ServiceAmount");


                return dt;
                */
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchTransactionWithSalesPersonByReceiptNo
            (DateTime StartDate, DateTime EndDate, string WithCommision,
             bool IsServiceItem, bool IsInventoryItem)
        {
            try
            {
                string productOrServiceFilter = "";
                string withcommision = "";

                if (WithCommision.ToLower() == "with commision")
                {
                    withcommision = " ISNULL(IsCommission,0)=1 ";
                }
                else if (WithCommision.ToLower() == "without commision")
                {
                    withcommision = " ISNULL(IsCommission,0)=0 ";
                }
                else
                {
                    withcommision = " 2=2 ";
                }

                if (IsServiceItem & IsInventoryItem)
                {
                    productOrServiceFilter = " 1=1 ";

                }
                else if (IsServiceItem & !IsInventoryItem)
                {
                    productOrServiceFilter = " IsServiceItem = 1 and IsInInventory=0 ";
                }
                else if (!IsServiceItem & IsInventoryItem)
                {
                    productOrServiceFilter = " IsServiceItem = 0 and IsInInventory=1 ";
                }
                else
                {
                    productOrServiceFilter = " IsServiceItem = 0 and IsInInventory=0 ";
                }
                string SQL = "select orderdate,isnull(nullif(b.userfld1,''),c.salespersonid) as salespersonid,  " +
                            "b.OrderHdrID as orderhdrid, sum(b.amount) as amount " +
                            "from orderhdr a " +
                            "inner join orderdet b " +
                            "on a.orderhdrid = b.orderhdrid " +
                            "inner join SalesCommissionRecord c " +
                            "on a.OrderHdrID = c.OrderHdrID " +
                            "inner join item d " +
                            "on b.itemno = d.itemno " +
                            "where OrderDate > '" + StartDate.ToString("yyyy-MM-dd") + "' and orderdate < '" + EndDate.AddDays(1).ToString("yyyy-MM-dd") + "' " +
                            "and ISNULL(a.isvoided,0)=0 and ISNULL(b.isvoided,0)=0 and " + withcommision + " " +
                            "and " + productOrServiceFilter +
                            "and b.ItemNo <> 'INST_PAYMENT' " +
                            "group by orderdate,isnull(nullif(b.userfld1,''),c.salespersonid),b.orderhdrid order by orderdate, isnull(nullif(b.userfld1,''),c.salespersonid) asc";
                DataTable dt = ReportController.Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "OrderHdrId", "SalesPersonID", "Amount");
                return dt;
                /*
                Query qr = ViewProductAndServiceBySalesPerson.CreateQuery();
                qr.QueryType = QueryType.Select;
                qr.SelectList = "OrderHdrID,OrderDate,Displayname,ProductAmount, ServiceAmount";
                qr.AddWhere(ViewProductAndServiceBySalesPerson.Columns.OrderDate, Comparison.GreaterOrEquals, StartDate);
                qr.AddWhere(ViewProductAndServiceBySalesPerson.Columns.OrderDate, Comparison.LessOrEquals, EndDate);
                qr.AddWhere(ViewProductAndServiceBySalesPerson.Columns.PointOfSaleID, PointOfSaleID);
                qr.ORDER_BY("OrderHdrID");

                DataTable dt = Pivot(qr.ExecuteReader(), "OrderHdrID", "displayname", "ServiceAmount");


                return dt;
                */
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }


        public static DataTable FetchTransactionDetailWithSalesPersonReport
            (DateTime StartDate, DateTime EndDate,
            string search,
            string PointOfSaleName,
            string outletName,
            string DeptID,
            string CategoryName,
            string SortColumn,
            string SortDir)
        {
            try
            {
                if (DeptID == "0")
                    DeptID = "%";

                if (CategoryName == "")
                    CategoryName = "%";

                //add attributes
                AttributesLabelCollection atCol = new AttributesLabelCollection();
                atCol.Load();
                string sqlAtt = "";
                string sqlGroupAtt = "";
                foreach (PowerPOS.AttributesLabel a in atCol)
                {
                    if (!String.IsNullOrEmpty(a.Label))
                    {
                        sqlAtt += "ViewItem.Attributes" + a.AttributesNo.ToString() + " as [" + a.Label + "], ";
                        sqlGroupAtt += "ViewItem.Attributes" + a.AttributesNo.ToString() + ", ";
                    }
                }

                string SQL = "declare @startdate datetime; " +
                        "declare @enddate datetime; " +
                        "declare @itemname varchar(50); " +
                        "declare @PointOfSaleName varchar(50); " +
                        "declare @OutletName varchar(50); " +
                        "declare @DeptID varchar(50); " +
                        "declare @CategoryName varchar(50)" +
                        "declare @IsVoided bit; " +
                        "set @startdate = '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                        "set @enddate = '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                        "set @itemname = '" + search + "' " +
                        "set @OutletName = '" + outletName + "'; " +
                        "set @PointOfSaleName = '" + PointOfSaleName + "'; " +
                        "set @DeptID = '" + DeptID + "'; " +
                        "set @CategoryName='" + CategoryName + "';" +
                        "set @IsVoided = 0; " +
                        "if (@OutletName = 'ALL' AND @PointOfSaleName = 'ALL')  " +
                        "Begin " +
                        "SELECT ViewItem.DepartmentName, ViewItem.CategoryName, ViewItem.ItemNo,  " +
                        "ViewItem.ItemName,    ";


                SQL += sqlAtt;
                SQL +=
                        "   Sum(OrderDet.Amount) AS TotalAmount,       " +
                        //"    isnull(nullif(OrderDet.userfld1,''),UserMst.DisplayName) as DisplayName " +
                        " case when orderdet.userfld1 is null or orderdet.userfld1=''  then usermst.displayname  else  SPerson.DIsplayName	end as DisplayName " +
                        "From ViewItem INNER JOIN " +
                        "  OrderDet ON ViewItem.ItemNo = OrderDet.ItemNo INNER JOIN " +
                        "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
                        "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
                        "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
                        "  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID   " +
                        "  INNER JOIN SalesCommissionRecord on SalesCommissionRecord.OrderHdrID = Orderhdr.OrderHdrID " +
                        " INNER JOIN USERMST on SalesCommissionRecord.salespersonid = USERMST.username " +
                        " left join usermst SPerson	on orderdet.userfld1 = SPerson.username " +
                        "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
                        "   AND ViewItem.search Like @itemname        " +
                        "   AND OrderDet.IsVoided = @IsVoided " +
                        "   AND OrderHdr.IsVoided = 0 " +
                        //"   AND OrderDet.Quantity > 0 " +
                        "   AND ViewItem.ItemDepartmentId like @DeptID " +
                        "   AND ViewItem.CategoryName like @CategoryName " +
                        "GROUP BY ViewItem.DepartmentName, ViewItem.CategoryName, ViewItem.ItemNo, ViewItem.ItemName,       ";
                SQL += sqlGroupAtt;
                SQL +=
                //"isnull(nullif(OrderDet.userfld1,''),UserMst.DisplayName) " +
                " case when orderdet.userfld1 is null or orderdet.userfld1=''  then usermst.displayname else  SPerson.DIsplayName end " +
                "End " +
                "Else if (@PointOfSaleName = 'ALL' and @OutletName != 'ALL') " +
                "Begin " +
                "SELECT ViewItem.DepartmentName, ViewItem.CategoryName, ViewItem.ItemNo, ViewItem.ItemName,    ";
                //add attributes

                SQL += sqlAtt;
                SQL +=
                "   Sum(OrderDet.Amount) AS TotalAmount,       " +
                //"    isnull(nullif(OrderDet.userfld1,''),UserMst.DisplayName) as DisplayName " +
                " case when orderdet.userfld1 is null or orderdet.userfld1=''  then usermst.displayname  else  SPerson.DIsplayName	end as DisplayName " +
                "From ViewItem INNER JOIN " +
                "  OrderDet ON ViewItem.ItemNo = OrderDet.ItemNo INNER JOIN " +
                "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
                "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
                "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
                "INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
                "INNER JOIN SalesCommissionRecord on SalesCommissionRecord.OrderHdrID = Orderhdr.OrderHdrID " +
                "INNER JOIN USERMST on SalesCommissionRecord.salespersonid = USERMST.username " +
                " left join usermst SPerson	on orderdet.userfld1 = SPerson.username " +
                "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
                "   AND ViewItem.search Like @itemname  " +
                "   AND Outlet.OutletName Like @OutletName    " +
                "   AND OrderDet.IsVoided = @IsVoided " +
                "   AND OrderHdr.IsVoided = 0 " +
                //"   AND OrderDet.Quantity > 0 " +
                "   AND ViewItem.ItemDepartmentId like @DeptID " +
                "   AND ViewItem.CategoryName like @CategoryName " +
                "GROUP BY ViewItem.DepartmentName, ViewItem.CategoryName, ViewItem.ItemNo, ViewItem.ItemName,       ";
                SQL += sqlGroupAtt;
                SQL +=
                //"isnull(nullif(OrderDet.userfld1,''),UserMst.DisplayName) " +
                " case when orderdet.userfld1 is null or orderdet.userfld1=''  then usermst.displayname else  SPerson.DIsplayName end " +
                "Order By DepartmentName,CategoryName, ItemName " +
                "End " +
                "Else if (@PointOfSaleName != 'ALL' and @OutletName = 'ALL')   " +
                "begin " +
                "set @OutletName = '%'; " +
                "SELECT ViewItem.DepartmentName, ViewItem.CategoryName, ViewItem.ItemNo, ViewItem.ItemName,    ";
                //add attributes

                SQL += sqlAtt;
                SQL +=
                "   Sum(OrderDet.Amount) AS TotalAmount,       " +
                //"    isnull(nullif(OrderDet.userfld1,''),UserMst.DisplayName) as DisplayName " +
                " case when orderdet.userfld1 is null or orderdet.userfld1=''  then usermst.displayname  else  SPerson.DIsplayName	end as DisplayName " +
                "From ViewItem INNER JOIN " +
                "  OrderDet ON ViewItem.ItemNo = OrderDet.ItemNo INNER JOIN " +
                "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
                "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
                "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
                "  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID   " +
                "  INNER JOIN SalesCommissionRecord on SalesCommissionRecord.OrderHdrID = Orderhdr.OrderHdrID " +
                "  INNER JOIN USERMST on SalesCommissionRecord.salespersonid = USERMST.username " +
                " left join usermst SPerson	on orderdet.userfld1 = SPerson.username " +
                "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
                "   AND ViewItem.search Like @itemname AND PointOfSale.PointOfSaleName Like @PointOfSaleName " +
                "   AND Outlet.OutletName Like @OutletName    " +
                "   AND OrderDet.IsVoided = @IsVoided " +
                "   AND OrderHdr.IsVoided = 0 " +
                //"   AND OrderDet.Quantity > 0 " +
                "   AND ViewItem.ItemDepartmentId like @DeptID " +
                "   AND ViewItem.CategoryName like @CategoryName " +
                "GROUP BY ViewItem.DepartmentName, ViewItem.CategoryName, ViewItem.ItemNo, ViewItem.ItemName,       ";
                SQL += sqlGroupAtt;
                SQL +=
                //"isnull(nullif(OrderDet.userfld1,''),UserMst.DisplayName) " +
                " case when orderdet.userfld1 is null or orderdet.userfld1=''  then usermst.displayname else  SPerson.DIsplayName end " +
                "Order By DepartmentName,CategoryName, ItemName " +
                "end " +
                "Else " +
                "Begin    " +
                "SELECT ViewItem.DepartmentName, ViewItem.CategoryName, ViewItem.ItemNo, ViewItem.ItemName,    ";
                SQL += sqlAtt;
                SQL +=
                "   Sum(OrderDet.Amount) AS TotalAmount,       " +
                //"    isnull(nullif(OrderDet.userfld1,''),UserMst.DisplayName) as DisplayName " +
                 " case when orderdet.userfld1 is null or orderdet.userfld1=''  then usermst.displayname  else  SPerson.DIsplayName	end as DisplayName " +
                "From ViewItem INNER JOIN " +
                "  OrderDet ON ViewItem.ItemNo = OrderDet.ItemNo INNER JOIN " +
                "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
                "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
                "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
                "  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID " +
                "  INNER JOIN SalesCommissionRecord on SalesCommissionRecord.OrderHdrID = Orderhdr.OrderHdrID " +
                "   INNER JOIN USERMST on SalesCommissionRecord.salespersonid = USERMST.username " +
                 " left join usermst SPerson	on orderdet.userfld1 = SPerson.username " +
                "WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate) " +
                "   AND ViewItem.search Like @itemname AND PointOfSale.PointOfSaleName Like @PointOfSaleName " +
                "   AND Outlet.OutletName Like @OutletName    " +
                "   AND OrderDet.IsVoided = @IsVoided " +
                "   AND OrderHdr.IsVoided = 0 " +
                //"   AND OrderDet.Quantity > 0 " +
                "   AND ViewItem.ItemDepartmentId like @DeptID " +
                "   AND ViewItem.CategoryName like @CategoryName " +
                "GROUP BY ViewItem.DepartmentName, ViewItem.CategoryName, ViewItem.ItemNo, ViewItem.ItemName,       ";
                SQL += sqlGroupAtt;
                SQL +=
                //"isnull(nullif(OrderDet.userfld1,''),UserMst.DisplayName) " +
                " case when orderdet.userfld1 is null or orderdet.userfld1=''  then usermst.displayname else  SPerson.DIsplayName end " +
                "Order By DepartmentName,CategoryName, ItemName " +
                "End ";
                /*
                DataTable dt = Pivot(SPs.FetchProductSalesReportBySalesPerson
                    (StartDate, EndDate, search, PointOfSaleName, outletName, "%", false).GetReader(),
                    "ItemNo", "DisplayName", "TotalAmount");
                */
                Logger.writeLog(SQL);
                DataTable dt = Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")),
                                "ItemNo", "DisplayName", "TotalAmount");

                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }


        public static DataTable FetchTransactionDetailWithAssistantReport
            (DateTime StartDate, DateTime EndDate,
            string search)
        {
            try
            {
                string SQL = "select ISNULL(NULLIF(b.UserFld1,''), d.SalesPersonID) as SalesPersonID, c.itemno, itemname, categoryname, " +
                                "sum(quantity) as quantity " +
                                "from OrderHdr a inner join " +
                                "OrderDet b " +
                                "on a.OrderHdrID = b.OrderHdrID " +
                                "inner join Item c " +
                                "on b.ItemNo = c.ItemNo " +
                                "inner join salescommissionrecord d " +
                                "on a.orderhdrid = d.orderhdrid " +
                                "WHERE " +
                    //"where not b.userfld1 = '' " +
                    //"and not b.userfld1 is null " +
                    //"and not b.userfld1 = d.salespersonid " +

                                "a.isvoided=0 and b.isvoided = 0 " +
                                "and orderdate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                "and orderdate < '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                "and (itemname like '%" + search + "%' or categoryname like '%" + search + "%' or c.itemno like '%" + search + "%') " +
                                "group by " +
                                "ISNULL(NULLIF(b.UserFld1,''), d.SalesPersonID), c.itemno, itemname, categoryname ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataTable dt = Pivot(SubSonic.DataService.GetReader(cmd), "ItemNo", "SalesPersonID", "Quantity");

                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        public static DataTable FetchAverageSalesAndQuantityPerTransactionReport
            (DateTime monthDate, string outletName, string SortColumn, string SortDir)
        {
            if (outletName == "")
            {
                ViewAvgTransactionCollection myViewAvgTransaction = new ViewAvgTransactionCollection();
                DateTime StartDate, EndDate;
                StartDate = new DateTime(monthDate.Year, monthDate.Month, 1);
                EndDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));
                EndDate.AddHours(24);

                myViewAvgTransaction.BetweenAnd(ViewAvgTransactionByOutlet.Columns.OrderDate, StartDate, EndDate);

                SubSonic.TableSchema.TableColumn t = ViewAvgTransaction.Schema.GetColumn(SortColumn);

                if (t != null)
                {
                    if (SortDir.Trim() == "ASC")
                    {
                        myViewAvgTransaction.OrderByAsc(SortColumn);
                    }
                    else if (SortDir.Trim() == "DESC")
                    {
                        myViewAvgTransaction.OrderByDesc(SortColumn);
                    }
                    else
                    {
                        myViewAvgTransaction.OrderByDesc("OrderDate");
                    }

                }
                else
                {
                    myViewAvgTransaction.OrderByDesc("OrderDate");
                }

                return myViewAvgTransaction.Load().ToDataTable();
            }
            else
            {

                ViewAvgTransactionByOutletCollection myViewAvgTransaction = new ViewAvgTransactionByOutletCollection();
                DateTime StartDate, EndDate;
                StartDate = new DateTime(monthDate.Year, monthDate.Month, 1);
                EndDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));
                EndDate.AddHours(24);

                myViewAvgTransaction.BetweenAnd(ViewAvgTransactionByOutlet.Columns.OrderDate, StartDate, EndDate);


                myViewAvgTransaction.Where(ViewAvgTransactionByOutlet.Columns.OutletName, SubSonic.Comparison.Like, outletName);


                SubSonic.TableSchema.TableColumn t = ViewAvgTransaction.Schema.GetColumn(SortColumn);

                if (t != null)
                {
                    if (SortDir.Trim() == "ASC")
                    {
                        myViewAvgTransaction.OrderByAsc(SortColumn);
                    }
                    else if (SortDir.Trim() == "DESC")
                    {
                        myViewAvgTransaction.OrderByDesc(SortColumn);
                    }
                    else
                    {
                        myViewAvgTransaction.OrderByDesc("OrderDate");
                    }

                }
                else
                {
                    myViewAvgTransaction.OrderByDesc("OrderDate");
                }

                return myViewAvgTransaction.Load().ToDataTable();
            }
        }



        public static DataTable
            FetchTransactionSalesCommReport(bool useStartDate, bool useEndDate,
            DateTime StartDate, DateTime EndDate, string ViewTransactionRefNo, string SalesPerson,
            int PointOfSaleID, string PointOfSaleName, string outletName, string SortColumn, string SortDir)
        {

            ViewTransactionWithSalesPersonCollection myViewTransaction = new ViewTransactionWithSalesPersonCollection();
            if (useStartDate & useEndDate)
            {
                myViewTransaction.BetweenAnd(ViewTransactionWithSalesPerson.Columns.OrderDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewTransaction.Where(ViewTransactionWithSalesPerson.Columns.OrderDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewTransaction.Where(ViewTransactionWithSalesPerson.Columns.OrderDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }
            if (ViewTransactionRefNo != "")
            {
                myViewTransaction.Where(ViewTransactionWithSalesPerson.Columns.OrderRefNo, SubSonic.Comparison.Like, ViewTransactionRefNo);
            }

            if (SalesPerson != "")
            {
                myViewTransaction.Where(ViewTransactionWithSalesPerson.Columns.UserName, SubSonic.Comparison.Like, SalesPerson);
            }
            if (outletName != "")
            {
                myViewTransaction.Where(ViewTransactionWithSalesPerson.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                myViewTransaction.Where(ViewTransactionWithSalesPerson.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    myViewTransaction.Where(ViewTransactionWithSalesPerson.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }
            SubSonic.TableSchema.TableColumn t = ViewTransactionWithSalesPerson.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewTransaction.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewTransaction.OrderByDesc(SortColumn);
                }
                else
                {
                    myViewTransaction.OrderByDesc("OrderDate");
                }

            }
            else
            {
                myViewTransaction.OrderByDesc("OrderDate");
            }

            return myViewTransaction.Load().ToDataTable();

        }

        /*
        public static DataTable FetchPurchaseOrderReport
        (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
        string ItemName, string CategoryName, string InventoryLocationID, int PointOfSaleID,
         string PointOfSaleName, string outletName, string DeptID,
        string SortColumn, string SortDir)
        {

            ViewPurchaseOrderCollection myViewPurchaseOrder = new ViewPurchaseOrderCollection();
            if (useStartDate & useEndDate)
            {
                myViewPurchaseOrder.BetweenAnd(ViewPurchaseOrder.Columns.PurchaseOrderDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewPurchaseOrder.Where(ViewPurchaseOrder.Columns.PurchaseOrderDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewPurchaseOrder.Where(ViewPurchaseOrder.Columns.PurchaseOrderDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }

            if (ItemName != "")
            {
                myViewPurchaseOrder.Where(ViewPurchaseOrder.Columns.ItemName, SubSonic.Comparison.Like, "%" + ItemName + "%");
            }

            if (CategoryName != "")
            {
                myViewPurchaseOrder.Where(ViewPurchaseOrder.Columns.CategoryName, SubSonic.Comparison.Like, CategoryName);
            }

            if (outletName != "")
            {
                myViewPurchaseOrder.Where
                    (ViewPurchaseOrder.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                myViewPurchaseOrder.Where
                    (ViewPurchaseOrder.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    myViewPurchaseOrder.Where
                        (ViewPurchaseOrder.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }

            if (DeptID != "0")
            {
                myViewPurchaseOrder.Where
                    (ViewPurchaseOrder.Columns.DepartmentID,
                    SubSonic.Comparison.Like, DeptID);
            }

            if (InventoryLocationID != "")
            {
                myViewPurchaseOrder.Where(ViewPurchaseOrder.Columns.InventoryLocationID, SubSonic.Comparison.Equals, int.Parse(InventoryLocationID));
            }
            SubSonic.TableSchema.TableColumn checkIfColumnExist = ViewPurchaseOrder.Schema.GetColumn(SortColumn);

            //Sorting
            if (checkIfColumnExist != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewPurchaseOrder.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewPurchaseOrder.OrderByDesc(SortColumn);
                }
                myViewPurchaseOrder.Load();
            }
            else
            {

                myViewPurchaseOrder.OrderByDesc(ViewPurchaseOrder.Columns.PurchaseOrderDate);
                myViewPurchaseOrder.Load();
            }

            return myViewPurchaseOrder.ToDataTable();

        }
         */

        public static DataTable FetchSpecialActivityReport
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
                string SortColumn, string SortDir)
        {

            SpecialActivityLogCollection myActivityLog = new SpecialActivityLogCollection();
            if (useStartDate & useEndDate)
            {
                myActivityLog.BetweenAnd(SpecialActivityLog.Columns.ActivityTime, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myActivityLog.Where(SpecialActivityLog.Columns.ActivityTime, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myActivityLog.Where(SpecialActivityLog.Columns.ActivityTime, SubSonic.Comparison.LessOrEquals, EndDate);
            }

            SubSonic.TableSchema.TableColumn t = SpecialActivityLog.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myActivityLog.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myActivityLog.OrderByDesc(SortColumn);
                }
            }

            return myActivityLog.Load().ToDataTable();

        }


        public static DataTable FetchCollectionReportGroupByDay
             (
              DateTime FromEndDate, DateTime ToEndDate,
              string Cashier, string supervisorId, int PointOfSaleID)
        {
            CounterCloseLogCollection myCounterCloseLog = new CounterCloseLogCollection();


            myCounterCloseLog.BetweenAnd(CounterCloseLog.Columns.EndTime, FromEndDate, ToEndDate);



            if (PointOfSaleID > 0) //<0 for all
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.PointOfSaleID, PointOfSaleID);
            }

            myCounterCloseLog.OrderByDesc(CounterCloseLog.Columns.StartTime);
            return myCounterCloseLog.Load().ToDataTable();

        }

        public static DataTable FetchCollectionReport
             (bool useFromStartDate, bool useToStartDate, DateTime FromStartDate, DateTime ToStartDate,
              bool useFromEndDate, bool useToEndDate, DateTime FromEndDate, DateTime ToEndDate,
              string CounterCloseLogRefNo, string Cashier, string supervisorId, int PointOfSaleID)
        {
            CounterCloseLogCollection myCounterCloseLog = new CounterCloseLogCollection();
            if (useFromStartDate & useToStartDate)
            {
                myCounterCloseLog.BetweenAnd(CounterCloseLog.Columns.StartTime, FromStartDate, ToStartDate);
            }
            else if (useFromStartDate)
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.StartTime, SubSonic.Comparison.GreaterOrEquals, FromStartDate);
            }
            else if (useToStartDate)
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.StartTime, SubSonic.Comparison.LessOrEquals, ToStartDate);
            }
            if (useFromEndDate & useToEndDate)
            {
                myCounterCloseLog.BetweenAnd(CounterCloseLog.Columns.EndTime, FromEndDate, ToEndDate);
            }
            else if (useFromEndDate)
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.EndTime, SubSonic.Comparison.GreaterOrEquals, FromEndDate);
            }
            else if (useToEndDate)
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.EndTime, SubSonic.Comparison.LessOrEquals, ToEndDate);
            }
            if (CounterCloseLogRefNo != "")
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.CounterCloseID, SubSonic.Comparison.Like, CounterCloseLogRefNo);
            }

            if (Cashier != "")
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.Cashier, SubSonic.Comparison.Like, Cashier);
            }

            if (supervisorId != "")
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.Supervisor, SubSonic.Comparison.Like, supervisorId);
            }
            if (PointOfSaleID > 0) //<0 for all
            {
                myCounterCloseLog.Where(CounterCloseLog.Columns.PointOfSaleID, PointOfSaleID);
            }

            myCounterCloseLog.OrderByDesc(CounterCloseLog.Columns.StartTime);
            return myCounterCloseLog.Load().ToDataTable();

        }

        public static DataTable FetchCounterCloseLogReport(bool UseFromEndDate, bool UseToEndDate,
            DateTime FromEndDate, DateTime ToEndDate, string CloseCounterReportRefNo,
            string Cashier, string SupervisorId, int PointOfSaleID, string OutletName,
            int DeptID, string SortBy, string SortDir)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = SPs.FetchClosingReport(UseFromEndDate, UseToEndDate, FromEndDate, ToEndDate,
                    CloseCounterReportRefNo, Cashier, SupervisorId, PointOfSaleID, OutletName,
                    DeptID, SortBy, SortDir).GetDataSet().Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }


        public static DataTable FetchCounterClosingReport
            (bool useFromEndDate, bool useToEndDate, DateTime FromEndDate, DateTime ToEndDate,
            string ViewCloseCounterReportRefNo, string Cashier, string supervisorId,
            int PointOfSaleID, string PointOfSaleName, string outletName, string deptID, string SortColumn, string SortDir)
        {
            string SQL = "SELECT dbo.CounterCloseLog.CounterCloseID, dbo.CounterCloseLog.StartTime, dbo.CounterCloseLog.EndTime, dbo.UserMst.DisplayName AS Cashier,  " +
                         "dbo.CounterCloseLog.TotalSystemRecorded, dbo.CounterCloseLog.TotalActualCollected, dbo.CounterCloseLog.Variance, dbo.CounterCloseLog.CashRecorded,  " +
                         "dbo.CounterCloseLog.CashCollected, dbo.CounterCloseLog.NetsRecorded, dbo.CounterCloseLog.NetsCollected, dbo.CounterCloseLog.NetsTerminalID,  " +
                         "dbo.CounterCloseLog.VisaRecorded, dbo.CounterCloseLog.VisaCollected, dbo.CounterCloseLog.VisaBatchNo, dbo.CounterCloseLog.AmexRecorded,  " +
                         "dbo.CounterCloseLog.AmexCollected, dbo.CounterCloseLog.AmexBatchNo, dbo.CounterCloseLog.ChinaNetsRecorded, dbo.CounterCloseLog.ChinaNetsCollected,  " +
                         "ISNULL(dbo.CounterCloseLog.NetsCashCardCollected,0) NetsCashCardCollected, ISNULL(dbo.CounterCloseLog.NetsCashCardRecorded,0) NetsCashCardRecorded, " +
                         "ISNULL(dbo.CounterCloseLog.NetsFlashPayCollected,0) NetsFlashPayCollected, ISNULL(dbo.CounterCloseLog.NetsFlashPayRecorded,0) NetsFlashPayRecorded, " +
                         "ISNULL(dbo.CounterCloseLog.NetsATMCardCollected,0) NetsATMCardCollected, ISNULL(dbo.CounterCloseLog.NetsATMCardRecorded,0) NetsATMCardRecorded, " +
                         "dbo.CounterCloseLog.ChinaNetsTerminalID,  " +
                         "dbo.CounterCloseLog.userfld3 AS Payment5Recorded,dbo.CounterCloseLog.userfld4 AS Payment5Collected, " +
                         "dbo.CounterCloseLog.userfld5 AS Payment6Recorded,dbo.CounterCloseLog.userfld6 AS Payment6Collected, " +
                         "dbo.CounterCloseLog.VoucherRecorded, dbo.CounterCloseLog.VoucherCollected,  " +
                         "dbo.CounterCloseLog.userfloat1 AS ChequeRecorded, dbo.CounterCloseLog.userfloat2 AS ChequeCollected, dbo.CounterCloseLog.ClosingCashOut,  " +
                         "dbo.CounterCloseLog.userfloat6 AS SMFRecorded, dbo.CounterCloseLog.userfloat7 AS SMFCollected, " +
                         "dbo.CounterCloseLog.userfloat8 AS PAMedRecorded, dbo.CounterCloseLog.userfloat9 AS PAMedCollected, " +
                         "dbo.CounterCloseLog.userfloat10 AS PWFRecorded, dbo.CounterCloseLog.userfloat11 AS PWFCollected, " +
                         "dbo.CounterCloseLog.DepositBagNo, UserMst_1.DisplayName AS Supervisor, dbo.CounterCloseLog.OpeningBalance, dbo.CounterCloseLog.CashIn,  " +
                         "dbo.CounterCloseLog.CashOut, dbo.CounterCloseLog.PointOfSaleID, dbo.PointOfSale.PointOfSaleName, dbo.CounterCloseLog.FloatBalance,  " +
                         "dbo.PointOfSale.DepartmentID, dbo.PointOfSale.OutletName, " +
                         "TotalGST = isnull(( " +
                         "    select SUM(a.GstAmount) " +
                         "      from OrderHdr a " +
                         "     where a.OrderDate between starttime and endtime " +
                         "  ), 0.00) " +
                         "FROM dbo.CounterCloseLog INNER JOIN " +
                         "dbo.PointOfSale ON dbo.CounterCloseLog.PointOfSaleID = dbo.PointOfSale.PointOfSaleID INNER JOIN " +
                         "dbo.UserMst ON dbo.CounterCloseLog.Cashier = dbo.UserMst.UserName INNER JOIN " +
                         "dbo.UserMst AS UserMst_1 ON dbo.CounterCloseLog.Supervisor = UserMst_1.UserName WHERE 1=1 ";

            /*
            ViewCloseCounterReportCollection myViewCloseCounterReport = new ViewCloseCounterReportCollection();
            
            */
            if (outletName == "ALL")
            {
                outletName = "%";
            }

            if (outletName == "ALL - Breakdown")
            {
                outletName = "%";
                PointOfSaleID = 0;
                PointOfSaleName = "%";
            }
            if (useFromEndDate & useToEndDate)
            {
                SQL += " AND endtime >= '" + FromEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND endtime <= '" + ToEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            }
            else if (useFromEndDate)
            {
                SQL += " AND endtime >= '" + FromEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            }
            else if (useToEndDate)
            {
                SQL += "' AND endtime <= '" + ToEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }
            if (ViewCloseCounterReportRefNo != "")
            {
                SQL += " AND CounterCloseID like '%" + ViewCloseCounterReportRefNo + "%' ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.CounterCloseID, SubSonic.Comparison.Like, ViewCloseCounterReportRefNo);
            }

            if (Cashier != "")
            {
                SQL += " AND Cashier like '%" + Cashier + "%' ";

                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.Cashier, SubSonic.Comparison.Like, Cashier);
            }

            if (supervisorId != "")
            {
                SQL += " AND Supervisor like '%" + supervisorId + "%' ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.Supervisor, SubSonic.Comparison.Like, supervisorId);
            }
            if (outletName != "")
            {
                SQL += " AND OutletName like '%" + outletName + "%' ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                SQL += " AND PointOfSale.PointOfSaleID = " + PointOfSaleID + " ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    SQL += " AND PointOfSaleName like '%" + PointOfSaleName + "%' ";
                    //myViewCloseCounterReport.Where(ViewTransaction.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }
            if (deptID != "0")
            {
                SQL += " AND DepartmentID like '%" + deptID + "%' ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.DepartmentID, SubSonic.Comparison.Like, deptID);
            }

            SubSonic.TableSchema.TableColumn t = ViewCloseCounterReport.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                SQL += " order by " + SortColumn + " " + SortDir.Trim();
                /*
                if (SortDir.Trim() == "ASC")
                {
                    
                    //myViewCloseCounterReport.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewCloseCounterReport.OrderByDesc(SortColumn);
                }
                 */
            }
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            return DataService.GetDataSet(cmd).Tables[0];
            //return myViewCloseCounterReport.Load().ToDataTable();

        }

        public static DataTable FetchCounterClosingReportByCounterCloseID
            (string CounterCloseID)
        {
            string SQL = "SELECT dbo.CounterCloseLog.CounterCloseID, dbo.CounterCloseLog.StartTime, dbo.CounterCloseLog.EndTime, dbo.UserMst.DisplayName AS Cashier,  " +
                         "dbo.CounterCloseLog.TotalSystemRecorded, dbo.CounterCloseLog.TotalActualCollected, dbo.CounterCloseLog.Variance, dbo.CounterCloseLog.CashRecorded,  " +
                         "dbo.CounterCloseLog.CashCollected, dbo.CounterCloseLog.NetsRecorded, dbo.CounterCloseLog.NetsCollected, dbo.CounterCloseLog.NetsTerminalID,  " +
                         "dbo.CounterCloseLog.VisaRecorded, dbo.CounterCloseLog.VisaCollected, dbo.CounterCloseLog.VisaBatchNo, dbo.CounterCloseLog.AmexRecorded,  " +
                         "dbo.CounterCloseLog.AmexCollected, dbo.CounterCloseLog.AmexBatchNo, dbo.CounterCloseLog.ChinaNetsRecorded, dbo.CounterCloseLog.ChinaNetsCollected,  " +
                         "dbo.CounterCloseLog.ChinaNetsTerminalID,  " +
                         "cast(dbo.CounterCloseLog.userfld3 as numeric(18,2)) AS Payment5Recorded, cast(dbo.CounterCloseLog.userfld4 as numeric(18,2)) AS Payment5Collected, " +
                         "cast(dbo.CounterCloseLog.userfld5 as numeric(18,2)) AS Payment6Recorded, cast(dbo.CounterCloseLog.userfld6 as numeric(18,2)) AS Payment6Collected, " +
                         "ISNULL(dbo.CounterCloseLog.Pay7Recorded,0) as Pay7Recorded, ISNULL(dbo.CounterCloseLog.Pay7Collected,0) as Pay7Collected, " +
                         "ISNULL(dbo.CounterCloseLog.Pay8Recorded,0) as Pay8Recorded, ISNULL(dbo.CounterCloseLog.Pay8Collected,0) as Pay8Collected," +
                         "ISNULL(dbo.CounterCloseLog.Pay9Recorded,0) as Pay9Recorded, ISNULL(dbo.CounterCloseLog.Pay9Collected,0) as Pay9Collected, " +
                         "ISNULL(dbo.CounterCloseLog.Pay10Recorded,0) as Pay10Recorded, ISNULL(dbo.CounterCloseLog.Pay10Collected,0) as Pay10Collected," +
                         "dbo.CounterCloseLog.VoucherRecorded, dbo.CounterCloseLog.VoucherCollected,  " +
                         "dbo.CounterCloseLog.userfloat1 AS ChequeRecorded, dbo.CounterCloseLog.userfloat2 AS ChequeCollected, dbo.CounterCloseLog.ClosingCashOut,  " +
                         "dbo.CounterCloseLog.DepositBagNo, UserMst_1.DisplayName AS Supervisor, dbo.CounterCloseLog.OpeningBalance, dbo.CounterCloseLog.CashIn,  " +
                         "dbo.CounterCloseLog.CashOut, dbo.CounterCloseLog.PointOfSaleID, dbo.PointOfSale.PointOfSaleName, dbo.CounterCloseLog.FloatBalance,  " +
                         "dbo.PointOfSale.DepartmentID, dbo.PointOfSale.OutletName, dbo.CounterCloseLog.userfloat3, dbo.CounterCloseLog.userfloat4, dbo.CounterCloseLog.userfloat5, dbo.CounterCloseLog.Userfld9, " +
                         "dbo.CounterCloseLog.userfloat6, dbo.CounterCloseLog.userfloat7, dbo.CounterCloseLog.userfloat8, dbo.CounterCloseLog.userfloat9, dbo.CounterCloseLog.userfloat10, " +
                         "dbo.CounterCloseLog.userfloat11, dbo.CounterCloseLog.userfloat12, dbo.CounterCloseLog.userfloat13, dbo.CounterCloseLog.userfloat14, dbo.CounterCloseLog.userfloat15, " +
                         "dbo.CounterCloseLog.userint1, dbo.CounterCloseLog.userint2, dbo.CounterCloseLog.userint3, dbo.CounterCloseLog.userint4, dbo.CounterCloseLog.userint5, " +
                         "COUNT(OH.OrderHdrID) as TotalTransaction, SUM(GSTAmount) as TotalGST " +
                         "FROM dbo.CounterCloseLog INNER JOIN " +
                         "dbo.PointOfSale ON dbo.CounterCloseLog.PointOfSaleID = dbo.PointOfSale.PointOfSaleID INNER JOIN " +
                         "dbo.UserMst ON dbo.CounterCloseLog.Cashier = dbo.UserMst.UserName INNER JOIN " +
                         "dbo.UserMst AS UserMst_1 ON dbo.CounterCloseLog.Supervisor = UserMst_1.UserName LEFT JOIN " +
                         "dbo.OrderHdr AS OH on OH.PointofSaleID =  dbo.CounterCloseLog.PointOfSaleID and OH.IsVoided = 0 and OH.OrderDate between dbo.CounterCloseLog.StartTime and dbo.CounterCloseLog.EndTime " +
                         "WHERE CounterCloseID = '" + CounterCloseID + "' " +
                         "GROUP BY dbo.CounterCloseLog.CounterCloseID, dbo.CounterCloseLog.StartTime, dbo.CounterCloseLog.EndTime, dbo.UserMst.DisplayName, " +
                        "dbo.CounterCloseLog.TotalSystemRecorded, dbo.CounterCloseLog.TotalActualCollected, dbo.CounterCloseLog.Variance, dbo.CounterCloseLog.CashRecorded, " +
                        "dbo.CounterCloseLog.CashCollected, dbo.CounterCloseLog.NetsRecorded, dbo.CounterCloseLog.NetsCollected, dbo.CounterCloseLog.NetsTerminalID, " +
                        "dbo.CounterCloseLog.VisaRecorded, dbo.CounterCloseLog.VisaCollected, dbo.CounterCloseLog.VisaBatchNo, dbo.CounterCloseLog.AmexRecorded, " +
                        "dbo.CounterCloseLog.AmexCollected, dbo.CounterCloseLog.AmexBatchNo, dbo.CounterCloseLog.ChinaNetsRecorded, dbo.CounterCloseLog.ChinaNetsCollected, " +
                        "dbo.CounterCloseLog.ChinaNetsTerminalID, dbo.CounterCloseLog.userfld3, dbo.CounterCloseLog.userfld4, dbo.CounterCloseLog.userfld5 ,dbo.CounterCloseLog.userfld6, " +
                        "dbo.CounterCloseLog.VoucherRecorded, dbo.CounterCloseLog.VoucherCollected, dbo.CounterCloseLog.userfloat1 , dbo.CounterCloseLog.userfloat2, dbo.CounterCloseLog.ClosingCashOut, " +
                        "dbo.CounterCloseLog.DepositBagNo, UserMst_1.DisplayName , dbo.CounterCloseLog.OpeningBalance, dbo.CounterCloseLog.CashIn, " +
                        "dbo.CounterCloseLog.CashOut, dbo.CounterCloseLog.PointOfSaleID, dbo.PointOfSale.PointOfSaleName, dbo.CounterCloseLog.FloatBalance, " +
                        "dbo.PointOfSale.DepartmentID, dbo.PointOfSale.OutletName, dbo.CounterCloseLog.userfloat3, dbo.CounterCloseLog.userfloat4, dbo.CounterCloseLog.userfloat5, dbo.CounterCloseLog.Userfld9, " +
                        "dbo.CounterCloseLog.userfloat6, dbo.CounterCloseLog.userfloat7, dbo.CounterCloseLog.userfloat8, dbo.CounterCloseLog.userfloat9, dbo.CounterCloseLog.userfloat10, " +
                        "dbo.CounterCloseLog.userfloat11, dbo.CounterCloseLog.userfloat12, dbo.CounterCloseLog.userfloat13, dbo.CounterCloseLog.userfloat14, dbo.CounterCloseLog.userfloat15, " +
                        "dbo.CounterCloseLog.Pay7Recorded, dbo.CounterCloseLog.Pay7Collected, dbo.CounterCloseLog.Pay8Recorded, dbo.CounterCloseLog.Pay8Collected, " +
                        "dbo.CounterCloseLog.Pay9Recorded, dbo.CounterCloseLog.Pay9Collected, dbo.CounterCloseLog.Pay10Recorded, dbo.CounterCloseLog.Pay10Collected, " +
                        "dbo.CounterCloseLog.userint1, dbo.CounterCloseLog.userint2, dbo.CounterCloseLog.userint3, dbo.CounterCloseLog.userint4, dbo.CounterCloseLog.userint5";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            return DataService.GetDataSet(cmd).Tables[0];
            //return myViewCloseCounterReport.Load().ToDataTable();

        }

        public static List<DateTime> FetchCounterClosingReport_DateOnly
            (bool useFromEndDate, bool useToEndDate, DateTime FromEndDate, DateTime ToEndDate,
            string ViewCloseCounterReportRefNo, string Cashier, string supervisorId,
            int PointOfSaleID, string PointOfSaleName, string outletName, string deptID, string SortColumn, string SortDir)
        {
            string SQL =
                "SELECT " +
                    "CASE " +
                        "WHEN (DATEDIFF(DD,StartTime,EndTime) = 0) THEN CAST(EndTime AS DATE) " +
                        "WHEN ((DATEDIFF(HH,StartTime,DATEADD(D,1,CAST(StartTime AS DATE))) < DATEDIFF(HH,CAST(EndTime AS DATE),EndTime))) THEN CAST(EndTime AS DATE) " +
                        "ELSE DATEADD(D, -1, CAST(EndTime AS DATE)) END SalesDate " +
                "FROM CounterCloseLog INNER JOIN " +
                    "PointOfSale ON CounterCloseLog.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
                    "UserMst ON CounterCloseLog.Cashier = UserMst.UserName INNER JOIN " +
                    "UserMst AS UserMst_1 ON CounterCloseLog.Supervisor = UserMst_1.UserName WHERE 1=1 ";

            /*
            ViewCloseCounterReportCollection myViewCloseCounterReport = new ViewCloseCounterReportCollection();
            
            */
            if (outletName == "ALL")
            {
                outletName = "%";
            }

            if (outletName == "ALL - Breakdown")
            {
                outletName = "%";
                PointOfSaleID = 0;
                PointOfSaleName = "%";
            }
            if (useFromEndDate & useToEndDate)
            {
                SQL += " AND endtime >= '" + FromEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND endtime <= '" + ToEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            }
            else if (useFromEndDate)
            {
                SQL += " AND endtime >= '" + FromEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            }
            else if (useToEndDate)
            {
                SQL += "' AND endtime <= '" + ToEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }
            if (ViewCloseCounterReportRefNo != "")
            {
                SQL += " AND CounterCloseID like '%" + ViewCloseCounterReportRefNo + "%' ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.CounterCloseID, SubSonic.Comparison.Like, ViewCloseCounterReportRefNo);
            }

            if (Cashier != "")
            {
                SQL += " AND Cashier like '%" + Cashier + "%' ";

                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.Cashier, SubSonic.Comparison.Like, Cashier);
            }

            if (supervisorId != "")
            {
                SQL += " AND Supervisor like '%" + supervisorId + "%' ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.Supervisor, SubSonic.Comparison.Like, supervisorId);
            }
            if (outletName != "")
            {
                SQL += " AND OutletName like '%" + outletName + "%' ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                SQL += " AND PointOfSale.PointOfSaleID = " + PointOfSaleID + " ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    SQL += " AND PointOfSaleName like '%" + PointOfSaleName + "%' ";
                    //myViewCloseCounterReport.Where(ViewTransaction.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }
            if (deptID != "0")
            {
                SQL += " AND DepartmentID like '%" + deptID + "%' ";
                //myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.DepartmentID, SubSonic.Comparison.Like, deptID);
            }

            SubSonic.TableSchema.TableColumn t = ViewCloseCounterReport.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                SQL += " order by " + SortColumn + " " + SortDir.Trim();
                /*
                if (SortDir.Trim() == "ASC")
                {
                    
                    //myViewCloseCounterReport.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewCloseCounterReport.OrderByDesc(SortColumn);
                }
                 */
            }
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            DataTable Result = DataService.GetDataSet(cmd).Tables[0];

            List<DateTime> allClosingTime = new List<DateTime>();
            for (int Counter = 0; Counter < Result.Rows.Count; Counter++)
            {
                DataRow Rw = Result.Rows[Counter];
                DateTime currSalesDate = DateTime.Parse(Rw["SalesDate"].ToString());
                if (!allClosingTime.Contains(currSalesDate))
                    allClosingTime.Add(currSalesDate);
            }

            return allClosingTime;
            //return myViewCloseCounterReport.Load().ToDataTable();

        }

        public static DataTable FetchMonthlyAverageReport
            (int OrderYear, int OrderMonth,
            string SortColumn, string SortDir)
        {

            ViewMonthlyAvgTransactionCollection myViewMonthlyAvgTransaction
                = new ViewMonthlyAvgTransactionCollection();


            myViewMonthlyAvgTransaction.Where
                 (ViewMonthlyAvgTransaction.Columns.OrderYear,
                  OrderYear);

            myViewMonthlyAvgTransaction.Where
                 (ViewMonthlyAvgTransaction.Columns.Ordermonth,
                  OrderMonth);

            SubSonic.TableSchema.TableColumn t = ViewMonthlyAvgTransaction.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMonthlyAvgTransaction.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMonthlyAvgTransaction.OrderByDesc(SortColumn);
                }
            }

            return myViewMonthlyAvgTransaction.Load().ToDataTable();

        }

        public static DataTable FetchLoginReport
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate, string LoginType, string UserName, int PointOfSaleID,
            string PointOfSaleName, string outletName, string deptID, string SortColumn, string SortDir)
        {

            ViewLoginActivityReportCollection myViewLoginActivityReport = new ViewLoginActivityReportCollection();
            if (useStartDate & useEndDate)
            {
                myViewLoginActivityReport.BetweenAnd(ViewLoginActivityReport.Columns.LoginDateTime, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewLoginActivityReport.Where(ViewLoginActivityReport.Columns.LoginDateTime, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewLoginActivityReport.Where(ViewLoginActivityReport.Columns.LoginDateTime, SubSonic.Comparison.LessOrEquals, EndDate);
            }
            if (LoginType != "")
            {
                myViewLoginActivityReport.Where(ViewLoginActivityReport.Columns.LoginType, SubSonic.Comparison.Like, LoginType);
            }

            if (UserName != "")
            {
                myViewLoginActivityReport.Where(ViewLoginActivityReport.Columns.UserName, SubSonic.Comparison.Like, UserName);
            }
            if (outletName != "")
            {
                myViewLoginActivityReport.Where(ViewLoginActivityReport.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                myViewLoginActivityReport.Where(ViewLoginActivityReport.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    myViewLoginActivityReport.Where(ViewLoginActivityReport.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }
            if (deptID != "0")
            {
                myViewLoginActivityReport.Where(ViewLoginActivityReport.Columns.DepartmentId, SubSonic.Comparison.Like, deptID);
            }


            SubSonic.TableSchema.TableColumn t = ViewLoginActivityReport.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewLoginActivityReport.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewLoginActivityReport.OrderByDesc(SortColumn);
                }
            }
            else
            {
                myViewLoginActivityReport.OrderByDesc(ViewLoginActivityReport.Columns.LoginDateTime);
            }
            return myViewLoginActivityReport.Load().ToDataTable();

        }

        public static DataTable FetchGSTReport(
                 DateTime startdate,
                 DateTime enddate
             )
        {

            DataSet ds = SPs.FetchGSTReport(startdate, enddate).GetDataSet();

            //Calculate
            OrderHdrCollection hdr = new OrderHdrCollection();
            hdr.Where("OrderDate", Comparison.GreaterOrEquals, startdate);
            hdr.Where("OrderDate", Comparison.LessOrEquals, enddate);
            hdr.Load();

            InventoryDetCollection invDet;
            double GST = 0;
            //For each order get the inventory and get the stock in inventory
            for (int i = 0; i < hdr.Count; i++)
            {
                invDet = (new InventoryHdr(hdr[i].InventoryHdrRefNo)).InventoryDetRecords();
                for (int k = 0; k < invDet.Count; k++)
                {
                    GST += (double)invDet[k].Quantity * (double)invDet[k].FactoryPrice * invDet[k].Gst / 100;
                }
            }
            ds.Tables[0].Rows[0]["GSTPaid"] = GST.ToString("N2");

            return ds.Tables[0];
        }

        public static DataTable FetchProductSalesReportByMonth
            (DateTime startDate, DateTime endDate, string search, string PointOfSaleName)
        {
            try
            {
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
                endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
                if (PointOfSaleName == "ALL") PointOfSaleName = "";
                //add attributes
                AttributesLabelCollection atCol = new AttributesLabelCollection();
                atCol.Load();
                string sqlAtt = "";
                string sqlGroupAtt = "";
                foreach (PowerPOS.AttributesLabel a in atCol)
                {
                    if (!String.IsNullOrEmpty(a.Label))
                    {
                        sqlAtt += "Item.Attributes" + a.AttributesNo.ToString() + " as [" + a.Label + "], ";
                        sqlGroupAtt += "Item.Attributes" + a.AttributesNo.ToString() + ", ";
                    }
                }


                string SQL = "SELECT  orderdet.itemno as [Item No],itemname as [Item Name], ";
                SQL += sqlAtt;
                SQL += "cast(Year(orderdate) as varchar) + '_' + cast(Month(orderdate)as varchar) as salesdate, Sum(amount) as Amount    " +
                                "FROM orderhdr   " +
                                "inner join PointOfSale  " +
                                "on orderhdr.PointOfSaleID = PointOfSale.PointOfSaleID " +
                                "inner join OrderDet on OrderHdr.OrderHdrID = orderdet.OrderHdrID " +
                                "inner join Item on orderdet.ItemNo = item.ItemNo  " +
                                "where orderhdr.isvoided=0 and orderdet.IsVoided=0 and orderdet.itemno <> 'inst_payment' " + //exclude installment
                                "and orderdate >= '" + startDate.ToString("yyyy-MM-dd") + "' and orderdate <= '" + endDate.AddSeconds(86359).ToString("yyyy-MM-dd HH:mm:ss") + "'   " +
                                "and PointOfSaleName like '%" + PointOfSaleName + "%' " +
                                "and OrderDet.ItemNo + ' ' +  ItemName + ' ' +  Barcode + ' ' +  CategoryName + ' ' +  isnull(Attributes1,'') + ' ' +  isnull(Attributes2,'') + ' ' +  isnull(Attributes3,'') + ' ' +  isnull(Attributes4,'') + ' ' +  isnull(Attributes5,'') like '%" + search + "%' " +
                                "GROUP BY orderdet.ItemNo,ItemName, " + sqlGroupAtt + " Year(orderdate), Month(orderdate)   " +
                                "Order by orderdet.ItemNo,ItemName, " + sqlGroupAtt + " Year(orderdate), Month(orderdate)  ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataTable dt = Pivot(DataService.GetReader(cmd), "Item No", "salesdate", "Amount");
                //Rename the columns name
                char[] separator = { '_' };
                int tmpMth = 0;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string[] splitted = dt.Columns[i].ColumnName.Split(separator);
                    if (splitted.Length >= 2 &&
                        int.TryParse(splitted[1], out tmpMth) &&
                        tmpMth >= 1 && tmpMth <= 12)
                    {
                        dt.Columns[i].ColumnName = (new DateTime(1900, tmpMth, 1)).ToString("MMM") + "_" + splitted[0];
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        public static DataTable FetchProductSalesReportByMonth_Qty
          (DateTime startDate, DateTime endDate, string search, string PointOfSaleName)
        {
            try
            {
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
                endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
                if (PointOfSaleName == "ALL") PointOfSaleName = "";

                string SQL =
                                "SELECT * FROM " +
                                "( " +
                                "SELECT C.CategoryName AS [Category Name], C.ItemNo as [Item No], C.ItemName as [Item Name], ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, '_Bal Qty' AS salesdate, SUM(CASE WHEN MovementType LIKE '% In' THEN Quantity ELSE 0 - Quantity END) Amount " +
                                "FROM InventoryDet A " +
                                    "INNER JOIN InventoryHdr B ON A.InventoryHdrRefNo = B.InventoryHdrRefNo  " +
                                    "INNER JOIN Item C ON A.ItemNo = C.ItemNo " +
                                "GROUP BY C.CategoryName, C.ItemNo, C.ItemName, ISNULL(Attributes1,''), ISNULL(Attributes2,'') " +
                                "UNION ALL " +
                                "SELECT Item.CategoryName AS [Category Name], orderdet.itemno as [Item No],itemname as [Item Name], ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, " +
                                    "cast(Year(orderdate) as varchar) + '_' + cast(Month(orderdate)as varchar) as salesdate, Sum(Quantity) as Amount    " +
                                "FROM orderhdr   " +
                                    "inner join PointOfSale  " +
                                        "on orderhdr.PointOfSaleID = PointOfSale.PointOfSaleID " +
                                    "inner join OrderDet on OrderHdr.OrderHdrID = orderdet.OrderHdrID " +
                                    "inner join Item on orderdet.ItemNo = item.ItemNo  " +
                                "where orderhdr.isvoided=0 and orderdet.IsVoided=0 and orderdet.itemno <> 'inst_payment'  " + //exclude installment
                                    "and orderdate >= '" + startDate.ToString("yyyy-MM-dd") + "' and orderdate <= '" + endDate.AddSeconds(86359).ToString("yyyy-MM-dd HH:mm:ss") + "'   " +
                                    "and PointOfSaleName like '%" + PointOfSaleName + "%' " +
                                    "and OrderDet.ItemNo + ' ' +  ItemName + ' ' +  Barcode + ' ' +  CategoryName + ' ' +  isnull(Attributes1,'') + ' ' +  isnull(Attributes2,'') + ' ' +  isnull(Attributes3,'') + ' ' +  isnull(Attributes4,'') + ' ' +  isnull(Attributes5,'') like '%" + search + "%' " +
                                "GROUP BY Item.CategoryName, orderdet.ItemNo,ItemName, ISNULL(Attributes1,''), ISNULL(Attributes2,''),Year(orderdate), Month(orderdate)   " +
                                ") A " +
                                "Order by [Category Name], [Item No],[Item Name],salesdate ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataTable dt = Pivot(DataService.GetReader(cmd), "Item No", "salesdate", "Amount");
                //Rename the columns name
                char[] separator = { '_' };
                int tmpMth = 0;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "_Bal Qty")
                    {
                        dt.Columns[i].ColumnName = "Bal Qty";
                    }
                    else
                    {
                        string[] splitted = dt.Columns[i].ColumnName.Split(separator);
                        if (splitted.Length >= 2 &&
                            int.TryParse(splitted[1], out tmpMth) &&
                            tmpMth >= 1 && tmpMth <= 12)
                        {
                            dt.Columns[i].ColumnName = (new DateTime(1900, tmpMth, 1)).ToString("MMM") + "_" + splitted[0];
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchProductSalesReportByDate
            (DateTime startDate, DateTime endDate, string search, string PointOfSaleName)
        {
            try
            {
                if (PointOfSaleName == "ALL") PointOfSaleName = "";

                //add attributes
                AttributesLabelCollection atCol = new AttributesLabelCollection();
                atCol.Load();
                string sqlAtt = "";
                string sqlGroupAtt = "";
                foreach (PowerPOS.AttributesLabel a in atCol)
                {
                    if (!String.IsNullOrEmpty(a.Label))
                    {
                        sqlAtt += "Item.Attributes" + a.AttributesNo.ToString() + " as [" + a.Label + "], ";
                        sqlGroupAtt += "Item.Attributes" + a.AttributesNo.ToString() + ", ";
                    }
                }

                string SQL = "SELECT  orderdet.itemno as [Item No],itemname as [Item Name], ";
                SQL += sqlAtt;
                SQL += "RIGHT('00' + (cast(Month(orderdate) as varchar)),2) + '_' + RIGHT('00' + (cast(Day(orderdate)as varchar)),2) +  " +
                                 "'_' + RIGHT('0000' + (cast(Year(orderdate) as varchar)),4) as salesdate, Sum(amount) as Amount    " +
                                 "FROM orderhdr   " +
                                 "inner join PointOfSale  " +
                                 "on orderhdr.PointOfSaleID = PointOfSale.PointOfSaleID " +
                                 "inner join OrderDet on OrderHdr.OrderHdrID = orderdet.OrderHdrID " +
                                 "inner join Item on orderdet.ItemNo = item.ItemNo  " +
                                 "where orderhdr.isvoided=0 and orderdet.IsVoided=0 and orderdet.itemno <> 'inst_payment' " + //exclude installment
                                 "and orderdate >= '" + startDate.ToString("yyyy-MM-dd") + "' and orderdate <= '" + endDate.AddSeconds(86359).ToString("yyyy-MM-dd HH:mm:ss") + "'   " +
                                 "and PointOfSaleName like '%" + PointOfSaleName + "%' " +
                                 "and OrderDet.ItemNo + ' ' +  ItemName + ' ' +  Barcode + ' ' +  CategoryName + ' ' +  isnull(Attributes1,'') + ' ' +  isnull(Attributes2,'') + ' ' +  isnull(Attributes3,'') + ' ' +  isnull(Attributes4,'') + ' ' +  isnull(Attributes5,'') like '%" + search + "%' " +
                                 "GROUP BY orderdet.ItemNo,ItemName, " +
                                 sqlGroupAtt + " RIGHT('0000' + (cast(Year(orderdate) as varchar)),4), RIGHT('00' + (cast(Month(orderdate)as varchar)),2), RIGHT('00' + (cast(Day(orderdate) as varchar)),2)   " +
                                 "Order by orderdet.ItemNo,ItemName, " + sqlGroupAtt + " RIGHT('0000' + (cast(Year(orderdate) as varchar)),4), RIGHT('00' + (cast(Month(orderdate)as varchar)),2), RIGHT('00' + (cast(Day(orderdate) as varchar)),2)  ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataTable dt = Pivot(DataService.GetReader(cmd), "Item No", "salesdate", "Amount");
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchHourlyTransactionReport(DateTime startDate, DateTime endDate, string PointOfSaleName)
        {
            try
            {
                if (PointOfSaleName == "ALL") PointOfSaleName = "";

                string SQL = "SELECT  RIGHT('00' + (cast(Day(orderdate) as varchar)),2) + '/' + RIGHT('00' + (cast(Month(orderdate)as varchar)),2) +  " +
                "'/' + RIGHT('0000' + (cast(Year(orderdate) as varchar)),4) as [Sales Date], " +
                "RIGHT('00' + (cast(DATEPART(hh, OrderDate) as varchar)),2) + '00' as saleshour, Sum(nettamount) as salesamount  " +
                "FROM orderhdr inner join pointofsale on orderhdr.pointofsaleid = pointofsale.pointofsaleid " +
                "where isvoided=0 " +
                "and orderdate >= '" + startDate.ToString("yyyy-MM-dd") + "' and orderdate <= '" + endDate.AddSeconds(86359).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "and pointofsaleName like '%" + PointOfSaleName + "%' " +
                "GROUP BY RIGHT('0000' + (cast(Year(orderdate) as varchar)),4), RIGHT('00' + (cast(Month(orderdate)as varchar)),2), RIGHT('00' + (cast(Day(orderdate) as varchar)),2),RIGHT('00' + (cast(DATEPART(hh, OrderDate) as varchar)),2) + '00' " +
                 "ORDER BY RIGHT('0000' + (cast(Year(orderdate) as varchar)),4), RIGHT('00' + (cast(Month(orderdate)as varchar)),2), RIGHT('00' + (cast(Day(orderdate) as varchar)),2),RIGHT('00' + (cast(DATEPART(hh, OrderDate) as varchar)),2) + '00' ";
                //"Order by Year(orderdate), Month(orderdate), Day(orderdate),DATEPART(hh, OrderDate) ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataTable dt = Pivot(DataService.GetReader(cmd), "sales date", "saleshour", "SalesAmount");
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        public static DataTable FetchCommissionReport(DateTime startDate, DateTime endDate)
        {

            try
            {
                string SQL;
                SQL = "declare @startDate as datetime; " +
                        "declare @endDate as datetime; " +
                        "set @startDate = '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                        "set @endDate = '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                        "select username, isnull(nettamount,0)  as salesamount, " +
                        "ISNULL(lineAmount, 0) as lineAmount,  " +
                        "isnull(nettamount,0) + ISNULL(lineAmount, 0) as totalamount " +
                        "from " +
                        "(select username from usermst where isAsalesperson=1 and deleted=0) Z " +
                        "LEFT OUTER JOIN " +
                        "(select SUM(nettamount) as nettamount, " +
                        "SalesPersonID from SalesCommissionRecord a inner join  " +
                        "OrderHdr b on " +
                        "a.OrderHdrID = b.OrderHdrID " +
                        "where b.IsVoided=0 " +
                        "and OrderDate > @startDate and OrderDate < @endDate " +
                        "group by SalesPersonID) y " +
                        "on z.UserName = y.salespersonID " +
                        "LEFT OUTER JOIN( " +
                        "select SUM(amount) as lineAmount, c.salespersonid from  " +
                        "OrderHdr a " +
                        "inner join  " +
                        "OrderDet b " +
                        "on a.OrderHdrID = b.OrderHdrID " +
                        "inner join SalesCommissionRecord c " +
                        "on c.OrderHdrID = b.OrderHdrID " +
                        "where a.IsVoided=0 and b.IsVoided = 0 and b.itemno <> 'inst_payment' " + //exclude installment
                        "and giveCommission=1 " +
                        "and not c.SalesPersonID = b.userfld1  " +
                        "and OrderDate > @startDate and OrderDate < @endDate " +
                        "group by SalesPersonID) X " +
                        "on X.SalesPersonID = Y.SalesPersonID; ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                return DataService.GetDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        public static DataTable FetchLineCommissionByProfitReport(DateTime startDate, DateTime endDate)
        {

            try
            {
                string SQL;
                SQL = "declare @startDate as datetime;  " +
                        "declare @endDate as datetime;  " +
                        "set @startDate = '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "';  " +
                        "set @endDate = '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "';  " +
                        "select username, username AS SalesPersonID, DisplayName AS SalesPersonName " +
                            ", ISNULL(lineAmount, 0) as SalesAmount,ISNULL(cogs,0) as CostAmount " +
                            ", ISNULL(lineAmount, 0) as totalamount,ISNULL(cogs,0) as cogs ,isnull(profit,0) as profit " +
                        "from (select username, DisplayName from usermst where isAsalesperson=1 and deleted=0) Z   " +
                        "LEFT OUTER JOIN(   " +
                        "Select isnull(Nullif(a.userfld1,''),c.salespersonid)  as salespersonid,    " +
                        "sum(a.amount) as lineamount, SUM(e.cogs) as cogs, sum(a.amount) - sum(e.cogs) as profit  " +
                        "from OrderDet a inner join OrderHdr b    " +
                        "on a.OrderHdrID = b.OrderHdrID    " +
                        "left outer join SalesCommissionRecord c    " +
                        "on a.OrderHdrID = c.OrderHdrID    " +
                        "inner join item d    " +
                        "on a.itemno = d.itemno    " +
                        "inner join  " +
                        "(select t.InventoryHdrRefNo, SUM(isnull(t.CostOfGoods,0)) as cogs  from " +
                        "InventoryHdr w " +
                        "inner join InventoryDet t " +
                        "on t.InventoryHdrRefNo = w.InventoryHdrRefNo " +
                        "group by t.InventoryHdrRefNo) e " +
                        "on e.InventoryHdrRefNo = a.InventoryHdrRefNo " +
                        "where a.IsVoided=0 and b.IsVoided = 0    " +
                        "and OrderDate >= @startDate and   " +
                        "OrderDetDate <= @endDate and   " +
                        "d.iscommission = 1    " +
                        "group by isnull(Nullif(a.userfld1,''),c.salespersonid)) X   " +
                        "on X.SalesPersonID = Z.UserName  ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                return DataService.GetDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchLineCommissionReport(DateTime startDate, DateTime endDate)
        {
            /*
            DataTable dt = SPs.StylistCommissionReport(startDate, endDate).GetDataSet().Tables[0];
            return dt;
            */
            try
            {
                #region -= OBSOLETED =-
                string SQL;
                SQL = "declare @startDate as datetime;  " +
                        "declare @endDate as datetime;  " +
                        "set @startDate = '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "';  " +
                        "set @endDate = '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "';  " +
                        "select username, isnull(servicesalesamount,0) as service,  isnull(productsalesamount,0) as product, ISNULL(lineAmount, 0) as totalamount " +
                        "from (select username from usermst where isAsalesperson=1 and deleted=0) Z " +
                        "LEFT OUTER JOIN( " +
                        "Select isnull(Nullif(a.userfld1,''),c.salespersonid)  as salespersonid,  " +
                        "sum(a.amount) as lineamount  " +
                        "from OrderDet a inner join OrderHdr b  " +
                        "on a.OrderHdrID = b.OrderHdrID  " +
                        "left outer join SalesCommissionRecord c  " +
                        "on a.OrderHdrID = c.OrderHdrID  " +
                        "inner join item d  " +
                        "on a.itemno = d.itemno  " +
                        "where a.IsVoided=0 and b.IsVoided = 0  " +
                        "and OrderDate >= @startDate and " +
                        "OrderDetDate <= @endDate and " +
                        "d.iscommission = 1  " +
                        "group by isnull(Nullif(a.userfld1,''),c.salespersonid)) X " +
                        "on X.SalesPersonID = Z.UserName " +
                        "LEFT OUTER JOIN( " +
                        "Select isnull(Nullif(a.userfld1,''),c.salespersonid)  as salespersonid,  " +
                        "sum(a.amount) as servicesalesamount  " +
                        "from OrderDet a inner join OrderHdr b  " +
                        "on a.OrderHdrID = b.OrderHdrID  " +
                        "left outer join SalesCommissionRecord c  " +
                        "on a.OrderHdrID = c.OrderHdrID  " +
                        "inner join item d  " +
                        "on a.itemno = d.itemno  " +
                        "where a.IsVoided=0 and b.IsVoided = 0 " +
                        "and d.IsServiceItem = 1  " +
                        "and OrderDate >= @startDate and " +
                        "OrderDetDate <= @endDate and " +
                        "d.iscommission = 1  " +
                        "group by isnull(Nullif(a.userfld1,''),c.salespersonid)) P " +
                        "on P.SalesPersonID = Z.UserName  " +
                        "LEFT OUTER JOIN( " +
                        "Select isnull(Nullif(a.userfld1,''),c.salespersonid)  as salespersonid,  " +
                        "sum(a.amount) as productsalesamount  " +
                        "from OrderDet a inner join OrderHdr b  " +
                        "on a.OrderHdrID = b.OrderHdrID  " +
                        "left outer join SalesCommissionRecord c  " +
                        "on a.OrderHdrID = c.OrderHdrID  " +
                        "inner join item d  " +
                        "on a.itemno = d.itemno  " +
                        "where a.IsVoided=0 and b.IsVoided = 0  " +
                        "and OrderDate >= @startDate and " +
                        "OrderDetDate <= @endDate and " +
                        "d.IsInInventory = 1 and " +
                        "d.iscommission = 1  " +
                        "group by isnull(Nullif(a.userfld1,''),c.salespersonid)) Q " +
                        "on Q.SalesPersonID = Z.UserName ";
                #endregion

                #region -= OBSOLETED 2 =-
                string newSql =
                    "declare @startDate AS datetime; " +
                    "declare @endDate AS datetime; " +
                    "" +
                    "set @startDate = '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                    "set @endDate = '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                    "" +
                    "SELECT UserName " +
                        ", ISNULL(ServiceSalesAmount,0) AS [Service] " +
                        ", ISNULL(ProductSalesAmount,0) AS [Product] " +
                        ", ISNULL(OpenProductSalesAmount,0) AS [OpenPriceProduct] " +
                        ", ISNULL(SystemItemAmount,0) AS [SystemItem] " +
                        ", ISNULL(NonCommissionableAmount,0) AS [NonCommission] " +
                        ", ISNULL(LineAmount, 0) AS [TotalProductAndService] " +
                        ", ISNULL(PointSoldAmount,0) AS [PointSold] " +
                        ", ISNULL(PointSoldValue,0) AS [PointSoldValue] " +
                        ", ISNULL(PackageSoldAmount,0) AS [PackageSold] " +
                        ", ISNULL(PackageRedeemQty,0) AS [PackageRedeemed] " +
                        ", ISNULL(PackageRedeemAmount,0) AS [PackageRedeemedValue] " +
                        ", ISNULL(TotalPointAndPackage,0) AS [TotalPointAndPackage] " +
                        ", ISNULL(LineAmount,0) + ISNULL(TotalPointAndPackage,0) AS [TotalAmount] " +
                        ", ISNULL(TotalOrderAmount,0) AS TotalOrderAmount " +
                    "FROM " +
                    "( " +
                        "SELECT UserName " +
                        "FROM UserMst " +
                        "WHERE IsASalesPerson = 1 " +
                    //"AND Deleted = 0 " +
                    ") Z " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS LineAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided = 0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND (d.IsServiceItem = 1 OR d.IsInInventory = 1) " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''),c.SalesPersonID) " +
                    ") X " +
                    "ON X.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS ServiceSalesAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided = 0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.IsCommission = 1 " +
                            "AND d.CategoryName <> 'SYSTEM' " +
                            "AND d.IsServiceItem = 1 " +
                            "AND d.IsInInventory = 0 " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") P " +
                    "ON P.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS ProductSalesAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.IsCommission = 1 " +
                            "AND d.CategoryName <> 'SYSTEM' " +
                            "AND d.IsInInventory = 1 " +
                            "AND d.IsServiceItem = 0 " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") Q " +
                    "ON Q.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS OpenProductSalesAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.IsCommission = 1 " +
                            "AND d.CategoryName <> 'SYSTEM' " +
                            "AND d.IsInInventory = 1 " +
                            "AND d.IsServiceItem = 1 " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") OP " +
                    "ON OP.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS SystemItemAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.CategoryName = 'SYSTEM' " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") SYS " +
                    "ON SYS.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS NonCommissionableAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.IsCommission = 0 " +
                            "AND d.CategoryName <> 'SYSTEM' " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") NC " +
                    "ON NC.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS PointSoldAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.userfld10 = 'D' " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") R " +
                    "ON R.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT a.userfld2 AS SalesPersonID" +
                            ",  SUM(PointAllocated) AS PointSoldValue " +
                        "FROM PointAllocationLog a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "INNER JOIN Item c " +
                                "ON a.userfld1 = c.ItemNo " +
                        "WHERE b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDate <= @endDate " +
                            "AND c.userfld10 = 'D' " +
                            "AND a.PointAllocated > 0 " +
                        "GROUP BY a.userfld2 " +
                    ") S " +
                    "ON S.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS PackageSoldAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.userfld10 = 'T' " +
                            "AND a.UnitPrice <> 0 " +
                    //"AND a.giveCommission = 0 " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") T " +
                    "ON T.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Quantity) AS PackageRedeemQty " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.userfld10 = 'T' " +
                    //"AND a.giveCommission = 1 " +
                            "AND a.UnitPrice = 0 " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") U " +
                    "ON U.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Quantity * d.userfloat3) AS PackageRedeemAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.userfld10 = 'T' " +
                    //"AND a.giveCommission = 1 " +
                            "AND a.UnitPrice = 0 " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") V " +
                    "ON V.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS TotalPointAndPackage " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided=0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                            "AND d.userfld10 IN ('D','T') " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") W " +
                    "ON W.SalesPersonID = Z.UserName " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID)  AS SalesPersonID " +
                            ",  SUM(a.Amount) AS TotalOrderAmount " +
                        "FROM OrderDet a " +
                            "INNER JOIN OrderHdr b " +
                                "ON a.OrderHdrID = b.OrderHdrID " +
                            "LEFT OUTER JOIN SalesCommissionRecord c " +
                                "ON a.OrderHdrID = c.OrderHdrID " +
                            "INNER JOIN Item d " +
                                "ON a.ItemNo = d.ItemNo " +
                        "WHERE a.IsVoided = 0 " +
                            "AND b.IsVoided = 0 " +
                            "AND OrderDate >= @startDate " +
                            "AND OrderDetDate <= @endDate " +
                        "GROUP BY ISNULL(NULLIF(a.userfld1,''), c.SalesPersonID) " +
                    ") Y " +
                    "ON Y.SalesPersonID = Z.UserName ";
                #endregion

                newSql = @"EXEC FetchSalesPersonPerformance
		                               @startDate = N'{0}',
		                               @endDate = N'{1}'";
                newSql = string.Format(newSql, startDate.ToString("yyyy-MM-dd HH:mm:ss")
                                             , endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                QueryCommand cmd = new QueryCommand(newSql, "PowerPOS");
                return DataService.GetDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchUnDeductedSales(int InventoryLocationID)
        {
            try
            {
                string SQL = "select c.categoryname,c.ItemNo,c.ItemName, " +
                            "f.InventoryLocationName, SUM(quantity) as quantity  " +
                            "from  " +
                            "OrderHdr a inner join " +
                            "OrderDet b on a.OrderHdrID = b.OrderHdrID  " +
                            "inner join Item c " +
                            "on b.ItemNo = c.ItemNo  " +
                            "inner join PointOfSale d " +
                            "on d.PointOfSaleID = a.PointOfSaleID  " +
                            "inner join Outlet e on d.outletname = e.OutletName  " +
                            "inner join InventoryLocation f  " +
                            "on e.InventoryLocationID = f.InventoryLocationID " +
                            "where " +
                            "(b.InventoryHdrRefNo = '' or b.InventoryHdrRefNo is null) " +
                            "and b.IsPreOrder = 0 " +
                            "and a.IsVoided =0 and b.IsVoided = 0 " +
                            "and IsInInventory = 1 ";
                string groupbyclause =
                            "group by  c.categoryname,c.ItemNo,c.ItemName,f.InventoryLocationName ";
                if (InventoryLocationID != 0)
                {
                    SQL = SQL +
                        "and f.InventoryLocationID = " + InventoryLocationID + " " +
                        groupbyclause;
                }
                else
                {
                    SQL = SQL + groupbyclause;
                }
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                cmd.CommandTimeout = 9999999;
                DataTable dt = DataService.GetDataSet(cmd).Tables[0];
                dt.Columns.Add("OnHand");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string status = "";
                    decimal onHandQty = InventoryController.GetStockBalanceQtyByItemByDate(dt.Rows[i]["ItemNo"].ToString(), InventoryLocationID, DateTime.Now, out status);
                    dt.Rows[i]["OnHand"] = onHandQty.ToString();
                }
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchLineCommissionItemByDate(
            DateTime startdate, DateTime enddate,
            string OutletName, string DeptID, string SalesPersons,
            string SortColumn, string SortDir)
        {
            string qryData;
            #region // Query to fetch data //
            qryData =
                "SELECT CAST(OrderHdr.OrderDate AS DATE) AS [ReportDate] " +
                    ", SalesCommissionRecord.SalesPersonID AS [SalesPerson] " +
                    ", ISNULL(OrderDet.userfld1, SalesCommissionRecord.SalesPersonID) AS [Assistant] " +
                    ", SUM(Amount) AS [Amount] " +
                "FROM OrderHdr " +
                    "INNER JOIN OrderDet ON OrderHdr.OrderHdrID = OrderDet.OrderHdrID " +
                    "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
                    "INNER JOIN PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID " +
                    "INNER JOIN Item ON OrderDet.ItemNo = Item.ItemNo " +
                "WHERE OrderHdr.IsVoided = 0 AND OrderDet.IsVoided = 0 " +
                    "AND Item.CategoryName <> 'SYSTEM' " +
                    "AND OutletName LIKE @OutletName " +
                    "AND DepartmentID LIKE @DeptID " +
                    "AND OrderHdr.OrderDate >= @StartDate AND OrderHdr.OrderDate < @EndDate " +
                    "AND OrderDet.userfld1 IS NOT NULL " +
                    "AND SalesCommissionRecord.SalesPersonID <> OrderDet.userfld1 " +
                    "AND OrderDet.userfld1 LIKE @SalesPerson " +
                "GROUP BY CAST(OrderHdr.OrderDate AS DATE) " +
                    ", SalesCommissionRecord.SalesPersonID " +
                    ", ISNULL(OrderDet.userfld1, SalesCommissionRecord.SalesPersonID) ";
            #endregion

            if (OutletName == "" || OutletName.ToLower() == "all") { OutletName = "%"; }
            if (DeptID == "0") { DeptID = "%"; }
            if (SalesPersons == "" || SalesPersons.ToLower() == "all") { SalesPersons = "%"; }

            QueryCommand Cmd = new QueryCommand(qryData);
            Cmd.AddParameter("@StartDate", startdate, DbType.DateTime);
            Cmd.AddParameter("@EndDate", enddate, DbType.DateTime);
            Cmd.AddParameter("@OutletName", OutletName, DbType.String);
            Cmd.AddParameter("@DeptID", DeptID, DbType.String);
            Cmd.AddParameter("@SalesPerson", SalesPersons, DbType.String);

            DataTable Output = new DataTable();
            Output.Load(DataService.GetReader(Cmd));

            #region *) Add Sorting settings
            if (SortColumn == "") { SortColumn = "ReportDate"; }
            if (SortDir == "") { SortDir = "ASC"; }
            Output.DefaultView.Sort = SortColumn + " " + SortDir;
            //qryData += " ORDER BY " + SortColumn + " " + SortDir + " ";
            #endregion

            return Output;
        }

        public static DataTable FetchCustomerInstallmentHistoryReport(DateTime startDate, DateTime endDate, string membershipNo)
        {
            try
            {
                string SQL = "declare @credit as varchar(50); " +
                        "declare @creditpayment  as varchar(50); " +
                        "declare @startdate as datetime; " +
                        "declare @enddate as datetime; " +
                        "declare @membershipno as varchar(50); " +
                        "set @credit = 'INSTALLMENT'; " +
                        "set @creditpayment = 'INST_PAYMENT' " +
                        "set @startdate = '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                        "set @enddate = '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                        "set @membershipno = '" + membershipNo + "' " +
                        "select orderhdrid as receiptno,orderdate as receiptdate,isnull(SUM(amount),0.00) as credit, 0.00 as debit from  " +
                        "OrderHdr a inner join ReceiptDet b " +
                        "on a.OrderHdrID = b.ReceiptHdrID " +
                        "inner join Membership c on c.MembershipNo = a.MembershipNo " +
                        "where  " +
                        "a.IsVoided=0 and PaymentType = @credit " +
                        "and OrderDate >= @startdate and OrderDate <= @enddate " +
                        "and c.MembershipNo = @membershipno " +
                        "group by OrderDate,orderhdrid " +
                        "select a.OrderHdrid as receiptno,OrderDate as receiptdate,0.00 as credit, isnull(sum(amount),0.00) as debit  " +
                        "from OrderHdr a inner join OrderDet b on " +
                        "a.OrderHdrID = b.OrderHdrID " +
                        "inner join Membership c on c.MembershipNo = a.MembershipNo " +
                        "where  " +
                        "a.IsVoided=0 and b.IsVoided=0 " +
                        "and c.MembershipNo = @membershipno " +
                        "and ItemNo = @creditpayment " +
                        "and OrderDate >= @startdate and OrderDate <= @enddate " +
                        "group by a.OrderHdrID,OrderDate ";

                DataSet ds = DataService.GetDataSet(new QueryCommand(SQL, "PowerPOS"));
                ds.Tables[0].Merge(ds.Tables[1]);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchOutstandingInstallmentReport(string search)
        {
            try
            {
                DataTable dt = new DataTable();

                //string SQL = "SELECT MembershipNo as MembershipNo, NameToAppear as NameToAppear FROM Membership";

                string SQL =
                "SELECT dt.MembershipNo as MembershipNo, m.NameToAppear as NameToAppear, SUM(credit) as Credit,"
                + "SUM(debit) as Debit, SUM(credit - debit) as Balance, m.Home as Home, m.Mobile as Mobile FROM"
                + "("
                + "    select MembershipNo,isnull(SUM(amount),0.00) as credit, 0 as debit from"
                + "        OrderHdr a inner join ReceiptDet b "
                + "        on a.OrderHdrID = b.ReceiptHdrID  "
                + "        where "
                + "        a.IsVoided=0 and PaymentType = 'INSTALLMENT' "
                + "        group by MembershipNo "
                + "UNION ALL"
                + "    select MembershipNo, 0 as credit, isnull(sum(amount),0.00) as debit"
                + "        from OrderHdr a inner join OrderDet b on"
                + "        a.OrderHdrID = b.OrderHdrID "
                + "        where "
                + "        a.IsVoided=0 and b.IsVoided=0 "
                + "        and ItemNo = 'INST_PAYMENT' "
                + "        group by MembershipNo"
                + ") AS dt inner join Membership m on dt.MembershipNo = m.MembershipNo"
                + " where dt.MembershipNo+m.NameToAppear+ISNULL(m.Home,'')+ISNULL(m.Mobile,'') LIKE '%" + search + "%' "
                + " group by dt.MembershipNo, m.NameToAppear, m.home, m.mobile"
                + " HAVING SUM(credit - debit) <> 0";


                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchInstallmentReport(string search, bool IsShowOutstandingOnly)
        {
            try
            {
                DataTable dt = new DataTable();

                //string SQL = "SELECT MembershipNo as MembershipNo, NameToAppear as NameToAppear FROM Membership";
                #region obsolete
                //string SQL =
                //"SELECT dt.MembershipNo as MembershipNo, m.NameToAppear as NameToAppear, SUM(credit) as Credit,"
                //+ "SUM(debit) as Debit, SUM(credit - debit) as Balance, m.Home as Home, m.Mobile as Mobile FROM"
                //+ "("
                //+ "    select MembershipNo,isnull(SUM(amount),0.00) as credit, 0 as debit from"
                //+ "        OrderHdr a inner join ReceiptDet b "
                //+ "        on a.OrderHdrID = b.ReceiptHdrID  "
                //+ "        where "
                //+ "        a.IsVoided=0 and PaymentType = 'INSTALLMENT' "
                //+ "        group by MembershipNo "
                //+ "UNION ALL"
                //+ "    select MembershipNo, 0 as credit, isnull(sum(amount),0.00) as debit"
                //+ "        from OrderHdr a inner join OrderDet b on"
                //+ "        a.OrderHdrID = b.OrderHdrID "
                //+ "        where "
                //+ "        a.IsVoided=0 and b.IsVoided=0 "
                //+ "        and ItemNo = 'INST_PAYMENT' "
                //+ "        group by MembershipNo"
                //+ ") AS dt inner join Membership m on dt.MembershipNo = m.MembershipNo"
                //+ " where ISNULL(dt.MembershipNo,'')+ISNULL(m.NameToAppear,'')+ISNULL(m.Home,'')+ISNULL(m.Mobile,'') LIKE '%" + search + "%' "
                //+ " group by dt.MembershipNo, m.NameToAppear, m.home, m.mobile";

                //if (IsShowOutstandingOnly)
                //{
                //    SQL += " HAVING SUM(credit - debit) <> 0";
                //}
                #endregion

                string SQL = @"select m.MembershipNo, m.NameToAppear as NameToAppear, SUM(i.TotalAmount) as Credit,
                        SUM(i.TotalAmount - i.CurrentBalance) as Debit, SUM(i.CurrentBalance) as Balance, m.Home as Home, m.Mobile as Mobile
                        from Installment i inner join 
                        Membership m on i.MembershipNo = m.MembershipNo and ISNULL(m.Deleted,0) = 0
                        where ISNULL(i.Deleted,0) = 0 and ISNULL(m.MembershipNo,'')+ISNULL(m.NameToAppear,'')+ISNULL(m.Home,'')+ISNULL(m.Mobile,'') LIKE '%{0}%' 
                         group by m.MembershipNo, m.NameToAppear, m.home, m.mobile"
                ;

                if (IsShowOutstandingOnly)
                {
                    SQL += " HAVING SUM(i.CurrentBalance) <> 0";
                }
                SQL = String.Format(SQL, search);

                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchAccountStatementReport(string startDate, string endDate, string search)
        {
            try
            {
                DataTable dt = new DataTable();

                //string SQL = "SELECT MembershipNo as MembershipNo, NameToAppear as NameToAppear FROM Membership";

                string SQL =
                "DECLARE @Membershipno nvarchar(10); " +
                "DECLARE @DateStart Datetime; " +
                "DECLARE @DateEnd Datetime; " +
                "SET @Membershipno = '%" + search + "%'; " +
                "SET @DateStart = '" + startDate + "'; " +
                "SET @DateEnd = '" + endDate + "'; " +
                "SELECT dt.MembershipNo as MembershipNo, m.NameToAppear as NameToAppear, " +
                "SUM(prevdebit - prevcredit) as OpeningBalance, SUM(credit) as Credit,SUM(debit) as Debit, " +
                "SUM(prevdebit - prevcredit) + SUM(debit - credit) as Balance, m.Home as Home, m.Mobile as Mobile " +
                "FROM " +
                "( " +
                "select MembershipNo,isnull(SUM(amount),0.00) as prevdebit, 0 as prevcredit , 0 as debit, 0 as credit " +
                "FROM OrderHdr a inner join ReceiptDet b on a.OrderHdrID = b.ReceiptHdrID          " +
                "where a.IsVoided=0 and PaymentType = 'INSTALLMENT' and a.orderdate < @DateStart " +
                "group by MembershipNo " +
                "UNION ALL    " +
                "select MembershipNo, 0 as prevdebit, isnull(sum(amount),0.00) as prevcredit, 0 as debit, 0 as credit       " +
                "from OrderHdr a inner join OrderDet b on " +
                "a.OrderHdrID = b.OrderHdrID " +
                "where a.IsVoided=0 and b.IsVoided=0 and a.orderdate < @DateStart  " +
                "and ItemNo = 'INST_PAYMENT' group by MembershipNo " +
                "union all " +
                "select MembershipNo, 0 as prevdebit, 0 as prevcredit, isnull(SUM(amount),0.00) as debit, 0 as credit " +
                "from OrderHdr a inner join ReceiptDet b on a.OrderHdrID = b.ReceiptHdrID         " +
                "where a.IsVoided=0 and PaymentType = 'INSTALLMENT' and a.orderdate >= @DateStart  and a.orderdate <= @DateEnd " +
                "group by MembershipNo " +
                "UNION ALL    " +
                "select MembershipNo, 0 as prevdebit, 0 as prevcredit, 0 as debit, isnull(sum(amount),0.00) as credit        " +
                "from OrderHdr a inner join OrderDet b on " +
                "a.OrderHdrID = b.OrderHdrID " +
                "where a.IsVoided=0 and b.IsVoided=0 and a.orderdate >= @DateStart  and a.orderdate <= @DateEnd " +
                "and ItemNo = 'INST_PAYMENT' group by MembershipNo " +
                ") AS dt " +
                "inner join Membership m on dt.MembershipNo = m.MembershipNo  " +
                "where dt.MembershipNo+m.NameToAppear+m.Home+m.Mobile LIKE @Membershipno " +
                "group by dt.MembershipNo, m.NameToAppear, m.home, m.mobile " +
                "having (SUM(prevdebit - prevcredit) > 0 or SUM(credit) > 0 or SUM(debit) > 0)";


                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static Boolean GetRangeDateInstallmentByMember(string MembershipNo, out DateTime StartDate, out DateTime EndDate)
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            try
            {
                DataTable dt = new DataTable();
                string query = @"SELECT ISNULL(MIN(InstallmentCreatedDate), getdate())  as StartDate, 
                                ISNULL(Max(InstallmentCreatedDate), getdate())  as EndDate, MembershipNo 
                                from installment
                                where MembershipNo = '{0}'
                                group by Membershipno";
                query = string.Format(query, MembershipNo);
                dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                if (dt.Rows.Count > 0) {
                    StartDate = (DateTime) dt.Rows[0]["StartDate"];
                    EndDate = (DateTime)dt.Rows[0]["EndDate"];
                }

                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public static DataTable FetchAccountStatementByMember(string MembershipNo, bool IsShowPaidInvoices, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                DataTable dt = new DataTable();

                //string SQL = "SELECT MembershipNo as MembershipNo, NameToAppear as NameToAppear FROM Membership";
                #region obsoletequery

                //string SQL =

//                @"select m.MembershipNo, m.NameToAppear, oh.OrderDate, oh.orderHdrID, oh.userfld5 as InvoiceNo, ISNULL(rd.userfld1,'') as PaymentTerm, 
//                CASE WHEN 0 > case when isnull(SUM(rd.amount),0.00) - isnull(SUM(db.amount),0.00) > 0 then
//				DATEDIFF(day,  CASE WHEN CHARINDEX('CREDIT',ISNULL(rd.userfld1,'')) > 0 
//				THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
//				CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), oh.orderdate)
//				ELSE CASE WHEN CHARINDEX('From Month End',ISNULL(rd.userfld1,'')) > 0 
//				THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
//				CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,oh.orderdate)+1,0)) )
//				ELSE oh.OrderDate END END, getdate()) else 0 end THEN 0 ELSE 
//				case when isnull(SUM(rd.amount),0.00) - isnull(SUM(db.amount),0.00) > 0 then
//				DATEDIFF(day,  CASE WHEN CHARINDEX('CREDIT',ISNULL(rd.userfld1,'')) > 0 
//				THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
//				CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), oh.orderdate)
//				ELSE CASE WHEN CHARINDEX('From Month End',ISNULL(rd.userfld1,'')) > 0 
//				THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
//				CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,oh.orderdate)+1,0)) )
//				ELSE oh.OrderDate END END, getdate()) else 0 end
//				END  as DaysOutStanding,
//	            isnull(SUM(rd.amount),0.00) as credit, isnull(SUM(db.amount),0.00) as debit, 
//	            isnull(SUM(rd.amount),0.00) - isnull(SUM(db.amount),0.00) as balance, p.OutletName " +
//                "from orderhdr oh " +
//                "inner join membership m on oh.MembershipNo = m.MembershipNo " +
//                "inner join PointOfSale p on oh.PointOfSaleID = p.PointOfSaleID " +
//                "left join " +
//                "(  " +
//                "    select sum(amount) as amount, ReceiptHdrID, userfld1  " +
//                "    from receiptdet  " +
//                "    where PaymentType = 'INSTALLMENT'  " +
//                "    group by ReceiptHdrID, userfld1  " +
//                ")rd on rd.ReceipthdrID = oh.OrderhdrID " +
//                "left join " +
//                "( " +
//                "	select od.amount, od.userfld3 as parInvNo " +
//                "	from orderhdr oh " +
//                "	inner join orderdet od on oh.OrderHdrID = od.OrderHdrID " +
//                "	where oh.IsVoided = 0 and od.IsVoided = 0 and od.ItemNo = 'INST_PAYMENT' " +
//                ")db on db.parInvNo = oh.OrderHdrID " +
//                "where oh.IsVoided = 0 and m.MembershipNo = '" + MembershipNo + "' " +
//                "and oh.OrderDate between '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
//                "GROUP BY  m.MembershipNo, m.NameToAppear, oh.orderHdrID, oh.userfld5, p.OutletName,oh.OrderDate, rd.userfld1 ";

//                if (!IsShowPaidInvoices)
//                {
//                    SQL += " HAVING isnull(SUM(rd.amount),0.00) - isnull(SUM(db.amount),0.00) > 0";
                //                }
                #endregion

                string SQL = @"
                select i.MembershipNo, m.NameToAppear, oh.OrderDate, i.orderHdrID, i.userfld1 as InvoiceNo, ISNULL(rd.userfld1,'') as PaymentTerm, 
                CASE WHEN 0 > case when isnull(i.CurrentBalance,0.00) > 0 then
                DATEDIFF(day,  CASE WHEN CHARINDEX('CREDIT',ISNULL(rd.userfld1,'')) > 0 
                THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
                CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), oh.orderdate)
                ELSE CASE WHEN CHARINDEX('From Month End',ISNULL(rd.userfld1,'')) > 0 
                THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
                CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,oh.orderdate)+1,0)) )
                ELSE oh.OrderDate END END, getdate()) else 0 end THEN 0 ELSE 
                case when isnull(i.CurrentBalance,0.00) > 0 then
                DATEDIFF(day,  CASE WHEN CHARINDEX('CREDIT',ISNULL(rd.userfld1,'')) > 0 
                THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
                CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), oh.orderdate)
                ELSE CASE WHEN CHARINDEX('From Month End',ISNULL(rd.userfld1,'')) > 0 
                THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
                CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,oh.orderdate)+1,0)) )
                ELSE oh.OrderDate END END, getdate()) else 0 end
                END  as DaysOutStanding,
                isnull(i.TotalAmount,0.00) as credit, isnull(i.TotalAmount - i.CurrentBalance,0.00) as debit, 
                isnull(i.CurrentBalance,0.00) as balance, p.OutletName 
                from installment i
                left join membership m on i.MembershipNo = m.MembershipNo 
				left  join  orderhdr oh  on i.orderhdrid = oh.OrderHdrID  and ISNULL(oh.IsVoided,0) = 0
                left join PointOfSale p on oh.PointOfSaleID = p.PointOfSaleID 
                left join 
                (  
                    select sum(amount) as amount, ReceiptHdrID, userfld1  
                    from receiptdet  
                    where PaymentType = 'INSTALLMENT'  
                    group by ReceiptHdrID, userfld1  
                )rd on rd.ReceipthdrID = oh.OrderhdrID     
                where oh.IsVoided = 0 and m.MembershipNo = '{0}' 
                and oh.orderdate between '{1}' and '{2}' {3}  
				GROUP BY  i.MembershipNo, m.NameToAppear, i.orderHdrID, i.userfld1, p.OutletName,oh.OrderDate, rd.userfld1
                , i.CurrentBalance, i.TotalAmount";

                string where = "";
                if (!IsShowPaidInvoices)
                {
                    //SQL += " HAVING isnull(SUM(rd.amount),0.00) - isnull(SUM(db.amount),0.00) > 0";
                    where = "AND isnull(i.CurrentBalance,0.00) > 0";
                }

                SQL = string.Format(SQL, MembershipNo, StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"), where);

                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable GetDueDateInstallmentPayment(string ReceiptDetID)
        {
            string sql = @"select oh.orderdate, rd.userfld1,
                        CASE WHEN CHARINDEX('CREDIT',ISNULL(rd.userfld1,'')) > 0 
	                        THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), oh.orderdate)
                        ELSE CASE WHEN CHARINDEX('From Month End',ISNULL(rd.userfld1,'')) > 0 
	                        THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,oh.orderdate)+1,0)) )
                        ELSE oh.OrderDate END END as DueDate
                        from ReceiptDet rd 
                        inner join OrderHdr oh on oh.OrderHdrID = rd.ReceiptHdrID
                        where rd.ReceiptDetID = '{0}'";

            sql = string.Format(sql, ReceiptDetID);
            return DataService.GetDataSet(new QueryCommand(sql)).Tables[0];
        }

        public static DataTable FetchAccountingReport(DateTime StartDate, DateTime EndDate, string OutletName)
        { 
            try
            {
                DataTable dt = new DataTable();

                #region set columns datatable
                dt.Columns.Add("Category");
                dt.Columns.Add("Description");
                dt.Columns.Add("Debit");
                dt.Columns.Add("Credit");

            
                for (DateTime day = StartDate; day <= EndDate; day = day.AddDays(1))
                {
                    dt.Columns.Add(day.ToString("dd_MMM_DB"));
                    dt.Columns.Add(day.ToString("dd_MMM_CR"));
                }
                #endregion

                #region set sales
                string sql = "declare @StartDate datetime " +
                                "declare @EndDate datetime " +
                                "declare @Outlet varchar(50) " +
                                "set @StartDate = '{0}' " +
                                "set @EndDate = '{1}' " +
                                "set @Outlet = '{2}' " +
                                "select Cast(oh.OrderDate as Date) as OrderDate, " + 
	                            "    SUM(od.Amount - od.GSTAmount) as AmountBeforeGST, " + 
	                            "    SUM(od.GSTAmount) as GSTAmount, " +
	                            "    SUM(od.Amount) as AmountAfterGST " +
                                "from OrderHdr oh " +
                                "inner join orderdet od on oh.orderhdrid = od.OrderHdrID and od.IsVoided = 0 " +
                                "inner join PointOfSale p on oh.PointOfSaleID = p.PointOfSaleID " +
                                "inner join outlet o on p.OutletName = o.OutletName " +
                                "where oh.isvoided = 0 and oh.OrderDate between @StartDate and @EndDate " +
                                "and (o.OutletName = @Outlet or @Outlet = 'ALL') " +
                                "group by Cast(oh.OrderDate as Date) order by Cast(oh.OrderDate as Date)";
                sql = string.Format(sql, StartDate.ToString("yyyy-MM-dd") + " 00:00:00"
                                        ,EndDate.ToString("yyyy-MM-dd") + " 23:59:59"
                                        ,OutletName);

                Logger.writeLog(sql);
                DataTable dt1 = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    decimal total = 0;
                    decimal totalGST = 0;
                    decimal totalAfterGST = 0;
                    #region Sales
                    DataRow dSales = dt.NewRow();
                    // add sales
                    dSales["Category"] = "Sales";
                    dSales["Description"] = "Sales";

                    //add GST Amount
                    DataRow dgst = dt.NewRow();
                    // add sales
                    dgst["Category"] = "Sales";
                    dgst["Description"] = "GST O/P";

                    DataRow dar = dt.NewRow();
                    // add sales
                    dar["Category"] = "Sales";
                    dar["Description"] = "A/R";


                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DateTime da = (DateTime)dt1.Rows[i]["OrderDate"];
                        decimal amountbeforeGST = dt1.Rows[i]["AmountBeforeGST"].ToString().GetDecimalValue();
                        decimal GSTAmount = dt1.Rows[i]["GSTAmount"].ToString().GetDecimalValue();
                        decimal AmountAfterGST = dt1.Rows[i]["AmountAfterGST"].ToString().GetDecimalValue();

                        total += amountbeforeGST;
                        totalGST += GSTAmount;
                        totalAfterGST += AmountAfterGST;

                        dSales[da.ToString("dd_MMM_") + "CR"] = amountbeforeGST.ToString("N2");
                        dgst[da.ToString("dd_MMM_") + "CR"] = GSTAmount.ToString("N2");
                         dar[da.ToString("dd_MMM_") + "DB"] = AmountAfterGST.ToString("N2");
                    }

                    dSales["Credit"] = total.ToString("N2");
                    dgst["Credit"] = totalGST.ToString("N2");
                    dar["Debit"] = totalAfterGST.ToString("N2");
                    

                    dt.Rows.Add(dSales);
                    dt.Rows.Add(dgst);
                    dt.Rows.Add(dar);
                    #endregion
                }
                else 
                {
                    DataRow dr = dt.NewRow();

                    dr["Category"] = "Sales";
                    dr["Description"] = "Sales";
                    dr["Credit"] = "0.00";

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["Category"] = "Sales";
                    dr["Description"] = "GST O/P";
                    dr["Credit"] = "0.00";

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["Category"] = "Sales";
                    dr["Description"] = "A/R";
                    dr["Debit"] = "0.00";

                    dt.Rows.Add(dr);
                }

                #endregion

                #region set collection of sales

                sql = "declare @StartDate datetime " +
                      "  declare @EndDate datetime " +
                      "  declare @Outlet varchar(50) " +
                      "  set @StartDate = '{0}' " +
                      "  set @EndDate = '{1}' " +
                      "  set @Outlet = '{2}' " +

                      "  select Cast(oh.OrderDate as Date) as OrderDate, UPPER(rd.PaymentType) as PaymentType , SUM(rd.Amount) as Amount " +
                      "   from OrderHdr oh " +
                      "  inner join receiptdet rd on oh.orderhdrid = rd.ReceiptHdrID " +
                      "  inner join PointOfSale p on oh.PointOfSaleID = p.PointOfSaleID " +
                      "  inner join outlet o on p.OutletName = o.OutletName " +
                      "  where oh.isvoided = 0 and oh.OrderDate between @StartDate and @EndDate " +
                      "  and (o.OutletName = @Outlet or @Outlet = 'ALL') " +
                      "  group by Cast(oh.OrderDate as Date), UPPER(rd.PaymentType)" +
                      "  order by Cast(oh.OrderDate as Date) ";
                sql = string.Format(sql, StartDate.ToString("yyyy-MM-dd") + " 00:00:00"
                                    , EndDate.ToString("yyyy-MM-dd") + " 23:59:59"
                                    , OutletName);

                DataTable dt2 = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    DataView dv = new DataView(dt2);
                    DataTable paymentTypes = dv.ToTable(true, "PaymentType");

                    for (int i = 0; i < paymentTypes.Rows.Count; i++)
                    {
                        decimal total = 0;
                        DataRow dr = dt.NewRow();
                        string paymentType = paymentTypes.Rows[i]["PaymentType"].ToString();

                        dr["Category"] = "Collection Of Sales";
                        dr["Description"] = paymentType;

                        var dpPt = dt2.Select("PaymentType = '"+ paymentType +"'");
                        for (int j = 0; j < dpPt.Length; j++)
                        {
                            DateTime dat = (DateTime)dpPt[j]["OrderDate"];

                            decimal amount = dpPt[j]["Amount"].ToString().GetDecimalValue();

                            total += amount;

                            dr[dat.ToString("dd_MMM_") + "DB"] = amount.ToString("N2");
                        }

                        dr["Debit"] = total.ToString("N2");
                        dt.Rows.Add(dr);
                    }

                    DataRow dr2 = dt.NewRow();
                    dr2["Category"] = "Collection Of Sales";
                    dr2["Description"] = "Account Receivable";
                    decimal grandtotal = 0;

                    for (int k = 0; k < dt2.Rows.Count; k++)
                    {
                        DateTime dat = (DateTime)dt2.Rows[k]["OrderDate"];

                        decimal am1 = dt2.Rows[k]["Amount"].ToString().GetDecimalValue();
                        if (dr2[dat.ToString("dd_MMM_") + "CR"] != null && dr2[dat.ToString("dd_MMM_") + "CR"].ToString() != "")
                        {
                            decimal amt = dr2[dat.ToString("dd_MMM_") + "CR"].ToString().GetDecimalValue();
                            dr2[dat.ToString("dd_MMM_") + "CR"] = (amt + am1).ToString("N2");
                        }
                        else 
                        {
                            dr2[dat.ToString("dd_MMM_") + "CR"] = am1.ToString("N2");
                        }
                        grandtotal += am1;
                    }
                   
                    dr2["Credit"] = grandtotal.ToString("N2");
                    dt.Rows.Add(dr2);
                    
                }
                else 
                {
                    DataRow dr = dt.NewRow();
                    dr["Category"] = "Collection Of Sales";
                    dr["Description"] = "Cash at Bank";
                    dr["Debit"] = "0.00";

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["Category"] = "Collection Of Sales";
                    dr["Description"] = "Account Receivable";
                    dr["Credit"] = "0.00";

                    dt.Rows.Add(dr);
                }

                #endregion

                #region PurchaseofInventory

                sql = "declare @StartDate datetime  " +
                        "declare @EndDate datetime  " +
                        "declare @Outlet varchar(50)  " +
                        "set @StartDate = '{0}'  " +
                        "set @EndDate = '{1}'  " +
                        "set @Outlet = '{2}'  " +

                        "SELECT Cast(IH.InventoryDate as Date) as InventoryDate, " +
                        "	SUM(ISNULL(ID.GSTAmount,0)) as GSTAmount, " +
                        "	SUM(ISNULL(ID.CostOfGoods,0) * ISNULL(ID.Quantity,0)) - SUM(ISNULL(ID.GSTAmount,0)) AS AmountBeforeGST, " +
                        "	SUM(ISNULL(ID.CostOfGoods,0) * ISNULL(ID.Quantity,0)) AS AmountAfterGST " +
                        "FROM InventoryHdr IH  " +
                        "    LEFT JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo  " +
                        "    LEFT JOIN Outlet O ON IH.InventoryLocationID = O.InventoryLocationID " +
                        "WHERE IH.InventoryDate BETWEEN @StartDate AND @EndDate  " +
                        "	AND (o.OutletName = @Outlet or @Outlet = 'ALL')  " +
                        "    AND IH.MovementType Like 'Stock In' " +
                        "GROUP BY Cast(IH.InventoryDate as Date) " +
                        "ORDER BY Cast(IH.InventoryDate as Date)";
                 sql = string.Format(sql, StartDate.ToString("yyyy-MM-dd") + " 00:00:00"
                                        ,EndDate.ToString("yyyy-MM-dd") + " 23:59:59"
                                        ,OutletName);

                Logger.writeLog(sql);
                DataTable dtSI = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

                if (dtSI != null && dtSI.Rows.Count > 0)
                {
                    
                    decimal total = 0;
                    decimal totalGST = 0;
                    decimal totalAfterGST = 0;
                   
                    DataRow dSales = dt.NewRow();
                    // add sales
                    dSales["Category"] = "Purchase Of Inventory";
                    dSales["Description"] = "Inventory";

                    //add GST Amount
                    DataRow dgst = dt.NewRow();
                    // add sales
                    dgst["Category"] = "Purchase Of Inventory";
                    dgst["Description"] = "GST Input";

                    DataRow dar = dt.NewRow();
                    // add sales
                    dar["Category"] = "Purchase Of Inventory";
                    dar["Description"] = "Account Payable";


                    for (int i = 0; i < dtSI.Rows.Count; i++)
                    {
                        DateTime da = (DateTime)dtSI.Rows[i]["InventoryDate"];
                        decimal amountbeforeGST = dtSI.Rows[i]["AmountBeforeGST"].ToString().GetDecimalValue();
                        decimal GSTAmount = dtSI.Rows[i]["GSTAmount"].ToString().GetDecimalValue();
                        decimal AmountAfterGST = dtSI.Rows[i]["AmountAfterGST"].ToString().GetDecimalValue();

                        total += amountbeforeGST;
                        totalGST += GSTAmount;
                        totalAfterGST += AmountAfterGST;

                        dSales[da.ToString("dd_MMM_") + "DB"] = amountbeforeGST.ToString("N2");
                        dgst[da.ToString("dd_MMM_") + "DB"] = GSTAmount.ToString("N2");
                        dar[da.ToString("dd_MMM_") + "CR"] = AmountAfterGST.ToString("N2");
                    }

                    dSales["Debit"] = total.ToString("N2");
                    dgst["Debit"] = totalGST.ToString("N2");
                    dar["Credit"] = totalAfterGST.ToString("N2");


                    dt.Rows.Add(dSales);
                    dt.Rows.Add(dgst);
                    dt.Rows.Add(dar);
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["Category"] = "Purchase Of Inventory";
                    dr["Description"] = "GST Input";
                    dr["Debit"] = "0.00";

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["Category"] = "Purchase Of Inventory";
                    dr["Description"] = "Inventory";
                    dr["Debit"] = "0.00";

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["Category"] = "Purchase Of Inventory";
                    dr["Description"] = "Account Payable";
                    dr["Credit"] = "0.00";

                    dt.Rows.Add(dr);
                }   
                #endregion

                #region stocktake

                sql = "declare @StartDate datetime " +
                      "declare @EndDate datetime " +
                      "declare @Outlet varchar(50) " +
                      "set @StartDate = '{0}' " +
                      "set @EndDate = '{1}' " +
                      "set @Outlet = '{2}' " +
                      "SELECT Cast(IH.InventoryDate as Date) as InventoryDate, " +
	                  "    SUM(ST.AdjustmentQty) as Amount " +
                      "FROM InventoryHdr IH  " +
                      "    INNER JOIN StockTake st ON IH.InventoryHdrRefNo = 'ST' + Convert(varchar(50), st.stocktakeid) " +
                      "    LEFT JOIN Outlet O ON IH.InventoryLocationID = O.InventoryLocationID " +
                      "WHERE IH.InventoryDate BETWEEN @StartDate AND @EndDate  " +
                      "    AND (o.OutletName = @Outlet or @Outlet = 'ALL') " +
                      "group by  Cast(IH.InventoryDate as Date)" +
                      "order by  Cast(IH.InventoryDate as Date)";
                sql = string.Format(sql, StartDate.ToString("yyyy-MM-dd") + " 00:00:00"
                                       , EndDate.ToString("yyyy-MM-dd") + " 23:59:59"
                                       , OutletName);

                Logger.writeLog(sql);
                DataTable dtST = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];
                if (dtST != null && dtST.Rows.Count > 0)
                {
                    decimal total = 0;
                    
                    DataRow dST= dt.NewRow();
                    // add sales
                    dST["Category"] = "Stock Take";
                    dST["Description"] = "Stock Gain";

                    //add GST Amount
                    DataRow dInv = dt.NewRow();
                    // add sales
                    dInv["Category"] = "Stock Take";
                    dInv["Description"] = "Inventory";

                    for (int i = 0; i < dtST.Rows.Count; i++)
                    {
                        DateTime da = (DateTime)dtST.Rows[i]["InventoryDate"];
                        decimal amount = dtST.Rows[i]["Amount"].ToString().GetDecimalValue();
                        total += amount;

                        if (amount > 0)
                        {
                            dST[da.ToString("dd_MMM_") + "CR"] = amount.ToString("N2");
                            dInv[da.ToString("dd_MMM_") + "DB"] = amount.ToString("N2");
                        }
                        else
                        {
                            dST[da.ToString("dd_MMM_") + "DB"] = Math.Abs(amount).ToString("N2");
                            dInv[da.ToString("dd_MMM_") + "CR"] = Math.Abs(amount).ToString("N2");
                        }
                    }

                    if (total > 0)
                    {
                        dST["Credit"] = total.ToString("N2");
                        dInv["Debit"] = total.ToString("N2");
                    }
                    else 
                    {
                        dST["Debit"] = Math.Abs(total).ToString("N2");
                        dInv["Credit"] = Math.Abs(total).ToString("N2");
                    }


                    dt.Rows.Add(dST);
                    dt.Rows.Add(dInv);
                }
                else
                {

                    DataRow dr = dt.NewRow();
                    dr["Category"] = "Stock Take";
                    dr["Description"] = "Stock Gain";
                    dr["Debit"] = "0.00";

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["Category"] = "Stock Take";
                    dr["Description"] = "Inventory";
                    dr["Credit"] = "0.00";

                    dt.Rows.Add(dr);
                }

                #endregion

                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchAgingReport(string MembershipNo, string InvoiceNo)
        {
            try
            {
                DataTable dt = new DataTable();

                string sql1 = @"
                     declare @searchText  nvarchar(500)
				        set @searchText=N'"+MembershipNo+"'";

                string sql = @"               
                select NameToAppear as CustomerName, InvoiceNo, DueDate, 
	                case when DaysOutStanding = 0 then Convert(varchar(10), Balance) else '' end as TotalCurrent, DueDate,
                    case when DaysOutStanding = 0 then Balance else 0 end as tAmount,
	                case when DaysOutStanding >= 1 and DaysOutStanding <= 30 then  Convert(varchar(10), Balance)  else '' end as aDays,
                    case when DaysOutStanding >= 1 and DaysOutStanding <= 30 then  Balance  else 0 end as aAmount,
	                case when DaysOutStanding >= 31 and DaysOutStanding <= 60 then  Convert(varchar(10), Balance)  else '' end as bDays,
                    case when DaysOutStanding >= 31 and DaysOutStanding <= 60 then  Balance  else 0 end as bAmount,
	                case when DaysOutStanding >= 61 then  Convert(varchar(10), Balance)  else '' end as cDays,
                    case when DaysOutStanding >= 61 then  Balance  else 0 end as cAmount
                from
                (
                select m.MembershipNo, m.NameToAppear, oh.OrderDate, oh.orderHdrID, oh.userfld5 as InvoiceNo, ISNULL(rd.userfld1,'') as PaymentTerm, 
                CASE WHEN 0 > case when isnull(SUM(rd.amount),0.00) - isnull(SUM(Inst.TotalAmount - Inst.CurrentBalance),0.00) > 0 then
                DATEDIFF(day,  CASE WHEN CHARINDEX('CREDIT',ISNULL(rd.userfld1,'')) > 0 
                THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
                CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), oh.orderdate)
                ELSE CASE WHEN CHARINDEX('From Month End',ISNULL(rd.userfld1,'')) > 0 
                THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
                CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,oh.orderdate)+1,0)) )
                ELSE oh.OrderDate END END, getdate()) else 0 end THEN 0 ELSE 
                case when isnull(SUM(rd.amount),0.00) - isnull(SUM(Inst.TotalAmount - Inst.CurrentBalance),0.00) > 0 then
                DATEDIFF(day,  CASE WHEN CHARINDEX('CREDIT',ISNULL(rd.userfld1,'')) > 0 
                THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
                CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), oh.orderdate)
                ELSE CASE WHEN CHARINDEX('From Month End',ISNULL(rd.userfld1,'')) > 0 
                THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,''), 
                CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,oh.orderdate)+1,0)) )
                ELSE oh.OrderDate END END, getdate()) else 0 end
                END  as DaysOutStanding,
                isnull(SUM(rd.amount),0.00) as credit, isnull(SUM(Inst.TotalAmount - Inst.CurrentBalance),0.00) as debit, 
                isnull(SUM(rd.amount),0.00) - isnull(SUM(Inst.TotalAmount - Inst.CurrentBalance),0.00) as balance, p.OutletName,
				CASE WHEN CHARINDEX('CREDIT',ISNULL(rd.userfld1,'')) > 0 THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,'')
				, CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), oh.orderdate)
                ELSE CASE WHEN CHARINDEX('From Month End',ISNULL(rd.userfld1,'')) > 0 THEN DATEADD(dd,CAST(LTRIM(RTRIM(LEFT(ISNULL(rd.userfld1,'')
				, CHARINDEX(' ',ISNULL(rd.userfld1,''))))) AS INT), DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,oh.orderdate)+1,0)) )
                ELSE oh.OrderDate END END as DueDate
                from orderhdr oh  
                inner join membership m on oh.MembershipNo = m.MembershipNo 
                inner join PointOfSale p on oh.PointOfSaleID = p.PointOfSaleID 
                left join 
                (  
	                select sum(amount) as amount, ReceiptHdrID, userfld1  
	                from receiptdet  
	                where PaymentType = 'INSTALLMENT'  
	                group by ReceiptHdrID, userfld1  
                )rd on rd.ReceipthdrID = oh.OrderhdrID  
                left join Installment Inst 
				on Inst.OrderHdrId = oh.OrderHdrID 
                where oh.IsVoided = 0 and m.MembershipNo != 'WALK-IN'
                GROUP BY  m.MembershipNo, m.NameToAppear, oh.orderHdrID, oh.userfld5, p.OutletName,oh.OrderDate, rd.userfld1 
                HAVING isnull(SUM(rd.amount),0.00) - isnull(SUM(Inst.TotalAmount - Inst.CurrentBalance),0.00) > 0
                ) a
                where ISNULL(a.MembershipNo,'') + ISNULL(a.NameToAppear,'') like '%' + @searchText + '%' " +
                "and ISNULL(a.InvoiceNo,'') like '%" + InvoiceNo + "%' ";
                sql = string.Concat(sql1, sql);
                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(sql)));

                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchAccountStatementHistory(string ReceiptNo)
        {
            try
            {
                DataTable dt = new DataTable();

                //string SQL = "SELECT MembershipNo as MembershipNo, NameToAppear as NameToAppear FROM Membership";
                #region obsolete
                //string SQL =

                //"SELECT m.MembershipNo, m.NameToAppear, dt.orderhdrid, dt.OrderDate, dt.ReceiptNo, dt.PaymentForID, dt.PaymentFor, " +
                //    "dt.Credit, dt.Debit, p.OutletName " +
                //"FROM " +
                //"( " +
                //"select oh.MembershipNo, oh.orderhdrid, oh.userfld5 as ReceiptNo, oh.orderhdrid as PaymentForID, oh.userfld5 as PaymentFor, " +
                //"	ISNULL(sum(rd.amount),0.00) as Credit, 0 as Debit, oh.PointOfSaleID, oh.OrderDate " +
                //"from orderhdr oh " +
                //"inner join receiptdet rd on oh.OrderHdrID = rd.ReceiptHdrID " +
                //"where oh.IsVoided = 0 and rd.PaymentType = 'INSTALLMENT'  " +
                //"GROUP by oh.MembershipNo, oh.orderhdrid, oh.userfld5, oh.PointOfSaleID, oh.OrderDate  " +
                //"UNION " +
                //"select oh.MembershipNo, oh.orderhdrid, oh.userfld5 as ReceiptNo, op.orderhdrid as PaymentForID, op.userfld5 as PaymentFor, " +
                //"	0 as Credit, ISNULL(sum(od.amount),0.00)  as Debit, oh.PointOfSaleID, oh.OrderDate  " +
                //"from orderhdr oh " +
                //"inner join orderdet od on oh.OrderHdrID = od.OrderHdrID " +
                //"inner join orderhdr op on od.userfld3 = op.OrderHdrID " +
                //"where oh.IsVoided = 0 and od.IsVoided = 0 and od.ItemNo = 'INST_PAYMENT'  " +
                //"GROUP by oh.MembershipNo, oh.orderhdrid, oh.userfld5, op.userfld5, op.OrderHdrID, oh.PointOfSaleID, oh.OrderDate " +
                //")dt INNER JOIN Membership m on dt.MembershipNo = m.MembershipNo " +
                //"INNER JOIN PointOfSale p on dt.PointOfSaleID = p.PointOfSaleID " +
                //"WHERE PaymentForID ='" + ReceiptNo + "'";
                #endregion

                string SQL = @"	select distinct ih.MembershipNo, m.NameToAppear, id.orderhdrid, oh.OrderDate, id.userfld1 as ReceiptNo
		            , ih.orderhdrid as PaymentForID, ih.userfld1 as PaymentFor
		            , ISNULL(sum(CASE WHEN id.InstallmentAmount > 0 then id.InstallmentAmount else 0 end),0.00) as Credit
		            , abs(ISNULL(sum(CASE WHEN id.InstallmentAmount < 0 then id.InstallmentAmount else 0 end),0.00)) as Debit
		            , p.OutletName
	            from Installment ih
	            inner join InstallmentDetail id on id.InstallmentRefNo = ih.InstallmentRefNo
	            left join OrderHdr oh on oh.OrderHdrID = id.OrderHdrID 
	            left join PointOfSale p on p.PointOfSaleID = oh.PointOfSaleID 
	            left join Membership m on m.MembershipNo = ih.MembershipNo
	            WHERE ih.OrderhdrID = '{0}'
	            group by ih.MembershipNo, m.NameToAppear, id.orderhdrid, oh.OrderDate, id.userfld1 
		            , ih.orderhdrid, ih.userfld1, p.OutletName ";

                SQL = string.Format(SQL, ReceiptNo);

                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchAccountStatementReportPerTransaction(string startDate, string endDate, string OrderHdrID)
        {
            try
            {
                DataTable dt = new DataTable();

                //string SQL = "SELECT MembershipNo as MembershipNo, NameToAppear as NameToAppear FROM Membership";

                string SQL =
                "DECLARE @DateStart Datetime; " +
                "DECLARE @DateEnd Datetime; " +
                "SET @DateStart = '" + startDate + "'; " +
                "SET @DateEnd = '" + endDate + "'; " +
                "SELECT dt.MembershipNo as MembershipNo, m.NameToAppear as NameToAppear, " +
                "SUM(prevdebit - prevcredit) as OpeningBalance, SUM(credit) as Credit,SUM(debit) as Debit, " +
                "SUM(prevdebit - prevcredit) + SUM(debit - credit) as Balance, m.Home as Home, m.Mobile as Mobile " +
                "FROM " +
                "( " +
                "select MembershipNo,isnull(SUM(amount),0.00) as prevdebit, 0 as prevcredit , 0 as debit, 0 as credit " +
                "FROM OrderHdr a inner join ReceiptDet b on a.OrderHdrID = b.ReceiptHdrID          " +
                "where a.IsVoided=0 and PaymentType = 'INSTALLMENT' and  orderhdrid = '" + OrderHdrID + "' " +
                "group by MembershipNo " +
                "UNION ALL    " +
                "select MembershipNo, 0 as prevdebit, isnull(sum(amount),0.00) as prevcredit, 0 as debit, 0 as credit       " +
                "from OrderHdr a inner join OrderDet b on " +
                "a.OrderHdrID = b.OrderHdrID " +
                "where a.IsVoided=0 and b.IsVoided=0 and isnull(b.userfld3,'') = '" + OrderHdrID + "' " +
                "and ItemNo = 'INST_PAYMENT' group by MembershipNo " +
                ") AS dt " +
                "inner join Membership m on dt.MembershipNo = m.MembershipNo  " +
                "group by dt.MembershipNo, m.NameToAppear, m.home, m.mobile " +
                "having (SUM(prevdebit - prevcredit) > 0 or SUM(credit) > 0 or SUM(debit) > 0)";


                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchAccountStatementDetail(string startDate, string endDate, string membershipNo)
        {
            try
            {
                DataTable dt = new DataTable();

                //string SQL = "SELECT MembershipNo as MembershipNo, NameToAppear as NameToAppear FROM Membership";

                string SQL =
                "DECLARE @Membershipno nvarchar(10); " +
                "DECLARE @DateStart Datetime; " +
                "DECLARE @DateEnd Datetime;  " +
                "SET @Membershipno = '%" + membershipNo + "%' " +
                "SET @DateStart = '" + startDate + "' " +
                "SET @DateEnd = '" + endDate + "' " +

                "DECLARE @T table ( membershipno nvarchar(10), orderdate Datetime, Particular nvarchar(50), debit decimal(18,2), credit decimal(18,2), balance decimal(18,2)  ); " +
                "DECLARE @Member nvarchar(20); " +
                "DECLARE @orderdate DATETIME ;" +
                "DECLARE @debit decimal(18,2); " +
                "DECLARE @credit decimal(18,2); " +
                "DECLARE @balance decimal(18,2); " +
                "DECLARE @particular nvarchar(50); " +
                "DECLARE @BeforeDateStart Datetime; " +
                "SET @BeforeDateStart =dateadd(day,-1, @dateStart); " +

                "insert into @T (membershipno, orderdate, particular, debit, credit, balance) " +
                "select m.membershipno, @DateStart as OrderDate, 'Previous Balance'  as Particular, isnull(pb.debit,0) as debit,  " +
                "isnull(pb.credit,0) as credit, isnull(pb.balance,0) as balance  " +
                "FROM membership m  " +
                "left join  " +
                "( " +
                "SELECT dt.MembershipNo as MembershipNo, 0 as credit, sum(debit-credit) as Debit, SUM(debit - credit) as Balance " +
                "FROM(    " +
                "select MembershipNo,isnull(SUM(amount),0.00) as debit, " +
                "0 as credit from OrderHdr a " +
                "inner join ReceiptDet b on a.OrderHdrID = b.ReceiptHdrID   " +
                "where a.IsVoided=0 and PaymentType = 'INSTALLMENT' and a.orderdate < @DateStart  " +
                "group by MembershipNo " +
                "UNION ALL    " +
                "select MembershipNo, 0 as debit, isnull(sum(amount),0.00) as credit        " +
                "from OrderHdr a " +
                "inner join OrderDet b on a.OrderHdrID = b.OrderHdrID         " +
                "where a.IsVoided=0 and b.IsVoided=0 and a.orderdate < @DateStart         " +
                "and ItemNo = 'INST_PAYMENT' " +
                "group by MembershipNo) AS dt " +
                "group by dt.MembershipNo " +
                ") pb on pb.membershipno = m.membershipno " +
                "where m.membershipno like @Membershipno " +
                "union all " +
                "(select MembershipNo, a.OrderDate, a.orderhdrid, isnull(amount,0.00) as debit, " +
                "0 as credit, 0 as balance from OrderHdr a " +
                "inner join ReceiptDet b on a.OrderHdrID = b.ReceiptHdrID " +
                "where a.IsVoided=0 and PaymentType = 'INSTALLMENT' and a.orderdate >= @DateStart and a.orderdate <= @DateEnd " +
                "and a.membershipno like @Membershipno " +
                "UNION ALL    " +
                "select MembershipNo, a.OrderDate, a.orderhdrid,0 as debit, isnull(amount,0.00) as credit, 0 as balance     " +
                "from OrderHdr a  " +
                "inner join OrderDet b on a.OrderHdrID = b.OrderHdrID          " +
                "where a.IsVoided=0 and b.IsVoided=0 and a.orderdate >= @DateStart and a.orderdate <= @DateEnd  " +
                "and ItemNo = 'INST_PAYMENT' " +
                "and a.membershipno like @Membershipno	)" +
                "declare crs cursor for " +
                "select membershipno, orderdate, particular, debit,credit, balance from @T order by membershipno, orderdate " +
                "OPEN crs " +
                "FETCH NEXT from crs into @Member, @orderdate,@particular, @debit, @credit, @balance " +
                "DECLARE @movingbalance decimal(18,2) " +

                "WHILE @@FETCH_STATUS = 0 " +
                "BEGIN " +
                    "IF @particular = 'Previous Balance' " +
                    "BEGIN " +
                        "SET @movingbalance = @balance " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                        "SET @movingbalance = @movingbalance - @credit + @debit " +
                        "update @T set balance = @movingbalance where orderdate = @orderdate " +
                    "END  " +
                "FETCH NEXT from crs into @Member, @orderdate,@particular, @debit, @credit, @balance  " +
                "END " +
                "CLOSE crs " +
                "DEALLOCATE crs " +
                "select t.*, m.NameToAppear from @T t, membership m where " +
                "t.membershipno = m.membershipno and " +
                "m.membershipno in (select distinct membershipno from @T where debit > 0 or credit > 0 or balance > 0) " +
                "order by m.membershipno, orderdate ";


                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchPreviousOutstandingBalanceDetail(string startDate, string endDate, string membershipNo)
        {
            try
            {
                DataTable dt = new DataTable();

                string SQL =
                "DECLARE @credit AS VARCHAR(50); " +
                "DECLARE @creditpayment AS VARCHAR(50); " +
                "DECLARE @startdate AS DATETIME; " +
                "DECLARE @enddate AS DATETIME; " +
                "DECLARE @membershipno AS VARCHAR(50);  " +
                "SET @credit = 'INSTALLMENT'; " +
                "SET @creditpayment = 'INST_PAYMENT' " +
                "SET @startdate = '01 Jan 1990'; " +
                "SET @enddate = '" + startDate + "'; " +
                "SET @membershipno = '%" + membershipNo + "%' " +
                "select a.membershipno, month(a.receiptdate) as receiptmonth, " +
                "isnull(a.debit,0), isnull(b.credit,0), " +
                "(isnull(a.debit,0) - isnull(b.credit,0)) balance from " +
                "(SELECT OrderHdrID AS ReceiptNo, oh.membershipno, OrderDate AS ReceiptDate , " +
                "ISNULL(SUM(Amount),0.00) AS Debit FROM OrderHdr OH " +
                "INNER JOIN ReceiptDet RD ON OH.OrderHdrID = RD.ReceiptHdrID INNER JOIN Membership MM " +
                "on MM.MembershipNo = OH.MembershipNo WHERE OH.IsVoided = 0 AND PaymentType = @credit AND " +
                "OrderDate >= @startdate AND OrderDate < @enddate AND MM.MembershipNo like @membershipno " +
                "GROUP BY OrderDate, oh.membershipno, OrderHdrID  )a " +
                "LEFT JOIN ( " +
                "SELECT OD.Userfld3 " +
                "AS PaymentFor, ISNULL(SUM(AMOUNT),0.00) as credit " +
                "FROM OrderHdr OH INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
                "INNER JOIN Membership MM on MM.MembershipNo = OH.MembershipNo " +
                "WHERE OH.IsVoided = 0 and OD.IsVoided = 0 AND MM.MembershipNo like @membershipno AND " +
                "ItemNo = @creditpayment AND OrderDate >= @startdate AND OrderDate < @enddate " +
                "GROUP BY  OD.Userfld3) b on a.ReceiptNo = b.PaymentFor " +
                "order by membershipno, receiptmonth ";

                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="displayCostPrice"></param>
        /// <param name="DeptID"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortDir"></param>
        /// <returns></returns>
        public static DataTable FetchSaleReturnReport
                   (
            DateTime StartDate, DateTime EndDate,
            string search,
            string PointOfSaleName,
            string SortColumn,
            string SortDir
            )
        {
            try
            {

                string searchSQL = " AND IC.CategoryName +OD.ItemNo+II.ItemName+ISNULL(OD.userfld5,'')+ISNULL(OH.userfld5,'')+OD.OrderHdrID+ ISNULL(II.Barcode, '') like '%' + @search + '%' ";

                string sql =
                    "declare @startdate datetime;   " +
                    "declare @enddate datetime;   " +
                    "declare @search varchar(50);   " +
                    "declare @PointOfSaleName varchar(50);   " +
                    "declare @IsVoided bit;   " +

                    "set @startdate = '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                    "set @enddate = '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                    "set @search = '" + search + "' " +
                    "set @PointOfSaleName = '" + PointOfSaleName + "'; " +
                    "set @IsVoided = 0;   " +

                    " SELECT OH.OrderHdrID , OD.userfld1 as 'SalesPerson', OH.OrderHdrID as ReceiptNo, OD.userfld5 as 'Returned Receipt No', orderdetdate AS 'Receipt Date',LP.PointOfSaleName, II.CategoryName, OD.ItemNo,II.ItemName,OD.Amount,OH.MembershipNo,M.NameToAppear, OH.userfld4 as 'Cheque No', " +
                    "  ISNull(OH.userflag1,'') AS 'Cheque Issued', II.Attributes1 FROM OrderDet OD INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID " +
                    " INNER JOIN Item II ON OD.ItemNo = II.ItemNo INNER JOIN Category IC ON II.CategoryName = IC.CategoryName " +
                " INNER JOIN PointOfSale LP ON OH.PointOfSaleID = LP.PointOfSaleID Inner join Membership M on M.MembershipNo=OH.MembershipNo  " +
                " WHERE OD.quantity < 0 " +
                " AND (OH.OrderDate BETWEEN @startdate AND @enddate) " +
                " AND LP.PointOfSaleName Like '%' + @PointOfSaleName + '%'" +
                " AND OD.IsVoided = @IsVoided " +
                " AND OH.IsVoided = 0 " +
                searchSQL + " order by OH.OrderHdrID,OD.ItemNo,II.ItemName ";


                DataTable finalResult = new DataTable();
                finalResult.TableName = "Result";
                finalResult = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];//, "PowerPOS")).Tables[0];

                return finalResult;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable GetInfoPreOrderForEmail(string orderHdrID, string orderDetID)
        {
            var dt = new EmailPreorderDataSet.DataTable1DataTable();
            DataRow row = dt.NewRow();

            /*get info from order header*/
            OrderHdr hdr = new OrderHdr(orderHdrID);
            if (hdr != null)
            {
                row["OrderDate"] = hdr.OrderDate;


                //string sql = "select r.amount as TotalSales, dbo.GetInstallmentOutstandingBalance(h.OrderHdrID, getdate()) as OutstandingBalance, h.OrderDate " +
                //            "from orderhdr h inner join receipthdr r on h.orderhdrid = r.orderhdrid " +
                //            "where h.OrderHdrID = '" + hdr.OrderHdrID + "'";

                //DataSet ds = DataService.GetDataSet(new QueryCommand(sql));

                //decimal TotalSales = 0;
                //decimal OutstandingBalance = 0;


                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    TotalSales = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalSales"].ToString());
                //    OutstandingBalance = Convert.ToDecimal(ds.Tables[0].Rows[0]["OutstandingBalance"].ToString());
                //}

                //row["InstallmentPayment"] = TotalSales - OutstandingBalance;
                //row["OutstandingBalance"] = OutstandingBalance;
            }

            OrderDet det = new OrderDet(orderDetID);
            if (det != null)
            {
                row["LineNo"] = det.LineInfo;
                row["Quantity"] = det.Quantity;
                row["Price"] = (double)det.UnitPrice;
                Item it = new Item(det.ItemNo);

                row["Attribute1"] = it.Attributes1;
                row["Attribute2"] = string.IsNullOrEmpty(it.Attributes2) ? DateTime.Now.ToString("MMMM yyyy") : it.Attributes2;
                row["ItemImage"] = GetResizeImage(it.ItemImage, 320, 180);
                row["ItemName"] = it.ItemName;

                row["InstallmentPayment"] = det.DepositAmount;
                row["OutstandingBalance"] = det.Amount - det.DepositAmount;
            }

            dt.Rows.Add(row);

            return dt;
        }

        public static byte[] GetResizeImage(byte[] b, int maxwidth, int maxheight)
        {
            byte[] output = new byte[0];

            if (b != null && b.Length > 0)
            {
                // Open a stream for the image and write the bytes into it
                System.IO.MemoryStream stream = new System.IO.MemoryStream(b, true);
                stream.Write(b, 0, b.Length);

                // Create a bitmap from the stream
                Bitmap bmp = new Bitmap(stream);

                Size new_size = new Size();

                //resize based on the longer dimension
                if (bmp.Width > bmp.Height)
                {
                    new_size.Width = maxwidth;
                    new_size.Height = (int)(((double)maxwidth / (double)bmp.Width) * (double)bmp.Height);
                }
                else
                {
                    new_size.Width = (int)(((double)maxheight / (double)bmp.Height) * (double)bmp.Width);
                    new_size.Height = maxheight;
                }
                Bitmap bitmap = new Bitmap(new_size.Width, new_size.Height, bmp.PixelFormat);


                Graphics new_g = Graphics.FromImage(bitmap);

                new_g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                new_g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                new_g.DrawImage(bmp, -1, -1, bitmap.Width + 1, bitmap.Height + 1);

                bmp.Dispose();
                System.IO.MemoryStream streamoutput = new System.IO.MemoryStream();
                //Draw the bitmapt to the outputstream
                bitmap.Save(streamoutput, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
                new_g.Dispose();

                // Close the stream
                stream.Close();

                output = streamoutput.ToArray();
                streamoutput.Close();
            }

            return output;
        }

        public static DataTable FetchPreOrderReport(DateTime StartDate, DateTime EndDate, string ItemName, string MembershipNo,
            string CustName, string IsOutstanding, string Notified, bool OnlyReadyToDeliver, string SortBy, string SortDir, string OutletName, string Status)
        {
            DataTable dt = SPs.FetchPreOrderReport(StartDate, EndDate, ItemName, MembershipNo, CustName, IsOutstanding,
                Notified, OnlyReadyToDeliver, SortBy, SortDir, Status, OutletName).GetDataSet().Tables[0];

            return dt;
        }

        public static string FetchPreOrderReportFromWeb(DateTime StartDate, DateTime EndDate, string ItemName, string MembershipNo,
            string CustName, string IsOutstanding, string Notified, bool OnlyReadyToDeliver, int InventoryLocationID, string SortBy, string SortDir, string Status)
        {
            DataTable dt = SPs.FetchPreOrderReport(StartDate, EndDate, ItemName, MembershipNo, CustName, IsOutstanding,
                Notified, OnlyReadyToDeliver, SortBy, SortDir, Status, "ALL").GetDataSet().Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string status;
                    dr["QtyOnHand"] = InventoryController.GetStockBalanceQtyByItemSummaryByDate(dr["ItemNo"].ToString(),
                                        InventoryLocationID, DateTime.Now, out status);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }

        public static DataTable FetchCampaignReportDetail(int PromoCampaignHdrID, DateTime StartDate, DateTime EndDate, string search, string outletName, string SortBy, string SortDir)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(SortBy))
            {
                SortBy = "OrderHdrID";
            }

            if (string.IsNullOrEmpty(SortDir))
            {
                SortDir = " ASC";
            }

            string query = " Select * from (" +
                           " select distinct oh.OrderHdrID, isnull(oh.userfld5, oh.OrderHdrID) as ReceiptNo, od.Quantity, od.unitprice * od.quantity as grossamount, od.Amount, " +
                           " od.unitprice * od.quantity - od.amount as PromoCost, oh.OrderDate, od.CostOfGoodSold, " +
                           " ISNULL(h.PromoCode, '') as PromoCode, h.PromoCampaignName, h.PromocampaignHdrID, " +
                           " h.ForNonMembersAlso, od.ItemNo, ISNULL(i.ItemName,'') as ItemName, ISNULL(i.CategoryName,'') as CategoryName,  " +
                           " d.UnitQty as PromoQty, d.AnyQty, d.promoprice as PromoPrice, d.DiscPercent, d.DiscAmount,  " +
                           " ps.outletname, op.QtyUsed  " +
                           " from  promocampaignhdr h  " +
                           " inner join PromoCampaignDet d on h.PromoCampaignHdrID = d.PromoCampaignhdrID  " +
                           " inner join orderdet od on od.Promohdrid = h.PromoCampaignHdrID and od.PromodetID = d.PromoCampaignDetID   " +
                           " inner join orderhdr oh on oh.Orderhdrid = od.Orderhdrid  " +
                           " inner join Item i on i.ItemNo = od.ItemNo  " +
                           " inner join PointOfSale ps on oh.PointOfSaleID = ps.PointOfSaleID  " +
                           " LEFT OUTER JOIN " +
                           " ( " +
                           "        select PromoCampaignHdrID, OrderHdrID, min(QtyUsed) as QtyUsed " +
                           "        from " +
                           "        ( " +
                           "            select od.PromohdrID as PromoCampaignHdrID, od.OrderHdrID, od.Quantity / pd.UnitQty as QtyUsed " +
                           "            from orderdet od  " +
                           "            inner join promocampaigndet pd on od.PromoDetID = pd.PromoCampaignDetID " +
                           "            inner join OrderHdr oh on oh.OrderHdrID = od.OrderHdrID " +
                           "            inner join PointofSale op on op.PointOfSaleID = oh.PointOfSaleID " +
                           "            where ISNULL(promodetid,'') != '' and isnull(oh.isvoided,0) = 0 AND cast(oh.OrderDate as date) >= '' + @StartDate +'' AND cast(oh.OrderDate as date) <= '' + @EndDate + ''  " +
                           "            and pd.UnitQty > 0 " +
                           "            UNION " +
                           "            select od.PromohdrID as PromoCampaignHdrID, od.OrderHdrID, sum(od.Quantity) / pd.AnyQty as QtyUsed " +
                           "            from orderdet od  " +
                           "            inner join promocampaigndet pd on od.PromoDetID = pd.PromoCampaignDetID " +
                           "            inner join OrderHdr oh on oh.OrderHdrID = od.OrderHdrID  " +
                           "            inner join PointofSale op on op.PointOfSaleID = oh.PointOfSaleID  " +
                           "            where ISNULL(promodetid,'') != '' and isnull(oh.isvoided,0) = 0 AND cast(oh.OrderDate as date) >= '' + @StartDate +'' AND cast(oh.OrderDate as date) <= '' + @EndDate + ''  " +
                           "            and pd.AnyQty > 0 " +
                           "            group by od.PromoHdrID, pd.AnyQty,  od.OrderHdrID  " +
                           "        ) ee group by  PromoCampaignHdrID, OrderHdrID " +
                           " ) op on op.orderhdrid = oh.orderhdrid and od.PromoHdrID = op.PromoCampaignHdrID " +
                           " WHERE ISNULL(h.Deleted,0) = 0 AND ISNULL(d.deleted,0) = 0 and isnull(oh.isvoided,0) = 0) ot " +
                           " WHERE (PromoCode + ' ' + PromoCampaignName + ' ' + ItemName + ' ' + CategoryName)  LIKE '%' +@Search+ '%' " +
                           " AND PromoCampaignHdrID = @PromoCampaignHdrID AND  cast(OrderDate as date) >= '' + @StartDate +'' AND cast(OrderDate as date) <= '' + @EndDate + ''";
            if (outletName != "ALL" && outletName != "")
            {
                query += "AND OutletName = '" + outletName.Trim() + "'";
            }
            query += string.Format("ORDER BY {0} {1}", SortBy, SortDir);

            QueryCommand cmd = new QueryCommand(query);
            cmd.Parameters.Add("@PromoCampaignHdrID", PromoCampaignHdrID, DbType.Int32);
            cmd.Parameters.Add("@Search", search, DbType.String);
            cmd.Parameters.Add("@StartDate", StartDate.ToString("yyyy-MM-dd"), DbType.String);
            cmd.Parameters.Add("@EndDate", EndDate.ToString("yyyy-MM-dd"), DbType.String);

            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);

            return dt;
        }

        public static DataTable FetchCampaignReport(DateTime StartDate, DateTime EndDate, string search, string outletName, string SortBy, string SortDir)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(SortBy))
            {
                SortBy = "PromoCode";
            }

            if (string.IsNullOrEmpty(SortDir))
            {
                SortDir = " ASC";
            }

            string query = "SELECT DISTINCT h.PromoCampaignHdrID, ISNULL(h.PromoCode, '') as PromoCode, h.PromoCampaignName, ISNULL(op.QtyUsed,0) as PromoUsedWithinPeriod " +
                           ",ISNULL(zz.TotalSold,0) as TotalSold, h.DateFrom, h.DateTo, h.ForNonMembersAlso " +
                           " ,CASE WHEN h.PromoPrice > 0 THEN '$ ' + CONVERT(varchar(50), h.PromoPrice) ELSE CONVERT(varchar(50), h.PromoDiscount) + '%' END AS [PriceDiscount] " +
                           " ,ISNULL(h.IsRestricHour, 0) as IsRestrictHour, /*dbo.GetApplicableDaysPromo(h.PromoCampaignHdrID,0) as ApplicableDays,*/ ol.liststr as OutletName " +
                           " from promocampaignhdr h " +
                           " INNER JOIN PromoCampaignDet d on h.PromoCampaignHdrID = d.PromoCampaignhdrID " +
                           " LEFT OUTER JOIN orderdet od on od.Promohdrid = h.PromoCampaignHdrID and od.PromodetID = d.PromoCampaignDetID " +
                           " LEFT OUTER JOIN orderhdr oh on oh.Orderhdrid = od.Orderhdrid " +
                           " LEFT OUTER JOIN " +
                           " ( " +
                           "    select PromoCampaignHdrID, sum(QtyUsed) as QtyUsed " +
                           "    from  " +
                           "    (  " +
                           "        select PromoCampaignHdrID, OrderHdrID, min(QtyUsed) as QtyUsed " +
                           "        from " +
                           "        ( " +
                           "            select od.PromohdrID as PromoCampaignHdrID, od.OrderHdrID, od.Quantity / pd.UnitQty as QtyUsed " +
                           "            from orderdet od  " +
                           "            inner join promocampaigndet pd on od.PromoDetID = pd.PromoCampaignDetID " +
                           "            inner join OrderHdr oh on oh.OrderHdrID = od.OrderHdrID " +
                           "            inner join PointofSale op on op.PointOfSaleID = oh.PointOfSaleID " +
                           "            where ISNULL(promodetid,'') != '' and isnull(oh.isvoided,0) = 0 AND cast(oh.OrderDate as date) >= '' + @StartDate +'' AND cast(oh.OrderDate as date) <= '' + @EndDate + ''  " +
                           "            and op.OutletName like '' + @Outlet + '' and pd.UnitQty > 0 " +
                           "            UNION " +
                           "            select od.PromohdrID as PromoCampaignHdrID, od.OrderHdrID, sum(od.Quantity) / pd.AnyQty as QtyUsed " +
                           "            from orderdet od  " +
                           "            inner join promocampaigndet pd on od.PromoDetID = pd.PromoCampaignDetID " +
                           "            inner join OrderHdr oh on oh.OrderHdrID = od.OrderHdrID  " +
                           "            inner join PointofSale op on op.PointOfSaleID = oh.PointOfSaleID  " +
                           "            where ISNULL(promodetid,'') != '' and isnull(oh.isvoided,0) = 0 AND cast(oh.OrderDate as date) >= '' + @StartDate +'' AND cast(oh.OrderDate as date) <= '' + @EndDate + ''  " +
                           "            and op.OutletName like '' + @Outlet + '' and pd.AnyQty > 0 " +
                           "            group by od.PromoHdrID, pd.AnyQty,  od.OrderHdrID  " +
                           "        ) ee group by  PromoCampaignHdrID, OrderHdrID " +
                           "     )dd " +
                           "     group by dd.PromoCampaignHdrID " +
                           " )op on op.PromoCampaignHdrID = h.PromoCampaignHdrID " +
                           " LEFT OUTER JOIN Item i on i.ItemNo = d.ItemNo " +
                           " LEFT OUTER JOIN " +
                           " ( " +
                           "     SELECT PromoCampaignHdrID, STUFF((SELECT  ',' + OutletName FROM PromoOutlet EE WHERE EE.PromoCampaignHdrID=E.PromoCampaignHdrID and ISNULL(EE.Deleted,0) = 0 and EE.OutletName like '' + @Outlet + '' ORDER BY OutletName FOR XML PATH('')), 1, 1, '') AS listStr FROM PromoOutlet E where ISNULL(E.Deleted,0) = 0 " +
                           "     GROUP BY E.PromoCampaignHdrID " +
                           " )ol on ol.PromoCampaignHdrID = h.PromoCampaignHdrID " +
                           " LEFT OUTER JOIN " +
                           " ( " +
                           "     select ph.PromoCampaignHdrID, sum(od.Amount) as TotalSold " +
                           "     from orderdet od " +
                           "     inner join orderhdr oh on od.OrderHdrID = oh.OrderHdrID " +
                           "     inner join PromoCampaignHdr ph on od.PromoHdrID = ph.PromoCampaignHdrID " +
                           "     inner join PointofSale op on op.PointOfSaleID = oh.PointOfSaleID " +
                           "     where isnull(oh.isvoided,0) = 0 AND cast(oh.OrderDate as date) >= '' + @StartDate +'' AND cast(oh.OrderDate as date) <= '' + @EndDate + '' " +
                           "     and op.OutletName like '' + @Outlet + '' " +
                           "     group by ph.PromoCampaignHdrID " +
                           " ) zz on zz.PromoCampaignHdrID  = h.PromoCampaignHdrID " +
                           " WHERE ISNULL(h.Deleted,0) = 0 AND ISNULL(d.deleted,0) = 0 " +
                           " AND (h.PromoCode + ' ' + h.PromoCampaignName + ' ' + ISNULL(CONVERT(varchar(50), h.PromoPrice), '') + ' ' " +
                           " + ISNULL(CONVERT(varchar(50), h.PromoDiscount), '')) LIKE '%' + @Search+ '%' ";
            var strOutlet = "";
            if (outletName != "ALL" && outletName != "")
            {
                //query += "AND OutletName = '" + outletName.Trim() + "' ";
                strOutlet = outletName;
            }
            else
            {
                strOutlet = "%";
            }
            query += string.Format("ORDER BY {0} {1}", SortBy, SortDir);

            QueryCommand cmd = new QueryCommand(query);
            cmd.Parameters.Add("@Search", search, DbType.String);
            cmd.Parameters.Add("@StartDate", StartDate.ToString("yyyy-MM-dd"), DbType.String);
            cmd.Parameters.Add("@EndDate", EndDate.ToString("yyyy-MM-dd"), DbType.String);
            cmd.Parameters.Add("@Outlet", strOutlet, DbType.String);

            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);

            return dt;
        }


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DataTable FetchProductSalesReportWithoutRedeem(DateTime startdate,
                DateTime enddate,
                string searchQry,
                string PointOfSaleName,
                string OutletName,
                string CategoryName,
                string DeptID,
                bool IsVoided,
                string SortColumn,
                string SortDir)
        {
            DataTable dt = new DataTable();

            try
            {
                if (OutletName == "") { OutletName = "ALL"; }
                if (OutletName == "ALL - BreakDown") { OutletName = "%"; }
                if (SortColumn == "") { SortColumn = "DepartmentName"; }
                if (SortDir == "") { SortDir = "DESC"; }
                if (DeptID == "0") { DeptID = ""; }
                string query = @" SELECT     ID.DepartmentName
		                                    ,C.CategoryName
		                                    ,I.ItemNo
		                                    ,I.ItemName
		                                    ,CAST(SUM(OD.Quantity) as decimal(18,2)) Quantity
		                                    ,SUM(CASE WHEN RD.TotalAmount > 0 THEN OD.Amount * (RD.NormalAmount/RD.TotalAmount) ELSE 0 END) Amount
		                                    ,ISNULL(I.Attributes1,0) Attributes1
		                                    ,ISNULL(I.Attributes2,0) Attributes2
		                                    ,ISNULL(I.Attributes3,0) Attributes3
		                                    ,ISNULL(I.Attributes4,0) Attributes4
		                                    ,ISNULL(I.Attributes5,0) Attributes5
		                                    ,ISNULL(I.Attributes6,0) Attributes6
		                                    ,ISNULL(I.Attributes7,0) Attributes7	 	
		                                    ,ISNULL(I.Attributes8,0) Attributes8	 	
		                                    ,OU.OutletName
		                                    ,POS.PointOfSaleName
                                            ,ISNULL(SUPP.SupplierName,'') as SupplierName				
                                    FROM	OrderHdr OH
		                                    INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
		                                    INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
		                                    INNER JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
		                                    INNER JOIN Item I ON I.ItemNo = OD.ItemNo
		                                    INNER JOIN Category C ON C.CategoryName = I.CategoryName
		                                    INNER JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
		                                    INNER JOIN ( 
			                                    SELECT ReceiptHdrID 
				                                    , SUM(CASE WHEN PaymentType NOT IN ('POINTS', 'PACKAGE') THEN Amount ELSE 0 END) NormalAmount 
				                                    , SUM(CASE WHEN PaymentType IN ('POINTS', 'PACKAGE') THEN Amount ELSE 0 END) PointUsageAmount 
				                                    , SUM(Amount) TotalAmount
			                                    FROM ReceiptDet 
			                                    GROUP BY ReceiptHdrID
		                                    ) RD ON OH.OrderHdrID = RD.ReceiptHdrID 
                                            LEFT JOIN (
			                                    SELECT  ISM.ItemNo
					                                    ,ISM.SupplierID
                                                        ,SP.SupplierName
					                                    ,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
			                                    FROM ItemSupplierMap ISM
                                                LEFT JOIN Supplier SP on ISM.SupplierID = SP.SupplierID
			                                    WHERE ISNULL(ISM.Deleted,0) = 0
		                                    ) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
                                    WHERE	OH.IsVoided = 0
		                                    AND CASE WHEN RD.TotalAmount > 0 THEN OD.Amount * (RD.NormalAmount/RD.TotalAmount) ELSE 0 END > 0
                                            AND OH.OrderDate BETWEEN '{0}' AND '{1}'
                                            AND (POS.PointOfSaleName LIKE '%{2}%' OR '{2}'='ALL' OR '{2}'='')
                                            AND (OU.OutletName LIKE '%{3}%' OR '{3}'='ALL' OR '{3}'='')
                                            AND (C.CategoryName LIKE '%{4}%' OR '{4}'='ALL' OR '{4}'='')
                                            AND (ID.DepartmentName LIKE '%{5}%' OR '{5}'='ALL' OR '{5}'='')
                                            AND ISNULL(ID.DepartmentName,'')+'%'+ 
                                                ISNULL(C.CategoryName,'')+'%'+ 
                                                ISNULL(I.ItemNo,'')+'%'+ 
                                                ISNULL(I.ItemName,'')+'%'+ 
                                                ISNULL(I.Attributes1,'')+'%'+ 
                                                ISNULL(I.Attributes2,'')+'%'+ 
                                                ISNULL(I.Attributes3,'')+'%'+ 
                                                ISNULL(I.Attributes4,'')+'%'+ 
                                                ISNULL(I.Attributes5,'')+'%'+ 
                                                ISNULL(I.Attributes6,'')+'%'+ 
                                                ISNULL(I.Attributes7,'')+'%'+ 
                                                ISNULL(I.Attributes8,'')+'%'+ 
                                                ISNULL(OU.OutletName,'')+'%'+
                                                ISNULL(SUPP.SupplierName,'')+'%'+ 
                                                ISNULL(POS.PointOfSaleName,'')+'%' LIKE '%{6}%'
                                    GROUP BY ID.DepartmentName
		                                    ,C.CategoryName
		                                    ,I.ItemNo
		                                    ,I.ItemName
		                                    ,ISNULL(I.Attributes1,0) 
		                                    ,ISNULL(I.Attributes2,0) 
		                                    ,ISNULL(I.Attributes3,0) 
		                                    ,ISNULL(I.Attributes4,0) 
		                                    ,ISNULL(I.Attributes5,0) 
		                                    ,ISNULL(I.Attributes6,0) 
		                                    ,ISNULL(I.Attributes7,0) 	 	
		                                    ,ISNULL(I.Attributes8,0) 	 	
		                                    ,OU.OutletName
		                                    ,POS.PointOfSaleName
                                            ,ISNULL(SUPP.SupplierName,'')
                                    ORDER BY {7} {8} ";
                query = string.Format(query, startdate.ToString("yyyy-MM-dd HH:mm:ss")
                                           , enddate.ToString("yyyy-MM-dd HH:mm:ss")
                                           , PointOfSaleName
                                           , OutletName
                                           , CategoryName
                                           , DeptID
                                           , searchQry
                                           , SortColumn
                                           , SortDir);
                dt.Load(DataService.GetReader(new QueryCommand(query)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable GetLNPreOrderReport(DateTime StartDate, DateTime EndDate, string ItemDepartment, string Category, string Outlet, string Search)
        {
            if (string.IsNullOrEmpty(Category))
                Category = "ALL";

            if (string.IsNullOrEmpty(Outlet))
                Outlet = "ALL";

            if (string.IsNullOrEmpty(ItemDepartment))
                ItemDepartment = "ALL";

            String sql = @"DECLARE @StartDate datetime; 
                            DECLARE @EndDate datetime; 
                            DECLARE @Outlet varchar(50); 
                            DECLARE @ItemDepartment varchar(50); 
                            DECLARE @Category nvarchar(250); 

                            SET @StartDate = '{0}' + ' 00:00:00'; 
                            SET @EndDate = '{1}' + ' 23:59:59'; 
                            SET @Outlet = '{2}'; 
                            SET @ItemDepartment = '{3}'; 
                            SET @Category = '{4}'; 


                            SELECT 
	                             Convert(varchar(50), do.TimeSlotFrom, 120) as OrderDate, 
	                             ISNULL(oh.userfld5, oh.OrderRefNo) as OrderRefNo, 
	                             it.itemno,
	                             CONVERT(Decimal(18,2), (od.Amount - od.GSTAmount) / case ISNULL(od.Quantity,1) when 0 then 1 else ISNULL(od.Quantity,1) end)  as UnitPrice,
	                             od.Quantity ,
	                             0 as LineDiscountAmount,
	                             0 as LineDiscountPercent,
	                             m.NameToAppear,
	                             pos.OutletName	 
                            FROM OrderHdr oh 
                                 INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID 
                                 INNER JOIN PointOfSale pos ON pos.PointOfSaleID = oh.PointOfSaleID 
                                 INNER JOIN Item it ON it.ItemNo = od.ItemNo
                                 INNER JOIN Category ct ON ct.CategoryName = it.CategoryName 
                                 INNER JOIN ItemDepartment id ON id.ItemDepartmentID = ct.ItemDepartmentId 
                                 LEFT JOIN Membership m on oh.MembershipNo = m.MembershipNo   
                                 INNER JOIN DeliveryOrder do on do.SalesOrderRefNo = oh.OrderHdrID
                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0 
                                  AND (ISNULL(oh.userfld8, '') = '' OR oh.userfld8 = 'PREORDER') 
                                  AND ((it.IsInInventory = 1 and it.CategoryName != 'SYSTEM') or (It.ItemNo = 'LINE_DISCOUNT'))  
                                  AND do.TimeSlotFrom BETWEEN @StartDate AND @EndDate 
                                  AND ISNULL(pos.OutletName,'') LIKE CASE WHEN @Outlet = 'ALL' THEN '%' ELSE @Outlet END  
                                  AND ISNULL(id.ItemDepartmentID,'') LIKE CASE WHEN @ItemDepartment = 'ALL' THEN '%' ELSE @ItemDepartment END  
                                  AND ISNULL(ct.CategoryName,'') LIKE CASE WHEN @Category = 'ALL' THEN '%' ELSE @Category END  
	                             AND it.ItemNo + ISNULL(m.NameToAppear,'')  like '%' + '{5}' +'%'  
                            ORDER BY oh.OrderDate, it.ItemNo
                            ";

            sql = string.Format(sql, StartDate.ToString("yyyy-MM-dd"),
                                      EndDate.ToString("yyyy-MM-dd"),
                                      Outlet,
                                      ItemDepartment,
                                      Category,
                                      Search);

            DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

            return dt;
        }


        public static DataTable GetLNCCReport(DateTime StartDate, DateTime EndDate, string ItemDepartment, string Category, string Outlet, string Search)
        {
            if (string.IsNullOrEmpty(Category))
                Category = "ALL";

            if (string.IsNullOrEmpty(Outlet))
                Outlet = "ALL";

            if (string.IsNullOrEmpty(ItemDepartment))
                ItemDepartment = "ALL";

            String sql = @"DECLARE @StartDate datetime; 
                            DECLARE @EndDate datetime; 
                            DECLARE @Outlet varchar(50); 
                            DECLARE @ItemDepartment varchar(50); 
                            DECLARE @Category nvarchar(250); 

                            SET @StartDate = '{0}' + ' 00:00:00'; 
                            SET @EndDate = '{1}' + ' 23:59:59'; 
                            SET @Outlet = '{2}'; 
                            SET @ItemDepartment = '{3}'; 
                            SET @Category = '{4}'; 


                            SELECT 
	                             Convert(varchar(50), oh.OrderDate, 120) as OrderDate,
	                             ISNULL(oh.userfld5, oh.OrderRefNo) as OrderRefNo,  
	                             it.itemno,
	                             CONVERT(Decimal(18,2), (od.Amount - od.GSTAmount) / case ISNULL(od.Quantity,1) when 0 then 1 else ISNULL(od.Quantity,1) end)  as UnitPrice,
	                             od.Quantity ,
	                             0 as LineDiscountAmount,
	                             0 as LineDiscountPercent,
	                             m.NameToAppear,
	                             pos.OutletName	 
                            FROM OrderHdr oh 
                                 INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID 
                                 INNER JOIN PointOfSale pos ON pos.PointOfSaleID = oh.PointOfSaleID 
                                 INNER JOIN Item it ON it.ItemNo = od.ItemNo 
                                 INNER JOIN Category ct ON ct.CategoryName = it.CategoryName 
                                 INNER JOIN ItemDepartment id ON id.ItemDepartmentID = ct.ItemDepartmentId 
                                 LEFT JOIN Membership m on oh.MembershipNo = m.MembershipNo   
                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0 
                                  AND (ISNULL(oh.userfld8, '') = '' OR oh.userfld8 = 'CASH_CARRY') 
                                  AND ((it.IsInInventory = 1 and it.CategoryName != 'SYSTEM') or (It.ItemNo = 'LINE_DISCOUNT'))  
                                  AND oh.OrderDate BETWEEN @StartDate AND @EndDate 
                                  AND ISNULL(pos.OutletName,'') LIKE CASE WHEN @Outlet = 'ALL' THEN '%' ELSE @Outlet END  
                                  AND ISNULL(id.ItemDepartmentID,'') LIKE CASE WHEN @ItemDepartment = 'ALL' THEN '%' ELSE @ItemDepartment END  
                                  AND ISNULL(ct.CategoryName,'') LIKE CASE WHEN @Category = 'ALL' THEN '%' ELSE @Category END  
	                              AND it.ItemNo + ISNULL(m.NameToAppear,'')  like '%' + '{5}' +'%'  
                            ORDER BY oh.OrderDate, it.ItemNo
                            ";

            sql = string.Format(sql, StartDate.ToString("yyyy-MM-dd"),
                                      EndDate.ToString("yyyy-MM-dd"),
                                      Outlet,
                                      ItemDepartment,
                                      Category,
                                      Search);

            DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

            return dt;
        }

        public static DataTable FetchDeliveryOrderReport(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();
            DataSet ds = SPs.ReportDeliveryOrder(startDate, endDate).GetDataSet();
            dt = ds.Tables[0];

            return dt;
        }
    }
}
