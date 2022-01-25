namespace AttendanceTracker
{
    partial class frmMembershipAttendance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMembershipAttendance));
            this.epOrder = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlBills = new System.Windows.Forms.Panel();
            this.btnSync = new System.Windows.Forms.Button();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvAttendance = new System.Windows.Forms.DataGridView();
            this.ActivityDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivityType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtScan = new System.Windows.Forms.TextBox();
            this.llPOSName = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.epOrder)).BeginInit();
            this.pnlBills.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // epOrder
            // 
            this.epOrder.ContainerControl = this;
            // 
            // pnlBills
            // 
            resources.ApplyResources(this.pnlBills, "pnlBills");
            this.pnlBills.BackColor = System.Drawing.Color.White;
            this.pnlBills.Controls.Add(this.llPOSName);
            this.pnlBills.Controls.Add(this.btnSync);
            this.pnlBills.Controls.Add(this.lblWarning);
            this.pnlBills.Controls.Add(this.lblMsg);
            this.pnlBills.Controls.Add(this.groupBox3);
            this.pnlBills.Controls.Add(this.groupBox2);
            this.pnlBills.Controls.Add(this.groupBox1);
            this.pnlBills.Name = "pnlBills";
            // 
            // btnSync
            // 
            this.btnSync.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnSync, "btnSync");
            this.btnSync.ForeColor = System.Drawing.Color.Green;
            this.btnSync.Name = "btnSync";
            this.btnSync.UseVisualStyleBackColor = false;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // lblWarning
            // 
            resources.ApplyResources(this.lblWarning, "lblWarning");
            this.lblWarning.BackColor = System.Drawing.Color.Transparent;
            this.lblWarning.ForeColor = System.Drawing.Color.Transparent;
            this.lblWarning.Name = "lblWarning";
            // 
            // lblMsg
            // 
            resources.ApplyResources(this.lblMsg, "lblMsg");
            this.lblMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblMsg.ForeColor = System.Drawing.Color.Transparent;
            this.lblMsg.Name = "lblMsg";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.pbImage);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // pbImage
            // 
            resources.ApplyResources(this.pbImage, "pbImage");
            this.pbImage.Name = "pbImage";
            this.pbImage.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dgvAttendance);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // dgvAttendance
            // 
            this.dgvAttendance.AllowUserToAddRows = false;
            this.dgvAttendance.AllowUserToDeleteRows = false;
            this.dgvAttendance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttendance.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActivityDateTime,
            this.MembershipNo,
            this.FirstName,
            this.ActivityType});
            this.dgvAttendance.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            resources.ApplyResources(this.dgvAttendance, "dgvAttendance");
            this.dgvAttendance.Name = "dgvAttendance";
            this.dgvAttendance.RowHeadersVisible = false;
            // 
            // ActivityDateTime
            // 
            this.ActivityDateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ActivityDateTime.DataPropertyName = "ActivityDateTime";
            resources.ApplyResources(this.ActivityDateTime, "ActivityDateTime");
            this.ActivityDateTime.Name = "ActivityDateTime";
            // 
            // MembershipNo
            // 
            this.MembershipNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MembershipNo.DataPropertyName = "MembershipNo";
            resources.ApplyResources(this.MembershipNo, "MembershipNo");
            this.MembershipNo.Name = "MembershipNo";
            // 
            // FirstName
            // 
            this.FirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FirstName.DataPropertyName = "NameToAppear";
            resources.ApplyResources(this.FirstName, "FirstName");
            this.FirstName.Name = "FirstName";
            // 
            // ActivityType
            // 
            this.ActivityType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ActivityType.DataPropertyName = "ActivityName";
            resources.ApplyResources(this.ActivityType, "ActivityType");
            this.ActivityType.Name = "ActivityType";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtScan);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtScan
            // 
            resources.ApplyResources(this.txtScan, "txtScan");
            this.txtScan.Name = "txtScan";
            this.txtScan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtScan_KeyDown);
            // 
            // llPOSName
            // 
            resources.ApplyResources(this.llPOSName, "llPOSName");
            this.llPOSName.BackColor = System.Drawing.Color.Transparent;
            this.llPOSName.Name = "llPOSName";
            this.llPOSName.TabStop = true;
            this.llPOSName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llPOSName_LinkClicked);
            // 
            // frmMembershipAttendance
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.pnlBills);
            this.KeyPreview = true;
            this.Name = "frmMembershipAttendance";
            this.Load += new System.EventHandler(this.frmAttendance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.epOrder)).EndInit();
            this.pnlBills.ResumeLayout(false);
            this.pnlBills.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider epOrder;
        private System.Windows.Forms.Panel pnlBills;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtScan;
        private System.Windows.Forms.DataGridView dgvAttendance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivityDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivityType;
        internal System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.LinkLabel llPOSName;
    }
}

