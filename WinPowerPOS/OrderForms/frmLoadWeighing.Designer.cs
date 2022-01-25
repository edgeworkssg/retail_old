namespace WinPowerPOS.OrderForms
{
    partial class frmLoadWeighing
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblPleaseWait = new System.Windows.Forms.Label();
            this.theTimer = new System.Windows.Forms.Timer(this.components);
            this.lblDot = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Weighing In Progress";
            // 
            // lblPleaseWait
            // 
            this.lblPleaseWait.AutoSize = true;
            this.lblPleaseWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPleaseWait.Location = new System.Drawing.Point(13, 58);
            this.lblPleaseWait.Name = "lblPleaseWait";
            this.lblPleaseWait.Size = new System.Drawing.Size(104, 24);
            this.lblPleaseWait.TabIndex = 1;
            this.lblPleaseWait.Text = "Please wait";
            // 
            // theTimer
            // 
            this.theTimer.Interval = 1000;
            this.theTimer.Tick += new System.EventHandler(this.theTimer_Tick);
            // 
            // lblDot
            // 
            this.lblDot.AutoSize = true;
            this.lblDot.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDot.Location = new System.Drawing.Point(117, 58);
            this.lblDot.Name = "lblDot";
            this.lblDot.Size = new System.Drawing.Size(15, 24);
            this.lblDot.TabIndex = 2;
            this.lblDot.Text = ".";
            // 
            // frmLoadWeighing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 94);
            this.ControlBox = false;
            this.Controls.Add(this.lblDot);
            this.Controls.Add(this.lblPleaseWait);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoadWeighing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmLoadWeighing_Load);
            this.Shown += new System.EventHandler(this.frmLoadWeighing_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPleaseWait;
        private System.Windows.Forms.Timer theTimer;
        private System.Windows.Forms.Label lblDot;
    }
}