namespace WinPowerPOS.OrderForms
{
    partial class frmCashPaymentLoading
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
        private void InitializeComponent(string instruction)
        {
            this.btnNETSATMCard = new System.Windows.Forms.Button();
            this.lbl1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblReceived = new System.Windows.Forms.Label();
            this.bwDeposit = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnNETSATMCard
            // 
            this.btnNETSATMCard.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.btnNETSATMCard.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnNETSATMCard.ForeColor = System.Drawing.Color.Black;
            this.btnNETSATMCard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNETSATMCard.Location = new System.Drawing.Point(194, 201);
            this.btnNETSATMCard.Margin = new System.Windows.Forms.Padding(4);
            this.btnNETSATMCard.Name = "btnNETSATMCard";
            this.btnNETSATMCard.Size = new System.Drawing.Size(133, 48);
            this.btnNETSATMCard.TabIndex = 79;
            this.btnNETSATMCard.Tag = "2";
            this.btnNETSATMCard.Text = "Cancel";
            this.btnNETSATMCard.UseVisualStyleBackColor = true;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.BackColor = System.Drawing.Color.Transparent;
            this.lbl1.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.lbl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl1.Location = new System.Drawing.Point(30, 26);
            this.lbl1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(74, 18);
            this.lbl1.TabIndex = 81;
            this.lbl1.Text = "Amount : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(30, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 18);
            this.label1.TabIndex = 82;
            this.label1.Text = "Received :";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblAmount.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.lblAmount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblAmount.Location = new System.Drawing.Point(112, 26);
            this.lblAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(74, 18);
            this.lblAmount.TabIndex = 83;
            this.lblAmount.Text = "Amount : ";
            // 
            // lblReceived
            // 
            this.lblReceived.AutoSize = true;
            this.lblReceived.BackColor = System.Drawing.Color.Transparent;
            this.lblReceived.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.lblReceived.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblReceived.Location = new System.Drawing.Point(112, 53);
            this.lblReceived.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReceived.Name = "lblReceived";
            this.lblReceived.Size = new System.Drawing.Size(74, 18);
            this.lblReceived.TabIndex = 84;
            this.lblReceived.Text = "Amount : ";
            // 
            // bwDeposit
            // 
            this.bwDeposit.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwDeposit_DoWork);
            this.bwDeposit.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwDeposit_RunWorkerCompleted);
            // 
            // frmCashPaymentLoading
            // 
            this.ClientSize = new System.Drawing.Size(340, 262);
            this.Controls.Add(this.lblReceived);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.btnNETSATMCard);
            this.Name = "frmCashPaymentLoading";
            this.Load += new System.EventHandler(this.frmCashPaymentLoading_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
        private System.Windows.Forms.TextBox textBox1;
        internal System.Windows.Forms.Button btnNETSATMCard;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblReceived;
        private System.ComponentModel.BackgroundWorker bwDeposit;
    }
}