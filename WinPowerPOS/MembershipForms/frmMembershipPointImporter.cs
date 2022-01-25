using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PowerPOS;
using SubSonic;

namespace WinPowerPOS.MembershipForms
{
    

    public partial class frmMembershipPointImporter : Form
    {
        private string POINT_ITEM = "I03700000007";

        public frmMembershipPointImporter()
        {
            InitializeComponent();
        }

        private void frmMembershipPointImporter_Load(object sender, EventArgs e)
        {

            string SQL = "Select ItemNo,ItemName,userfld10 from Item where Userfld10 <> '' AND not Userfld10 is null AND Userfld10 <> 'N'";
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            DataTable dt = DataService.GetDataSet(cmd).Tables[0];
            cmbPointItems.DataSource = dt;
            cmbPointItems.DisplayMember = "ItemName";
            cmbPointItems.Refresh();
            
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            ///
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                DataTable message = null;
                DataTable ErrorDb;
                //DataSet ds = null;

                FileInfo inf = new FileInfo(openFileDialog1.FileName);
                
                if (inf.Extension.ToLower() == ".xls")
                {
                    if (!ExcelController.ImportExcelXLS
                        (openFileDialog1.FileName, out message, true))
                        throw new Exception("");
                }
                
                if (message == null || message.Rows.Count < 1)
                    throw new Exception("No data inside");

                dgvMembership.DataSource = message;
                dgvMembership.Refresh();
                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error. Load FAILED!." + ex.Message);
            }
        }

