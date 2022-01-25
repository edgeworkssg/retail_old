using WinPowerPOS.OrderForms;

namespace WinPowerPOS.AppointmentForms
{
	partial class frmSelectServices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectServices));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.tmrFilter = new System.Windows.Forms.Timer(this.components);
            this.lblHint = new System.Windows.Forms.Label();
            this.dgvItemList = new System.Windows.Forms.DataGridView();
            this.cbSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pagingControl = new WinPowerPOS.DataGridViewPagingControl();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.TextChanged += new System.EventHandler(this.btSearch_TextChanged);
            // 
            // tmrFilter
            // 
            this.tmrFilter.Interval = 300;
            this.tmrFilter.Tick += new System.EventHandler(this.tmrFilter_Tick);
            // 
            // lblHint
            // 
            resources.ApplyResources(this.lblHint, "lblHint");
            this.lblHint.Name = "lblHint";
            // 
            // dgvItemList
            // 
            this.dgvItemList.AllowUserToAddRows = false;
            this.dgvItemList.AllowUserToDeleteRows = false;
            this.dgvItemList.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvItemList, "dgvItemList");
            this.dgvItemList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvItemList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvItemList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cbSelect,
            this.colItemNo,
            this.colItemName,
            this.colPrice,
            this.colCategoryName});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemList.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItemList.Name = "dgvItemList";
            this.dgvItemList.RowHeadersVisible = false;
            this.dgvItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItemList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemList_CellDoubleClick);
            this.dgvItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemList_CellClick);
            this.dgvItemList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemList_CellContentClick);
            // 
            // cbSelect
            // 
            resources.ApplyResources(this.cbSelect, "cbSelect");
            this.cbSelect.Name = "cbSelect";
            // 
            // colItemNo
            // 
            this.colItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
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
            // colPrice
            // 
            this.colPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPrice.DataPropertyName = "RetailPrice";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "0";
            this.colPrice.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.colPrice, "colPrice");
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            // 
            // colCategoryName
            // 
            this.colCategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCategoryName.DataPropertyName = "CategoryName";
            this.colCategoryName.FillWeight = 30F;
            resources.ApplyResources(this.colCategoryName, "colCategoryName");
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.ReadOnly = true;
            // 
            // pagingControl
            // 
            resources.ApplyResources(this.pagingControl, "pagingControl");
            this.pagingControl.AutoGenerateColumns = false;
            this.pagingControl.CurrentPage = 0;
            this.pagingControl.DataGrid = null;
            this.pagingControl.Name = "pagingControl";
            this.pagingControl.PageSize = 50;
            // 
            // frmSelectServices
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.pagingControl);
            this.Controls.Add(this.dgvItemList);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "frmSelectServices";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAddMember_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		internal System.Windows.Forms.Button btnOk;
		internal System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox tbSearch;
		private System.Windows.Forms.Timer tmrFilter;
		private System.Windows.Forms.Label lblHint;
		private System.Windows.Forms.DataGridView dgvItemList;
		private DataGridViewPagingControl pagingControl;
		private System.Windows.Forms.DataGridViewCheckBoxColumn cbSelect;
		private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
		private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
		private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryName;

	}
}