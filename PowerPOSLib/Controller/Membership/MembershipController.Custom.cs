using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using SubSonic;
using System.Data;
using System.Collections;
using PowerPOS.Container;
using System.IO;
using System.Net.NetworkInformation;
namespace PowerPOS
{
    public partial class MembershipController
    {

        /*public struct MembershipQuickInfo
        {
            public string firstname;
            public string lastname;
            public string nric;
            public string membershipNo;
            public int membershipgroupid;            
        }*/
        public const int DEFAULT_GROUPID = 13;
        public const int GOLD_GROUPID = 14;
        public const string MEMBERSHIP_PREFIX = "";
        public const string MEMBERSHIP_SIGNUP_BARCODE = "MEMBER"; //Membership signup
        public static ArrayList SendMembershipNotification(ArrayList emails, DateTime startDate, DateTime endDate, string rootDir)
        {
            try
            {

                MassEmail ms;
                ArrayList failed = new ArrayList();
                string status = "";
                ArrayList failed2;
                string filepath = rootDir + "\\temp\\";

                DataTable dt = ReportController.FetchMembershipReport
                    (true, true, startDate, endDate, false, false, DateTime.Now, DateTime.Now, false, 1, "", "", 0, "", "", "", "","", "", "", "", "MembershipNo", "ASC");
                if (dt != null && dt.Rows.Count > 0)
                {
                
                    if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);
                    filepath = filepath + "NewSignupReport" + endDate.ToString("yyyyMMdd") + ".csv";
                    ExportController.ExportToCSV(dt, filepath);

                    ms = new MassEmail();
                    failed =
                        ms.SendEmails(emails, "", AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                        "Membership Signup", "", "", AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP),
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password), false, filepath, 
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port), out status);

                  
                }

                ViewMembershipUpgradeCollection v = new ViewMembershipUpgradeCollection();
                v.Where(ViewMembershipUpgrade.Columns.OrderDate, Comparison.GreaterOrEquals, startDate);
                v.Where(ViewMembershipUpgrade.Columns.OrderDate, Comparison.LessOrEquals, endDate);
                dt = v.Load().ToDataTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    filepath = rootDir + "\\temp\\";
                    filepath = filepath + "MembershipUpgrade" + endDate.ToString("yyyyMMdd") + ".csv";
                    ExportController.ExportToCSV(dt, filepath);
                    ms = new MassEmail();
                    failed2 =
                        ms.SendEmails(emails, "", AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                        "Membership Upgrade", "", "", AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP),
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password), false, filepath, 
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port), out status);
                    failed.AddRange(failed2);
                }
                return failed;
            }
            catch (Exception x)
            {
                Logger.writeLog(x);
                return null;
            }
        }
        public static bool updateExpiryDateFromRenewalRequest()
        {
            try
            {
                MembershipRenewalCollection mbr = new MembershipRenewalCollection();
                mbr.Where(MembershipRenewal.Columns.Deleted, false);
                mbr.Load();

                for (int i = 0; i < mbr.Count; i++)
                {
                    QueryCommandCollection cmd = new QueryCommandCollection();
 

                    //create update command for membership
                    string SQL;
                    //Query qr = Membership.CreateQuery();
                    //qr.QueryType = QueryType.Update;
                    //qr.AddWhere(Membership.Columns.MembershipNo, mbr[i].MembershipNo);
                    //qr.AddUpdateSetting(Membership.Columns.ExpiryDate, mbr[i].NewExpiryDate);
                    //cmd.Add(qr.BuildUpdateCommand());
                    SQL = "update membership set expirydate = '" + mbr[i].NewExpiryDate.ToString("yyyy-MM-dd HH:mm:ss") + "',modifiedon=getdate() where membershipno = '" + mbr[i].MembershipNo + "'";
                    cmd.Add(new QueryCommand(SQL, "PowerPOS"));

                    if (mbr[i].NewMembershipGroupID != -1 && mbr[i].NewMembershipGroupID != 0)
                    {
                        SQL = "update membership set MembershipGroupID = " + mbr[i].NewMembershipGroupID.ToString() + ",modifiedon=getdate() where membershipno = '" + mbr[i].MembershipNo + "'";
                        cmd.Add(new QueryCommand(SQL, "PowerPOS"));
                    }

                    SQL = "update membershiprenewal set deleted = 1 where renewalID = " + mbr[i].RenewalID.ToString() + "";
                    cmd.Add(new QueryCommand(SQL,"PowerPOS"));
                    try
                    {
                        DataService.ExecuteTransaction(cmd);
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog("Unable to update renewal> Renewal ID " + mbr[i].RenewalID );
                        Logger.writeLog(ex);
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        public static DataTable FetchLastPurchasedTransactions(string membershipNo)
        {
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 2000;
                PingReply reply = pingSender.Send("www.google.com", timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    //Connected....
                    //Call Web Service
                    PowerPOSLib.FetchMembershipHistory.FetchMembersPastTransaction ws = new PowerPOSLib.FetchMembershipHistory.FetchMembersPastTransaction();

                    //TO DO: Replace this with non-harcoded
                    ws.Url = "http://ahava.edgeworks.com.sg/FetchMembersPastTransaction.asmx";
                    DataSet ds = ws.FetchHistory(membershipNo);

                    if (ds != null && ds.Tables.Count > 0)
                    {

                        return ds.Tables[0];
                    }
                    return null;

                }
                else
                {
                    //i am not connected
                    return null;
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static double getDiscountByMembershipNo(string membershipNo)
        {
            try
            {
                return ((MembershipCollection)Membership.FetchByParameter(Membership.Columns.MembershipNo, membershipNo))[0].MembershipGroup.Discount;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return -1;
            }
        }

        public static bool CheckMembershipValid(string membershipNo, out Membership member, out DateTime ExpiryDate)
        {
            ExpiryDate = DateTime.MinValue;
            member = new Membership();

            //MembershipCollection myColl = (MembershipCollection)Membership.FetchByParameter(Membership.Columns.MembershipNo, membershipNo);
            member.LoadByParam(Membership.Columns.MembershipNo, membershipNo);
            if (member == null || member.ExpiryDate == null) return false;
            ExpiryDate = (DateTime)member.ExpiryDate;

            if (member.ExpiryDate < DateTime.Now)
            {
                //Expired Liao
                return false;
            }
            else
            {
                return true;
            }
        }
        //for generating MCYYYYMMDDXXXYYY
        public static string getNewMembershipNo(string membershipCode)
        {
            int runningNo = 0;



            IDataReader rdr = SPs.GetNewMembershipNoByPointOfSalePrefix(membershipCode).GetReader();

            while (rdr.Read())
            {
                if (!int.TryParse(rdr.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            rdr.Close();
            runningNo += 1;
            return membershipCode +
                runningNo.ToString().PadLeft(5, '0');
        }
        public static bool IsMembersNRIC(string Nric, out Membership member)
        {
            member = null;
            MembershipCollection m = new MembershipCollection();
            m.Where(Membership.Columns.Nric, Nric);
            m.Where(Membership.Columns.Deleted, false);
            m.Load();

            if (m.Count > 0)
            {
                member = m[0];
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsMembersTagNo(string TagNo, out Membership member)
        {
            member = null;
            MembershipCollection m = new MembershipCollection();
            m.Where(Membership.UserColumns.TagNo, TagNo);
            m.Where(Membership.Columns.Deleted, false);
            m.Load();

            if (m.Count > 0)
            {
                member = m[0];
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsExistingMember(
            string membershipno,
            out bool hasExpired,
            out DateTime ExpiryDate)
        {
            hasExpired = false;
            bool result = false;
            Membership member;
            ExpiryDate = DateTime.MinValue;
            member = new Membership();

            MembershipCollection myColl = new MembershipCollection();
            myColl.Where(Membership.Columns.MembershipNo, membershipno);
            myColl.Where(Membership.Columns.Deleted, false);
            myColl.Load();

            if (myColl.Count > 0)
            {
                member = myColl[0];
            }
            else
            {
                result = false;
                return result;
            }

                //(MembershipCollection)Membership.FetchByParameter(Membership.Columns.MembershipNo, membershipNo);
            //member.LoadByParam(Membership.Columns.MembershipNo, membershipno);

            if (member == null ||
                member.ExpiryDate == null ||
                (member.Deleted.HasValue && member.Deleted.Value == true))
            {
                result = false;
                return result;
            }

            ExpiryDate = (DateTime)member.ExpiryDate;

            if (member.ExpiryDate < DateTime.Now)
            {
                //Expired Liao
                hasExpired = true;
                result = false;
                return result;
            }
            else
            {
                result = true;
                return result;
            }
        }

        public static bool IsExistingMember(string membershipno, out Membership m)
        {
            m = null;
            Membership member;
            DateTime ExpiryDate;
            ExpiryDate = DateTime.MinValue;
            member = new Membership();
            //MembershipCollection myColl = (MembershipCollection)Membership.FetchByParameter(Membership.Columns.MembershipNo, membershipNo);
            member.LoadByParam(Membership.Columns.MembershipNo, membershipno);

            if (member == null ||
                member.ExpiryDate == null ||
                member.Deleted == true)
                return false;

            ExpiryDate = (DateTime)member.ExpiryDate;

            if (member.ExpiryDate < DateTime.Now)
            {
                //Expired Liao
                m = member;
                return false;
            }
            else
            {
                m = member;
                return true;
            }
        }

        public static MembershipGroupCollection FetchMembershipGroupArrayList()
        {
            Query qr = MembershipGroup.CreateQuery();
            qr.QueryType = QueryType.Select;
            DataSet ds = qr.ExecuteDataSet();
            DataTable dt = ds.Tables[0];
            MembershipGroupCollection b = new MembershipGroupCollection();
            b.Load(dt);
            /*
            ArrayList ar = new ArrayList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ar.Add(dt.Rows[i][0].ToString());
            }*/
            return b;
        }
        public static DataTable FetchMembershipGroupList()
        {
            Query qr = MembershipGroup.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = "GroupName, MembershipGroupId";
            qr.WHERE(MembershipGroup.Columns.Deleted, false);
            DataSet ds = qr.ExecuteDataSet();

            DataTable dt = ds.Tables[0];
            DataRow dr = dt.NewRow();
            dr["GroupName"] = "ALL";
            dr["MembershipGroupId"] = 0;
            dt.Rows.InsertAt(dr, 0);

            return dt;
        }

        /// <summary>
        /// Check whether a member's particular already exist in database
        /// </summary>
        /// <param name="nric">Member's NRIC</param>
        /// <param name="MobileNo">Member's Mobile No</param>
        /// <param name="duplicate">Return the existing MembershipNo if the particular exists in database</param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        /// </remarks>
        public static bool IsParticularAlreadyExist
            (string membershipNo, string nric, string MobileNo, out string duplicate)
        {
            duplicate = "";

            #region *)Validate: Check MembershipNo

            string sqlMembershipNo = "select count(*) from membership where (deleted is null or deleted = 0) and MembershipNo = '" + membershipNo + "'";
            int duplicateCount = (int) DataService.ExecuteScalar(new QueryCommand(sqlMembershipNo));
            if (duplicateCount > 0)
            {
                duplicate = membershipNo;
                return true;
            }
            #endregion

            string SearchQuery_NRIC = "SELECT TOP 1 (MembershipNo) FROM Membership WHERE (Deleted IS NULL OR Deleted = 0) AND NRIC = @NRIC";
            string SearchQuery_MobileNo = "SELECT TOP 1 (MembershipNo) FROM Membership WHERE (Deleted IS NULL OR Deleted = 0) AND Mobile = @MobileNo";

            #region *) Validate: Check NRIC
            if (!string.IsNullOrEmpty(nric))
            {
                QueryCommand cmd = new QueryCommand(SearchQuery_NRIC);
                cmd.AddParameter("@NRIC", nric.Trim().Replace("-", ""));
                Object obj = DataService.ExecuteScalar(cmd);
                if (obj != null)
                {
                    duplicate = obj.ToString();
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(duplicate)) return true;  /// If found any duplicate, return;

            #region Validate: Check Mobile No
            if (!string.IsNullOrEmpty(MobileNo))
            {
                QueryCommand cmd = new QueryCommand(SearchQuery_MobileNo);
                cmd.AddParameter("@MobileNo", MobileNo.Trim().Replace(" ","")/*.Replace("-", "")*/);
                Object obj = DataService.ExecuteScalar(cmd);
                if (obj != null)
                {
                    duplicate = obj.ToString();
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(duplicate)) return true;  /// If found any duplicate, return;

            return false;
        }

        public static bool IsEmailExist(string email)
        {
            string SearchQuery_NRIC = "SELECT TOP 1 (MembershipNo) FROM Membership WHERE (Deleted IS NULL OR Deleted = 0) AND Email = @Email";
            if (!string.IsNullOrEmpty(email))
            {
                QueryCommand cmd = new QueryCommand(SearchQuery_NRIC);
                cmd.AddParameter("@Email", email.Trim().Replace("-", ""));
                Object obj = DataService.ExecuteScalar(cmd);
                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsEmailAlreadyExist(string email, out string duplicate)
        {
            string SearchQuery_NRIC = "SELECT TOP 1 (MembershipNo) FROM Membership WHERE (Deleted IS NULL OR Deleted = 0) AND Email = @Email";
            duplicate = "";
            if (!string.IsNullOrEmpty(email))
            {
                QueryCommand cmd = new QueryCommand(SearchQuery_NRIC);
                cmd.AddParameter("@Email", email.Trim().Replace("-", ""));
                Object obj = DataService.ExecuteScalar(cmd);
                if (obj != null)
                {
                    duplicate = obj.ToString();
                    if (duplicate != String.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsNRICAlreadyExist
            (string nric, out string duplicate)
        {
            duplicate = "";

            Query qr = new Query("Membership");
            qr.AddWhere(Membership.Columns.Deleted, false);
            qr.AddWhere(Membership.Columns.Nric,
                nric.TrimStart().TrimEnd().Replace("-", ""));
            qr.SelectList = "membershipno";
            qr.Top = "1";
            Object obj = qr.ExecuteScalar();
            if (obj != null)
            {
                duplicate = obj.ToString();
                if (duplicate != String.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsTagNoAlreadyExist(string tagno, string membershipno, out string duplicate)
        {
            duplicate = "";

            Query qr = new Query("Membership");
            qr.AddWhere(Membership.Columns.Deleted, false);
            qr.AddWhere(Membership.UserColumns.TagNo,
                tagno.TrimStart().TrimEnd().Replace("-", ""));
            qr.AddWhere(Membership.Columns.MembershipNo, Comparison.NotEquals, membershipno);
            qr.SelectList = "membershipno";
            qr.Top = "1";
            Object obj = qr.ExecuteScalar();
            if (obj != null)
            {
                duplicate = obj.ToString();
                if (duplicate != String.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsTagNoSameWithNRIC(string tagno, string membershipno, out string duplicate) {
            duplicate = "";

            Query qr = new Query("Membership");
            qr.AddWhere(Membership.Columns.Deleted, false);
            qr.AddWhere(Membership.Columns.Nric,
                tagno.TrimStart().TrimEnd().Replace("-", ""));
            qr.AddWhere(Membership.Columns.MembershipNo, Comparison.NotEquals, membershipno);
            qr.SelectList = "membershipno";
            qr.Top = "1";
            Object obj = qr.ExecuteScalar();
            if (obj != null)
            {
                duplicate = obj.ToString();
                if (duplicate != String.Empty)
                {
                    return true;
                }
            }
            return false;
        } 

        public ViewMembershipCollection SearchMembership(string query)
        {
     				
           
            string SQL = "select * from ViewMembership where " +
                         "isnull(MembershipNo,'') + isnull(nametoappear,'') + isnull(firstname,'') + isnull(lastname,'') + isnull(chinesename,'') + isnull(christianname,'') + " +
                         "isnull(salespersonid,'') +isnull(groupname,'') +isnull(office,'') +isnull(home,'') +isnull(mobile,'') + isnull(email,'') + isnull(nric,'') + isnull(streetname,'') + isnull(streetname2,'') + isnull(zipcode,'') + isnull(UnitNo,'') + isnull(City,'')  + isnull(Country,'')+ isnull(BuildingName,'')+ isnull(Block,'') + isnull(userfld10,'') " +
                         "like N'%" + query + "%' " +
                         "and deleted=0 order by createdon desc";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            DataSet ds = DataService.GetDataSet(cmd);
            ViewMembershipCollection v = new ViewMembershipCollection();
            v.Load(ds.Tables[0]);

            return v;
        }
        public ViewMembershipCollection SearchMembershipByName(string name)
        {
            ArrayList membershipNo = new ArrayList();

            ViewMembershipCollection coll1 = new ViewMembershipCollection().
                Where(ViewMembership.Columns.NameToAppear, Comparison.Equals, name).
                Where(ViewMembership.Columns.Deleted, false).Load();
            coll1.Sort(ViewMembership.Columns.NameToAppear, true);

            for (int i = 0; i < coll1.Count; i++)
            {
                membershipNo.Add(coll1[i].MembershipNo);
            }

            Query qr = ViewMembership.CreateQuery();
            qr.AddWhere(ViewMembership.Columns.NameToAppear, Comparison.Like, name + "%");
            qr.AddWhere(ViewMembership.Columns.Deleted, false);
            qr.OrderBy = OrderBy.Asc("NameToAppear");
            DataTable dt;

            if (membershipNo.Count > 0)
            {
                dt = qr.NOT_IN(Membership.Columns.MembershipNo, membershipNo).ExecuteDataSet().Tables[0];
            }
            else
            {
                dt = qr.ExecuteDataSet().Tables[0];
            }
            coll1.Load(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                membershipNo.Add(dt.Rows[i]["MembershipNo"]);
            }

            qr = ViewMembership.CreateQuery();
            qr.AddWhere(ViewMembership.Columns.NameToAppear, Comparison.Like, "%" + name.ToString().Replace(" ", "%") + "%");
            qr.AddWhere(ViewMembership.Columns.Deleted, false);
            //qr.OrderBy = OrderBy.Asc("NameToAppear");
            if (membershipNo.Count > 0)
            {
                dt = qr.NOT_IN(Membership.Columns.MembershipNo, membershipNo).ORDER_BY(ViewMembership.Columns.MembershipNo, "ASC").ExecuteDataSet().Tables[0];
            }
            else
            {
                dt = qr.ORDER_BY(ViewMembership.Columns.NameToAppear, "ASC").ExecuteDataSet().Tables[0];
            }
            coll1.Load(dt);

            return coll1;
        }

        public static bool CheckAttachedParticularTable()
        {
            try
            {
                // Create the table if it does not exist
                string sql = "SET ANSI_NULLS ON " +
                             "SET QUOTED_IDENTIFIER ON " +
                             "SET ANSI_PADDING ON " +
                             "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AttachedParticular]') AND type in (N'U')) " +
                             "BEGIN " +
                             "CREATE TABLE [dbo].[AttachedParticular]( " +
                             "    [AttachedParticularID] [int] IDENTITY(1,1) NOT NULL, " +
                             "    [MembershipNo] [varchar](50) NOT NULL, " +
                             "    [FirstName] [varchar](80) NULL, " +
                             "    [LastName] [varchar](80) NULL, " +
                             "    [ChristianName] [varchar](80) NULL, " +
                             "    [ChineseName] [nvarchar](50) NULL, " +
                             "    [Gender] [nvarchar](1) NULL, " +
                             "    [Occupation] [varchar](100) NULL, " +
                             "    [DateOfBirth] [datetime] NULL, " +
                             "    [CreatedOn] [datetime] NULL, " +
                             "    [CreatedBy] [varchar](50) NULL, " +
                             "    [ModifiedOn] [datetime] NULL, " +
                             "    [ModifiedBy] [varchar](50) NULL, " +
                             "    [Deleted] [bit] NULL, " +
                             "    [UniqueID] [uniqueidentifier] NOT NULL, " +
                             "    [userfld1] [nvarchar](50) NULL, " +
                             "    [userfld2] [nvarchar](50) NULL, " +
                             "    [userfld3] [nvarchar](50) NULL, " +
                             "    [userfld4] [nvarchar](50) NULL, " +
                             "    [userfld5] [nvarchar](50) NULL, " +
                             "    [userfld6] [nvarchar](50) NULL, " +
                             "    [userfld7] [nvarchar](50) NULL, " +
                             "    [userfld8] [nvarchar](50) NULL, " +
                             "    [userfld9] [nvarchar](50) NULL, " +
                             "    [userfld10] [nvarchar](50) NULL, " +
                             "    [userflag1] [bit] NULL, " +
                             "    [userflag2] [bit] NULL, " +
                             "    [userflag3] [bit] NULL, " +
                             "    [userflag4] [bit] NULL, " +
                             "    [userflag5] [bit] NULL, " +
                             "    [userfloat1] [money] NULL, " +
                             "    [userfloat2] [money] NULL, " +
                             "    [userfloat3] [money] NULL, " +
                             "    [userfloat4] [money] NULL, " +
                             "    [userfloat5] [money] NULL, " +
                             "    [userint1] [int] NULL, " +
                             "    [userint2] [int] NULL, " +
                             "    [userint3] [int] NULL, " +
                             "    [userint4] [int] NULL, " +
                             "    [userint5] [int] NULL, " +
                             " CONSTRAINT [PK_AttachedParticular] PRIMARY KEY CLUSTERED  " +
                             "( " +
                             "    [AttachedParticularID] ASC " +
                             ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " +
                             ") ON [PRIMARY] " +
                             "END " +
                             "SET ANSI_PADDING OFF " +
                             "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_AttachedParticular_UniqueID]') AND type = 'D') " +
                             "BEGIN " +
                             "ALTER TABLE [dbo].[AttachedParticular] ADD  CONSTRAINT [DF_AttachedParticular_UniqueID]  DEFAULT (newid()) FOR [UniqueID] " +
                             "END ";

                QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                DataService.ExecuteQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static DataTable GetPreOrderSummary(string membershipno)
        { 
            string sql =// "select Row_number() over (order by d.orderdetid asc) as PreOrderNo, d.OrderDetID, d.OrderHdrID "+
                         "SELECT ISNULL(h.userfld5, h.OrderRefNo) as PreOrderNo, d.OrderDetID, d.OrderHdrID " +
	                    ",convert(varchar(50), h.orderdate,106) as PreOrderDate, i.itemno as PreOrderItemNo, i.ItemName as PreOrderItemName "+
	                    ",d.UnitPrice as PreOrderPrice, ISNULL(d.Quantity,0) as PreOrderQty "+
	                    ",ISNULL(d.Quantity,0) - ISNULL(dd.Quantity,0) as PreOrderOutstandingQty "+
	                    //",dbo.GetInstallmentOutstandingBalance(h.OrderhdrID, Getdate()) as PreOrderBalancePayment "+
                        ",d.Amount - ISNULL(d.userfloat1,0) as PreOrderBalancePayment " +
                        "from OrderDet d  "+
	                    "inner join OrderHdr h on d.OrderHdrID = h.OrderHdrID "+
	                    "inner join item i on d.itemno = i.ItemNo "+
	                    "left outer join  "+
	                    "(select d.OrderDetID, SUM(d.Quantity) as Quantity from DeliveryOrderDetails d group by d.OrderDetID)dd on dd.OrderdetID = d.OrderDetID "+
	                    "left outer join receipthdr rh on rh.orderhdrid = h.orderhdrid "+
                        "where h.membershipno = '" + membershipno + "' and "+
	                    "isnull(d.isvoided,0) = 0 and isnull(d.ispreorder,0) = 1 ";

            DataSet ds = DataService.GetDataSet(new QueryCommand(sql));
            DataTable dt = ds.Tables[0];
            dt.Columns.Add("PreOrderStockQty");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["PreOrderStockQty"] = ItemController.GetStockOnHand(dt.Rows[i]["PreOrderItemNo"].ToString(), PointOfSaleInfo.InventoryLocationID);
                    string status = "";
                    dt.Rows[i]["PreOrderStockQty"] = InventoryController.GetStockBalanceQtyByItemSummaryByDate(dt.Rows[i]["PreOrderItemNo"].ToString(), PointOfSaleInfo.InventoryLocationID, DateTime.Now, out status);
                }
            }

            return dt;
        }
    }
}
