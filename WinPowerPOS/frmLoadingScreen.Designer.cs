namespace WinPowerPOS
{
    partial class frmLoadingScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtStatus = new System.Windows.Forms.Label();
            this.Processor = new System.ComponentModel.BackgroundWorker();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelEquipPOSTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(37, 102);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(256, 7);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            // 
            // txtStatus
            // 
            this.txtStatus.AutoSize = true;
            this.txtStatus.Location = new System.Drawing.Point(34, 43);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(47, 13);
            this.txtStatus.TabIndex = 2;
            this.txtStatus.Text = "lblStatus";
            this.txtStatus.Visible = false;
            // 
            // Processor
            // 
            this.Processor.WorkerReportsProgress = true;
            this.Processor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Processor_DoWork);
            this.Processor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Processor_RunWorkerCompleted);
            this.Processor.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Processor_ProgressChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::WinPowerPOS.Properties.Resources.loading200red;
            this.pictureBox2.Location = new System.Drawing.Point(11, 220);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(312, 33);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(3, 246);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 22);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(2, 101);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(285, 10);
            this.panel2.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::WinPowerPOS.Properties.Resources.LogoEdgeworks;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(7, 36);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(333, 72);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelEquipPOSTitle);
            this.panel3.Location = new System.Drawing.Point(7, 145);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(333, 38);
            this.panel3.TabIndex = 9;
            // 
            // labelEquipPOSTitle
            // 
            this.labelEquipPOSTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEquipPOSTitle.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEquipPOSTitle.Location = new System.Drawing.Point(0, 0);
            this.labelEquipPOSTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEquipPOSTitle.Name = "labelEquipPOSTitle";
            this.labelEquipPOSTitle.Size = new System.Drawing.Size(333, 38);
            this.labelEquipPOSTitle.TabIndex = 0;
            this.labelEquipPOSTitle.Text = "EQuipPOS Retail";
            this.labelEquipPOSTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmLoadingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(344, 274);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoadingScreen";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmLoadingScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label txtStatus;
        private System.ComponentModel.BackgroundWorker Processor;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelEquipPOSTitle;

    }
}
