using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PowerPOS.Container;
using System.Collections;
using POSDevices;
using PowerPOS;
using System.Configuration;
using System.IO;
using System.Diagnostics;
namespace WinPowerPOS.OrderForms
{
    public partial class frmAddItem : Form
    {
        public string searchReq;
        ItemController itemLogic;
        public decimal PreferedDiscount;
        public ArrayList itemNumbers;

        private bool IsItemPictureShown = false;

        #region "Form Initialization and loading"
        public frmAddItem()
        {
            InitializeComponent();
            IsItemPictureShown = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false);

            dgvItemList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvItemList.AllowUserToResizeRows = false;
        }
        private void frmAddItem_Load(object sender, EventArgs e)
        {
            try
            {
                Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
                               

                itemLogic = new ItemController();

                //find item using the given text            
                //ViewItemCollection coll = itemLogic.FetchByName(searchReq);
                bool onlySearchItemNoItemName = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.OnlySearchItemNoItemName), false);
                ViewItemCollection coll = itemLogic.SearchItem(searchReq, false, false, onlySearchItemNoItemName);

                if (coll.Count == 0)
                {
                    MessageBox.Show("Search produce no result.");
                    this.Close();
                }

                if (!IsItemPictureShown)
                {
                    dgvItemList.Columns["Photo"].Visible = false;
                }

                //item exist?
                dgvItemList.AutoGenerateColumns = false;
                this.dgvItemList.DataSource = coll;
                this.dgvItemList.Refresh();

