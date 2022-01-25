using SubSonic;
using PowerPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.ItemForms
{
    public partial class frmManageXMLButtons : Form
    {
        public frmManageXMLButtons()
        {
            InitializeComponent();
        }

        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string[] ColumnNames =
                (
                    "Department,Department Pos,Department ForeColor,Department BackColor" +
                    ",Category,Category Pos,Category ForeColor,Category BackColor"+
                    ",Item No,Item Pos,Item ForeColor,Item BackColor,Item Name"
                ).Split(',');
            string[] ColumnTypes = 
                (
                    "System.String,System.String,System.String,System.String"+
                    ",System.String,System.String,System.String,System.String"+
                    ",System.String,System.String,System.String,System.String,System.String"
                ).Split(',');

            Tabel.AutoGenerateColumns = false;

            try
            {
                string Status = "";

                #region *) Core: Attach data to Tabel
                Tabel.DataSource = ExcelDataLogic.OpenExcelFile(openFileDialog1.FileName, ColumnNames, ColumnTypes, out Status);

                if (!string.IsNullOrEmpty(Status)) throw new Exception(Status);
                #endregion
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string ErrMsg = X.Message;
                    for (Exception Err = X; Err.InnerException != null; Err = Err.InnerException, ErrMsg += Environment.NewLine + Err.Message) ;

                    //Logger.writeLog(ErrMsg + "Error at:" + Environment.NewLine + X.StackTrace);
                    MessageBox.Show("Some error occurred. Please contact your administrator. " + X.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void frmManageXMLButtons_Load(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT * FROM Temp_TouchScreen";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(SQL)));
                Tabel.DataSource = dt;

                const string XMLFILE = "\\TouchScreenQuickAccess.xml";

                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);

                ds.Tables[2].Rows.Clear();
                ds.Tables[1].Rows.Clear();
                ds.Tables[0].Rows.Clear();

                for (int Counter = 0; Counter < dt.Rows.Count; Counter++)
                {

                    string Location = "";

                    DataRow curr = dt.Rows[Counter];

                    List<DataRow> dr_Dept = new List<DataRow>();
                    List<DataRow> dr_Cate = new List<DataRow>();
                    List<DataRow> dr_Item = new List<DataRow>();

                    dr_Dept.AddRange(ds.Tables[0].Select("ItemDepartmentID = '" + curr["Department"].ToString() + "'"));
                    dr_Cate.AddRange(ds.Tables[1].Select("CategoryName = '" + curr["Category"].ToString() + "'"));
                    dr_Item.AddRange(ds.Tables[2].Select("ItemNo = '" + curr["ItemNo"].ToString() + "'"));

                    DataRow DeptRow;
                    if (dr_Dept.Count == 0)
                    {
                        DataRow newRow = ds.Tables[0].NewRow();
                        newRow["ItemDepartmentID"] = curr["Department"].ToString();
                        ds.Tables[0].Rows.Add(newRow);
                        DeptRow = newRow;
                    }
                    else
                    { DeptRow = dr_Dept[0]; }
                    //DeptRow = ds.Tables[0].Rows[0];
                    Location = curr["DepartmentPOS"].ToString();
                    DeptRow["X"] = Location.Substring(1, 1);
                    DeptRow["Y"] = Location.Substring(0, 1);
                    DeptRow["ForeColor"] = curr["DepartmentForeColor"].ToString();
                    DeptRow["BackColor"] = curr["DepartmentBackColor"].ToString();

                    DataRow CateRow;
                    if (dr_Cate.Count == 0)
                    {
                        DataRow newRow = ds.Tables[1].NewRow();
                        newRow["CategoryName"] = curr["Category"].ToString();
                        newRow["ItemDepartment_ID"] = DeptRow["ItemDepartment_ID"];
                        ds.Tables[1].Rows.Add(newRow);
                        CateRow = newRow;
                    }
                    else
                    { CateRow = dr_Cate[0]; }
                    //CateRow = ds.Tables[1].Rows[0];
                    Location = curr["CategoryPOS"].ToString();
                    CateRow["X"] = Location.Substring(1, 1);
                    CateRow["Y"] = Location.Substring(0, 1);
                    CateRow["ForeColor"] = curr["CategoryForeColor"].ToString();
                    CateRow["BackColor"] = curr["CategoryBackColor"].ToString();

                    DataRow ItemRow;
                    //if (dr_Cate.Count == 0)
                    {
                        DataRow newRow = ds.Tables[2].NewRow();
                        newRow["ItemNo"] = curr["ItemNo"].ToString();
                        newRow["Category_ID"] = CateRow["Category_ID"];
                        ds.Tables[2].Rows.Add(newRow);
                        ItemRow = newRow;
                    }
                    //ItemRow = ds.Tables[2].Rows[0];
                    Location = curr["ItemPOS"].ToString();
                    ItemRow["X"] = Location.Substring(1, 1);
                    ItemRow["Y"] = Location.Substring(0, 1);
                    ItemRow["ForeColor"] = curr["ItemForeColor"].ToString();
                    ItemRow["BackColor"] = curr["ItemBackColor"].ToString();
                }

                ds.WriteXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(ex.Message);
            }
    
        }

        private void Tabel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
