using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace GenericReport
{
    public class Page
    {
        #region fields

        // used to write xml
        public XmlWriter xmlWriter;
        private string xml;
        private StringBuilder sb;
        private string nsrd = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";
        private string ns = "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition";

        // page properties
        public PageFormat pageFormat;
        private Content pageContent;
        private List<Image> listPageImages;
        private List<DataSet> listDS;

        #endregion

        #region constructor

        public Page(Content content, PageFormat format)
        {
            sb = new StringBuilder();
            listDS = new List<DataSet>();
            pageContent = new Content();
            listPageImages = new List<Image>();
            DataSet ds = null;
            this.pageFormat = format;
            for (int i = 0; i < content.listTables.Count; i++)
            {
                if (content.listTables[i].dt == null)
                    continue;
                ds = new DataSet("ds" + content.listTables[i].dt.TableName);
                ds.DataSetName = "ds" + content.listTables[i].dt.TableName;
                ds.Tables.Add(content.listTables[i].dt);
                listDS.Add(ds);
                pageContent.listTables.Add(content.listTables[i]);
            }

            for (int i = 0; i < content.listImages.Count; i++)
            {
                if (content.listImages[i] == null)
                    continue;
                pageContent.listImages.Add(content.listImages[i]);
            }

            for (int i = 0; i < content.listTextboxes.Count; i++)
            {
                if (content.listTextboxes[i] == null)
                    continue;
                pageContent.listTextboxes.Add(content.listTextboxes[i]);
            }
        }

        #endregion

        public List<DataSet> GetAllDataSets()
        {
            return listDS;
        }

        public void CreateNewPage()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = true;
            settings.Indent = true;

            xmlWriter = XmlWriter.Create(sb, settings);
            string strReportPath = System.IO.Directory.GetCurrentDirectory();
            strReportPath = strReportPath.Replace("\\bin\\Debug", "");
            strReportPath += "\\report.rdlc";
            //xmlWriter = XmlWriter.Create(strReportPath, settings);       

            xmlWriter.WriteStartDocument();
            // start element: Report
            xmlWriter.WriteStartElement("Report", ns);
            xmlWriter.WriteAttributeString("xmlns","rd","", nsrd);            

            xmlWriter.WriteElementString("LeftMargin", pageFormat.LeftMargin.ToString() + "cm");
            xmlWriter.WriteElementString("RightMargin", pageFormat.RightMargin.ToString() + "cm");
            xmlWriter.WriteElementString("TopMargin", pageFormat.TopMargin.ToString() + "cm");
            xmlWriter.WriteElementString("BottomMargin", pageFormat.BottomMargin.ToString() + "cm");
            xmlWriter.WriteElementString("PageWidth", pageFormat.PageWidth.ToString() + "cm");
            xmlWriter.WriteElementString("PageHeight", pageFormat.PageHeight.ToString() + "cm");
            xmlWriter.WriteElementString("Width", pageFormat.BodyWidth.ToString() + "cm");

            AddDataSource(xmlWriter, listDS);

            AddDataSet(xmlWriter, listDS);

            if (pageContent.listImages.Count != 0)
            {
                xmlWriter.WriteStartElement("EmbeddedImages");
                Image img;
                for (int i = 0; i < pageContent.listImages.Count; i++)
                {
                    img = pageContent.listImages[i];
                    ContentController.EmbedImage(img.ImageName, img.ImageMIMEType.ToString(), img.ImagePath, xmlWriter);
                }
                xmlWriter.WriteEndElement();
            }

            // start element: Body
            xmlWriter.WriteStartElement("Body");

            //xmlWriter.WriteElementString("Width", pageFormat.BodyWidth + "cm");
            xmlWriter.WriteElementString("Height", pageFormat.BodyHeight.ToString() + "cm");


            // start element: ReportItems
            xmlWriter.WriteStartElement("ReportItems");
            
            for (int i = 0; i < listPageImages.Count; i++)
                ContentController.AddImage(pageContent.listImages[i], xmlWriter);

            for (int i = 0; i < pageContent.listTables.Count; i++)
            {
                ContentController.AddTable(pageContent.listTables[i], xmlWriter);
            }

            for (int i = 0; i < pageContent.listTextboxes.Count; i++)
            {
                ContentController.AddTextbox(pageContent.listTextboxes[i], xmlWriter);
            }

        }

        public Stream ReturnPage()
        {
                    // end element: ReportItems
                    xmlWriter.WriteEndElement();

                // end element: Body
                xmlWriter.WriteEndElement();

            /* Page Footer Section
             
                xmlWriter.WriteStartElement("PageFooter");
                    xmlWriter.WriteStartElement("PrintOnLastPage");                    
                    xmlWriter.WriteValue(true);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("Height", "4cm");
                    // start element: ReportItems
                    xmlWriter.WriteStartElement("ReportItems");
                    // start element: ReportItems
                    xmlWriter.WriteStartElement("Rectangle");
                    xmlWriter.WriteAttributeString("Name", "rectFooter");
                    xmlWriter.WriteStartElement("ReportItems");
                    xmlWriter.WriteStartElement("Textbox");
                    xmlWriter.WriteAttributeString("Name", "txtBoxFooter");
                        xmlWriter.WriteElementString("Height", "3cm");
                        xmlWriter.WriteElementString("Width", "3cm");
                        xmlWriter.WriteElementString("Value", "ReportItems!tblOrderDetails.Height");
                    xmlWriter.WriteEndElement();
                    
                    xmlWriter.WriteEndElement();
                    // end element: Rectangle
                    xmlWriter.WriteEndElement();
                    // end element: ReportItems
                    xmlWriter.WriteEndElement();
                // end element: PageFooter
                xmlWriter.WriteEndElement();
            */

            // end element: Report
            xmlWriter.WriteEndElement();

            // flush XmlWriter object
            xmlWriter.Flush();
            xmlWriter.Close();
            xml = sb.ToString().Replace("utf-16", "utf-8");
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            return stream;
        }

        public static void AddDataSource(XmlWriter xmlWriter, List<DataSet> listDS)
        {
            xmlWriter.WriteStartElement("DataSources");
            {
                for (int i = 0; i < listDS.Count; i++)
                {
                    xmlWriter.WriteStartElement("DataSource");
                    {
                        xmlWriter.WriteAttributeString("Name", listDS[i].DataSetName);
                        xmlWriter.WriteElementString("DataSourceReference", listDS[i].DataSetName);
                    }
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
        }

        public static void AddDataSet(XmlWriter xmlWriter, List<DataSet> listDS)
        {
            xmlWriter.WriteStartElement("DataSets");
            {
                for (int i = 0; i < listDS.Count; i++)
                {
                    xmlWriter.WriteStartElement("DataSet");
                    xmlWriter.WriteAttributeString("Name", listDS[i].DataSetName);
                    {
                        xmlWriter.WriteStartElement("Fields");
                        {
                            DataTable dt = listDS[i].Tables[0];
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {                                
                                xmlWriter.WriteStartElement("Field");
                                xmlWriter.WriteAttributeString("Name", dt.TableName + dt.Columns[j].ColumnName);
                                {
                                    xmlWriter.WriteElementString("DataField", dt.Columns[j].ColumnName);                                   
                                }
                                xmlWriter.WriteEndElement();
                                
                            }
                        }
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("Query");
                        {
                            xmlWriter.WriteElementString("DataSourceName", listDS[i].DataSetName);
                            xmlWriter.WriteElementString("CommandText", "");
                            //xmlWriter.WriteElementString("rd", "DataSourceName", "", "true");
                        }
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
        }

        public void AddImageToPage(Image img)
        {
            string strImgName = img.ImageName;
            bool bImageAlreadyEmbedded = false;
            // check if the image that the user wants to add has been embedded to the report yet or not
            for (int i = 0; i < pageContent.listImages.Count; i++)
            {
                if (pageContent.listImages[i].ImageName == strImgName)
                {
                    bImageAlreadyEmbedded = true;
                    break;
                }
            }

            if (bImageAlreadyEmbedded == true)
                listPageImages.Add(img);

            else
                MessageBox.Show("Image has yet to be embedded to the report. Please add image for embedding first.", "ERROR");
        }
    }

}
