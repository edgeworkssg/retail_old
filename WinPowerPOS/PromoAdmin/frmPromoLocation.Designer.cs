namespace WinPowerPOS.PromoAdmin
{
    partial class frmPromoLocation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPromoLocation));
            this.label1 = new System.Windows.Forms.Label();
            this.dgvPOS = new System.Windows.Forms.DataGridView();
            this.colCB = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PointOfSaleID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PointOfSaleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Outlet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPromo = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSetAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPOS)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // dgvPOS
            // 
            this.dgvPOS.AllowUserToAddRows = false;
            this.dgvPOS.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvPOS, "dgvPOS");
            this.dgvPOS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPOS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCB,
            this.PointOfSaleID,
            this.PointOfSaleName,
            this.Outlet});
            this.dgvPOS.Name = "dgvPOS";
            this.dgvPOS.ReadOnly = true;
            this.dgvPOS.RowHeadersVisible = false;
            this.dgvPOS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPOS_CellClick);
            // 
            // colCB
            // 
            this.colCB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.colCB, "colCB");
            this.colCB.Name = "colCB";
            this.colCB.ReadOnly = true;
            // 
            // PointOfSaleID
            // 
            this.PointOfSaleID.DataPropertyName = "PointOfSaleId";
            resources.ApplyResources(this.PointOfSaleID, "PointOfSaleID");
            this.PointOfSaleID.Name = "PointOfSaleID";
            this.PointOfSaleID.ReadOnly = true;
            // 
            // PointOfSaleName
            // 
            this.PointOfSaleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PointOfSaleName.DataPropertyName = "PointOfSaleName";
            resources.ApplyResources(this.PointOfSaleName, "PointOfSaleName");
            this.PointOfSaleName.Name = "PointOfSaleName";
            this.PointOfSaleName.ReadOnly = true;
            // 
            // Outlet
            // 
            this.Outlet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Outlet.DataPropertyName = "OutletName";
            resources.ApplyResources(this.Outlet, "Outlet");
            this.Outlet.Name = "Outlet";
            this.Outlet.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // lblPromo
            // 
            resources.ApplyResources(this.lblPromo, "lblPromo");
            this.lblPromo.BackColor = System.Drawing.Color.Transparent;
            this.lblPromo.Name = "lblPromo";
            // 
            // btnGenerate
            // 
            resources.ApplyResources(this.btnGenerate, "btnGenerate");
            this.btnGenerate.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSetAll
            // 
            this.btnSetAll.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            resources.ApplyResources(this.btnSetAll, "btnSetAll");
            this.btnSetAll.ForeColor = System.Drawing.Color.White;
            this.btnSetAll.Name = "btnSetAll";
            this.btnSetAll.UseVisualStyleBackColor = true;
            // 
            // frmPromoLocation
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.Controls.Add(this.btnSetAll);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lblPromo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvPOS);
            this.Controls.Add(this.label1);
            this.Name = "frmPromoLocation";
            this.Load += new System.EventHandler(this.frmPromoLocation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPOS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPOS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPromo;
        internal System.Windows.Forms.Button btnGenerate;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSetAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCB;
        private System.Windows.Forms.DataGridViewTextBoxColumn PointOfSaleID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PointOfSaleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Outlet;
    }
}