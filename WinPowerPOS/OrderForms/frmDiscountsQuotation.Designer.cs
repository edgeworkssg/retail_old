namespace WinPowerPOS.OrderForms
{
    partial class frmDiscountsQuotation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDiscountsQuotation));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDiscAmt = new System.Windows.Forms.TextBox();
            this.btnApplySpecifiedDiscount = new System.Windows.Forms.Button();
            this.pnlAdditionalDiscount = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAddDiscDollar = new System.Windows.Forms.Button();
            this.btnAddDiscPercent = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlAdditionalDiscount.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.btnDiscount);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 131);
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(365, 500);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(361, 226);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnDiscount
            // 
            this.btnDiscount.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.btnDiscount.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnDiscount.ForeColor = System.Drawing.Color.Black;
            this.btnDiscount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDiscount.Location = new System.Drawing.Point(3, 3);
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(110, 45);
            this.btnDiscount.TabIndex = 61;
            this.btnDiscount.Tag = "0";
            this.btnDiscount.Text = "NO DISCOUNT";
            this.btnDiscount.UseVisualStyleBackColor = true;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.button6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button6.Location = new System.Drawing.Point(252, 12);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(110, 45);
            this.button6.TabIndex = 67;
            this.button6.Text = "CANCEL";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.txtDiscAmt);
            this.groupBox1.Controls.Add(this.btnApplySpecifiedDiscount);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Location = new System.Drawing.Point(9, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 62);
            this.groupBox1.TabIndex = 68;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Discount Amount";
            // 
            // txtDiscAmt
            // 
            this.txtDiscAmt.Location = new System.Drawing.Point(8, 23);
            this.txtDiscAmt.Name = "txtDiscAmt";
            this.txtDiscAmt.Size = new System.Drawing.Size(215, 22);
            this.txtDiscAmt.TabIndex = 0;
            // 
            // btnApplySpecifiedDiscount
            // 
            this.btnApplySpecifiedDiscount.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnApplySpecifiedDiscount.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnApplySpecifiedDiscount.ForeColor = System.Drawing.Color.Black;
            this.btnApplySpecifiedDiscount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnApplySpecifiedDiscount.Location = new System.Drawing.Point(237, 19);
            this.btnApplySpecifiedDiscount.Name = "btnApplySpecifiedDiscount";
            this.btnApplySpecifiedDiscount.Size = new System.Drawing.Size(110, 29);
            this.btnApplySpecifiedDiscount.TabIndex = 63;
            this.btnApplySpecifiedDiscount.Tag = "0";
            this.btnApplySpecifiedDiscount.Text = "APPLY";
            this.btnApplySpecifiedDiscount.UseVisualStyleBackColor = true;
            this.btnApplySpecifiedDiscount.Click += new System.EventHandler(this.btnApplySpecifiedDiscount_Click_1);
            // 
            // pnlAdditionalDiscount
            // 
            this.pnlAdditionalDiscount.BackColor = System.Drawing.Color.Transparent;
            this.pnlAdditionalDiscount.Controls.Add(this.label7);
            this.pnlAdditionalDiscount.Controls.Add(this.btnAddDiscDollar);
            this.pnlAdditionalDiscount.Controls.Add(this.btnAddDiscPercent);
            this.pnlAdditionalDiscount.Location = new System.Drawing.Point(4, 363);
            this.pnlAdditionalDiscount.Name = "pnlAdditionalDiscount";
            this.pnlAdditionalDiscount.Size = new System.Drawing.Size(361, 93);
            this.pnlAdditionalDiscount.TabIndex = 104;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label7.Location = new System.Drawing.Point(17, 11);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(153, 18);
            this.label7.TabIndex = 100;
            this.label7.Text = "Additional Discount";
            // 
            // btnAddDiscDollar
            // 
            this.btnAddDiscDollar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddDiscDollar.BackgroundImage")));
            this.btnAddDiscDollar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDiscDollar.ForeColor = System.Drawing.Color.Black;
            this.btnAddDiscDollar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddDiscDollar.Location = new System.Drawing.Point(93, 34);
            this.btnAddDiscDollar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddDiscDollar.Name = "btnAddDiscDollar";
            this.btnAddDiscDollar.Size = new System.Drawing.Size(65, 45);
            this.btnAddDiscDollar.TabIndex = 102;
            this.btnAddDiscDollar.Tag = "$";
            this.btnAddDiscDollar.Text = "$";
            this.btnAddDiscDollar.UseVisualStyleBackColor = true;
            this.btnAddDiscDollar.Click += new System.EventHandler(this.btnAddDisc_Click);
            // 
            // btnAddDiscPercent
            // 
            this.btnAddDiscPercent.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddDiscPercent.BackgroundImage")));
            this.btnAddDiscPercent.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDiscPercent.ForeColor = System.Drawing.Color.Black;
            this.btnAddDiscPercent.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddDiscPercent.Location = new System.Drawing.Point(20, 34);
            this.btnAddDiscPercent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddDiscPercent.Name = "btnAddDiscPercent";
            this.btnAddDiscPercent.Size = new System.Drawing.Size(65, 45);
            this.btnAddDiscPercent.TabIndex = 101;
            this.btnAddDiscPercent.Tag = "%";
            this.btnAddDiscPercent.Text = "%";
            this.btnAddDiscPercent.UseVisualStyleBackColor = true;
            this.btnAddDiscPercent.Click += new System.EventHandler(this.btnAddDisc_Click);
            // 
            // frmDiscountsQuotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.longdarkbg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(369, 459);
            this.ControlBox = false;
            this.Controls.Add(this.pnlAdditionalDiscount);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(385, 500);
            this.Name = "frmDiscountsQuotation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmDiscounts_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlAdditionalDiscount.ResumeLayout(false);
            this.pnlAdditionalDiscount.PerformLayout();
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
        private System.Windows.Forms.Panel pnlAdditionalDiscount;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button btnAddDiscDollar;
        internal System.Windows.Forms.Button btnAddDiscPercent;
    }
}