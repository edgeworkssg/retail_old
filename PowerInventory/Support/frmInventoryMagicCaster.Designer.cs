namespace PowerInventory.Support
{
    partial class frmInventoryMagicCaster
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
            this.Caster = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.pbValidation = new System.Windows.Forms.ProgressBar();
            this.lblValidation = new System.Windows.Forms.Label();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.InventoryMagix1 = new PowerInventory.Support.InventoryMagix();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAllOutMagic = new System.Windows.Forms.Button();
            this.cbNormalTransaction = new System.Windows.Forms.CheckBox();
            this.cbUndeductedSales = new System.Windows.Forms.CheckBox();
            this.cbErrorStockOut = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.Search = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Caster
            // 
            this.Caster.WorkerReportsProgress = true;
            this.Caster.WorkerSupportsCancellation = true;
            this.Caster.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Caster_DoWork);
            this.Caster.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Caster_RunWorkerCompleted);
            this.Caster.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Caster_ProgressChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 41);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "Replenish Magix";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pbValidation
            // 
            this.pbValidation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbValidation.Location = new System.Drawing.Point(120, 60);
            this.pbValidation.Name = "pbValidation";
            this.pbValidation.Size = new System.Drawing.Size(465, 23);
            this.pbValidation.Step = 1;
            this.pbValidation.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbValidation.TabIndex = 1;
            // 
            // lblValidation
            // 
            this.lblValidation.AutoSize = true;
            this.lblValidation.Location = new System.Drawing.Point(120, 41);
            this.lblValidation.Name = "lblValidation";
            this.lblValidation.Size = new System.Drawing.Size(68, 16);
            this.lblValidation.TabIndex = 2;
            this.lblValidation.Text = "Validating";
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.DisplayGroupTree = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(13, 90);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.Size = new System.Drawing.Size(381, 254);
            this.crystalReportViewer1.TabIndex = 3;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.btnAllOutMagic);
            this.panel1.Controls.Add(this.cbNormalTransaction);
            this.panel1.Controls.Add(this.cbUndeductedSales);
            this.panel1.Controls.Add(this.cbErrorStockOut);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(400, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(185, 255);
            this.panel1.TabIndex = 4;
            // 
            // btnAllOutMagic
            // 
            this.btnAllOutMagic.Location = new System.Drawing.Point(6, 3);
            this.btnAllOutMagic.Name = "btnAllOutMagic";
            this.btnAllOutMagic.Size = new System.Drawing.Size(176, 26);
            this.btnAllOutMagic.TabIndex = 8;
            this.btnAllOutMagic.Text = "All Out Magix";
            this.btnAllOutMagic.UseVisualStyleBackColor = true;
            this.btnAllOutMagic.Click += new System.EventHandler(this.btnAllOutMagic_Click);
            // 
            // cbNormalTransaction
            // 
            this.cbNormalTransaction.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbNormalTransaction.Checked = true;
            this.cbNormalTransaction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNormalTransaction.Location = new System.Drawing.Point(6, 81);
            this.cbNormalTransaction.Name = "cbNormalTransaction";
            this.cbNormalTransaction.Size = new System.Drawing.Size(176, 26);
            this.cbNormalTransaction.TabIndex = 7;
            this.cbNormalTransaction.Text = "Normal Transaction";
            this.cbNormalTransaction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbNormalTransaction.UseVisualStyleBackColor = true;
            // 
            // cbUndeductedSales
            // 
            this.cbUndeductedSales.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbUndeductedSales.Checked = true;
            this.cbUndeductedSales.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUndeductedSales.Location = new System.Drawing.Point(6, 55);
            this.cbUndeductedSales.Name = "cbUndeductedSales";
            this.cbUndeductedSales.Size = new System.Drawing.Size(176, 26);
            this.cbUndeductedSales.TabIndex = 6;
            this.cbUndeductedSales.Text = "Undeducted Sales";
            this.cbUndeductedSales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbUndeductedSales.UseVisualStyleBackColor = true;
            // 
            // cbErrorStockOut
            // 
            this.cbErrorStockOut.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbErrorStockOut.Checked = true;
            this.cbErrorStockOut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbErrorStockOut.Location = new System.Drawing.Point(6, 29);
            this.cbErrorStockOut.Name = "cbErrorStockOut";
            this.cbErrorStockOut.Size = new System.Drawing.Size(176, 26);
            this.cbErrorStockOut.TabIndex = 5;
            this.cbErrorStockOut.Text = "Error Stock Out";
            this.cbErrorStockOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbErrorStockOut.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Magix Word";
            // 
            // tSearch
            // 
            this.tSearch.Location = new System.Drawing.Point(96, 12);
            this.tSearch.Name = "tSearch";
            this.tSearch.Size = new System.Drawing.Size(489, 22);
            this.tSearch.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(46, 132);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 42);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Cast Magix";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // Search
            // 
            this.Search.WorkerReportsProgress = true;
            this.Search.WorkerSupportsCancellation = true;
            this.Search.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Search_DoWork);
            this.Search.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Search_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(6, 181);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(176, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 5;
            // 
            // frmInventoryMagicCaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 356);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.lblValidation);
            this.Controls.Add(this.pbValidation);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tSearch);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmInventoryMagicCaster";
            this.Text = "Inventory Magic Caster";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker Caster;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar pbValidation;
        private System.Windows.Forms.Label lblValidation;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private InventoryMagix InventoryMagix1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbNormalTransaction;
        private System.Windows.Forms.CheckBox cbUndeductedSales;
        private System.Windows.Forms.CheckBox cbErrorStockOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAllOutMagic;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker Search;
    }
}