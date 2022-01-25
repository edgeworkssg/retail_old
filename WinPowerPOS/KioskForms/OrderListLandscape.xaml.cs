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

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for OrderList.xaml
    /// </summary>
    public partial class OrderListLandscape : UserControl
    {
        public DataTable DataSource { get; set; }

        public delegate void OnKeyDownEventHandler(object sender, KeyEventArgs args);

        public event OnKeyDownEventHandler OnKeyDown;

        public OrderListLandscape()
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

            listView1.KeyDown += new KeyEventHandler(listView1_KeyDown);
        }

        void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (OnKeyDown != null)
            {
                OnKeyDown(this, e);
            }
        }

        public void SetDataSource(DataTable dt)
        {
            DataSource = dt;

            if (DataSource != null)
            {
                listView1.ItemsSource = DataSource.DefaultView;
                listView1.SelectedIndex = 0;
            }
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (OnKeyDown != null)
            {
                OnKeyDown(this, e);
            }
        }

        public int ToInt(double d)
        {
            return (int)d;
        }
    }
}
