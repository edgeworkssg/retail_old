namespace WinPowerPOS.OrderForms
{
    partial class frmPrintReceipt
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnNoPrint = new System.Windows.Forms.Button();
            this.btnEmailReceipt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(88, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "Do you want to print receipt?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Green;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(78, 78);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(99, 31);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print Receipt";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnNoPrint
            // 
            this.btnNoPrint.BackColor = System.Drawing.Color.Brown;
            this.btnNoPrint.ForeColor = System.Drawing.Color.White;
            this.btnNoPrint.Location = new System.Drawing.Point(183, 78);
            this.btnNoPrint.Name = "btnNoPrint";
            this.btnNoPrint.Size = new System.Drawing.Size(99, 31);
            this.btnNoPrint.TabIndex = 3;
            this.btnNoPrint.Text = "No Receipt";
            this.btnNoPrint.UseVisualStyleBackColor = false;
            this.btnNoPrint.Click += new System.EventHandler(this.btnNoPrint_Click);
            // 
            // btnEmailReceipt
            // 
            this.btnEmailReceipt.BackColor = System.Drawing.Color.Blue;
            this.btnEmailReceipt.ForeColor = System.Drawing.Color.White;
            this.btnEmailReceipt.Location = new System.Drawing.Point(289, 78);
            this.btnEmailReceipt.Name = "btnEmailReceipt";
            this.btnEmailReceipt.Size = new System.Drawing.Size(99, 31);
            this.btnEmailReceipt.TabIndex = 4;
            this.btnEmailReceipt.Text = "Email Receipt";
            this.btnEmailReceipt.UseVisualStyleBackColor = false;
            this.btnEmailReceipt.Click += new System.EventHandler(this.btnEmailReceipt_Click);
            // 
            // frmPrintReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 138);
            this.Controls.Add(this.btnEmailReceipt);
            this.Controls.Add(this.btnNoPrint);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPrintReceipt";
            this.ShowInTaskbar = false;
            this.Text = "frmPrintReceipt";
            this.Load += new System.EventHandler(this.frmPrintReceipt_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNoPrint;
        private System.Windows.Forms.Button btnEmailReceipt;
    }
}