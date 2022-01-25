using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PowerPOS;
using SubSonic;
using PowerPOS.Container;

namespace PowerPOS
{
    public partial class ResourceController
    {

        public static string ROOM_DEFAULT = "ROOM_DEFAULT";
        public static string getNewResourceID()
        {
            int runningNo = 0;

            IDataReader rdr = SPs.GetMaxResourceID(PointOfSaleInfo.PointOfSaleID).GetReader();
            while (rdr.Read())
            {
                if (!int.TryParse(rdr.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            rdr.Close();
            runningNo += 1;
            return "RS" + PointOfSaleInfo.PointOfSaleID.ToString().PadLeft(3, '0') + runningNo.ToString().PadLeft(4, '0');
        }
         
        public static DataTable GetResourceData(string search)
        {
            DataTable dt = new DataTable();

            string query = "select r.ResourceID, r.ResourceName, g.ResourceGroupID, ISNULL(g.GroupName,'') as [ResourceGroupName], Capacity " +
                            "from [Resource] r  " +
                            "left outer join [ResourceGroup] g on r.ResourceGroupID = g.ResourceGroupID  " +
                            "where ISNULL(r.Deleted, 0) = 0 and ISNULL(g.Deleted,0) = 0  and r.resourceid != 'ROOM_DEFAULT' " +
                            "and ISNULL(r.ResourceID,'') + ' ' + ISNULL(r.ResourceName,'') + ' ' + ISNULL(g.GroupName,'') like '%' +@Search+ '%' ";

            QueryCommand cmd = new QueryCommand(query);
            cmd.Parameters.Add("@Search", search, DbType.String);
            
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);

            return dt;
        }

        public static DataTable GetAvailableResource()
        {
            string query = "select distinct r.ResourceID, r.ResourceName, r.Capacity,  " +
	                       "ISNULL(dd.checkinqty,0) as CheckInQty, ISNULL(r.Capacity,0) - ISNULL(dd.checkinqty,0) as AvailableQty " +
                           "from [resource] r  " +
                           "left outer join appointment a on a.ResourceID = r.ResourceID  " +
                           "left outer join membership m on a.MembershipNo = m.Membershipno " +
                           "left outer join " +
                           "( " +
                           "    select e.ResourceID, count(e.id) as checkinqty " +
                           "    from appointment e " +
                           "    inner join [resource] ee on e.ResourceID = ee.ResourceID " +
                           "    where ISNULL(e.CheckInByWho,'') != '' and ISNULL(e.CheckOutByWho,'') = '' " +
                           "        and ISNULL(e.deleted,0) = 0 and ISNULL(ee.deleted,0) = 0 and convert(date, e.StartTime) = convert(date,getdate()) " +
                           "     group by e.ResourceID " +
                           ")dd on dd.resourceID = r.ResourceID " +
                           "where ISNULL(a.deleted,0) = 0 and ISNULL(r.deleted,0) = 0 and r.resourceid != 'ROOM_DEFAULT' and ISNULL(m.deleted,0) = 0 ";
            DataSet ds = DataService.GetDataSet(new QueryCommand(query));
            return ds.Tables[0];
        }

        public static DataTable GetRoomListing(DateTime Day)
        {
            string Query = "select r.ResourceID, m.StartTime, r.ResourceName, ISNULL(u.DisplayName,'') as Staff, ISNULL(s.NameToAppear,'') as Customer, isnull(m.listStr,'') as ItemName, m.Duration, m.Id as AppointmentID, m.CheckInByWho, m.CheckOutByWho " +
                           " from [resource] r " +
                           " left outer join  " +
                           " (	 " +
                           "     select *, STUFF((SELECT  ',' + ItemName FROM AppointmentItem EE inner join Item i on EE.ItemNo = i.ItemNo WHERE EE.AppointmentId=E.Id and ISNULL(EE.Deleted,0) = 0 ORDER BY ItemName FOR XML PATH('')), 1, 1, '') AS listStr " +
                           "     from Appointment E " +
                           "     WHERE ISNULL(E.Deleted,0) = 0 and Convert(date,E.StartTime) = '' + @date +'' " +
                           " )m on m.ResourceID = r.ResourceID " +
                           " left outer join UserMst u on m.SalesPersonID = u.UserName " +
                           " left outer join Membership s on s.MembershipNo = m.MembershipNo " +
                           " where ISNULL(r.Deleted,0) = 0 and r.resourceid != 'ROOM_DEFAULT' and ISNULL(u.Deleted,0) = 0 and ISNULL(s.Deleted,0) = 0 " +
                           " ORDER BY r.ResourceName ";

            QueryCommand qc = new QueryCommand(Query);
            qc.Parameters.Add("@date", Day.ToString("yyyy-MM-dd"));
            DataSet ds = DataService.GetDataSet(qc);


            return ds.Tables[0];
        }

        public static DataTable GetAppointment(DateTime Day)
        {
            string Query = "select r.ResourceID, m.StartTime, r.ResourceName, ISNULL(u.DisplayName,'') as Staff, s.MembershipNo ,ISNULL(s.NameToAppear,'') as Customer, isnull(m.listStr,'') as ItemName, m.Duration, m.Id as AppointmentID, m.CheckInByWho, m.CheckOutByWho, m.Remark, m.CheckInTime, m.CheckOutTime " +
                           " from [resource] r " +
                           " inner join  " +
                           " (	 " +
                           "     select *, STUFF((SELECT  ',' + ItemName FROM AppointmentItem EE inner join Item i on EE.ItemNo = i.ItemNo WHERE EE.AppointmentId=E.Id and ISNULL(EE.Deleted,0) = 0 ORDER BY ItemName FOR XML PATH('')), 1, 1, '') AS listStr " +
                           "     from Appointment E " +
                           "     WHERE ISNULL(E.Deleted,0) = 0 and Convert(date,E.StartTime) = '' + @date +'' " +
                           " )m on m.ResourceID = r.ResourceID " +
                           " left outer join UserMst u on m.SalesPersonID = u.UserName " +
                           " left outer join Membership s on s.MembershipNo = m.MembershipNo " +
                           " where ISNULL(r.Deleted,0) = 0  and ISNULL(u.Deleted,0) = 0 and ISNULL(s.Deleted,0) = 0 " +
                           " ORDER BY r.ResourceName ";

            QueryCommand qc = new QueryCommand(Query);
            qc.Parameters.Add("@date", Day.ToString("yyyy-MM-dd"));
            DataSet ds = DataService.GetDataSet(qc);


            return ds.Tables[0];
        }

        public static DataTable GetAppointment(string AppointmentID)
        {
            string Query = "select r.ResourceID, m.StartTime, r.ResourceName, ISNULL(u.DisplayName,'') as Staff, s.MembershipNo ,ISNULL(s.NameToAppear,'') as Customer, isnull(m.listStr,'') as ItemName, m.Duration, m.Id as AppointmentID, m.CheckInByWho, m.CheckOutByWho, m.Remark, m.CheckInTime, m.CheckOutTime " +
                           " from [resource] r " +
                           " inner join  " +
                           " (	 " +
                           "     select *, STUFF((SELECT  ',' + ItemName FROM AppointmentItem EE inner join Item i on EE.ItemNo = i.ItemNo WHERE EE.AppointmentId=E.Id and ISNULL(EE.Deleted,0) = 0 ORDER BY ItemName FOR XML PATH('')), 1, 1, '') AS listStr " +
                           "     from Appointment E " +
                           "     WHERE ISNULL(E.Deleted,0) = 0 and E.id = '' + @id +'' " +
                           " )m on m.ResourceID = r.ResourceID " +
                           " left outer join UserMst u on m.SalesPersonID = u.UserName " +
                           " left outer join Membership s on s.MembershipNo = m.MembershipNo " +
                           " where ISNULL(r.Deleted,0) = 0  and ISNULL(u.Deleted,0) = 0 and ISNULL(s.Deleted,0) = 0 " +
                           " ORDER BY r.ResourceName ";

            QueryCommand qc = new QueryCommand(Query);
            qc.Parameters.Add("@id", AppointmentID);
            DataSet ds = DataService.GetDataSet(qc);


            return ds.Tables[0];
        }
    }
}
