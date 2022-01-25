namespace WinPowerPOS.BarcodePrinter
{
    partial class frmPrint
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
            this.Tabel = new System.Windows.Forms.DataGridView();
            this.dgQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgPlus = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgMinus = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgCopyCustomQty = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAlternateBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAttributes1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAttributes2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAttributes3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAttributes4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAttributes5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAttributes6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAttributes7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAttributes8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgBarcodeText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadQtyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromLastGoodsReceiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromGoodsReceiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.editTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tstxtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.cbCategory = new System.Windows.Forms.ToolStripComboBox();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTemplateDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.printableFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSaveDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabel
            // 
            this.Tabel.AllowUserToAddRows = false;
            this.Tabel.AllowUserToDeleteRows = false;
            this.Tabel.AllowUserToResizeRows = false;
            this.Tabel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tabel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgQuantity,
            this.dgPlus,
            this.dgMinus,
            this.dgCopyCustomQty,
            this.dgItemName,
            this.dgItemNo,
            this.dgCategory,
            this.dgDepartment,
            this.dgBarcode,
            this.dgAlternateBarcode,
            this.dgPrice,
            this.dgAttributes1,
            this.dgAttributes2,
            this.dgAttributes3,
            this.dgAttributes4,
            this.dgAttributes5,
            this.dgAttributes6,
            this.dgAttributes7,
            this.dgAttributes8,
            this.dgBarcodeText});
            this.Tabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabel.Location = new System.Drawing.Point(5, 32);
            this.Tabel.Name = "Tabel";
            this.Tabel.RowHeadersVisible = false;
            this.Tabel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Tabel.Size = new System.Drawing.Size(829, 324);
            this.Tabel.TabIndex = 1;
            this.Tabel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellClick);
            this.Tabel.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Tabel_DataError);
            this.Tabel.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellContentClick);
            // 
            // dgQuantity
            // 
            this.dgQuantity.DataPropertyName = "Qty";
            this.dgQuantity.HeaderText = "Qty";
            this.dgQuantity.Name = "dgQuantity";
            this.dgQuantity.Width = 45;
            // 
            // dgPlus
            // 
            this.dgPlus.HeaderText = "dgPlus";
            this.dgPlus.Name = "dgPlus";
            this.dgPlus.ReadOnly = true;
            this.dgPlus.Text = "+";
            this.dgPlus.UseColumnTextForButtonValue = true;
            this.dgPlus.Width = 25;
            // 
            // dgMinus
            // 
            this.dgMinus.HeaderText = "dgMinus";
            this.dgMinus.Name = "dgMinus";
            this.dgMinus.ReadOnly = true;
            this.dgMinus.Text = "-";
            this.dgMinus.UseColumnTextForButtonValue = true;
            this.dgMinus.Width = 25;
            // 
            // dgCopyCustomQty
            // 
            this.dgCopyCustomQty.HeaderText = "dgCopyCustomQty";
            this.dgCopyCustomQty.Name = "dgCopyCustomQty";
            this.dgCopyCustomQty.Text = "Copy";
            this.dgCopyCustomQty.UseColumnTextForButtonValue = true;
            // 
            // dgItemName
            // 
            this.dgItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgItemName.DataPropertyName = "ItemName";
            this.dgItemName.HeaderText = "Item Name";
            this.dgItemName.MinimumWidth = 150;
            this.dgItemName.Name = "dgItemName";
            this.dgItemName.ReadOnly = true;
            // 
            // dgItemNo
            // 
            this.dgItemNo.DataPropertyName = "ItemNo";
            this.dgItemNo.HeaderText = "Item No";
            this.dgItemNo.Name = "dgItemNo";
            this.dgItemNo.ReadOnly = true;
            // 
            // dgCategory
            // 
            this.dgCategory.DataPropertyName = "CategoryName";
            this.dgCategory.HeaderText = "Category";
            this.dgCategory.Name = "dgCategory";
            this.dgCategory.ReadOnly = true;
            this.dgCategory.Width = 150;
            // 
            // dgDepartment
            // 
            this.dgDepartment.DataPropertyName = "ItemDepartmentID";
            this.dgDepartment.HeaderText = "Department";
            this.dgDepartment.Name = "dgDepartment";
            this.dgDepartment.ReadOnly = true;
            // 
            // dgBarcode
            // 
            this.dgBarcode.DataPropertyName = "Barcode";
            this.dgBarcode.HeaderText = "Barcode";
            this.dgBarcode.Name = "dgBarcode";
            this.dgBarcode.ReadOnly = true;
            // 
            // dgAlternateBarcode
            // 
            this.dgAlternateBarcode.DataPropertyName = "AlternateBarcode";
            this.dgAlternateBarcode.HeaderText = "Alternate Barcode";
            this.dgAlternateBarcode.Name = "dgAlternateBarcode";
            // 
            // dgPrice
            // 
            this.dgPrice.DataPropertyName = "RetailPrice";
            this.dgPrice.HeaderText = "Price";
            this.dgPrice.Name = "dgPrice";
            this.dgPrice.ReadOnly = true;
            // 
            // dgAttributes1
            // 
            this.dgAttributes1.DataPropertyName = "Attributes1";
            this.dgAttributes1.HeaderText = "Attributes1";
            this.dgAttributes1.Name = "dgAttributes1";
            // 
            // dgAttributes2
            // 
            this.dgAttributes2.DataPropertyName = "Attributes2";
            this.dgAttributes2.HeaderText = "Attributes2";
            this.dgAttributes2.Name = "dgAttributes2";
            // 
            // dgAttributes3
            // 
            this.dgAttributes3.DataPropertyName = "Attributes3";
            this.dgAttributes3.HeaderText = "Attributes3";
            this.dgAttributes3.Name = "dgAttributes3";
            // 
            // dgAttributes4
            // 
            this.dgAttributes4.DataPropertyName = "Attributes4";
            this.dgAttributes4.HeaderText = "Attributes4";
            this.dgAttributes4.Name = "dgAttributes4";
            // 
            // dgAttributes5
            // 
            this.dgAttributes5.DataPropertyName = "Attributes5";
            this.dgAttributes5.HeaderText = "Attributes5";
            this.dgAttributes5.Name = "dgAttributes5";
            // 
            // dgAttributes6
            // 
            this.dgAttributes6.DataPropertyName = "Attributes6";
            this.dgAttributes6.HeaderText = "Attributes6";
            this.dgAttributes6.Name = "dgAttributes6";
            // 
            // dgAttributes7
            // 
            this.dgAttributes7.DataPropertyName = "Attributes7";
            this.dgAttributes7.HeaderText = "Attributes7";
            this.dgAttributes7.Name = "dgAttributes7";
            // 
            // dgAttributes8
            // 
            this.dgAttributes8.DataPropertyName = "Attributes8";
            this.dgAttributes8.HeaderText = "Attributes8";
            this.dgAttributes8.Name = "dgAttributes8";
            // 
            // dgBarcodeText
            // 
            this.dgBarcodeText.DataPropertyName = "BarcodeText";
            this.dgBarcodeText.HeaderText = "BarcodeText";
            this.dgBarcodeText.Name = "dgBarcodeText";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printToolStripMenuItem,
            this.loadQtyToolStripMenuItem,
            this.clearSelectionToolStripMenuItem,
            this.templateToolStripMenuItem,
            this.tstxtSearch,
            this.cbCategory,
            this.exportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(5, 5);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(829, 27);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // loadQtyToolStripMenuItem
            // 
            this.loadQtyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromLastGoodsReceiveToolStripMenuItem,
            this.fromGoodsReceiveToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.loadQtyToolStripMenuItem.Name = "loadQtyToolStripMenuItem";
            this.loadQtyToolStripMenuItem.Size = new System.Drawing.Size(67, 23);
            this.loadQtyToolStripMenuItem.Text = "Load Qty";
            // 
            // fromLastGoodsReceiveToolStripMenuItem
            // 
            this.fromLastGoodsReceiveToolStripMenuItem.Name = "fromLastGoodsReceiveToolStripMenuItem";
            this.fromLastGoodsReceiveToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.fromLastGoodsReceiveToolStripMenuItem.Text = "From Last Goods Receive";
            this.fromLastGoodsReceiveToolStripMenuItem.Click += new System.EventHandler(this.fromLastGoodsReceiveToolStripMenuItem_Click);
            // 
            // fromGoodsReceiveToolStripMenuItem
            // 
            this.fromGoodsReceiveToolStripMenuItem.Name = "fromGoodsReceiveToolStripMenuItem";
            this.fromGoodsReceiveToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.fromGoodsReceiveToolStripMenuItem.Text = "From Goods Receive";
            this.fromGoodsReceiveToolStripMenuItem.Click += new System.EventHandler(this.fromGoodsReceiveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.loadToolStripMenuItem.Text = "Set All Quantity";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // clearSelectionToolStripMenuItem
            // 
            this.clearSelectionToolStripMenuItem.Name = "clearSelectionToolStripMenuItem";
            this.clearSelectionToolStripMenuItem.Size = new System.Drawing.Size(97, 23);
            this.clearSelectionToolStripMenuItem.Text = "Clear Selection";
            this.clearSelectionToolStripMenuItem.Click += new System.EventHandler(this.clearSelectionToolStripMenuItem_Click);
            // 
            // templateToolStripMenuItem
            // 
            this.templateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTemplateToolStripMenuItem,
            this.toolStripMenuItem1,
            this.editTemplateToolStripMenuItem,
            this.checkBToolStripMenuItem});
            this.templateToolStripMenuItem.Name = "templateToolStripMenuItem";
            this.templateToolStripMenuItem.Size = new System.Drawing.Size(69, 23);
            this.templateToolStripMenuItem.Text = "Template";
            // 
            // loadTemplateToolStripMenuItem
            // 
            this.loadTemplateToolStripMenuItem.Name = "loadTemplateToolStripMenuItem";
            this.loadTemplateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.loadTemplateToolStripMenuItem.Text = "Load Template";
            this.loadTemplateToolStripMenuItem.Click += new System.EventHandler(this.loadTemplateToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 6);
            // 
            // editTemplateToolStripMenuItem
            // 
            this.editTemplateToolStripMenuItem.Name = "editTemplateToolStripMenuItem";
            this.editTemplateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.editTemplateToolStripMenuItem.Text = "Edit Template";
            this.editTemplateToolStripMenuItem.Click += new System.EventHandler(this.editTemplateToolStripMenuItem_Click);
            // 
            // checkBToolStripMenuItem
            // 
            this.checkBToolStripMenuItem.Name = "checkBToolStripMenuItem";
            this.checkBToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.checkBToolStripMenuItem.Text = "Check Barcode Digit";
            this.checkBToolStripMenuItem.Click += new System.EventHandler(this.checkBToolStripMenuItem_Click);
            // 
            // tstxtSearch
            // 
            this.tstxtSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tstxtSearch.AutoToolTip = true;
            this.tstxtSearch.BackColor = System.Drawing.Color.White;
            this.tstxtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tstxtSearch.Name = "tstxtSearch";
            this.tstxtSearch.Size = new System.Drawing.Size(150, 23);
            this.tstxtSearch.ToolTipText = "Search for Item Name, Item No, Attributes, Barcode, Department and Category";
            this.tstxtSearch.Leave += new System.EventHandler(this.tstxtSearch_Leave);
            this.tstxtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tstxtSearch_KeyPress);
            // 
            // cbCategory
            // 
            this.cbCategory.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(121, 23);
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.cbCategory_SelectedIndexChanged);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printableFormatToolStripMenuItem,
            this.selectedItemsToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 23);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // openTemplateDialog
            // 
            this.openTemplateDialog.FileName = "openFileDialog1";
            this.openTemplateDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openTemplateDialog_FileOk);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ShowAlways = true;
            // 
            // printableFormatToolStripMenuItem
            // 
            this.printableFormatToolStripMenuItem.Name = "printableFormatToolStripMenuItem";
            this.printableFormatToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.printableFormatToolStripMenuItem.Text = "Printable Format";
            this.printableFormatToolStripMenuItem.Click += new System.EventHandler(this.printableFormatToolStripMenuItem_Click);
            // 
            // selectedItemsToolStripMenuItem
            // 
            this.selectedItemsToolStripMenuItem.Name = "selectedItemsToolStripMenuItem";
            this.selectedItemsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.selectedItemsToolStripMenuItem.Text = "Selected Items";
            this.selectedItemsToolStripMenuItem.Click += new System.EventHandler(this.selectedItemsToolStripMenuItem_Click);
            // 
            // exportSaveDialog
            // 
            this.exportSaveDialog.DefaultExt = "xlsx";
            this.exportSaveDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm\"";
            this.exportSaveDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.exportSaveDialog_FileOk);
            // 
            // frmPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 361);
            this.Controls.Add(this.Tabel);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmPrint";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Print Barcode";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPrint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Tabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSelectionToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openTemplateDialog;
        private System.Windows.Forms.ToolStripMenuItem templateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadQtyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromLastGoodsReceiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox tstxtSearch;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem fromGoodsReceiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox cbCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgQuantity;
        private System.Windows.Forms.DataGridViewButtonColumn dgPlus;
        private System.Windows.Forms.DataGridViewButtonColumn dgMinus;
        private System.Windows.Forms.DataGridViewButtonColumn dgCopyCustomQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAlternateBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAttributes1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAttributes2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAttributes3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAttributes4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAttributes5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAttributes6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAttributes7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAttributes8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgBarcodeText;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printableFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedItemsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog exportSaveDialog;
    }
}

