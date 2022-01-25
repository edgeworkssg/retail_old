namespace WinPowerPOS.KioskForms
{
    partial class frmStaffFunction
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hostReprint = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlReprint = new WinPowerPOS.KioskForms.CircleButton();
            this.hostClose = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlClose = new WinPowerPOS.KioskForms.CircleButton();
            this.hostLogout = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlLogout = new WinPowerPOS.KioskForms.CircleButton();
            this.hostBagging = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlResetBagging = new WinPowerPOS.KioskForms.CircleButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.hostBagging, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.hostReprint, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.hostClose, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.hostLogout, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(526, 158);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // hostReprint
            // 
            this.hostReprint.Location = new System.Drawing.Point(8, 12);
            this.hostReprint.Margin = new System.Windows.Forms.Padding(8, 12, 5, 5);
            this.hostReprint.Name = "hostReprint";
            this.hostReprint.Size = new System.Drawing.Size(135, 135);
            this.hostReprint.TabIndex = 1;
            this.hostReprint.TabStop = false;
            this.hostReprint.Text = "elementHost2";
            this.hostReprint.Child = this.ctrlReprint;
            // 
            // hostClose
            // 
            this.hostClose.Location = new System.Drawing.Point(453, 12);
            this.hostClose.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.hostClose.Name = "hostClose";
            this.hostClose.Size = new System.Drawing.Size(70, 70);
            this.hostClose.TabIndex = 2;
            this.hostClose.TabStop = false;
            this.hostClose.Text = "elementHost1";
            this.hostClose.Child = this.ctrlClose;
            // 
            // hostLogout
            // 
            this.hostLogout.Location = new System.Drawing.Point(308, 12);
            this.hostLogout.Margin = new System.Windows.Forms.Padding(8, 12, 5, 5);
            this.hostLogout.Name = "hostLogout";
            this.hostLogout.Size = new System.Drawing.Size(135, 135);
            this.hostLogout.TabIndex = 0;
            this.hostLogout.TabStop = false;
            this.hostLogout.Text = "elementHost1";
            this.hostLogout.Child = this.ctrlLogout;
            // 
            // hostBagging
            // 
            this.hostBagging.Location = new System.Drawing.Point(158, 12);
            this.hostBagging.Margin = new System.Windows.Forms.Padding(8, 12, 5, 5);
            this.hostBagging.Name = "hostBagging";
            this.hostBagging.Size = new System.Drawing.Size(135, 135);
            this.hostBagging.TabIndex = 3;
            this.hostBagging.TabStop = false;
            this.hostBagging.Text = "elementHost2";
            this.hostBagging.Child = this.ctrlResetBagging;
            // 
            // frmStaffFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(526, 158);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmStaffFunction";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Integration.ElementHost hostLogout;
        private System.Windows.Forms.Integration.ElementHost hostReprint;
        private CircleButton ctrlReprint;
        private System.Windows.Forms.Integration.ElementHost hostClose;
        private CircleButton ctrlClose;
        private CircleButton ctrlLogout;
        private System.Windows.Forms.Integration.ElementHost hostBagging;
        private CircleButton ctrlResetBagging;
    }
}