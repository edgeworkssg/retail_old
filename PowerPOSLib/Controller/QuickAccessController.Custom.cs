using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;

namespace PowerPOS
{
    /// <summary>
    /// Controller class for QuickAccessCategory
    /// </summary>    
    public partial class QuickAccessController
    {
        //Row Count & Col Count
        public const int ROW_COUNT = 8;
        public const int COL_COUNT = 7;
        
        public const string ButtonImageFolder = "\\ButtonImage\\";
        //Colors
        public const string BLUE = "blue";
        public const string GOLD = "gold";
        public const string GREEN = "green";
        public const string LIGHTBLUE = "lightblue";
        public const string PURPLE = "purple";
        public const string LIGHTGREEN = "lightgreen";
        public const string YELLOW = "yellow";
        public const string GREY = "grey";
        public const string ORANGE = "orange";
        public const string LIGHTORANGE = "lightorange";
        public const string RED = "red";
        public const string BROWN = "brown";
        //
        public static DataTable FetchCategories(int? PointOfSaleID,Guid GroupID)
        {

            if (GroupID != new Guid("00000000-0000-0000-0000-000000000000"))
            {                
                if (!PointOfSaleID.HasValue) PointOfSaleID = 0;
                string SQL1 = "select * from " +
                              "QuickAccessGroup a inner join QuickAccessGroupMap b " +
                              "on a.quickaccessgroupid=b.quickaccessgroupid " +
                              "inner join quickaccesscategory c " +
                              "on b.quickaccesscategoryid = c.quickaccesscategoryid " +
                                "where " +
                                "(Pointofsaleid = 0 or Pointofsaleid is null or Pointofsaleid=" + PointOfSaleID + ") " +
                                "and a.QuickAccessGroupID = '" + GroupID.ToString()+ "' " +
                                "and a.deleted=0 and b.deleted=0 and c.deleted=0 " +
                                "Order By c.PriorityLevel ";
                return DataService.GetDataSet(new QueryCommand(SQL1, "PowerPOS")).Tables[0];
            }
            else
            {
                string SQL = "select * from " +
		                    "QuickAccessGroup a inner join QuickAccessGroupMap b " +
		                    "on a.quickaccessgroupid=b.quickaccessgroupid " +
		                    "inner join quickaccesscategory c " +
		                    "on b.quickaccesscategoryid = c.quickaccesscategoryid " +
	                        "where a.deleted=0 and b.deleted=0 and c.deleted=0 " +
	                        "Order By c.PriorityLevel";
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                return DataService.GetDataSet(cmd).Tables[0];
            }
        }

        public static QuickAccessButtonCollection FetchButtons(Guid CategoryID)
        {
            QuickAccessButtonCollection qb = new QuickAccessButtonCollection();
            qb.Where(QuickAccessButton.Columns.Deleted, false);
            qb.Where(QuickAccessButton.Columns.QuickAccessCategoryID, CategoryID);
            qb.OrderByDesc(QuickAccessButton.Columns.PriorityLevel);

            return qb.Load();
        }
        public static bool AddCategoryGroupMap(Guid GroupID, Guid CatID)
        {
            //count how many
            Query qr = QuickAccessGroupMap.CreateQuery();
            qr.SelectList = QuickAccessGroupMap.Columns.QuickAccessGroupMapID;
            object o = qr.WHERE(QuickAccessGroupMap.Columns.QuickAccessGroupID, GroupID).
                WHERE(QuickAccessGroupMap.Columns.QuickAccessCategoryID, CatID).
                WHERE(QuickAccessGroupMap.Columns.Deleted, false).ExecuteScalar();

            if (o == null)
            {
                QuickAccessGroupMapController a = new QuickAccessGroupMapController();
                a.Insert(new Guid(), GroupID, CatID, false, DateTime.Now, "", DateTime.Now, "");
            }
            return true;
            
        }
    }
}

