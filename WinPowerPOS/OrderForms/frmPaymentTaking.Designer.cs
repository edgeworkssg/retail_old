namespace WinPowerPOS.OrderForms
{
    partial class frmPaymentTaking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaymentTaking));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCurrSignReturn = new System.Windows.Forms.Label();
            this.lblCurrSignTotal = new System.Windows.Forms.Label();
            this.lblDefaultCurrency = new System.Windows.Forms.Label();
            this.txtCashReceived = new System.Windows.Forms.TextBox();
            this.lblChange = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ep1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblForeignCurrReturned = new System.Windows.Forms.Label();
            this.lblCurrSignForeignReturn = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblForeignCurrAmount = new System.Windows.Forms.Label();
            this.lblCurrSignAmount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtForeignAmount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblExchange = new System.Windows.Forms.Label();
            this.lblCurrSignRate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbExchange = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnPlus = new System.Windows.Forms.Button();
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
            this.label5 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblNumOfItems = new System.Windows.Forms.Label();
            this.lblRefNo = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.flpQuickCash = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "TOTAL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(8, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "COLLECTED";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblAmount.ForeColor = System.Drawing.Color.Green;
            this.lblAmount.Location = new System.Drawing.Point(139, 19);
            this.lblAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(19, 20);
            this.lblAmount.TabIndex = 2;
            this.lblAmount.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblCurrSignReturn);
            this.groupBox1.Controls.Add(this.lblCurrSignTotal);
            this.groupBox1.Controls.Add(this.lblDefaultCurrency);
            this.groupBox1.Controls.Add(this.txtCashReceived);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblChange);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblAmount);
            this.groupBox1.Location = new System.Drawing.Point(16, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(360, 138);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Payment";
            // 
            // lblCurrSignReturn
            // 
            this.lblCurrSignReturn.AutoSize = true;
            this.lblCurrSignReturn.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrSignReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrSignReturn.ForeColor = System.Drawing.Color.Green;
            this.lblCurrSignReturn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurrSignReturn.Location = new System.Drawing.Point(333, 102);
            this.lblCurrSignReturn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrSignReturn.Name = "lblCurrSignReturn";
            this.lblCurrSignReturn.Size = new System.Drawing.Size(19, 20);
            this.lblCurrSignReturn.TabIndex = 12;
            this.lblCurrSignReturn.Text = "$";
            this.lblCurrSignReturn.Visible = false;
            // 
            // lblCurrSignTotal
            // 
            this.lblCurrSignTotal.AutoSize = true;
            this.lblCurrSignTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrSignTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrSignTotal.ForeColor = System.Drawing.Color.Green;
            this.lblCurrSignTotal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurrSignTotal.Location = new System.Drawing.Point(333, 22);
            this.lblCurrSignTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrSignTotal.Name = "lblCurrSignTotal";
            this.lblCurrSignTotal.Size = new System.Drawing.Size(19, 20);
            this.lblCurrSignTotal.TabIndex = 11;
            this.lblCurrSignTotal.Text = "$";
            this.lblCurrSignTotal.Visible = false;
            // 
            // lblDefaultCurrency
            // 
            this.lblDefaultCurrency.AutoSize = true;
            this.lblDefaultCurrency.BackColor = System.Drawing.Color.Transparent;
            this.lblDefaultCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefaultCurrency.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDefaultCurrency.Location = new System.Drawing.Point(140, 47);
            this.lblDefaultCurrency.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefaultCurrency.Name = "lblDefaultCurrency";
            this.lblDefaultCurrency.Size = new System.Drawing.Size(171, 15);
            this.lblDefaultCurrency.TabIndex = 6;
            this.lblDefaultCurrency.Text = " (default currency is SGD)";
            this.lblDefaultCurrency.Visible = false;
            // 
            // txtCashReceived
            // 
            this.txtCashReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCashReceived.Location = new System.Drawing.Point(143, 73);
            this.txtCashReceived.Margin = new System.Windows.Forms.Padding(4);
            this.txtCashReceived.Name = "txtCashReceived";
            this.txtCashReceived.Size = new System.Drawing.Size(178, 26);
            this.txtCashReceived.TabIndex = 5;
            this.txtCashReceived.TextChanged += new System.EventHandler(this.txtCashReceived_TextChanged);
            this.txtCashReceived.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCashReceived_KeyDown_1);
            this.txtCashReceived.Validating += new System.ComponentModel.CancelEventHandler(this.txtCashReceived_Validating);
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.BackColor = System.Drawing.Color.Transparent;
            this.lblChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblChange.ForeColor = System.Drawing.Color.Green;
            this.lblChange.Location = new System.Drawing.Point(139, 102);
            this.lblChange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(19, 20);
            this.lblChange.TabIndex = 2;
            this.lblChange.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(8, 107);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "RETURNED";
            // 
            // ep1
            // 
            this.ep1.ContainerControl = this;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.lblForeignCurrReturned);
            this.groupBox4.Controls.Add(this.lblCurrSignForeignReturn);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.lblForeignCurrAmount);
            this.groupBox4.Controls.Add(this.lblCurrSignAmount);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.txtForeignAmount);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.lblExchange);
            this.groupBox4.Controls.Add(this.lblCurrSignRate);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.cmbExchange);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(16, 160);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(360, 193);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Foreign Currency";
            // 
            // lblForeignCurrReturned
            // 
            this.lblForeignCurrReturned.AutoSize = true;
            this.lblForeignCurrReturned.BackColor = System.Drawing.Color.Transparent;
            this.lblForeignCurrReturned.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblForeignCurrReturned.ForeColor = System.Drawing.Color.Green;
            this.lblForeignCurrReturned.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblForeignCurrReturned.Location = new System.Drawing.Point(139, 151);
            this.lblForeignCurrReturned.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForeignCurrReturned.Name = "lblForeignCurrReturned";
            this.lblForeignCurrReturned.Size = new System.Drawing.Size(15, 20);
            this.lblForeignCurrReturned.TabIndex = 13;
            this.lblForeignCurrReturned.Text = "-";
            // 
            // lblCurrSignForeignReturn
            // 
            this.lblCurrSignForeignReturn.AutoSize = true;
            this.lblCurrSignForeignReturn.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrSignForeignReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrSignForeignReturn.ForeColor = System.Drawing.Color.Green;
            this.lblCurrSignForeignReturn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurrSignForeignReturn.Location = new System.Drawing.Point(333, 148);
            this.lblCurrSignForeignReturn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrSignForeignReturn.Name = "lblCurrSignForeignReturn";
            this.lblCurrSignForeignReturn.Size = new System.Drawing.Size(19, 20);
            this.lblCurrSignForeignReturn.TabIndex = 12;
            this.lblCurrSignForeignReturn.Text = "$";
            this.lblCurrSignForeignReturn.Visible = false;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(8, 151);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 36);
            this.label8.TabIndex = 11;
            this.label8.Text = "Foreign Amount\r\nReturned";
            // 
            // lblForeignCurrAmount
            // 
            this.lblForeignCurrAmount.AutoSize = true;
            this.lblForeignCurrAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblForeignCurrAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblForeignCurrAmount.ForeColor = System.Drawing.Color.Green;
            this.lblForeignCurrAmount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblForeignCurrAmount.Location = new System.Drawing.Point(139, 73);
            this.lblForeignCurrAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForeignCurrAmount.Name = "lblForeignCurrAmount";
            this.lblForeignCurrAmount.Size = new System.Drawing.Size(15, 20);
            this.lblForeignCurrAmount.TabIndex = 10;
            this.lblForeignCurrAmount.Text = "-";
            // 
            // lblCurrSignAmount
            // 
            this.lblCurrSignAmount.AutoSize = true;
            this.lblCurrSignAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrSignAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrSignAmount.ForeColor = System.Drawing.Color.Green;
            this.lblCurrSignAmount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurrSignAmount.Location = new System.Drawing.Point(333, 73);
            this.lblCurrSignAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrSignAmount.Name = "lblCurrSignAmount";
            this.lblCurrSignAmount.Size = new System.Drawing.Size(19, 20);
            this.lblCurrSignAmount.TabIndex = 9;
            this.lblCurrSignAmount.Text = "$";
            this.lblCurrSignAmount.Visible = false;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(8, 73);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(127, 36);
            this.label10.TabIndex = 8;
            this.label10.Text = "Foreign Currency Amount";
            // 
            // txtForeignAmount
            // 
            this.txtForeignAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtForeignAmount.Location = new System.Drawing.Point(143, 112);
            this.txtForeignAmount.Margin = new System.Windows.Forms.Padding(4);
            this.txtForeignAmount.Name = "txtForeignAmount";
            this.txtForeignAmount.Size = new System.Drawing.Size(178, 26);
            this.txtForeignAmount.TabIndex = 7;
            this.txtForeignAmount.TextChanged += new System.EventHandler(this.txtForeignAmount_TextChanged);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(8, 112);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 36);
            this.label7.TabIndex = 6;
            this.label7.Text = "Foreign Amount Received";
            // 
            // lblExchange
            // 
            this.lblExchange.AutoSize = true;
            this.lblExchange.BackColor = System.Drawing.Color.Transparent;
            this.lblExchange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblExchange.ForeColor = System.Drawing.Color.Green;
            this.lblExchange.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblExchange.Location = new System.Drawing.Point(139, 47);
            this.lblExchange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExchange.Name = "lblExchange";
            this.lblExchange.Size = new System.Drawing.Size(15, 20);
            this.lblExchange.TabIndex = 5;
            this.lblExchange.Text = "-";
            // 
            // lblCurrSignRate
            // 
            this.lblCurrSignRate.AutoSize = true;
            this.lblCurrSignRate.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrSignRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrSignRate.ForeColor = System.Drawing.Color.Green;
            this.lblCurrSignRate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurrSignRate.Location = new System.Drawing.Point(333, 54);
            this.lblCurrSignRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrSignRate.Name = "lblCurrSignRate";
            this.lblCurrSignRate.Size = new System.Drawing.Size(19, 20);
            this.lblCurrSignRate.TabIndex = 4;
            this.lblCurrSignRate.Text = "$";
            this.lblCurrSignRate.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(8, 50);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Rate";
            // 
            // cmbExchange
            // 
            this.cmbExchange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExchange.FormattingEnabled = true;
            this.cmbExchange.Location = new System.Drawing.Point(143, 19);
            this.cmbExchange.Margin = new System.Windows.Forms.Padding(4);
            this.cmbExchange.Name = "cmbExchange";
            this.cmbExchange.Size = new System.Drawing.Size(178, 24);
            this.cmbExchange.TabIndex = 1;
            this.cmbExchange.SelectedIndexChanged += new System.EventHandler(this.cmbExchange_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(8, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Currency";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.btnPlus);
            this.groupBox5.Controls.Add(this.btnMin);
            this.groupBox5.Controls.Add(this.btnCLEAR);
            this.groupBox5.Controls.Add(this.btnBackSpace);
            this.groupBox5.Controls.Add(this.btnDot);
            this.groupBox5.Controls.Add(this.button10);
            this.groupBox5.Controls.Add(this.button7);
            this.groupBox5.Controls.Add(this.button8);
            this.groupBox5.Controls.Add(this.button9);
            this.groupBox5.Controls.Add(this.button4);
            this.groupBox5.Controls.Add(this.button3);
            this.groupBox5.Controls.Add(this.button5);
            this.groupBox5.Controls.Add(this.button2);
            this.groupBox5.Controls.Add(this.button6);
            this.groupBox5.Controls.Add(this.btn1);
            this.groupBox5.Location = new System.Drawing.Point(384, 14);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Size = new System.Drawing.Size(305, 340);
            this.groupBox5.TabIndex = 217;
            this.groupBox5.TabStop = false;
            // 
            // btnPlus
            // 
            this.btnPlus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlus.BackgroundImage")));
            this.btnPlus.CausesValidation = false;
            this.btnPlus.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnPlus.ForeColor = System.Drawing.Color.White;
            this.btnPlus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPlus.Location = new System.Drawing.Point(227, 12);
            this.btnPlus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(75, 78);
            this.btnPlus.TabIndex = 75;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnMin
            // 
            this.btnMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMin.BackgroundImage")));
            this.btnMin.CausesValidation = false;
            this.btnMin.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnMin.ForeColor = System.Drawing.Color.White;
            this.btnMin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMin.Location = new System.Drawing.Point(227, 90);
            this.btnMin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(75, 78);
            this.btnMin.TabIndex = 74;
            this.btnMin.Text = "-";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnCLEAR
            // 
            this.btnCLEAR.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCLEAR.BackgroundImage")));
            this.btnCLEAR.CausesValidation = false;
            this.btnCLEAR.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnCLEAR.ForeColor = System.Drawing.Color.White;
            this.btnCLEAR.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCLEAR.Location = new System.Drawing.Point(152, 244);
            this.btnCLEAR.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCLEAR.Name = "btnCLEAR";
            this.btnCLEAR.Size = new System.Drawing.Size(150, 78);
            this.btnCLEAR.TabIndex = 72;
            this.btnCLEAR.Text = "CLEAR";
            this.btnCLEAR.UseVisualStyleBackColor = true;
            this.btnCLEAR.Click += new System.EventHandler(this.btnCLEAR_Click);
            // 
            // btnBackSpace
            // 
            this.btnBackSpace.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBackSpace.BackgroundImage")));
            this.btnBackSpace.CausesValidation = false;
            this.btnBackSpace.Font = new System.Drawing.Font("Symbol", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnBackSpace.ForeColor = System.Drawing.Color.White;
            this.btnBackSpace.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBackSpace.Location = new System.Drawing.Point(227, 168);
            this.btnBackSpace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBackSpace.Name = "btnBackSpace";
            this.btnBackSpace.Size = new System.Drawing.Size(75, 78);
            this.btnBackSpace.TabIndex = 70;
            this.btnBackSpace.Text = "";
            this.btnBackSpace.UseVisualStyleBackColor = true;
            this.btnBackSpace.Click += new System.EventHandler(this.btnBackSpace_Click);
            // 
            // btnDot
            // 
            this.btnDot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDot.BackgroundImage")));
            this.btnDot.CausesValidation = false;
            this.btnDot.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnDot.ForeColor = System.Drawing.Color.White;
            this.btnDot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDot.Location = new System.Drawing.Point(2, 244);
            this.btnDot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(75, 78);
            this.btnDot.TabIndex = 69;
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = true;
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);
            // 
            // button10
            // 
            this.button10.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button10.BackgroundImage")));
            this.button10.CausesValidation = false;
            this.button10.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button10.ForeColor = System.Drawing.Color.White;
            this.button10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button10.Location = new System.Drawing.Point(77, 244);
            this.button10.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 78);
            this.button10.TabIndex = 68;
            this.button10.Text = "0";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button7
            // 
            this.button7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button7.BackgroundImage")));
            this.button7.CausesValidation = false;
            this.button7.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button7.Location = new System.Drawing.Point(152, 168);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 78);
            this.button7.TabIndex = 67;
            this.button7.Text = "9";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button8
            // 
            this.button8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button8.BackgroundImage")));
            this.button8.CausesValidation = false;
            this.button8.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button8.ForeColor = System.Drawing.Color.White;
            this.button8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button8.Location = new System.Drawing.Point(77, 168);
            this.button8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 78);
            this.button8.TabIndex = 66;
            this.button8.Text = "8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button9
            // 
            this.button9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button9.BackgroundImage")));
            this.button9.CausesValidation = false;
            this.button9.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button9.ForeColor = System.Drawing.Color.White;
            this.button9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button9.Location = new System.Drawing.Point(2, 168);
            this.button9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 78);
            this.button9.TabIndex = 65;
            this.button9.Text = "7";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button4
            // 
            this.button4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button4.BackgroundImage")));
            this.button4.CausesValidation = false;
            this.button4.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button4.Location = new System.Drawing.Point(152, 90);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 78);
            this.button4.TabIndex = 64;
            this.button4.Text = "6";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.CausesValidation = false;
            this.button3.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button3.Location = new System.Drawing.Point(152, 12);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 78);
            this.button3.TabIndex = 8;
            this.button3.Text = "3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button5
            // 
            this.button5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button5.BackgroundImage")));
            this.button5.CausesValidation = false;
            this.button5.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button5.Location = new System.Drawing.Point(77, 90);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 78);
            this.button5.TabIndex = 63;
            this.button5.Text = "5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.CausesValidation = false;
            this.button2.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(77, 12);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 78);
            this.button2.TabIndex = 7;
            this.button2.Text = "2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn1_Click);
            // 
            // button6
            // 
            this.button6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button6.BackgroundImage")));
            this.button6.CausesValidation = false;
            this.button6.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button6.Location = new System.Drawing.Point(2, 90);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 78);
            this.button6.TabIndex = 62;
            this.button6.Text = "4";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btn1
            // 
            this.btn1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn1.BackgroundImage")));
            this.btn1.CausesValidation = false;
            this.btn1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.btn1.ForeColor = System.Drawing.Color.White;
            this.btn1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn1.Location = new System.Drawing.Point(2, 12);
            this.btn1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(75, 78);
            this.btn1.TabIndex = 6;
            this.btn1.Text = "1";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(17, 357);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 18);
            this.label5.TabIndex = 219;
            this.label5.Text = "# ITEMS";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(17, 384);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 29);
            this.label12.TabIndex = 221;
            this.label12.Text = "REF NO:";
            // 
            // lblNumOfItems
            // 
            this.lblNumOfItems.AutoSize = true;
            this.lblNumOfItems.BackColor = System.Drawing.Color.Transparent;
            this.lblNumOfItems.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.lblNumOfItems.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblNumOfItems.Location = new System.Drawing.Point(91, 357);
            this.lblNumOfItems.Name = "lblNumOfItems";
            this.lblNumOfItems.Size = new System.Drawing.Size(13, 18);
            this.lblNumOfItems.TabIndex = 220;
            this.lblNumOfItems.Text = "-";
            // 
            // lblRefNo
            // 
            this.lblRefNo.AutoSize = true;
            this.lblRefNo.BackColor = System.Drawing.Color.Transparent;
            this.lblRefNo.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblRefNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblRefNo.Location = new System.Drawing.Point(126, 384);
            this.lblRefNo.Name = "lblRefNo";
            this.lblRefNo.Size = new System.Drawing.Size(23, 29);
            this.lblRefNo.TabIndex = 222;
            this.lblRefNo.Text = "-";
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.CausesValidation = false;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(589, 363);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 50);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::WinPowerPOS.Properties.Resources.buttonGold;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Location = new System.Drawing.Point(481, 363);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 50);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // flpQuickCash
            // 
            this.flpQuickCash.AutoScroll = true;
            this.flpQuickCash.BackColor = System.Drawing.Color.Transparent;
            this.flpQuickCash.Location = new System.Drawing.Point(689, 21);
            this.flpQuickCash.Name = "flpQuickCash";
            this.flpQuickCash.Size = new System.Drawing.Size(152, 392);
            this.flpQuickCash.TabIndex = 223;
            // 
            // frmPaymentTaking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 419);
            this.ControlBox = false;
            this.Controls.Add(this.flpQuickCash);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblNumOfItems);
            this.Controls.Add(this.lblRefNo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPaymentTaking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cash Payment";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PaymentTaking_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider ep1;
        private System.Windows.Forms.TextBox txtCashReceived;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbExchange;
        public System.Windows.Forms.Label lblExchange;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox5;
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblNumOfItems;
        private System.Windows.Forms.Label lblRefNo;
        private System.Windows.Forms.TextBox txtForeignAmount;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label lblDefaultCurrency;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.Label lblForeignCurrAmount;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label lblForeignCurrReturned;
        public System.Windows.Forms.Label lblCurrSignReturn;
        public System.Windows.Forms.Label lblCurrSignTotal;
        public System.Windows.Forms.Label lblCurrSignForeignReturn;
        public System.Windows.Forms.Label lblCurrSignAmount;
        public System.Windows.Forms.Label lblCurrSignRate;
        private System.Windows.Forms.FlowLayoutPanel flpQuickCash;
    }
}