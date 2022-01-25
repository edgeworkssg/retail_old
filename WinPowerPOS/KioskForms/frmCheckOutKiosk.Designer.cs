namespace WinPowerPOS.KioskForms
{
    partial class frmCheckOutKiosk
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
            this.pnlGlobal = new System.Windows.Forms.Panel();
            this.pnlExit = new System.Windows.Forms.Panel();
            this.hostExit = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlExit = new WinPowerPOS.KioskForms.CircleButton();
            this.pnlAccept = new System.Windows.Forms.Panel();
            this.hostAccept = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlAccept = new WinPowerPOS.KioskForms.Accept();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.checkOut1 = new WinPowerPOS.KioskForms.CheckOut();
            this.pnlGlobal.SuspendLayout();
            this.pnlExit.SuspendLayout();
            this.pnlAccept.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlGlobal
            // 
            this.pnlGlobal.Controls.Add(this.pnlExit);
            this.pnlGlobal.Controls.Add(this.pnlAccept);
            this.pnlGlobal.Controls.Add(this.elementHost1);
            this.pnlGlobal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGlobal.Location = new System.Drawing.Point(0, 0);
            this.pnlGlobal.Name = "pnlGlobal";
            this.pnlGlobal.Size = new System.Drawing.Size(1024, 768);
            this.pnlGlobal.TabIndex = 1;
            // 
            // pnlExit
            // 
            this.pnlExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlExit.BackColor = System.Drawing.Color.White;
            this.pnlExit.Controls.Add(this.hostExit);
            this.pnlExit.Location = new System.Drawing.Point(926, 38);
            this.pnlExit.Name = "pnlExit";
            this.pnlExit.Size = new System.Drawing.Size(53, 55);
            this.pnlExit.TabIndex = 5;
            // 
            // hostExit
            // 
            this.hostExit.BackColorTransparent = true;
            this.hostExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hostExit.Location = new System.Drawing.Point(0, 0);
            this.hostExit.Name = "hostExit";
            this.hostExit.Size = new System.Drawing.Size(53, 55);
            this.hostExit.TabIndex = 3;
            this.hostExit.Text = "elementHost4";
            this.hostExit.Child = this.ctrlExit;
            // 
            // pnlAccept
            // 
            this.pnlAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAccept.BackColor = System.Drawing.Color.White;
            this.pnlAccept.Controls.Add(this.hostAccept);
            this.pnlAccept.Location = new System.Drawing.Point(780, 644);
            this.pnlAccept.Name = "pnlAccept";
            this.pnlAccept.Size = new System.Drawing.Size(200, 100);
            this.pnlAccept.TabIndex = 4;
            // 
            // hostAccept
            // 
            this.hostAccept.BackColorTransparent = true;
            this.hostAccept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hostAccept.Location = new System.Drawing.Point(0, 0);
            this.hostAccept.Name = "hostAccept";
            this.hostAccept.Size = new System.Drawing.Size(200, 100);
            this.hostAccept.TabIndex = 2;
            this.hostAccept.Text = "elementHost3";
            this.hostAccept.Child = this.ctrlAccept;
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1024, 768);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.checkOut1;
            // 
            // frmCheckOutKiosk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.pnlGlobal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCheckOutKiosk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCheckOutKiosk_Load);
            this.pnlGlobal.ResumeLayout(false);
            this.pnlExit.ResumeLayout(false);
            this.pnlAccept.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private CheckOut checkOut1;
        private System.Windows.Forms.Panel pnlGlobal;
        private System.Windows.Forms.Integration.ElementHost hostAccept;
        private Accept ctrlAccept;
        private System.Windows.Forms.Integration.ElementHost hostExit;
        private CircleButton ctrlExit;
        private System.Windows.Forms.Panel pnlExit;
        private System.Windows.Forms.Panel pnlAccept;
    }
}