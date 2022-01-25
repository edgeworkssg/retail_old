using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using System.Collections;
using PowerPOS.Container;

namespace PowerPOS
{
    public partial class MembershipController
    {
        public static bool UpdateMemberDataWeb(DataTable data, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                #region *) Update Data
                
                DataSet ds = new DataSet("Membership");
                ds.Tables.Add(data);
                isSuccess = SyncRealTimeController.DownloadData(ds, "Membership", "MembershipNo", false);

                #endregion
                if (isSuccess)
                {
                    #region *) Update ModifiedOn
                    QueryCommandCollection qmc = new QueryCommandCollection();
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string membershipNo = (string)data.Rows[i]["MembershipNo"];
                        if (!string.IsNullOrEmpty(membershipNo))
                        {
                            Membership mp = new Membership(Membership.Columns.MembershipNo, membershipNo);
                            if (!mp.IsNew)
                            {
                                mp.Deleted = false;
                                qmc.Add(mp.GetUpdateCommand("SYNC"));
                            }
                        }
                    }
                    if (qmc.Count > 0)
                        DataService.ExecuteTransaction(qmc);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool UpdateMemberTagNoWeb(string MembershipNo, string TagNo, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                #region *) Update Data
                QueryCommandCollection qmc = new QueryCommandCollection();
                Membership member = new Membership(MembershipNo);
                if(member.IsNew)
                    throw new Exception("Member doesn't exist");

                member.TagNo = TagNo;
                qmc.Add(member.GetUpdateCommand("SYNC"));

                if (qmc.Count > 0)
                    DataService.ExecuteTransaction(qmc);
                #endregion

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool UpdateMemberData(DataTable data, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                #region *) Update Data

                DataSet ds = new DataSet("Membership");
                ds.Tables.Add(data);
                isSuccess = SyncRealTimeController.DownloadData(ds, "Membership", "MembershipNo", false);
                #endregion

                if (isSuccess && !PointOfSaleInfo.IntegrateWithInventory)
                {
                    #region *) Update The Server
                    string XMLFILENAME = "\\WS.XML";
                    string WS_URL = "http://localhost:51831/PowerPOSWeb/Synchronization/Synchronization.asmx";
                    try
                    {
                        DataSet dsURL = new DataSet();
                        dsURL.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                        WS_URL = dsURL.Tables[0].Rows[0]["URL"].ToString();
                        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        ws.Url = WS_URL;

                        isSuccess = ws.UpdateMemberDataWeb(data, out status);
                    }
                    catch (Exception ex) //system exception
                    {
                        isSuccess = false;
                        status = "ERROR : "+ex.Message;
                        Logger.writeLog("Load_WS_URL");
                        Logger.writeLog(ex);
                    }

                    #endregion
                }
                else
                {
                    status = "Failed update data";
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool UpdateMemberTagNo(string MembershipNo, string TagNo, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                #region *) Update Data
                Membership member = new Membership(MembershipNo);
                if (member.IsNew)
                    throw new Exception("Member doesn't exist");
                
                
                member.TagNo = TagNo;
                member.Save("SYNC");

                #endregion

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    #region *) Update The Server
                    string XMLFILENAME = "\\WS.XML";
                    string WS_URL = "http://localhost:51831/PowerPOSWeb/Synchronization/Synchronization.asmx";
                    try
                    {
                        DataSet dsURL = new DataSet();
                        dsURL.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                        WS_URL = dsURL.Tables[0].Rows[0]["URL"].ToString();
                        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        ws.Url = WS_URL;

                        isSuccess = ws.UpdateMemberTagNoWeb(MembershipNo, TagNo, out status);
                    }
                    catch (Exception ex) //system exception
                    {
                        isSuccess = false;
                        status = "ERROR : " + ex.Message;
                        Logger.writeLog("Load_WS_URL");
                        Logger.writeLog(ex);
                    }

                    #endregion
                }
                else
                {
                    status = "Failed update data";
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool checkAdditionalFieldExist()
        {
            bool exist = false;

            MembershipCustomFieldCollection cr = new MembershipCustomFieldCollection();
            cr.Where(MembershipCustomField.Columns.Deleted, false);
            cr.Load();

            if (cr != null && cr.Count != 0)
                exist = true;

            return exist;
        }
    }
}
