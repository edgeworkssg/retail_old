using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using PowerPOS;
using SubSonic;
/// <summary>
/// Summary description for FetchMembersPastTransaction
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class FetchMembersPastTransaction : System.Web.Services.WebService {

    public FetchMembersPastTransaction () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public DataSet FetchHistory(string membershipNo) {
        try
        {
            //Set up many to many relationship between Membership and inventory header info
            Query qry = new Query("OrderHdr");
            qry.OrderBy = OrderBy.Desc(OrderHdr.Columns.OrderDate);
            qry.AddWhere(OrderHdr.Columns.MembershipNo, membershipNo);
            
            qry.SelectList = "OrderHdrId";

            string refno = (string)qry.ExecuteScalar();
            if (refno != "")
            {
                OrderDetCollection det = new OrderDetCollection();
                det.Where(OrderDet.Columns.OrderHdrID, refno);
                det.Load();

                DataTable dt = new DataTable("History");
                dt.Columns.Add("DateTime");
                dt.Columns.Add("Outlet");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("unitprice");
                dt.Columns.Add("Quantity");
                DataRow dr;
                for (int j = 0; j < det.Count; j++)
                {
                    dr = dt.NewRow();
                    dr["DateTime"] = det[j].OrderHdr.OrderDate.ToString("dd-MMM-yyyy");
                    dr["Outlet"] = det[j].OrderHdr.PointOfSale.OutletName;
                    dr["ItemName"] = det[j].Item.ItemName;
                    dr["unitprice"] = det[j].UnitPrice.ToString("N2");
                    dr["quantity"] = det[j].Quantity;
                    dt.Rows.Add(dr);
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
            return null;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return null;
        }     
    }

    [WebMethod]
    public DataSet FetchMembershipReport
        (bool useStartMembershipDate, bool useEndMembershipDate,
        DateTime StartMembershipDate, DateTime EndMembershipDate,
        bool useStartBirthDate, bool useEndBirthDate,
        DateTime StartBirthDate, DateTime EndBirthDate,        
        string StartMembershipNo, string EndMembershipNo, int ViewGroupID, 
        string SortColumn, string SortDir, string username)
    {
        if (!UserController.ValidUserID(UserController.DecryptData(username)))
        {
            return null;
        }
        DataSet ds = new DataSet();
        DataTable dt = ReportController.FetchMembershipReport
            (useStartMembershipDate,  useEndMembershipDate,
             StartMembershipDate,  EndMembershipDate,
             useStartBirthDate,  useEndBirthDate,
             StartBirthDate,  EndBirthDate,            
             StartMembershipNo, EndMembershipNo, ViewGroupID,
             SortColumn,  SortDir);
        /*
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["email"] = "albert_tahu@hotmail.com";
        }*/
        ds.Tables.Add(dt);

        return ds;
    }


    [WebMethod]
    public DataSet FetchMembershipGroupName()
    {        
        DataTable dt = MembershipController.FetchMembershipGroupList();
        DataRow dr = dt.NewRow();
        dr["GroupName"] = "ALL";
        dr["MembershipGroupId"] = 0;
        dt.Rows.InsertAt(dr, 0);                       

        return dt.DataSet;
    }

    [WebMethod]
    public bool Login(string username, string encryptedPassword, out string status)
    {                
        username = UserController.DecryptData(username);
        UserMst myUser = new UserMst(UserMst.Columns.UserName, username);
        if (myUser == null || myUser.UserName == null || myUser.UserName == "" || myUser.Password != encryptedPassword)
        {
            status = "Please enter a valid user ID/password";
            return false;
        }
        else
        {           
           status = "";
           return true;
            
        }
    }
}