        QueryCommandCollection Cmds;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPointItems.SelectedValue == null || cmbPointItems.SelectedIndex < 0)
                {
                    MessageBox.Show("You dont have no point/package items");
                    return;
                }
                DataTable dt = ((DataTable)dgvMembership.DataSource);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }
                DataRowView dr = (DataRowView)cmbPointItems.SelectedItem;
                POINT_ITEM = dr["ItemNo"].ToString();
                //loop through data table
                //add points to update all....

                string status;
                decimal diffPoint, initialPoint;
                Cmds = new QueryCommandCollection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string membershipNo = "";
                    Membership m;
                    if (MembershipController.IsExistingMember
                        (dt.Rows[i]["MembershipNo"].ToString(), out m))
                    {
                        membershipNo = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        if (MembershipController.IsMembersNRIC(dt.Rows[i]["MembershipNo"].ToString(), out m))
                        {

                            if (m != null)
                            {
                                membershipNo = m.MembershipNo;
                            }
                            else
                            {
                                MessageBox.Show("Error. Members at row " + (i + 1).ToString() + " are not registered in the system or has already expired. Membership No > " + dt.Rows[i][0].ToString());
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error. Members at row " + (i + 1).ToString() + " are not registered in the system or has already expired. Membership No > " + dt.Rows[i][0].ToString());
                            return;
                        }
                    }
                    //add points....
                    decimal point = 0;
                    if (decimal.TryParse(dt.Rows[i]["Points"].ToString(), out point))
                    {
                        UpdateAll(POINT_ITEM, point, "IMPORTER", DateTime.Now, 6,
                            membershipNo, "", "", out initialPoint,
                            out diffPoint, out status);
                    }
                    else
                    {
                        MessageBox.Show("Points in wrong format for Row No " + (i + 1).ToString());
                        return;
                    }

                }
                DataService.ExecuteTransaction(Cmds);
                MessageBox.Show("Successful!");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public bool UpdateAll(string ItemNo, decimal pointAmount, string OrderHdrID, 
            DateTime TransactionDate
            , int ValidPeriods, string MembershipNo, string SalesPersonID, string UserName
            , out decimal InitialPoint, out decimal DiffPoint, out string Status)
        {
            InitialPoint = 0;
            DiffPoint = 0;
            Status = "";

            try
            {
                DateTime StartValidPeriod, EndValidPeriod;
                #region *) Initialize: Set StartValidPeriod & EndValidPeriod
                StartValidPeriod = TransactionDate;
                if (ValidPeriods < 1)
                {   /// Valid Periods = 100 Years
                    EndValidPeriod = StartValidPeriod.AddYears(100).AddMilliseconds(-1);
                }
                else
                {   /// From TransactionDate till Expiry
                    EndValidPeriod = StartValidPeriod.AddMonths(ValidPeriods).AddMilliseconds(-1);
                }
                #endregion

                #region *) Initialize: Set InitialPoint (Dollar Type only)
                string GetInitialPoint =
                   "SELECT ISNULL(SUM(Points),0) FROM MembershipPoints " +
                   "WHERE StartValidPeriod <= @StartDate AND EndValidPeriod >= @EndDate " +
                       "AND MembershipNo = @MembershipNo AND userfld2 = @PointType";
                QueryCommand CmdInitialPoint = new QueryCommand(GetInitialPoint);
                CmdInitialPoint.AddParameter("@StartDate", TransactionDate, DbType.DateTime);
                CmdInitialPoint.AddParameter("@EndDate", TransactionDate, DbType.DateTime);
                CmdInitialPoint.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                CmdInitialPoint.AddParameter("@PointType", Item.PointMode.Dollar, DbType.String);
                InitialPoint = (decimal)SubSonic.DataService.ExecuteScalar(CmdInitialPoint);
                #endregion

                //foreach (DataRow Rw in PointData.Rows)
                {
                    decimal OneDiffPoint = pointAmount;
                    /*
                    #region *) Initialize: Parse OneDiffPoint (Total Points changed for 1 transaction)
                    if (!decimal.TryParse(Rw[1].ToString(), out OneDiffPoint))
                        throw new Exception("(warning)Cannot parse Point Value for Package " + Rw[0].ToString());
                    #endregion

                    #region *) Core: If PointType == Dollar, append to DiffPoint
                    if (Rw[2].ToString() == Item.PointMode.Dollar)
                        DiffPoint += OneDiffPoint;
                    #endregion
                    */
                    if (OneDiffPoint > 0)
                    {
                        #region *) Core: Upsert MembershipPoint
                        string AddQuery =
                            "IF(( " +
                                "SELECT COUNT(*) " +
                                "FROM MembershipPoints " +
                                "WHERE MembershipNo = @MembershipNo " +
                                    "AND StartValidPeriod <= @TransactionDate " +
                                    "AND EndValidPeriod >= @TransactionDate " +
                                    "AND " + MembershipPoint.UserColumns.PackageItemNo + " = @ItemNo " +
                                ")>0) " +
                            "BEGIN " +
                                "UPDATE MembershipPoints " +
                                "SET Points = Points + @DiffPoints, EndValidPeriod=@EndValidPeriod,ModifiedOn = @TransactionDate, ModifiedBy = @UserName " +
                                "WHERE PointID = " +
                                    "(SELECT TOP 1 PointID " +
                                    "FROM MembershipPoints " +
                                    "WHERE MembershipNo = @MembershipNo " +
                                        "AND StartValidPeriod <= @TransactionDate " +
                                        "AND EndValidPeriod >= @TransactionDate " +
                                        "AND " + MembershipPoint.UserColumns.PackageItemNo + " = @ItemNo " +
                                        "AND Points <> 0 " +
                                    "ORDER BY StartValidPeriod,PointID) " +
                            "END " +
                            "ELSE " +
                            "BEGIN " +
                                "INSERT INTO [MembershipPoints] " +
                                "([StartValidPeriod],[EndValidPeriod],[MembershipNo],[Points],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[" + MembershipPoint.UserColumns.PackageItemNo + "],[" + MembershipPoint.UserColumns.PackageType + "],[" + MembershipPoint.UserColumns.CourseBreakdownPrice + "]) " +
                                "SELECT " +
                                "@StartValidPeriod,@EndValidPeriod,@MembershipNo,@DiffPoints,@TransactionDate,@UserName,@TransactionDate,@UserName,@ItemNo,@PointType," + Item.UserColumns.PointUnitPrice + " " +
                                "FROM Item WHERE [ItemNo] = @ItemNo " +
                            "END";
                        QueryCommand AddCmd = new QueryCommand(AddQuery);
                        AddCmd.AddParameter("@TransactionDate", TransactionDate, DbType.DateTime);
                        AddCmd.AddParameter("@StartValidPeriod", StartValidPeriod, DbType.DateTime);
                        AddCmd.AddParameter("@EndValidPeriod", EndValidPeriod, DbType.DateTime);
                        AddCmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                        AddCmd.AddParameter("@DiffPoints", OneDiffPoint, DbType.Decimal);
                        AddCmd.AddParameter("@ItemNo", ItemNo, DbType.String);
                        AddCmd.AddParameter("@PointType", "D", DbType.String);
                        AddCmd.AddParameter("@UserName", UserName, DbType.String);

                        Cmds.Add(AddCmd);
                        #endregion
                    }
                    else if (OneDiffPoint < 0)
                    {
                        decimal absDiffPoint = Math.Abs(OneDiffPoint);

                        DataSet ds = MembershipPoint.Query().
                            WHERE("StartValidPeriod", Comparison.LessOrEquals, TransactionDate).
                            AND("EndValidPeriod", Comparison.GreaterOrEquals, TransactionDate).
                            AND(MembershipPoint.Columns.MembershipNo, MembershipNo).
                            AND(MembershipPoint.UserColumns.PackageItemNo, ItemNo).
                            ORDER_BY("StartValidPeriod", "ASC").
                            ExecuteDataSet();

                        #region *) Option 01: Points exist in system [Do Update]
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            int CurrentPointID;
                            decimal CurrentPoint;

                            int i = 0;
                            while (absDiffPoint > 0)
                            {
                                #region ^) Terminator: Point insufficient
                                if (absDiffPoint > 0 & i == ds.Tables[0].Rows.Count)
                                    throw new Exception("(warning)Insufficient points to be deducted");
                                #endregion

                                CurrentPoint = decimal.Parse(ds.Tables[0].Rows[i]["Points"].ToString());
                                CurrentPointID = int.Parse(ds.Tables[0].Rows[i]["PointID"].ToString());

                                if (absDiffPoint >= CurrentPoint)
                                {
                                    string MinQuery =
                                        "UPDATE MembershipPoints " +
                                        "SET Points = Points - @DiffPoints, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy " +
                                        "WHERE PointID = @PointID";
                                    QueryCommand MinCmd = new QueryCommand(MinQuery);
                                    MinCmd.AddParameter("@DiffPoints", CurrentPoint, DbType.Decimal);
                                    MinCmd.AddParameter("@ModifiedOn", DateTime.Now, DbType.DateTime);
                                    MinCmd.AddParameter("@ModifiedBy", UserName, DbType.String);
                                    MinCmd.AddParameter("@PointID", CurrentPointID, DbType.Int32);

                                    //qry.AddUpdateSetting("Points", 0);
                                    //Cmds.Add(qry.BuildDeleteCommand());
                                    Cmds.Add(MinCmd);
                                    absDiffPoint = absDiffPoint - CurrentPoint;
                                }
                                else
                                {

                                    string MinQuery =
                                        "UPDATE MembershipPoints " +
                                        "SET Points = Points - @DiffPoints, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy " +
                                        "WHERE PointID = @PointID";
                                    QueryCommand MinCmd = new QueryCommand(MinQuery);
                                    MinCmd.AddParameter("@DiffPoints", absDiffPoint, DbType.Decimal);
                                    MinCmd.AddParameter("@ModifiedOn", DateTime.Now, DbType.DateTime);
                                    MinCmd.AddParameter("@ModifiedBy", UserName, DbType.String);
                                    MinCmd.AddParameter("@PointID", CurrentPointID, DbType.Int32);


                                    //Cmds.Add(qry.BuildUpdateCommand());
                                    Cmds.Add(MinCmd);

                                    absDiffPoint = 0;
                                }
                                i += 1;
                            }
                        }
                        #endregion
                        #region *) Option 02: Points do not exist in system [Send Error]
                        else
                        {
                            throw new Exception("(warning)Insufficient points to be deducted");
                        }
                        #endregion
                    }

                    #region *) Core: Save Point Allocation Log
                    PointAllocationLog PointLogger = new PointAllocationLog();
                    PointLogger.AllocationDate = TransactionDate;
                    PointLogger.OrderHdrID = OrderHdrID;
                    PointLogger.MembershipNo = MembershipNo;
                    PointLogger.PointAllocated = OneDiffPoint;
                    PointLogger.UniqueID = Guid.NewGuid();
                    PointLogger.Userfld1 = ItemNo;
                    PointLogger.Userfld2 = SalesPersonID;
                    PointLogger.Userfld3 = "";
                    PointLogger.Userfld4 = "";
                    PointLogger.Userfld5 = "";
                    PointLogger.Userfld6 = "";
                    PointLogger.Userfld7 = "";
                    PointLogger.Userfld8 = "";
                    PointLogger.Userfld9 = "";
                    PointLogger.Userfld10 = "";
                    Cmds.Add(PointLogger.GetInsertCommand(UserName));
                    #endregion
                }

                string DelQuery =
                    "DELETE FROM MembershipPoints " +
                    "WHERE Points = 0 AND MembershipNo = @MembershipNo";
                QueryCommand DelCmd = new QueryCommand(DelQuery);
                DelCmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                Cmds.Add(DelCmd);

                //DataService.ExecuteTransaction(Cmds);

                return true;
            }
            catch (Exception X)
            {
                InitialPoint = 0;
                DiffPoint = 0;
                Status = X.Message;
                return false;
            }
        }
        
    }
}
