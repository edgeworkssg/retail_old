namespace WinPowerPOS.AppointmentForms
{
    partial class frmGuestbook
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.Search = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.bgDownload = new System.ComponentModel.BackgroundWorker();
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MembershipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContactNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nationality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutletName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlProgress);
            this.panel1.Controls.Add(this.dgvPreview);
            this.panel1.Controls.Add(this.pnlSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(754, 459);
            this.panel1.TabIndex = 0;
            // 
            // pnlProgress
            // 
            this.pnlProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Location = new System.Drawing.Point(277, 179);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(200, 100);
            this.pnlProgress.TabIndex = 70;
            this.pnlProgress.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(55, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Please Wait...";
            // 
            // pgb1
            // 
            this.pgb1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pgb1.Location = new System.Drawing.Point(25, 42);
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Size = new System.Drawing.Size(159, 23);
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgb1.TabIndex = 0;
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MembershipNo,
            this.MembershipName,
            this.NRIC,
            this.ContactNo,
            this.InTime,
            this.OutTime,
            this.Nationality,
            this.Remarks,
            this.OutletName});
            this.dgvPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPreview.Location = new System.Drawing.Point(10, 129);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.RowHeadersVisible = false;
            this.dgvPreview.Size = new System.Drawing.Size(734, 320);
            this.dgvPreview.TabIndex = 7;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.Search);
            this.pnlSearch.Controls.Add(this.btnClose);
            this.pnlSearch.Controls.Add(this.btnConfirm);
            this.pnlSearch.Controls.Add(this.dtEnd);
            this.pnlSearch.Controls.Add(this.label7);
            this.pnlSearch.Controls.Add(this.dtStart);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(10, 10);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(734, 119);
            this.pnlSearch.TabIndex = 6;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(432, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(244, 20);
            this.txtSearch.TabIndex = 71;
            // 
            // Search
            // 
            this.Search.AutoSize = true;
            this.Search.Location = new System.Drawing.Point(385, 11);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(41, 13);
            this.Search.TabIndex = 70;
            this.Search.Text = "Search";
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(176, 60);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 35);
            this.btnClose.TabIndex = 69;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnConfirm.CausesValidation = false;
            this.btnConfirm.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfirm.Location = new System.Drawing.Point(88, 59);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(0);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(85, 35);
            this.btnConfirm.TabIndex = 62;
            this.btnConfirm.Text = "SEARCH";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // dtEnd
            // 
            this.dtEnd.CausesValidation = false;
            this.dtEnd.CustomFormat = "dd MMM yyyy HH:mm:ss";
            this.dtEnd.Location = new System.Drawing.Point(88, 34);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.ShowCheckBox = true;
            this.dtEnd.Size = new System.Drawing.Size(262, 20);
            this.dtEnd.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "End Date";
            // 
            // dtStart
            // 
            this.dtStart.CausesValidation = false;
            this.dtStart.CustomFormat = "dd MMM yyyy HH:mm:ss";
            this.dtStart.Location = new System.Drawing.Point(88, 6);
            this.dtStart.Name = "dtStart";
            this.dtStart.ShowCheckBox = true;
            this.dtStart.Size = new System.Drawing.Size(262, 20);
            this.dtStart.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Date";
            // 
            // bgDownload
            // 
            this.bgDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDownload_DoWork);
            this.bgDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgDownload_RunWorkerCompleted);
            // 
            // MembershipNo
            // 
            this.MembershipNo.DataPropertyName = "MembershipNo";
            this.MembershipNo.HeaderText = "Member Code";
            this.MembershipNo.Name = "MembershipNo";
            // 
            // MembershipName
            // 
            this.MembershipName.DataPropertyName = "NameToAppear";
            this.MembershipName.HeaderText = "Member Name";
            this.MembershipName.Name = "MembershipName";
            // 
            // NRIC
            // 
            this.NRIC.DataPropertyName = "NRIC";
            this.NRIC.HeaderText = "IC No";
            this.NRIC.Name = "NRIC";
            // 
            // ContactNo
            // 
            this.ContactNo.DataPropertyName = "ContactNo";
            this.ContactNo.HeaderText = "Contact No";
            this.ContactNo.Name = "ContactNo";
            // 
            // InTime
            // 
            this.InTime.DataPropertyName = "InTime";
            this.InTime.HeaderText = "Sign In";
            this.InTime.Name = "InTime";
            // 
            // OutTime
            // 
            this.OutTime.DataPropertyName = "OutTime";
            this.OutTime.HeaderText = "Sign Out";
            this.OutTime.Name = "OutTime";
            // 
            // Nationality
            // 
            this.Nationality.DataPropertyName = "Nationality";
            this.Nationality.HeaderText = "Nationality";
            this.Nationality.Name = "Nationality";
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            // 
            // OutletName
            // 
            this.OutletName.DataPropertyName = "OutletName";
            this.OutletName.HeaderText = "Outlet Name";
            this.OutletName.Name = "OutletName";
            // 
            // frmGuestbook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 459);
            this.Controls.Add(this.panel1);
            this.Name = "frmGuestbook";
            this.Text = "Guest Book";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmGuestbook_Load);
            this.panel1.ResumeLayout(false);
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlSearch;
        internal System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.ComponentModel.BackgroundWorker bgDownload;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
        internal System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label Search;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContactNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn InTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nationality;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutletName;

    }
}