namespace WinPowerPOS.OrderForms
{
    partial class frmErrorMessage
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnTemp = new System.Windows.Forms.Button();
            this.pnlVoucherPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlVoucherPanel
            // 
            this.pnlVoucherPanel.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.pnlVoucherPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVoucherPanel.Controls.Add(this.btnTemp);
            this.pnlVoucherPanel.Controls.Add(this.lblMsg);
            this.pnlVoucherPanel.Controls.Add(this.btnOK);
            this.pnlVoucherPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlVoucherPanel.Name = "pnlVoucherPanel";
            this.pnlVoucherPanel.Size = new System.Drawing.Size(311, 148);
            this.pnlVoucherPanel.TabIndex = 78;
            this.pnlVoucherPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlVoucherPanel_Paint);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(23, 35);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(48, 16);
            this.lblMsg.TabIndex = 1;
            this.lblMsg.Text = "lblMsg";
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnOK.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(112, 80);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 41);
            this.btnOK.TabIndex = 2;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnSellVouchers_Click);
            this.btnOK.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnOK_KeyDown);
            // 
            // btnTemp
            // 
            this.btnTemp.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnTemp.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnTemp.ForeColor = System.Drawing.Color.Black;
            this.btnTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTemp.Location = new System.Drawing.Point(-1, -1);
            this.btnTemp.Name = "btnTemp";
            this.btnTemp.Size = new System.Drawing.Size(10, 10);
            this.btnTemp.TabIndex = 0;
            this.btnTemp.Text = "OK";
            this.btnTemp.UseVisualStyleBackColor = true;
            // 
            // frmErrorMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 148);
            this.ControlBox = false;
            this.Controls.Add(this.pnlVoucherPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmErrorMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmErrorMessage_Load);
            this.pnlVoucherPanel.ResumeLayout(false);
            this.pnlVoucherPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVoucherPanel;
        internal System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblMsg;
        internal System.Windows.Forms.Button btnTemp;
    }
}