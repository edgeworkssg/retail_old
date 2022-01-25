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
using System.Windows.Interop;
using PowerPOS;
using System.Diagnostics;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for OrderList.xaml
    /// </summary>
    public partial class OrderList : UserControl
    {
        public DataTable DataSource { get; set; }

        public delegate void OnKeyDownEventHandler(object sender, KeyEventArgs args);
        public delegate void OnSelectedItemEventHandler(object sender, MouseButtonEventArgs args);
        public delegate void OnQtyChangedHandler(object sender, QtyChangeArgs args);
        /*public delegate void OnLoadedHandler(object sender);
        public delegate void OnLayoutUpdatedHandler(object sender);
        public delegate void OnTargetUpdatedHandler(object sender);*/

        public event OnKeyDownEventHandler OnKeyDown;
        public event OnSelectedItemEventHandler OnSelectedItem;
        public event OnQtyChangedHandler OnQtyChanged;
        /*public event OnLoadedHandler OnLoaded;
        public event OnLayoutUpdatedHandler OnLayoutUpdated;
        public event OnTargetUpdatedHandler OnTargetUpdated;*/

        public OrderList()
        {
            InitializeComponent();

            this.Loaded += delegate
            {
                var source = PresentationSource.FromVisual(this);
                var hwndTarget = source.CompositionTarget as HwndTarget;

                if (hwndTarget != null)
                {
                    hwndTarget.RenderMode = RenderMode.SoftwareOnly;
                }
            };

            lvOrder.KeyDown += new KeyEventHandler(lvOrder_KeyDown);
            //colPlus.Width = 0;
            //colMin.Width = 0;
        }

        public void ShowQtyColumn(bool isShow)
        {
            //colPlus.Width = isShow ? 65 : 0;
            //colMin.Width = isShow ? 65 : 0;
        }


        public void SetDataSource(DataTable dt)
        {
            DataSource = dt;

            if (DataSource != null)
            {
                lvOrder.ItemsSource = DataSource.DefaultView;
                lvOrder.SelectedIndex = 0;
                lvOrder.UpdateLayout();
                ListViewItem firstRow = ((ListViewItem)lvOrder.ItemContainerGenerator.ContainerFromIndex(0));
                if (firstRow == null)
                    return;
                firstRow.Focus();
                /*if (lvOrder.Items.Count > 0)
                    lvOrder.ScrollIntoView(lvOrder.Items[0]);*/
            }
        }

        public int ToInt(double d)
        {
            return (int)d;
        }

        protected void lvOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (OnKeyDown != null)
                OnKeyDown(this, e);
        }

        private void lvOrder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.OriginalSource is Border)
            //{
            //    Border btn = (Border)e.OriginalSource;
            //    if (btn != null)
            //    {
            //        if ((btn.Name + "").ToLower().Equals("button"))
            //        {
            //            DataRowView dr = (DataRowView)btn.DataContext;

            //            string id = dr["ID"] + "";
            //            decimal qty = (dr["Quantity"] + "").GetDecimalValue();
            //            string itemName = dr["ItemName"] + "";


            //            return;
            //        }
            //    }
            //}
            //else if (e.OriginalSource is TextBlock)
            //{
            //    TextBlock btn = (TextBlock)e.OriginalSource;

            //    if (btn.Text == "+" || btn.Text == "-")
            //    {
            //        DataRowView dr = (DataRowView)btn.DataContext;

            //        string id = dr["ID"] + "";
            //        decimal qty = (dr["Quantity"] + "").GetDecimalValue();
            //        string itemName = dr["ItemName"] + "";

            //        if (btn.Text == "+")
            //            qty++;
            //        else
            //            qty--;

            //        if (OnQtyChanged != null)
            //            OnQtyChanged(this, new QtyChangeArgs { ID = id, Quantity = qty, ItemName = itemName }); 
            //    }
            //}

            for (int i = 0; i < lvOrder.Items.Count; i++)
            {
                object selectedEntry = (object)lvOrder.Items[i];
                ListViewItem lbi = this.lvOrder.ItemContainerGenerator.ContainerFromItem(selectedEntry) as ListViewItem;
                if (lbi == null)
                    continue;
                lbi.IsSelected = false;
            }

            var item = sender as System.Windows.Controls.ListViewItem;
            item.IsSelected = true;
            if (OnSelectedItem != null)
                OnSelectedItem(sender, e);
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DataRowView dr = (DataRowView)btn.DataContext;

            string id = dr["ID"] + "";
            decimal qty = (dr["Quantity"] + "").GetDecimalValue();
            string itemName = dr["ItemName"] + "";

            qty++;

            if (OnQtyChanged != null)
                OnQtyChanged(this, new QtyChangeArgs { ID = id, Quantity = qty, ItemName = itemName }); 

            //MessageBox.Show("PLUS " + dr["ID"]);
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DataRowView dr = (DataRowView)btn.DataContext;

            string id = dr["ID"] + "";
            decimal qty = (dr["Quantity"] + "").GetDecimalValue();
            string itemName = dr["ItemName"] + "";
            
            qty--;

            if (OnQtyChanged != null)
                OnQtyChanged(this, new QtyChangeArgs { ID = id, Quantity = qty, ItemName = itemName }); 

            //MessageBox.Show("MIN " + dr["ID"]);
        }

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            //if (OnLayoutUpdated != null)
            //    OnLayoutUpdated(this); 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            //if (OnLoaded != null)
            //    OnLoaded(this); 
        }

        private void UserControl_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            //if (OnTargetUpdated != null)
            //    OnTargetUpdated(this); 

        }
    }

    public class QtyChangeArgs : EventArgs
    {
        public string ID { set; get; }
        public string ItemNo { set; get; }
        public string ItemName { set; get; }
        public decimal Quantity { set; get; }
    }
}
