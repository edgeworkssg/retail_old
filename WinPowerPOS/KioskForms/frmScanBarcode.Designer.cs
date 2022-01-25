namespace WinPowerPOS.KioskForms
{
    partial class frmScanBarcode
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
            this.hostScanBarcode = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlScanBarcode = new WinPowerPOS.KioskForms.ScanBarcode();
            this.bwBarcode = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // hostScanBarcode
            // 
            this.hostScanBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hostScanBarcode.Location = new System.Drawing.Point(0, 0);
            this.hostScanBarcode.Name = "hostScanBarcode";
            this.hostScanBarcode.Size = new System.Drawing.Size(368, 166);
            this.hostScanBarcode.TabIndex = 0;
            this.hostScanBarcode.TabStop = false;
            this.hostScanBarcode.Text = "elementHost1";
            this.hostScanBarcode.Child = this.ctrlScanBarcode;
            // 
            // bwBarcode
            // 
            this.bwBarcode.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwBarcode_DoWork);
            // 
            // frmScanBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(368, 166);
            this.Controls.Add(this.hostScanBarcode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmScanBarcode";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Shown += new System.EventHandler(this.frmScanBarcode_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmScanBarcode_KeyPress);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScanBarcode_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost hostScanBarcode;
        private ScanBarcode ctrlScanBarcode;
        private System.ComponentModel.BackgroundWorker bwBarcode;
    }
}