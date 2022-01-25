namespace PowerInventory
{
    partial class frmPurchaseOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPurchaseOrder));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tbControl = new System.Windows.Forms.TabControl();
            this.tbHeader = new System.Windows.Forms.TabPage();
            this.lblGrandTotalValue = new System.Windows.Forms.Label();
            this.lblGSTValue = new System.Windows.Forms.Label();
            this.lblSubtotalValue = new System.Windows.Forms.Label();
            this.lblDeliveryChargeValue = new System.Windows.Forms.Label();
            this.lblOrderAmountValue = new System.Windows.Forms.Label();
            this.lblGrandTotal1 = new System.Windows.Forms.Label();
            this.lblGST1 = new System.Windows.Forms.Label();
            this.lblSubtotal1 = new System.Windows.Forms.Label();
            this.lblDeliveryCharge1 = new System.Windows.Forms.Label();
            this.lblOrderAmount1 = new System.Windows.Forms.Label();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.tblInventory = new System.Windows.Forms.TableLayoutPanel();
            this.lblGST = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpInventoryDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.lblPaymentTerm = new System.Windows.Forms.Label();
            this.txtPaymentTerm = new System.Windows.Forms.TextBox();
            this.lblDeliveryAddress = new System.Windows.Forms.Label();
            this.txtDeliveryAddress = new System.Windows.Forms.TextBox();
            this.txtDeliveryDate = new System.Windows.Forms.DateTimePicker();
            this.lblDeliveryDate = new System.Windows.Forms.Label();
            this.lblReceivingTime = new System.Windows.Forms.Label();
            this.txtReceivingTime = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbGST = new System.Windows.Forms.ComboBox();
            this.lblMinPurchase = new System.Windows.Forms.Label();
            this.txtMinPurchase = new System.Windows.Forms.TextBox();
            this.lblDeliveryCharge = new System.Windows.Forms.Label();
            this.txtDeliveryCharge = new System.Windows.Forms.TextBox();
            this.cmbCurrencies = new System.Windows.Forms.ComboBox();
            this.lblCurrencies = new System.Windows.Forms.Label();
            this.btnLoadAddress = new System.Windows.Forms.Button();
            this.tbBody = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDefaultQty = new System.Windows.Forms.TextBox();
            this.cbDefaultQty = new System.Windows.Forms.CheckBox();
            this.lblTotalCostPriceAmount = new System.Windows.Forms.Label();
            this.lblTotalCostPrice = new System.Windows.Forms.Label();
            this.llInvert = new System.Windows.Forms.LinkLabel();
            this.llSelectNone = new System.Windows.Forms.LinkLabel();
            this.llSelectAll = new System.Windows.Forms.LinkLabel();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnScanItemNo = new System.Windows.Forms.Button();
            this.txtItemNoBarcode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.SN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OnHand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuggestedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackingSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChoosePackingSize = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Currency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostPerPackingSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactoryPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PurchaseOrderDetRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLoadingMessage = new System.Windows.Forms.Label();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.saveProduct = new System.Windows.Forms.SaveFileDialog();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSaveToDisk = new System.Windows.Forms.Button();
            this.btnDeleteChecked = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnImport1 = new System.Windows.Forms.Button();
            this.tbControl.SuspendLayout();
            this.tbHeader.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.tblInventory.SuspendLayout();
            this.tbBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.pnlLoading.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbControl
            // 
            resources.ApplyResources(this.tbControl, "tbControl");
            this.tbControl.Controls.Add(this.tbHeader);
            this.tbControl.Controls.Add(this.tbBody);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 0;
            this.tbControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            // 
            // tbHeader
            // 
            this.tbHeader.BackColor = System.Drawing.Color.Gainsboro;
            this.tbHeader.Controls.Add(this.lblGrandTotalValue);
            this.tbHeader.Controls.Add(this.lblGSTValue);
            this.tbHeader.Controls.Add(this.lblSubtotalValue);
            this.tbHeader.Controls.Add(this.lblDeliveryChargeValue);
            this.tbHeader.Controls.Add(this.lblOrderAmountValue);
            this.tbHeader.Controls.Add(this.lblGrandTotal1);
            this.tbHeader.Controls.Add(this.lblGST1);
            this.tbHeader.Controls.Add(this.lblSubtotal1);
            this.tbHeader.Controls.Add(this.lblDeliveryCharge1);
            this.tbHeader.Controls.Add(this.lblOrderAmount1);
            this.tbHeader.Controls.Add(this.pnlProgress);
            this.tbHeader.Controls.Add(this.tblInventory);
            resources.ApplyResources(this.tbHeader, "tbHeader");
            this.tbHeader.Name = "tbHeader";
            // 
            // lblGrandTotalValue
            // 
            resources.ApplyResources(this.lblGrandTotalValue, "lblGrandTotalValue");
            this.lblGrandTotalValue.Name = "lblGrandTotalValue";
            // 
            // lblGSTValue
            // 
            resources.ApplyResources(this.lblGSTValue, "lblGSTValue");
            this.lblGSTValue.Name = "lblGSTValue";
            // 
            // lblSubtotalValue
            // 
            resources.ApplyResources(this.lblSubtotalValue, "lblSubtotalValue");
            this.lblSubtotalValue.Name = "lblSubtotalValue";
            // 
            // lblDeliveryChargeValue
            // 
            resources.ApplyResources(this.lblDeliveryChargeValue, "lblDeliveryChargeValue");
            this.lblDeliveryChargeValue.Name = "lblDeliveryChargeValue";
            this.lblDeliveryChargeValue.Click += new System.EventHandler(this.label17_Click);
            // 
            // lblOrderAmountValue
            // 
            resources.ApplyResources(this.lblOrderAmountValue, "lblOrderAmountValue");
            this.lblOrderAmountValue.Name = "lblOrderAmountValue";
            this.lblOrderAmountValue.Click += new System.EventHandler(this.lblOrderAmount_Click);
            // 
            // lblGrandTotal1
            // 
            resources.ApplyResources(this.lblGrandTotal1, "lblGrandTotal1");
            this.lblGrandTotal1.Name = "lblGrandTotal1";
            // 
            // lblGST1
            // 
            resources.ApplyResources(this.lblGST1, "lblGST1");
            this.lblGST1.Name = "lblGST1";
            // 
            // lblSubtotal1
            // 
            resources.ApplyResources(this.lblSubtotal1, "lblSubtotal1");
            this.lblSubtotal1.Name = "lblSubtotal1";
            // 
            // lblDeliveryCharge1
            // 
            resources.ApplyResources(this.lblDeliveryCharge1, "lblDeliveryCharge1");
            this.lblDeliveryCharge1.Name = "lblDeliveryCharge1";
            // 
            // lblOrderAmount1
            // 
            resources.ApplyResources(this.lblOrderAmount1, "lblOrderAmount1");
            this.lblOrderAmount1.Name = "lblOrderAmount1";
            // 
            // pnlProgress
            // 
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.label9);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Name = "pnlProgress";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Name = "label9";
            // 
            // pgb1
            // 
            resources.ApplyResources(this.pgb1, "pgb1");
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // tblInventory
            // 
            resources.ApplyResources(this.tblInventory, "tblInventory");
            this.tblInventory.BackColor = System.Drawing.Color.Gainsboro;
            this.tblInventory.Controls.Add(this.lblGST, 4, 2);
            this.tblInventory.Controls.Add(this.label2, 0, 0);
            this.tblInventory.Controls.Add(this.label7, 3, 1);
            this.tblInventory.Controls.Add(this.label4, 0, 1);
            this.tblInventory.Controls.Add(this.txtRefNo, 1, 1);
            this.tblInventory.Controls.Add(this.label6, 0, 3);
            this.tblInventory.Controls.Add(this.dtpInventoryDate, 4, 1);
            this.tblInventory.Controls.Add(this.label5, 3, 2);
            this.tblInventory.Controls.Add(this.cmbLocation, 1, 2);
            this.tblInventory.Controls.Add(this.label1, 0, 2);
            this.tblInventory.Controls.Add(this.lblSupplier, 0, 4);
            this.tblInventory.Controls.Add(this.cmbSupplier, 1, 4);
            this.tblInventory.Controls.Add(this.lblPaymentTerm, 3, 8);
            this.tblInventory.Controls.Add(this.txtPaymentTerm, 4, 8);
            this.tblInventory.Controls.Add(this.lblDeliveryAddress, 0, 8);
            this.tblInventory.Controls.Add(this.txtDeliveryAddress, 1, 8);
            this.tblInventory.Controls.Add(this.txtDeliveryDate, 4, 4);
            this.tblInventory.Controls.Add(this.lblDeliveryDate, 3, 4);
            this.tblInventory.Controls.Add(this.lblReceivingTime, 3, 5);
            this.tblInventory.Controls.Add(this.txtReceivingTime, 4, 5);
            this.tblInventory.Controls.Add(this.label11, 0, 5);
            this.tblInventory.Controls.Add(this.cmbGST, 1, 5);
            this.tblInventory.Controls.Add(this.lblMinPurchase, 0, 6);
            this.tblInventory.Controls.Add(this.txtMinPurchase, 1, 6);
            this.tblInventory.Controls.Add(this.lblDeliveryCharge, 3, 6);
            this.tblInventory.Controls.Add(this.txtDeliveryCharge, 4, 6);
            this.tblInventory.Controls.Add(this.cmbCurrencies, 1, 7);
            this.tblInventory.Controls.Add(this.lblCurrencies, 0, 7);
            this.tblInventory.Controls.Add(this.btnLoadAddress, 2, 8);
            this.tblInventory.Name = "tblInventory";
            this.tblInventory.Paint += new System.Windows.Forms.PaintEventHandler(this.tblInventory_Paint);
            // 
            // lblGST
            // 
            resources.ApplyResources(this.lblGST, "lblGST");
            this.lblGST.Name = "lblGST";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Navy;
            this.tblInventory.SetColumnSpan(this.label2, 5);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Gainsboro;
            this.label7.Name = "label7";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Gainsboro;
            this.label4.Name = "label4";
            // 
            // txtRefNo
            // 
            resources.ApplyResources(this.txtRefNo, "txtRefNo");
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Navy;
            this.tblInventory.SetColumnSpan(this.label6, 5);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // dtpInventoryDate
            // 
            resources.ApplyResources(this.dtpInventoryDate, "dtpInventoryDate");
            this.dtpInventoryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInventoryDate.Name = "dtpInventoryDate";
            this.dtpInventoryDate.ValueChanged += new System.EventHandler(this.dtpInventoryDate_ValueChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Gainsboro;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // cmbLocation
            // 
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Name = "label1";
            // 
            // lblSupplier
            // 
            resources.ApplyResources(this.lblSupplier, "lblSupplier");
            this.lblSupplier.Name = "lblSupplier";
            // 
            // cmbSupplier
            // 
            resources.ApplyResources(this.cmbSupplier, "cmbSupplier");
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.SelectedValueChanged += new System.EventHandler(this.cmbSupplier_SelectedValueChanged);
            // 
            // lblPaymentTerm
            // 
            resources.ApplyResources(this.lblPaymentTerm, "lblPaymentTerm");
            this.lblPaymentTerm.Name = "lblPaymentTerm";
            // 
            // txtPaymentTerm
            // 
            resources.ApplyResources(this.txtPaymentTerm, "txtPaymentTerm");
            this.txtPaymentTerm.Name = "txtPaymentTerm";
            // 
            // lblDeliveryAddress
            // 
            resources.ApplyResources(this.lblDeliveryAddress, "lblDeliveryAddress");
            this.lblDeliveryAddress.Name = "lblDeliveryAddress";
            // 
            // txtDeliveryAddress
            // 
            resources.ApplyResources(this.txtDeliveryAddress, "txtDeliveryAddress");
            this.txtDeliveryAddress.Name = "txtDeliveryAddress";
            // 
            // txtDeliveryDate
            // 
            resources.ApplyResources(this.txtDeliveryDate, "txtDeliveryDate");
            this.txtDeliveryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtDeliveryDate.Name = "txtDeliveryDate";
            // 
            // lblDeliveryDate
            // 
            resources.ApplyResources(this.lblDeliveryDate, "lblDeliveryDate");
            this.lblDeliveryDate.Name = "lblDeliveryDate";
            // 
            // lblReceivingTime
            // 
            resources.ApplyResources(this.lblReceivingTime, "lblReceivingTime");
            this.lblReceivingTime.Name = "lblReceivingTime";
            // 
            // txtReceivingTime
            // 
            resources.ApplyResources(this.txtReceivingTime, "txtReceivingTime");
            this.txtReceivingTime.Name = "txtReceivingTime";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // cmbGST
            // 
            resources.ApplyResources(this.cmbGST, "cmbGST");
            this.cmbGST.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGST.FormattingEnabled = true;
            this.cmbGST.Name = "cmbGST";
            this.cmbGST.SelectedValueChanged += new System.EventHandler(this.cmbGST_SelectedValueChanged);
            // 
            // lblMinPurchase
            // 
            resources.ApplyResources(this.lblMinPurchase, "lblMinPurchase");
            this.lblMinPurchase.Name = "lblMinPurchase";
            // 
            // txtMinPurchase
            // 
            resources.ApplyResources(this.txtMinPurchase, "txtMinPurchase");
            this.txtMinPurchase.Name = "txtMinPurchase";
            this.txtMinPurchase.Validated += new System.EventHandler(this.txtMinPurchase_Validated);
            // 
            // lblDeliveryCharge
            // 
            resources.ApplyResources(this.lblDeliveryCharge, "lblDeliveryCharge");
            this.lblDeliveryCharge.Name = "lblDeliveryCharge";
            // 
            // txtDeliveryCharge
            // 
            resources.ApplyResources(this.txtDeliveryCharge, "txtDeliveryCharge");
            this.txtDeliveryCharge.Name = "txtDeliveryCharge";
            this.txtDeliveryCharge.Validated += new System.EventHandler(this.txtDeliveryCharge_Validated);
            // 
            // cmbCurrencies
            // 
            resources.ApplyResources(this.cmbCurrencies, "cmbCurrencies");
            this.cmbCurrencies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurrencies.FormattingEnabled = true;
            this.cmbCurrencies.Name = "cmbCurrencies";
            // 
            // lblCurrencies
            // 
            resources.ApplyResources(this.lblCurrencies, "lblCurrencies");
            this.lblCurrencies.Name = "lblCurrencies";
            // 
            // btnLoadAddress
            // 
            resources.ApplyResources(this.btnLoadAddress, "btnLoadAddress");
            this.btnLoadAddress.Name = "btnLoadAddress";
            this.btnLoadAddress.UseVisualStyleBackColor = true;
            this.btnLoadAddress.Click += new System.EventHandler(this.btnMembership_Click);
            // 
            // tbBody
            // 
            this.tbBody.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgnd;
            this.tbBody.Controls.Add(this.label10);
            this.tbBody.Controls.Add(this.txtDefaultQty);
            this.tbBody.Controls.Add(this.cbDefaultQty);
            this.tbBody.Controls.Add(this.lblTotalCostPriceAmount);
            this.tbBody.Controls.Add(this.lblTotalCostPrice);
            this.tbBody.Controls.Add(this.llInvert);
            this.tbBody.Controls.Add(this.llSelectNone);
            this.tbBody.Controls.Add(this.llSelectAll);
            this.tbBody.Controls.Add(this.btnAddItem);
            this.tbBody.Controls.Add(this.btnScanItemNo);
            this.tbBody.Controls.Add(this.txtItemNoBarcode);
            this.tbBody.Controls.Add(this.label8);
            this.tbBody.Controls.Add(this.dgvStock);
            resources.ApplyResources(this.tbBody, "tbBody");
            this.tbBody.Name = "tbBody";
            this.tbBody.UseVisualStyleBackColor = true;
            this.tbBody.Enter += new System.EventHandler(this.tbBody_Enter);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // txtDefaultQty
            // 
            resources.ApplyResources(this.txtDefaultQty, "txtDefaultQty");
            this.txtDefaultQty.Name = "txtDefaultQty";
            // 
            // cbDefaultQty
            // 
            resources.ApplyResources(this.cbDefaultQty, "cbDefaultQty");
            this.cbDefaultQty.Name = "cbDefaultQty";
            this.cbDefaultQty.UseVisualStyleBackColor = true;
            // 
            // lblTotalCostPriceAmount
            // 
            resources.ApplyResources(this.lblTotalCostPriceAmount, "lblTotalCostPriceAmount");
            this.lblTotalCostPriceAmount.Name = "lblTotalCostPriceAmount";
            // 
            // lblTotalCostPrice
            // 
            resources.ApplyResources(this.lblTotalCostPrice, "lblTotalCostPrice");
            this.lblTotalCostPrice.Name = "lblTotalCostPrice";
            // 
            // llInvert
            // 
            resources.ApplyResources(this.llInvert, "llInvert");
            this.llInvert.BackColor = System.Drawing.Color.Transparent;
            this.llInvert.Name = "llInvert";
            this.llInvert.TabStop = true;
            this.llInvert.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llInvert.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llInvert_LinkClicked);
            // 
            // llSelectNone
            // 
            resources.ApplyResources(this.llSelectNone, "llSelectNone");
            this.llSelectNone.BackColor = System.Drawing.Color.Transparent;
            this.llSelectNone.Name = "llSelectNone";
            this.llSelectNone.TabStop = true;
            this.llSelectNone.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llSelectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectNone_LinkClicked);
            // 
            // llSelectAll
            // 
            resources.ApplyResources(this.llSelectAll, "llSelectAll");
            this.llSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.llSelectAll.Name = "llSelectAll";
            this.llSelectAll.TabStop = true;
            this.llSelectAll.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectAll_LinkClicked);
            // 
            // btnAddItem
            // 
            resources.ApplyResources(this.btnAddItem, "btnAddItem");
            this.btnAddItem.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnAddItem.ForeColor = System.Drawing.Color.White;
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnScanItemNo
            // 
            this.btnScanItemNo.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnScanItemNo, "btnScanItemNo");
            this.btnScanItemNo.ForeColor = System.Drawing.Color.White;
            this.btnScanItemNo.Name = "btnScanItemNo";
            this.btnScanItemNo.UseVisualStyleBackColor = true;
            this.btnScanItemNo.Click += new System.EventHandler(this.btnScanItemNo_Click);
            // 
            // txtItemNoBarcode
            // 
            resources.ApplyResources(this.txtItemNoBarcode, "txtItemNoBarcode");
            this.txtItemNoBarcode.Name = "txtItemNoBarcode";
            this.txtItemNoBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItemNoBarcode_KeyDown);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            this.dgvStock.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvStock, "dgvStock");
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvStock.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SN,
            this.ItemNo,
            this.ItemName,
            this.OnHand,
            this.SuggestedQty,
            this.GST,
            this.PackingSize,
            this.ChoosePackingSize,
            this.Quantity,
            this.UOM,
            this.Currency,
            this.CostPerPackingSize,
            this.FactoryPrice,
            this.RetailPrice,
            this.TotalCostPrice,
            this.ItemDesc,
            this.Remark,
            this.PurchaseOrderDetRefNo});
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.TabStop = false;
            this.dgvStock.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStock_CellMouseClick);
            this.dgvStock.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvStock_RowPrePaint);
            this.dgvStock.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStock_CellClick);
            this.dgvStock.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStock_CellContentClick);
            // 
            // SN
            // 
            this.SN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SN.DataPropertyName = "Deleted";
            this.SN.FalseValue = "false";
            resources.ApplyResources(this.SN, "SN");
            this.SN.IndeterminateValue = "false";
            this.SN.Name = "SN";
            this.SN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SN.TrueValue = "true";
            // 
            // ItemNo
            // 
            this.ItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.FillWeight = 1F;
            resources.ApplyResources(this.ItemNo, "ItemNo");
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            this.ItemNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ItemName.FillWeight = 14.71503F;
            resources.ApplyResources(this.ItemName, "ItemName");
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            this.ItemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // OnHand
            // 
            this.OnHand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OnHand.DataPropertyName = "OnHand";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.OnHand.DefaultCellStyle = dataGridViewCellStyle2;
            this.OnHand.FillWeight = 1F;
            resources.ApplyResources(this.OnHand, "OnHand");
            this.OnHand.Name = "OnHand";
            this.OnHand.ReadOnly = true;
            this.OnHand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SuggestedQty
            // 
            this.SuggestedQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SuggestedQty.DataPropertyName = "SuggestedQty";
            resources.ApplyResources(this.SuggestedQty, "SuggestedQty");
            this.SuggestedQty.Name = "SuggestedQty";
            this.SuggestedQty.ReadOnly = true;
            this.SuggestedQty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // GST
            // 
            this.GST.DataPropertyName = "GST";
            resources.ApplyResources(this.GST, "GST");
            this.GST.Name = "GST";
            this.GST.ReadOnly = true;
            // 
            // PackingSize
            // 
            this.PackingSize.DataPropertyName = "Packing";
            resources.ApplyResources(this.PackingSize, "PackingSize");
            this.PackingSize.Name = "PackingSize";
            // 
            // ChoosePackingSize
            // 
            resources.ApplyResources(this.ChoosePackingSize, "ChoosePackingSize");
            this.ChoosePackingSize.Name = "ChoosePackingSize";
            this.ChoosePackingSize.UseColumnTextForButtonValue = true;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Quantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Quantity, "Quantity");
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            resources.ApplyResources(this.UOM, "UOM");
            this.UOM.Name = "UOM";
            // 
            // Currency
            // 
            this.Currency.DataPropertyName = "Currency";
            resources.ApplyResources(this.Currency, "Currency");
            this.Currency.Name = "Currency";
            // 
            // CostPerPackingSize
            // 
            this.CostPerPackingSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.CostPerPackingSize.DataPropertyName = "CostPerPackingSize";
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CostPerPackingSize.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.CostPerPackingSize, "CostPerPackingSize");
            this.CostPerPackingSize.Name = "CostPerPackingSize";
            this.CostPerPackingSize.ReadOnly = true;
            this.CostPerPackingSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FactoryPrice
            // 
            this.FactoryPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.FactoryPrice.DataPropertyName = "FactoryPrice";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = "0";
            this.FactoryPrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.FactoryPrice.FillWeight = 14.71503F;
            resources.ApplyResources(this.FactoryPrice, "FactoryPrice");
            this.FactoryPrice.Name = "FactoryPrice";
            this.FactoryPrice.ReadOnly = true;
            this.FactoryPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RetailPrice
            // 
            this.RetailPrice.DataPropertyName = "RetailPrice";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = "0";
            this.RetailPrice.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.RetailPrice, "RetailPrice");
            this.RetailPrice.Name = "RetailPrice";
            this.RetailPrice.ReadOnly = true;
            // 
            // TotalCostPrice
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = "0";
            this.TotalCostPrice.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.TotalCostPrice, "TotalCostPrice");
            this.TotalCostPrice.Name = "TotalCostPrice";
            this.TotalCostPrice.ReadOnly = true;
            // 
            // ItemDesc
            // 
            this.ItemDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDesc.DataPropertyName = "ItemDesc";
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemDesc.DefaultCellStyle = dataGridViewCellStyle8;
            this.ItemDesc.FillWeight = 14.71503F;
            resources.ApplyResources(this.ItemDesc, "ItemDesc");
            this.ItemDesc.Name = "ItemDesc";
            this.ItemDesc.ReadOnly = true;
            this.ItemDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Remark.DataPropertyName = "Remark";
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Remark.DefaultCellStyle = dataGridViewCellStyle9;
            this.Remark.FillWeight = 14.71503F;
            resources.ApplyResources(this.Remark, "Remark");
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            this.Remark.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PurchaseOrderDetRefNo
            // 
            this.PurchaseOrderDetRefNo.DataPropertyName = "PurchaseOrderDetRefNo";
            resources.ApplyResources(this.PurchaseOrderDetRefNo, "PurchaseOrderDetRefNo");
            this.PurchaseOrderDetRefNo.Name = "PurchaseOrderDetRefNo";
            this.PurchaseOrderDetRefNo.ReadOnly = true;
            this.PurchaseOrderDetRefNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pnlLoading
            // 
            this.pnlLoading.BackColor = System.Drawing.Color.Yellow;
            this.pnlLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoading.Controls.Add(this.label3);
            this.pnlLoading.Controls.Add(this.lblLoadingMessage);
            resources.ApplyResources(this.pnlLoading, "pnlLoading");
            this.pnlLoading.Name = "pnlLoading";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lblLoadingMessage
            // 
            resources.ApplyResources(this.lblLoadingMessage, "lblLoadingMessage");
            this.lblLoadingMessage.Name = "lblLoadingMessage";
            // 
            // saveFileDialogExport
            // 
            this.saveFileDialogExport.DefaultExt = "csv";
            resources.ApplyResources(this.saveFileDialogExport, "saveFileDialogExport");
            this.saveFileDialogExport.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogExport_FileOk);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.csv";
            this.openFileDialog1.FileName = "openFileDialog1";
            resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // bgSearch
            // 
            this.bgSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSearch_DoWork);
            this.bgSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgSearch_RunWorkerCompleted);
            this.bgSearch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // saveProduct
            // 
            this.saveProduct.DefaultExt = "csv";
            resources.ApplyResources(this.saveProduct, "saveProduct");
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSaveToDisk
            // 
            resources.ApplyResources(this.btnSaveToDisk, "btnSaveToDisk");
            this.btnSaveToDisk.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSaveToDisk.ForeColor = System.Drawing.Color.White;
            this.btnSaveToDisk.Name = "btnSaveToDisk";
            this.btnSaveToDisk.UseVisualStyleBackColor = true;
            this.btnSaveToDisk.Click += new System.EventHandler(this.btnSaveToDisk_Click);
            // 
            // btnDeleteChecked
            // 
            resources.ApplyResources(this.btnDeleteChecked, "btnDeleteChecked");
            this.btnDeleteChecked.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnDeleteChecked.ForeColor = System.Drawing.Color.White;
            this.btnDeleteChecked.Name = "btnDeleteChecked";
            this.btnDeleteChecked.TabStop = false;
            this.btnDeleteChecked.UseVisualStyleBackColor = true;
            this.btnDeleteChecked.Click += new System.EventHandler(this.btnDeleteChecked_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSetting
            // 
            resources.ApplyResources(this.btnSetting, "btnSetting");
            this.btnSetting.BackgroundImage = global::PowerInventory.Properties.Resources.lightorange;
            this.btnSetting.ForeColor = System.Drawing.Color.Black;
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnImport1
            // 
            resources.ApplyResources(this.btnImport1, "btnImport1");
            this.btnImport1.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnImport1.ForeColor = System.Drawing.Color.White;
            this.btnImport1.Name = "btnImport1";
            this.btnImport1.UseVisualStyleBackColor = true;
            this.btnImport1.Click += new System.EventHandler(this.btnImport1_Click);
            // 
            // frmPurchaseOrder
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.btnImport1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnSaveToDisk);
            this.Controls.Add(this.btnDeleteChecked);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbControl);
            this.Controls.Add(this.pnlLoading);
            this.Controls.Add(this.btnSetting);
            this.DoubleBuffered = true;
            this.Name = "frmPurchaseOrder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmInventoryParent_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInventoryParent_FormClosed);
            this.tbControl.ResumeLayout(false);
            this.tbHeader.ResumeLayout(false);
            this.tbHeader.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.tblInventory.ResumeLayout(false);
            this.tblInventory.PerformLayout();
            this.tbBody.ResumeLayout(false);
            this.tbBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.pnlLoading.ResumeLayout(false);
            this.pnlLoading.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tbHeader;
        private System.Windows.Forms.TabPage tbBody;
        protected System.Windows.Forms.TabControl tbControl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnScanItemNo;
        private System.Windows.Forms.TextBox txtItemNoBarcode;
        protected System.Windows.Forms.TextBox lblGST;
        protected System.Windows.Forms.DateTimePicker dtpInventoryDate;
        protected System.Windows.Forms.TextBox txtRefNo;
        protected System.Windows.Forms.ComboBox cmbLocation;
        protected System.Windows.Forms.Button btnSave;
        protected System.Windows.Forms.TableLayoutPanel tblInventory;
        protected System.Windows.Forms.Button btnDeleteChecked;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        protected System.Windows.Forms.DataGridView dgvStock;
        protected System.Windows.Forms.Button btnSaveToDisk;
        protected System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.LinkLabel llInvert;
        private System.Windows.Forms.LinkLabel llSelectNone;
        private System.Windows.Forms.LinkLabel llSelectAll;
        private System.Windows.Forms.Label lblLoadingMessage;
        protected System.Windows.Forms.Panel pnlLoading;
        protected System.Windows.Forms.Button btnExport;
        protected System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ProgressBar pgb1;
        protected System.ComponentModel.BackgroundWorker bgSearch;
        protected System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog saveProduct;
        protected System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Label lblTotalCostPriceAmount;
        private System.Windows.Forms.Label lblTotalCostPrice;
        protected System.Windows.Forms.Button btnImport1;
        protected System.Windows.Forms.Label label10;
        protected System.Windows.Forms.TextBox txtDefaultQty;
        protected System.Windows.Forms.CheckBox cbDefaultQty;
        private System.Windows.Forms.Label lblSupplier;
        protected System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label lblDeliveryDate;
        private System.Windows.Forms.Label lblPaymentTerm;
        protected System.Windows.Forms.TextBox txtDeliveryAddress;
        protected System.Windows.Forms.TextBox txtPaymentTerm;
        protected System.Windows.Forms.DateTimePicker txtDeliveryDate;
        private System.Windows.Forms.Label lblDeliveryAddress;
        private System.Windows.Forms.Label lblReceivingTime;
        protected System.Windows.Forms.TextBox txtReceivingTime;
        private System.Windows.Forms.Label label11;
        protected System.Windows.Forms.ComboBox cmbGST;
        private System.Windows.Forms.Label lblMinPurchase;
        protected System.Windows.Forms.TextBox txtMinPurchase;
        private System.Windows.Forms.Label lblDeliveryCharge;
        protected System.Windows.Forms.TextBox txtDeliveryCharge;
        private System.Windows.Forms.Label lblOrderAmountValue;
        private System.Windows.Forms.Label lblGrandTotal1;
        private System.Windows.Forms.Label lblGST1;
        private System.Windows.Forms.Label lblSubtotal1;
        private System.Windows.Forms.Label lblDeliveryCharge1;
        private System.Windows.Forms.Label lblOrderAmount1;
        private System.Windows.Forms.Label lblDeliveryChargeValue;
        private System.Windows.Forms.Label lblGrandTotalValue;
        private System.Windows.Forms.Label lblGSTValue;
        private System.Windows.Forms.Label lblSubtotalValue;
        protected System.Windows.Forms.ComboBox cmbCurrencies;
        private System.Windows.Forms.Label lblCurrencies;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OnHand;
        private System.Windows.Forms.DataGridViewTextBoxColumn SuggestedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn GST;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackingSize;
        private System.Windows.Forms.DataGridViewButtonColumn ChoosePackingSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Currency;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostPerPackingSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactoryPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn RetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCostPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchaseOrderDetRefNo;
        private System.Windows.Forms.Button btnLoadAddress;
    }
}