namespace WinPowerPOS.OrderForms
{
    partial class frmSelectPrintSize
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectPrintSize));
            this.btnA4 = new System.Windows.Forms.Button();
            this.btnReceipt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnA4
            // 
            this.btnA4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnA4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnA4.BackgroundImage")));
            this.btnA4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnA4.ForeColor = System.Drawing.Color.White;
            this.btnA4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnA4.Location = new System.Drawing.Point(69, 30);
            this.btnA4.Margin = new System.Windows.Forms.Padding(8);
            this.btnA4.Name = "btnA4";
            this.btnA4.Size = new System.Drawing.Size(133, 74);
            this.btnA4.TabIndex = 29;
            this.btnA4.Text = "A4";
            this.btnA4.UseVisualStyleBackColor = true;
            this.btnA4.Click += new System.EventHandler(this.btnA4_Click);
            // 
            // btnReceipt
            // 
            this.btnReceipt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReceipt.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnReceipt.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnReceipt.ForeColor = System.Drawing.Color.White;
            this.btnReceipt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnReceipt.Location = new System.Drawing.Point(218, 30);
            this.btnReceipt.Margin = new System.Windows.Forms.Padding(8);
            this.btnReceipt.Name = "btnReceipt";
            this.btnReceipt.Size = new System.Drawing.Size(133, 74);
            this.btnReceipt.TabIndex = 30;
            this.btnReceipt.Tag = "1";
            this.btnReceipt.Text = "Receipt";
            this.btnReceipt.UseVisualStyleBackColor = true;
            this.btnReceipt.Click += new System.EventHandler(this.btnReceipt_Click);
            // 
            // frmSelectPrintSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(420, 134);
            this.Controls.Add(this.btnA4);
            this.Controls.Add(this.btnReceipt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectPrintSize";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Print Size";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnA4;
        internal System.Windows.Forms.Button btnReceipt;
    }
}