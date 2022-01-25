namespace WinPowerPOS
{
    partial class MDIPowerPOS
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIPowerPOS));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tslWarranty = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslDelivery = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslWarranty,
            this.toolStripSeparator1,
            this.tslDelivery,
            this.toolStripSeparator4});
            this.toolStrip.Name = "toolStrip";
            // 
            // tslWarranty
            // 
            this.tslWarranty.Name = "tslWarranty";
            resources.ApplyResources(this.tslWarranty, "tslWarranty");
            this.tslWarranty.Click += new System.EventHandler(this.tslWarranty_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tslDelivery
            // 
            this.tslDelivery.Name = "tslDelivery";
            resources.ApplyResources(this.tslDelivery, "tslDelivery");
            this.tslDelivery.Click += new System.EventHandler(this.tslDelivery_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            resources.ApplyResources(this.toolStripStatusLabel, "toolStripStatusLabel");
            // 
            // MDIPowerPOS
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "MDIPowerPOS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MDIPowerPOS_Load);
            this.MdiChildActivate += new System.EventHandler(this.MDIPowerPOS_MdiChildActivate);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.ToolStripLabel tslWarranty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel tslDelivery;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}



