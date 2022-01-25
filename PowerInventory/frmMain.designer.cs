namespace PowerInventory
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.btnLogOut = new System.Windows.Forms.Button();
			this.lblLocation = new System.Windows.Forms.Label();
			this.lblCashierName = new System.Windows.Forms.LinkLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.btnPreOrdered = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnSearch = new System.Windows.Forms.Button();
			this.txtProductSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbPOS = new System.Windows.Forms.TabControl();
			this.tbInventory = new System.Windows.Forms.TabPage();
			this.btnProducts = new System.Windows.Forms.Button();
			this.btnStockTransfer = new System.Windows.Forms.Button();
			this.btnStockAdjustment = new System.Windows.Forms.Button();
			this.btnStockIssue = new System.Windows.Forms.Button();
			this.btnGoodsReceive = new System.Windows.Forms.Button();
			this.tbStockTake = new System.Windows.Forms.TabPage();
			this.btnStockTakeReport = new System.Windows.Forms.Button();
			this.btnAdjustStockTake = new System.Windows.Forms.Button();
			this.btnStockTake = new System.Windows.Forms.Button();
			this.tbReports = new System.Windows.Forms.TabPage();
			this.btnViewActivityHeader = new System.Windows.Forms.Button();
			this.btnViewActivityLog = new System.Windows.Forms.Button();
			this.btnStockCard = new System.Windows.Forms.Button();
			this.btnStockOnHand = new System.Windows.Forms.Button();
			this.tbSetup = new System.Windows.Forms.TabPage();
			this.btnChangePassword = new System.Windows.Forms.Button();
			this.btnPrivilegeSetup = new System.Windows.Forms.Button();
			this.btnUserSetup = new System.Windows.Forms.Button();
			this.btnLocationSetup = new System.Windows.Forms.Button();
			this.btnChangeLocation = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.tbPOS.SuspendLayout();
			this.tbInventory.SuspendLayout();
			this.tbStockTake.SuspendLayout();
			this.tbReports.SuspendLayout();
			this.tbSetup.SuspendLayout();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
			// 
			// btnLogOut
			// 
			this.btnLogOut.BackColor = System.Drawing.Color.White;
			this.btnLogOut.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
			this.btnLogOut.ForeColor = System.Drawing.Color.White;
			this.btnLogOut.Location = new System.Drawing.Point(335, 432);
			this.btnLogOut.Margin = new System.Windows.Forms.Padding(4);
			this.btnLogOut.Name = "btnLogOut";
			this.btnLogOut.Size = new System.Drawing.Size(183, 48);
			this.btnLogOut.TabIndex = 4;
			this.btnLogOut.Text = "Log Out";
			this.btnLogOut.UseVisualStyleBackColor = false;
			this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
			// 
			// lblLocation
			// 
			this.lblLocation.AutoSize = true;
			this.lblLocation.Location = new System.Drawing.Point(4, 440);
			this.lblLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(14, 17);
			this.lblLocation.TabIndex = 13;
			this.lblLocation.Text = "-";
			// 
			// lblCashierName
			// 
			this.lblCashierName.AutoSize = true;
			this.lblCashierName.Location = new System.Drawing.Point(4, 421);
			this.lblCashierName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCashierName.Name = "lblCashierName";
			this.lblCashierName.Size = new System.Drawing.Size(14, 17);
			this.lblCashierName.TabIndex = 21;
			this.lblCashierName.TabStop = true;
			this.lblCashierName.Text = "-";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.White;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(-1, -1);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(933, 102);
			this.pictureBox1.TabIndex = 14;
			this.pictureBox1.TabStop = false;
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersion.Location = new System.Drawing.Point(4, 461);
			this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(34, 13);
			this.lblVersion.TabIndex = 24;
			this.lblVersion.Text = "v1.00";
			// 
			// btnPreOrdered
			// 
			this.btnPreOrdered.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPreOrdered.BackgroundImage")));
			this.btnPreOrdered.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPreOrdered.ForeColor = System.Drawing.Color.White;
			this.btnPreOrdered.Location = new System.Drawing.Point(901, 116);
			this.btnPreOrdered.Margin = new System.Windows.Forms.Padding(4);
			this.btnPreOrdered.Name = "btnPreOrdered";
			this.btnPreOrdered.Size = new System.Drawing.Size(140, 60);
			this.btnPreOrdered.TabIndex = 10;
			this.btnPreOrdered.Text = "PRE-ORDER SETUP";
			this.btnPreOrdered.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.panel1.BackColor = System.Drawing.Color.LightGray;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.btnLoad);
			this.panel1.Controls.Add(this.btnSearch);
			this.panel1.Controls.Add(this.txtProductSearch);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.tbPOS);
			this.panel1.Controls.Add(this.btnPreOrdered);
			this.panel1.Controls.Add(this.lblVersion);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Controls.Add(this.lblCashierName);
			this.panel1.Controls.Add(this.lblLocation);
			this.panel1.Controls.Add(this.btnLogOut);
			this.panel1.Location = new System.Drawing.Point(24, 9);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(524, 487);
			this.panel1.TabIndex = 22;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// btnLoad
			// 
			this.btnLoad.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoad.BackgroundImage")));
			this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnLoad.ForeColor = System.Drawing.Color.White;
			this.btnLoad.Location = new System.Drawing.Point(187, 432);
			this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(140, 48);
			this.btnLoad.TabIndex = 31;
			this.btnLoad.Text = "LOAD SAVED";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(382, 115);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 26);
			this.btnSearch.TabIndex = 28;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// txtProductSearch
			// 
			this.txtProductSearch.Location = new System.Drawing.Point(162, 115);
			this.txtProductSearch.Name = "txtProductSearch";
			this.txtProductSearch.Size = new System.Drawing.Size(213, 23);
			this.txtProductSearch.TabIndex = 27;
			this.txtProductSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductSearch_KeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(38, 120);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 17);
			this.label1.TabIndex = 26;
			this.label1.Text = "Product Inquiry";
			// 
			// tbPOS
			// 
			this.tbPOS.Controls.Add(this.tbInventory);
			this.tbPOS.Controls.Add(this.tbStockTake);
			this.tbPOS.Controls.Add(this.tbReports);
			this.tbPOS.Controls.Add(this.tbSetup);
			this.tbPOS.HotTrack = true;
			this.tbPOS.Location = new System.Drawing.Point(3, 158);
			this.tbPOS.Name = "tbPOS";
			this.tbPOS.SelectedIndex = 0;
			this.tbPOS.Size = new System.Drawing.Size(515, 260);
			this.tbPOS.TabIndex = 25;
			// 
			// tbInventory
			// 
			this.tbInventory.Controls.Add(this.btnProducts);
			this.tbInventory.Controls.Add(this.btnStockTransfer);
			this.tbInventory.Controls.Add(this.btnStockAdjustment);
			this.tbInventory.Controls.Add(this.btnStockIssue);
			this.tbInventory.Controls.Add(this.btnGoodsReceive);
			this.tbInventory.Location = new System.Drawing.Point(4, 25);
			this.tbInventory.Name = "tbInventory";
			this.tbInventory.Padding = new System.Windows.Forms.Padding(3);
			this.tbInventory.Size = new System.Drawing.Size(507, 231);
			this.tbInventory.TabIndex = 3;
			this.tbInventory.Text = "Inventory";
			this.tbInventory.UseVisualStyleBackColor = true;
			// 
			// btnProducts
			// 
			this.btnProducts.BackColor = System.Drawing.Color.White;
			this.btnProducts.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProducts.BackgroundImage")));
			this.btnProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnProducts.ForeColor = System.Drawing.Color.White;
			this.btnProducts.Location = new System.Drawing.Point(264, 72);
			this.btnProducts.Margin = new System.Windows.Forms.Padding(4);
			this.btnProducts.Name = "btnProducts";
			this.btnProducts.Size = new System.Drawing.Size(140, 60);
			this.btnProducts.TabIndex = 32;
			this.btnProducts.Text = "PRODUCT SETUP";
			this.btnProducts.UseVisualStyleBackColor = false;
			this.btnProducts.Visible = false;
			this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
			// 
			// btnStockTransfer
			// 
			this.btnStockTransfer.BackColor = System.Drawing.Color.White;
			this.btnStockTransfer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStockTransfer.BackgroundImage")));
			this.btnStockTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStockTransfer.ForeColor = System.Drawing.Color.White;
			this.btnStockTransfer.Location = new System.Drawing.Point(9, 140);
			this.btnStockTransfer.Margin = new System.Windows.Forms.Padding(4);
			this.btnStockTransfer.Name = "btnStockTransfer";
			this.btnStockTransfer.Size = new System.Drawing.Size(140, 60);
			this.btnStockTransfer.TabIndex = 25;
			this.btnStockTransfer.Text = "STOCK TRANSFER";
			this.btnStockTransfer.UseVisualStyleBackColor = false;
			this.btnStockTransfer.Click += new System.EventHandler(this.btnStockTransfer_Click);
			// 
			// btnStockAdjustment
			// 
			this.btnStockAdjustment.BackColor = System.Drawing.Color.White;
			this.btnStockAdjustment.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStockAdjustment.BackgroundImage")));
			this.btnStockAdjustment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStockAdjustment.ForeColor = System.Drawing.Color.White;
			this.btnStockAdjustment.Location = new System.Drawing.Point(264, 4);
			this.btnStockAdjustment.Margin = new System.Windows.Forms.Padding(4);
			this.btnStockAdjustment.Name = "btnStockAdjustment";
			this.btnStockAdjustment.Size = new System.Drawing.Size(140, 60);
			this.btnStockAdjustment.TabIndex = 20;
			this.btnStockAdjustment.Text = "STOCK ADJUSTMENT";
			this.btnStockAdjustment.UseVisualStyleBackColor = false;
			this.btnStockAdjustment.Click += new System.EventHandler(this.btnStockAdjustment_Click);
			// 
			// btnStockIssue
			// 
			this.btnStockIssue.BackColor = System.Drawing.Color.White;
			this.btnStockIssue.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStockIssue.BackgroundImage")));
			this.btnStockIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStockIssue.ForeColor = System.Drawing.Color.White;
			this.btnStockIssue.Location = new System.Drawing.Point(7, 72);
			this.btnStockIssue.Margin = new System.Windows.Forms.Padding(4);
			this.btnStockIssue.Name = "btnStockIssue";
			this.btnStockIssue.Size = new System.Drawing.Size(140, 60);
			this.btnStockIssue.TabIndex = 18;
			this.btnStockIssue.Text = "STOCK ISSUE";
			this.btnStockIssue.UseVisualStyleBackColor = false;
			this.btnStockIssue.Click += new System.EventHandler(this.btnStockIssue_Click_1);
			// 
			// btnGoodsReceive
			// 
			this.btnGoodsReceive.BackColor = System.Drawing.Color.White;
			this.btnGoodsReceive.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGoodsReceive.BackgroundImage")));
			this.btnGoodsReceive.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGoodsReceive.ForeColor = System.Drawing.Color.White;
			this.btnGoodsReceive.Location = new System.Drawing.Point(7, 4);
			this.btnGoodsReceive.Margin = new System.Windows.Forms.Padding(4);
			this.btnGoodsReceive.Name = "btnGoodsReceive";
			this.btnGoodsReceive.Size = new System.Drawing.Size(140, 60);
			this.btnGoodsReceive.TabIndex = 17;
			this.btnGoodsReceive.Text = "GOODS RECEIVE";
			this.btnGoodsReceive.UseVisualStyleBackColor = false;
			this.btnGoodsReceive.Click += new System.EventHandler(this.btnGoodsReceive_Click);
			// 
			// tbStockTake
			// 
			this.tbStockTake.Controls.Add(this.btnStockTakeReport);
			this.tbStockTake.Controls.Add(this.btnAdjustStockTake);
			this.tbStockTake.Controls.Add(this.btnStockTake);
			this.tbStockTake.Location = new System.Drawing.Point(4, 25);
			this.tbStockTake.Name = "tbStockTake";
			this.tbStockTake.Padding = new System.Windows.Forms.Padding(3);
			this.tbStockTake.Size = new System.Drawing.Size(507, 231);
			this.tbStockTake.TabIndex = 8;
			this.tbStockTake.Text = "Stock Take";
			this.tbStockTake.UseVisualStyleBackColor = true;
			// 
			// btnStockTakeReport
			// 
			this.btnStockTakeReport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStockTakeReport.BackgroundImage")));
			this.btnStockTakeReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStockTakeReport.ForeColor = System.Drawing.Color.White;
			this.btnStockTakeReport.Location = new System.Drawing.Point(180, 7);
			this.btnStockTakeReport.Margin = new System.Windows.Forms.Padding(4);
			this.btnStockTakeReport.Name = "btnStockTakeReport";
			this.btnStockTakeReport.Size = new System.Drawing.Size(140, 60);
			this.btnStockTakeReport.TabIndex = 33;
			this.btnStockTakeReport.Text = "STOCK TAKE REPORT";
			this.btnStockTakeReport.UseVisualStyleBackColor = true;
			this.btnStockTakeReport.Click += new System.EventHandler(this.btnStockTakeReport_Click);
			// 
			// btnAdjustStockTake
			// 
			this.btnAdjustStockTake.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdjustStockTake.BackgroundImage")));
			this.btnAdjustStockTake.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAdjustStockTake.ForeColor = System.Drawing.Color.White;
			this.btnAdjustStockTake.Location = new System.Drawing.Point(7, 75);
			this.btnAdjustStockTake.Margin = new System.Windows.Forms.Padding(4);
			this.btnAdjustStockTake.Name = "btnAdjustStockTake";
			this.btnAdjustStockTake.Size = new System.Drawing.Size(140, 60);
			this.btnAdjustStockTake.TabIndex = 32;
			this.btnAdjustStockTake.Text = "ADJUST STOCK TAKE";
			this.btnAdjustStockTake.UseVisualStyleBackColor = true;
			this.btnAdjustStockTake.Click += new System.EventHandler(this.btnAdjustStockTake_Click);
			// 
			// btnStockTake
			// 
			this.btnStockTake.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStockTake.BackgroundImage")));
			this.btnStockTake.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStockTake.ForeColor = System.Drawing.Color.White;
			this.btnStockTake.Location = new System.Drawing.Point(7, 7);
			this.btnStockTake.Margin = new System.Windows.Forms.Padding(4);
			this.btnStockTake.Name = "btnStockTake";
			this.btnStockTake.Size = new System.Drawing.Size(140, 60);
			this.btnStockTake.TabIndex = 31;
			this.btnStockTake.Text = "STOCK TAKE";
			this.btnStockTake.UseVisualStyleBackColor = true;
			this.btnStockTake.Click += new System.EventHandler(this.btnStockTake_Click);
			// 
			// tbReports
			// 
			this.tbReports.Controls.Add(this.btnViewActivityHeader);
			this.tbReports.Controls.Add(this.btnViewActivityLog);
			this.tbReports.Controls.Add(this.btnStockCard);
			this.tbReports.Controls.Add(this.btnStockOnHand);
			this.tbReports.Location = new System.Drawing.Point(4, 25);
			this.tbReports.Name = "tbReports";
			this.tbReports.Padding = new System.Windows.Forms.Padding(3);
			this.tbReports.Size = new System.Drawing.Size(507, 231);
			this.tbReports.TabIndex = 6;
			this.tbReports.Text = "Reports";
			this.tbReports.UseVisualStyleBackColor = true;
			// 
			// btnViewActivityHeader
			// 
			this.btnViewActivityHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnViewActivityHeader.BackgroundImage")));
			this.btnViewActivityHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnViewActivityHeader.ForeColor = System.Drawing.Color.White;
			this.btnViewActivityHeader.Location = new System.Drawing.Point(201, 7);
			this.btnViewActivityHeader.Margin = new System.Windows.Forms.Padding(4);
			this.btnViewActivityHeader.Name = "btnViewActivityHeader";
			this.btnViewActivityHeader.Size = new System.Drawing.Size(140, 60);
			this.btnViewActivityHeader.TabIndex = 27;
			this.btnViewActivityHeader.Text = "VIEW ACTIVITY";
			this.btnViewActivityHeader.UseVisualStyleBackColor = true;
			this.btnViewActivityHeader.Click += new System.EventHandler(this.btnActivityHeader_Click);
			// 
			// btnViewActivityLog
			// 
			this.btnViewActivityLog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnViewActivityLog.BackgroundImage")));
			this.btnViewActivityLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnViewActivityLog.ForeColor = System.Drawing.Color.White;
			this.btnViewActivityLog.Location = new System.Drawing.Point(201, 75);
			this.btnViewActivityLog.Margin = new System.Windows.Forms.Padding(4);
			this.btnViewActivityLog.Name = "btnViewActivityLog";
			this.btnViewActivityLog.Size = new System.Drawing.Size(140, 60);
			this.btnViewActivityLog.TabIndex = 26;
			this.btnViewActivityLog.Text = "VIEW ACTIVITY BY ITEM";
			this.btnViewActivityLog.UseVisualStyleBackColor = true;
			this.btnViewActivityLog.Click += new System.EventHandler(this.btnActivityLog_Click);
			// 
			// btnStockCard
			// 
			this.btnStockCard.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStockCard.BackgroundImage")));
			this.btnStockCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStockCard.ForeColor = System.Drawing.Color.White;
			this.btnStockCard.Location = new System.Drawing.Point(7, 75);
			this.btnStockCard.Margin = new System.Windows.Forms.Padding(4);
			this.btnStockCard.Name = "btnStockCard";
			this.btnStockCard.Size = new System.Drawing.Size(140, 60);
			this.btnStockCard.TabIndex = 25;
			this.btnStockCard.Text = "STOCK CARD";
			this.btnStockCard.UseVisualStyleBackColor = true;
			this.btnStockCard.Click += new System.EventHandler(this.btnStockCard_Click);
			// 
			// btnStockOnHand
			// 
			this.btnStockOnHand.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStockOnHand.BackgroundImage")));
			this.btnStockOnHand.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStockOnHand.ForeColor = System.Drawing.Color.White;
			this.btnStockOnHand.Location = new System.Drawing.Point(7, 7);
			this.btnStockOnHand.Margin = new System.Windows.Forms.Padding(4);
			this.btnStockOnHand.Name = "btnStockOnHand";
			this.btnStockOnHand.Size = new System.Drawing.Size(140, 60);
			this.btnStockOnHand.TabIndex = 24;
			this.btnStockOnHand.Text = "STOCK ON HAND";
			this.btnStockOnHand.UseVisualStyleBackColor = true;
			this.btnStockOnHand.Click += new System.EventHandler(this.btnStockOnHand_Click_1);
			// 
			// tbSetup
			// 
			this.tbSetup.Controls.Add(this.btnChangePassword);
			this.tbSetup.Controls.Add(this.btnPrivilegeSetup);
			this.tbSetup.Controls.Add(this.btnUserSetup);
			this.tbSetup.Controls.Add(this.btnLocationSetup);
			this.tbSetup.Controls.Add(this.btnChangeLocation);
			this.tbSetup.Location = new System.Drawing.Point(4, 25);
			this.tbSetup.Name = "tbSetup";
			this.tbSetup.Padding = new System.Windows.Forms.Padding(3);
			this.tbSetup.Size = new System.Drawing.Size(507, 231);
			this.tbSetup.TabIndex = 4;
			this.tbSetup.Text = "Setup";
			this.tbSetup.UseVisualStyleBackColor = true;
			// 
			// btnChangePassword
			// 
			this.btnChangePassword.BackColor = System.Drawing.Color.White;
			this.btnChangePassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnChangePassword.BackgroundImage")));
			this.btnChangePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnChangePassword.ForeColor = System.Drawing.Color.White;
			this.btnChangePassword.Location = new System.Drawing.Point(7, 143);
			this.btnChangePassword.Margin = new System.Windows.Forms.Padding(4);
			this.btnChangePassword.Name = "btnChangePassword";
			this.btnChangePassword.Size = new System.Drawing.Size(140, 60);
			this.btnChangePassword.TabIndex = 31;
			this.btnChangePassword.Text = "CHANGE PASSWORD";
			this.btnChangePassword.UseVisualStyleBackColor = false;
			this.btnChangePassword.Visible = false;
			this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
			// 
			// btnPrivilegeSetup
			// 
			this.btnPrivilegeSetup.BackColor = System.Drawing.Color.White;
			this.btnPrivilegeSetup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrivilegeSetup.BackgroundImage")));
			this.btnPrivilegeSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPrivilegeSetup.ForeColor = System.Drawing.Color.White;
			this.btnPrivilegeSetup.Location = new System.Drawing.Point(201, 75);
			this.btnPrivilegeSetup.Margin = new System.Windows.Forms.Padding(4);
			this.btnPrivilegeSetup.Name = "btnPrivilegeSetup";
			this.btnPrivilegeSetup.Size = new System.Drawing.Size(140, 60);
			this.btnPrivilegeSetup.TabIndex = 30;
			this.btnPrivilegeSetup.Text = "PRIVILEGE SETUP";
			this.btnPrivilegeSetup.UseVisualStyleBackColor = false;
			this.btnPrivilegeSetup.Visible = false;
			// 
			// btnUserSetup
			// 
			this.btnUserSetup.BackColor = System.Drawing.Color.White;
			this.btnUserSetup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUserSetup.BackgroundImage")));
			this.btnUserSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUserSetup.ForeColor = System.Drawing.Color.White;
			this.btnUserSetup.Location = new System.Drawing.Point(9, 75);
			this.btnUserSetup.Margin = new System.Windows.Forms.Padding(4);
			this.btnUserSetup.Name = "btnUserSetup";
			this.btnUserSetup.Size = new System.Drawing.Size(140, 60);
			this.btnUserSetup.TabIndex = 29;
			this.btnUserSetup.Text = "USER SETUP";
			this.btnUserSetup.UseVisualStyleBackColor = false;
			this.btnUserSetup.Visible = false;
			this.btnUserSetup.Click += new System.EventHandler(this.btnUserSetup_Click);
			// 
			// btnLocationSetup
			// 
			this.btnLocationSetup.BackColor = System.Drawing.Color.White;
			this.btnLocationSetup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLocationSetup.BackgroundImage")));
			this.btnLocationSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnLocationSetup.ForeColor = System.Drawing.Color.White;
			this.btnLocationSetup.Location = new System.Drawing.Point(7, 7);
			this.btnLocationSetup.Margin = new System.Windows.Forms.Padding(4);
			this.btnLocationSetup.Name = "btnLocationSetup";
			this.btnLocationSetup.Size = new System.Drawing.Size(140, 60);
			this.btnLocationSetup.TabIndex = 28;
			this.btnLocationSetup.Text = "SETUP LOCATIONS";
			this.btnLocationSetup.UseVisualStyleBackColor = false;
			this.btnLocationSetup.Click += new System.EventHandler(this.btnLocationSetup_Click);
			// 
			// btnChangeLocation
			// 
			this.btnChangeLocation.BackColor = System.Drawing.Color.White;
			this.btnChangeLocation.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnChangeLocation.BackgroundImage")));
			this.btnChangeLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnChangeLocation.ForeColor = System.Drawing.Color.White;
			this.btnChangeLocation.Location = new System.Drawing.Point(201, 7);
			this.btnChangeLocation.Margin = new System.Windows.Forms.Padding(4);
			this.btnChangeLocation.Name = "btnChangeLocation";
			this.btnChangeLocation.Size = new System.Drawing.Size(140, 60);
			this.btnChangeLocation.TabIndex = 8;
			this.btnChangeLocation.Text = "SETTINGS";
			this.btnChangeLocation.UseVisualStyleBackColor = false;
			this.btnChangeLocation.Visible = false;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::PowerInventory.Properties.Resources.longdarkbg1;
			this.ClientSize = new System.Drawing.Size(576, 518);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Power Inventory";
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tbPOS.ResumeLayout(false);
			this.tbInventory.ResumeLayout(false);
			this.tbStockTake.ResumeLayout(false);
			this.tbReports.ResumeLayout(false);
			this.tbSetup.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.LinkLabel lblCashierName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnPreOrdered;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tbPOS;
        private System.Windows.Forms.TabPage tbInventory;
        private System.Windows.Forms.TabPage tbSetup;
        private System.Windows.Forms.Button btnStockAdjustment;
        private System.Windows.Forms.Button btnStockIssue;
        private System.Windows.Forms.Button btnGoodsReceive;
        private System.Windows.Forms.Button btnStockTransfer;
        private System.Windows.Forms.Button btnChangeLocation;
        private System.Windows.Forms.Button btnLocationSetup;
        private System.Windows.Forms.TabPage tbReports;
        private System.Windows.Forms.Button btnStockCard;
        private System.Windows.Forms.Button btnStockOnHand;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.TabPage tbStockTake;
        private System.Windows.Forms.Button btnAdjustStockTake;
        private System.Windows.Forms.Button btnStockTake;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Button btnPrivilegeSetup;
        private System.Windows.Forms.Button btnUserSetup;
        private System.Windows.Forms.Button btnViewActivityLog;
        private System.Windows.Forms.Button btnViewActivityHeader;
        private System.Windows.Forms.Button btnStockTakeReport;
    }
}