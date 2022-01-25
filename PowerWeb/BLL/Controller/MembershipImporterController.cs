using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS;
using SubSonic;
using System.Collections.Generic;

namespace PowerWeb
{
    public class MembershipImporterController
    {
        public static DataTable FetchMembership(int membershipGroupId)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = @"
                            DECLARE @MembershipGroupId int;
                            SET @MembershipGroupId = {0};

                            SELECT m.MembershipNo AS [Membership No],
                                g.GroupName AS [Group],
                                m.NameToAppear AS [Name To Appear],
                                m.ChineseName AS [Chinese Name],
                                m.FirstName AS [First Name],
                                m.LastName AS [Last Name],
                                m.ChristianName AS [Christian Name],
                                CASE 
                                    WHEN m.Gender = 'M' THEN 'Male'
                                    WHEN m.Gender = 'F' THEN 'Female'
                                    ELSE ''
                                END AS [Gender],
                                m.SalesPersonID AS [Staff],
                                m.DateOfBirth AS [Date Of Birth],
                                m.Email,
                                m.NRIC,
                                m.StreetName AS [Address1],
                                m.StreetName2 AS [Address2],
                                m.ZipCode AS [Zip Code],
                                m.City,
                                m.Country,
                                m.Nationality,
                                m.Mobile,
                                m.Home,
                                m.Occupation,
                                m.SubscriptionDate AS [Subscription Date],
                                m.ExpiryDate AS [Expiry Date],
                                m.Remarks,
                                CASE WHEN ISNULL(m.Deleted, 0) = 0 THEN 'No' ELSE 'Yes' END [Deleted]
                            FROM Membership m
                                INNER JOIN MembershipGroup g ON g.MembershipGroupId = m.MembershipGroupId
                            WHERE (m.MembershipGroupId = @MembershipGroupId OR @MembershipGroupId = 0) AND m.MembershipNo <> 'WALK-IN'
                            ";

