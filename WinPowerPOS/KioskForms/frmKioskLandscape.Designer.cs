namespace SelfServiceKiosk
{
    partial class frmKioskLandscape
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKioskLandscape));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlBanner = new System.Windows.Forms.Panel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlGroupLeft = new System.Windows.Forms.Panel();
            this.pnlFinishAll = new System.Windows.Forms.Panel();
            this.ehBtnFinishAndPay = new System.Windows.Forms.Integration.ElementHost();
            this.btnFinishAndPay = new WinPowerPOS.KioskForms.FinishAndPayButtonLandscape();
            this.pnlCancelAll = new System.Windows.Forms.Panel();
            this.ehBtnCancelAll = new System.Windows.Forms.Integration.ElementHost();
            this.btnCancelAll = new WinPowerPOS.KioskForms.CancelAllButtonLandscape();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.elementHost5 = new System.Windows.Forms.Integration.ElementHost();
            this.lvOrders = new WinPowerPOS.KioskForms.OrderListLandscape();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pnlGroup2 = new System.Windows.Forms.Panel();
            this.pnlKeyCode = new System.Windows.Forms.Panel();
            this.ehBtnKeyCode = new System.Windows.Forms.Integration.ElementHost();
            this.btnKeyCode = new WinPowerPOS.KioskForms.KeyCodeButton();
            this.pnlWeight = new System.Windows.Forms.Panel();
            this.ehBtnWeight = new System.Windows.Forms.Integration.ElementHost();
            this.btnWeight = new WinPowerPOS.KioskForms.WeightButton();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlBanner.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.panel5.SuspendLayout();
            this.pnlGroupLeft.SuspendLayout();
            this.pnlFinishAll.SuspendLayout();
            this.pnlCancelAll.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlGroup2.SuspendLayout();
            this.pnlKeyCode.SuspendLayout();
            this.pnlWeight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.ImageLocation = "Resources/banner_kiosk.jpg";
            this.pictureBox1.Location = new System.Drawing.Point(94, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(14, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.LoadCompleted += new System.ComponentModel.AsyncCompletedEventHandler(this.pictureBox1_LoadCompleted);
            // 
            // pnlBanner
            // 
            this.pnlBanner.Controls.Add(this.pictureBox1);
            this.pnlBanner.Location = new System.Drawing.Point(0, 0);
            this.pnlBanner.Name = "pnlBanner";
            this.pnlBanner.Size = new System.Drawing.Size(512, 88);
            this.pnlBanner.TabIndex = 1;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.panel5);
            this.pnlLeft.Controls.Add(this.pnlGroupLeft);
            this.pnlLeft.Controls.Add(this.txtBarcode);
            this.pnlLeft.Controls.Add(this.panel4);
            this.pnlLeft.Controls.Add(this.elementHost5);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(512, 733);
            this.pnlLeft.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Location = new System.Drawing.Point(1, 617);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(512, 41);
            this.panel5.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(307, 41);
            this.label4.TabIndex = 0;
            this.label4.Text = "TOTAL : ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(512, 41);
            this.label5.TabIndex = 1;
            this.label5.Text = "0.00";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlGroupLeft
            // 
            this.pnlGroupLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGroupLeft.AutoSize = true;
            this.pnlGroupLeft.Controls.Add(this.pnlFinishAll);
            this.pnlGroupLeft.Controls.Add(this.pnlCancelAll);
            this.pnlGroupLeft.Location = new System.Drawing.Point(0, 658);
            this.pnlGroupLeft.Name = "pnlGroupLeft";
            this.pnlGroupLeft.Size = new System.Drawing.Size(512, 72);
            this.pnlGroupLeft.TabIndex = 2;
            // 
            // pnlFinishAll
            // 
            this.pnlFinishAll.Controls.Add(this.ehBtnFinishAndPay);
            this.pnlFinishAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFinishAll.Location = new System.Drawing.Point(256, 0);
            this.pnlFinishAll.Name = "pnlFinishAll";
            this.pnlFinishAll.Size = new System.Drawing.Size(256, 72);
            this.pnlFinishAll.TabIndex = 1;
            // 
            // ehBtnFinishAndPay
            // 
            this.ehBtnFinishAndPay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehBtnFinishAndPay.Location = new System.Drawing.Point(0, 0);
            this.ehBtnFinishAndPay.Name = "ehBtnFinishAndPay";
            this.ehBtnFinishAndPay.Size = new System.Drawing.Size(256, 72);
            this.ehBtnFinishAndPay.TabIndex = 0;
            this.ehBtnFinishAndPay.TabStop = false;
            this.ehBtnFinishAndPay.Text = "elementHost2";
            this.ehBtnFinishAndPay.Child = this.btnFinishAndPay;
            // 
            // pnlCancelAll
            // 
            this.pnlCancelAll.Controls.Add(this.ehBtnCancelAll);
            this.pnlCancelAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCancelAll.Location = new System.Drawing.Point(0, 0);
            this.pnlCancelAll.Name = "pnlCancelAll";
            this.pnlCancelAll.Size = new System.Drawing.Size(256, 72);
            this.pnlCancelAll.TabIndex = 0;
            // 
            // ehBtnCancelAll
            // 
            this.ehBtnCancelAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehBtnCancelAll.Location = new System.Drawing.Point(0, 0);
            this.ehBtnCancelAll.Name = "ehBtnCancelAll";
            this.ehBtnCancelAll.Size = new System.Drawing.Size(256, 72);
            this.ehBtnCancelAll.TabIndex = 0;
            this.ehBtnCancelAll.TabStop = false;
            this.ehBtnCancelAll.Text = "elementHost1";
            this.ehBtnCancelAll.Child = this.btnCancelAll;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(-1000, 67);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(321, 22);
            this.txtBarcode.TabIndex = 0;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(512, 40);
            this.panel4.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(358, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 40);
            this.label3.TabIndex = 1;
            this.label3.Text = "AMOUNT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(26, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(358, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "ITEM";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // elementHost5
            // 
            this.elementHost5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost5.Location = new System.Drawing.Point(0, 41);
            this.elementHost5.Name = "elementHost5";
            this.elementHost5.Size = new System.Drawing.Size(512, 577);
            this.elementHost5.TabIndex = 3;
            this.elementHost5.TabStop = false;
            this.elementHost5.Text = "elementHost5";
            this.elementHost5.Child = this.lvOrders;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.panel1);
            this.pnlRight.Controls.Add(this.pnlGroup2);
            this.pnlRight.Controls.Add(this.label1);
            this.pnlRight.Controls.Add(this.pnlBanner);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(512, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(500, 733);
            this.pnlRight.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Location = new System.Drawing.Point(0, 205);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.panel1.Size = new System.Drawing.Size(500, 310);
            this.panel1.TabIndex = 7;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(30, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(440, 310);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // pnlGroup2
            // 
            this.pnlGroup2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGroup2.Controls.Add(this.pnlKeyCode);
            this.pnlGroup2.Controls.Add(this.pnlWeight);
            this.pnlGroup2.Location = new System.Drawing.Point(0, 511);
            this.pnlGroup2.Name = "pnlGroup2";
            this.pnlGroup2.Size = new System.Drawing.Size(500, 108);
            this.pnlGroup2.TabIndex = 6;
            // 
            // pnlKeyCode
            // 
            this.pnlKeyCode.Controls.Add(this.ehBtnKeyCode);
            this.pnlKeyCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKeyCode.Location = new System.Drawing.Point(256, 0);
            this.pnlKeyCode.Name = "pnlKeyCode";
            this.pnlKeyCode.Padding = new System.Windows.Forms.Padding(23, 7, 30, 7);
            this.pnlKeyCode.Size = new System.Drawing.Size(244, 108);
            this.pnlKeyCode.TabIndex = 1;
            // 
            // ehBtnKeyCode
            // 
            this.ehBtnKeyCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehBtnKeyCode.Location = new System.Drawing.Point(23, 7);
            this.ehBtnKeyCode.Name = "ehBtnKeyCode";
            this.ehBtnKeyCode.Size = new System.Drawing.Size(191, 94);
            this.ehBtnKeyCode.TabIndex = 0;
            this.ehBtnKeyCode.TabStop = false;
            this.ehBtnKeyCode.Text = "elementHost4";
            this.ehBtnKeyCode.Child = this.btnKeyCode;
            // 
            // pnlWeight
            // 
            this.pnlWeight.Controls.Add(this.ehBtnWeight);
            this.pnlWeight.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlWeight.Location = new System.Drawing.Point(0, 0);
            this.pnlWeight.Name = "pnlWeight";
            this.pnlWeight.Padding = new System.Windows.Forms.Padding(30, 7, 23, 7);
            this.pnlWeight.Size = new System.Drawing.Size(256, 108);
            this.pnlWeight.TabIndex = 1;
            // 
            // ehBtnWeight
            // 
            this.ehBtnWeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehBtnWeight.Location = new System.Drawing.Point(30, 7);
            this.ehBtnWeight.Name = "ehBtnWeight";
            this.ehBtnWeight.Size = new System.Drawing.Size(203, 94);
            this.ehBtnWeight.TabIndex = 0;
            this.ehBtnWeight.TabStop = false;
            this.ehBtnWeight.Text = "elementHost3";
            this.ehBtnWeight.Child = this.btnWeight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(448, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "STEP 1 OF 2: SCAN & PACK YOUR ITEMS";
            // 
            // frmKioskLandscape
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1012, 733);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmKioskLandscape";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlBanner.ResumeLayout(false);
            this.pnlBanner.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.pnlGroupLeft.ResumeLayout(false);
            this.pnlFinishAll.ResumeLayout(false);
            this.pnlCancelAll.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlGroup2.ResumeLayout(false);
            this.pnlKeyCode.ResumeLayout(false);
            this.pnlWeight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlBanner;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlGroupLeft;
        private System.Windows.Forms.Panel pnlFinishAll;
        private System.Windows.Forms.Panel pnlCancelAll;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlGroup2;
        private System.Windows.Forms.Panel pnlKeyCode;
        private System.Windows.Forms.Panel pnlWeight;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Integration.ElementHost ehBtnFinishAndPay;
        private System.Windows.Forms.Integration.ElementHost ehBtnCancelAll;
        private System.Windows.Forms.Integration.ElementHost ehBtnWeight;
        private WinPowerPOS.KioskForms.WeightButton btnWeight;
        private System.Windows.Forms.Integration.ElementHost ehBtnKeyCode;
        private WinPowerPOS.KioskForms.KeyCodeButton btnKeyCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Integration.ElementHost elementHost5;
        private WinPowerPOS.KioskForms.CancelAllButtonLandscape btnCancelAll;
        private WinPowerPOS.KioskForms.FinishAndPayButtonLandscape btnFinishAndPay;
        private WinPowerPOS.KioskForms.OrderListLandscape lvOrders;
    }
}

