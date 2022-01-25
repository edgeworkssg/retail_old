using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Reporting.WinForms;
using PowerPOS;

namespace GenericReport
{
    public class PageTemplate
    {
        #region fields
        public CellFormat cellFormat;
        public PageFormat pageFormat;
        public string pageName;

        public List<Table> listPageBodyTables;
        public List<Image> listPageImages;
        public List<Textbox> listPageTextboxes;

        protected Textbox txtVoided;
        protected Textbox txtReprint;

        protected bool bReprint;

        Content pageContent;
        public Page page;

        #endregion

        // constructors
        public PageTemplate(PageFormat pageFormat)
        {
            this.pageFormat = pageFormat;

            listPageBodyTables = new List<Table>();
            listPageImages = new List<Image>();
            listPageTextboxes = new List<Textbox>();
            pageContent = new Content();
            pageName = "";

            txtVoided = new Textbox();
            txtReprint = new Textbox();

            txtVoided.TextValue = "VOIDED";
            txtVoided.TextHeight = 1.0;
            txtVoided.TextWidth = 3.5;
            txtVoided.TextName = "txtVoided";
            txtVoided.cellFormat.FontColor = System.Drawing.Color.Red;
            txtVoided.cellFormat.FontWeight = "Bold";
            txtVoided.cellFormat.FontSize = 14;
            txtVoided.cellFormat.BorderStyle = BorderStyle.Double;
            txtVoided.cellFormat.TextAlignment = "Center";
            txtVoided.posTxt.x = 6.5;
            txtVoided.posTxt.y = 1.5;

            txtReprint.TextValue = "RE-PRINT";
            txtReprint.TextHeight = 1.0;
            txtReprint.TextWidth = 3.5;
            txtReprint.TextName = "txtReprint";
            txtReprint.cellFormat.FontColor = System.Drawing.Color.Red;
            txtReprint.cellFormat.FontWeight = "Bold";
            txtReprint.cellFormat.FontSize = 14;
            txtReprint.cellFormat.BorderStyle = BorderStyle.None;
            txtVoided.cellFormat.TextAlignment = "Center";
            txtReprint.posTxt.x = 6.5;
            txtReprint.posTxt.y = 0;
        }
        public PageTemplate()
        {
            listPageBodyTables = new List<Table>();
            listPageImages = new List<Image>();
            listPageTextboxes = new List<Textbox>();
            this.pageFormat = new PageFormat();
            pageContent = new Content();
            pageName = "";

            txtVoided = new Textbox();
            txtReprint = new Textbox();

            txtVoided.TextValue = "VOIDED";
            txtVoided.TextHeight = 1.0;
            txtVoided.TextWidth = 3.5;
            txtVoided.TextName = "txtVoided";
            txtVoided.cellFormat.FontColor = System.Drawing.Color.Red;
            txtVoided.cellFormat.FontWeight = "Bold";
            txtVoided.cellFormat.FontSize = 14;
            txtVoided.cellFormat.BorderStyle = BorderStyle.None;
            txtVoided.cellFormat.TextAlignment = "Center";
            txtVoided.posTxt.x = 6.5;
            txtVoided.posTxt.y = 1.5;

            txtReprint.TextValue = "RE-PRINT";
            txtReprint.TextHeight = 1.0;
            txtReprint.TextWidth = 3.5;
            txtReprint.TextName = "txtReprint";
            txtReprint.cellFormat.FontColor = System.Drawing.Color.Red;
            txtReprint.cellFormat.FontWeight = "Bold";
            txtReprint.cellFormat.FontSize = 14;
            txtReprint.cellFormat.BorderStyle = BorderStyle.None;
            txtReprint.cellFormat.TextAlignment = "Center";
            txtReprint.posTxt.x = 6.5;
            txtReprint.posTxt.y = 0;
        }

        #region virtual functions
        public virtual void CreateReport(POSController posController, bool reprint, string reportType) { }

        #endregion

        #region public functions
        public void AddTable(Table table)
        {
            listPageBodyTables.Add(table);
        }

        public void ShowReport(ReportViewer reportViewer)
        {
            List<DataSet> listDS = page.GetAllDataSets();
            for (int i = 0; i < listDS.Count; i++)
                reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource(listDS[i].DataSetName, listDS[i].Tables[0]));
            
            reportViewer.LocalReport.LoadReportDefinition(ReturnPageDefinition());
            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer.LocalReport.EnableExternalImages = true;            
            reportViewer.ZoomMode = ZoomMode.Percent;
            reportViewer.ZoomPercent = 100;
            
            

            string strReportPath = System.IO.Directory.GetCurrentDirectory();
            strReportPath = strReportPath.Replace("\\bin\\Debug", "");
            strReportPath += "\\report.rdlc";
            //reportViewer.LocalReport.ReportPath = strReportPath;
            //reportViewer.RefreshReport();
        }
        
        public void InitializePageCreation()
        {
            AddImagesToContent();
            AddTablesToContent();
            AddTextboxesToContent();
            page = new Page(pageContent, pageFormat);               
        }
        public void CreatePage()
        {
            page.CreateNewPage();
        }
        public System.IO.Stream ReturnPageDefinition()
        {
            return page.ReturnPage();
        }

        #endregion

        #region private helper functions

        private void AddTablesToContent()
        {
            for (int i = 0; i < listPageBodyTables.Count; i++)
                pageContent.listTables.Add(listPageBodyTables[i]);
        }

        private void AddImagesToContent()
        {
            for (int i = 0; i < listPageImages.Count; i++)
                pageContent.listImages.Add(listPageImages[i]);
        }

        private void AddTextboxesToContent()
        {
            for (int i = 0; i < listPageTextboxes.Count; i++)
                pageContent.listTextboxes.Add(listPageTextboxes[i]);
        }

        #endregion
        
    }
}
