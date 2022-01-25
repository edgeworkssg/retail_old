namespace WinPowerPOS.OrderForms
{
    partial class frmVoucherDialog
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
            this.pnlVoucherPanel = new System.Windows.Forms.Panel();
            this.btnRedeemVoucher = new System.Windows.Forms.Button();
            this.btnSellVouchers = new System.Windows.Forms.Button();
            this.pnlVoucherPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlVoucherPanel
            // 
            this.pnlVoucherPanel.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.pnlVoucherPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVoucherPanel.Controls.Add(this.btnRedeemVoucher);
            this.pnlVoucherPanel.Controls.Add(this.btnSellVouchers);
            this.pnlVoucherPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlVoucherPanel.Name = "pnlVoucherPanel";
            this.pnlVoucherPanel.Size = new System.Drawing.Size(322, 148);
            this.pnlVoucherPanel.TabIndex = 78;
            // 
            // btnRedeemVoucher
            // 
            this.btnRedeemVoucher.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.btnRedeemVoucher.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnRedeemVoucher.ForeColor = System.Drawing.Color.Black;
            this.btnRedeemVoucher.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRedeemVoucher.Location = new System.Drawing.Point(164, 33);
            this.btnRedeemVoucher.Name = "btnRedeemVoucher";
            this.btnRedeemVoucher.Size = new System.Drawing.Size(113, 69);
            this.btnRedeemVoucher.TabIndex = 74;
            this.btnRedeemVoucher.Text = "Redeem Voucher";
            this.btnRedeemVoucher.UseVisualStyleBackColor = true;
            this.btnRedeemVoucher.Click += new System.EventHandler(this.btnRedeemVoucher_Click);
            // 
            // btnSellVouchers
            // 
            this.btnSellVouchers.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnSellVouchers.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnSellVouchers.ForeColor = System.Drawing.Color.Black;
            this.btnSellVouchers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSellVouchers.Location = new System.Drawing.Point(37, 33);
            this.btnSellVouchers.Name = "btnSellVouchers";
            this.btnSellVouchers.Size = new System.Drawing.Size(113, 69);
            this.btnSellVouchers.TabIndex = 73;
            this.btnSellVouchers.Text = "Sell Voucher";
            this.btnSellVouchers.UseVisualStyleBackColor = true;
            this.btnSellVouchers.Click += new System.EventHandler(this.btnSellVouchers_Click);
            // 
            // frmVoucherDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 148);
            this.ControlBox = false;
            this.Controls.Add(this.pnlVoucherPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmVoucherDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.pnlVoucherPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVoucherPanel;
        internal System.Windows.Forms.Button btnRedeemVoucher;
        internal System.Windows.Forms.Button btnSellVouchers;
    }
}