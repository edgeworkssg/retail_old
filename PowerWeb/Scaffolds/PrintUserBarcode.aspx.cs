using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;
using SubSonic;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace PowerWeb.Scaffolds
{
    public partial class PrintUserBarcode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            string param = (Request.QueryString["Param"] + "").BinaryToString();
            if (string.IsNullOrEmpty(param))
                param = "";

            string sql = @"SELECT   UM.UserName
		                            ,UM.DisplayName
		                            ,UM.Remark  
                                    ,CASE WHEN ISNULL(UM.Userfld7,'') = '' THEN '' ELSE ''+ISNULL(UM.Userfld7,'')+'' END Barcode
		                            ,UG.GroupID
		                            ,UG.GroupName
                            FROM	UserMSt UM
		                            INNER JOIN UserGroup UG ON UG.GroupID = UM.GroupName
                            WHERE	(@UserName = '' OR UM.UserName = @UserName)
                                    AND ISNULL(UM.Deleted,0) = 0
                                    AND ISNULL(UM.Userfld7,'') <> ''";
            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@UserName", param);

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));
            dt.Columns.Add("BarcodeImage", typeof(byte[]));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string barcode = dt.Rows[i]["Barcode"] + "";
                var img = CreateBarcode(string.Format("*{0}*", barcode));
                //var img = CreateBarcode(barcode);
                dt.Rows[i]["BarcodeImage"] = img;
            }

            ReportDocument report = new ReportDocument();
            report.Load(Server.MapPath("~/Bin/Reports/UserBarcode.rpt"));
            report.SetDataSource(dt);
            report.Refresh();

            crReport.ReportSource = report;
            crReport.HasExportButton = true;
            crReport.RefreshReport();
        }

        protected void CR_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        {
            BindData();
        }

        private static byte[] CreateBarcode(string code)
        {
            byte[] result = null;

            int width = 240;
            int height = 40;

            var myBitmap = new Bitmap(width, height);
            var g = Graphics.FromImage(myBitmap);
            var jgpEncoder = GetEncoder(ImageFormat.Jpeg);

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
                myBitmap.Save(ms, ImageFormat.Jpeg);
                result = ms.ToArray();
            }

            return result;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
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
    }
}
