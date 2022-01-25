namespace WinPowerPOS
{
    partial class frmMainAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainAdmin));
            this.btnPromoByGroupList = new System.Windows.Forms.Button();
            this.btnPromoByItemList = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCreateVoucher = new System.Windows.Forms.Button();
            this.btnVouchers = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnImportPoints = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPromoByGroupList
            // 
            this.btnPromoByGroupList.BackColor = System.Drawing.Color.White;
            this.btnPromoByGroupList.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPromoByGroupList.BackgroundImage")));
            this.btnPromoByGroupList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromoByGroupList.ForeColor = System.Drawing.Color.White;
            this.btnPromoByGroupList.Location = new System.Drawing.Point(7, 74);
            this.btnPromoByGroupList.Margin = new System.Windows.Forms.Padding(4);
            this.btnPromoByGroupList.Name = "btnPromoByGroupList";
            this.btnPromoByGroupList.Size = new System.Drawing.Size(180, 46);
            this.btnPromoByGroupList.TabIndex = 27;
            this.btnPromoByGroupList.Text = "PROMO BY GROUP";
            this.btnPromoByGroupList.UseVisualStyleBackColor = false;
            this.btnPromoByGroupList.Click += new System.EventHandler(this.btnPromoByGroupList_Click);
            // 
            // btnPromoByItemList
            // 
            this.btnPromoByItemList.BackColor = System.Drawing.Color.White;
            this.btnPromoByItemList.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPromoByItemList.BackgroundImage")));
            this.btnPromoByItemList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromoByItemList.ForeColor = System.Drawing.Color.White;
            this.btnPromoByItemList.Location = new System.Drawing.Point(7, 20);
            this.btnPromoByItemList.Margin = new System.Windows.Forms.Padding(4);
            this.btnPromoByItemList.Name = "btnPromoByItemList";
            this.btnPromoByItemList.Size = new System.Drawing.Size(180, 46);
            this.btnPromoByItemList.TabIndex = 25;
            this.btnPromoByItemList.Text = "PROMO BY ITEM";
            this.btnPromoByItemList.UseVisualStyleBackColor = false;
            this.btnPromoByItemList.Click += new System.EventHandler(this.btnPromoByItemList_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnPromoByItemList);
            this.groupBox1.Controls.Add(this.btnPromoByGroupList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(196, 135);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PROMOTIONS";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.btnCreateVoucher);
            this.groupBox2.Controls.Add(this.btnVouchers);
            this.groupBox2.Location = new System.Drawing.Point(214, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(196, 135);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "VOUCHERS";
            // 
            // btnCreateVoucher
            // 
            this.btnCreateVoucher.BackColor = System.Drawing.Color.White;
            this.btnCreateVoucher.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreateVoucher.BackgroundImage")));
            this.btnCreateVoucher.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateVoucher.ForeColor = System.Drawing.Color.White;
            this.btnCreateVoucher.Location = new System.Drawing.Point(8, 75);
            this.btnCreateVoucher.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreateVoucher.Name = "btnCreateVoucher";
            this.btnCreateVoucher.Size = new System.Drawing.Size(180, 46);
            this.btnCreateVoucher.TabIndex = 26;
            this.btnCreateVoucher.Text = "CREATE VOUCHERS";
            this.btnCreateVoucher.UseVisualStyleBackColor = false;
            this.btnCreateVoucher.Click += new System.EventHandler(this.btnCreateVoucher_Click);
            // 
            // btnVouchers
            // 
            this.btnVouchers.BackColor = System.Drawing.Color.White;
            this.btnVouchers.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVouchers.BackgroundImage")));
            this.btnVouchers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVouchers.ForeColor = System.Drawing.Color.White;
            this.btnVouchers.Location = new System.Drawing.Point(7, 20);
            this.btnVouchers.Margin = new System.Windows.Forms.Padding(4);
            this.btnVouchers.Name = "btnVouchers";
            this.btnVouchers.Size = new System.Drawing.Size(180, 46);
            this.btnVouchers.TabIndex = 25;
            this.btnVouchers.Text = "VOUCHERS";
            this.btnVouchers.UseVisualStyleBackColor = false;
            this.btnVouchers.Click += new System.EventHandler(this.btnVoucherList_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.btnImportPoints);
            this.groupBox3.Location = new System.Drawing.Point(416, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(196, 135);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "POINTS";
            // 
            // btnImportPoints
            // 
            this.btnImportPoints.BackColor = System.Drawing.Color.White;
            this.btnImportPoints.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnImportPoints.BackgroundImage")));
            this.btnImportPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportPoints.ForeColor = System.Drawing.Color.White;
            this.btnImportPoints.Location = new System.Drawing.Point(7, 20);
            this.btnImportPoints.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportPoints.Name = "btnImportPoints";
            this.btnImportPoints.Size = new System.Drawing.Size(180, 46);
            this.btnImportPoints.TabIndex = 25;
            this.btnImportPoints.Text = "IMPORT POINTS";
            this.btnImportPoints.UseVisualStyleBackColor = false;
            this.btnImportPoints.Click += new System.EventHandler(this.btnImportPoints_Click);
            // 
            // frmMainAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(619, 156);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMainAdmin";
            this.Text = "Administrator Page";
            this.Load += new System.EventHandler(this.frmMainAdmin_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPromoByGroupList;
        private System.Windows.Forms.Button btnPromoByItemList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnVouchers;
        private System.Windows.Forms.Button btnCreateVoucher;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnImportPoints;
    }
}