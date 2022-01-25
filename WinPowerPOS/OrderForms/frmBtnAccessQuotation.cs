using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;
using PowerPOS.Container;
using WinPowerPOS.OrderForms;
using System.Configuration;

namespace WinPowerPOS
{
    public partial class frmBtnAccessQuotation : Form
    {
        public frmBtnAccessQuotation()
        {
            isSuccessful = false;
            InitializeComponent();

            IsItemPictureShown = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false);
        }
        public bool useItemForHotKeys;
        public bool isSuccessful;
        public Guid CatID;
        public string CategoryName;
        public string DisplayName;
        public QuoteController pos;

        private bool IsItemPictureShown = false; 

        private void frmCatAccess_Load(object sender, EventArgs e)
        {
            try
            {
                lblDisplayName.Text = DisplayName;

                DataTable dt;
                if (useItemForHotKeys)
                {
                    ItemCollection it = new ItemCollection();
                    it.Where(Item.Columns.CategoryName, CategoryName);
                    it.Where(Item.Columns.Deleted, false);
                    it.Load();
                    dt = it.ToDataTable();

                }
                else
                {
                    QuickAccessButtonCollection qb = QuickAccessController.FetchButtons(CatID);
                    dt = qb.ToDataTable();
                }
                //populate the flow lay out with programmable button
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Button b = new Button();
                    b.Width = 145;
                    b.Height = 110;
                    b.Tag = dt.Rows[i]["ItemNo"].ToString();

                    if (IsItemPictureShown)
                    {
                        string ImagePhotosFolder = ConfigurationManager.AppSettings["ItemPhotosFolder"];
                        if (!String.IsNullOrEmpty(ImagePhotosFolder))
                        {
                            if (!ImagePhotosFolder[ImagePhotosFolder.Length - 1].ToString().Equals(@"\"))
                                ImagePhotosFolder += @"\";

                            string[] extensions = { "jpg", "png", "bmp", "jpeg" };
                            string ImagePath = ImagePhotosFolder + dt.Rows[i]["ItemNo"].ToString();

                            bool found = false;
                            foreach (string ext in extensions)
                            {
                                if (System.IO.File.Exists(ImagePath + "." + ext))
                                {
                                    ImagePath = ImagePath + "." + ext;
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                            {
                                b.Image = ResizeImage(Image.FromFile(ImagePath), new Size(75, 70));
                                b.ImageAlign = ContentAlignment.TopCenter;
                                b.TextAlign = ContentAlignment.BottomCenter;
                            }
                        }
                    }
                    if (useItemForHotKeys)
                    {
                        decimal tmpDec = 0.00M;
                        decimal.TryParse(dt.Rows[i]["RetailPrice"].ToString(), out tmpDec);
                        b.Text = tmpDec.ToString("N2") + " " +
                            dt.Rows[i]["ItemName"].ToString();
                    }
                    else
                    {
                        string SQL = "Select itemName from item where itemno = '" + dt.Rows[i]["ItemNo"].ToString() + "'";
                        QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                        object obj = DataService.ExecuteScalar(cmd);
                        if (obj != null)
                            b.Text = obj.ToString();
                    }
                    Font bFont = b.Font;
                    b.Font = new Font(bFont.FontFamily, 10, FontStyle.Bold);
                    
                    TextBox t = new TextBox();
                    t.Text = "0";
                    t.Width = 140;
                    t.Tag = dt.Rows[i]["ItemNo"].ToString();

                    if (!useItemForHotKeys)
                    {
                        if (dt.Rows[i]["ForeColor"] != null && dt.Rows[i]["ForeColor"].ToString() != "")
                        {
                            b.ForeColor = System.Drawing.Color.FromName(dt.Rows[i]["ForeColor"].ToString());
                        }
                        else
                        {
                            b.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        b.ForeColor = System.Drawing.Color.Black;
                    }
                    
                    if (!useItemForHotKeys)
                    {
                        if (dt.Rows[i]["BackColor"] != null && dt.Rows[i]["BackColor"].ToString() != "")
                        {
                            string name = AppDomain.CurrentDomain.BaseDirectory +
                                            QuickAccessController.ButtonImageFolder + dt.Rows[i]["BackColor"].ToString() + ".png";
                            if (System.IO.File.Exists(name))
                            {
                                b.BackgroundImage = System.Drawing.Image.FromFile(name);
                            }
                            else
                            {
                                b.BackColor = System.Drawing.Color.FromName(dt.Rows[i]["BackColor"].ToString());
                            }
                        }
                        else
                        {
                            b.BackColor = System.Drawing.Color.White;
                        }
                    }
                    else
                    {
                        b.BackColor = System.Drawing.Color.White;
                    }
                    b.Click += delegate
                    {
                        btnBtn_Click(b, new EventArgs());
                    };
                    
                    t.Click += delegate
                    {
                        txtQty_Click(t, new EventArgs());
                    };
                    
                    Panel pnl = new Panel();
                    pnl.Height = 135;
                    pnl.Width = 150;
                    t.Location = new Point(0, 110);
                    pnl.Controls.Add(t);
                    pnl.Controls.Add(b);
                    t.Parent = pnl;
                    b.Parent = pnl;
                    pnl.Parent = tableLayoutPanel1;
                    if (!useItemForHotKeys)
                    {
                        tableLayoutPanel1.Controls.Add(pnl, (int)dt.Rows[i]["Col"], (int)dt.Rows[i]["Row"]);
                    }
                    else
                    {
                        tableLayoutPanel1.Controls.Add(pnl);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error:" + ex.Message);
                return;
            }
        }
        private void txtQty_Click(object sender, EventArgs e)
        {
            //prompt keypad
            frmKeypad f = new frmKeypad();
            f.IsInteger = true;
            f.initialValue = ((TextBox)sender).Text;
            f.ShowDialog();

            ((TextBox)sender).Text = f.value.ToString();

        }
        private void btnBtn_Click(object sender, EventArgs e)
        {            
            //Add item to POS controller.            
            string status;
            Button sendButton =((Button)sender);
            string itemno = sendButton.Tag.ToString();
            TextBox t = ((TextBox)(sendButton.Parent).Controls[0]);
            int qty = 0;
            int.TryParse(t.Text, out qty);
            t.Text = (qty + 1).ToString();
            //MessageBox.Show(itemno);
            Item myItem = new Item(itemno);
            decimal price = 0;

            //if (pos.ListOpenItemFromHotKeys.ContainsKey(itemno))
            //    price = pos.ListOpenItemFromHotKeys[itemno];

            //if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory)
            //{
            //    frmKeypad keyPad = new frmKeypad();
            //    keyPad.initialValue = price.ToString("N2");
            //    keyPad.textMessage = "Enter the price";
            //    keyPad.ShowDialog();
            //    decimal.TryParse(keyPad.value, out price);
            //    if (pos.ListOpenItemFromHotKeys.ContainsKey(itemno))
            //        pos.ListOpenItemFromHotKeys[itemno] = price;
            //    else
            //        pos.ListOpenItemFromHotKeys.Add(itemno, price);
            //}
            /*
            if (myItem != null && myItem.IsLoaded)
            {
                if (pos.AddItemToOrder(myItem, 1, pos.GetPreferredDiscount(), true, out status))
                {
                    //.....
                    lblDisplayName.Text = "1x" + myItem.ItemName;
                }
                else
                {
                    MessageBox.Show("Error:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            */ 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SaveWork();
            isSuccessful = false;
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //pos.OpenPriceItemAddedHotKey += new OpenPriceItemHandler(pos_OpenPriceItemAddedHotKey);
            SaveWork();
            isSuccessful = true;
            this.Close();
            this.Dispose();
            //if (_isAddOpenPrice)
            //    pos.CallOpenPriceItemAdded(_openPriceOrderDetID);
        }
        private bool _isAddOpenPrice = false;
        string _openPriceOrderDetID = "";
        void pos_OpenPriceItemAddedHotKey(object sender, string orderDetID)
        {
            //_isAddOpenPrice = true;
            //_openPriceOrderDetID = orderDetID;
        }

        private void SaveWork()
        {
            string status;
            TableLayoutControlCollection cols = tableLayoutPanel1.Controls;
            string itemno; int qty;
            for (int i = 0; i < cols.Count; i++)
            {
                if (cols[i] is Panel && cols[i].Controls[0] is TextBox)
                {
                    itemno = ((TextBox)(cols[i].Controls[0])).Tag.ToString();
                    qty = 0;
                    int.TryParse(((TextBox)(cols[i].Controls[0])).Text, out qty);

                    if (itemno != "" && qty > 0)
                    {
                        Item myItem = new Item(itemno);
                        if (myItem != null && myItem.IsLoaded)
                        {
                            if (pos.AddItemToOrder(myItem, qty, pos.GetPreferredDiscount(), true, out status, false))
                            {
                                //.....
                                //added so that nett amount is also updated, not just gross sales
                                pos.ApplyMembershipDiscount();

                                // LowQuantity Feature
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                                {
                                    if (ItemController.IsLowQuantityItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID,0))
                                    {
                                        MessageBox.Show("This item is only left with " + ItemController.GetMinQty(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                        }
                    }

                }
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
