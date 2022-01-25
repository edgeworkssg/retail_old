namespace PowerInventory
{
    partial class frmExportTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExportTemplate));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbIsInInventory = new System.Windows.Forms.CheckBox();
            this.cbDeleted = new System.Windows.Forms.CheckBox();
            this.listCategory = new System.Windows.Forms.CheckedListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSelectAll = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listCategory);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.cbIsInInventory);
            this.flowLayoutPanel1.Controls.Add(this.cbDeleted);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // cbIsInInventory
            // 
            resources.ApplyResources(this.cbIsInInventory, "cbIsInInventory");
            this.cbIsInInventory.BackColor = System.Drawing.Color.Transparent;
            this.cbIsInInventory.Checked = true;
            this.cbIsInInventory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsInInventory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbIsInInventory.Name = "cbIsInInventory";
            this.cbIsInInventory.UseVisualStyleBackColor = false;
            // 
            // cbDeleted
            // 
            resources.ApplyResources(this.cbDeleted, "cbDeleted");
            this.cbDeleted.BackColor = System.Drawing.Color.Transparent;
            this.cbDeleted.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbDeleted.Name = "cbDeleted";
            this.cbDeleted.UseVisualStyleBackColor = false;
            this.cbDeleted.CheckedChanged += new System.EventHandler(this.cbDeleted_CheckedChanged);
            // 
            // listCategory
            // 
            this.listCategory.CheckOnClick = true;
            resources.ApplyResources(this.listCategory, "listCategory");
            this.listCategory.FormattingEnabled = true;
            this.listCategory.Name = "listCategory";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSelectAll);
            this.panel3.Controls.Add(this.label3);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // btnSelectAll
            // 
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.TabStop = true;
            this.btnSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnSelectAll_LinkClicked);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.BackColor = System.Drawing.Color.DarkGreen;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Controls.Add(this.btnExport);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.BackColor = System.Drawing.Color.Maroon;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // saveFileDialogExport
            // 
            this.saveFileDialogExport.DefaultExt = "csv";
            resources.ApplyResources(this.saveFileDialogExport, "saveFileDialogExport");
            this.saveFileDialogExport.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogExport_FileOk);
            // 
            // frmExportTemplate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.brown;
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmExportTemplate";
            this.Load += new System.EventHandler(this.frmExportTemplate_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox cbIsInInventory;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbDeleted;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox listCategory;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.LinkLabel btnSelectAll;
        private System.Windows.Forms.Label label3;
    }
}