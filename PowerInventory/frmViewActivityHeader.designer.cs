namespace PowerInventory.InventoryForms
{
    partial class frmViewActivityHeader
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewActivityHeader));
            this.epOrder = new System.Windows.Forms.ErrorProvider(this.components);
            this.fsdExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.btnExport = new System.Windows.Forms.Button();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbTransferTo = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbMovementType = new System.Windows.Forms.ComboBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.bgSearch = new System.ComponentModel.BackgroundWorker();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.dgvPurchase = new FixReEntrantDataGridView();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivityType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReasonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryLocationCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransferToFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransferOutRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bgDownload = new System.ComponentModel.BackgroundWorker();
            this.txtRefno = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.epOrder)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).BeginInit();
            this.SuspendLayout();
            // 
            // epOrder
            // 
            this.epOrder.ContainerControl = this;
            // 
            // fsdExportToExcel
            // 
            this.fsdExportToExcel.DefaultExt = "csv";
            resources.ApplyResources(this.fsdExportToExcel, "fsdExportToExcel");
            this.fsdExportToExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.fsdExportToExcel_FileOk);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.Name = "cmbLocation";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtRefno);
            this.groupBox2.Controls.Add(this.cmbTransferTo);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtRemark);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtUserName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbMovementType);
            this.groupBox2.Controls.Add(this.dtpEndDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.dtpStartDate);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbLocation);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cmbTransferTo
            // 
            this.cmbTransferTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransferTo.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTransferTo, "cmbTransferTo");
            this.cmbTransferTo.Name = "cmbTransferTo";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // txtRemark
            // 
            resources.ApplyResources(this.txtRemark, "txtRemark");
            this.txtRemark.Name = "txtRemark";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Name = "label7";
            // 
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            this.txtUserName.Name = "txtUserName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // cmbMovementType
            // 
            this.cmbMovementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMovementType.FormattingEnabled = true;
            this.cmbMovementType.Items.AddRange(new object[] {
            resources.GetString("cmbMovementType.Items"),
            resources.GetString("cmbMovementType.Items1"),
            resources.GetString("cmbMovementType.Items2"),
            resources.GetString("cmbMovementType.Items3"),
            resources.GetString("cmbMovementType.Items4"),
            resources.GetString("cmbMovementType.Items5"),
            resources.GetString("cmbMovementType.Items6"),
            resources.GetString("cmbMovementType.Items7")});
            resources.ApplyResources(this.cmbMovementType, "cmbMovementType");
            this.cmbMovementType.Name = "cmbMovementType";
            // 
            // dtpEndDate
            // 
            resources.ApplyResources(this.dtpEndDate, "dtpEndDate");
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // dtpStartDate
            // 
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // bgSearch
            // 
            this.bgSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSearch_DoWork);
            this.bgSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgSearch_RunWorkerCompleted);
            this.bgSearch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // pnlProgress
            // 
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Name = "pnlProgress";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // pgb1
            // 
            resources.ApplyResources(this.pgb1, "pgb1");
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // dgvPurchase
            // 
            this.dgvPurchase.AllowUserToAddRows = false;
            this.dgvPurchase.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvPurchase, "dgvPurchase");
            this.dgvPurchase.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPurchase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.View,
            this.DateTime,
            this.RefNo,
            this.User,
            this.ActivityType,
            this.ReasonName,
            this.InventoryLocationCol,
            this.colTransferToFrom,
            this.TransferOutRef});
            this.dgvPurchase.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvPurchase.Name = "dgvPurchase";
            this.dgvPurchase.RowHeadersVisible = false;
            this.dgvPurchase.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPurchase.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchase_CellContentClick);
            // 
            // View
            // 
            this.View.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.View, "View");
            this.View.Name = "View";
            this.View.Text = "View";
            this.View.UseColumnTextForButtonValue = true;
            // 
            // DateTime
            // 
            this.DateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DateTime.DataPropertyName = "InventoryDate";
            this.DateTime.FillWeight = 94.31673F;
            resources.ApplyResources(this.DateTime, "DateTime");
            this.DateTime.Name = "DateTime";
            this.DateTime.ReadOnly = true;
            // 
            // RefNo
            // 
            this.RefNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RefNo.DataPropertyName = "InventoryHdrRefNo";
            this.RefNo.FillWeight = 94.31673F;
            resources.ApplyResources(this.RefNo, "RefNo");
            this.RefNo.Name = "RefNo";
            this.RefNo.ReadOnly = true;
            // 
            // User
            // 
            this.User.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.User.DataPropertyName = "UserName";
            resources.ApplyResources(this.User, "User");
            this.User.Name = "User";
            this.User.ReadOnly = true;
            // 
            // ActivityType
            // 
            this.ActivityType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ActivityType.DataPropertyName = "MovementType";
            resources.ApplyResources(this.ActivityType, "ActivityType");
            this.ActivityType.Name = "ActivityType";
            this.ActivityType.ReadOnly = true;
            // 
            // ReasonName
            // 
            this.ReasonName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ReasonName.DataPropertyName = "ReasonName";
            resources.ApplyResources(this.ReasonName, "ReasonName");
            this.ReasonName.Name = "ReasonName";
            this.ReasonName.ReadOnly = true;
            // 
            // InventoryLocationCol
            // 
            this.InventoryLocationCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InventoryLocationCol.DataPropertyName = "InventoryLocationName";
            this.InventoryLocationCol.FillWeight = 94.31673F;
            resources.ApplyResources(this.InventoryLocationCol, "InventoryLocationCol");
            this.InventoryLocationCol.Name = "InventoryLocationCol";
            this.InventoryLocationCol.ReadOnly = true;
            // 
            // colTransferToFrom
            // 
            this.colTransferToFrom.DataPropertyName = "TransferToFrom";
            resources.ApplyResources(this.colTransferToFrom, "colTransferToFrom");
            this.colTransferToFrom.Name = "colTransferToFrom";
            // 
            // TransferOutRef
            // 
            this.TransferOutRef.DataPropertyName = "TransferOutRef";
            resources.ApplyResources(this.TransferOutRef, "TransferOutRef");
            this.TransferOutRef.Name = "TransferOutRef";
            // 
            // bgDownload
            // 
            this.bgDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDownload_DoWork);
            this.bgDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgDownload_RunWorkerCompleted);
            this.bgDownload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
            // 
            // txtRefno
            // 
            resources.ApplyResources(this.txtRefno, "txtRefno");
            this.txtRefno.Name = "txtRefno";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // frmViewActivityHeader
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.dgvPurchase);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.groupBox2);
            this.KeyPreview = true;
            this.Name = "frmViewActivityHeader";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.OrderTaking_Load);
            this.Activated += new System.EventHandler(this.OrderTaking_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmViewActivityHeader_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmOrderTaking_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.epOrder)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider epOrder;
        private FixReEntrantDataGridView dgvPurchase;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog fsdExportToExcel;
        protected System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSearch;
        private System.ComponentModel.BackgroundWorker bgSearch;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MovementType;
        private System.Windows.Forms.Label label2;
        protected System.Windows.Forms.ComboBox cmbMovementType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUserName;
        private System.ComponentModel.BackgroundWorker bgDownload;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRemark;
        protected System.Windows.Forms.ComboBox cmbTransferTo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewButtonColumn View;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn RefNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivityType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReasonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryLocationCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransferToFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransferOutRef;
        private System.Windows.Forms.TextBox txtRefno;
        private System.Windows.Forms.Label label9;
    }
}

