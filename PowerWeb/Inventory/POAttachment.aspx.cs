using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;

namespace PowerWeb.Inventory
{
    public partial class POAttachment : System.Web.UI.Page
    {
        public string refNo = "";
        public const int COL_FileSize = 1;
        public const int COL_FileURL = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            refNo = Request.QueryString["RefNo"] + "";
            if (!string.IsNullOrEmpty(refNo))
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            FileAttachmentCollection faColl = new FileAttachmentCollection();
            faColl.Where("ModuleName", "PurchaseOrder");
            faColl.Where("RefID", refNo);
            faColl.Load();
            if (faColl.Count == 0)
            {
                lblNoAttachment.Visible = true;
                gvAttachment.Visible = false;
                return;
            }
            gvAttachment.DataSource = faColl;
            gvAttachment.DataBind();
        }

        protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                FileAttachment fa = (FileAttachment)e.Row.DataItem;
                string fileSize;
                if (fa.FileSize > 1024 * 1024) // MB
                    fileSize = Math.Round((decimal)fa.FileSize / 1024 / 1024, 2).ToString("N2") + " MB";
                else if (fa.FileSize > 1024) // KB
                    fileSize = Math.Round((decimal)fa.FileSize / 1024, 2).ToString("N2") + " KB";
                else
                    fileSize = fa.FileSize.ToString() + " bytes";

                e.Row.Cells[COL_FileSize].Text = fileSize;

                string url = fa.FileLocation;
                HyperLink hlDownload = (HyperLink)e.Row.FindControl("hlDownload");
                hlDownload.NavigateUrl = fa.FileLocation.EndsWith("/") ? fa.FileLocation + fa.FileName : fa.FileLocation + "/" + fa.FileName;
            }
        }
    }
}
