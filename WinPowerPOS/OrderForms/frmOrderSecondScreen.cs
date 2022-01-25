using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.IO;
using System.Collections;
using PowerPOSLib.Controller;
using PowerPOS.Container;
using SubSonic;

namespace WinPowerPOS.OrderForms
{
    public partial class frmOrderSecondScreen : Form
    {

        private int xpos, ypos;
        string txt;
        Bitmap pics;
        Graphics gr;
        Font f;
        String[] slideshow;
        int totalPicture, posPicture, ctPicture;
        public POSController pos;
        bool countClear = false;
        int counterClear = 0;
        bool isShowForeignCurrency = false;
        decimal exchangeRate = 1;
        string currencyCode = "";
        string currencySymbol = "";
        public frmOrderTaking frmOrderTaking;
        private Timer t = new Timer();
        private bool IsRatingActivated = false;

        public frmOrderSecondScreen()
        {
            InitializeComponent();            
        }

        private void frmOrderSecondScreen_Load(object sender, EventArgs e)
        {
            isShowForeignCurrency = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.ShowForeignCurrency), false);
            dgvPurchase.AutoGenerateColumns = false;
            if (!isShowForeignCurrency)
                tblSlideShow.RowStyles[0].Height = 0;
            else
            {
                ExchangeRateController ex = new ExchangeRateController();
                ex.Load("ExchangeRate");
                Hashtable exchangeRateData = ex.GetHashTable();
                currencyCode = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.ForeignCurrency);
                Currency curr = new Currency(Currency.Columns.CurrencyCode, currencyCode);
                if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                    currencySymbol = curr.CurrencySymbol;
                else
                    currencySymbol = "$";
                if (exchangeRateData != null)
                    exchangeRate = (exchangeRateData[currencyCode] + "").GetDecimalValue();
            }
            pnlForeignCurrency.Visible = isShowForeignCurrency;

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.HideItemNo), false))
            {
                dgvPurchase.Columns["ItemNo"].Visible = false;
            }
            //txtEntry.Text = initialValue;
            //lblMessage.Text = textMessage;
            this.StartPosition = FormStartPosition.Manual;
            try
            {
                this.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            catch
            {
                this.Location = Screen.AllScreens[0].WorkingArea.Location;
            }
            this.WindowState = FormWindowState.Maximized;
            pnlGrandTotal.Left = (this.ClientSize.Width - pnlGrandTotal.Width) / 2;
            pnlGrandTotal.Top = (this.ClientSize.Height - pnlGrandTotal.Height) / 2;

            /*tpOrderSecondScreen.RowStyles[0] = new RowStyle(SizeType.Percent, 50f);
            tpOrderSecondScreen.RowStyles[1] = new RowStyle(SizeType.Absolute, 0F);
            tpOrderSecondScreen.RowStyles[2] = new RowStyle(SizeType.Percent, 50F);*/

            //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.HideLogo), false))
            //{
            //    tpOrderSecondScreen.RowStyles[3] = new RowStyle(SizeType.Absolute, 00f);
            //    tpOrderSecondScreen.RowStyles[4] = new RowStyle(SizeType.Percent, 10f);
            //}
            // Show middle row.
            
            //this.tableLayoutPanel1.RowStyles[1] = new RowStyle(SizeType.Percent, 20F);
            //this.tableLayoutPanel1.RowStyles[2] = new RowStyle(SizeType.Percent, 20F);
 

            //load logo 
            //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logo2.jpg"))
            //{
            //    pbLogo.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Logo2.jpg");
            //    pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            //}
            
            //load slideshow
            if(Directory.Exists(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.SlideShowFolder)))
            {
                slideshow = Directory.GetFiles(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.SlideShowFolder));
                totalPicture = slideshow.Count();
                if (totalPicture > 0)
                {
                    posPicture = 0;
                    ctPicture = 0;
                    //pbSlideShow.Image = Image.FromFile(slideshow[0]);
                    //pbSlideShow.SizeMode = PictureBoxSizeMode.StretchImage;
                    try
                    {
                        // Change the image loading to use filestream so the file won't be locked by the process and can be deleted.
                        using (FileStream fs = new FileStream(slideshow[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            pbSlideShow.Image = Image.FromStream(fs);
                            pbSlideShow.SizeMode = PictureBoxSizeMode.StretchImage;
                            fs.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                    }
                }
            }

            //setting marquee
            txt = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.MarqueeText);
            xpos = 200 ;
            ypos = 20;
            pics = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            ypos = (pictureBox1.Height / 2) - 7 < 0 ? 5 : (pictureBox1.Height / 2) - 7; 
            gr = Graphics.FromImage(pics);
            f = new Font("tahoma",14,FontStyle.Bold);
            gr.FillRectangle (Brushes.DarkOrange, 0, 0, pictureBox1.Width,pictureBox1.Height);
            gr.DrawString(txt, f, Brushes.White, xpos,ypos);
            
            pictureBox1.Image = pics;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logo3.jpg"))
            {
                pictureBox2.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Logo3.jpg");
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            timer1.Enabled = true;


            #region Setup Rating

            this.pnlRating.Width = this.Width;

            this.pnlRating.Left = (this.Width - this.pnlRating.Width) / 2;
            this.pnlRating.Top = (this.Height - this.pnlRating.Height) / 2;

            this.pnlFeedback.Left = (this.Width - this.pnlFeedback.Width) / 2;
            this.pnlFeedback.Top = (this.Height - this.pnlFeedback.Height) / 2;

            this.pnlRating.Visible = false;
            this.pnlFeedback.Visible = false;

            // set position pbr
            this.label2.Width = this.pnlRating.Width;
            this.label4.Width = this.pnlRating.Width;

            int left = (this.pnlRating.Width - (150 * 4 + 5 * 5)) / 2;
            this.pbr5.Left = left;
            this.pbr4.Left = left + 5 * 1 + 150 * 1;
            this.pbr3.Left = left + 5 * 2 + 150 * 2;
            this.pbr2.Left = left + 5 * 3 + 150 * 3;
            //this.pbr1.Left = left + 5 * 4 + 150 * 4;

            this.pbr1.Click += new EventHandler(onClickRating);
            this.pbr2.Click += new EventHandler(onClickRating);
            this.pbr3.Click += new EventHandler(onClickRating);
            this.pbr4.Click += new EventHandler(onClickRating);
            this.pbr5.Click += new EventHandler(onClickRating);

            #endregion

            t.Interval = 30000; // 30s            
            t.Tick += new EventHandler(t_Tick);
        }

        void onClickRating(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            try
            {
                QueryCommandCollection cmd = new QueryCommandCollection();
                cmd.Add(new QueryCommand("INSERT INTO rating (POSID, Rating, Staff, Timestamp, CreatedOn, CreatedBy, UniqueID, OrderHdrID, ModifiedOn, ModifiedBy) VALUES (" + PointOfSaleInfo.PointOfSaleID + ", " + pb.Tag + ", '" + UserInfo.username + "', GETDATE(), GETDATE(), '" + UserInfo.username + "', '" + PointOfSaleInfo.PointOfSaleID + "_" + Guid.NewGuid().ToString() + "', '" + frmOrderTaking.OrderHdrID + "', GETDATE(), '" + UserInfo.username + "')"));
                SubSonic.DataService.ExecuteTransaction(cmd);

                if (!frmOrderTaking.SyncRatingThread.IsBusy)
                    frmOrderTaking.SyncRatingThread.RunWorkerAsync();
                frmOrderTaking.SetFocusBarcode();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            setVisiblePanelRating(false);
            setVisiblePanelFeedback(true);

            frmOrderTaking.SetFocusBarcode();
            
        }

        public void setVisiblePanelRating(bool b)
        {

            IsRatingActivated = b;

            //UserMst usm = new UserMst.
            this.label2.Text = "Hi, I'm " + UserInfo.displayName;
            this.pnlRating.Visible = b;

            t.Enabled = b;

            frmOrderTaking.SetFocusBarcode();
        }

        void t_Tick(object sender, EventArgs e)
        {
            setVisiblePanelFeedback(false);
            setVisiblePanelRating(false);

            Timer t = (Timer)sender;
            t.Stop();
            t.Dispose();
        }

        public void setVisiblePanelFeedback(bool b)
        {
            this.pnlFeedback.Visible = b;

            if (b)
            {
                Timer t = new Timer();
                t.Interval = 5000; // 5s
                t.Enabled = true;
                t.Tick += new EventHandler(t_Tick);
            }

            frmOrderTaking.SetFocusBarcode();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            xpos = xpos - 2;
            if (xpos <= 0)
            {
                xpos = pictureBox1.Width - txt.Length;
            }

            pics = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gr = Graphics.FromImage(pics);
            f = new Font("tahoma", 12, FontStyle.Bold);
            gr.FillRectangle(Brushes.DarkOrange, 0, 0, pictureBox1.Width, pictureBox1.Height);
            gr.DrawString(txt, f, Brushes.Black, xpos, ypos);
            pictureBox1.Image = pics;

            //if (totalPicture > 1)
            //{
            //    ctPicture += 1;
            //    if (ctPicture >= 100)
            //    {
            //        ctPicture = 0;
            //        posPicture = posPicture + 1;
            //        if (posPicture == totalPicture) posPicture = 0;
            //        pbSlideShow.Image = Image.FromFile(slideshow[posPicture]);
            //        pbSlideShow.SizeMode = PictureBoxSizeMode.StretchImage;
            //    }
            //}

            //if (countClear)
            //{
            //    counterClear++;
            //    if (counterClear >= 310)
            //    {
            //        ClearControls();
            //    }
            //}

            ctPicture += 1;
            if (ctPicture >= 100)
            {
                ctPicture = 0;
                if (Directory.Exists(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.SlideShowFolder)))
                {
                    slideshow = Directory.GetFiles(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.SlideShowFolder));
                    totalPicture = slideshow.Count();
                    if (totalPicture > 0)
                    {
                        posPicture = posPicture + 1;
                        if (posPicture >= totalPicture) posPicture = 0;
                        //pbSlideShow.Image = Image.FromFile(slideshow[posPicture]);
                        //pbSlideShow.SizeMode = PictureBoxSizeMode.StretchImage;
                        try
                        {
                            // Change the image loading to use filestream so the file won't be locked by the process and can be deleted.
                            using (FileStream fs = new FileStream(slideshow[posPicture], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                if (slideshow[posPicture].ToUpper().EndsWith("JPG") || slideshow[0].ToUpper().EndsWith("BMP") || slideshow[0].ToUpper().EndsWith("JPEG"))
                                {
                                    pbSlideShow.Image = Image.FromStream(fs);
                                    pbSlideShow.SizeMode = PictureBoxSizeMode.StretchImage;
                                    fs.Dispose();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                    else
                    {
                        pbSlideShow.Image = null;
                    }
                }
            }
            
        }

        public void updateClear(bool startClear)
        {
            countClear = startClear;
            counterClear = 0;
        }

        public void ClearControls()
        {
            while (dgvPurchase.Rows.Count > 0)
            {
                dgvPurchase.Rows.RemoveAt(0);
            }
            dgvPurchase.Refresh();
            pos = new POSController();
            lblTotalAmount.Text = "0";
            lblTenderAmt.Text = "0";
            lblChange.Text = "0";
            updateClear(false);

            //lblChange.Text = "0";
            frmOrderTaking.SetFocusBarcode();
        }

        public void UpdateTenderAmount(string TenderAmt)
        {
            lblTenderAmt.Text = TenderAmt;
        }

        public void UpdateChange(string change)
        {
            lblChange.Text = change;
        }

        public void UpdateView()
        {
            try
            {
                if (!string.IsNullOrEmpty(pos.myOrderHdr.OrderHdrID) && (pos.myOrderDet.Count == 0))
                {
                    string interval = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.RefreshInterval);
                    int intv = 0;
                    int.TryParse(interval, out intv);
                    if (intv == 0) intv = 6000;
                    timer2.Interval = intv;
                    timer2.Start();
                }
                else
                {
                    UpdateViewHelper();
                    //pos.myOrderHdr.OrderHdrID;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public void UpdateViewHelper()
        {
            string status = "";
            updateClear(false);
            dgvPurchase.DataSource = pos.FetchUnSavedOrderItemsforSecondScreen(out status);
            if (status != "")
            {
                MessageBox.Show("Error: " + status);
                return;
            }
            
            dgvPurchase.Refresh();
            if (dgvPurchase.Rows.Count > 0)
                dgvPurchase.Rows[0].Selected = true;
            else
            {
                
            }
            updateTotalAmount();

            this.Validate();
            this.ValidateChildren(); 
        }

        public void updateTotalAmount()
        {
            try
            {
                string status = "";
                //Calculate total discount
                decimal TotalAmount = pos.CalculateTotalAmount(out status);
                lblTotalAmount.Text = TotalAmount.ToString("N");
                lblTotalAmount2.Text = TotalAmount.ToString("N");
                if (isShowForeignCurrency)
                {
                    if (exchangeRate != 0)
                    {
                        lblForeignCurrency.Text = string.Format("{0} : {2}{1}", currencyCode,
                            (TotalAmount / exchangeRate).ToString("N2"), currencySymbol);
                    }
                }
                lblTenderAmt.Text = "0";
                lblChange.Text = "0";
                if (status != "")
                {
                    MessageBox.Show("Error while calculating total amount: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }
		
		private void timer2_Tick(object sender, EventArgs e)
        {
            UpdateViewHelper();
            //orderRefNo = pos.GetOrderHdrID();
            timer2.Stop();
        }

        public bool WaitConfirmSecondaryDialog()
        {
            frmConfirmAmount frm = new frmConfirmAmount();
            frm.ShowDialog(this);

            return frm.ConfirmStatus;
        }

        private void pbSlideShow_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void pnlRating_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void dgvPurchase_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void pnlGrandTotal_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void pnlForeignCurrency_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void pnlFeedback_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void lblTotalAmount2_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void lblTenderAmt_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void lblChange_Click(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            frmOrderTaking.SetFocusBarcode();
        }

        private void frmOrderSecondScreen_Leave(object sender, EventArgs e)
        {

        }

        private void frmOrderSecondScreen_Deactivate(object sender, EventArgs e)
        {
           
        }

        private void frmOrderSecondScreen_Activated(object sender, EventArgs e)
        {
            if(!IsRatingActivated)
                frmOrderTaking.SetFocusBarcode();
        }
    }
}
