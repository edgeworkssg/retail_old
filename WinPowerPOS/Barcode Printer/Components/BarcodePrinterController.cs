using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Drawing.Printing;
using PowerPOS;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Serialization;
using BarcodePrinter;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
//using DocumentFormat.OpenXml.Spreadsheet;


namespace WinPowerPOS.BarcodePrinter
{
    public class BarcodePrinterController
    {
        private PrintedTemplate Template;

        private string SaveLocation = "PrintTemplate";
        private string SavePrefix = "pnt-";

        private Font DefaultFont = new Font("Arial Narrow", 8);

        public void SaveTemplate()
        {
            string FileLocation = SaveLocation + "\\" + SavePrefix + Template.Name + ".bin";

            if (File.Exists(FileLocation))
            {
                File.Delete(FileLocation);
            }
            Stream a = File.OpenWrite(FileLocation);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(a, Template);
            a.Close();
        }

        public void LoadTemplate(string TemplateName)
        {
            string FileLocation = SaveLocation + "\\" + SavePrefix + TemplateName + ".bin";

            if (File.Exists(FileLocation))
            {
                try
                {
                    FileInfo inf = new FileInfo(FileLocation);

                    FileStream file = new FileStream(FileLocation, FileMode.Open);

                    BinaryFormatter bf = new BinaryFormatter();
                    //bf.Binder = new Version1ToVersion2DeserializationBinder();
                    Template = bf.Deserialize(file) as PrintedTemplate;

                    file.Close();
                }
                catch (Exception e)
                {
                    Logger.writeLog("Failed deserialization" + e.Message);
                }
            }
            else
            {
                Template = new PrintedTemplate(TemplateName);
            }
        }

        public void LoadTemplate(Uri TemplateFile)
        {
            string FileLocation = TemplateFile.OriginalString;//.AbsolutePath;// SaveLocation + "\\" + SavePrefix + TemplateName + ".bin";

            if (File.Exists(FileLocation))
            {
                FileInfo inf = new FileInfo(FileLocation);

                FileStream file = new FileStream(FileLocation, FileMode.Open);

                BinaryFormatter bf = new BinaryFormatter();
                //bf.Binder = new Version1ToVersion2DeserializationBinder();
                Template = bf.Deserialize(file) as PrintedTemplate;

                file.Close();
            }
            else
            {
                Template = new PrintedTemplate("Default");
            }
        }

        public BarcodePrinterController(string TemplateName)
        {
            LoadTemplate(TemplateName);
        }
        public BarcodePrinterController(Uri TemplateFile)
        {
            LoadTemplate(TemplateFile);
        }

        public string TemplateName
        {
            get { return Template.Name; }
            set { Template.Name = value; }
        }

        public string[] GetAllComponent()
        {
            List<string> Output = new List<string>();
            for (int Counter = 0; Counter < Template.Values.Count; Counter++)
                Output.Add(Template.Values[Counter].Name);

            return Output.ToArray();
        }

        private void AddComponent(PrintedComponent Value)
        {
            Template.AddComponent(Value);
            SaveTemplate();
        }

        public void AddBarcode(string Name, int Left, int Top, int Width, int Height, System.Drawing.StringAlignment HAllign)
        {
            AddComponent(new BarcodeComponent(Name, new System.Drawing.Rectangle(Left, Top, Width, Height), HAllign));
        }
        public void AddText(string Name, int Left, int Top, int Width, int Height, System.Drawing.StringAlignment HAllign, Font PrintedFont)
        {
            AddComponent(new TextComponent(Name, new System.Drawing.Rectangle(Left, Top, Width, Height), PrintedFont, HAllign));
        }
        public void AddCurrency(string Name, int Left, int Top, int Width, int Height, System.Drawing.StringAlignment HAllign, Font PrintedFont)
        {
            AddComponent(new CurrencyComponent(Name, new System.Drawing.Rectangle(Left, Top, Width, Height), PrintedFont, HAllign));
        }
        public void AddStaticText(string Text, int Left, int Top, int Width, int Height, System.Drawing.StringAlignment HAllign, Font PrintedFont)
        {
            AddComponent(new StaticComponent(Text, new System.Drawing.Rectangle(Left, Top, Width, Height), PrintedFont, HAllign));
        }

        public void DeleteComponent(string Name)
        {
            Template.DeleteComponent(Name);
            SaveTemplate();
        }

        public PrintedComponent LoadComponentInformation(string ComponentName)
        {
            return Template.LoadComponentInformation(ComponentName);
        }


