using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Threading;
using System.Globalization;
using System.IO;
using PowerInventory.ItemForms;
using PowerInventory.InventoryForms;
using PowerInventory.Setup;

namespace PowerInventory
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void DisableButtons()
        {
            btnLoad.Enabled = false;
            btnChangePassword.Enabled = false;                            
            btnGoodsReceive.Enabled = false;                            
            btnStockIssue.Enabled = false;                            
            btnStockTransfer.Enabled = false;                            
            btnStockAdjustment.Enabled = false;                            
            btnStockOnHand.Enabled = false;                            
            btnStockCard.Enabled = false;                            
            btnStockTake.Enabled = false;                            
            btnProducts.Enabled = false;
            btnStockOnHand.Enabled = false;
            btnStockTakeReport.Enabled = false;
            btnLocationSetup.Enabled = false;                                                        
            btnUserSetup.Enabled = false;                            
            btnChangeLocation.Enabled = false;                            
            btnAdjustStockTake.Enabled = false;
            btnPrivilegeSetup.Enabled = false;
            btnViewActivityHeader.Enabled = false;
            btnViewActivityLog.Enabled = false;
        }
        private void EnableByPrivilegesButtons()
        {            
            if (UserInfo.username != "")
            {
                DisableButtons();
                //loop through the privilige list and enable the user are allowed
                for (int i = 0; i < UserInfo.privileges.Rows.Count; i++)
                {
                    //do a switch
                    switch (UserInfo.privileges.Rows[i]["privilegeName"].ToString().ToLower())
                    {
                        /*
                        case "Change Password":
                            btnChangePassword.Enabled = true;
                            break;*/
                        case "goods receive":
                            btnGoodsReceive.Enabled = true;
                            btnLoad.Enabled = true;
                            break;
                        case "stock issue":
                            btnStockIssue.Enabled = true;
                            btnLoad.Enabled = true;
                            break;
                        case "stock transfer":
                            btnStockTransfer.Enabled = true;
                            btnLoad.Enabled = true;
                            break;
                        case "stock adjustment":
                            btnStockAdjustment.Enabled = true;
                            btnLoad.Enabled = true;
                            break;
                        case "stock on hand":
                            btnStockOnHand.Enabled = true;
                            break;
                        case "stock card":
                            btnStockCard.Enabled = true;
                            break;
                        case "stock take":
                            btnStockTake.Enabled = true;
                            break;                        
                        case "product setup":
                            btnProducts.Enabled = true;
                            break;
                        case "category setup":
                            //btnCategory.Enabled = true;
                            break;
                        case "department setup":
                            //btnDepartment.Enabled = true;
                            break;
                        case "inventory location setup":
                            btnLocationSetup.Enabled = true;
                            break;
                        case "import inventory":
                            //btnImportInventory.Enabled = true;
                            break;
                            /*
                        case "User Setup":
                            btnUserSetup.Enabled = true;
                            break;*/
                        case "pos setup":
                            btnChangeLocation.Enabled = true;
                            break;
                        case "adjust stock take":
                            btnAdjustStockTake.Enabled = true;
                            break;
                        case "privilege setup":
                            btnPrivilegeSetup.Enabled = true;
                            break;
                        case "view activity":
                            btnViewActivityHeader.Enabled = true;
                            break;
                        case "view activity by item":
                            btnViewActivityLog.Enabled = true;
                            break;
                        case "stock take report":
                            btnStockTakeReport.Enabled = true;
                            break;       
                    }
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //DisableButtons();
            if (File.Exists(Application.StartupPath + "\\banner.jpg"))
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\banner.jpg");
                pictureBox1.Refresh();
            }
           
            LoginForms.frmPOSLogin f = new LoginForms.frmPOSLogin();
            f.allowClose = false;
            CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            if (f.IsSuccessful)
            {
                PointOfSaleController.GetPointOfSaleInfo();
                AttributesLabelController.FetchProductAttributeLabel();
                lblCashierName.Text = UserInfo.displayName;
                lblLocation.Text = PointOfSaleInfo.InventoryLocationName;
                EnableByPrivilegesButtons();
            }
            else 
            {
                Application.Exit();
                return;
            }
            f.Dispose();

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {            
            DialogResult dr = MessageBox.Show("Are you sure you want to logout?", "", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                lblCashierName.Text = "-";
                UserController.ClearUserInfo();                                
                frmMain_Load(this, new EventArgs());
            }            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStockOnHand_Click(object sender, EventArgs e)
        {

        }

        private void btnImportInventory_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent();openFileDialog1.ShowDialog();CommonUILib.hideTransparent();                        
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (ExportController.ImportInventoryFromExcel(openFileDialog1.FileName))
            {
                MessageBox.Show("Import successful");
            }
            else
            {
                MessageBox.Show("Import failed. Please check your excel file");
            }
        }

        frmStockIn FfrmStockIn;
        private void btnGoodsReceive_Click(object sender, EventArgs e)
        {
            if (StockTakeController.IsThereUnAdjustedStockTake())
            {
                MessageBox.Show("There are Stock take in progress, no stock movement is allowed");
                return;
            }
            if (FfrmStockIn == null || FfrmStockIn.IsDisposed)
            {
                FfrmStockIn = new frmStockIn();
                FfrmStockIn.Show();                
            }
            else
            {
                FfrmStockIn.WindowState = FormWindowState.Maximized;
                FfrmStockIn.Activate();
            }
        }

        frmStockOut FfrmStockOut;
        private void btnStockIssue_Click_1(object sender, EventArgs e)
        {
            if (StockTakeController.IsThereUnAdjustedStockTake())
            {
                MessageBox.Show("There are Stock take in progress, no stock movement is allowed");
                return;
            }
            if (FfrmStockOut == null || FfrmStockOut.IsDisposed)
            {
                FfrmStockOut = new frmStockOut();
                FfrmStockOut.Show();
            }
            else
            {
                FfrmStockOut.WindowState = FormWindowState.Maximized;
                FfrmStockOut.Activate();
            }
        }

        PowerInventory.frmStockTake FfrmStockTake;
        private void btnStockTake_Click(object sender, EventArgs e)
        {

            if (FfrmStockTake == null || FfrmStockTake.IsDisposed)
            {
                FfrmStockTake = new frmStockTake();
                FfrmStockTake.Show();
            }
            else
            {
                FfrmStockTake.WindowState = FormWindowState.Maximized;
                FfrmStockTake.Activate();
            }
        }

        frmAdjustStock FfrmAdjustStock;
        private void btnStockAdjustment_Click(object sender, EventArgs e)
        {
            if (StockTakeController.IsThereUnAdjustedStockTake())
            {
                MessageBox.Show("There are Stock take in progress, no stock movement is allowed");
                return;
            }
            if (FfrmAdjustStock == null || FfrmAdjustStock.IsDisposed)
            {
                FfrmAdjustStock = new frmAdjustStock();
                FfrmAdjustStock.Show();
            }
            else
            {
                FfrmAdjustStock.WindowState = FormWindowState.Maximized;
                FfrmAdjustStock.Activate();
            }
        }

        frmStockTransfer FfrmStockTransfer;
        private void btnStockTransfer_Click(object sender, EventArgs e)
        {
            if (StockTakeController.IsThereUnAdjustedStockTake())
            {
                MessageBox.Show("There are Stock take in progress, no stock movement is allowed");
                return;
            }
            if (FfrmStockTransfer == null || FfrmStockTransfer.IsDisposed)
            {
                FfrmStockTransfer = new frmStockTransfer();
                FfrmStockTransfer.Show();
            }
            else
            {
                FfrmStockTransfer.WindowState = FormWindowState.Maximized;
                FfrmStockTransfer.Activate();
            }
        }

        frmInventoryLocationList FfrmInventoryLocationList;
        private void btnLocationSetup_Click(object sender, EventArgs e)
        {
            if (FfrmInventoryLocationList == null || FfrmInventoryLocationList.IsDisposed)
            {
                FfrmInventoryLocationList = new frmInventoryLocationList();
                FfrmInventoryLocationList.Show();
            }
            else
            {
                FfrmInventoryLocationList.WindowState = FormWindowState.Maximized;
                FfrmInventoryLocationList.Activate();
            }
        }
        frmAdjustStockTake FfrmAdjustStockTake;
        private void btnAdjustStockTake_Click(object sender, EventArgs e)
        {
            if (FfrmAdjustStockTake == null || FfrmAdjustStockTake.IsDisposed)
            {
                FfrmAdjustStockTake = new frmAdjustStockTake();
                FfrmAdjustStockTake.Show();
            }
            else
            {
                FfrmAdjustStockTake.WindowState = FormWindowState.Maximized;
                FfrmAdjustStockTake.Activate();
            }
        }

        frmItemMst FfrmItemMst;
        private void btnProducts_Click(object sender, EventArgs e)
        {
            if (FfrmItemMst == null || FfrmItemMst.IsDisposed)
            {
                FfrmItemMst = new frmItemMst();
                FfrmItemMst.Show();
            }
            else
            {
                FfrmItemMst.WindowState = FormWindowState.Maximized;
                FfrmItemMst.Activate();
            }
        }

        frmCategoryList FfrmCategoryList;
        private void btnCategory_Click(object sender, EventArgs e)
        {
            if (FfrmCategoryList == null || FfrmCategoryList.IsDisposed)
            {
                FfrmCategoryList = new frmCategoryList();
                FfrmCategoryList.Show();
            }
            else
            {
                FfrmCategoryList.WindowState = FormWindowState.Maximized;
                FfrmCategoryList.Activate();
            }
        }
        frmStockOnHand FfrmStockOnHand;
        private void btnStockOnHand_Click_1(object sender, EventArgs e)
        {
            if (FfrmStockOnHand == null || FfrmStockOnHand.IsDisposed)
            {
                FfrmStockOnHand = new frmStockOnHand();
                FfrmStockOnHand.Show();
            }
            else
            {
                FfrmStockOnHand.WindowState = FormWindowState.Maximized;
                FfrmStockOnHand.Activate();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            frmSavedFiles f = new frmSavedFiles();
            CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            f.Dispose();
        }

        frmStockCardReport FfrmStockCardReport;
        private void btnStockCard_Click(object sender, EventArgs e)
        {
            if (FfrmStockCardReport == null || FfrmStockCardReport.IsDisposed)
            {
                FfrmStockCardReport = new frmStockCardReport();
                FfrmStockCardReport.Show();
            }
            else
            {
                FfrmStockCardReport.WindowState = FormWindowState.Maximized;
                FfrmStockCardReport.Activate();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmStockOnHand f = new frmStockOnHand();
            f.searchQueryString = txtProductSearch.Text;
            f.Show();
        }

        frmUserMst FfrmUserMst;
        private void btnUserSetup_Click(object sender, EventArgs e)
        {
            if (FfrmUserMst == null || FfrmUserMst.IsDisposed)
            {
                FfrmUserMst = new frmUserMst();
                FfrmUserMst.Show();
            }
            else
            {
                FfrmUserMst.WindowState = FormWindowState.Maximized;
                FfrmUserMst.Activate();
            }
        }
        frmChangePassword FfrmChangePassword;
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (FfrmChangePassword == null || FfrmChangePassword.IsDisposed)
            {
                FfrmChangePassword = new frmChangePassword();
                FfrmChangePassword.Show();
            }
            else
            {
                FfrmChangePassword.WindowState = FormWindowState.Maximized;
                FfrmChangePassword.Activate();
            }
        }

        private void txtProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }
        frmViewActivityDetail FfrmViewActivityLog;
        private void btnActivityLog_Click(object sender, EventArgs e)
        {
            if (FfrmViewActivityLog == null || FfrmViewActivityLog.IsDisposed)
            {
                FfrmViewActivityLog = new frmViewActivityDetail();
                FfrmViewActivityLog.Show();
            }
            else
            {
                FfrmViewActivityLog.WindowState = FormWindowState.Maximized;
                FfrmViewActivityLog.Activate();
            }
        }
        frmViewActivityHeader FfrmViewActivityHeader;
        private void btnActivityHeader_Click(object sender, EventArgs e)
        {
            if (FfrmViewActivityHeader == null || FfrmViewActivityHeader.IsDisposed)
            {
                FfrmViewActivityHeader = new frmViewActivityHeader();
                FfrmViewActivityHeader.Show();
            }
            else
            {
                FfrmViewActivityHeader.WindowState = FormWindowState.Maximized;
                FfrmViewActivityHeader.Activate();
            }
        }
        frmStockTakeReport FfrmStockTakeReport;
        private void btnStockTakeReport_Click(object sender, EventArgs e)
        {
            if (FfrmStockTakeReport == null || FfrmStockTakeReport.IsDisposed)
            {
                FfrmStockTakeReport = new frmStockTakeReport();
                FfrmStockTakeReport.Show();
            }
            else
            {
                FfrmStockTakeReport.WindowState = FormWindowState.Maximized;
                FfrmStockTakeReport.Activate();
            }
        }          
    }
}