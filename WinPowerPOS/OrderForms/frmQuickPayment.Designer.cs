namespace WinPowerPOS.OrderForms
{
    partial class frmQuickPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuickPayment));
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvPayment = new System.Windows.Forms.DataGridView();
            this.del = new System.Windows.Forms.DataGridViewButtonColumn();
            this.PaymentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtAmt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbRounding = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblShortFall = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalPaid = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblNumOfItems = new System.Windows.Forms.Label();
            this.lblRefNo = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flpDelPay = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDelCASH = new System.Windows.Forms.Button();
            this.btnDelInstallment = new System.Windows.Forms.Button();
            this.btnDelPoints = new System.Windows.Forms.Button();
            this.btnDelCheque = new System.Windows.Forms.Button();
            this.flpLabelPay = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCash = new System.Windows.Forms.Panel();
            this.lblPayCash = new System.Windows.Forms.Label();
            this.pnlInstallment = new System.Windows.Forms.Panel();
            this.lblPayInstallment1 = new System.Windows.Forms.Label();
            this.pnlPoints = new System.Windows.Forms.Panel();
            this.lblPayPoints = new System.Windows.Forms.Label();
            this.pnlCheque = new System.Windows.Forms.Panel();
            this.lblPayCheque = new System.Windows.Forms.Label();
            this.flpButtonPay = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCash = new System.Windows.Forms.Button();
            this.btnInstallment = new System.Windows.Forms.Button();
            this.btnPoint = new System.Windows.Forms.Button();
            this.btnCheque = new System.Windows.Forms.Button();
            this.btn10 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btn5 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn50 = new System.Windows.Forms.Button();
            this.btnMin = new System.Windows.Forms.Button();
            this.btnCLEAR = new System.Windows.Forms.Button();
            this.btnBackSpace = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayment)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flpDelPay.SuspendLayout();
            this.flpLabelPay.SuspendLayout();
            this.pnlCash.SuspendLayout();
            this.pnlInstallment.SuspendLayout();
            this.pnlPoints.SuspendLayout();
            this.pnlCheque.SuspendLayout();
            this.flpButtonPay.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvPayment
            // 
            this.dgvPayment.AllowUserToAddRows = false;
            this.dgvPayment.AllowUserToDeleteRows = false;
            this.dgvPayment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPayment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.del,
            this.PaymentType,
            this.Amount,
            this.ID});
            resources.ApplyResources(this.dgvPayment, "dgvPayment");
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
            resources.ApplyResources(this.txtAmt, "txtAmt");
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.TextChanged += new System.EventHandler(this.txtAmt_TextChanged);
            this.txtAmt.Click += new System.EventHandler(this.txtAmt_Click);
            this.txtAmt.Validating += new System.ComponentModel.CancelEventHandler(this.txtAmt_Validating);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.cbRounding);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblChange);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblShortFall);
            this.groupBox1.Controls.Add(this.lblAmount);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblTotalPaid);
            this.groupBox1.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbRounding
            // 
            resources.ApplyResources(this.cbRounding, "cbRounding");
            this.cbRounding.Checked = true;
            this.cbRounding.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRounding.Name = "cbRounding";
            this.cbRounding.UseVisualStyleBackColor = true;
            this.cbRounding.CheckedChanged += new System.EventHandler(this.cbRounding_CheckedChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Name = "label7";
            // 
            // lblChange
            // 
            resources.ApplyResources(this.lblChange, "lblChange");
            this.lblChange.BackColor = System.Drawing.Color.Transparent;
            this.lblChange.Name = "lblChange";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Name = "label10";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lblShortFall
            // 
            resources.ApplyResources(this.lblShortFall, "lblShortFall");
            this.lblShortFall.BackColor = System.Drawing.Color.Transparent;
            this.lblShortFall.Name = "lblShortFall";
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.Name = "lblAmount";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // lblTotalPaid
            // 
            resources.ApplyResources(this.lblTotalPaid, "lblTotalPaid");
            this.lblTotalPaid.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalPaid.Name = "lblTotalPaid";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // btnConfirm
            // 
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::WinPowerPOS.Properties.Resources.buttonGold;
            this.btnConfirm.ForeColor = System.Drawing.Color.Black;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblNumOfItems
            // 
            resources.ApplyResources(this.lblNumOfItems, "lblNumOfItems");
            this.lblNumOfItems.BackColor = System.Drawing.Color.Transparent;
            this.lblNumOfItems.Name = "lblNumOfItems";
            // 
            // lblRefNo
            // 
            resources.ApplyResources(this.lblRefNo, "lblRefNo");
            this.lblRefNo.BackColor = System.Drawing.Color.Transparent;
            this.lblRefNo.Name = "lblRefNo";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Name = "label12";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.lblTotalQty);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.lblNumOfItems);
            this.groupBox3.Controls.Add(this.lblRefNo);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Name = "label14";
            // 
            // lblTotalQty
            // 
            resources.ApplyResources(this.lblTotalQty, "lblTotalQty");
            this.lblTotalQty.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalQty.Name = "lblTotalQty";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.btn10);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.btn5);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnPlus);
            this.groupBox2.Controls.Add(this.btn2);
            this.groupBox2.Controls.Add(this.btn50);
            this.groupBox2.Controls.Add(this.btnMin);
            this.groupBox2.Controls.Add(this.btnCLEAR);
            this.groupBox2.Controls.Add(this.btnBackSpace);
            this.groupBox2.Controls.Add(this.txtAmt);
            this.groupBox2.Controls.Add(this.btnDot);
            this.groupBox2.Controls.Add(this.button10);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.btn1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.flpDelPay);
            this.panel1.Controls.Add(this.flpLabelPay);
            this.panel1.Controls.Add(this.flpButtonPay);
            this.panel1.Name = "panel1";
            // 
            // flpDelPay
            // 
            resources.ApplyResources(this.flpDelPay, "flpDelPay");
            this.flpDelPay.Controls.Add(this.btnDelCASH);
            this.flpDelPay.Controls.Add(this.btnDelInstallment);
            this.flpDelPay.Controls.Add(this.btnDelPoints);
            this.flpDelPay.Controls.Add(this.btnDelCheque);
            this.flpDelPay.Name = "flpDelPay";
            // 
            // btnDelCASH
            // 
            this.btnDelCASH.BackColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.btnDelCASH, "btnDelCASH");
            this.btnDelCASH.ForeColor = System.Drawing.Color.White;
            this.btnDelCASH.Name = "btnDelCASH";
            this.btnDelCASH.Tag = "CASH";
            this.btnDelCASH.UseVisualStyleBackColor = false;
            this.btnDelCASH.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnDelInstallment
            // 
            this.btnDelInstallment.BackColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.btnDelInstallment, "btnDelInstallment");
            this.btnDelInstallment.ForeColor = System.Drawing.Color.White;
            this.btnDelInstallment.Name = "btnDelInstallment";
            this.btnDelInstallment.Tag = "INSTALLMENT";
            this.btnDelInstallment.UseVisualStyleBackColor = false;
            this.btnDelInstallment.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnDelPoints
            // 
            this.btnDelPoints.BackColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.btnDelPoints, "btnDelPoints");
            this.btnDelPoints.ForeColor = System.Drawing.Color.White;
            this.btnDelPoints.Name = "btnDelPoints";
            this.btnDelPoints.Tag = "POINTS";
            this.btnDelPoints.UseVisualStyleBackColor = false;
            this.btnDelPoints.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnDelCheque
            // 
            this.btnDelCheque.BackColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.btnDelCheque, "btnDelCheque");
            this.btnDelCheque.ForeColor = System.Drawing.Color.White;
            this.btnDelCheque.Name = "btnDelCheque";
            this.btnDelCheque.Tag = "CHEQUE";
            this.btnDelCheque.UseVisualStyleBackColor = false;
            this.btnDelCheque.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // flpLabelPay
            // 
            resources.ApplyResources(this.flpLabelPay, "flpLabelPay");
            this.flpLabelPay.Controls.Add(this.pnlCash);
            this.flpLabelPay.Controls.Add(this.pnlInstallment);
            this.flpLabelPay.Controls.Add(this.pnlPoints);
            this.flpLabelPay.Controls.Add(this.pnlCheque);
            this.flpLabelPay.Name = "flpLabelPay";
            // 
            // pnlCash
            // 
            this.pnlCash.Controls.Add(this.lblPayCash);
            resources.ApplyResources(this.pnlCash, "pnlCash");
            this.pnlCash.Name = "pnlCash";
            this.pnlCash.Tag = "CASH";
            // 
            // lblPayCash
            // 
            resources.ApplyResources(this.lblPayCash, "lblPayCash");
            this.lblPayCash.Name = "lblPayCash";
            this.lblPayCash.Tag = "CASH";
            // 
            // pnlInstallment
            // 
            this.pnlInstallment.Controls.Add(this.lblPayInstallment1);
            resources.ApplyResources(this.pnlInstallment, "pnlInstallment");
            this.pnlInstallment.Name = "pnlInstallment";
            this.pnlInstallment.Tag = "INSTALLMENT";
            // 
            // lblPayInstallment1
            // 
            resources.ApplyResources(this.lblPayInstallment1, "lblPayInstallment1");
            this.lblPayInstallment1.Name = "lblPayInstallment1";
            this.lblPayInstallment1.Tag = "INSTALLMENT";
            // 
            // pnlPoints
            // 
            this.pnlPoints.Controls.Add(this.lblPayPoints);
            resources.ApplyResources(this.pnlPoints, "pnlPoints");
            this.pnlPoints.Name = "pnlPoints";
            this.pnlPoints.Tag = "POINTS";
            // 
            // lblPayPoints
            // 
            resources.ApplyResources(this.lblPayPoints, "lblPayPoints");
            this.lblPayPoints.Name = "lblPayPoints";
            this.lblPayPoints.Tag = "POINTS";
            // 
            // pnlCheque
            // 
            this.pnlCheque.Controls.Add(this.lblPayCheque);
            resources.ApplyResources(this.pnlCheque, "pnlCheque");
            this.pnlCheque.Name = "pnlCheque";
            this.pnlCheque.Tag = "CHEQUE";
            // 
            // lblPayCheque
            // 
            resources.ApplyResources(this.lblPayCheque, "lblPayCheque");
            this.lblPayCheque.Name = "lblPayCheque";
            this.lblPayCheque.Tag = "CHEQUE";
            // 
            // flpButtonPay
            // 
            resources.ApplyResources(this.flpButtonPay, "flpButtonPay");
            this.flpButtonPay.Controls.Add(this.btnCash);
            this.flpButtonPay.Controls.Add(this.btnInstallment);
            this.flpButtonPay.Controls.Add(this.btnPoint);
            this.flpButtonPay.Controls.Add(this.btnCheque);
            this.flpButtonPay.Name = "flpButtonPay";
            // 
            // btnCash
            // 
            this.btnCash.BackColor = System.Drawing.Color.Transparent;
            this.btnCash.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            resources.ApplyResources(this.btnCash, "btnCash");
            this.btnCash.ForeColor = System.Drawing.Color.White;
            this.btnCash.Name = "btnCash";
            this.btnCash.Tag = "CASH";
            this.btnCash.UseVisualStyleBackColor = false;
            this.btnCash.Click += new System.EventHandler(this.btnCash_Click);
            // 
            // btnInstallment
            // 
            this.btnInstallment.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnInstallment, "btnInstallment");
            this.btnInstallment.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnInstallment.Name = "btnInstallment";
            this.btnInstallment.Tag = "INSTALLMENT";
            this.btnInstallment.UseVisualStyleBackColor = false;
            this.btnInstallment.Click += new System.EventHandler(this.btnInstallment_Click);
            // 
            // btnPoint
            // 
            this.btnPoint.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnPoint, "btnPoint");
            this.btnPoint.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPoint.Name = "btnPoint";
            this.btnPoint.Tag = "POINTS";
            this.btnPoint.UseVisualStyleBackColor = false;
            this.btnPoint.Click += new System.EventHandler(this.btnPoint_Click);
            // 
            // btnCheque
            // 
            this.btnCheque.BackColor = System.Drawing.Color.Transparent;
            this.btnCheque.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            resources.ApplyResources(this.btnCheque, "btnCheque");
            this.btnCheque.ForeColor = System.Drawing.Color.Black;
            this.btnCheque.Name = "btnCheque";
            this.btnCheque.Tag = "CHEQUE";
            this.btnCheque.UseVisualStyleBackColor = false;
            this.btnCheque.Click += new System.EventHandler(this.btnCheque_Click);
            // 
            // btn10
            // 
            resources.ApplyResources(this.btn10, "btn10");
            this.btn10.CausesValidation = false;
            this.btn10.ForeColor = System.Drawing.Color.White;
            this.btn10.Name = "btn10";
            this.btn10.Tag = "10";
            this.btn10.UseVisualStyleBackColor = true;
            this.btn10.Click += new System.EventHandler(this.btnDirectAmount_Click);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Name = "label13";
            // 
            // btn5
            // 
            resources.ApplyResources(this.btn5, "btn5");
            this.btn5.CausesValidation = false;
            this.btn5.ForeColor = System.Drawing.Color.White;
            this.btn5.Name = "btn5";
            this.btn5.Tag = "5";
            this.btn5.UseVisualStyleBackColor = true;
            this.btn5.Click += new System.EventHandler(this.btnDirectAmount_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // btnPlus
            // 
            resources.ApplyResources(this.btnPlus, "btnPlus");
            this.btnPlus.ForeColor = System.Drawing.Color.White;
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btn2
            // 
            resources.ApplyResources(this.btn2, "btn2");
            this.btn2.CausesValidation = false;
            this.btn2.ForeColor = System.Drawing.Color.White;
            this.btn2.Name = "btn2";
            this.btn2.Tag = "2";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btnDirectAmount_Click);
            // 
            // btn50
            // 
            resources.ApplyResources(this.btn50, "btn50");
            this.btn50.CausesValidation = false;
            this.btn50.ForeColor = System.Drawing.Color.White;
            this.btn50.Name = "btn50";
            this.btn50.Tag = "50";
            this.btn50.UseVisualStyleBackColor = true;
            this.btn50.Click += new System.EventHandler(this.btnDirectAmount_Click);
            // 
            // btnMin
            // 
            resources.ApplyResources(this.btnMin, "btnMin");
            this.btnMin.ForeColor = System.Drawing.Color.White;
            this.btnMin.Name = "btnMin";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnCLEAR
            // 
            resources.ApplyResources(this.btnCLEAR, "btnCLEAR");
            this.btnCLEAR.ForeColor = System.Drawing.Color.White;
            this.btnCLEAR.Name = "btnCLEAR";
            this.btnCLEAR.UseVisualStyleBackColor = true;
            this.btnCLEAR.Click += new System.EventHandler(this.btnCLEAR_Click);
            // 
            // btnBackSpace
            // 
            resources.ApplyResources(this.btnBackSpace, "btnBackSpace");
            this.btnBackSpace.ForeColor = System.Drawing.Color.White;
            this.btnBackSpace.Name = "btnBackSpace";
            this.btnBackSpace.UseVisualStyleBackColor = true;
            this.btnBackSpace.Click += new System.EventHandler(this.btnBackSpace_Click);
            // 
            // btnDot
            // 
            resources.ApplyResources(this.btnDot, "btnDot");
            this.btnDot.ForeColor = System.Drawing.Color.White;
            this.btnDot.Name = "btnDot";
            this.btnDot.UseVisualStyleBackColor = true;
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.SlateGray;
            resources.ApplyResources(this.button10, "button10");
            this.button10.ForeColor = System.Drawing.Color.White;
            this.button10.Name = "button10";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button7
            // 
            resources.ApplyResources(this.button7, "button7");
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.Name = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button8
            // 
            resources.ApplyResources(this.button8, "button8");
            this.button8.ForeColor = System.Drawing.Color.White;
            this.button8.Name = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button9
            // 
            resources.ApplyResources(this.button9, "button9");
            this.button9.ForeColor = System.Drawing.Color.White;
            this.button9.Name = "button9";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Name = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btn1
            // 
            resources.ApplyResources(this.btn1, "btn1");
            this.btn1.ForeColor = System.Drawing.Color.White;
            this.btn1.Name = "btn1";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnConfirm);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // frmQuickPayment
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dgvPayment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmQuickPayment";
            this.Load += new System.EventHandler(this.frmPartialPayment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayment)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flpDelPay.ResumeLayout(false);
            this.flpLabelPay.ResumeLayout(false);
            this.pnlCash.ResumeLayout(false);
            this.pnlCash.PerformLayout();
            this.pnlInstallment.ResumeLayout(false);
            this.pnlInstallment.PerformLayout();
            this.pnlPoints.ResumeLayout(false);
            this.pnlPoints.PerformLayout();
            this.pnlCheque.ResumeLayout(false);
            this.pnlCheque.PerformLayout();
            this.flpButtonPay.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvPayment;
        private System.Windows.Forms.TextBox txtAmt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblAmount;
        internal System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label lblTotalPaid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label lblShortFall;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblNumOfItems;
        private System.Windows.Forms.Label lblRefNo;
        private System.Windows.Forms.CheckBox cbRounding;
        private System.Windows.Forms.DataGridViewButtonColumn del;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.Button btnPlus;
        internal System.Windows.Forms.Button btnMin;
        internal System.Windows.Forms.Button btnCLEAR;
        internal System.Windows.Forms.Button btnBackSpace;
        internal System.Windows.Forms.Button btnDot;
        internal System.Windows.Forms.Button button10;
        internal System.Windows.Forms.Button button7;
        internal System.Windows.Forms.Button button8;
        internal System.Windows.Forms.Button button9;
        internal System.Windows.Forms.Button button4;
        internal System.Windows.Forms.Button button3;
        internal System.Windows.Forms.Button button5;
        internal System.Windows.Forms.Button button2;
        internal System.Windows.Forms.Button button6;
        internal System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn10;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn50;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flpButtonPay;
        private System.Windows.Forms.FlowLayoutPanel flpLabelPay;
        private System.Windows.Forms.FlowLayoutPanel flpDelPay;
        internal System.Windows.Forms.Button btnDelCASH;
        private System.Windows.Forms.Panel pnlCash;
        private System.Windows.Forms.Label lblPayCash;
        internal System.Windows.Forms.Button btnCash;
        private System.Windows.Forms.Panel pnlInstallment;
        private System.Windows.Forms.Label lblPayInstallment1;
        internal System.Windows.Forms.Button btnInstallment;
        internal System.Windows.Forms.Button btnDelInstallment;
        internal System.Windows.Forms.Button btnDelPoints;
        private System.Windows.Forms.Panel pnlPoints;
        private System.Windows.Forms.Label lblPayPoints;
        private System.Windows.Forms.Panel pnlCheque;
        private System.Windows.Forms.Label lblPayCheque;
        internal System.Windows.Forms.Button btnPoint;
        internal System.Windows.Forms.Button btnDelCheque;
        internal System.Windows.Forms.Button btnCheque;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblTotalQty;

    }
}