namespace WinPowerPOS.MembershipForms
{
    partial class frmSelectCancelPreOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectCancelPreOrder));
            this.btnReturnDeposit = new System.Windows.Forms.Button();
            this.btnNormalPayment = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReturnDeposit
            // 
            this.btnReturnDeposit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReturnDeposit.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnReturnDeposit.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnReturnDeposit.ForeColor = System.Drawing.Color.Orange;
            this.btnReturnDeposit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnReturnDeposit.Location = new System.Drawing.Point(184, 33);
            this.btnReturnDeposit.Margin = new System.Windows.Forms.Padding(4);
            this.btnReturnDeposit.Name = "btnReturnDeposit";
            this.btnReturnDeposit.Size = new System.Drawing.Size(133, 74);
            this.btnReturnDeposit.TabIndex = 28;
            this.btnReturnDeposit.Tag = "1";
            this.btnReturnDeposit.Text = "RETURN DEPOSIT";
            this.btnReturnDeposit.UseVisualStyleBackColor = true;
            this.btnReturnDeposit.Click += new System.EventHandler(this.btnCreditNote_Click);
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
            this.btnNormalPayment.Text = "KEEP DEPOSIT";
            this.btnNormalPayment.UseVisualStyleBackColor = true;
            this.btnNormalPayment.Click += new System.EventHandler(this.btnNormalPayment_Click);
            // 
            // frmSelectCancelPreOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 140);
            this.ControlBox = false;
            this.Controls.Add(this.btnNormalPayment);
            this.Controls.Add(this.btnReturnDeposit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectCancelPreOrder";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cancel Pre Order";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnReturnDeposit;
        internal System.Windows.Forms.Button btnNormalPayment;
    }
}