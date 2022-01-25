using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;
using SubSonic;

namespace PowerWeb.Reports
{
    public partial class InventoryDataRecording : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InventoryLocationController ctr = new InventoryLocationController();
                InventoryLocationCollection r = ctr.FetchAll();
                ListItem l;
                l = new ListItem();
                l.Text = "ALL";
                l.Value = "";
                ddInventoryLocation.Items.Add(l);
                for (int i = 0; i < r.Count; i++)
                {
                    l = new ListItem();
                    l.Text = r[i].InventoryLocationName;
                    l.Value = r[i].InventoryLocationID.ToString();
                    ddInventoryLocation.Items.Add(l);
                }

                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");

                BindGrid();
            }
        }

        protected void gvReport_DataBound(object sender, EventArgs e)
        {

        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            cbUseStartDate.Checked = true;
            cbUseEndDate.Checked = true;
            ddInventoryLocation.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = (DataTable)gvReport.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        }

        private void BindGrid()
        {
            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);

            DataTable dt = new DataTable();

            string[] tmp = new string[] { 
                "SELECT",
                "   *",
                "FROM RecordData",
                "LEFT JOIN InventoryLocation ON InventoryLocation.InventoryLocationID = RecordData.InventoryLocationID",
                "WHERE 1 = 1",
                cbUseStartDate.Checked && cbUseEndDate.Checked ? "   AND Timestamp >= @startdate AND Timestamp <= @enddate" : "",
                cbUseStartDate.Checked && !cbUseEndDate.Checked ? "   AND Timestamp >= @startdate" : "",
                !cbUseStartDate.Checked && cbUseEndDate.Checked ? "   AND Timestamp <= @enddate" : "",
                !ddInventoryLocation.SelectedValue.Equals("") ? "   AND InventoryLocationID = @inventoryLocationID" : "",
                "ORDER BY Timestamp ASC"
            };

            var sql = String.Join(" ", tmp);

            var qc = new QueryCommand(sql);
            qc.AddParameter("startdate", startDate.ToString("yyyy-MM-dd") + " 00:00:00");
            qc.AddParameter("enddate", endDate.ToString("yyyy-MM-dd") + " 23:59:59");
            if (!ddInventoryLocation.SelectedValue.Equals(""))
                qc.AddParameter("inventoryLocationID", ddInventoryLocation.SelectedValue);

            dt.Load(SubSonic.DataService.GetReader(qc));

            string[] tmp2 = new string[] {
                "SELECT",
                "   *",
                "FROM AppSetting",
                "WHERE",
                "   AppSettingKey LIKE 'Mobile_RecordData%'"
            };

            var sql2 = String.Join(" ", tmp2);

            var qc2 = new QueryCommand(sql2);
            IDataReader rdr = SubSonic.DataService.GetReader(qc2);
            while (rdr.Read())
            {
                foreach (BoundField c in gvReport.Columns)
	            {
                    if (rdr["AppSettingKey"].ToString().Replace("Mobile_RecordData", "").Equals(c.DataField.Replace("Val", "")))
                    {
                        c.HeaderText = rdr["AppSettingValue"].ToString();
                        break;
                    }
	            }
            }


            gvReport.DataSource = dt;
            gvReport.DataBind();
        }
    }
}
