namespace WinPowerPOS.MembershipForms
{
    partial class frmSelectInstallmentPay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectInstallmentPay));
            this.btnCreditNote = new System.Windows.Forms.Button();
            this.btnNormalPayment = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreditNote
            // 
            this.btnCreditNote.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCreditNote.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnCreditNote.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCreditNote.ForeColor = System.Drawing.Color.Orange;
            this.btnCreditNote.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCreditNote.Location = new System.Drawing.Point(184, 33);
            this.btnCreditNote.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreditNote.Name = "btnCreditNote";
            this.btnCreditNote.Size = new System.Drawing.Size(133, 74);
            this.btnCreditNote.TabIndex = 28;
            this.btnCreditNote.Tag = "1";
            this.btnCreditNote.Text = "CREDIT NOTE";
            this.btnCreditNote.UseVisualStyleBackColor = true;
            this.btnCreditNote.Click += new System.EventHandler(this.btnCreditNote_Click);
            // 
            // btnNormalPayment
            // 
            this.btnNormalPayment.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNormalPayment.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNormalPayment.BackgroundImage")));
            this.btnNormalPayment.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnNormalPayment.ForeColor = System.Drawing.Color.White;
            this.btnNormalPayment.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNormalPayment.Location = new System.Drawing.Point(43, 33);
            this.btnNormalPayment.Margin = new System.Windows.Forms.Padding(4);
            this.btnNormalPayment.Name = "btnNormalPayment";
            this.btnNormalPayment.Size = new System.Drawing.Size(133, 74);
            this.btnNormalPayment.TabIndex = 29;
            this.btnNormalPayment.Text = "NORMAL PAYMENT";
            this.btnNormalPayment.UseVisualStyleBackColor = true;
            this.btnNormalPayment.Click += new System.EventHandler(this.btnNormalPayment_Click);
            // 
            // frmSelectInstallmentPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 140);
            this.Controls.Add(this.btnNormalPayment);
            this.Controls.Add(this.btnCreditNote);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectInstallmentPay";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Installment Payment";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnCreditNote;
        internal System.Windows.Forms.Button btnNormalPayment;
    }
}