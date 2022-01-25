namespace WinPowerPOS.OrderForms
{
    partial class frmWaitForDrawer
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
            this.lblChange1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.bwCashDrawer = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblChange1
            // 
            this.lblChange1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblChange1.AutoSize = true;
            this.lblChange1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold);
            this.lblChange1.ForeColor = System.Drawing.Color.Yellow;
            this.lblChange1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblChange1.Location = new System.Drawing.Point(214, 8);
            this.lblChange1.Name = "lblChange1";
            this.lblChange1.Size = new System.Drawing.Size(101, 33);
            this.lblChange1.TabIndex = 5;
            this.lblChange1.Text = "0.00";
            this.lblChange1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.Yellow;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(56, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(282, 31);
            this.label8.TabIndex = 4;
            this.label8.Text = "BEFORE PROCEED";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.Yellow;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(19, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(365, 31);
            this.label9.TabIndex = 3;
            this.label9.Text = "PLEASE CLOSE DRAWER";
            // 
            // bwCashDrawer
            // 
            this.bwCashDrawer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCashDrawer_DoWork);
            this.bwCashDrawer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCashDrawer_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(71, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 31);
            this.label1.TabIndex = 6;
            this.label1.Text = "CHANGE";
            // 
            // frmWaitForDrawer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(402, 168);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblChange1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmWaitForDrawer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmErrorMessage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblChange1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.ComponentModel.BackgroundWorker bwCashDrawer;
        private System.Windows.Forms.Label label1;

    }
}