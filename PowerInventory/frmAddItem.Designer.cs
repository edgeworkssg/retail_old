namespace PowerInventory
{
    partial class frmAddItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddItem));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlAddItems = new System.Windows.Forms.Panel();
            this.cbShowDetails = new System.Windows.Forms.CheckBox();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.llInvert = new System.Windows.Forms.LinkLabel();
            this.llSelectNone = new System.Windows.Forms.LinkLabel();
            this.llSelectAll = new System.Windows.Forms.LinkLabel();
            this.btnAddItems = new System.Windows.Forms.Button();
            this.btnCancelAdd = new System.Windows.Forms.Button();
            this.dgvItemList = new System.Windows.Forms.DataGridView();
            this.cbSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttributes8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Userflag1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlAddItems.SuspendLayout();
            this.pnlLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlAddItems
            // 
            resources.ApplyResources(this.pnlAddItems, "pnlAddItems");
            this.pnlAddItems.BackColor = System.Drawing.Color.White;
            this.pnlAddItems.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.pnlAddItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAddItems.Controls.Add(this.cbShowDetails);
            this.pnlAddItems.Controls.Add(this.pnlLoading);
            this.pnlAddItems.Controls.Add(this.llInvert);
            this.pnlAddItems.Controls.Add(this.llSelectNone);
            this.pnlAddItems.Controls.Add(this.llSelectAll);
            this.pnlAddItems.Controls.Add(this.btnAddItems);
            this.pnlAddItems.Controls.Add(this.btnCancelAdd);
            this.pnlAddItems.Controls.Add(this.dgvItemList);
            this.pnlAddItems.Name = "pnlAddItems";
            // 
            // cbShowDetails
            // 
            resources.ApplyResources(this.cbShowDetails, "cbShowDetails");
            this.cbShowDetails.BackColor = System.Drawing.Color.Gainsboro;
            this.cbShowDetails.Name = "cbShowDetails";
            this.cbShowDetails.UseVisualStyleBackColor = false;
            this.cbShowDetails.CheckedChanged += new System.EventHandler(this.cbShowDetails_CheckedChanged);
            // 
            // pnlLoading
            // 
            resources.ApplyResources(this.pnlLoading, "pnlLoading");
            this.pnlLoading.BackColor = System.Drawing.Color.White;
            this.pnlLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoading.Controls.Add(this.label3);
            this.pnlLoading.Name = "pnlLoading";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // llInvert
            // 
            resources.ApplyResources(this.llInvert, "llInvert");
            this.llInvert.BackColor = System.Drawing.Color.Transparent;
            this.llInvert.Name = "llInvert";
            this.llInvert.TabStop = true;
            this.llInvert.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llInvert_LinkClicked);
            // 
            // llSelectNone
            // 
            resources.ApplyResources(this.llSelectNone, "llSelectNone");
            this.llSelectNone.BackColor = System.Drawing.Color.Transparent;
            this.llSelectNone.Name = "llSelectNone";
            this.llSelectNone.TabStop = true;
            this.llSelectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectNone_LinkClicked);
            // 
            // llSelectAll
            // 
            resources.ApplyResources(this.llSelectAll, "llSelectAll");
            this.llSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.llSelectAll.Name = "llSelectAll";
            this.llSelectAll.TabStop = true;
            this.llSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectAll_LinkClicked);
            // 
            // btnAddItems
            // 
            this.btnAddItems.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItems.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            resources.ApplyResources(this.btnAddItems, "btnAddItems");
            this.btnAddItems.ForeColor = System.Drawing.Color.White;
            this.btnAddItems.Name = "btnAddItems";
            this.btnAddItems.UseVisualStyleBackColor = false;
            this.btnAddItems.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnCancelAdd
            // 
            this.btnCancelAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelAdd.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            resources.ApplyResources(this.btnCancelAdd, "btnCancelAdd");
            this.btnCancelAdd.ForeColor = System.Drawing.Color.White;
            this.btnCancelAdd.Name = "btnCancelAdd";
            this.btnCancelAdd.UseVisualStyleBackColor = false;
            this.btnCancelAdd.Click += new System.EventHandler(this.btnCancelAdd_Click);
            // 
            // dgvItemList
            // 
            this.dgvItemList.AllowUserToAddRows = false;
            this.dgvItemList.AllowUserToDeleteRows = false;
            this.dgvItemList.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvItemList, "dgvItemList");
            this.dgvItemList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cbSelect,
            this.colItemNo,
            this.colItemName,
            this.ItemDesc,
            this.colCategoryName,
            this.colDepartment,
            this.colAttributes1,
            this.colAttributes2,
            this.colAttributes3,
            this.colAttributes4,
            this.colAttributes5,
            this.colAttributes6,
            this.colAttributes7,
            this.colAttributes8,
            this.colPrice,
            this.Userflag1});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemList.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItemList.Name = "dgvItemList";
            this.dgvItemList.RowHeadersVisible = false;
            this.dgvItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemList_CellClick);
            // 
            // cbSelect
            // 
            resources.ApplyResources(this.cbSelect, "cbSelect");
            this.cbSelect.Name = "cbSelect";
            // 
            // colItemNo
            // 
            this.colItemNo.DataPropertyName = "ItemNo";
            resources.ApplyResources(this.colItemNo, "colItemNo");
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.ReadOnly = true;
            // 
            // colItemName
            // 
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemName.DataPropertyName = "ItemName";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemName.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.colItemName, "colItemName");
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // ItemDesc
            // 
            this.ItemDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDesc.DataPropertyName = "ItemDesc";
            resources.ApplyResources(this.ItemDesc, "ItemDesc");
            this.ItemDesc.Name = "ItemDesc";
            this.ItemDesc.ReadOnly = true;
            // 
            // colCategoryName
            // 
            this.colCategoryName.DataPropertyName = "CategoryName";
            resources.ApplyResources(this.colCategoryName, "colCategoryName");
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.ReadOnly = true;
            // 
            // colDepartment
            // 
            this.colDepartment.DataPropertyName = "ItemDepartmentID";
            resources.ApplyResources(this.colDepartment, "colDepartment");
            this.colDepartment.Name = "colDepartment";
            // 
            // colAttributes1
            // 
            this.colAttributes1.DataPropertyName = "Attributes1";
            resources.ApplyResources(this.colAttributes1, "colAttributes1");
            this.colAttributes1.Name = "colAttributes1";
            // 
            // colAttributes2
            // 
            this.colAttributes2.DataPropertyName = "Attributes2";
            resources.ApplyResources(this.colAttributes2, "colAttributes2");
            this.colAttributes2.Name = "colAttributes2";
            // 
            // colAttributes3
            // 
            this.colAttributes3.DataPropertyName = "Attributes3";
            resources.ApplyResources(this.colAttributes3, "colAttributes3");
            this.colAttributes3.Name = "colAttributes3";
            // 
            // colAttributes4
            // 
            this.colAttributes4.DataPropertyName = "Attributes4";
            resources.ApplyResources(this.colAttributes4, "colAttributes4");
            this.colAttributes4.Name = "colAttributes4";
            // 
            // colAttributes5
            // 
            this.colAttributes5.DataPropertyName = "Attributes5";
            resources.ApplyResources(this.colAttributes5, "colAttributes5");
            this.colAttributes5.Name = "colAttributes5";
            // 
            // colAttributes6
            // 
            this.colAttributes6.DataPropertyName = "Attributes6";
            resources.ApplyResources(this.colAttributes6, "colAttributes6");
            this.colAttributes6.Name = "colAttributes6";
            // 
            // colAttributes7
            // 
            this.colAttributes7.DataPropertyName = "Attributes7";
            resources.ApplyResources(this.colAttributes7, "colAttributes7");
            this.colAttributes7.Name = "colAttributes7";
            // 
            // colAttributes8
            // 
            this.colAttributes8.DataPropertyName = "Attributes8";
            resources.ApplyResources(this.colAttributes8, "colAttributes8");
            this.colAttributes8.Name = "colAttributes8";
            // 
            // colPrice
            // 
            this.colPrice.DataPropertyName = "RetailPrice";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "0";
            this.colPrice.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.colPrice, "colPrice");
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            // 
            // Userflag1
            // 
            this.Userflag1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Userflag1.DataPropertyName = "Userflag1";
            resources.ApplyResources(this.Userflag1, "Userflag1");
            this.Userflag1.Name = "Userflag1";
            this.Userflag1.ReadOnly = true;
            // 
            // frmAddItem
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlAddItems);
            this.Name = "frmAddItem";
            this.Load += new System.EventHandler(this.frmAddItem_Load);
            this.pnlAddItems.ResumeLayout(false);
            this.pnlAddItems.PerformLayout();
            this.pnlLoading.ResumeLayout(false);
            this.pnlLoading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlAddItems;
        internal System.Windows.Forms.Button btnCancelAdd;
        private System.Windows.Forms.DataGridView dgvItemList;
        internal System.Windows.Forms.Button btnAddItems;
        private System.Windows.Forms.LinkLabel llInvert;
        private System.Windows.Forms.LinkLabel llSelectNone;
        private System.Windows.Forms.LinkLabel llSelectAll;
        protected System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbShowDetails;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cbSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes6;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes7;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttributes8;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Userflag1;
    }
}