namespace WinPowerPOS.OrderForms
{
    partial class frmDiscountSpecialQuotation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDiscountSpecialQuotation));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDiscAmt = new System.Windows.Forms.TextBox();
            this.btnApplySpecifiedDiscount = new System.Windows.Forms.Button();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AccessibleDescription = null;
            this.flowLayoutPanel1.AccessibleName = null;
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.BackgroundImage = null;
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.btnDiscount);
            this.flowLayoutPanel1.Font = null;
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(365, 500);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.txtDiscAmt);
            this.groupBox1.Controls.Add(this.btnApplySpecifiedDiscount);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtDiscAmt
            // 
            this.txtDiscAmt.AccessibleDescription = null;
            this.txtDiscAmt.AccessibleName = null;
            resources.ApplyResources(this.txtDiscAmt, "txtDiscAmt");
            this.txtDiscAmt.BackgroundImage = null;
            this.txtDiscAmt.Font = null;
            this.txtDiscAmt.Name = "txtDiscAmt";
            // 
            // btnApplySpecifiedDiscount
            // 
            this.btnApplySpecifiedDiscount.AccessibleDescription = null;
            this.btnApplySpecifiedDiscount.AccessibleName = null;
            resources.ApplyResources(this.btnApplySpecifiedDiscount, "btnApplySpecifiedDiscount");
            this.btnApplySpecifiedDiscount.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnApplySpecifiedDiscount.ForeColor = System.Drawing.Color.Black;
            this.btnApplySpecifiedDiscount.Name = "btnApplySpecifiedDiscount";
            this.btnApplySpecifiedDiscount.Tag = "0";
            this.btnApplySpecifiedDiscount.UseVisualStyleBackColor = true;
            this.btnApplySpecifiedDiscount.Click += new System.EventHandler(this.btnApplySpecifiedDiscount_Click);
            // 
            // btnDiscount
            // 
            this.btnDiscount.AccessibleDescription = null;
            this.btnDiscount.AccessibleName = null;
            resources.ApplyResources(this.btnDiscount, "btnDiscount");
            this.btnDiscount.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.btnDiscount.ForeColor = System.Drawing.Color.Black;
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Tag = "0";
            this.btnDiscount.UseVisualStyleBackColor = true;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // button6
            // 
            this.button6.AccessibleDescription = null;
            this.button6.AccessibleName = null;
            resources.ApplyResources(this.button6, "button6");
            this.button6.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Name = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // frmDiscountSpecialQuotation
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.longdarkbg;
            this.ControlBox = false;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.button6);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.Name = "frmDiscountSpecialQuotation";
            this.Load += new System.EventHandler(this.frmDiscounts_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal System.Windows.Forms.Button btnDiscount;
        internal System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDiscAmt;
        internal System.Windows.Forms.Button btnApplySpecifiedDiscount;
    }
}