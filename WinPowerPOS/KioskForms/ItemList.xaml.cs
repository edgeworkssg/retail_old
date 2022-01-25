using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for ItemList.xaml
    /// </summary>
    public partial class ItemList : UserControl
    {
        public delegate void ItemSelectedHandler(object sender, ItemSelectedArgs e);
        public event ItemSelectedHandler ItemSelected;
        public event EventHandler CloseForm;

        private int page = 0;
        private int pageCount = 0;

        private int itemPerPage = 16;

        private DataTable _itemDataSource = new DataTable();
        public DataTable ItemDataSource
        {
            set
            {
                _itemDataSource = value;
                pageCount = (_itemDataSource.Rows.Count / itemPerPage);
                if (_itemDataSource.Rows.Count % itemPerPage != 0)
                    pageCount++;
                if (pageCount == 0)
                    pageCount = 1;
                DataBind();
            }
        }

        public ItemList()
        {
            this.InitializeComponent();
        }

        public void DataBind()
        {
            gridItems.Children.Clear();
            int max = ((page + 1) * itemPerPage);
            int min = (page * itemPerPage);

            for (int i = min; i < max; i++)
            {
                if (i >= _itemDataSource.Rows.Count)
                    break;

                string itemNo = _itemDataSource.Rows[i]["ItemNo"] + "";
                string itemName = _itemDataSource.Rows[i]["ItemName"] + "";
                string barcode = _itemDataSource.Rows[i]["Barcode"] + "";
                decimal unitPrice = ((decimal)_itemDataSource.Rows[i]["UnitPrice"]);

                ItemSelectedArgs theItem = new ItemSelectedArgs();
                theItem.ItemNo = itemNo;
                theItem.ItemName = itemName;
                theItem.Barcode = barcode;
                theItem.UnitPrice = unitPrice;

                Button btn = new Button();
                btn.Name = "btn__" + i;
                btn.Style = Resources["button"] as Style;
                btn.Tag = theItem;
                btn.Content = string.Format("{0}{1}{2}", barcode, Environment.NewLine, itemName);
                btn.Click += new RoutedEventHandler(btn_Click);
                btn.Height = 80;
                btn.Width = 150;
                btn.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                btn.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                gridItems.Children.Add(btn);
            }

            txtPage.Text = string.Format("Page {0} of {1} ", (page + 1), pageCount);

            btnPrev.Visibility = page != 0 ? Visibility.Visible : System.Windows.Visibility.Hidden;
            btnNext.Visibility = (page + 1) != pageCount ? Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (ItemSelected != null)
                ItemSelected(this, (ItemSelectedArgs)btn.Tag);
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page--;
                DataBind();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (page + 1 < pageCount)
            {
                page++;
                DataBind();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (CloseForm != null)
                CloseForm(this, e);
        }
    }

    public class ItemSelectedArgs : EventArgs
    {
        public string ItemNo { set; get; }
        public string ItemName { set; get; }
        public string Barcode { set; get; }
        public decimal UnitPrice { set; get; }
    }
}
