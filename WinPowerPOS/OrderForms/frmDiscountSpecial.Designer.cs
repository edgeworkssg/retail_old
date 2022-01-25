namespace WinPowerPOS.OrderForms
{
    partial class frmDiscountSpecial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDiscountSpecial));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDiscDollar = new System.Windows.Forms.Button();
            this.btnDiscPercent = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlAddDiscount = new System.Windows.Forms.Panel();
            this.btnFinalPrice = new System.Windows.Forms.Button();
            this.btnAddDiscDollar = new System.Windows.Forms.Button();
            this.btnAddDiscPercent = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.pnlAddDiscount.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // btnDiscount
            // 
            resources.ApplyResources(this.btnDiscount, "btnDiscount");
            this.btnDiscount.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnDiscount.ForeColor = System.Drawing.Color.White;
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Tag = "0";
            this.btnDiscount.UseVisualStyleBackColor = true;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnDiscDollar);
            this.panel1.Controls.Add(this.btnDiscPercent);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.btnDiscount);
            this.panel1.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnDiscDollar
            // 
            resources.ApplyResources(this.btnDiscDollar, "btnDiscDollar");
            this.btnDiscDollar.ForeColor = System.Drawing.Color.Black;
            this.btnDiscDollar.Name = "btnDiscDollar";
            this.btnDiscDollar.Tag = "$";
            this.btnDiscDollar.UseVisualStyleBackColor = true;
            this.btnDiscDollar.Click += new System.EventHandler(this.btnDisc_Click);
            // 
            // btnDiscPercent
            // 
            resources.ApplyResources(this.btnDiscPercent, "btnDiscPercent");
            this.btnDiscPercent.ForeColor = System.Drawing.Color.Black;
            this.btnDiscPercent.Name = "btnDiscPercent";
            this.btnDiscPercent.Tag = "%";
            this.btnDiscPercent.UseVisualStyleBackColor = true;
            this.btnDiscPercent.Click += new System.EventHandler(this.btnDisc_Click);
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lblTitle.Name = "lblTitle";
            // 
            // pnlAddDiscount
            // 
            this.pnlAddDiscount.BackColor = System.Drawing.Color.Transparent;
            this.pnlAddDiscount.Controls.Add(this.flowLayoutPanel2);
            this.pnlAddDiscount.Controls.Add(this.label7);
            resources.ApplyResources(this.pnlAddDiscount, "pnlAddDiscount");
            this.pnlAddDiscount.Name = "pnlAddDiscount";
            // 
            // btnFinalPrice
            // 
            resources.ApplyResources(this.btnFinalPrice, "btnFinalPrice");
            this.btnFinalPrice.ForeColor = System.Drawing.Color.Black;
            this.btnFinalPrice.Name = "btnFinalPrice";
            this.btnFinalPrice.Tag = "$";
            this.btnFinalPrice.UseVisualStyleBackColor = true;
            this.btnFinalPrice.Click += new System.EventHandler(this.btnFinalPrice_Click);
            // 
            // btnAddDiscDollar
            // 
            resources.ApplyResources(this.btnAddDiscDollar, "btnAddDiscDollar");
            this.btnAddDiscDollar.ForeColor = System.Drawing.Color.Black;
            this.btnAddDiscDollar.Name = "btnAddDiscDollar";
            this.btnAddDiscDollar.Tag = "$";
            this.btnAddDiscDollar.UseVisualStyleBackColor = true;
            this.btnAddDiscDollar.Click += new System.EventHandler(this.btnAddDisc_Click);
            // 
            // btnAddDiscPercent
            // 
            resources.ApplyResources(this.btnAddDiscPercent, "btnAddDiscPercent");
            this.btnAddDiscPercent.ForeColor = System.Drawing.Color.Black;
            this.btnAddDiscPercent.Name = "btnAddDiscPercent";
            this.btnAddDiscPercent.Tag = "%";
            this.btnAddDiscPercent.UseVisualStyleBackColor = true;
            this.btnAddDiscPercent.Click += new System.EventHandler(this.btnAddDisc_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label7.Name = "label7";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnAddDiscPercent);
            this.flowLayoutPanel2.Controls.Add(this.btnAddDiscDollar);
            this.flowLayoutPanel2.Controls.Add(this.btnFinalPrice);
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // frmDiscountSpecial
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.longdarkbg;
            this.ControlBox = false;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.pnlAddDiscount);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmDiscountSpecial";
            this.Load += new System.EventHandler(this.frmDiscounts_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlAddDiscount.ResumeLayout(false);
            this.pnlAddDiscount.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal System.Windows.Forms.Button btnDiscount;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlAddDiscount;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button btnAddDiscDollar;
        internal System.Windows.Forms.Button btnAddDiscPercent;
        private System.Windows.Forms.Label lblTitle;
        internal System.Windows.Forms.Button btnDiscDollar;
        internal System.Windows.Forms.Button btnDiscPercent;
        internal System.Windows.Forms.Button btnFinalPrice;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    }
}