                sql = string.Format(sql, membershipGroupId);
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static bool ImportData(string userName, DataTable data, out DataTable result, out string message)
        {
            bool isSuccess = false;
            result = data.Copy();
            message = "";

            try
            {
                result.Columns.Add("Status", typeof(string));
                result.Columns.Add("MembershipGroupId", typeof(int));  // Temp column to store MembershipGroupId
                QueryCommandCollection qmc = new QueryCommandCollection();

                #region *) Validation
                List<string> theMemberNos = new List<string>();
                List<string> theNRICs = new List<string>();
                List<string> theMobileNos = new List<string>();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    bool isValid = true;
                    string memberNo = (string)result.Rows[i]["Membership No"];
                    string groupName = (string)result.Rows[i]["Group"];
                    string nameToAppear = (string)result.Rows[i]["Name To Appear"];
                    string staff = (string)result.Rows[i]["Staff"];
                    string dobStr = (string)result.Rows[i]["Date Of Birth"];
                    string subsDateStr = (string)result.Rows[i]["Subscription Date"];
                    string expDateStr = (string)result.Rows[i]["Expiry Date"];
                    string nric = (string)result.Rows[i]["NRIC"];
                    string mobileNo = (string)result.Rows[i]["Mobile"];

                    DateTime dob, subsDate, expDate;
                    string status = "";

                    if (string.IsNullOrEmpty(memberNo))
                    {
                        status = "- Membership No Cannot Empty\n";
                        isValid = false;
                    }
                    if (theMemberNos.Contains(memberNo.Trim()))
                    {
                        status = "- Duplicated Membership No\n";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(groupName))
                    {
                        status = "- Group Cannot Empty\n";
                        isValid = false;
                    }
                    else
                    {
                        MembershipGroup mg = new MembershipGroup(MembershipGroup.Columns.GroupName, groupName);
                        if (mg != null && mg.GroupName == groupName)
                        {
                            result.Rows[i]["MembershipGroupId"] = mg.MembershipGroupId;
                        }
                        else
                        {
                            status = "- Group Is Not Found\n";
                            isValid = false;
                        }

                    }
                    if (string.IsNullOrEmpty(nameToAppear))
                    {
                        status = "- Name To Appear Cannot Empty\n";
                        isValid = false;
                    }
                    if (!string.IsNullOrEmpty(staff))
                    {
                        UserMst usr = new UserMst(staff);
                        if (usr == null || usr.UserName != staff)
                        {
                            status = "- Staff Is Not Found\n";
                            isValid = false;
                        }
                    }
                    if (!string.IsNullOrEmpty(dobStr))
                    {
                        if (!DateTime.TryParse(dobStr, out dob))
                        {
                            status = "- Date Of Birth Is Not A Valid Date\n";
                            isValid = false;
                        }
                    }
                    if (string.IsNullOrEmpty(subsDateStr))
                    {
                        status = "- Subscription Date Cannot Empty\n";
                        isValid = false;
                    }
                    if (!DateTime.TryParse(subsDateStr, out subsDate))
                    {
                        status = "- Subscription Date Is Not A Valid Date\n";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(expDateStr))
                    {
                        status = "- Expiry Date Cannot Empty\n";
                        isValid = false;
                    }
                    if (!DateTime.TryParse(expDateStr, out expDate))
                    {
                        status = "- Expiry Date Is Not A Valid Date\n";
                        isValid = false;
                    }
                    if (nric.Trim() != "")
                    {
                        if (theNRICs.Contains(nric.Trim()))
                        {
                            status = "- Duplicated NRIC in Excel file\n";
                            isValid = false;
                        }
                        else
                        {
                            Query qry = new Query("Membership");
                            qry.AddWhere(PowerPOS.Membership.Columns.Deleted, Comparison.Equals, false);
                            qry.AddWhere(PowerPOS.Membership.Columns.Nric, Comparison.Equals, nric.Trim());
                            qry.AddWhere(PowerPOS.Membership.Columns.MembershipNo, Comparison.NotEquals, memberNo.Trim());
                            int count = qry.GetRecordCount();
                            if (count > 0) // Found duplicated NRIC
                            {
                                status = "- This NRIC has been used by other member\n";
                                isValid = false;
                            }
                        }
                    }
                    if (mobileNo.Trim() != "")
                    {
                        if (theMobileNos.Contains(mobileNo.Trim()))
                        {
                            status = "- Duplicated Mobile in Excel file\n";
                            isValid = false;
                        }
                        else
                        {
                            Query qry = new Query("Membership");
                            qry.AddWhere(PowerPOS.Membership.Columns.Deleted, Comparison.Equals, false);
                            qry.AddWhere(PowerPOS.Membership.Columns.Mobile, Comparison.Equals, mobileNo.Trim());
                            qry.AddWhere(PowerPOS.Membership.Columns.MembershipNo, Comparison.NotEquals, memberNo.Trim());
                            int count = qry.GetRecordCount();
                            if (count > 0) // Found duplicated Mobile
                            {
                                status = "- This MobileNo has been used by other member\n";
                                isValid = false;
                            }
                        }
                    }
                    if (nric.Trim() == "" && mobileNo.Trim() == "")
                    {
                        status = "- Please enter either NRIC or MobileNo\n";
                        isValid = false;
                    }

                    result.Rows[i]["Status"] = status;
                    theMemberNos.Add(memberNo.Trim());
                    if (nric.Trim() != "") theNRICs.Add(nric.Trim());
                    if (mobileNo.Trim() != "") theMobileNos.Add(mobileNo.Trim());
                }
                #endregion

                //#region *) MembershipGroup
                //foreach (var grp in theGroupNames)
                //{
                //    MembershipGroup mg = new MembershipGroup(MembershipGroup.Columns.GroupName, grp);
                //    if (mg.IsNew)
                //    {
                //        mg.GroupName = grp;
                //        mg.Discount = 0;
                //        mg.PointsPercentage = 0;
                //    }
                //    mg.Deleted = false;
                //    if (mg.IsNew)
                //        qmc.Add(mg.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                //    else
                //        qmc.Add(mg.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                //}
                //#endregion

                #region *) Membership
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    string err = (string)result.Rows[i]["Status"];
                    if (string.IsNullOrEmpty(err))
                    {
                        PowerPOS.Membership originalMember = new PowerPOS.Membership();

                        var theMember = new PowerPOS.Membership((string)result.Rows[i]["Membership No"]);
                        if (theMember.IsNew)
                        {
                            theMember.UniqueID = Guid.NewGuid();
                            theMember.MembershipNo = (string)result.Rows[i]["Membership No"];
                        }
                        else
                        {
                            theMember.CopyTo(originalMember);
                            originalMember.IsNew = false;
                        }

                        theMember.MembershipGroupId = (int)result.Rows[i]["MembershipGroupId"];
                        theMember.NameToAppear = (string)result.Rows[i]["Name To Appear"];
                        theMember.ChineseName = (string)result.Rows[i]["Chinese Name"];
                        theMember.FirstName = (string)result.Rows[i]["First Name"];
                        theMember.LastName = (string)result.Rows[i]["Last Name"];
                        theMember.ChristianName = (string)result.Rows[i]["Christian Name"];

                        string gender = ((string)result.Rows[i]["Gender"] + "").ToUpper();
                        if (gender == "MALE")
                            theMember.Gender = "M";
                        else if (gender == "FEMALE")
                            theMember.Gender = "F";
                        else
                            theMember.Gender = "";

                        if (!string.IsNullOrEmpty((string)result.Rows[i]["Staff"]))
                            theMember.SalesPersonID = (string)result.Rows[i]["Staff"];
                        else
                            theMember.SalesPersonID = null;

                        if (!string.IsNullOrEmpty((string)result.Rows[i]["Date Of Birth"]))
                            theMember.DateOfBirth = DateTime.Parse((string)result.Rows[i]["Date Of Birth"]);
                        else
                            theMember.DateOfBirth = null;

                        theMember.Email = (string)result.Rows[i]["Email"];
                        theMember.Nric = (string)result.Rows[i]["NRIC"];
                        theMember.StreetName = (string)result.Rows[i]["Address1"];
                        theMember.StreetName2 = (string)result.Rows[i]["Address2"];
                        theMember.ZipCode = (string)result.Rows[i]["Zip Code"];
                        theMember.City = (string)result.Rows[i]["City"];
                        theMember.Country = (string)result.Rows[i]["Country"];
                        theMember.Nationality = (string)result.Rows[i]["Nationality"];
                        theMember.Mobile = (string)result.Rows[i]["Mobile"];
                        theMember.Home = (string)result.Rows[i]["Home"];
                        theMember.Occupation = (string)result.Rows[i]["Occupation"];

                        theMember.SubscriptionDate = DateTime.Parse((string)result.Rows[i]["Subscription Date"]);
                        theMember.ExpiryDate = DateTime.Parse((string)result.Rows[i]["Expiry Date"]);
                        theMember.Remarks = (string)result.Rows[i]["Remarks"];
                        theMember.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");

                        if (theMember.IsNew)
                            qmc.Add(theMember.GetInsertCommand(string.Format("{0} via WEB Membership Importer", userName)));
                        else
                            qmc.Add(theMember.GetUpdateCommand(string.Format("{0} via WEB Membership Importer", userName)));
                    }
                }
                #endregion

                // Remove this temp column
                result.Columns.Remove("MembershipGroupId");

                if (qmc.Count > 0)
                    DataService.ExecuteTransaction(qmc);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }
    }
}
