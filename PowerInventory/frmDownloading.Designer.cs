namespace PowerInventory
{
    partial class frmDownloading
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
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlProgress
            // 
            this.pnlProgress.BackColor = System.Drawing.Color.White;
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.label9);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlProgress.Location = new System.Drawing.Point(1, 1);
            this.pnlProgress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(310, 159);
            this.pnlProgress.TabIndex = 71;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(82, 23);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 20);
            this.label9.TabIndex = 16;
            this.label9.Text = "Please Wait...";
            // 
            // pgb1
            // 
            this.pgb1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pgb1.Location = new System.Drawing.Point(38, 65);
            this.pgb1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Size = new System.Drawing.Size(238, 35);
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgb1.TabIndex = 0;
            // 
            // frmDownloading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 159);
            this.ControlBox = false;
            this.Controls.Add(this.pnlProgress);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmDownloading";
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ProgressBar pgb1;
    }
}