        private string PrinterName = AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.PrinterName);
        private int PageWidth = 250;
        private int PageHeight = 40;
        private int Padding = 3;

        private Font printFont = new Font("Verdana", 12);
        private Font FontBarcode = new Font("Code 128", 36);


        public void PrintBarcodeLabel(List<DataRow> drow, string SortBy, SortOrder SortingOrder)
        {
            try
            {
                //if (dt == null || !dt.IsInitialized) return;
                if (drow == null || !(drow.Count > 0)) return;

                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }
                printDoc.PrintPage += new PrintPageEventHandler(HandlerPrintBarcodeLabel);
                printDoc.DocumentName = "Barcode Printer";
                var pageSetting = new System.Drawing.Printing.PageSettings();
                pageSetting.Margins = new Margins(0, 0, 0, 0);
                PrinterResolution pr = new PrinterResolution()
                {
                    Kind = PrinterResolutionKind.High
                };
                pageSetting.PrinterResolution = pr;
                PaperSource ps = new PaperSource()
                {
                    RawKind = 256
                };
                pageSetting.PaperSource = ps;
                pageSetting.Landscape = false;
                printDoc.DefaultPageSettings = pageSetting;

                //dt.DefaultView.Sort = SortBy + " " + SortOrder.ToString().Substring(0,3);
                //dt.TableName = "List";
                //DataView dv = new DataView();
                //dv.Table = dt;
                //dv.AllowDelete = true;
                //dv.AllowEdit = true;
                //dv.AllowNew = true;
                //if (dt.Columns.Contains("Qty"))
                //    dv.RowFilter = "Qty > 0";
                //else if (dt.Columns.Contains("Quantity"))
                //    dv.RowFilter = "Quantity > 0";
                //dv.RowStateFilter = DataViewRowState.ModifiedCurrent;
                //dv.Sort = SortBy + " " + (SortingOrder == SortOrder.Descending ? "DESC" : "ASC");

                foreach (DataRow item in drow)
                {
                    int Copies = 0;
                    Copies = int.Parse(item["Qty"].ToString());

                    Template.SetData(item);
                    for (/*NOTHING*/; Copies > 0; Copies--)
                    {
                        printDoc.Print();
                    }
                }

                //for (int Counter = 0; Counter < dv.Count; Counter++)
                //{
                //    int Copies = 0;
                //    if (dt.Columns.Contains("Qty"))
                //        Copies = int.Parse(dv[Counter]["Qty"].ToString());
                //    else if (dt.Columns.Contains("Quantity"))
                //        Copies = int.Parse(dv[Counter]["Quantity"].ToString());
                //    else
                //        Copies = 1;

                //    Template.SetData(dv[Counter].Row);

                //    for (/*NOTHING*/; Copies > 0; Copies--)
                //    {
                //        printDoc.Print();
                //    }
                //}
            }
            catch (Exception E)
            {
                return;
            }
        }

        private void HandlerPrintBarcodeLabel(object sender, PrintPageEventArgs e)
        {
            try
            {
                int PageHeight = e.MarginBounds.Height;
                int y = Padding;

                int RowHeight = (int)printFont.GetHeight();

                //Template.Values.Clear();
                //Template.Values.Add(new TextComponent("Item Name", new Rectangle(e.PageBounds.X, y, e.PageBounds.Width, RowHeight), printFont, StringAlignment.Near));
                //Template.Values.Add(new BarcodeComponent("Barcode", new Rectangle(e.PageBounds.X, RowHeight, e.PageBounds.Width, 40), StringAlignment.Center));
                //SaveTemplate("PrintBarcode");

                foreach (PrintedComponent onePrint in Template.Values)
                {
                    if ((onePrint.Name == "Barcode" || onePrint.Name == "ItemNo") && onePrint.GetType().Name == "TextComponent")
                    {
                        //((TextComponent)onePrint).Value = "*" + ((TextComponent)onePrint).Value + "*";
                        if (((TextComponent)onePrint).PrintedFont.FontFamily.Name.Contains("Free") && !((TextComponent)onePrint).Value.StartsWith("*"))
                            ((TextComponent)onePrint).Value = "*" + ((TextComponent)onePrint).Value + "*";
                    }
                    onePrint.Print(ref e);
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        public void HandlerPaintBarcodeLabel(Object sender, PaintEventArgs e)
        {
            try
            {
                //Template.Values.Clear();
                //Template.Values.Add(new TextComponent("Item Name", new Rectangle(e.PageBounds.X, y, e.PageBounds.Width, RowHeight), printFont, StringAlignment.Near));
                //Template.Values.Add(new BarcodeComponent("Barcode", new Rectangle(e.PageBounds.X, RowHeight, e.PageBounds.Width, 40), StringAlignment.Center));
                //SaveTemplate("PrintBarcode");

                //LoadTemplate("PrintBarcode");

                //Template.Values[0].SetValue(Template.Values[0].Name);
                //Template.Values[1].SetValue(Template.Values[1].Name);

                e.Graphics.FillRectangle(Brushes.Khaki, e.ClipRectangle);

                foreach (PrintedComponent onePrint in Template.Values)
                {
                    onePrint.Paint(ref e);
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        public static bool ExportSelectedItems(List<DataRow> drow, string fileName, out string message)
        {
            message = "";
            try
            {
                if (drow == null || !(drow.Count > 0))
                    throw new Exception("No Data to print");

                SLDocument sl = new SLDocument();

                DataTable dt = new DataTable();
                dt.Columns.Add("ItemName", typeof(string));
                dt.Columns.Add("ItemNo", typeof(string));
                dt.Columns.Add("Barcode", typeof(string));
                dt.Columns.Add("CategoryName", typeof(string));
                dt.Columns.Add("RetailPrice", typeof(decimal));
                dt.Columns.Add("Attributes1", typeof(string));
                dt.Columns.Add("Attributes2", typeof(string));
                dt.Columns.Add("Attributes3", typeof(string));
                dt.Columns.Add("Attributes4", typeof(string));
                dt.Columns.Add("Attributes5", typeof(string));
                dt.Columns.Add("Attributes6", typeof(string));
                dt.Columns.Add("Attributes7", typeof(string));


                for(int i = 0; i < drow.Count; i++)
                {
                    DataRow item = drow[i];
                    int Copies = int.Parse(item["Qty"].ToString());

                    for (int j =0; j < Copies; j++)
                    {
                        DataRow add = dt.NewRow();
                        add["ItemName"] = drow[i]["ItemName"].ToString();
                        add["ItemNo"] = drow[i]["ItemNo"].ToString();
                        add["Barcode"] = drow[i]["Barcode"].ToString();
                        add["CategoryName"] = drow[i]["CategoryName"].ToString();
                        add["RetailPrice"] = drow[i]["RetailPrice"].ToString().GetDecimalValue();
                        add["Attributes1"] = drow[i]["Attributes1"].ToString();
                        add["Attributes2"] = drow[i]["Attributes2"].ToString();
                        add["Attributes3"] = drow[i]["Attributes3"].ToString();
                        add["Attributes4"] = drow[i]["Attributes4"].ToString();
                        add["Attributes5"] = drow[i]["Attributes5"].ToString();
                        add["Attributes6"] = drow[i]["Attributes6"].ToString();
                        add["Attributes7"] = drow[i]["Attributes7"].ToString();

                        dt.Rows.Add(add);
                    }
                }


                int iStartRowIndex = 1;
                int iStartColumnIndex = 1;
                sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);
                int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
                int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
                SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
                table.SetTableStyle(SLTableStyleTypeValues.Medium2);
                table.HasTotalRow = false;
                sl.InsertTable(table);

                sl.AutoFitColumn(1, iEndColumnIndex);

                sl.SaveAs(fileName);

                return true;
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
                return false;
            }
        }
    }
}


sealed class Version1ToVersion2DeserializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Type typeToDeserialize = null;

        // For each assemblyName/typeName that you want to deserialize to
        // a different type, set typeToDeserialize to the desired type.
        //String assemVer1 = Assembly.GetExecutingAssembly().FullName;
        String assemVer1 = "BarcodePrinter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        String typeVer1 = "BarcodePrinter.PrintedTemplate";

        if (assemblyName == assemVer1 && typeName == typeVer1)
        {
            // To use a type from a different assembly version, 
            // change the version number.
            // To do this, uncomment the following line of code.
            //assemblyName = assemblyName.Replace("1.0.0.0", "3.6.7.0");
            assemblyName = Assembly.GetExecutingAssembly().FullName;

            // To use a different type from the same assembly, 
            // change the type name.
            typeName = "PowerPOS.PrintedTemplate";
        }

        // The following line of code returns the type.
        typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
            typeName, assemblyName));

        return typeToDeserialize;
    }
}