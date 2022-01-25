namespace WinPowerPOS.OrderForms
{
    partial class frmSelectPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectPayment));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCash = new System.Windows.Forms.Button();
            this.btnPay1 = new System.Windows.Forms.Button();
            this.btnPay2 = new System.Windows.Forms.Button();
            this.btnPay3 = new System.Windows.Forms.Button();
            this.btnPay4 = new System.Windows.Forms.Button();
            this.btnPay5 = new System.Windows.Forms.Button();
            this.btnPay6 = new System.Windows.Forms.Button();
            this.btnPay7 = new System.Windows.Forms.Button();
            this.btnPay8 = new System.Windows.Forms.Button();
            this.btnPay9 = new System.Windows.Forms.Button();
            this.btnPay10 = new System.Windows.Forms.Button();
            this.btnOther = new System.Windows.Forms.Button();
            this.btnPoints = new System.Windows.Forms.Button();
            this.btnInstallment = new System.Windows.Forms.Button();
            this.btnPAMedifund = new System.Windows.Forms.Button();
            this.btnSMF = new System.Windows.Forms.Button();
            this.btnPWF = new System.Windows.Forms.Button();
            this.btnCreditCard = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNETSATMCard = new System.Windows.Forms.Button();
            this.btnFlashPay = new System.Windows.Forms.Button();
            this.btnCashCard = new System.Windows.Forms.Button();
            this.lblNumOfItems = new System.Windows.Forms.Label();
            this.lblRefNo = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlNETSIntegration = new System.Windows.Forms.Panel();
            this.btnNETSQR = new System.Windows.Forms.Button();
            this.btnNETSBack = new System.Windows.Forms.Button();
            this.bgDownloadPoints = new System.ComponentModel.BackgroundWorker();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlNETSIntegration.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
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
            this.flowLayoutPanel1.Controls.Add(this.btnOther);
            this.flowLayoutPanel1.Controls.Add(this.btnPoints);
            this.flowLayoutPanel1.Controls.Add(this.btnInstallment);
            this.flowLayoutPanel1.Controls.Add(this.btnPAMedifund);
            this.flowLayoutPanel1.Controls.Add(this.btnSMF);
            this.flowLayoutPanel1.Controls.Add(this.btnPWF);
            this.flowLayoutPanel1.Controls.Add(this.btnCreditCard);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // btnCash
            // 
            resources.ApplyResources(this.btnCash, "btnCash");
            this.btnCash.ForeColor = System.Drawing.Color.White;
            this.btnCash.Name = "btnCash";
            this.btnCash.UseVisualStyleBackColor = true;
            this.btnCash.Click += new System.EventHandler(this.btnCashPayment_Click);
            // 
            // btnPay1
            // 
            this.btnPay1.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnPay1, "btnPay1");
            this.btnPay1.ForeColor = System.Drawing.Color.Orange;
            this.btnPay1.Name = "btnPay1";
            this.btnPay1.Tag = "1";
            this.btnPay1.UseVisualStyleBackColor = true;
            this.btnPay1.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay2
            // 
            this.btnPay2.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            resources.ApplyResources(this.btnPay2, "btnPay2");
            this.btnPay2.ForeColor = System.Drawing.Color.Black;
            this.btnPay2.Name = "btnPay2";
            this.btnPay2.Tag = "2";
            this.btnPay2.UseVisualStyleBackColor = true;
            this.btnPay2.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay3
            // 
            this.btnPay3.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            resources.ApplyResources(this.btnPay3, "btnPay3");
            this.btnPay3.ForeColor = System.Drawing.Color.Black;
            this.btnPay3.Name = "btnPay3";
            this.btnPay3.Tag = "3";
            this.btnPay3.UseVisualStyleBackColor = true;
            this.btnPay3.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay4
            // 
            this.btnPay4.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            resources.ApplyResources(this.btnPay4, "btnPay4");
            this.btnPay4.ForeColor = System.Drawing.Color.White;
            this.btnPay4.Name = "btnPay4";
            this.btnPay4.Tag = "4";
            this.btnPay4.UseVisualStyleBackColor = true;
            this.btnPay4.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay5
            // 
            this.btnPay5.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            resources.ApplyResources(this.btnPay5, "btnPay5");
            this.btnPay5.ForeColor = System.Drawing.Color.Black;
            this.btnPay5.Name = "btnPay5";
            this.btnPay5.Tag = "5";
            this.btnPay5.UseVisualStyleBackColor = true;
            this.btnPay5.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay6
            // 
            this.btnPay6.BackgroundImage = global::WinPowerPOS.Properties.Resources.grey;
            resources.ApplyResources(this.btnPay6, "btnPay6");
            this.btnPay6.ForeColor = System.Drawing.Color.White;
            this.btnPay6.Name = "btnPay6";
            this.btnPay6.Tag = "6";
            this.btnPay6.UseVisualStyleBackColor = true;
            this.btnPay6.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay7
            // 
            this.btnPay7.BackgroundImage = global::WinPowerPOS.Properties.Resources.bluebackground;
            resources.ApplyResources(this.btnPay7, "btnPay7");
            this.btnPay7.ForeColor = System.Drawing.Color.White;
            this.btnPay7.Name = "btnPay7";
            this.btnPay7.Tag = "7";
            this.btnPay7.UseVisualStyleBackColor = true;
            this.btnPay7.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay8
            // 
            this.btnPay8.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbackground;
            resources.ApplyResources(this.btnPay8, "btnPay8");
            this.btnPay8.ForeColor = System.Drawing.Color.White;
            this.btnPay8.Name = "btnPay8";
            this.btnPay8.Tag = "8";
            this.btnPay8.UseVisualStyleBackColor = true;
            this.btnPay8.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay9
            // 
            this.btnPay9.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            resources.ApplyResources(this.btnPay9, "btnPay9");
            this.btnPay9.ForeColor = System.Drawing.Color.Black;
            this.btnPay9.Name = "btnPay9";
            this.btnPay9.Tag = "9";
            this.btnPay9.UseVisualStyleBackColor = true;
            this.btnPay9.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnPay10
            // 
            this.btnPay10.BackgroundImage = global::WinPowerPOS.Properties.Resources.logobg;
            resources.ApplyResources(this.btnPay10, "btnPay10");
            this.btnPay10.ForeColor = System.Drawing.Color.Black;
            this.btnPay10.Name = "btnPay10";
            this.btnPay10.Tag = "10";
            this.btnPay10.UseVisualStyleBackColor = true;
            this.btnPay10.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnOther
            // 
            this.btnOther.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightyellowbutton;
            resources.ApplyResources(this.btnOther, "btnOther");
            this.btnOther.ForeColor = System.Drawing.Color.Black;
            this.btnOther.Name = "btnOther";
            this.btnOther.UseVisualStyleBackColor = true;
            this.btnOther.Click += new System.EventHandler(this.btnOther_Click);
            // 
            // btnPoints
            // 
            resources.ApplyResources(this.btnPoints, "btnPoints");
            this.btnPoints.ForeColor = System.Drawing.Color.Black;
            this.btnPoints.Name = "btnPoints";
            this.btnPoints.UseVisualStyleBackColor = true;
            this.btnPoints.Click += new System.EventHandler(this.btnPoints_Click);
            // 
            // btnInstallment
            // 
            resources.ApplyResources(this.btnInstallment, "btnInstallment");
            this.btnInstallment.ForeColor = System.Drawing.Color.Black;
            this.btnInstallment.Name = "btnInstallment";
            this.btnInstallment.Tag = "INSTALLMENT";
            this.btnInstallment.UseVisualStyleBackColor = true;
            this.btnInstallment.Click += new System.EventHandler(this.btnInstallment_Click);
            // 
            // btnPAMedifund
            // 
            this.btnPAMedifund.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnPAMedifund, "btnPAMedifund");
            this.btnPAMedifund.ForeColor = System.Drawing.Color.White;
            this.btnPAMedifund.Name = "btnPAMedifund";
            this.btnPAMedifund.UseVisualStyleBackColor = true;
            this.btnPAMedifund.Click += new System.EventHandler(this.btnPAMedifund_Click);
            // 
            // btnSMF
            // 
            this.btnSMF.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightgreenbutton;
            resources.ApplyResources(this.btnSMF, "btnSMF");
            this.btnSMF.ForeColor = System.Drawing.Color.White;
            this.btnSMF.Name = "btnSMF";
            this.btnSMF.UseVisualStyleBackColor = true;
            this.btnSMF.Click += new System.EventHandler(this.btnSMF_Click);
            // 
            // btnPWF
            // 
            this.btnPWF.BackgroundImage = global::WinPowerPOS.Properties.Resources.brown;
            resources.ApplyResources(this.btnPWF, "btnPWF");
            this.btnPWF.ForeColor = System.Drawing.Color.Black;
            this.btnPWF.Name = "btnPWF";
            this.btnPWF.UseVisualStyleBackColor = true;
            this.btnPWF.Click += new System.EventHandler(this.btnPWF_Click);
            // 
            // btnCreditCard
            // 
            this.btnCreditCard.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnCreditCard, "btnCreditCard");
            this.btnCreditCard.ForeColor = System.Drawing.Color.White;
            this.btnCreditCard.Name = "btnCreditCard";
            this.btnCreditCard.UseVisualStyleBackColor = true;
            this.btnCreditCard.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNETSATMCard
            // 
            this.btnNETSATMCard.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            resources.ApplyResources(this.btnNETSATMCard, "btnNETSATMCard");
            this.btnNETSATMCard.ForeColor = System.Drawing.Color.Black;
            this.btnNETSATMCard.Name = "btnNETSATMCard";
            this.btnNETSATMCard.Tag = "2";
            this.btnNETSATMCard.UseVisualStyleBackColor = true;
            this.btnNETSATMCard.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnFlashPay
            // 
            resources.ApplyResources(this.btnFlashPay, "btnFlashPay");
            this.btnFlashPay.ForeColor = System.Drawing.Color.White;
            this.btnFlashPay.Name = "btnFlashPay";
            this.btnFlashPay.UseVisualStyleBackColor = true;
            this.btnFlashPay.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnCashCard
            // 
            this.btnCashCard.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnCashCard, "btnCashCard");
            this.btnCashCard.ForeColor = System.Drawing.Color.Orange;
            this.btnCashCard.Name = "btnCashCard";
            this.btnCashCard.Tag = "1";
            this.btnCashCard.UseVisualStyleBackColor = true;
            this.btnCashCard.Click += new System.EventHandler(this.btnMakePayment_Click);
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
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // pnlNETSIntegration
            // 
            this.pnlNETSIntegration.Controls.Add(this.btnNETSQR);
            this.pnlNETSIntegration.Controls.Add(this.btnNETSBack);
            this.pnlNETSIntegration.Controls.Add(this.btnFlashPay);
            this.pnlNETSIntegration.Controls.Add(this.btnNETSATMCard);
            this.pnlNETSIntegration.Controls.Add(this.btnCashCard);
            resources.ApplyResources(this.pnlNETSIntegration, "pnlNETSIntegration");
            this.pnlNETSIntegration.Name = "pnlNETSIntegration";
            // 
            // btnNETSQR
            // 
            resources.ApplyResources(this.btnNETSQR, "btnNETSQR");
            this.btnNETSQR.ForeColor = System.Drawing.Color.White;
            this.btnNETSQR.Name = "btnNETSQR";
            this.btnNETSQR.UseVisualStyleBackColor = true;
            this.btnNETSQR.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnNETSBack
            // 
            resources.ApplyResources(this.btnNETSBack, "btnNETSBack");
            this.btnNETSBack.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnNETSBack.ForeColor = System.Drawing.Color.White;
            this.btnNETSBack.Name = "btnNETSBack";
            this.btnNETSBack.UseVisualStyleBackColor = true;
            this.btnNETSBack.Click += new System.EventHandler(this.btnNETSBack_Click);
            // 
            // frmSelectPayment
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNumOfItems);
            this.Controls.Add(this.lblRefNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pnlNETSIntegration);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSelectPayment";
            this.Load += new System.EventHandler(this.frmSelectPayment_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlNETSIntegration.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal System.Windows.Forms.Button btnCash;
        internal System.Windows.Forms.Button btnPay1;
        internal System.Windows.Forms.Button btnPay2;
        internal System.Windows.Forms.Button btnPay3;
        internal System.Windows.Forms.Button btnOther;
        private System.Windows.Forms.Label lblNumOfItems;
        private System.Windows.Forms.Label lblRefNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Button btnPoints;
        internal System.Windows.Forms.Button btnPay4;
        internal System.Windows.Forms.Button btnPay5;
        internal System.Windows.Forms.Button btnPay6;
        internal System.Windows.Forms.Button btnInstallment;
        internal System.Windows.Forms.Button btnPAMedifund;
        internal System.Windows.Forms.Button btnSMF;
        internal System.Windows.Forms.Button btnPWF;
        internal System.Windows.Forms.Button btnNETSATMCard;
        internal System.Windows.Forms.Button btnFlashPay;
        internal System.Windows.Forms.Button btnCashCard;
        private System.Windows.Forms.Panel pnlNETSIntegration;
        internal System.Windows.Forms.Button btnNETSBack;
        internal System.Windows.Forms.Button btnPay9;
        internal System.Windows.Forms.Button btnPay8;
        internal System.Windows.Forms.Button btnPay10;
        internal System.Windows.Forms.Button btnPay7;
		private System.ComponentModel.BackgroundWorker bgDownloadPoints;
        internal System.Windows.Forms.Button btnCreditCard;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnNETSQR;
    }
}