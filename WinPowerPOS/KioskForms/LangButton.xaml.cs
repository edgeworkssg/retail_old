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
using PowerPOS;
using System.Windows.Interop;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for LangButton.xaml
    /// </summary>
    public partial class LangButton : UserControl
    {
        public delegate void OnClickEventHandler(object sender, EventArgs args);

        public event OnClickEventHandler OnClicked;

        public LangButton()
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
        }

        public void Reload()
        {
            //string text = "";

            string langID = AppSetting.GetSetting(AppSetting.SettingsName.LanguageSetting);
            langID = String.IsNullOrEmpty(langID) ? "ENG" : langID;

            if (langID == "ENG")
                btn.Content = "中文";
            else
                btn.Content = "ENG";

            //QueryCommand qc = new QueryCommand("SELECT ID, " + langID + " FROM TEXT_LANGUAGE WHERE ID LIKE @ID", "PowerPOS");
            //qc.AddParameter("ID", "StarkKioskButton.Start");

            //IDataReader rdr = DataService.GetReader(qc);
            //if (rdr.Read())
            //    text = rdr[langID].ToString();
        }

        private void Clicked(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();
            if (OnClicked != null)
                OnClicked(this, args);
        }
    }
}
