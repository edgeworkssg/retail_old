namespace WinPowerPOS.AppointmentForms
{
    partial class frmRoomListing
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Staff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Service = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppointmentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckInByWho = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckOutByWho = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dateControl = new AppointmentBook.DateControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(748, 482);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(10, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(728, 462);
            this.panel3.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvPreview);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 63);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(728, 340);
            this.panel5.TabIndex = 7;
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.DateTimeColumn,
            this.ResourceName,
            this.Staff,
            this.Customer,
            this.Service,
            this.Duration,
            this.AppointmentID,
            this.CheckInByWho,
            this.CheckOutByWho});
            this.dgvPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPreview.Location = new System.Drawing.Point(0, 0);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.RowHeadersVisible = false;
            this.dgvPreview.Size = new System.Drawing.Size(728, 340);
            this.dgvPreview.TabIndex = 3;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ResourceID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // DateTimeColumn
            // 
            this.DateTimeColumn.DataPropertyName = "StartTime";
            this.DateTimeColumn.HeaderText = "Date / Time";
            this.DateTimeColumn.Name = "DateTimeColumn";
            // 
            // ResourceName
            // 
            this.ResourceName.DataPropertyName = "ResourceName";
            this.ResourceName.HeaderText = "Room";
            this.ResourceName.Name = "ResourceName";
            // 
            // Staff
            // 
            this.Staff.DataPropertyName = "Staff";
            this.Staff.HeaderText = "Staff";
            this.Staff.Name = "Staff";
            // 
            // Customer
            // 
            this.Customer.DataPropertyName = "Customer";
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            // 
            // Service
            // 
            this.Service.DataPropertyName = "ItemName";
            this.Service.HeaderText = "Service";
            this.Service.Name = "Service";
            // 
            // Duration
            // 
            this.Duration.DataPropertyName = "Duration";
            this.Duration.HeaderText = "Duration";
            this.Duration.Name = "Duration";
            // 
            // AppointmentID
            // 
            this.AppointmentID.DataPropertyName = "AppointmentID";
            this.AppointmentID.HeaderText = "AppointmentID";
            this.AppointmentID.Name = "AppointmentID";
            this.AppointmentID.Visible = false;
            // 
            // CheckInByWho
            // 
            this.CheckInByWho.DataPropertyName = "CheckInByWho";
            this.CheckInByWho.HeaderText = "CheckInByWho";
            this.CheckInByWho.Name = "CheckInByWho";
            this.CheckInByWho.Visible = false;
            // 
            // CheckOutByWho
            // 
            this.CheckOutByWho.DataPropertyName = "CheckOutByWho";
            this.CheckOutByWho.HeaderText = "CheckOutByWho";
            this.CheckOutByWho.Name = "CheckOutByWho";
            this.CheckOutByWho.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dateControl);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(728, 63);
            this.panel4.TabIndex = 6;
            // 
            // dateControl
            // 
            this.dateControl.BackColor = System.Drawing.SystemColors.Control;
            this.dateControl.Day = new System.DateTime(2014, 1, 28, 0, 0, 0, 0);
            this.dateControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.dateControl.Format = "dddd";
            this.dateControl.Location = new System.Drawing.Point(0, 0);
            this.dateControl.MaximumSize = new System.Drawing.Size(400, 50);
            this.dateControl.MinimumSize = new System.Drawing.Size(310, 45);
            this.dateControl.Name = "dateControl";
            this.dateControl.Size = new System.Drawing.Size(310, 50);
            this.dateControl.TabIndex = 6;
            this.dateControl.Changed += new System.EventHandler(this.dateControl_Changed);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 403);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(728, 59);
            this.panel2.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(604, 8);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 48);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // frmRoomListing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 482);
            this.Controls.Add(this.panel1);
            this.Name = "frmRoomListing";
            this.Text = "ROOM LISTING";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmRoomListing_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dgvPreview;
        private AppointmentBook.DateControl dateControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResourceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Staff;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Service;
        private System.Windows.Forms.DataGridViewTextBoxColumn Duration;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppointmentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckInByWho;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckOutByWho;
    }
}