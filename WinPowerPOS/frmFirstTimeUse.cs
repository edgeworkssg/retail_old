using PowerPOS;
using SubSonic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS
{
    public partial class frmFirstTimeUse : Form
    {
        private string ConStr;
        public frmFirstTimeUse(string conStr)
        {
            InitializeComponent();
            ConStr = conStr;
        }

        private void processFirstTime_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string FileLocation = Application.StartupPath + "\\";
                System.IO.TextReader Rdr;
                string sqlString = "";

                //Rdr = new System.IO.StreamReader(FileLocation + "Update Database\\Version 2.0\\04 Update AppSetting Field Length.sql");
                //sqlString = Rdr.ReadToEnd();
                //Rdr.Close();
                //DataService.ExecuteQuery(new QueryCommand(sqlString));

                System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c sqlcmd -E -S LOCALHOST\\SQLEXPRESS  -i \"" + FileLocation + "Update Database\\Version 2.0\\04 Update AppSetting Field Length.sql\"");
                StartInfo.CreateNoWindow = true;
                System.Diagnostics.Process.Start(StartInfo);

                StartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c sqlcmd -E -S LOCALHOST\\SQLEXPRESS  -i \"" + FileLocation + "Update Database\\Version 2.0\\05 Insert More Settings.sql\"");
                StartInfo.CreateNoWindow = true;
                System.Diagnostics.Process.Start(StartInfo);
                //AppSetting.SettingsName.Appointment.IsAvailable 
                //System.Diagnostics.Process.Start (
                //PowerPOS.Logger.writeLogToFile("PowerPOS: StartupPath: " + Application.StartupPath);
                //string backupPath = Application.StartupPath + "\\Resources\\Clean - Retail.bak";
                //System.Data.SqlCustomTools.Create_NewDatabase(ConStr);
                //System.Data.SqlCustomTools.Restore_FromBakFile(ConStr, backupPath);
                //e.Result = true;
            }
            catch (Exception X)
            {
                PowerPOS.Logger.writeLogToFile(X.Message);
                e.Result = false;
            }
        }

        private void processFirstTime_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            { this.DialogResult = DialogResult.Yes; }
            else
            { this.DialogResult = DialogResult.No; }

            this.Close();
        }

        private void frmFirstTimeUse_Load(object sender, EventArgs e)
        {
            PowerPOS.Logger.writeLogToFile("Do First Time Settings");
            processFirstTime.RunWorkerAsync();
        }
    }
}
