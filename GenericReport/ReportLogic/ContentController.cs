using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.IO;
using PowerPOS;

namespace GenericReport
{
    class ContentController
    {
        public static void AddTable(Table table, XmlWriter xmlWriter)
        {
            try
            {
                int nNumberOfColumns = table.dt.Columns.Count;
                double nTableWidth = 0;
                for (int i = 0; i < table.listColumnWidth.Count; i++)
                    nTableWidth += table.listColumnWidth[i];

                // start element: Table
                xmlWriter.WriteStartElement("Table");
                xmlWriter.WriteAttributeString("Name", table.dt.TableName);

                // define table position (top)
                // start element: Top
                xmlWriter.WriteStartElement("Top");
                xmlWriter.WriteValue(table.tablePosition.y.ToString() + "cm");
                xmlWriter.WriteEndElement();
                // end element: Top

                // start element: Left
                xmlWriter.WriteStartElement("Left");
                xmlWriter.WriteValue(table.tablePosition.x.ToString() + "cm");
                xmlWriter.WriteEndElement();
                // end element: Left

                // define table width
                // start element: Width
                xmlWriter.WriteStartElement("Width");
                xmlWriter.WriteValue(nTableWidth.ToString() + "cm");
                xmlWriter.WriteEndElement();
                // end element: Width

                // define ZIndex
                // start element: ZIndex
                xmlWriter.WriteStartElement("ZIndex");
                xmlWriter.WriteValue(1);
                xmlWriter.WriteEndElement();
                // end element: ZIndex

                xmlWriter.WriteStartElement("Style");
                SetBorderColor(xmlWriter, table.tblFormat.BorderColorToString);
                xmlWriter.WriteEndElement();

                // define DataSetName
                // start element: DataSetName
                xmlWriter.WriteStartElement("DataSetName");
                xmlWriter.WriteValue("ds" + table.dt.TableName);
                xmlWriter.WriteEndElement();

                #region add column names
                // start element: Header
                xmlWriter.WriteStartElement("Header");
                // check if we want to repeat table header on each page
                if (table.tblFormat.RepeatHeaderOnNewPage == true)
                    SetRepeatOnNewPage(xmlWriter, true);
                // start element: TableRows
                xmlWriter.WriteStartElement("TableRows");
                // start element: TableRow
                xmlWriter.WriteStartElement("TableRow");
                // define element: Height 
                xmlWriter.WriteElementString("Height", table.tblFormat.TableHeaderRowHeight.ToString() + "cm");
                // start element: TableCells
                xmlWriter.WriteStartElement("TableCells");
                // add table cells
                string strColumnName = "";
                for (int i = 0; i < nNumberOfColumns; i++)
                {
                    if (table.dtHeader != null)
                        strColumnName = table.dtHeader.Rows[0][i].ToString();
                    else
                        strColumnName = table.dt.Columns[i].ColumnName;

                    AddCell(xmlWriter, strColumnName, table.dt.TableName + "hdr", i, table.tblFormat.HeaderCellFormat);
                }
                // end element: TableCells
                xmlWriter.WriteEndElement();
                // end element: TableRow
                xmlWriter.WriteEndElement();
                // end element: TableRows
                xmlWriter.WriteEndElement();
                // end element: Header
                xmlWriter.WriteEndElement();

                #endregion

                #region set column width

                SetColumnWidth(xmlWriter, table.listColumnWidth);

                #endregion

                #region add rows

                // start element: Detail
                xmlWriter.WriteStartElement("Details");
                // start element: TableRows
                DataRow dr = null;
                xmlWriter.WriteStartElement("TableRows");
                if (table.dt.Rows.Count > 0)
                {
                    dr = table.dt.Rows[0];
                    AddRow(xmlWriter, nNumberOfColumns, dr, table.tblFormat.listColumnFormat, table.tblFormat.TableDetailRowHeight, table.tblFormat.DetailCellFormat, "dtl", 0);
                }
                else
                {
                    dr = table.dt.NewRow();
                }
                // end element: TableRows
                xmlWriter.WriteEndElement();
                // end element: Header
                xmlWriter.WriteEndElement();

                #endregion

                // end element: Table
                xmlWriter.WriteEndElement();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
        }

        public static void EmbedImage(string strImgName, string stringType, string strImgPath, XmlWriter xmlWriter)
        {
            try
            {
                xmlWriter.WriteStartElement("EmbeddedImage");
                xmlWriter.WriteAttributeString("Name", strImgName);
                xmlWriter.WriteElementString("MIMEType", "image/jpeg");
                xmlWriter.WriteStartElement("ImageData");
                int bufferSize = 1000;
                byte[] buffer = new byte[bufferSize];
                int readBytes = 0;

                FileStream inputFile = new FileStream(strImgPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(inputFile);

                do
                {
                    readBytes = br.Read(buffer, 0, bufferSize);
                    xmlWriter.WriteBase64(buffer, 0, readBytes);
                } while (bufferSize <= readBytes);
                br.Close();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
        }

        public static void AddImage(Image img, XmlWriter xmlWriter)
        {
            try
            {
                // start element: Image
                xmlWriter.WriteStartElement("Image");
                xmlWriter.WriteAttributeString("Name", img.ImageName);
                xmlWriter.WriteElementString("Source", "Embedded");
                xmlWriter.WriteElementString("MIMEType", img.ImageMIMEType);
                xmlWriter.WriteElementString("Sizing", img.ImageSizing);
                xmlWriter.WriteElementString("Value", img.ImageName);
                xmlWriter.WriteElementString("Top", img.posImg.y.ToString() + "cm");
                xmlWriter.WriteElementString("Left", img.posImg.x.ToString() + "cm");
                xmlWriter.WriteElementString("Height", img.ImageHeight.ToString() + "cm");
                xmlWriter.WriteElementString("Width", img.ImageWidth.ToString() + "cm");

                xmlWriter.WriteEndElement();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
        }

        public static void AddTextbox(Textbox txt, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Textbox");
            xmlWriter.WriteAttributeString("Name", txt.TextName);            
            xmlWriter.WriteElementString("Value", txt.TextValue);
            xmlWriter.WriteElementString("Top", txt.posTxt.y.ToString() + "cm");
            xmlWriter.WriteElementString("Left", txt.posTxt.x.ToString() + "cm");
            xmlWriter.WriteElementString("Height", txt.TextHeight.ToString() + "cm");
            xmlWriter.WriteElementString("Width", txt.TextWidth.ToString() + "cm");
            xmlWriter.WriteStartElement("Style");
                SetBackgroundColor(xmlWriter, txt.cellFormat.BackgroundColorColorToString);
                SetBorderColor(xmlWriter, txt.cellFormat.BorderColorToString);
                SetBorderStyle(xmlWriter, txt.cellFormat.BorderStyle);
                xmlWriter.WriteElementString("PaddingLeft", txt.cellFormat.PaddingLeft.ToString() + "pt");
                xmlWriter.WriteElementString("PaddingRight", txt.cellFormat.PaddingRight.ToString() + "pt");
                xmlWriter.WriteElementString("PaddingTop", txt.cellFormat.PaddingTop.ToString() + "pt");
                xmlWriter.WriteElementString("PaddingBottom", txt.cellFormat.PaddingBottom.ToString() + "pt");
                SetTextAlignment(xmlWriter, txt.cellFormat.TextAlignment);
                SetFontFamily(xmlWriter, txt.cellFormat.FontFamily.Name);
                SetFontSize(xmlWriter, txt.cellFormat.FontSize.ToString());
                SetFontWeight(xmlWriter, txt.cellFormat.FontWeight);
                SetFontStyle(xmlWriter, txt.cellFormat.FontStyle);
                SetColor(xmlWriter, txt.cellFormat.FontColorToString);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
        }

        #region Helper functions
        private static void AddCell(XmlWriter xmlWriter, string cellValue, string CellName, int CellNumber, CellFormat cellFormat, DataRow dr)
        {
            xmlWriter.WriteStartElement("TableCell");

                xmlWriter.WriteStartElement("ReportItems");

                    xmlWriter.WriteStartElement("Textbox");
                    xmlWriter.WriteAttributeString("Name", CellName + CellNumber.ToString());

                        xmlWriter.WriteStartElement("Style");
                            SetBackgroundColor(xmlWriter, cellFormat.BackgroundColorColorToString);
                            SetBorderColor(xmlWriter, cellFormat.BorderColorToString);
                            SetBorderStyle(xmlWriter, cellFormat.BorderStyle);
                            xmlWriter.WriteElementString("PaddingLeft", cellFormat.PaddingLeft.ToString() + "pt");
                            xmlWriter.WriteElementString("PaddingRight", cellFormat.PaddingRight.ToString() + "pt");
                            xmlWriter.WriteElementString("PaddingTop", cellFormat.PaddingTop.ToString() + "pt");
                            xmlWriter.WriteElementString("PaddingBottom", cellFormat.PaddingBottom.ToString() + "pt");
                            SetTextAlignment(xmlWriter, cellFormat.TextAlignment);
                            SetFontFamily(xmlWriter, cellFormat.FontFamily.Name);
                            SetFontSize(xmlWriter, cellFormat.FontSize.ToString());
                            SetFontWeight(xmlWriter, cellFormat.FontWeight);
                            SetFontStyle(xmlWriter, cellFormat.FontStyle);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("CanGrow");
                        xmlWriter.WriteValue(true);
                        xmlWriter.WriteEndElement();                      
                        
                        xmlWriter.WriteElementString("Value", cellValue);                       
                        //xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
        }

        private static void AddCell(XmlWriter xmlWriter, string cellValue, string CellName, int CellNumber, CellFormat cellFormat)
        {
            xmlWriter.WriteStartElement("TableCell");

            xmlWriter.WriteStartElement("ReportItems");

            xmlWriter.WriteStartElement("Textbox");
            xmlWriter.WriteAttributeString("Name", CellName + CellNumber.ToString());

            xmlWriter.WriteStartElement("Style");
                SetBackgroundColor(xmlWriter, cellFormat.BackgroundColorColorToString);
                SetBorderColor(xmlWriter, cellFormat.BorderColorToString);
                SetBorderStyle(xmlWriter, cellFormat.BorderStyle);
                xmlWriter.WriteElementString("PaddingLeft", cellFormat.PaddingLeft.ToString() + "pt");
                xmlWriter.WriteElementString("PaddingRight", cellFormat.PaddingRight.ToString() + "pt");
                xmlWriter.WriteElementString("PaddingTop", cellFormat.PaddingTop.ToString() + "pt");
                xmlWriter.WriteElementString("PaddingBottom", cellFormat.PaddingBottom.ToString() + "pt");
                SetTextAlignment(xmlWriter, cellFormat.TextAlignment);
                SetFontFamily(xmlWriter, cellFormat.FontFamily.Name);
                SetFontSize(xmlWriter, cellFormat.FontSize.ToString());
                SetFontWeight(xmlWriter, cellFormat.FontWeight);
                SetFontStyle(xmlWriter, cellFormat.FontStyle);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CanGrow");
            xmlWriter.WriteValue(true);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteElementString("Value", cellValue);
            //xmlWriter.WriteStartElement("Value");                        
            //xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
        }

        private static void AddRow(XmlWriter xmlWriter, int nNumberOfColumns, DataRow dr, List<ColumnFormat> columnFormat, double tblRowHeight, CellFormat cellFormat, string strSection, int nRowNumber)
        {
            // start element: TableRow
            xmlWriter.WriteStartElement("TableRow");
                // define element: Height 
            xmlWriter.WriteElementString("Height", tblRowHeight.ToString() + "cm");
                // start element: TableCells
                xmlWriter.WriteStartElement("TableCells");
                    // add table cells
                    string strCellValue = "";
                    string strColumnName = "";
                    ColumnFormat format = null;
                    for (int i = 0; i < nNumberOfColumns; i++)
                    {
                        strColumnName = dr.Table.Columns[i].ColumnName;
                        for (int j = 0; j < columnFormat.Count; j++)
                        {
                            if (columnFormat[j].columnName == strColumnName)
                            {
                                format = columnFormat[j];
                                break;
                            }
                        }
                        
                        if (format != null)                        
                            cellFormat = format.cellFormat;

                        strCellValue = "=Fields!" + dr.Table.TableName + dr.Table.Columns[i].ColumnName + ".Value"; //dr[i].ToString();//.
                        AddCell(xmlWriter, strCellValue, dr.Table.TableName + strSection, nNumberOfColumns * nRowNumber + i, cellFormat, dr);
                        format = null;
                    }
                // end element: TableCells
                xmlWriter.WriteEndElement();
            // end element: TableRow
            xmlWriter.WriteEndElement();
        }

        private static void SetColumnWidth(XmlWriter xmlWriter, List<double> listColumnWidth)
        {
            xmlWriter.WriteStartElement("TableColumns");

            for (int i = 0; i < listColumnWidth.Count; i++)
            {
                xmlWriter.WriteStartElement("TableColumn");
                xmlWriter.WriteElementString("Width", listColumnWidth[i].ToString() + "cm");
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

        private static void SetBackgroundColor(XmlWriter xmlWriter, string color)
        {
            xmlWriter.WriteElementString("BackgroundColor", color);
        }

        private static void SetBorderColor(XmlWriter xmlWriter, string color)
        {
            xmlWriter.WriteStartElement("BorderColor");
                xmlWriter.WriteElementString("Left", color);
                xmlWriter.WriteElementString("Right", color);
                xmlWriter.WriteElementString("Top", color);
                xmlWriter.WriteElementString("Bottom", color);
            xmlWriter.WriteEndElement();
        }

        private static void SetBorderStyle(XmlWriter xmlWriter, string style)
        {
            xmlWriter.WriteStartElement("BorderStyle");
            xmlWriter.WriteElementString("Left", style);
            xmlWriter.WriteElementString("Right", style);
            xmlWriter.WriteElementString("Top", style);
            xmlWriter.WriteElementString("Bottom", style);
            xmlWriter.WriteEndElement();
        }

        private static void SetTextAlignment(XmlWriter xmlWriter, string alignment)
        {
            xmlWriter.WriteElementString("TextAlign", alignment);
        }

        private static void SetFontFamily(XmlWriter xmlWriter, string fontFamily)
        {
            xmlWriter.WriteElementString("FontFamily", fontFamily);
        }

        private static void SetFontSize(XmlWriter xmlWriter, string fontSize)
        {
            xmlWriter.WriteElementString("FontSize", fontSize + "pt");
        }        

        private static void SetFontStyle(XmlWriter xmlWriter, string fontStyle)
        {
            xmlWriter.WriteElementString("FontStyle", fontStyle);
        }

        private static void SetFontWeight(XmlWriter xmlWriter, string fontWeight)
        {
            xmlWriter.WriteElementString("FontWeight", fontWeight);
        }

        private static void SetRepeatOnNewPage(XmlWriter xmlWriter, bool choice)
        {
            xmlWriter.WriteStartElement("RepeatOnNewPage");
            xmlWriter.WriteValue(choice);
            xmlWriter.WriteEndElement();
        }

        private static void SetColor(XmlWriter xmlWriter, string color)
        {
            xmlWriter.WriteElementString("Color", color);
        }
        #endregion
    }
}
