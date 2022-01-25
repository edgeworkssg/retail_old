using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace PowerPOS
{
    public partial class UserGroupController
    {
        public static string MinimumPrice = "MinimumPrice";
        public static string CostPrice = "CostPrice";
        public static string AnyPrice = "AnyPrice";

        public static List<ListItem> GetPriceOverrideRestrictionList() {
            List<ListItem> list = new List<ListItem>();

            list.Add(new ListItem() { Text = "--Please Select--", Value = "" });
            list.Add(new ListItem() { Text = "Price Override Up To Minimum Selling Price", Value = MinimumPrice });
            list.Add(new ListItem() { Text = "Price Override Up To Cost Price", Value = CostPrice });
            list.Add(new ListItem() { Text = "Price Override Up To Any Price", Value = AnyPrice });

            return list;
        }
    }
}
