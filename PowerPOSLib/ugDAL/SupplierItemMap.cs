using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    /// <summary>
    /// This DAL only supports Insert for now.
    /// </summary>
    public class SupplierItemMap
    {
        static string providerName = "PowerPOS";

        public int SupplierID;
        public string ItemNo;
        public Nullable<DateTime> CreatedOn;
        public string CreatedBy;
        public Nullable<DateTime> ModifiedOn;
        public string ModifiedBy;
        public Guid UniqueID;
        public bool Deleted;
        public string userfld1;
        public string userfld2;
        public string userfld3;
        public string userfld4;
        public string userfld5;
        public string userfld6;
        public string userfld7;
        public string userfld8;
        public string userfld9;
        public string userfld10;
        public Nullable<bool> userflag1;
        public Nullable<bool> userflag2;
        public Nullable<bool> userflag3;
        public Nullable<bool> userflag4;
        public Nullable<bool> userflag5;
        public Nullable<double> userfloat1;
        public Nullable<double> userfloat2;
        public Nullable<double> userfloat3;
        public Nullable<double> userfloat4;
        public Nullable<double> userfloat5;
        public Nullable<int> userint1;
        public Nullable<int> userint2;
        public Nullable<int> userint3;
        public Nullable<int> userint4;
        public Nullable<int> userint5;

        /// <summary>
        /// Only insert is supported for now.
        /// </summary>
        public void Insert()
        {
            try
            {
                QueryCommand cmd = new QueryCommand("INSERT INTO SupplierItemMap VALUES (@SupplierID, @ItemNo, @CreatedOn, " +
                    "@CreatedBy, @ModifiedOn, @ModifiedBy, @UniqueID, @Deleted, @userfld1, @userfld2, @userfld3, " +
                    "@userfld4, @userfld5, @userfld6, @userfld7, @userfld8, @userfld9, @userfld10, @userflag1, @userflag2, " +
                    "@userflag3, @userflag4, @userflag5, @userfloat1, @userfloat2, @userfloat3, @userfloat4, @userfloat5, " +
                    "@userint1, @userint2, @userint3, @userint4, @userint5)",
                    providerName);

                cmd.Parameters.Add("@SupplierID", SupplierID);
                cmd.Parameters.Add("@ItemNo", ItemNo);
                cmd.Parameters.Add("@CreatedOn", CreatedOn);
                cmd.Parameters.Add("@CreatedBy", CreatedBy);
                cmd.Parameters.Add("@ModifiedOn", ModifiedOn);
                cmd.Parameters.Add("@ModifiedBy", ModifiedBy);
                cmd.Parameters.Add("@UniqueID", UniqueID, DbType.Guid);
                cmd.Parameters.Add("@Deleted", Deleted);
                cmd.Parameters.Add("@userfld1", userfld1);
                cmd.Parameters.Add("@userfld2", userfld2);
                cmd.Parameters.Add("@userfld3", userfld3);
                cmd.Parameters.Add("@userfld4", userfld4);
                cmd.Parameters.Add("@userfld5", userfld5);
                cmd.Parameters.Add("@userfld6", userfld6);
                cmd.Parameters.Add("@userfld7", userfld7);
                cmd.Parameters.Add("@userfld8", userfld8);
                cmd.Parameters.Add("@userfld9", userfld9);
                cmd.Parameters.Add("@userfld10", userfld10);
                cmd.Parameters.Add("@userflag1", userflag1);
                cmd.Parameters.Add("@userflag2", userflag2);
                cmd.Parameters.Add("@userflag3", userflag3);
                cmd.Parameters.Add("@userflag4", userflag4);
                cmd.Parameters.Add("@userflag5", userflag5);
                cmd.Parameters.Add("@userfloat1", userfloat1);
                cmd.Parameters.Add("@userfloat2", userfloat2);
                cmd.Parameters.Add("@userfloat3", userfloat3);
                cmd.Parameters.Add("@userfloat4", userfloat4);
                cmd.Parameters.Add("@userfloat5", userfloat5);
                cmd.Parameters.Add("@userint1", userint1);
                cmd.Parameters.Add("@userint2", userint2);
                cmd.Parameters.Add("@userint3", userint3);
                cmd.Parameters.Add("@userint4", userint4);
                cmd.Parameters.Add("@userint5", userint5);

                DataService.ExecuteQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QueryCommand GetInsertCommand()
        {
            try
            {
                QueryCommand cmd = new QueryCommand("INSERT INTO SupplierItemMap VALUES (@SupplierID, @ItemNo, @CreatedOn, " +
                    "@CreatedBy, @ModifiedOn, @ModifiedBy, @UniqueID, @Deleted, @userfld1, @userfld2, @userfld3, " +
                    "@userfld4, @userfld5, @userfld6, @userfld7, @userfld8, @userfld9, @userfld10, @userflag1, @userflag2, " +
                    "@userflag3, @userflag4, @userflag5, @userfloat1, @userfloat2, @userfloat3, @userfloat4, @userfloat5, " +
                    "@userint1, @userint2, @userint3, @userint4, @userint5)",
                    providerName);

                cmd.Parameters.Add("@SupplierID", SupplierID);
                cmd.Parameters.Add("@ItemNo", ItemNo);
                cmd.Parameters.Add("@CreatedOn", CreatedOn);
                cmd.Parameters.Add("@CreatedBy", CreatedBy);
                cmd.Parameters.Add("@ModifiedOn", ModifiedOn);
                cmd.Parameters.Add("@ModifiedBy", ModifiedBy);
                cmd.Parameters.Add("@UniqueID", UniqueID, DbType.Guid);
                cmd.Parameters.Add("@Deleted", Deleted);
                cmd.Parameters.Add("@userfld1", userfld1);
                cmd.Parameters.Add("@userfld2", userfld2);
                cmd.Parameters.Add("@userfld3", userfld3);
                cmd.Parameters.Add("@userfld4", userfld4);
                cmd.Parameters.Add("@userfld5", userfld5);
                cmd.Parameters.Add("@userfld6", userfld6);
                cmd.Parameters.Add("@userfld7", userfld7);
                cmd.Parameters.Add("@userfld8", userfld8);
                cmd.Parameters.Add("@userfld9", userfld9);
                cmd.Parameters.Add("@userfld10", userfld10);
                cmd.Parameters.Add("@userflag1", userflag1);
                cmd.Parameters.Add("@userflag2", userflag2);
                cmd.Parameters.Add("@userflag3", userflag3);
                cmd.Parameters.Add("@userflag4", userflag4);
                cmd.Parameters.Add("@userflag5", userflag5);
                cmd.Parameters.Add("@userfloat1", userfloat1);
                cmd.Parameters.Add("@userfloat2", userfloat2);
                cmd.Parameters.Add("@userfloat3", userfloat3);
                cmd.Parameters.Add("@userfloat4", userfloat4);
                cmd.Parameters.Add("@userfloat5", userfloat5);
                cmd.Parameters.Add("@userint1", userint1);
                cmd.Parameters.Add("@userint2", userint2);
                cmd.Parameters.Add("@userint3", userint3);
                cmd.Parameters.Add("@userint4", userint4);
                cmd.Parameters.Add("@userint5", userint5);

                return cmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QueryCommand GetInsertIfNotExistsCommand()
        {
            try
            {
                QueryCommand cmd = new QueryCommand(
                    "IF EXISTS ( SELECT SupplierID FROM SupplierItemMap WHERE SupplierID = @SupplierID " + 
                        "AND ItemNo = @ItemNo ) " +
                    "BEGIN " +
                        "SELECT 0 " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                        "INSERT INTO SupplierItemMap VALUES (@SupplierID, @ItemNo, @CreatedOn, " +
                            "@CreatedBy, @ModifiedOn, @ModifiedBy, @UniqueID, @Deleted, @userfld1, @userfld2, @userfld3, " +
                            "@userfld4, @userfld5, @userfld6, @userfld7, @userfld8, @userfld9, @userfld10, @userflag1, @userflag2, " +
                            "@userflag3, @userflag4, @userflag5, @userfloat1, @userfloat2, @userfloat3, @userfloat4, @userfloat5, " +
                            "@userint1, @userint2, @userint3, @userint4, @userint5)" + 
                    "END",
                    providerName);

                cmd.Parameters.Add("@SupplierID", SupplierID);
                cmd.Parameters.Add("@ItemNo", ItemNo);
                cmd.Parameters.Add("@CreatedOn", CreatedOn);
                cmd.Parameters.Add("@CreatedBy", CreatedBy);
                cmd.Parameters.Add("@ModifiedOn", ModifiedOn);
                cmd.Parameters.Add("@ModifiedBy", ModifiedBy);
                cmd.Parameters.Add("@UniqueID", UniqueID, DbType.Guid);
                cmd.Parameters.Add("@Deleted", Deleted);
                cmd.Parameters.Add("@userfld1", userfld1);
                cmd.Parameters.Add("@userfld2", userfld2);
                cmd.Parameters.Add("@userfld3", userfld3);
                cmd.Parameters.Add("@userfld4", userfld4);
                cmd.Parameters.Add("@userfld5", userfld5);
                cmd.Parameters.Add("@userfld6", userfld6);
                cmd.Parameters.Add("@userfld7", userfld7);
                cmd.Parameters.Add("@userfld8", userfld8);
                cmd.Parameters.Add("@userfld9", userfld9);
                cmd.Parameters.Add("@userfld10", userfld10);
                cmd.Parameters.Add("@userflag1", userflag1);
                cmd.Parameters.Add("@userflag2", userflag2);
                cmd.Parameters.Add("@userflag3", userflag3);
                cmd.Parameters.Add("@userflag4", userflag4);
                cmd.Parameters.Add("@userflag5", userflag5);
                cmd.Parameters.Add("@userfloat1", userfloat1);
                cmd.Parameters.Add("@userfloat2", userfloat2);
                cmd.Parameters.Add("@userfloat3", userfloat3);
                cmd.Parameters.Add("@userfloat4", userfloat4);
                cmd.Parameters.Add("@userfloat5", userfloat5);
                cmd.Parameters.Add("@userint1", userint1);
                cmd.Parameters.Add("@userint2", userint2);
                cmd.Parameters.Add("@userint3", userint3);
                cmd.Parameters.Add("@userint4", userint4);
                cmd.Parameters.Add("@userint5", userint5);

                return cmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckIfExistBySupplierIDAndItemNo(int SupplierID, string ItemNo)
        {
            try
            {
                QueryCommand cmd = new QueryCommand(
                    "SELECT SupplierID FROM SupplierItemMap WHERE SupplierID = @SupplierID AND ItemNo = @ItemNo AND Deleted = 0"
                    ,providerName);

                cmd.Parameters.Add("@SupplierID", SupplierID);
                cmd.Parameters.Add("@ItemNo", ItemNo);

                DataSet ds = DataService.GetDataSet(cmd);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static QueryCommand DeleteSupplierItemMapByItemNo(string ItemNo)
        {
            try
            {
                QueryCommand cmd = new QueryCommand("DELETE FROM SupplierItemMap WHERE ItemNo = @ItemNo", providerName);
                cmd.Parameters.Add("@ItemNo", ItemNo);
                return cmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static QueryCommand WipeOutTable()
        {
            try
            {
                QueryCommand cmd = new QueryCommand("DELETE FROM SupplierItemMap", providerName);
                return cmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetSupplierItemMapListByItemNo(string ItemNo)
        {
            try
            {
                DataSet ds = new DataSet();

                QueryCommand cmd = new QueryCommand("SELECT SIM.SupplierID, SUP.SupplierName FROM SupplierItemMap SIM " +
                    "INNER JOIN Supplier SUP ON SIM.SupplierID = SUP.SupplierID " +
                    "WHERE SIM.ItemNo = @ItemNo", providerName);
                cmd.Parameters.Add("@ItemNo", ItemNo);

                return DataService.GetDataSet(cmd); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateSupplierItemMapTable()
        {
            string sqlCheckSupplierItemMapTable =
                "IF (NOT EXISTS (SELECT * " +
                     "FROM INFORMATION_SCHEMA.TABLES " +
                     "WHERE TABLE_NAME = 'SupplierItemMap')) " +
                "BEGIN " +
                    "CREATE TABLE [dbo].[SupplierItemMap]( " +
	                    "[SupplierID] [int] NULL, " +
	                    "[ItemNo] [varchar](50) NULL, " +
	                    "[CreatedOn] [datetime] NULL, " +
	                    "[CreatedBy] [varchar](50) NULL, " +
	                    "[ModifiedOn] [datetime] NULL, " +
	                    "[ModifiedBy] [varchar](50) NULL, " +
	                    "[UniqueID] [uniqueidentifier] NOT NULL, " +
	                    "[Deleted] [bit] NULL, " +
	                    "[userfld1] [varchar](50) NULL, " +
	                    "[userfld2] [varchar](50) NULL, " +
	                    "[userfld3] [varchar](50) NULL, " +
	                    "[userfld4] [varchar](50) NULL, " +
	                    "[userfld5] [varchar](50) NULL, " +
	                    "[userfld6] [varchar](50) NULL, " +
	                    "[userfld7] [varchar](50) NULL, " +
	                    "[userfld8] [varchar](50) NULL, " +
	                    "[userfld9] [varchar](50) NULL, " +
	                    "[userfld10] [varchar](50) NULL, " +
	                    "[userflag1] [bit] NULL, " +
	                    "[userflag2] [bit] NULL, " +
	                    "[userflag3] [bit] NULL, " +
	                    "[userflag4] [bit] NULL, " +
	                    "[userflag5] [bit] NULL, " +
	                    "[userfloat1] [money] NULL, " +
	                    "[userfloat2] [money] NULL, " +
	                    "[userfloat3] [money] NULL, " +
	                    "[userfloat4] [money] NULL, " +
	                    "[userfloat5] [money] NULL, " +
	                    "[userint1] [int] NULL, " +
	                    "[userint2] [int] NULL, " +
	                    "[userint3] [int] NULL, " +
	                    "[userint4] [int] NULL, " +
	                    "[userint5] [int] NULL, " +
                    "CONSTRAINT [PK_SupplierItemMap] PRIMARY KEY CLUSTERED " +
                    "( " +
                        "[UniqueID] ASC " +
                    ") WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                    ") ON [PRIMARY] " +
                "END";
            QueryCommand checkSupplierItemMapTable = new QueryCommand(sqlCheckSupplierItemMapTable, "PowerPOS");
            DataService.ExecuteQuery(checkSupplierItemMapTable);
        }
    }
}
