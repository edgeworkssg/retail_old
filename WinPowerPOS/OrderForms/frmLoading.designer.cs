namespace WinPowerPOS.OrderForms
{
    partial class frmLoading
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
        private void InitializeComponent(string instruction)
        {
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlProgress
            // 
            this.pnlProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Location = new System.Drawing.Point(203, 90);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(200, 100);
            this.pnlProgress.TabIndex = 70;
            this.pnlProgress.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(61, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Please Wait...";
            // 
            // pgb1
            // 
            this.pgb1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pgb1.Location = new System.Drawing.Point(25, 42);
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Size = new System.Drawing.Size(159, 23);
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgb1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(590, 48);
            this.textBox1.TabIndex = 71;
            this.textBox1.Text = instruction;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 227);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pnlProgress);
            this.Name = "frmLoading";
            this.Text = "Info";
            this.Load += new System.EventHandler(this.frmLoading_Load);
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
        private System.Windows.Forms.TextBox textBox1;
    }
}