                string sqlLoadAttributes = "SELECT * FROM AttributesLabel";
                IDataReader Rdr = SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sqlLoadAttributes));
                while (Rdr.Read())
                {
                    switch (Rdr.GetInt32(0))
                    {
                        case 1: colAttributes1.HeaderText = Rdr.GetString(1); colAttributes1.Visible = true; break;
                        case 2: colAttributes2.HeaderText = Rdr.GetString(1); colAttributes2.Visible = true; break;
                        case 3: colAttributes3.HeaderText = Rdr.GetString(1); colAttributes3.Visible = true; break;
                        case 4: colAttributes4.HeaderText = Rdr.GetString(1); colAttributes4.Visible = true; break;
                        case 5: colAttributes5.HeaderText = Rdr.GetString(1); colAttributes5.Visible = true; break;
                        case 6: colAttributes6.HeaderText = Rdr.GetString(1); colAttributes6.Visible = true; break;
                        case 7: colAttributes7.HeaderText = Rdr.GetString(1); colAttributes7.Visible = true; break;
                        case 8: colAttributes8.HeaderText = Rdr.GetString(1); colAttributes8.Visible = true; break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region "Add Item Logic"
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                itemNumbers = new ArrayList();

                //Add item from the selected checkboxes into the gridview
                for (int i = 0; i < dgvItemList.Rows.Count; i++)
                {
                    //if (dgvItemList.Rows[i].Cells[0]
                    if (dgvItemList.Rows[i].Cells[0].Value is bool &&
                        (bool)dgvItemList.Rows[i].Cells[0].Value == true)
                    {
                        itemNumbers.Add(dgvItemList.Rows[i].Cells[1].Value);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
            
        #region "Close Form"
        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                itemNumbers = null;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void dgvItemList_CellClick
            (object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == 3 || e.ColumnIndex == 1 || e.ColumnIndex == 0))
            {
                if (dgvItemList.Rows[e.RowIndex].Cells[0].Value is bool)
                {
                    if ((bool)dgvItemList.Rows[e.RowIndex].Cells[0].Value == true)
                        dgvItemList.Rows[e.RowIndex].Cells[0].Value = false;
                    else
                        dgvItemList.Rows[e.RowIndex].Cells[0].Value = true;
                }
                else
                {
                    dgvItemList.Rows[e.RowIndex].Cells[0].Value = true;
                }
            }
        }

        private void dgvItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && (e.ColumnIndex == 3 || e.ColumnIndex == 1))
            //{
            //    //Get the item no            
            //    itemNumbers = new ArrayList();
            //    itemNumbers.Add(dgvItemList.Rows[e.RowIndex].Cells[1].Value);
            //    this.Close();
            //}
        }

        private void llSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItemList.Rows.Count; i++)
            {
                dgvItemList.Rows[i].Cells[0].Value = true;
            }
        }

        private void llSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItemList.Rows.Count; i++)
            {
                dgvItemList.Rows[i].Cells[0].Value = false;
            }
        }

        private void llInvert_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItemList.Rows.Count; i++)
            {
                if (dgvItemList.Rows[i].Cells[0].Value is bool)
                {
                    dgvItemList.Rows[i].Cells[0].Value = !((bool)dgvItemList.Rows[i].Cells[0].Value);
                }
                else
                {
                    dgvItemList.Rows[i].Cells[0].Value = true;
                }
            }
        }

        private void dgvItemList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            ShowItemPicture();

            foreach (DataGridViewRow row in dgvItemList.Rows)
            {
                row.Height = 80;
            }

        }
        private void ShowItemPicture()
        {
            if (IsItemPictureShown)
            {
                //string ImagePhotosFolder = ConfigurationManager.AppSettings["ItemPhotosFolder"];
                //if (!String.IsNullOrEmpty(ImagePhotosFolder))
                //{
                //    if (!ImagePhotosFolder[ImagePhotosFolder.Length - 1].ToString().Equals(@"\"))
                //        ImagePhotosFolder += @"\";

                    //string[] extensions = { "jpg", "png", "bmp", "jpeg" };
                    foreach (DataGridViewRow row in dgvItemList.Rows)
                    {
                        string itemNo = row.Cells["colItemNo"].Value.ToString();
                        var myItem = new Item(Item.Columns.ItemNo, itemNo);
                        string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseImageItemFromLocalFolder), false))
                        {
                            string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Item.ItemImageFolderLocal);
                            if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                            {
                                
                                foreach (string ext in extensions)
                                {
                                    string ImagePath = ImagePhotosFolder + itemNo + "." + ext;
                                    if (System.IO.File.Exists(ImagePath))
                                    {
                                        Image img = ResizeImage(Image.FromFile(ImagePath), new Size(40, 40));
                                        row.Cells["Photo"].Value = img;
                                        row.Cells["Photo"].Tag = ImagePath;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (myItem.ItemImage != null)
                            {
                                row.Cells["Photo"].Value = ResizeImage(Image.FromStream(new MemoryStream(myItem.ItemImage)), new Size(40, 40));
                            }
                        }
                        //string ImagePath = ImagePhotosFolder + row.Cells["colItemNo"].Value.ToString();
                        //bool found = false;
                        //foreach (string ext in extensions)
                        //{
                        //    if (System.IO.File.Exists(ImagePath + "." + ext))
                        //    {
                        //        ImagePath = ImagePath + "." + ext;
                        //        found = true;
                        //        break;
                        //    }

                        //}
                        //if (found)
                        //{
                        //    Image img = ResizeImage(Image.FromFile(ImagePath), new Size(40, 40));
                        //    row.Cells["Photo"].Value = img;
                        //    row.Cells["Photo"].Tag = ImagePath;
                        //}
                    }
                //}
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
    /*
     *  else
                        {
                            double total = 0;
                            for (int k = 0; k< myDet.Count; k++)
                            {
                                total += (double)myDet[k].Amount;
                            }
                            PriceDisplay myDisplay = new PriceDisplay();
                            myDisplay.ClearScreen();
                            myDisplay.ShowItemPrice(myItem.ItemName, (double)myItem.RetailPrice, total);
                        }
                    }
                }

                if (errEncountered)
                {
                    //log the failure items to DB

                    //return list of failed insert to the function caller
                    status = "Not all items can be added due to internal error. Please try again. Failed Items: ";
                    for (int i = 0; i < errItemID.Count; i++)
                    {
                        status = status + errItemID[i].ToString() + Environment.NewLine;
                    }
                    //alert(status);
                    MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    this.Close();
                }*/
//}