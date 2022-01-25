namespace WinPowerPOS.OrderForms
{
    partial class frmPartialPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPartialPayment));
            this.btnCash = new System.Windows.Forms.Button();
            this.btnPay4 = new System.Windows.Forms.Button();
            this.btnPay6 = new System.Windows.Forms.Button();
            this.btnPay1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvPayment = new System.Windows.Forms.DataGridView();
            this.del = new System.Windows.Forms.DataGridViewButtonColumn();
            this.PaymentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtAmt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbRounding = new System.Windows.Forms.CheckBox();
            this.lblChange = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalPaid = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblShortFall = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnPay5 = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnPoint = new System.Windows.Forms.Button();
            this.btnPay2 = new System.Windows.Forms.Button();
            this.btnCheque = new System.Windows.Forms.Button();
            this.btnPay3 = new System.Windows.Forms.Button();
            this.lblNumOfItems = new System.Windows.Forms.Label();
            this.lblRefNo = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPay7 = new System.Windows.Forms.Button();
            this.btnPay8 = new System.Windows.Forms.Button();
            this.btnPay9 = new System.Windows.Forms.Button();
            this.btnPay10 = new System.Windows.Forms.Button();
            this.btnInstallment = new System.Windows.Forms.Button();
            this.btnCreditCard = new System.Windows.Forms.Button();
            this.btnPAMedifund = new System.Windows.Forms.Button();
            this.btnSMF = new System.Windows.Forms.Button();
            this.btnPWF = new System.Windows.Forms.Button();
            this.pnlNETSIntegration = new System.Windows.Forms.Panel();
            this.btnNETSQR = new System.Windows.Forms.Button();
            this.btnNETSBack = new System.Windows.Forms.Button();
            this.btnFlashPay = new System.Windows.Forms.Button();
            this.btnNETSATMCard = new System.Windows.Forms.Button();
            this.btnCashCard = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayment)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlNETSIntegration.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCash
            // 
            this.btnCash.AccessibleDescription = null;
            this.btnCash.AccessibleName = null;
            resources.ApplyResources(this.btnCash, "btnCash");
            this.btnCash.BackColor = System.Drawing.Color.Transparent;
            this.btnCash.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.errorProvider1.SetError(this.btnCash, resources.GetString("btnCash.Error"));
            this.btnCash.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnCash, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnCash.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnCash, ((int)(resources.GetObject("btnCash.IconPadding"))));
            this.btnCash.Name = "btnCash";
            this.btnCash.UseVisualStyleBackColor = false;
            this.btnCash.Click += new System.EventHandler(this.btnCash_Click);
            // 
            // btnPay4
            // 
            this.btnPay4.AccessibleDescription = null;
            this.btnPay4.AccessibleName = null;
            resources.ApplyResources(this.btnPay4, "btnPay4");
            this.btnPay4.BackColor = System.Drawing.Color.Transparent;
            this.btnPay4.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.errorProvider1.SetError(this.btnPay4, resources.GetString("btnPay4.Error"));
            this.btnPay4.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnPay4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay4.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay4, ((int)(resources.GetObject("btnPay4.IconPadding"))));
            this.btnPay4.Name = "btnPay4";
            this.btnPay4.Tag = "4";
            this.btnPay4.UseVisualStyleBackColor = false;
            this.btnPay4.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay6
            // 
            this.btnPay6.AccessibleDescription = null;
            this.btnPay6.AccessibleName = null;
            resources.ApplyResources(this.btnPay6, "btnPay6");
            this.btnPay6.BackColor = System.Drawing.Color.White;
            this.btnPay6.BackgroundImage = global::WinPowerPOS.Properties.Resources.grey;
            this.errorProvider1.SetError(this.btnPay6, resources.GetString("btnPay6.Error"));
            this.btnPay6.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnPay6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay6.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay6, ((int)(resources.GetObject("btnPay6.IconPadding"))));
            this.btnPay6.Name = "btnPay6";
            this.btnPay6.Tag = "6";
            this.btnPay6.UseVisualStyleBackColor = false;
            this.btnPay6.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay1
            // 
            this.btnPay1.AccessibleDescription = null;
            this.btnPay1.AccessibleName = null;
            resources.ApplyResources(this.btnPay1, "btnPay1");
            this.btnPay1.BackColor = System.Drawing.Color.Transparent;
            this.btnPay1.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.errorProvider1.SetError(this.btnPay1, resources.GetString("btnPay1.Error"));
            this.btnPay1.ForeColor = System.Drawing.Color.Orange;
            this.errorProvider1.SetIconAlignment(this.btnPay1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay1.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay1, ((int)(resources.GetObject("btnPay1.IconPadding"))));
            this.btnPay1.Name = "btnPay1";
            this.btnPay1.Tag = "1";
            this.btnPay1.UseVisualStyleBackColor = false;
            this.btnPay1.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.errorProvider1.SetError(this.btnCancel, resources.GetString("btnCancel.Error"));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnCancel.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnCancel, ((int)(resources.GetObject("btnCancel.IconPadding"))));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvPayment
            // 
            this.dgvPayment.AccessibleDescription = null;
            this.dgvPayment.AccessibleName = null;
            this.dgvPayment.AllowUserToAddRows = false;
            this.dgvPayment.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvPayment, "dgvPayment");
            this.dgvPayment.BackgroundImage = null;
            this.dgvPayment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPayment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.del,
            this.PaymentType,
            this.Amount,
            this.ID});
            this.errorProvider1.SetError(this.dgvPayment, resources.GetString("dgvPayment.Error"));
            this.dgvPayment.Font = null;
            this.errorProvider1.SetIconAlignment(this.dgvPayment, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("dgvPayment.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.dgvPayment, ((int)(resources.GetObject("dgvPayment.IconPadding"))));
            this.dgvPayment.Name = "dgvPayment";
            this.dgvPayment.ReadOnly = true;
            this.dgvPayment.RowHeadersVisible = false;
            this.dgvPayment.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayment_CellClick);
            // 
            // del
            // 
            resources.ApplyResources(this.del, "del");
            this.del.Name = "del";
            this.del.ReadOnly = true;
            this.del.Text = "Delete";
            this.del.UseColumnTextForButtonValue = true;
            // 
            // PaymentType
            // 
            this.PaymentType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PaymentType.DataPropertyName = "PaymentType";
            resources.ApplyResources(this.PaymentType, "PaymentType");
            this.PaymentType.Name = "PaymentType";
            this.PaymentType.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.DataPropertyName = "Amount";
            resources.ApplyResources(this.Amount, "Amount");
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ReceiptDetID";
            resources.ApplyResources(this.ID, "ID");
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // txtAmt
            // 
            this.txtAmt.AccessibleDescription = null;
            this.txtAmt.AccessibleName = null;
            resources.ApplyResources(this.txtAmt, "txtAmt");
            this.txtAmt.BackgroundImage = null;
            this.errorProvider1.SetError(this.txtAmt, resources.GetString("txtAmt.Error"));
            this.txtAmt.Font = null;
            this.errorProvider1.SetIconAlignment(this.txtAmt, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("txtAmt.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.txtAmt, ((int)(resources.GetObject("txtAmt.IconPadding"))));
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.ReadOnly = true;
            this.txtAmt.Click += new System.EventHandler(this.txtAmt_Click);
            this.txtAmt.Validating += new System.ComponentModel.CancelEventHandler(this.txtAmt_Validating);
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.cbRounding);
            this.groupBox1.Controls.Add(this.lblChange);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lblAmount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblTotalPaid);
            this.groupBox1.Controls.Add(this.label6);
            this.errorProvider1.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.groupBox1.Font = null;
            this.errorProvider1.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BackgroundImage = null;
            this.errorProvider1.SetError(this.panel1, resources.GetString("panel1.Error"));
            this.panel1.Font = null;
            this.errorProvider1.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.panel1, ((int)(resources.GetObject("panel1.IconPadding"))));
            this.panel1.Name = "panel1";
            // 
            // cbRounding
            // 
            this.cbRounding.AccessibleDescription = null;
            this.cbRounding.AccessibleName = null;
            resources.ApplyResources(this.cbRounding, "cbRounding");
            this.cbRounding.BackgroundImage = null;
            this.errorProvider1.SetError(this.cbRounding, resources.GetString("cbRounding.Error"));
            this.errorProvider1.SetIconAlignment(this.cbRounding, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("cbRounding.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.cbRounding, ((int)(resources.GetObject("cbRounding.IconPadding"))));
            this.cbRounding.Name = "cbRounding";
            this.cbRounding.UseVisualStyleBackColor = true;
            this.cbRounding.CheckedChanged += new System.EventHandler(this.cbRounding_CheckedChanged);
            // 
            // lblChange
            // 
            this.lblChange.AccessibleDescription = null;
            this.lblChange.AccessibleName = null;
            resources.ApplyResources(this.lblChange, "lblChange");
            this.lblChange.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.lblChange, resources.GetString("lblChange.Error"));
            this.errorProvider1.SetIconAlignment(this.lblChange, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblChange.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblChange, ((int)(resources.GetObject("lblChange.IconPadding"))));
            this.lblChange.Name = "lblChange";
            // 
            // label10
            // 
            this.label10.AccessibleDescription = null;
            this.label10.AccessibleName = null;
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.label10, resources.GetString("label10.Error"));
            this.errorProvider1.SetIconAlignment(this.label10, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label10.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label10, ((int)(resources.GetObject("label10.IconPadding"))));
            this.label10.Name = "label10";
            // 
            // lblAmount
            // 
            this.lblAmount.AccessibleDescription = null;
            this.lblAmount.AccessibleName = null;
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.errorProvider1.SetError(this.lblAmount, resources.GetString("lblAmount.Error"));
            this.errorProvider1.SetIconAlignment(this.lblAmount, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblAmount.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblAmount, ((int)(resources.GetObject("lblAmount.IconPadding"))));
            this.lblAmount.Name = "lblAmount";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider1.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider1.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // lblTotalPaid
            // 
            this.lblTotalPaid.AccessibleDescription = null;
            this.lblTotalPaid.AccessibleName = null;
            resources.ApplyResources(this.lblTotalPaid, "lblTotalPaid");
            this.lblTotalPaid.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.lblTotalPaid, resources.GetString("lblTotalPaid.Error"));
            this.errorProvider1.SetIconAlignment(this.lblTotalPaid, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblTotalPaid.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblTotalPaid, ((int)(resources.GetObject("lblTotalPaid.IconPadding"))));
            this.lblTotalPaid.Name = "lblTotalPaid";
            // 
            // label6
            // 
            this.label6.AccessibleDescription = null;
            this.label6.AccessibleName = null;
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider1.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // lblShortFall
            // 
            this.lblShortFall.AccessibleDescription = null;
            this.lblShortFall.AccessibleName = null;
            resources.ApplyResources(this.lblShortFall, "lblShortFall");
            this.lblShortFall.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.lblShortFall, resources.GetString("lblShortFall.Error"));
            this.errorProvider1.SetIconAlignment(this.lblShortFall, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblShortFall.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblShortFall, ((int)(resources.GetObject("lblShortFall.IconPadding"))));
            this.lblShortFall.Name = "lblShortFall";
            // 
            // label8
            // 
            this.label8.AccessibleDescription = null;
            this.label8.AccessibleName = null;
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider1.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider1.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider1.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleDescription = null;
            this.btnConfirm.AccessibleName = null;
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::WinPowerPOS.Properties.Resources.buttonGold;
            this.errorProvider1.SetError(this.btnConfirm, resources.GetString("btnConfirm.Error"));
            this.btnConfirm.ForeColor = System.Drawing.Color.Black;
            this.errorProvider1.SetIconAlignment(this.btnConfirm, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnConfirm.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnConfirm, ((int)(resources.GetObject("btnConfirm.IconPadding"))));
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnPay5
            // 
            this.btnPay5.AccessibleDescription = null;
            this.btnPay5.AccessibleName = null;
            resources.ApplyResources(this.btnPay5, "btnPay5");
            this.btnPay5.BackColor = System.Drawing.Color.Transparent;
            this.btnPay5.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbackground;
            this.errorProvider1.SetError(this.btnPay5, resources.GetString("btnPay5.Error"));
            this.btnPay5.ForeColor = System.Drawing.Color.Black;
            this.errorProvider1.SetIconAlignment(this.btnPay5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay5.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay5, ((int)(resources.GetObject("btnPay5.IconPadding"))));
            this.btnPay5.Name = "btnPay5";
            this.btnPay5.Tag = "5";
            this.btnPay5.UseVisualStyleBackColor = false;
            this.btnPay5.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            resources.ApplyResources(this.errorProvider1, "errorProvider1");
            // 
            // btnPoint
            // 
            this.btnPoint.AccessibleDescription = null;
            this.btnPoint.AccessibleName = null;
            resources.ApplyResources(this.btnPoint, "btnPoint");
            this.btnPoint.BackColor = System.Drawing.SystemColors.Control;
            this.btnPoint.BackgroundImage = null;
            this.errorProvider1.SetError(this.btnPoint, resources.GetString("btnPoint.Error"));
            this.btnPoint.ForeColor = System.Drawing.SystemColors.ControlText;
            this.errorProvider1.SetIconAlignment(this.btnPoint, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPoint.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPoint, ((int)(resources.GetObject("btnPoint.IconPadding"))));
            this.btnPoint.Name = "btnPoint";
            this.btnPoint.UseVisualStyleBackColor = false;
            this.btnPoint.Click += new System.EventHandler(this.btnPoint_Click);
            // 
            // btnPay2
            // 
            this.btnPay2.AccessibleDescription = null;
            this.btnPay2.AccessibleName = null;
            resources.ApplyResources(this.btnPay2, "btnPay2");
            this.btnPay2.BackColor = System.Drawing.Color.Transparent;
            this.btnPay2.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.errorProvider1.SetError(this.btnPay2, resources.GetString("btnPay2.Error"));
            this.btnPay2.ForeColor = System.Drawing.Color.DarkRed;
            this.errorProvider1.SetIconAlignment(this.btnPay2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay2.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay2, ((int)(resources.GetObject("btnPay2.IconPadding"))));
            this.btnPay2.Name = "btnPay2";
            this.btnPay2.Tag = "2";
            this.btnPay2.UseVisualStyleBackColor = false;
            this.btnPay2.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnCheque
            // 
            this.btnCheque.AccessibleDescription = null;
            this.btnCheque.AccessibleName = null;
            resources.ApplyResources(this.btnCheque, "btnCheque");
            this.btnCheque.BackColor = System.Drawing.Color.Transparent;
            this.btnCheque.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.errorProvider1.SetError(this.btnCheque, resources.GetString("btnCheque.Error"));
            this.btnCheque.ForeColor = System.Drawing.Color.Black;
            this.errorProvider1.SetIconAlignment(this.btnCheque, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnCheque.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnCheque, ((int)(resources.GetObject("btnCheque.IconPadding"))));
            this.btnCheque.Name = "btnCheque";
            this.btnCheque.UseVisualStyleBackColor = false;
            this.btnCheque.Click += new System.EventHandler(this.btnCheque_Click);
            // 
            // btnPay3
            // 
            this.btnPay3.AccessibleDescription = null;
            this.btnPay3.AccessibleName = null;
            resources.ApplyResources(this.btnPay3, "btnPay3");
            this.btnPay3.BackColor = System.Drawing.Color.Transparent;
            this.btnPay3.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.errorProvider1.SetError(this.btnPay3, resources.GetString("btnPay3.Error"));
            this.btnPay3.ForeColor = System.Drawing.Color.Black;
            this.errorProvider1.SetIconAlignment(this.btnPay3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay3.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay3, ((int)(resources.GetObject("btnPay3.IconPadding"))));
            this.btnPay3.Name = "btnPay3";
            this.btnPay3.Tag = "3";
            this.btnPay3.UseVisualStyleBackColor = false;
            this.btnPay3.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // lblNumOfItems
            // 
            this.lblNumOfItems.AccessibleDescription = null;
            this.lblNumOfItems.AccessibleName = null;
            resources.ApplyResources(this.lblNumOfItems, "lblNumOfItems");
            this.lblNumOfItems.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.lblNumOfItems, resources.GetString("lblNumOfItems.Error"));
            this.errorProvider1.SetIconAlignment(this.lblNumOfItems, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblNumOfItems.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblNumOfItems, ((int)(resources.GetObject("lblNumOfItems.IconPadding"))));
            this.lblNumOfItems.Name = "lblNumOfItems";
            // 
            // lblRefNo
            // 
            this.lblRefNo.AccessibleDescription = null;
            this.lblRefNo.AccessibleName = null;
            resources.ApplyResources(this.lblRefNo, "lblRefNo");
            this.lblRefNo.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.lblRefNo, resources.GetString("lblRefNo.Error"));
            this.errorProvider1.SetIconAlignment(this.lblRefNo, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblRefNo.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblRefNo, ((int)(resources.GetObject("lblRefNo.IconPadding"))));
            this.lblRefNo.Name = "lblRefNo";
            // 
            // label11
            // 
            this.label11.AccessibleDescription = null;
            this.label11.AccessibleName = null;
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.label11, resources.GetString("label11.Error"));
            this.errorProvider1.SetIconAlignment(this.label11, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label11.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label11, ((int)(resources.GetObject("label11.IconPadding"))));
            this.label11.Name = "label11";
            // 
            // label12
            // 
            this.label12.AccessibleDescription = null;
            this.label12.AccessibleName = null;
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetError(this.label12, resources.GetString("label12.Error"));
            this.errorProvider1.SetIconAlignment(this.label12, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label12.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label12, ((int)(resources.GetObject("label12.IconPadding"))));
            this.label12.Name = "label12";
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleDescription = null;
            this.groupBox3.AccessibleName = null;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.BackgroundImage = null;
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.lblNumOfItems);
            this.groupBox3.Controls.Add(this.lblRefNo);
            this.errorProvider1.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.groupBox3.Font = null;
            this.errorProvider1.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AccessibleDescription = null;
            this.flowLayoutPanel1.AccessibleName = null;
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.BackgroundImage = null;
            this.flowLayoutPanel1.Controls.Add(this.btnCash);
            this.flowLayoutPanel1.Controls.Add(this.btnPay1);
            this.flowLayoutPanel1.Controls.Add(this.btnPay2);
            this.flowLayoutPanel1.Controls.Add(this.btnPay3);
            this.flowLayoutPanel1.Controls.Add(this.btnPay4);
            this.flowLayoutPanel1.Controls.Add(this.btnPay5);
            this.flowLayoutPanel1.Controls.Add(this.btnPay6);
            this.flowLayoutPanel1.Controls.Add(this.btnPay7);
            this.flowLayoutPanel1.Controls.Add(this.btnPay8);
            this.flowLayoutPanel1.Controls.Add(this.btnPay9);
            this.flowLayoutPanel1.Controls.Add(this.btnPay10);
            this.flowLayoutPanel1.Controls.Add(this.btnCheque);
            this.flowLayoutPanel1.Controls.Add(this.btnPoint);
            this.flowLayoutPanel1.Controls.Add(this.btnInstallment);
            this.flowLayoutPanel1.Controls.Add(this.btnCreditCard);
            this.flowLayoutPanel1.Controls.Add(this.btnPAMedifund);
            this.flowLayoutPanel1.Controls.Add(this.btnSMF);
            this.flowLayoutPanel1.Controls.Add(this.btnPWF);
            this.errorProvider1.SetError(this.flowLayoutPanel1, resources.GetString("flowLayoutPanel1.Error"));
            this.flowLayoutPanel1.Font = null;
            this.errorProvider1.SetIconAlignment(this.flowLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel1.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.flowLayoutPanel1, ((int)(resources.GetObject("flowLayoutPanel1.IconPadding"))));
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // btnPay7
            // 
            this.btnPay7.AccessibleDescription = null;
            this.btnPay7.AccessibleName = null;
            resources.ApplyResources(this.btnPay7, "btnPay7");
            this.btnPay7.BackColor = System.Drawing.Color.White;
            this.btnPay7.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.errorProvider1.SetError(this.btnPay7, resources.GetString("btnPay7.Error"));
            this.btnPay7.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnPay7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay7.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay7, ((int)(resources.GetObject("btnPay7.IconPadding"))));
            this.btnPay7.Name = "btnPay7";
            this.btnPay7.Tag = "7";
            this.btnPay7.UseVisualStyleBackColor = false;
            this.btnPay7.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay8
            // 
            this.btnPay8.AccessibleDescription = null;
            this.btnPay8.AccessibleName = null;
            resources.ApplyResources(this.btnPay8, "btnPay8");
            this.btnPay8.BackColor = System.Drawing.Color.White;
            this.btnPay8.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbackground;
            this.errorProvider1.SetError(this.btnPay8, resources.GetString("btnPay8.Error"));
            this.btnPay8.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnPay8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay8.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay8, ((int)(resources.GetObject("btnPay8.IconPadding"))));
            this.btnPay8.Name = "btnPay8";
            this.btnPay8.Tag = "8";
            this.btnPay8.UseVisualStyleBackColor = false;
            this.btnPay8.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay9
            // 
            this.btnPay9.AccessibleDescription = null;
            this.btnPay9.AccessibleName = null;
            resources.ApplyResources(this.btnPay9, "btnPay9");
            this.btnPay9.BackColor = System.Drawing.Color.White;
            this.btnPay9.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.errorProvider1.SetError(this.btnPay9, resources.GetString("btnPay9.Error"));
            this.btnPay9.ForeColor = System.Drawing.Color.Black;
            this.errorProvider1.SetIconAlignment(this.btnPay9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay9.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay9, ((int)(resources.GetObject("btnPay9.IconPadding"))));
            this.btnPay9.Name = "btnPay9";
            this.btnPay9.Tag = "9";
            this.btnPay9.UseVisualStyleBackColor = false;
            this.btnPay9.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay10
            // 
            this.btnPay10.AccessibleDescription = null;
            this.btnPay10.AccessibleName = null;
            resources.ApplyResources(this.btnPay10, "btnPay10");
            this.btnPay10.BackColor = System.Drawing.Color.Black;
            this.btnPay10.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.errorProvider1.SetError(this.btnPay10, resources.GetString("btnPay10.Error"));
            this.btnPay10.ForeColor = System.Drawing.Color.Black;
            this.errorProvider1.SetIconAlignment(this.btnPay10, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPay10.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPay10, ((int)(resources.GetObject("btnPay10.IconPadding"))));
            this.btnPay10.Name = "btnPay10";
            this.btnPay10.Tag = "10";
            this.btnPay10.UseVisualStyleBackColor = false;
            this.btnPay10.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnInstallment
            // 
            this.btnInstallment.AccessibleDescription = null;
            this.btnInstallment.AccessibleName = null;
            resources.ApplyResources(this.btnInstallment, "btnInstallment");
            this.btnInstallment.BackColor = System.Drawing.SystemColors.Control;
            this.btnInstallment.BackgroundImage = null;
            this.errorProvider1.SetError(this.btnInstallment, resources.GetString("btnInstallment.Error"));
            this.btnInstallment.ForeColor = System.Drawing.SystemColors.ControlText;
            this.errorProvider1.SetIconAlignment(this.btnInstallment, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnInstallment.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnInstallment, ((int)(resources.GetObject("btnInstallment.IconPadding"))));
            this.btnInstallment.Name = "btnInstallment";
            this.btnInstallment.UseVisualStyleBackColor = false;
            this.btnInstallment.Click += new System.EventHandler(this.btnInstallment_Click);
            // 
            // btnCreditCard
            // 
            this.btnCreditCard.AccessibleDescription = null;
            this.btnCreditCard.AccessibleName = null;
            resources.ApplyResources(this.btnCreditCard, "btnCreditCard");
            this.btnCreditCard.BackColor = System.Drawing.SystemColors.Control;
            this.btnCreditCard.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.errorProvider1.SetError(this.btnCreditCard, resources.GetString("btnCreditCard.Error"));
            this.btnCreditCard.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnCreditCard, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnCreditCard.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnCreditCard, ((int)(resources.GetObject("btnCreditCard.IconPadding"))));
            this.btnCreditCard.Name = "btnCreditCard";
            this.btnCreditCard.UseVisualStyleBackColor = false;
            this.btnCreditCard.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPAMedifund
            // 
            this.btnPAMedifund.AccessibleDescription = null;
            this.btnPAMedifund.AccessibleName = null;
            resources.ApplyResources(this.btnPAMedifund, "btnPAMedifund");
            this.btnPAMedifund.BackColor = System.Drawing.SystemColors.Control;
            this.btnPAMedifund.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.errorProvider1.SetError(this.btnPAMedifund, resources.GetString("btnPAMedifund.Error"));
            this.btnPAMedifund.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnPAMedifund, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPAMedifund.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPAMedifund, ((int)(resources.GetObject("btnPAMedifund.IconPadding"))));
            this.btnPAMedifund.Name = "btnPAMedifund";
            this.btnPAMedifund.UseVisualStyleBackColor = false;
            this.btnPAMedifund.Click += new System.EventHandler(this.btnPAMedifund_Click);
            // 
            // btnSMF
            // 
            this.btnSMF.AccessibleDescription = null;
            this.btnSMF.AccessibleName = null;
            resources.ApplyResources(this.btnSMF, "btnSMF");
            this.btnSMF.BackColor = System.Drawing.SystemColors.Control;
            this.btnSMF.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.errorProvider1.SetError(this.btnSMF, resources.GetString("btnSMF.Error"));
            this.btnSMF.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnSMF, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnSMF.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnSMF, ((int)(resources.GetObject("btnSMF.IconPadding"))));
            this.btnSMF.Name = "btnSMF";
            this.btnSMF.UseVisualStyleBackColor = false;
            this.btnSMF.Click += new System.EventHandler(this.btnSMF_Click);
            // 
            // btnPWF
            // 
            this.btnPWF.AccessibleDescription = null;
            this.btnPWF.AccessibleName = null;
            resources.ApplyResources(this.btnPWF, "btnPWF");
            this.btnPWF.BackColor = System.Drawing.SystemColors.Control;
            this.btnPWF.BackgroundImage = global::WinPowerPOS.Properties.Resources.brown;
            this.errorProvider1.SetError(this.btnPWF, resources.GetString("btnPWF.Error"));
            this.btnPWF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.errorProvider1.SetIconAlignment(this.btnPWF, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnPWF.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnPWF, ((int)(resources.GetObject("btnPWF.IconPadding"))));
            this.btnPWF.Name = "btnPWF";
            this.btnPWF.UseVisualStyleBackColor = false;
            this.btnPWF.Click += new System.EventHandler(this.btnPWF_Click);
            // 
            // pnlNETSIntegration
            // 
            this.pnlNETSIntegration.AccessibleDescription = null;
            this.pnlNETSIntegration.AccessibleName = null;
            resources.ApplyResources(this.pnlNETSIntegration, "pnlNETSIntegration");
            this.pnlNETSIntegration.BackgroundImage = null;
            this.pnlNETSIntegration.Controls.Add(this.btnNETSQR);
            this.pnlNETSIntegration.Controls.Add(this.btnNETSBack);
            this.pnlNETSIntegration.Controls.Add(this.btnFlashPay);
            this.pnlNETSIntegration.Controls.Add(this.btnNETSATMCard);
            this.pnlNETSIntegration.Controls.Add(this.btnCashCard);
            this.errorProvider1.SetError(this.pnlNETSIntegration, resources.GetString("pnlNETSIntegration.Error"));
            this.pnlNETSIntegration.Font = null;
            this.errorProvider1.SetIconAlignment(this.pnlNETSIntegration, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pnlNETSIntegration.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.pnlNETSIntegration, ((int)(resources.GetObject("pnlNETSIntegration.IconPadding"))));
            this.pnlNETSIntegration.Name = "pnlNETSIntegration";
            // 
            // btnNETSQR
            // 
            this.btnNETSQR.AccessibleDescription = null;
            this.btnNETSQR.AccessibleName = null;
            resources.ApplyResources(this.btnNETSQR, "btnNETSQR");
            this.errorProvider1.SetError(this.btnNETSQR, resources.GetString("btnNETSQR.Error"));
            this.btnNETSQR.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnNETSQR, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnNETSQR.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnNETSQR, ((int)(resources.GetObject("btnNETSQR.IconPadding"))));
            this.btnNETSQR.Name = "btnNETSQR";
            this.btnNETSQR.UseVisualStyleBackColor = true;
            this.btnNETSQR.Click += new System.EventHandler(this.btnMakePaymentNetsDetail_Click);
            // 
            // btnNETSBack
            // 
            this.btnNETSBack.AccessibleDescription = null;
            this.btnNETSBack.AccessibleName = null;
            resources.ApplyResources(this.btnNETSBack, "btnNETSBack");
            this.btnNETSBack.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.errorProvider1.SetError(this.btnNETSBack, resources.GetString("btnNETSBack.Error"));
            this.btnNETSBack.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnNETSBack, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnNETSBack.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnNETSBack, ((int)(resources.GetObject("btnNETSBack.IconPadding"))));
            this.btnNETSBack.Name = "btnNETSBack";
            this.btnNETSBack.UseVisualStyleBackColor = true;
            this.btnNETSBack.Click += new System.EventHandler(this.btnNETSBack_Click);
            // 
            // btnFlashPay
            // 
            this.btnFlashPay.AccessibleDescription = null;
            this.btnFlashPay.AccessibleName = null;
            resources.ApplyResources(this.btnFlashPay, "btnFlashPay");
            this.errorProvider1.SetError(this.btnFlashPay, resources.GetString("btnFlashPay.Error"));
            this.btnFlashPay.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.btnFlashPay, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnFlashPay.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnFlashPay, ((int)(resources.GetObject("btnFlashPay.IconPadding"))));
            this.btnFlashPay.Name = "btnFlashPay";
            this.btnFlashPay.UseVisualStyleBackColor = true;
            this.btnFlashPay.Click += new System.EventHandler(this.btnMakePaymentNetsDetail_Click);
            // 
            // btnNETSATMCard
            // 
            this.btnNETSATMCard.AccessibleDescription = null;
            this.btnNETSATMCard.AccessibleName = null;
            resources.ApplyResources(this.btnNETSATMCard, "btnNETSATMCard");
            this.btnNETSATMCard.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.errorProvider1.SetError(this.btnNETSATMCard, resources.GetString("btnNETSATMCard.Error"));
            this.btnNETSATMCard.ForeColor = System.Drawing.Color.Black;
            this.errorProvider1.SetIconAlignment(this.btnNETSATMCard, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnNETSATMCard.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnNETSATMCard, ((int)(resources.GetObject("btnNETSATMCard.IconPadding"))));
            this.btnNETSATMCard.Name = "btnNETSATMCard";
            this.btnNETSATMCard.Tag = "2";
            this.btnNETSATMCard.UseVisualStyleBackColor = true;
            this.btnNETSATMCard.Click += new System.EventHandler(this.btnMakePaymentNetsDetail_Click);
            // 
            // btnCashCard
            // 
            this.btnCashCard.AccessibleDescription = null;
            this.btnCashCard.AccessibleName = null;
            resources.ApplyResources(this.btnCashCard, "btnCashCard");
            this.btnCashCard.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.errorProvider1.SetError(this.btnCashCard, resources.GetString("btnCashCard.Error"));
            this.btnCashCard.ForeColor = System.Drawing.Color.Orange;
            this.errorProvider1.SetIconAlignment(this.btnCashCard, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnCashCard.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnCashCard, ((int)(resources.GetObject("btnCashCard.IconPadding"))));
            this.btnCashCard.Name = "btnCashCard";
            this.btnCashCard.Tag = "1";
            this.btnCashCard.UseVisualStyleBackColor = true;
            this.btnCashCard.Click += new System.EventHandler(this.btnMakePaymentNetsDetail_Click);
            // 
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BackgroundImage = null;
            this.panel2.Controls.Add(this.label9);
            this.errorProvider1.SetError(this.panel2, resources.GetString("panel2.Error"));
            this.panel2.Font = null;
            this.errorProvider1.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.panel2, ((int)(resources.GetObject("panel2.IconPadding"))));
            this.panel2.Name = "panel2";
            // 
            // label9
            // 
            this.label9.AccessibleDescription = null;
            this.label9.AccessibleName = null;
            resources.ApplyResources(this.label9, "label9");
            this.errorProvider1.SetError(this.label9, resources.GetString("label9.Error"));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.errorProvider1.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            // 
            // frmPartialPayment
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvPayment);
            this.Controls.Add(this.txtAmt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblShortFall);
            this.Controls.Add(this.pnlNETSIntegration);
            this.Controls.Add(this.label8);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = null;
            this.Name = "frmPartialPayment";
            this.Load += new System.EventHandler(this.frmPartialPayment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayment)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlNETSIntegration.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnCash;
        internal System.Windows.Forms.Button btnPay4;
        internal System.Windows.Forms.Button btnPay6;
        internal System.Windows.Forms.Button btnPay1;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvPayment;
        private System.Windows.Forms.TextBox txtAmt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblAmount;
        internal System.Windows.Forms.Button btnConfirm;
        public System.Windows.Forms.Label lblTotalPaid;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label lblShortFall;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Button btnPay5;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        internal System.Windows.Forms.Button btnPoint;
        internal System.Windows.Forms.Button btnPay2;
        internal System.Windows.Forms.Button btnCheque;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblNumOfItems;
        private System.Windows.Forms.Label lblRefNo;
        private System.Windows.Forms.CheckBox cbRounding;
        private System.Windows.Forms.Button btnPay3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal System.Windows.Forms.Button btnInstallment;
        private System.Windows.Forms.DataGridViewButtonColumn del;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        internal System.Windows.Forms.Button btnPWF;
        internal System.Windows.Forms.Button btnPAMedifund;
        internal System.Windows.Forms.Button btnSMF;
        internal System.Windows.Forms.Button btnPay7;
        internal System.Windows.Forms.Button btnPay8;
        internal System.Windows.Forms.Button btnPay9;
        internal System.Windows.Forms.Button btnPay10;
        private System.Windows.Forms.Panel pnlNETSIntegration;
        internal System.Windows.Forms.Button btnNETSBack;
        internal System.Windows.Forms.Button btnFlashPay;
        internal System.Windows.Forms.Button btnNETSATMCard;
        internal System.Windows.Forms.Button btnCashCard;
        internal System.Windows.Forms.Button btnCreditCard;
        internal System.Windows.Forms.Button btnNETSQR;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;

    }
}