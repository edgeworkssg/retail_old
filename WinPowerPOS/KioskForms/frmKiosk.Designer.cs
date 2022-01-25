namespace WinPowerPOS.KioskForms
{
    partial class frmKiosk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKiosk));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel18 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.hostOrder = new System.Windows.Forms.Integration.ElementHost();
            this.lvOrders = new WinPowerPOS.KioskForms.OrderList();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.panelKeyCode = new System.Windows.Forms.Panel();
            this.btnLog = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.hostVideo = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlVideo = new WinPowerPOS.KioskForms.VideoControl();
            this.panelPaymentList = new System.Windows.Forms.Panel();
            this.elementHost6 = new System.Windows.Forms.Integration.ElementHost();
            this.paymentPanel = new WinPowerPOS.KioskForms.PaymentPanel();
            this.panel23 = new System.Windows.Forms.Panel();
            this.elementHost9 = new System.Windows.Forms.Integration.ElementHost();
            this.keyCode = new WinPowerPOS.KioskForms.KeyCode();
            this.panelNetsPayment = new System.Windows.Forms.Panel();
            this.elementHost7 = new System.Windows.Forms.Integration.ElementHost();
            this.videoControl2 = new WinPowerPOS.KioskForms.VideoControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel21 = new System.Windows.Forms.Panel();
            this.elementHost8 = new System.Windows.Forms.Integration.ElementHost();
            this.btnSelectOtherPayment = new WinPowerPOS.KioskForms.SelectOtherPaymentButton();
            this.panel15 = new System.Windows.Forms.Panel();
            this.elementHost4 = new System.Windows.Forms.Integration.ElementHost();
            this.btnCancel = new WinPowerPOS.KioskForms.CancelButton();
            this.panel13 = new System.Windows.Forms.Panel();
            this.elementHost3 = new System.Windows.Forms.Integration.ElementHost();
            this.btnCheckOut = new WinPowerPOS.KioskForms.CheckOutButton();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlPaidAmount = new System.Windows.Forms.Panel();
            this.elementHost5 = new System.Windows.Forms.Integration.ElementHost();
            this.summaryPanel = new WinPowerPOS.KioskForms.SummaryPanel();
            this.pnlFunctionMenu = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.hostKioskStaff = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlStaff = new WinPowerPOS.KioskForms.CircleButton();
            this.hostLogOut = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlLogOut = new WinPowerPOS.KioskForms.CircleButton();
            this.hostKeyCode = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlKeyCode = new WinPowerPOS.KioskForms.DefaultButton();
            this.timerThankYou = new System.Windows.Forms.Timer(this.components);
            this.panel22 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.bwNets = new System.ComponentModel.BackgroundWorker();
            this.timerIdle = new System.Windows.Forms.Timer(this.components);
            this.bwBarcode = new System.ComponentModel.BackgroundWorker();
            this.pnlStaffFunction = new System.Windows.Forms.Panel();
            this.tblStaffFunction = new System.Windows.Forms.TableLayoutPanel();
            this.hostHostBagging = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlResetBagging = new WinPowerPOS.KioskForms.CircleButton();
            this.hostReprint = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlReprint = new WinPowerPOS.KioskForms.CircleButton();
            this.hostCloseStaffFunction = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlCloseStaffFunction = new WinPowerPOS.KioskForms.CircleButton();
            this.hostResetTransaction = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlResetTransaction = new WinPowerPOS.KioskForms.CircleButton();
            this.hostCheckOut = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlCheckOutKiosk = new WinPowerPOS.KioskForms.CircleButton();
            this.timerChange = new System.Windows.Forms.Timer(this.components);
            this.hostScanner = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlScanner = new WinPowerPOS.KioskForms.ScanBarcode();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel18.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel8.SuspendLayout();
            this.panelKeyCode.SuspendLayout();
            this.panelPaymentList.SuspendLayout();
            this.panel23.SuspendLayout();
            this.panelNetsPayment.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel21.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel9.SuspendLayout();
            this.pnlPaidAmount.SuspendLayout();
            this.pnlFunctionMenu.SuspendLayout();
            this.panel22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlStaffFunction.SuspendLayout();
            this.tblStaffFunction.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1024, 607);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(683, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(341, 607);
            this.panel3.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel11);
            this.panel7.Controls.Add(this.panel10);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 96);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(341, 511);
            this.panel7.TabIndex = 1;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(154)))), ((int)(((byte)(215)))));
            this.panel11.Controls.Add(this.panel18);
            this.panel11.Controls.Add(this.panel12);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(0, 472);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(341, 39);
            this.panel11.TabIndex = 1;
            // 
            // panel18
            // 
            this.panel18.Controls.Add(this.label6);
            this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel18.Location = new System.Drawing.Point(143, 0);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(198, 39);
            this.panel18.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.MaximumSize = new System.Drawing.Size(198, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(198, 39);
            this.label6.TabIndex = 2;
            this.label6.Text = "TOTAL: $0.00";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(154)))), ((int)(((byte)(215)))));
            this.panel12.Controls.Add(this.label3);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(143, 39);
            this.panel12.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = "TAX: $0.00";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.hostOrder);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(341, 472);
            this.panel10.TabIndex = 0;
            // 
            // hostOrder
            // 
            this.hostOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hostOrder.Location = new System.Drawing.Point(0, 0);
            this.hostOrder.Name = "hostOrder";
            this.hostOrder.Size = new System.Drawing.Size(341, 472);
            this.hostOrder.TabIndex = 0;
            this.hostOrder.TabStop = false;
            this.hostOrder.Text = "elementHost1";
            this.hostOrder.Child = this.lvOrders;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(341, 96);
            this.panel4.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(113, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(228, 96);
            this.panel6.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 96);
            this.label1.TabIndex = 0;
            this.label1.Text = "Your shopping cart is empty";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.pictureBox2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(10);
            this.panel5.Size = new System.Drawing.Size(113, 96);
            this.panel5.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(10, 10);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(93, 76);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.txtBarcode);
            this.panel8.Controls.Add(this.panelKeyCode);
            this.panel8.Controls.Add(this.panelPaymentList);
            this.panel8.Controls.Add(this.panel23);
            this.panel8.Controls.Add(this.panelNetsPayment);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(683, 607);
            this.panel8.TabIndex = 1;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(-1000, 220);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(100, 20);
            this.txtBarcode.TabIndex = 1;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // panelKeyCode
            // 
            this.panelKeyCode.Controls.Add(this.btnLog);
            this.panelKeyCode.Controls.Add(this.txtLog);
            this.panelKeyCode.Controls.Add(this.hostVideo);
            this.panelKeyCode.Location = new System.Drawing.Point(0, 0);
            this.panelKeyCode.Name = "panelKeyCode";
            this.panelKeyCode.Size = new System.Drawing.Size(683, 607);
            this.panelKeyCode.TabIndex = 3;
            // 
            // btnLog
            // 
            this.btnLog.AutoSize = true;
            this.btnLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLog.Location = new System.Drawing.Point(575, 12);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(100, 39);
            this.btnLog.TabIndex = 4;
            this.btnLog.TabStop = false;
            this.btnLog.Text = "LOG";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(147, 12);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(417, 482);
            this.txtLog.TabIndex = 3;
            this.txtLog.TabStop = false;
            this.txtLog.Visible = false;
            // 
            // hostVideo
            // 
            this.hostVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hostVideo.Location = new System.Drawing.Point(0, 0);
            this.hostVideo.Name = "hostVideo";
            this.hostVideo.Size = new System.Drawing.Size(683, 607);
            this.hostVideo.TabIndex = 2;
            this.hostVideo.TabStop = false;
            this.hostVideo.Text = "elementHost2";
            this.hostVideo.Child = this.ctrlVideo;
            // 
            // panelPaymentList
            // 
            this.panelPaymentList.Controls.Add(this.elementHost6);
            this.panelPaymentList.Location = new System.Drawing.Point(0, 0);
            this.panelPaymentList.Name = "panelPaymentList";
            this.panelPaymentList.Size = new System.Drawing.Size(683, 607);
            this.panelPaymentList.TabIndex = 4;
            // 
            // elementHost6
            // 
            this.elementHost6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost6.Location = new System.Drawing.Point(0, 0);
            this.elementHost6.Name = "elementHost6";
            this.elementHost6.Size = new System.Drawing.Size(683, 607);
            this.elementHost6.TabIndex = 0;
            this.elementHost6.TabStop = false;
            this.elementHost6.Text = "elementHost6";
            this.elementHost6.Child = this.paymentPanel;
            // 
            // panel23
            // 
            this.panel23.Controls.Add(this.elementHost9);
            this.panel23.Location = new System.Drawing.Point(0, 0);
            this.panel23.Name = "panel23";
            this.panel23.Size = new System.Drawing.Size(683, 607);
            this.panel23.TabIndex = 6;
            // 
            // elementHost9
            // 
            this.elementHost9.Location = new System.Drawing.Point(174, 55);
            this.elementHost9.Name = "elementHost9";
            this.elementHost9.Size = new System.Drawing.Size(334, 497);
            this.elementHost9.TabIndex = 0;
            this.elementHost9.TabStop = false;
            this.elementHost9.Text = "elementHost9";
            this.elementHost9.Child = this.keyCode;
            // 
            // panelNetsPayment
            // 
            this.panelNetsPayment.Controls.Add(this.elementHost7);
            this.panelNetsPayment.Location = new System.Drawing.Point(0, 0);
            this.panelNetsPayment.Name = "panelNetsPayment";
            this.panelNetsPayment.Size = new System.Drawing.Size(683, 607);
            this.panelNetsPayment.TabIndex = 5;
            // 
            // elementHost7
            // 
            this.elementHost7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost7.Location = new System.Drawing.Point(0, 0);
            this.elementHost7.Name = "elementHost7";
            this.elementHost7.Size = new System.Drawing.Size(683, 607);
            this.elementHost7.TabIndex = 0;
            this.elementHost7.Text = "elementHost7";
            this.elementHost7.Child = this.videoControl2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel21);
            this.panel2.Controls.Add(this.panel15);
            this.panel2.Controls.Add(this.panel13);
            this.panel2.Controls.Add(this.panel9);
            this.panel2.Controls.Add(this.pnlPaidAmount);
            this.panel2.Controls.Add(this.pnlFunctionMenu);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 607);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1024, 161);
            this.panel2.TabIndex = 1;
            // 
            // panel21
            // 
            this.panel21.Controls.Add(this.elementHost8);
            this.panel21.Location = new System.Drawing.Point(693, 26);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(314, 110);
            this.panel21.TabIndex = 9;
            // 
            // elementHost8
            // 
            this.elementHost8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost8.Location = new System.Drawing.Point(0, 0);
            this.elementHost8.Name = "elementHost8";
            this.elementHost8.Size = new System.Drawing.Size(314, 110);
            this.elementHost8.TabIndex = 0;
            this.elementHost8.Text = "elementHost8";
            this.elementHost8.Child = this.btnSelectOtherPayment;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.elementHost4);
            this.panel15.Location = new System.Drawing.Point(693, 26);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(314, 110);
            this.panel15.TabIndex = 7;
            // 
            // elementHost4
            // 
            this.elementHost4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost4.Location = new System.Drawing.Point(0, 0);
            this.elementHost4.Name = "elementHost4";
            this.elementHost4.Size = new System.Drawing.Size(314, 110);
            this.elementHost4.TabIndex = 0;
            this.elementHost4.TabStop = false;
            this.elementHost4.Text = "elementHost4";
            this.elementHost4.Child = this.btnCancel;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.elementHost3);
            this.panel13.Location = new System.Drawing.Point(693, 26);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(314, 110);
            this.panel13.TabIndex = 5;
            // 
            // elementHost3
            // 
            this.elementHost3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost3.Location = new System.Drawing.Point(0, 0);
            this.elementHost3.Name = "elementHost3";
            this.elementHost3.Size = new System.Drawing.Size(314, 110);
            this.elementHost3.TabIndex = 0;
            this.elementHost3.TabStop = false;
            this.elementHost3.Text = "elementHost3";
            this.elementHost3.Child = this.btnCheckOut;
            // 
            // panel9
            // 
            this.panel9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel9.BackgroundImage")));
            this.panel9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel9.Controls.Add(this.label2);
            this.panel9.Location = new System.Drawing.Point(693, 26);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(314, 116);
            this.panel9.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(58, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(236, 51);
            this.label2.TabIndex = 0;
            this.label2.Text = "Scan barcode or select item to start";
            // 
            // pnlPaidAmount
            // 
            this.pnlPaidAmount.Controls.Add(this.elementHost5);
            this.pnlPaidAmount.Location = new System.Drawing.Point(0, 0);
            this.pnlPaidAmount.Name = "pnlPaidAmount";
            this.pnlPaidAmount.Size = new System.Drawing.Size(448, 161);
            this.pnlPaidAmount.TabIndex = 8;
            // 
            // elementHost5
            // 
            this.elementHost5.Location = new System.Drawing.Point(61, 8);
            this.elementHost5.Name = "elementHost5";
            this.elementHost5.Size = new System.Drawing.Size(350, 108);
            this.elementHost5.TabIndex = 0;
            this.elementHost5.TabStop = false;
            this.elementHost5.Text = "elementHost5";
            this.elementHost5.Child = this.summaryPanel;
            // 
            // pnlFunctionMenu
            // 
            this.pnlFunctionMenu.Controls.Add(this.lblUserName);
            this.pnlFunctionMenu.Controls.Add(this.hostKioskStaff);
            this.pnlFunctionMenu.Controls.Add(this.hostLogOut);
            this.pnlFunctionMenu.Controls.Add(this.hostKeyCode);
            this.pnlFunctionMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlFunctionMenu.Name = "pnlFunctionMenu";
            this.pnlFunctionMenu.Size = new System.Drawing.Size(683, 161);
            this.pnlFunctionMenu.TabIndex = 6;
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lblUserName.Location = new System.Drawing.Point(454, 121);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(226, 34);
            this.lblUserName.TabIndex = 9;
            this.lblUserName.Text = "label5";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // hostKioskStaff
            // 
            this.hostKioskStaff.BackColor = System.Drawing.Color.White;
            this.hostKioskStaff.BackColorTransparent = true;
            this.hostKioskStaff.Location = new System.Drawing.Point(570, 6);
            this.hostKioskStaff.Name = "hostKioskStaff";
            this.hostKioskStaff.Size = new System.Drawing.Size(110, 110);
            this.hostKioskStaff.TabIndex = 5;
            this.hostKioskStaff.TabStop = false;
            this.hostKioskStaff.Text = "elementHost10";
            this.hostKioskStaff.Child = this.ctrlStaff;
            // 
            // hostLogOut
            // 
            this.hostLogOut.Location = new System.Drawing.Point(454, 6);
            this.hostLogOut.Name = "hostLogOut";
            this.hostLogOut.Size = new System.Drawing.Size(110, 110);
            this.hostLogOut.TabIndex = 8;
            this.hostLogOut.TabStop = false;
            this.hostLogOut.Text = "elementHost11";
            this.hostLogOut.Visible = false;
            this.hostLogOut.Child = this.ctrlLogOut;
            // 
            // hostKeyCode
            // 
            this.hostKeyCode.Location = new System.Drawing.Point(2, 5);
            this.hostKeyCode.Name = "hostKeyCode";
            this.hostKeyCode.Size = new System.Drawing.Size(176, 144);
            this.hostKeyCode.TabIndex = 6;
            this.hostKeyCode.TabStop = false;
            this.hostKeyCode.Text = "elementHost10";
            this.hostKeyCode.Child = this.ctrlKeyCode;
            // 
            // timerThankYou
            // 
            this.timerThankYou.Interval = 5000;
            this.timerThankYou.Tick += new System.EventHandler(this.timerThankYou_Tick);
            // 
            // panel22
            // 
            this.panel22.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel22.Controls.Add(this.pictureBox3);
            this.panel22.Controls.Add(this.label4);
            this.panel22.Controls.Add(this.pictureBox1);
            this.panel22.Location = new System.Drawing.Point(0, 0);
            this.panel22.Name = "panel22";
            this.panel22.Size = new System.Drawing.Size(1024, 768);
            this.panel22.TabIndex = 2;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(358, 625);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(308, 70);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 26F);
            this.label4.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label4.Location = new System.Drawing.Point(247, 540);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(530, 42);
            this.label4.TabIndex = 3;
            this.label4.Text = "Thank you for shopping with ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1024, 451);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // timerClose
            // 
            this.timerClose.Interval = 5000;
            this.timerClose.Tick += new System.EventHandler(this.timerClose_Tick);
            // 
            // bwNets
            // 
            this.bwNets.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwNets_DoWork);
            this.bwNets.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwNets_RunWorkerCompleted);
            // 
            // timerIdle
            // 
            this.timerIdle.Interval = 1000;
            this.timerIdle.Tick += new System.EventHandler(this.timerIdle_Tick);
            // 
            // bwBarcode
            // 
            this.bwBarcode.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwBarcode_DoWork);
            this.bwBarcode.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwBarcode_RunWorkerCompleted);
            // 
            // pnlStaffFunction
            // 
            this.pnlStaffFunction.Controls.Add(this.tblStaffFunction);
            this.pnlStaffFunction.Location = new System.Drawing.Point(118, 300);
            this.pnlStaffFunction.Name = "pnlStaffFunction";
            this.pnlStaffFunction.Size = new System.Drawing.Size(658, 158);
            this.pnlStaffFunction.TabIndex = 5;
            this.pnlStaffFunction.Visible = false;
            // 
            // tblStaffFunction
            // 
            this.tblStaffFunction.ColumnCount = 5;
            this.tblStaffFunction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblStaffFunction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblStaffFunction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblStaffFunction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblStaffFunction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblStaffFunction.Controls.Add(this.hostHostBagging, 1, 0);
            this.tblStaffFunction.Controls.Add(this.hostReprint, 1, 0);
            this.tblStaffFunction.Controls.Add(this.hostCloseStaffFunction, 4, 0);
            this.tblStaffFunction.Controls.Add(this.hostResetTransaction, 3, 0);
            this.tblStaffFunction.Controls.Add(this.hostCheckOut, 0, 0);
            this.tblStaffFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblStaffFunction.Location = new System.Drawing.Point(0, 0);
            this.tblStaffFunction.Margin = new System.Windows.Forms.Padding(10);
            this.tblStaffFunction.Name = "tblStaffFunction";
            this.tblStaffFunction.RowCount = 2;
            this.tblStaffFunction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblStaffFunction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblStaffFunction.Size = new System.Drawing.Size(658, 158);
            this.tblStaffFunction.TabIndex = 1;
            // 
            // hostHostBagging
            // 
            this.hostHostBagging.Location = new System.Drawing.Point(158, 12);
            this.hostHostBagging.Margin = new System.Windows.Forms.Padding(8, 12, 5, 5);
            this.hostHostBagging.Name = "hostHostBagging";
            this.hostHostBagging.Size = new System.Drawing.Size(135, 121);
            this.hostHostBagging.TabIndex = 3;
            this.hostHostBagging.TabStop = false;
            this.hostHostBagging.Text = "elementHost2";
            this.hostHostBagging.Child = this.ctrlResetBagging;
            // 
            // hostReprint
            // 
            this.hostReprint.Location = new System.Drawing.Point(308, 12);
            this.hostReprint.Margin = new System.Windows.Forms.Padding(8, 12, 5, 5);
            this.hostReprint.Name = "hostReprint";
            this.hostReprint.Size = new System.Drawing.Size(135, 121);
            this.hostReprint.TabIndex = 1;
            this.hostReprint.TabStop = false;
            this.hostReprint.Text = "elementHost2";
            this.hostReprint.Child = this.ctrlReprint;
            // 
            // hostCloseStaffFunction
            // 
            this.hostCloseStaffFunction.Location = new System.Drawing.Point(603, 12);
            this.hostCloseStaffFunction.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.hostCloseStaffFunction.Name = "hostCloseStaffFunction";
            this.hostCloseStaffFunction.Size = new System.Drawing.Size(52, 70);
            this.hostCloseStaffFunction.TabIndex = 2;
            this.hostCloseStaffFunction.TabStop = false;
            this.hostCloseStaffFunction.Text = "elementHost1";
            this.hostCloseStaffFunction.Child = this.ctrlCloseStaffFunction;
            // 
            // hostResetTransaction
            // 
            this.hostResetTransaction.Location = new System.Drawing.Point(458, 12);
            this.hostResetTransaction.Margin = new System.Windows.Forms.Padding(8, 12, 5, 5);
            this.hostResetTransaction.Name = "hostResetTransaction";
            this.hostResetTransaction.Size = new System.Drawing.Size(135, 121);
            this.hostResetTransaction.TabIndex = 0;
            this.hostResetTransaction.TabStop = false;
            this.hostResetTransaction.Text = "elementHost1";
            this.hostResetTransaction.Child = this.ctrlResetTransaction;
            // 
            // hostCheckOut
            // 
            this.hostCheckOut.Location = new System.Drawing.Point(8, 12);
            this.hostCheckOut.Margin = new System.Windows.Forms.Padding(8, 12, 5, 5);
            this.hostCheckOut.Name = "hostCheckOut";
            this.hostCheckOut.Size = new System.Drawing.Size(135, 121);
            this.hostCheckOut.TabIndex = 4;
            this.hostCheckOut.TabStop = false;
            this.hostCheckOut.Text = "elementHost2";
            this.hostCheckOut.Child = this.ctrlCheckOutKiosk;
            // 
            // timerChange
            // 
            this.timerChange.Interval = 1000;
            // 
            // hostScanner
            // 
            this.hostScanner.Location = new System.Drawing.Point(298, 285);
            this.hostScanner.Name = "hostScanner";
            this.hostScanner.Size = new System.Drawing.Size(488, 252);
            this.hostScanner.TabIndex = 5;
            this.hostScanner.Text = "elementHost1";
            this.hostScanner.Visible = false;
            this.hostScanner.Child = this.ctrlScanner;
            // 
            // frmKiosk
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.pnlStaffFunction);
            this.Controls.Add(this.hostScanner);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel22);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmKiosk";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmKiosk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmKiosk_Load);
            this.Shown += new System.EventHandler(this.frmKiosk_Shown);
            this.Activated += new System.EventHandler(this.frmKiosk_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmKiosk_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmKiosk_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel18.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panelKeyCode.ResumeLayout(false);
            this.panelKeyCode.PerformLayout();
            this.panelPaymentList.ResumeLayout(false);
            this.panel23.ResumeLayout(false);
            this.panelNetsPayment.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel21.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.pnlPaidAmount.ResumeLayout(false);
            this.pnlFunctionMenu.ResumeLayout(false);
            this.panel22.ResumeLayout(false);
            this.panel22.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlStaffFunction.ResumeLayout(false);
            this.tblStaffFunction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Integration.ElementHost hostOrder;
        private OrderList lvOrders;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Integration.ElementHost elementHost3;
        private CheckOutButton btnCheckOut;
        private System.Windows.Forms.Panel pnlFunctionMenu;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Integration.ElementHost elementHost4;
        private CancelButton btnCancel;
        private System.Windows.Forms.Panel pnlPaidAmount;
        private System.Windows.Forms.Integration.ElementHost elementHost5;
        private SummaryPanel summaryPanel;
        private System.Windows.Forms.Panel panelKeyCode;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.Panel panelPaymentList;
        private System.Windows.Forms.Integration.ElementHost elementHost6;
        private PaymentPanel paymentPanel;
        private System.Windows.Forms.Panel panelNetsPayment;
        private System.Windows.Forms.Integration.ElementHost elementHost7;
        private VideoControl videoControl2;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.Integration.ElementHost elementHost8;
        private SelectOtherPaymentButton btnSelectOtherPayment;
        private System.Windows.Forms.Timer timerThankYou;
        private System.Windows.Forms.Panel panel22;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Timer timerClose;
        private System.ComponentModel.BackgroundWorker bwNets;
        private System.Windows.Forms.Panel panel23;
        private System.Windows.Forms.Integration.ElementHost elementHost9;
        private KeyCode keyCode;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Timer timerIdle;
        private System.Windows.Forms.Integration.ElementHost hostKioskStaff;
        private System.Windows.Forms.Integration.ElementHost hostVideo;
        private VideoControl ctrlVideo;
        private System.Windows.Forms.Integration.ElementHost hostKeyCode;
        private System.Windows.Forms.Integration.ElementHost hostLogOut;
        private CircleButton ctrlStaff;
        private System.ComponentModel.BackgroundWorker bwBarcode;
        private DefaultButton ctrlKeyCode;
        private CircleButton ctrlLogOut;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Integration.ElementHost hostScanner;
        private System.Windows.Forms.Panel pnlStaffFunction;
        private System.Windows.Forms.TableLayoutPanel tblStaffFunction;
        private System.Windows.Forms.Integration.ElementHost hostHostBagging;
        private CircleButton ctrlResetBagging;
        private System.Windows.Forms.Integration.ElementHost hostReprint;
        private CircleButton ctrlReprint;
        private System.Windows.Forms.Integration.ElementHost hostCloseStaffFunction;
        private CircleButton ctrlCloseStaffFunction;
        private System.Windows.Forms.Integration.ElementHost hostResetTransaction;
        private CircleButton ctrlResetTransaction;
        private System.Windows.Forms.Integration.ElementHost hostCheckOut;
        private CircleButton ctrlCheckOutKiosk;
        private System.Windows.Forms.Timer timerChange;
        private ScanBarcode ctrlScanner;
    }
}