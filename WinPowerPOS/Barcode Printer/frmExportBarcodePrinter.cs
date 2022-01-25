using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpreadsheetLight;
using PowerPOS;
using SpreadsheetLight.Drawing;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace WinPowerPOS.Barcode_Printer
{
    public partial class frmExportBarcodePrinter : Form
    {
        public frmExportBarcodePrinter()
        {
            InitializeComponent();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ExportWithTemplate();
            //ExportDefault();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmExportBarcodePrinter_Load(object sender, EventArgs e)
        {
            #region Load ComboBox Category
            cbCategory.Items.Clear();
            string sqlString = "Select CategoryName from category where deleted = 0 and CategoryName <> 'SYSTEM'";
            DataTable dtCategory = new DataTable();
            dtCategory.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sqlString)));
            cbCategory.Items.Add("ALL");
            cbCategory.Items.Add("PROMO");
            foreach (DataRow dr in dtCategory.Rows)
            {
                cbCategory.Items.Add(dr[0].ToString());
            }
            cbCategory.SelectedIndex = 0;
            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void ExportDefault()
        {
            try
            {
                SLDocument sl = new SLDocument();
                string categoryFilter = cbCategory.Items[cbCategory.SelectedIndex].ToString();
                string SQLString =
                    "Select * From ( " +
                    "SELECT ItemName, ItemNo, Barcode, Category.CategoryName " +
                    "FROM Item " +
                        "INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                        "INNER JOIN ItemDepartment ON Category.ItemDepartmentID = ItemDepartment.ItemDepartmentID " +
                    "WHERE Category.CategoryName <> 'SYSTEM' AND Category.Deleted = 0 AND Item.Deleted = 0 ";
                if (!String.IsNullOrEmpty(categoryFilter) && categoryFilter != "ALL")
                {
                    SQLString += " and Category.CategoryName = '" + categoryFilter + "' ";
                }

                if (cbCreatedOn.Checked)
                {
                    SQLString += " and cast(Item.CreatedOn as date) between '" + dtpStartDate.Value.ToString("yyyy-MM-dd") + "' and '" + dtpEndDate.Value.ToString("yyyy-MM-dd") + "' ";
                }

                if (String.IsNullOrEmpty(categoryFilter) && (categoryFilter == "ALL" || categoryFilter == "PROMO"))
                {
                    SQLString +=
                    @"UNION ALL
                SELECT PromoCampaignName as ItemName, ISNULL(PromoCode, CAST(ph.PromoCampaignHdrID AS VARCHAR(MAX))) as ItemNo,
                ISNULL(ph.Barcode,'') as Barcode, 'Promo' as CategoryName  
                FROM PromoCampaignHdr ph, PromoCampaignDet pd
                where ph.PromoCampaignHdrID = pd.PromoCampaignHdrID AND GETDATE() between ph.DateFrom and ph.DateTo 
                group by PromoCampaignName, ISNULL(PromoCode, CAST(ph.PromoCampaignHdrID AS VARCHAR(MAX))),
                ISNULL(ph.Barcode,'') " + " ) a " +
                    "ORDER BY ItemName";
                }
                else
                {
                    SQLString += " ) a ";
                    SQLString += "ORDER BY ItemName";
                }


                DataTable dt = new DataTable();
                dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false))
                {
                    dt.Columns.Add("Image", typeof(string));
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

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SLPicture slp = null;

                        string itemNo = dt.Rows[i]["ItemNo"].ToString();
                        var myItem = new PowerPOS.Item(PowerPOS.Item.Columns.ItemNo, itemNo);
                        Image img = null;
                        int height = 10;

                        string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseImageItemFromLocalFolder), false))
                        {
                            string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Item.ItemImageFolderLocal);
                            if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                            {

                                foreach (string ext in extensions)
                                {
                                    string ImagePath = ImagePhotosFolder + itemNo.Trim() + "." + ext;
                                    if (System.IO.File.Exists(ImagePath))
                                    {
                                        slp = new SLPicture(ImagePath);
                                        img = Image.FromFile(ImagePath);
                                        height = img.Height;
                                        img.Dispose();
                                        break;
                                    }

                                }
                            }
                        }
                        else
                        {
                            if (myItem.ItemImage != null)
                            {
                                slp = new SLPicture(myItem.ItemImage, ImagePartType.Jpeg);
                            }
                        }

                        if (slp != null)
                        {

                            sl.SetRowHeight(iStartRowIndex + i, 5, height);
                            slp.SetPosition(iStartRowIndex + i, 5);
                            sl.InsertPicture(slp);
                        }
                    }
                }
                sl.AutoFitColumn(1, iEndColumnIndex);
                sl.SaveAs(saveFileDialog1.FileName);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Export File: " + ex.Message);
                Logger.writeLog("Error Export File: " + ex.Message);
            }
        }

        private void ExportWithTemplate()
        {
            try
            {
                SLDocument sl = new SLDocument();
                string categoryFilter = cbCategory.Items[cbCategory.SelectedIndex].ToString();
                string SQLString =
                    "Select * From ( " +
                    "SELECT ItemName, ItemNo, Barcode, Category.CategoryName " +
                    "FROM Item " +
                        "INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                        "INNER JOIN ItemDepartment ON Category.ItemDepartmentID = ItemDepartment.ItemDepartmentID " +
                    "WHERE Category.CategoryName <> 'SYSTEM' AND Category.Deleted = 0 AND Item.Deleted = 0 ";
                if (!String.IsNullOrEmpty(categoryFilter) && categoryFilter != "ALL")
                {
                    SQLString += " and Category.CategoryName = '" + categoryFilter + "' ";
                }

                if (cbCreatedOn.Checked)
                {
                    SQLString += " and cast(Item.CreatedOn as date) between '" + dtpStartDate.Value.ToString("yyyy-MM-dd") + "' and '" + dtpEndDate.Value.ToString("yyyy-MM-dd") + "' ";
                }

                if (String.IsNullOrEmpty(categoryFilter) && (categoryFilter == "ALL" || categoryFilter == "PROMO"))
                {
                    SQLString +=
                    @"UNION ALL
                SELECT PromoCampaignName as ItemName, ISNULL(PromoCode, CAST(ph.PromoCampaignHdrID AS VARCHAR(MAX))) as ItemNo,
                ISNULL(ph.Barcode,'') as Barcode, 'Promo' as CategoryName  
                FROM PromoCampaignHdr ph, PromoCampaignDet pd
                where ph.PromoCampaignHdrID = pd.PromoCampaignHdrID AND GETDATE() between ph.DateFrom and ph.DateTo 
                group by PromoCampaignName, ISNULL(PromoCode, CAST(ph.PromoCampaignHdrID AS VARCHAR(MAX))),
                ISNULL(ph.Barcode,'') " + " ) a " +
                    "ORDER BY ItemName";
                }
                else
                {
                    SQLString += " ) a ";
                    SQLString += "ORDER BY CategoryName, ItemName";
                }


                DataTable dt = new DataTable();
                dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));

                int rowCount = 10;
                int columnCount = 4;

                int index = 0;
                int iStartRowIndex = 2;
                int iStartColumnIndex = 1;
                
                //set the border
                SLStyle sTyle1 = sl.CreateStyle();
                sTyle1.SetBottomBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle1.SetLeftBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle1.SetRightBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle1.SetTopBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle1.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
                sTyle1.SetVerticalAlignment(VerticalAlignmentValues.Top);

                //set the barcode
                SLStyle sTyle2 = sl.CreateStyle();
                sTyle2.SetBottomBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle2.SetLeftBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle2.SetRightBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle2.SetTopBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle2.SetFont("Free 3 of 9 Extended", 20);
                sTyle2.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
                sTyle2.SetVerticalAlignment(VerticalAlignmentValues.Center);

                //set the barcode
                SLStyle sTyle3 = sl.CreateStyle();
                sTyle3.SetBottomBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle3.SetLeftBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle3.SetRightBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle3.SetTopBorder(BorderStyleValues.Medium, System.Drawing.Color.Black);
                sTyle3.SetFont("Arial", 8);
                sTyle3.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
                sTyle3.SetVerticalAlignment(VerticalAlignmentValues.Center);
                sTyle3.SetWrapText(true);

                do
                {
                    string category = dt.Rows[index]["CategoryName"].ToString();
                    int i = 0;

                    if (i == rowCount)
                        i = 0;

                    for (i = 0; i < rowCount; i++)
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            if (index == dt.Rows.Count)
                                break;

                            int columnIndex = (2 * j) + 1;
                            string itemNo = dt.Rows[index]["ItemNo"].ToString();
                            var myItem = new PowerPOS.Item(PowerPOS.Item.Columns.ItemNo, itemNo);

                            /*set barcode*/
                            string Barcode = dt.Rows[index]["Barcode"].ToString();
                            sl.MergeWorksheetCells(iStartRowIndex, columnIndex + 1, iStartRowIndex + 1, columnIndex + 1);
                            sl.SetCellStyle(iStartRowIndex, columnIndex + 1, iStartRowIndex + 1, columnIndex + 1, sTyle2);
                            sl.SetCellValue(iStartRowIndex, columnIndex + 1, string.IsNullOrEmpty(Barcode) ? "" : "*" + Barcode + "*");
                            sl.SetColumnWidth(columnIndex + 1, 18);

                            string ItemName = dt.Rows[index]["ItemName"].ToString();
                            sl.MergeWorksheetCells(iStartRowIndex + 2, columnIndex + 1, iStartRowIndex + 3, columnIndex + 1);
                            sl.SetCellStyle(iStartRowIndex + 2, columnIndex + 1, iStartRowIndex + 3, columnIndex + 1, sTyle3);
                            sl.SetCellValue(iStartRowIndex + 2, columnIndex + 1, ItemName);
    
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false))
                            {

                                SLPicture slp = null;

                               
                                Image img = null;
                                int height = 10;

                                string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseImageItemFromLocalFolder), false))
                                {
                                    string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Item.ItemImageFolderLocal);
                                    if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                                    {

                                        foreach (string ext in extensions)
                                        {
                                            string ImagePath = ImagePhotosFolder + itemNo.Trim() + "." + ext;
                                            if (System.IO.File.Exists(ImagePath))
                                            {
                                                img = Image.FromFile(ImagePath);
                                                byte[] pictSize = ItemController.ResizeImageBiteToArray(img, new Size() {Height= 60, Width= 60});
                                                slp = new SLPicture(pictSize, ImagePartType.Jpeg);
                                                height = img.Height;
                                                img.Dispose();
                                                break;
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (myItem.ItemImage != null)
                                    {
                                        MemoryStream ms = new MemoryStream(myItem.ItemImage);
                                        byte[] pictSize = ItemController.ResizeImageBiteToArray(Image.FromStream(ms), new Size() { Height = 60, Width = 60 });

                                        slp = new SLPicture(pictSize, ImagePartType.Jpeg);
                                        ms.Dispose();
                                    }
                                }

                                if (slp != null)
                                {
                                    
                                    sl.SetColumnWidth(columnIndex, columnIndex, 12);
                                    slp.SetPosition(iStartRowIndex, columnIndex-1);
                                    slp.SetRelativePositionInPixels(iStartRowIndex, columnIndex, 5, 5);
                                    sl.InsertPicture(slp);
                                }

                            }

                            /*set picture*/
                            sl.MergeWorksheetCells(iStartRowIndex, columnIndex, iStartRowIndex + 3, columnIndex);
                            sl.SetCellStyle(iStartRowIndex, columnIndex, iStartRowIndex + 3, columnIndex, sTyle1);

                            index++;
                        }

                        iStartRowIndex += 4;
                    }
                } while (index < dt.Rows.Count);


                //sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);
                //int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
                //int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
                //SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
                //table.SetTableStyle(SLTableStyleTypeValues.Medium2);
                //table.HasTotalRow = false;
                //sl.InsertTable(table);


                //sl.AutoFitColumn(1, iEndColumnIndex);
                sl.SaveAs(saveFileDialog1.FileName);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Export File: " + ex.Message);
                Logger.writeLog("Error Export File: " + ex.Message);
            }
        }

        private Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}
