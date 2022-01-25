namespace PowerInventory
{
    partial class frmSavedFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSavedFiles));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.SN = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.SaveDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SavedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MovementType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SavedRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryDetRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.lblLoadingMessage = new System.Windows.Forms.Label();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            this.dgvStock.AllowUserToResizeColumns = false;
            this.dgvStock.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvStock, "dgvStock");
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvStock.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SN,
            this.Delete,
            this.SaveDate,
            this.SavedBy,
            this.MovementType,
            this.SavedRemark,
            this.InventoryDetRefNo,
            this.FileName});
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.RowTemplate.Height = 24;
            this.dgvStock.TabStop = false;
            this.dgvStock.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStock_CellContentClick);
            // 
            // SN
            // 
            this.SN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.SN, "SN");
            this.SN.Name = "SN";
            this.SN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SN.Text = "Load";
            this.SN.UseColumnTextForButtonValue = true;
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            this.Delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Delete.Text = "Delete";
            this.Delete.UseColumnTextForButtonValue = true;
            resources.ApplyResources(this.Delete, "Delete");
            // 
            // SaveDate
            // 
            this.SaveDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SaveDate.DataPropertyName = "SavedDate";
            dataGridViewCellStyle1.Format = "f";
            dataGridViewCellStyle1.NullValue = null;
            this.SaveDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.SaveDate.FillWeight = 20.22345F;
            resources.ApplyResources(this.SaveDate, "SaveDate");
            this.SaveDate.Name = "SaveDate";
            this.SaveDate.ReadOnly = true;
            // 
            // SavedBy
            // 
            this.SavedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SavedBy.DataPropertyName = "SavedBy";
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SavedBy.DefaultCellStyle = dataGridViewCellStyle2;
            this.SavedBy.FillWeight = 1F;
            resources.ApplyResources(this.SavedBy, "SavedBy");
            this.SavedBy.Name = "SavedBy";
            this.SavedBy.ReadOnly = true;
            // 
            // MovementType
            // 
            this.MovementType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MovementType.DataPropertyName = "MovementType";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MovementType.DefaultCellStyle = dataGridViewCellStyle3;
            this.MovementType.FillWeight = 54.60331F;
            resources.ApplyResources(this.MovementType, "MovementType");
            this.MovementType.Name = "MovementType";
            this.MovementType.ReadOnly = true;
            // 
            // SavedRemark
            // 
            this.SavedRemark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SavedRemark.DataPropertyName = "Remark";
            resources.ApplyResources(this.SavedRemark, "SavedRemark");
            this.SavedRemark.Name = "SavedRemark";
            this.SavedRemark.ReadOnly = true;
            // 
            // InventoryDetRefNo
            // 
            this.InventoryDetRefNo.DataPropertyName = "SaveID";
            resources.ApplyResources(this.InventoryDetRefNo, "InventoryDetRefNo");
            this.InventoryDetRefNo.Name = "InventoryDetRefNo";
            this.InventoryDetRefNo.ReadOnly = true;
            // 
            // FileName
            // 
            this.FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.FileName.DataPropertyName = "SaveName";
            this.FileName.FillWeight = 1F;
            resources.ApplyResources(this.FileName, "FileName");
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            // 
            // btnDeleteAll
            // 
            resources.ApplyResources(this.btnDeleteAll, "btnDeleteAll");
            this.btnDeleteAll.BackColor = System.Drawing.Color.White;
            this.btnDeleteAll.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnDeleteAll.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.UseVisualStyleBackColor = false;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // pnlProgress
            // 
            this.pnlProgress.BackColor = System.Drawing.Color.Yellow;
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.lblLoadingMessage);
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.Name = "pnlProgress";
            // 
            // lblLoadingMessage
            // 
            resources.ApplyResources(this.lblLoadingMessage, "lblLoadingMessage");
            this.lblLoadingMessage.Name = "lblLoadingMessage";
            // 
            // bgWorker
            // 
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // frmSavedFiles
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.btnDeleteAll);
            this.Controls.Add(this.dgvStock);
            this.Name = "frmSavedFiles";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSavedFiles_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.DataGridViewButtonColumn SN;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn SaveDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SavedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn MovementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn SavedRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryDetRefNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        protected System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lblLoadingMessage;
        private System.ComponentModel.BackgroundWorker bgWorker;
    }
}