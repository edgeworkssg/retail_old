namespace WinPowerPOS.OrderForms
{
    partial class frmErrorMessageWithTextArea
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnTemp = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.TextBox();
            this.pnlVoucherPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlVoucherPanel
            // 
            this.pnlVoucherPanel.AutoSize = true;
            this.pnlVoucherPanel.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.pnlVoucherPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVoucherPanel.Controls.Add(this.panel2);
            this.pnlVoucherPanel.Controls.Add(this.panel1);
            this.pnlVoucherPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVoucherPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlVoucherPanel.Name = "pnlVoucherPanel";
            this.pnlVoucherPanel.Size = new System.Drawing.Size(348, 198);
            this.pnlVoucherPanel.TabIndex = 78;
            this.pnlVoucherPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlVoucherPanel_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(346, 196);
            this.panel2.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnTemp);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(346, 25);
            this.panel5.TabIndex = 5;
            // 
            // btnTemp
            // 
            this.btnTemp.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnTemp.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnTemp.ForeColor = System.Drawing.Color.Black;
            this.btnTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTemp.Location = new System.Drawing.Point(3, 3);
            this.btnTemp.Name = "btnTemp";
            this.btnTemp.Size = new System.Drawing.Size(10, 10);
            this.btnTemp.TabIndex = 0;
            this.btnTemp.Text = "OK";
            this.btnTemp.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.Controls.Add(this.lblMsg);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 29);
            this.panel4.MinimumSize = new System.Drawing.Size(309, 120);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(346, 120);
            this.panel4.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 149);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(346, 47);
            this.panel3.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnOK.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(139, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 41);
            this.btnOK.TabIndex = 2;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnSellVouchers_Click);
            this.btnOK.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnOK_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Location = new System.Drawing.Point(11, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(0, 0);
            this.panel1.TabIndex = 3;
            // 
            // lblMsg
            // 
            this.lblMsg.Location = new System.Drawing.Point(3, 4);
            this.lblMsg.MinimumSize = new System.Drawing.Size(340, 110);
            this.lblMsg.Multiline = true;
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.ReadOnly = true;
            this.lblMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lblMsg.Size = new System.Drawing.Size(340, 110);
            this.lblMsg.TabIndex = 0;
            // 
            // frmErrorMessageWithTextArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(348, 198);
            this.ControlBox = false;
            this.Controls.Add(this.pnlVoucherPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmErrorMessageWithTextArea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmErrorMessage_Load);
            this.pnlVoucherPanel.ResumeLayout(false);
            this.pnlVoucherPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlVoucherPanel;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnTemp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox lblMsg;
    }
}