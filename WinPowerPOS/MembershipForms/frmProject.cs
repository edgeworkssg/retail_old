using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using WinPowerPOS.OrderForms;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;
namespace WinPowerPOS.MembershipForms
{
    public partial class frmProject : Form
    {

        SqlConnection Sqlcon = new SqlConnection();
        String fsMemberShipNo = "";
        public String fsProjectName = "";

        public bool IsSuccessful = false;
       

        public frmProject(string MembershipNo)
        {
            this.fsMemberShipNo = MembershipNo;
            InitializeComponent();
        }
        public frmProject()
        {
            InitializeComponent();
        }

        private void frmProject_Load(object sender, EventArgs e)
        {
            try
            {
                Sqlcon.ConnectionString = ConfigurationManager.ConnectionStrings["PowerPOS"].ConnectionString;
                Sqlcon.Open();
                FillProject(fsMemberShipNo);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #region Private Methods
        private void FillProject(String fsMemberShip)
        {
            SqlCommand Sqlcom = new SqlCommand();
            Sqlcom.CommandText = "Select ID,ProjectName from Project Where MembershipNumber=@fsMemberShipNumber OR MembershipNumber='DEFAULT' ";
            Sqlcom.CommandType = CommandType.Text;
            Sqlcom.Connection = Sqlcon;
            Sqlcom.Parameters.AddWithValue("@fsMemberShipNumber", fsMemberShip);
            SqlDataAdapter SqlDap = new SqlDataAdapter(Sqlcom);
            DataSet ds = new DataSet();
            SqlDap.Fill(ds);
            this.cmbProject.ValueMember = "ID";
            this.cmbProject.DisplayMember = "ProjectName";
            this.cmbProject.DataSource = ds.Tables[0];


        }
        //public void InsertNewProject(String fsProjectName, String fsMemberShipNumber)
        //{
        //    String MyQuery = "Insert Into Project(ProjectName,MembershipNumber) Values(@fsProjectName,@fsMembershipNumber)";
        //    QueryCommand Qcmd = new QueryCommand(MyQuery);

        //    DataService.ExecuteQuery(Qcmd); 

        //}
        private void InsertNewProject(String fsProjectName, String fsMemberShipNumber)
        {
            SqlCommand Sqlcom = new SqlCommand();
            Sqlcom.CommandText = "Insert Into Project(ProjectName,MembershipNumber, CreatedOn, ModifiedOn,ModifiedBy, CreatedBy , UniqueID) Values(@fsProjectName,@fsMembershipNumber,@CreatedOn, @ModifiedOn,@ModifiedBy, @CreatedBy, @UniqueID)";
            Sqlcom.CommandType = CommandType.Text;
            Sqlcom.Connection = Sqlcon;
            Sqlcom.Parameters.AddWithValue("@fsProjectName", fsProjectName);
            Sqlcom.Parameters.AddWithValue("@fsMembershipNumber", fsMemberShipNo);
            Sqlcom.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
            Sqlcom.Parameters.AddWithValue("@CreatedBy", UserInfo.username);
            Sqlcom.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
            Sqlcom.Parameters.AddWithValue("@ModifiedBy", UserInfo.username);
            Sqlcom.Parameters.AddWithValue("@UniqueID", Guid.NewGuid().ToString());
            Sqlcom.ExecuteNonQuery();
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tbxNewProject.Text == String.Empty)
                {

                    if (this.cmbProject.SelectedIndex >= 0 && fsMemberShipNo.ToUpper() != "WALK-IN")
                    {
                        fsProjectName = this.cmbProject.Text;
                        //fnProjectId = Convert.ToInt32(this.cmbProject.SelectedValue.ToString());
                        IsSuccessful = true;
                    }
                    
                    this.Close();
                }
                else
                {
                    ProjectCollection project = new ProjectCollection();
                    DataTable dt = project.Where(Project.Columns.ProjectName, tbxNewProject.Text).Where(Project.Columns.MembershipNumber, fsMemberShipNo).Load().ToDataTable();
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Project already exists in this particular member");
                        return;
                    }

                    InsertNewProject(this.tbxNewProject.Text, fsMemberShipNo);
                    FillProject(fsMemberShipNo);
                    if (cmbProject.Items.Count > 0)
                    {
                        cmbProject.SelectedIndex = cmbProject.Items.Count - 1;
                    }
                    
                    this.tbxNewProject.Text = String.Empty;
                    this.cmbProject.Focus();

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //try
            //{
            //    if (this.tbxNewProject.Text != String.Empty)
            //    {
            //        InsertNewProject(this.tbxNewProject.Text, fsMemberShipNo);
            //        this.Close();
            //    }
            //    this.Close();
            //}
            //catch (Exception Ex)
            //{
            //    MessageBox.Show(Ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);   
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbxNewProject.Text = "";
        }

    }
}
