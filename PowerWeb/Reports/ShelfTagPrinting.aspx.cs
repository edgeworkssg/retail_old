using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SubSonic;
using System.Data;
using System.Globalization;
using PowerPOS.Container;

namespace PowerWeb.Reports
{
    public partial class ShelfTagPrinting : System.Web.UI.Page
    {
        private const string SELECTED_ITEMS_KEY = "ShelfTagPrinting.SelectedItems";
        private const string RPT_PATHS_KEY = "ShelfTagPrinting.RptPaths";
        private const string REPORT_DOCUMENT_KEY = "ShelfTagPrinting.ReportDocument";
        private const string REPORT_OBJECT_KEY = "ShelfTagPrinting.ReportObject";
        private const string REPORT_LOADED_KEY = "ShelfTagPrinting.ReportLoaded";


        private List<ShelfTagPrintingCart> carts;

        private List<string> rptPaths;

        public List<ShelfTagPrintingCart> GetCarts()
        {
            return (List<ShelfTagPrintingCart>)Session[SELECTED_ITEMS_KEY];
        }

        public List<string> GetRptPaths()
        {
            return (List<string>)Session[RPT_PATHS_KEY];
        }

        public void BindOutlet()
        {
            string userFld1 = "";

            #region get assigned outlets

            var sql2 = "SELECT userfld1 FROM UserMst UM WHERE UM.UserName = @UserName";
            QueryCommand cmd2 = new QueryCommand(sql2);
            cmd2.AddParameter("@UserName", Session["UserName"] != null ? Session["UserName"].ToString() : "");
            IDataReader rdr2 = DataService.GetReader(cmd2);
            if (rdr2.Read())
                userFld1 = rdr2["userFld1"].ToString();

            #endregion

            List<string> outlets = new List<string>();

            var sql = "SELECT OutletName FROM Outlet WHERE Deleted = 0";
            if (userFld1 != "ALL")
            {
                string tmp = " AND ";
                var tmps = userFld1.Split(',');
                if (tmps.Length > 0)
                {
                    for (int i = 0; i < tmps.Length; i++)
                    {
                        tmp += "OutletName = '" + tmps[i] + "'";
                        if (i < tmps.Length - 1)
                            tmp += " OR ";
                    }
                }
                else
                    tmp = "0";

                sql += tmp;
            }

            QueryCommand cmd = new QueryCommand(sql);

            IDataReader rdr = DataService.GetReader(cmd);
            while (rdr.Read())
                outlets.Add(rdr["OutletName"].ToString());

            ddlOutlet.DataSource = outlets;
            ddlOutlet.DataBind();
        }

