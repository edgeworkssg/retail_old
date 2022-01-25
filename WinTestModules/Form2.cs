using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;
using System.Collections;
namespace WinTestModules
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            DataTable dt = ReportController.FetchTransactionDetailWithSalesPersonReport(false, false, new DateTime(2007, 1, 1), DateTime.Now, "", "", 0, "", "", "", "", "");
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            */
            /*
            ArrayList emails = new ArrayList();
            emails.Add("tirtohadi@gmail.com");
            emails.Add("albert_tahu@hotmail.com");
            emails.Add("albert@edgeworks.com.sg");
            string status = "";
            MassEmail ms = new MassEmail();
            ArrayList failed = 
                ms.SendEmails(emails, "", "edgeworks123@gmail.com", 
                "TEST EMAIL", "test html", "text body", "smtp.gmail.com", 
                "edgeworks123", "pressingon", false, "d:\\test\\View_Report14Jul2009.csv", out status);

            MessageBox.Show(status + "done " +failed.Count.ToString());
            */
            /*
            bool useStartSalesDate, useEndSalesDate;
            DateTime startSalesDate, endSalesDate;
            ArrayList ItemList;
            bool useStartMembershipDate, useEndMembershipDate;
            DateTime StartMembershipDate, EndMembershipDate;
            bool useStartBirthDate,  useEndBirthDate;
            DateTime StartBirthDate, EndBirthDate;
            bool useBirthDayMonth; int BirthdayMonth;
            string ViewMembershipNo; int ViewGroupID; string NRIC; string gender; string name;
            string address;

            useStartSalesDate = true; useEndSalesDate = false; 
            startSalesDate = DateTime.Now; endSalesDate = DateTime.Now;
            ItemList = new ArrayList();
            useStartMembershipDate = false; useEndMembershipDate=false;
            StartMembershipDate = DateTime.Now ; EndMembershipDate = DateTime.Now;
            useStartBirthDate = false; useEndBirthDate = false;
            StartBirthDate = DateTime.Now; EndBirthDate = DateTime.Now;
            useBirthDayMonth = false; BirthdayMonth=1;
            ViewMembershipNo = ""; ViewGroupID=0; NRIC = ""; gender = ""; name = "alice"; 
            address = "";            
            ItemList.Add("I00100000016");
            ItemList.Add("VM-000001");
            ItemList.Add("HEALTH-000005");
            ItemList.Add("HEALTH-000010");
            ItemList.Add("FACIAL-000020");
            ItemList.Add("FACIAL-000016");

            dataGridView1.DataSource =
             ReportController.FetchMembershipProductSalesReport(
             useStartSalesDate, useEndSalesDate,
             startSalesDate, endSalesDate,
             ItemList,
             useStartMembershipDate, useEndMembershipDate,
             StartMembershipDate, EndMembershipDate,
             useStartBirthDate, useEndBirthDate,
             StartBirthDate, EndBirthDate,
             useBirthDayMonth, BirthdayMonth,
             ViewMembershipNo, ViewGroupID, NRIC, gender, name,
             address, "", "");
           
           dataGridView1.Refresh();*/
        }
    }
}