namespace WinPowerPOS.KioskForms
{
    partial class frmLoginKiosk
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
            this.hostLoginKiosk = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlLoginKiosk = new WinPowerPOS.KioskForms.LoginKiosk();
            this.SuspendLayout();
            // 
            // hostLoginKiosk
            // 
            this.hostLoginKiosk.BackColor = System.Drawing.Color.Black;
            this.hostLoginKiosk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hostLoginKiosk.Location = new System.Drawing.Point(0, 0);
            this.hostLoginKiosk.Name = "hostLoginKiosk";
            this.hostLoginKiosk.Size = new System.Drawing.Size(373, 373);
            this.hostLoginKiosk.TabIndex = 1;
            this.hostLoginKiosk.TabStop = false;
            this.hostLoginKiosk.Text = "elementHost1";
            this.hostLoginKiosk.Child = this.ctrlLoginKiosk;
            // 
            // frmLoginKiosk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 373);
            this.ControlBox = false;
            this.Controls.Add(this.hostLoginKiosk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLoginKiosk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost hostLoginKiosk;
        private LoginKiosk ctrlLoginKiosk;
    }
}