        public void BindItem()
        {
            string term = "%" + txtProduct.Text + "%";

            string sql = "SELECT ItemNo, ItemName FROM Item WHERE ItemNo LIKE @term OR ItemName LIKE @term OR Barcode LIKE @term";
            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@term", term);

            List<DropdownListItem> tmps = new List<DropdownListItem>();

            IDataReader rdr = DataService.GetReader(cmd);
            while (rdr.Read())
            {
                tmps.Add(new DropdownListItem
                {
                    Value = rdr["ItemNo"].ToString(),
                    DisplayName = rdr["ItemName"].ToString()
                });
            }

            ddlItemDropdown.DataSource = tmps;
            ddlItemDropdown.DataTextField = "DisplayName";
            ddlItemDropdown.DataValueField = "Value";
            ddlItemDropdown.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region TEMPLATES

                rptPaths = new List<string>();

                try
                {
                    string[] files = Directory.GetFiles(Server.MapPath(@"~\bin\Reports\ReportFiles\ShelfTagPrinting\"), "*.rpt");
                    foreach (var file in files)
                    {
                        string tmp = file.Split(new string[] { @"\" }, StringSplitOptions.None).ToList().LastOrDefault();
                        rptPaths.Add(tmp);
                    }
                }
                catch (IOException ex) { }

                ddlTemplate.DataSource = rptPaths;
                ddlTemplate.DataBind();

                Session[RPT_PATHS_KEY] = rptPaths;

                #endregion

                #region OUTLETS

                BindOutlet();

                #endregion

                #region PRODUCTS

                carts = new List<ShelfTagPrintingCart>();
                Session[SELECTED_ITEMS_KEY] = carts;

                #endregion
            }
            else
            {
                if (Session[SELECTED_ITEMS_KEY] != null)
                    carts = (List<ShelfTagPrintingCart>)Session[SELECTED_ITEMS_KEY];

                if (Session[RPT_PATHS_KEY] != null)
                    rptPaths = (List<string>)Session[RPT_PATHS_KEY];


                string rptFile = "";
                if (Session[REPORT_DOCUMENT_KEY] != null)
                    rptFile = (string)Session[REPORT_DOCUMENT_KEY];

                List<ShelfTagPrintingObject> objs = new List<ShelfTagPrintingObject>();
                if (Session[REPORT_OBJECT_KEY] != null)
                    objs = (List<ShelfTagPrintingObject>)Session[REPORT_OBJECT_KEY];

                bool loaded = false;
                if (Session[REPORT_LOADED_KEY] != null)
                    loaded = (bool)Session[REPORT_LOADED_KEY];

                if (loaded)
                {
                    ReportDocument rptDoc = new ReportDocument();
                    rptDoc.Load(rptFile);
                    rptDoc.SetDataSource(objs);

                    crViewer.ReportSource = rptDoc;
                }
            }
        }

        //protected void btnAddProduct_Click(object sender, EventArgs e)
        //{
        //    Item item = new Item(hfSelectedItemNo.Value);
        //    if (item.IsLoaded && hfSelectedItemNo.Value != "")
        //    {
        //        products.Add(item);

        //        Session[SELECTED_ITEMS_KEY] = products;

        //        ObjectDataSource1.Select();
        //        gvCart.DataBind();
        //    }
        //    else
        //    {
        //        Response.Write("<script>alert('Item is not found')</script>");
        //    }

        //    hfSelectedItemNo.Value = null;

        //    Page.ClientScript.GetPostBackEventReference(btnAddProduct, "onclick");

        //}

        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            carts.RemoveAt(e.RowIndex);

            Session[SELECTED_ITEMS_KEY] = carts;

            ObjectDataSource1.Select();
            gvCart.DataBind();

            e.Cancel = true;
        }

        protected void btnCreateTag_Click(object sender, EventArgs e)
        {
            List<ShelfTagPrintingObject> objs = new List<ShelfTagPrintingObject>();
            string status = "";
            lblResult.Text = "";
            if (!GetPrintingData(out objs, out status))
            {
                lblResult.Text = "Error Get Data :" + status;
                return;
            }

            try
            {
                List<string> tmps = new List<string>();
                string[] files = Directory.GetFiles(Server.MapPath(@"~\bin\Reports\ReportFiles\ShelfTagPrinting\"), "*.rpt");
                foreach (var file in files)
                    tmps.Add(file);

                var rptFile = tmps.Where(x => x.Contains(ddlTemplate.SelectedValue)).FirstOrDefault();

                Session[REPORT_DOCUMENT_KEY] = rptFile;
                Session[REPORT_OBJECT_KEY] = objs;
                Session[REPORT_LOADED_KEY] = true;

                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(rptFile);
                rptDoc.SetDataSource(objs);

                crViewer.ReportSource = rptDoc;
            }
            catch (IOException ex) { }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            List<ShelfTagPrintingObject> objs = new List<ShelfTagPrintingObject>();
            string status = "";
            lblResult.Text = "";
            if(!GetPrintingData(out objs, out status))
            {
                lblResult.Text = "Error Get Data :" + status;
                return;
            }

            try
            {
                List<string> tmps = new List<string>();
                string[] files = Directory.GetFiles(Server.MapPath(@"~\bin\Reports\ReportFiles\ShelfTagPrinting\"), "*.rpt");
                foreach (var file in files)
                    tmps.Add(file);

                var rptFile = tmps.Where(x => x.Contains(ddlTemplate.SelectedValue)).FirstOrDefault();

                Session[REPORT_DOCUMENT_KEY] = rptFile;
                Session[REPORT_OBJECT_KEY] = objs;
                Session[REPORT_LOADED_KEY] = true;

                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(rptFile);
                rptDoc.SetDataSource(objs);

                rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "ShelfTagPrinting " + DateTime.Now.ToString("dd MMM yyyy HHmmss"));
            }
            catch (IOException ex) { lblResult.Text = "Error Print :" + ex.Message; }
        }

        private bool GetPrintingData(out List<ShelfTagPrintingObject> objs, out string status)
        {
            objs = new List<ShelfTagPrintingObject>();
            status = "";
            try
            {
                int perPage = 1;

                int startFrom = 1;
                int.TryParse(txtStartFrom.Text, out startFrom);

                string selectedTemplate = ddlTemplate.SelectedValue;
                string format = selectedTemplate.Replace(".rpt", "").Split(new string[] { @"_" }, StringSplitOptions.None)[1];
                {
                    int x = 0;
                    int y = 0;

                    string[] tmp = format.Split(new string[] { "x" }, StringSplitOptions.None);

                    int.TryParse(tmp[0], out x);
                    int.TryParse(tmp[1], out y);

                    perPage = x * y;
                }



                ShelfTagPrintingObject obj = new ShelfTagPrintingObject();

                // convert qty to single obj
                List<ShelfTagPrintingCart> singleQtyCarts = new List<ShelfTagPrintingCart>();

                for (int i = 0; i < startFrom - 1; i++)
                    singleQtyCarts.Add(new ShelfTagPrintingCart { ItemNo = "", ItemName = "", Qty = 0 });

                foreach (var c in carts)
                {
                    for (int i = 0; i < c.Qty; i++)
                    {
                        singleQtyCarts.Add(new ShelfTagPrintingCart { ItemNo = c.ItemNo, ItemName = c.ItemName, Qty = 1 });
                    }
                }

                string sql = "SELECT OutletGroupID FROM Outlet WHERE OutletName = @OutletName";
                QueryCommand cmd = new QueryCommand(sql);
                cmd.AddParameter("@OutletName", ddlOutlet.SelectedValue);

                int outletGroupID = (int)DataService.ExecuteScalar(cmd);

                int ctr = 0;
                for (int i = 0; i < singleQtyCarts.Count; i++)
                {
                    Item item = new Item(singleQtyCarts[i].ItemNo);

                    string sql2 = @"
                    SELECT TOP 1
                        CASE WHEN ISNULL(ou.RetailPrice, -1) <> -1 THEN ou.RetailPrice ELSE case when ISNULL(og.RetailPrice, -1) <> -1 THEN og.RetailPrice ELSE i.RetailPrice END END RetailPrice
                    FROM Item I
                    LEFT JOIN OutletGroupItemMap OG ON I.itemno = OG.ItemNo AND ISNULL(OG.Deleted,0) = 0 AND OG.OutletGroupID = @OutletGroupID
                    LEFT OUTER JOIN OutletGroupItemMap OU ON i.itemno = OU.ItemNo AND ISNULL(OU.Deleted,0) = 0 AND OU.OutletName = @OutletName
                    WHERE I.itemno = @ItemNo
                ";
                    QueryCommand cmd2 = new QueryCommand(sql2);
                    cmd2.AddParameter("@OutletName", ddlOutlet.SelectedValue);
                    cmd2.AddParameter("@OutletGroupID", outletGroupID);
                    cmd2.AddParameter("@ItemNo", item.ItemNo);
                    double retailPrice = 0d;

                    string sRetailPrice = "0";

                    IDataReader rdr2 = DataService.GetReader(cmd2);
                    if (rdr2.Read())
                    {
                        sRetailPrice = rdr2["RetailPrice"].ToString();
                    }

                    double.TryParse(sRetailPrice, out retailPrice);

                    string sql3 = @"
                    SELECT TOP 1
	                    PromoCampaignName
                        , promoprice
                        , UnitQty        
	                    , Priority
                    FROM
                    (
                    SELECT 
                        PH.PromoCampaignName
                        , PD.promoprice
                        , PD.UnitQty        
	                    , PH.[Priority]
                    FROM PromoCampaignHdr PH
                    JOIN PromoCampaignDet PD ON PD.PromoCampaignHdrID = PH.PromoCampaignHdrID
                    WHERE PD.ItemNo = @ItemNo AND PD.Deleted = 0 AND getdate() between PH.DateFrom and PH.DateTo
                    UNION
                    SELECT 
                        PH.PromoCampaignName
                        , PD.promoprice
                        , PD.UnitQty    
	                    , PH.[Priority]
                    FROM PromoCampaignHdr PH
                    JOIN PromoCampaignDet PD ON PD.PromoCampaignHdrID = PH.PromoCampaignHdrID
                    JOIN ItemgroupMap IG on IG.ItemGroupID = PD.ItemGroupID
                    WHERE IG.ItemNo = @ItemNo AND PD.Deleted = 0 AND getdate() between PH.DateFrom and PH.DateTo
                    ) X
                    ORDER BY [Priority] ASC
                ";
                    QueryCommand cmd3 = new QueryCommand(sql3);
                    cmd3.AddParameter("@ItemNo", item.ItemNo);
                    //cmd3.AddParameter("@Today", DateTime.Now);

                    string promoName = "";
                    double promoPrice = 0d;
                    DateTime promoDateTo = new DateTime();
                    DateTime promoDateFrom = new DateTime();
                    int promoQty = 0;
                    IDataReader rdr = DataService.GetReader(cmd3);
                    if (rdr.Read())
                    {
                        promoName = rdr["PromoCampaignName"].ToString();
                        double.TryParse(rdr["promoprice"].ToString(), out promoPrice);
                        int.TryParse(rdr["UnitQty"].ToString(), out promoQty);
                    }

                    string sql4 = @"
                    SELECT TOP 1
	                    PromoCampaignName
                        , promoprice
                        , UnitQty        
	                    , Priority
                        , DateFrom  
                        , DateTo 
                    FROM
                    (
                    SELECT 
                        PH.PromoCampaignName
                        , PD.promoprice
                        , PD.UnitQty        
	                    , PH.[Priority]
                        , PH.DateFrom  
                        , PH.DateTo 
                    FROM PromoCampaignHdr PH
                    JOIN PromoCampaignDet PD ON PD.PromoCampaignHdrID = PH.PromoCampaignHdrID
                    WHERE PD.ItemNo = @ItemNo AND PD.Deleted = 0 
                    UNION
                    SELECT 
                        PH.PromoCampaignName
                        , PD.promoprice
                        , PD.UnitQty    
	                    , PH.[Priority]
                        , PH.DateFrom  
                        , PH.DateTo 
                    FROM PromoCampaignHdr PH
                    JOIN PromoCampaignDet PD ON PD.PromoCampaignHdrID = PH.PromoCampaignHdrID
                    JOIN ItemgroupMap IG on IG.ItemGroupID = PD.ItemGroupID
                    WHERE IG.ItemNo = @ItemNo AND PD.Deleted = 0 
                    ) X
                    ORDER BY [Priority] ASC
                ";
                    QueryCommand cmd4 = new QueryCommand(sql4);
                    cmd4.AddParameter("@ItemNo", item.ItemNo);
                    IDataReader rdr3 = DataService.GetReader(cmd4);
                    if (rdr3.Read())
                    {
                        if ((DateTime)rdr3["DateTo"] != null)
                            promoDateTo = (DateTime)rdr3["DateTo"];

                        if ((DateTime)rdr3["DateFrom"] != null)
                            promoDateFrom = (DateTime)rdr3["DateFrom"];
                    }

                    if (promoQty <= 0)
                        promoQty = 1;

                    if (i % perPage == 0)
                    {
                        obj.T1Visible = i + 1 >= startFrom;
                        obj.T1ItemNo = item.ItemNo;
                        obj.T1ItemName = item.ItemName;
                        obj.T1Attributes1 = item.Attributes1;
                        obj.T1Attributes2 = item.Attributes2;
                        obj.T1Attributes3 = item.Attributes3;
                        obj.T1Attributes4 = item.Attributes4;
                        obj.T1Attributes5 = item.Attributes5;
                        obj.T1Attributes6 = item.Attributes6;
                        obj.T1Attributes7 = item.Attributes7;
                        obj.T1Attributes8 = item.Attributes8;
                        obj.T1OriginalRetailPrice = item.RetailPrice;
                        obj.T1RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T1P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T1PromoName = promoName;
                        obj.T1PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T1PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T1PromoDateFrom = promoDateFrom;
                        obj.T1PromoDateTo = promoDateTo;
                        obj.T1P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T1P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T1P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T1P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T1P5Price = item.P5Price.GetValueOrDefault(0);
                        ctr++;
                    }

                    if (i % perPage == 1)
                    {
                        obj.T2Visible = i + 1 >= startFrom;
                        obj.T2ItemNo = singleQtyCarts[i].ItemNo;
                        obj.T2ItemName = singleQtyCarts[i].ItemName;
                        obj.T2Attributes1 = item.Attributes1;
                        obj.T2Attributes2 = item.Attributes2;
                        obj.T2Attributes3 = item.Attributes3;
                        obj.T2Attributes4 = item.Attributes4;
                        obj.T2Attributes5 = item.Attributes5;
                        obj.T2Attributes6 = item.Attributes6;
                        obj.T2Attributes7 = item.Attributes7;
                        obj.T2Attributes8 = item.Attributes8;
                        obj.T2OriginalRetailPrice = item.RetailPrice;
                        obj.T2RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T2PromoName = promoName;
                        obj.T2PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T2PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T2PromoDateFrom = promoDateFrom;
                        obj.T2PromoDateTo = promoDateTo;
                        obj.T2P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T2P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T2P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T2P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T2P5Price = item.P5Price.GetValueOrDefault(0);
                        ctr++;
                    }

                    if (i % perPage == 2)
                    {
                        obj.T3Visible = i + 1 >= startFrom;
                        obj.T3ItemNo = singleQtyCarts[i].ItemNo;
                        obj.T3ItemName = singleQtyCarts[i].ItemName;
                        obj.T3Attributes1 = item.Attributes1;
                        obj.T3Attributes2 = item.Attributes2;
                        obj.T3Attributes3 = item.Attributes3;
                        obj.T3Attributes4 = item.Attributes4;
                        obj.T3Attributes5 = item.Attributes5;
                        obj.T3Attributes6 = item.Attributes6;
                        obj.T3Attributes7 = item.Attributes7;
                        obj.T3Attributes8 = item.Attributes8;
                        obj.T3OriginalRetailPrice = item.RetailPrice;
                        obj.T3RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T3PromoName = promoName;
                        obj.T3PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T3PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T3PromoDateFrom = promoDateFrom;
                        obj.T3PromoDateTo = promoDateTo;
                        obj.T3P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T3P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T3P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T3P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T3P5Price = item.P5Price.GetValueOrDefault(0);
                        ctr++;
                    }

                    if (i % perPage == 3)
                    {
                        obj.T4Visible = i + 1 >= startFrom;
                        obj.T4ItemNo = singleQtyCarts[i].ItemNo;
                        obj.T4ItemName = singleQtyCarts[i].ItemName;
                        obj.T4Attributes1 = item.Attributes1;
                        obj.T4Attributes2 = item.Attributes2;
                        obj.T4Attributes3 = item.Attributes3;
                        obj.T4Attributes4 = item.Attributes4;
                        obj.T4Attributes5 = item.Attributes5;
                        obj.T4Attributes6 = item.Attributes6;
                        obj.T4Attributes7 = item.Attributes7;
                        obj.T4Attributes8 = item.Attributes8;
                        obj.T4OriginalRetailPrice = item.RetailPrice;
                        obj.T4RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T4PromoName = promoName;
                        obj.T4PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T4PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T4PromoDateFrom = promoDateFrom;
                        obj.T4PromoDateTo = promoDateTo;
                        obj.T4P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T4P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T4P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T4P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T4P5Price = item.P5Price.GetValueOrDefault(0);
                        ctr++;
                    }

                    if (i % perPage == 4)
                    {
                        obj.T5Visible = i + 1 >= startFrom;
                        obj.T5ItemNo = singleQtyCarts[i].ItemNo;
                        obj.T5ItemName = singleQtyCarts[i].ItemName;
                        obj.T5Attributes1 = item.Attributes1;
                        obj.T5Attributes2 = item.Attributes2;
                        obj.T5Attributes3 = item.Attributes3;
                        obj.T5Attributes4 = item.Attributes4;
                        obj.T5Attributes5 = item.Attributes5;
                        obj.T5Attributes6 = item.Attributes6;
                        obj.T5Attributes7 = item.Attributes7;
                        obj.T5Attributes8 = item.Attributes8;
                        obj.T5OriginalRetailPrice = item.RetailPrice;
                        obj.T5RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T5PromoName = promoName;
                        obj.T5PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T5PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T5PromoDateFrom = promoDateFrom;
                        obj.T5PromoDateTo = promoDateTo;
                        obj.T5P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T5P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T5P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T5P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T5P5Price = item.P5Price.GetValueOrDefault(0);

                        ctr++;
                    }

                    if (i % perPage == 5)
                    {
                        obj.T6Visible = i + 1 >= startFrom;
                        obj.T6ItemNo = singleQtyCarts[i].ItemNo;
                        obj.T6ItemName = singleQtyCarts[i].ItemName;
                        obj.T6Attributes1 = item.Attributes1;
                        obj.T6Attributes2 = item.Attributes2;
                        obj.T6Attributes3 = item.Attributes3;
                        obj.T6Attributes4 = item.Attributes4;
                        obj.T6Attributes5 = item.Attributes5;
                        obj.T6Attributes6 = item.Attributes6;
                        obj.T6Attributes7 = item.Attributes7;
                        obj.T6Attributes8 = item.Attributes8;
                        obj.T6OriginalRetailPrice = item.RetailPrice;
                        obj.T6RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T6PromoName = promoName;
                        obj.T6PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T6PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T6PromoDateFrom = promoDateFrom;
                        obj.T6PromoDateTo = promoDateTo;
                        obj.T6P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T6P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T6P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T6P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T6P5Price = item.P5Price.GetValueOrDefault(0);
                        ctr++;
                    }

                    if (i % perPage == 6)
                    {
                        obj.T7Visible = i + 1 >= startFrom;
                        obj.T7ItemNo = singleQtyCarts[i].ItemNo;
                        obj.T7ItemName = singleQtyCarts[i].ItemName;
                        obj.T7Attributes1 = item.Attributes1;
                        obj.T7Attributes2 = item.Attributes2;
                        obj.T7Attributes3 = item.Attributes3;
                        obj.T7Attributes4 = item.Attributes4;
                        obj.T7Attributes5 = item.Attributes5;
                        obj.T7Attributes6 = item.Attributes6;
                        obj.T7Attributes7 = item.Attributes7;
                        obj.T7Attributes8 = item.Attributes8;
                        obj.T7OriginalRetailPrice = item.RetailPrice;
                        obj.T7RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T7PromoName = promoName;
                        obj.T7PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T7PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T7PromoDateFrom = promoDateFrom;
                        obj.T7PromoDateTo = promoDateTo;
                        obj.T7P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T7P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T7P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T7P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T7P5Price = item.P5Price.GetValueOrDefault(0);
                        ctr++;
                    }

                    if (i % perPage == 7)
                    {
                        obj.T8Visible = i + 1 >= startFrom;
                        obj.T8ItemNo = singleQtyCarts[i].ItemNo;
                        obj.T8ItemName = singleQtyCarts[i].ItemName;
                        obj.T8Attributes1 = item.Attributes1;
                        obj.T8Attributes2 = item.Attributes2;
                        obj.T8Attributes3 = item.Attributes3;
                        obj.T8Attributes4 = item.Attributes4;
                        obj.T8Attributes5 = item.Attributes5;
                        obj.T8Attributes6 = item.Attributes6;
                        obj.T8Attributes7 = item.Attributes7;
                        obj.T8Attributes8 = item.Attributes8;
                        obj.T8OriginalRetailPrice = item.RetailPrice;
                        obj.T8RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T8PromoName = promoName;
                        obj.T8PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T8PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T8PromoDateFrom = promoDateFrom;
                        obj.T8PromoDateTo = promoDateTo;
                        obj.T8P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T8P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T8P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T8P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T8P5Price = item.P5Price.GetValueOrDefault(0);
                        ctr++;
                    }

                    if (i % perPage == 8)
                    {
                        obj.T9Visible = i + 1 >= startFrom;
                        obj.T9ItemNo = singleQtyCarts[i].ItemNo;
                        obj.T9ItemName = singleQtyCarts[i].ItemName;
                        obj.T9Attributes1 = item.Attributes1;
                        obj.T9Attributes2 = item.Attributes2;
                        obj.T9Attributes3 = item.Attributes3;
                        obj.T9Attributes4 = item.Attributes4;
                        obj.T9Attributes5 = item.Attributes5;
                        obj.T9Attributes6 = item.Attributes6;
                        obj.T9Attributes7 = item.Attributes7;
                        obj.T9Attributes8 = item.Attributes8;
                        obj.T9OriginalRetailPrice = item.RetailPrice;
                        obj.T9RetailPrice = promoPrice > 0 ? "U.P:" + (retailPrice * promoQty).ToString("N2", CultureInfo.CurrentCulture) : "";
                        obj.T9PromoName = promoName;
                        obj.T9PromoPrice = promoPrice <= 0 ? retailPrice : promoPrice;
                        obj.T9PromoQty = promoQty <= 1 ? "EACH" : "FOR " + promoQty;
                        obj.T9PromoDateFrom = promoDateFrom;
                        obj.T9PromoDateTo = promoDateTo;
                        obj.T9P1Price = item.P1Price.GetValueOrDefault(0);
                        obj.T9P2Price = item.P2Price.GetValueOrDefault(0);
                        obj.T9P3Price = item.P3Price.GetValueOrDefault(0);
                        obj.T9P4Price = item.P4Price.GetValueOrDefault(0);
                        obj.T9P5Price = item.P5Price.GetValueOrDefault(0);
                        ctr++;
                    }

                    if (ctr % perPage == 0 || i == singleQtyCarts.Count - 1)
                    {

                        //set a level price name 
                        obj.P1DiscountName = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
                        obj.P2DiscountName = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
                        obj.P3DiscountName = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
                        obj.P4DiscountName = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
                        obj.P5DiscountName = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);
                        objs.Add(obj);

                        obj = new ShelfTagPrintingObject();
                        ctr = 0;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindItem();
        }

        protected void btnAddToList_Click(object sender, EventArgs e)
        {
            Item item = new Item(ddlItemDropdown.SelectedValue);
            if (item.IsLoaded && ddlItemDropdown.SelectedValue != "")
            {
                int qty = 0;
                int.TryParse(txtQty.Text, out qty);

                ShelfTagPrintingCart c = new ShelfTagPrintingCart
                {
                    ItemNo = item.ItemNo,
                    ItemName = item.ItemName,
                    Qty = qty > 1 ? qty : 1
                };

                carts.Add(c);

                Session[SELECTED_ITEMS_KEY] = carts;

                ObjectDataSource1.Select();
                gvCart.DataBind();
            }
        }
    }

    public class ShelfTagPrintingCart
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
    }

    public class ShelfTagPrintingObject
    {
        public string P1DiscountName { get; set; }
        public string P2DiscountName { get; set; }
        public string P3DiscountName { get; set; }
        public string P4DiscountName { get; set; }
        public string P5DiscountName { get; set; }

        public string T1ItemNo { get; set; }
        public string T1ItemName { get; set; }
        public string T1Attributes1 { get; set; }
        public string T1Attributes2 { get; set; }
        public string T1Attributes3 { get; set; }
        public string T1Attributes4 { get; set; }
        public string T1Attributes5 { get; set; }
        public string T1Attributes6 { get; set; }
        public string T1Attributes7 { get; set; }
        public string T1Attributes8 { get; set; }       
        public string T1PromoName { get; set; }
        public string T1PromoQty { get; set; }
        public double T1PromoPrice { get; set; }
        public DateTime T1PromoDateFrom { get; set; }
        public DateTime T1PromoDateTo { get; set; }
        public decimal T1OriginalRetailPrice { get; set; }
        public string T1RetailPrice { get; set; }
        public decimal T1P1Price { get; set; }
        public decimal T1P2Price { get; set; }
        public decimal T1P3Price { get; set; }
        public decimal T1P4Price { get; set; }
        public decimal T1P5Price { get; set; }
        public bool T1Visible { get; set; }

        public string T2ItemNo { get; set; }
        public string T2ItemName { get; set; }
        public string T2Attributes1 { get; set; }
        public string T2Attributes2 { get; set; }
        public string T2Attributes3 { get; set; }
        public string T2Attributes4 { get; set; }
        public string T2Attributes5 { get; set; }
        public string T2Attributes6 { get; set; }
        public string T2Attributes7 { get; set; }
        public string T2Attributes8 { get; set; }
        public string T2PromoName { get; set; }
        public string T2PromoQty { get; set; }
        public double T2PromoPrice { get; set; }
        public DateTime T2PromoDateFrom { get; set; }
        public DateTime T2PromoDateTo { get; set; }
        public decimal T2OriginalRetailPrice { get; set; }
        public string T2RetailPrice { get; set; }
        public decimal T2P1Price { get; set; }
        public decimal T2P2Price { get; set; }
        public decimal T2P3Price { get; set; }
        public decimal T2P4Price { get; set; }
        public decimal T2P5Price { get; set; }
        public bool T2Visible { get; set; }

        public string T3ItemNo { get; set; }
        public string T3ItemName { get; set; }
        public string T3Attributes1 { get; set; }
        public string T3Attributes2 { get; set; }
        public string T3Attributes3 { get; set; }
        public string T3Attributes4 { get; set; }
        public string T3Attributes5 { get; set; }
        public string T3Attributes6 { get; set; }
        public string T3Attributes7 { get; set; }
        public string T3Attributes8 { get; set; }
        public string T3PromoName { get; set; }
        public string T3PromoQty { get; set; }
        public double T3PromoPrice { get; set; }
        public DateTime T3PromoDateFrom { get; set; }
        public DateTime T3PromoDateTo { get; set; }
        public decimal T3OriginalRetailPrice { get; set; }
        public string T3RetailPrice { get; set; }
        public decimal T3P1Price { get; set; }
        public decimal T3P2Price { get; set; }
        public decimal T3P3Price { get; set; }
        public decimal T3P4Price { get; set; }
        public decimal T3P5Price { get; set; }
        public bool T3Visible { get; set; }

        public string T4ItemNo { get; set; }
        public string T4ItemName { get; set; }
        public string T4Attributes1 { get; set; }
        public string T4Attributes2 { get; set; }
        public string T4Attributes3 { get; set; }
        public string T4Attributes4 { get; set; }
        public string T4Attributes5 { get; set; }
        public string T4Attributes6 { get; set; }
        public string T4Attributes7 { get; set; }
        public string T4Attributes8 { get; set; }
        public string T4PromoName { get; set; }
        public string T4PromoQty { get; set; }
        public double T4PromoPrice { get; set; }
        public DateTime T4PromoDateFrom { get; set; }
        public DateTime T4PromoDateTo { get; set; }
        public decimal T4OriginalRetailPrice { get; set; }
        public string T4RetailPrice { get; set; }
        public decimal T4P1Price { get; set; }
        public decimal T4P2Price { get; set; }
        public decimal T4P3Price { get; set; }
        public decimal T4P4Price { get; set; }
        public decimal T4P5Price { get; set; }
        public bool T4Visible { get; set; }

        public string T5ItemNo { get; set; }
        public string T5ItemName { get; set; }
        public string T5Attributes1 { get; set; }
        public string T5Attributes2 { get; set; }
        public string T5Attributes3 { get; set; }
        public string T5Attributes4 { get; set; }
        public string T5Attributes5 { get; set; }
        public string T5Attributes6 { get; set; }
        public string T5Attributes7 { get; set; }
        public string T5Attributes8 { get; set; }
        public string T5PromoName { get; set; }
        public string T5PromoQty { get; set; }
        public double T5PromoPrice { get; set; }
        public DateTime T5PromoDateFrom { get; set; }
        public DateTime T5PromoDateTo { get; set; }
        public decimal T5OriginalRetailPrice { get; set; }
        public string T5RetailPrice { get; set; }
        public decimal T5P1Price { get; set; }
        public decimal T5P2Price { get; set; }
        public decimal T5P3Price { get; set; }
        public decimal T5P4Price { get; set; }
        public decimal T5P5Price { get; set; }
        public bool T5Visible { get; set; }

        public string T6ItemNo { get; set; }
        public string T6ItemName { get; set; }
        public string T6Attributes1 { get; set; }
        public string T6Attributes2 { get; set; }
        public string T6Attributes3 { get; set; }
        public string T6Attributes4 { get; set; }
        public string T6Attributes5 { get; set; }
        public string T6Attributes6 { get; set; }
        public string T6Attributes7 { get; set; }
        public string T6Attributes8 { get; set; }
        public string T6PromoName { get; set; }
        public string T6PromoQty { get; set; }
        public double T6PromoPrice { get; set; }
        public DateTime T6PromoDateFrom { get; set; }
        public DateTime T6PromoDateTo { get; set; }
        public decimal T6OriginalRetailPrice { get; set; }
        public string T6RetailPrice { get; set; }
        public decimal T6P1Price { get; set; }
        public decimal T6P2Price { get; set; }
        public decimal T6P3Price { get; set; }
        public decimal T6P4Price { get; set; }
        public decimal T6P5Price { get; set; }
        public bool T6Visible { get; set; }

        public string T7ItemNo { get; set; }
        public string T7ItemName { get; set; }
        public string T7Attributes1 { get; set; }
        public string T7Attributes2 { get; set; }
        public string T7Attributes3 { get; set; }
        public string T7Attributes4 { get; set; }
        public string T7Attributes5 { get; set; }
        public string T7Attributes6 { get; set; }
        public string T7Attributes7 { get; set; }
        public string T7Attributes8 { get; set; }
        public string T7PromoName { get; set; }
        public string T7PromoQty { get; set; }
        public double T7PromoPrice { get; set; }
        public DateTime T7PromoDateFrom { get; set; }
        public DateTime T7PromoDateTo { get; set; }
        public decimal T7OriginalRetailPrice { get; set; }
        public string T7RetailPrice { get; set; }
        public decimal T7P1Price { get; set; }
        public decimal T7P2Price { get; set; }
        public decimal T7P3Price { get; set; }
        public decimal T7P4Price { get; set; }
        public decimal T7P5Price { get; set; }
        public bool T7Visible { get; set; }

        public string T8ItemNo { get; set; }
        public string T8ItemName { get; set; }
        public string T8Attributes1 { get; set; }
        public string T8Attributes2 { get; set; }
        public string T8Attributes3 { get; set; }
        public string T8Attributes4 { get; set; }
        public string T8Attributes5 { get; set; }
        public string T8Attributes6 { get; set; }
        public string T8Attributes7 { get; set; }
        public string T8Attributes8 { get; set; }
        public string T8PromoName { get; set; }
        public string T8PromoQty { get; set; }
        public double T8PromoPrice { get; set; }
        public DateTime T8PromoDateFrom { get; set; }
        public DateTime T8PromoDateTo { get; set; }
        public decimal T8OriginalRetailPrice { get; set; }
        public string T8RetailPrice { get; set; }
        public decimal T8P1Price { get; set; }
        public decimal T8P2Price { get; set; }
        public decimal T8P3Price { get; set; }
        public decimal T8P4Price { get; set; }
        public decimal T8P5Price { get; set; }
        public bool T8Visible { get; set; }

        public string T9ItemNo { get; set; }
        public string T9ItemName { get; set; }
        public string T9Attributes1 { get; set; }
        public string T9Attributes2 { get; set; }
        public string T9Attributes3 { get; set; }
        public string T9Attributes4 { get; set; }
        public string T9Attributes5 { get; set; }
        public string T9Attributes6 { get; set; }
        public string T9Attributes7 { get; set; }
        public string T9Attributes8 { get; set; }
        public string T9PromoName { get; set; }
        public string T9PromoQty { get; set; }
        public double T9PromoPrice { get; set; }
        public DateTime T9PromoDateFrom { get; set; }
        public DateTime T9PromoDateTo { get; set; }
        public decimal T9OriginalRetailPrice { get; set; }
        public string T9RetailPrice { get; set; }
        public decimal T9P1Price { get; set; }
        public decimal T9P2Price { get; set; }
        public decimal T9P3Price { get; set; }
        public decimal T9P4Price { get; set; }
        public decimal T9P5Price { get; set; }
        public bool T9Visible { get; set; }
    }

    public class GetProductsItem
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
    }

    public class GetProductsResult
    {
        public List<GetProductsItem> Items { get; set; }
    }

    public class DropdownListItem
    {
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }
}
