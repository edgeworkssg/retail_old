using SubSonic;
using CrystalDecisions.Shared;
using System.Collections.Generic;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using PowerPOS;
using System.Drawing;
using System.Drawing.Imaging;

namespace PowerReport
{
    public partial class CRReport 
    {
        public bool IsReportLoaded;

        private string ReportName;
        private ReportDocument TheDocument;

        public CRReport(string NewReportName)
        {
            LoadReport(NewReportName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Data.SqlTypes.SqlNullValueException">Occur if no data is loaded</exception>
        public void LoadReport(string NewReportName)
        {
            try
            {
                ReportName = NewReportName;
                
                if (ReportName != null && ReportName.ToLower().EndsWith(".rpt") && File.Exists(ReportName))
                {
                    TheDocument = new ReportDocument();
                    TheDocument.Load(ReportName);
                    IsReportLoaded = true;
                }
                else
                {
                    TheDocument = new ReportDocument();
                    IsReportLoaded = false;
                }
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                TheDocument = new ReportDocument();
                IsReportLoaded = false;
            }
        }

        public string ReportTitle
        {
            get
            {
                return TheDocument.SummaryInfo.ReportTitle;
            }
        }
        public string SQLString
        {
            get
            {
                if (!IsReportLoaded) return "";

                int Index;
                for (Index = 0; Index < TheDocument.DataDefinition.FormulaFields.Count; Index++)
                    if (TheDocument.DataDefinition.FormulaFields[Index].Name.ToLower() == "sqlstring")
                        return TheDocument.DataDefinition.FormulaFields[Index].Text.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                return "";
            }
        }

        public SortedList<string, ParameterFieldDefinition> GetFilters()
        {
            if (!IsReportLoaded) return new SortedList<string, ParameterFieldDefinition>();

            SortedList<string, ParameterFieldDefinition> Rst = new SortedList<string, ParameterFieldDefinition>();

            for (int Counter = 0; Counter < TheDocument.DataDefinition.ParameterFields.Count; Counter++)
            {
                //if (TheDocument.DataDefinition.ParameterFields[Counter].Name.ToLower() == "") continue;
                ParameterFieldDefinition ParamType = TheDocument.DataDefinition.ParameterFields[Counter];
                string ParamName = TheDocument.DataDefinition.ParameterFields[Counter].Name;
                if (!Rst.ContainsKey(ParamName))
                {
                    Rst.Add(ParamName, ParamType);
                }
            }

            return Rst;
        }

        public string GetSQLString(string SubReportName)
        {
            if (!IsReportLoaded) return "";

            if (SubReportName == "MAIN") return SQLString;


            for (int Counter = 0; Counter < TheDocument.Subreports.Count; Counter++)
            {
                if (TheDocument.Subreports[Counter].Name != SubReportName) continue;

                for (int Index = 0; Index < TheDocument.Subreports[Counter].DataDefinition.FormulaFields.Count; Index++)
                    if (TheDocument.Subreports[Counter].DataDefinition.FormulaFields[Index].Name.ToLower() == "sqlstring")
                        return TheDocument.Subreports[Counter].DataDefinition.FormulaFields[Index].Text.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            }

            return "";
        }

        public DataTable GetData(SortedList<string, string> Parameters)
        {
            string SQL = SQLString;
            string ShowBarcode = "";

            if (SQL == "") return new DataTable();

            SortedList<string, ParameterFieldDefinition> theFilters = GetFilters();

            foreach (string Key in theFilters.Keys)
            {
                string Val;
                if (Parameters.Keys.Contains(Key))
                    Val = Parameters[Key];
                else
                    Val = ((ParameterDiscreteValue)theFilters[Key].DefaultValues[0]).Value.ToString();

                if (Parameters.Keys.Contains("ShowBarcode"))
                    ShowBarcode = Parameters[Key];

                if (theFilters[Key].ValueType == FieldValueType.DateField)
                    SQL = SQL.Replace("{?" + Key + "}", "'" + DateTime.Parse(Val).ToString("yyyy-MM-dd") + "'");
                else if (theFilters[Key].ValueType == FieldValueType.DateTimeField)
                {
                    SQL = SQL.Replace("{?" + Key + "}", "'" + DateTime.Parse(Val).ToString("yyyy-MM-dd HH:mm:ss") + "'");
                }
                else if (theFilters[Key].ValueType == FieldValueType.StringField)
                {
                    if (Val.IndexOf("'") > 0)
                    {
                        Val = Val.Insert(Val.IndexOf("'"), "'");
                    }
                    SQL = SQL.Replace("{?" + Key + "}", "'" + Val + "'");
                }
                else if (theFilters[Key].ValueType == FieldValueType.NumberField)
                    SQL = SQL.Replace("{?" + Key + "}", Val);
            }


            Logger.writeLog(SQL);

            DataTable DT = new DataTable();
            QueryCommand cmd = new QueryCommand(SQL);
            cmd.CommandTimeout = 60000;
            DT.Load(SubSonic.DataService.GetReader(cmd));

            if (!string.IsNullOrEmpty(ShowBarcode) && (ShowBarcode.ToUpper() == "YES" || ShowBarcode.ToUpper() == "TRUE"))
            {
                DT.Columns.Add("BarcodeImage", typeof(byte[]));

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string barcode = DT.Rows[i]["Barcode"] + "";
                    var img = CreateBarcode(string.Format("*{0}*", barcode));
                    //var img = CreateBarcode(barcode);
                    DT.Rows[i]["BarcodeImage"] = img;
                }
            }

            return DT;
        }

        private static byte[] CreateBarcode(string code)
        {
            byte[] result = null;

            //int width = 240;
            //int height = 40;

            int width = 360;
            int height = 50;

            var myBitmap = new Bitmap(width, height);
            var g = Graphics.FromImage(myBitmap);
            var jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

            g.Clear(Color.White);

            var strFormat = new StringFormat { Alignment = StringAlignment.Center };
            g.DrawString(code, new Font("Free 3 of 9 Extended", 40), Brushes.Black, new RectangleF(0, 2, width, height - 4), strFormat);
                       
            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            
            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            //myBitmap.Save(@"D:\Barcode.jpg", jgpEncoder, myEncoderParameters);
            //myBitmap.Save(
            using (MemoryStream ms = new MemoryStream())
            {
                myBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = ms.ToArray();
            }

            return result;
        }

        private static ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {

            var codecs = ImageCodecInfo.GetImageDecoders();

            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        } 

        public DataTable GetData(SortedList<string, string> Parameters, string SubReportName, bool defaultProvider)
        {
            string SQL = GetSQLString(SubReportName);

            if (SQL == "") return new DataTable();

            SortedList<string, ParameterFieldDefinition> theFilters = GetFilters();

            foreach (string Key in theFilters.Keys)
            {
                if (Key == "ShowLastWeek") continue;


                string Val;
                if (Parameters.Keys.Contains(Key))
                    Val = Parameters[Key];
                else
                    Val = theFilters[Key].DefaultValues.Count > 0 ? ((ParameterDiscreteValue)theFilters[Key].DefaultValues[0]).Value.ToString() : string.Empty;

                if (theFilters[Key].ValueType == FieldValueType.DateField)
                    SQL = SQL.Replace("{?" + Key + "}", "'" + DateTime.Parse(Val).ToString("yyyy-MM-dd") + "'");
                else if (theFilters[Key].ValueType == FieldValueType.DateTimeField)
                    SQL = SQL.Replace("{?" + Key + "}", "'" + DateTime.Parse(Val).ToString("yyyy-MM-dd HH:mm:ss") + "'");
                else if (theFilters[Key].ValueType == FieldValueType.StringField)
                    SQL = SQL.Replace("{?" + Key + "}", "N'" + Val + "'");
                else if (theFilters[Key].ValueType == FieldValueType.NumberField)
                    SQL = SQL.Replace("{?" + Key + "}", Val);
                else if (theFilters[Key].ValueType == FieldValueType.BooleanField)
                {
                    string tempval = "0";
                    if (Val.ToLower().Trim() == "true")
                    {
                        tempval = "1";
                    }
                    SQL = SQL.Replace("{?" + Key + "}", tempval);
                }
            }

            Logger.writeLog(SQL);

            DataTable DT = new DataTable();
            QueryCommand Cmd;
            if (defaultProvider)
            {
                Cmd = new QueryCommand(SQL);
            }
            else
            {
                Cmd = new QueryCommand(SQL, "PowerMGT");
            }

            //Logger.writeLog("Start Query : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //Logger.writeLog(Cmd.CommandSql);
            Cmd.CommandTimeout = 60000;
            DT.Load(SubSonic.DataService.GetReader(Cmd));
            //Logger.writeLog("End Query : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return DT;
        }

        /// <summary>
        /// Get Report, complete with DataTable loaded.
        /// </summary>
        /// <param name="Parameters">Filter Parameters to filter the Data Table</param>
        /// <returns>Crystal Report loaded with data. Ready to be shown in GUI.</returns>
        public ReportDocument GetReport(SortedList<string, string> Parameters)
        {
            if (!IsReportLoaded) return new ReportDocument();

            TheDocument.SetDataSource(GetData(Parameters));
            for (int Counter = 0; Counter < TheDocument.Subreports.Count; Counter++)
                TheDocument.Subreports[Counter].SetDataSource(GetData(Parameters, TheDocument.Subreports[Counter].Name, true));
            TheDocument.Refresh();

            return TheDocument;
        }
    }
}
