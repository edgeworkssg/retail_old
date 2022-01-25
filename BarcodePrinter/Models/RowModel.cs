using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;

namespace BarcodePrinter.Models
{
    public class RowModel
    {
        public object ItemName { get; set; }
        public object ItemNo { get; set; }
        public object CategoryName { get; set; }
        public object ItemDepartmentID { get; set; }
        public object Barcode { get; set; }
        public object AlternateBarcode { get; set; }
        public object RetailPrice { get; set; }
        public object Qty { get; set; }
        public object Attributes1 { get; set; }
        public object Attributes2 { get; set; }
        public object Attributes3 { get; set; }
        public object Attributes4 { get; set; }
        public object Attributes5 { get; set; }
        public object Attributes6 { get; set; }
        public object Attributes7 { get; set; }
        public object Attributes8 { get; set; }
        public object BarcodeText { get; set; }
        public int MatchLevel { get; set; }

        public RowModel()
        {
            Qty = new object();
            ItemName = new object();
            ItemNo = new object();
            CategoryName = new object();
            ItemDepartmentID = new object();
            Barcode = new object();
            AlternateBarcode = new object();
            RetailPrice = new object();
            Attributes1 = new object();
            Attributes2 = new object();
            Attributes3 = new object();
            Attributes4 = new object();
            Attributes5 = new object();
            Attributes6 = new object();
            Attributes7 = new object();
            Attributes8 = new object();
            BarcodeText = new object();
            MatchLevel = 0;
        }
    }

}
