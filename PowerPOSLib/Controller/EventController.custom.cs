using System;
using System.Collections.Generic;
using System.Text;
using SubSonic;
using PowerPOS;
using System.Data;

namespace PowerPOS
{
    public partial class SpecialEventController
    {
        public static bool InsertEventItemMapping
            (int EventId, string ItemNo, decimal ItemPrice, out string status)
        {
            //Check Uniqueness
            DataSet ds = EventItemMap.CreateQuery().
                WHERE("ItemNo = " + ItemNo).
                AND("EventID = " + EventId).
                AND("Deleted=false").
                ORDER_BY("ModifiedOn", "Desc").ExecuteDataSet();
            status = "";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {

                //Do Create
                EventItemMap myMap = new EventItemMap();
                myMap.EventId = EventId;
                myMap.ItemNo = ItemNo;
                myMap.ItemPrice = ItemPrice;
                myMap.Deleted = false;
                myMap.UniqueID = Guid.NewGuid();
                myMap.Save();
                return true;
            }
            else
            {
                //Do update
                EventItemMap myMap = new EventItemMap(ds.Tables[0].Rows[0]["EventItemMapID"].ToString());
                myMap.ItemPrice = ItemPrice;
                myMap.Save();
                return true;
            }                        
        }

        public static bool InsertEventLocationMapping
            (int EventId, int PointOfSaleID, out string status)
        {
            //Check Uniqueness
            DataSet ds = EventLocationMap.CreateQuery().
                            WHERE("PointOfSaleID = " + PointOfSaleID).
                            AND("EventID = " + EventId).AND("deleted=false").
                            ORDER_BY("ModifiedOn", "Desc").
                            ExecuteDataSet();

            status = "";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {

                //Do Create
                EventLocationMap myMap = new EventLocationMap();
                myMap.EventID  = EventId;
                myMap.PointOfSaleID  = PointOfSaleID;                
                myMap.Deleted = false;
                myMap.UniqueID = Guid.NewGuid();
                myMap.Save();
                return true;
            }
            return true;
            
        }

        public static DataTable ViewItemByEvent(int EventID)
        {
            ViewSpecialEventItemCollection vs = new ViewSpecialEventItemCollection();
            vs.Where(ViewSpecialEventItem.Columns.EventId, EventID);
            vs.OrderByDesc(ViewSpecialEventItem.Columns.EventItemMapID);                   
            vs.Load();
            
            return vs.ToDataTable();
        }

        public static DataTable ViewLocationByEvent(int EventID)
        {
            ViewEventLocationMapCollection vs = new ViewEventLocationMapCollection();
            vs.Where(ViewEventLocationMap.Columns.EventID, EventID);
            vs.OrderByDesc(ViewEventLocationMap.Columns.EventLocationMapID);
            vs.Load();

            return vs.ToDataTable();
        }

        public static decimal FetchSpecialEventPrice(DateTime CurrentDate, string ItemNo, int PointOfSaleID, out int eventID)
        {
            eventID = -1;
            DataSet ds = SPs.FetchSpecialEventPrice(CurrentDate, ItemNo, PointOfSaleID).GetDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() != "-1")
                {
                    eventID = int.Parse(ds.Tables[0].Rows[0][1].ToString());
                }
                return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            return -1;
         
        }

    }
}

