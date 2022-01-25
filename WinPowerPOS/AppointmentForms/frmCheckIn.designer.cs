namespace WinPowerPOS.AppointmentForms
{
    partial class frmCheckIn
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCheckIn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPax = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.colChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Room = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Staff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckIn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckOut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnViewAppointment = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(693, 469);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvPreview);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(10, 80);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(673, 304);
            this.panel4.TabIndex = 2;
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChecked,
            this.ID,
            this.Customer,
            this.Room,
            this.StartTime,
            this.Staff,
            this.CheckIn,
            this.CheckOut,
            this.btnViewAppointment});
            this.dgvPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPreview.Location = new System.Drawing.Point(0, 0);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.RowHeadersVisible = false;
            this.dgvPreview.Size = new System.Drawing.Size(673, 304);
            this.dgvPreview.TabIndex = 0;
            this.dgvPreview.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvPreview_RowPostPaint);
            this.dgvPreview.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPreview_CellContentClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 384);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10);
            this.panel3.Size = new System.Drawing.Size(673, 75);
            this.panel3.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnClose);
            this.panel5.Controls.Add(this.btnCheckIn);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(358, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(305, 55);
            this.panel5.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(161, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 45);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCheckIn
            // 
            this.btnCheckIn.BackColor = System.Drawing.Color.White;
            this.btnCheckIn.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnCheckIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckIn.ForeColor = System.Drawing.Color.White;
            this.btnCheckIn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCheckIn.Location = new System.Drawing.Point(13, 4);
            this.btnCheckIn.Margin = new System.Windows.Forms.Padding(4);
            this.btnCheckIn.Name = "btnCheckIn";
            this.btnCheckIn.Size = new System.Drawing.Size(140, 45);
            this.btnCheckIn.TabIndex = 27;
            this.btnCheckIn.Text = "CHECK IN";
            this.btnCheckIn.UseVisualStyleBackColor = false;
            this.btnCheckIn.Click += new System.EventHandler(this.btnCheckIn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblDate);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.lblPax);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 10);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(673, 70);
            this.panel2.TabIndex = 0;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(96, 19);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(45, 16);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date";
            // 
            // lblPax
            // 
            this.lblPax.AutoSize = true;
            this.lblPax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPax.Location = new System.Drawing.Point(96, 44);
            this.lblPax.Name = "lblPax";
            this.lblPax.Size = new System.Drawing.Size(45, 16);
            this.lblPax.TabIndex = 1;
            this.lblPax.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "No Of Pax";
            // 
            // colChecked
            // 
            this.colChecked.FillWeight = 35.53299F;
            this.colChecked.HeaderText = "";
            this.colChecked.Name = "colChecked";
            // 
            // ID
            // 
            this.ID.DataPropertyName = "AppointmentId";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // Customer
            // 
            this.Customer.DataPropertyName = "NameToAppear";
            this.Customer.FillWeight = 110.7445F;
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            // 
            // Room
            // 
            this.Room.DataPropertyName = "ResourceName";
            this.Room.FillWeight = 110.7445F;
            this.Room.HeaderText = "Room";
            this.Room.Name = "Room";
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartTime";
            this.StartTime.FillWeight = 110.7445F;
            this.StartTime.HeaderText = "Appoinment Date & Time";
            this.StartTime.Name = "StartTime";
            // 
            // Staff
            // 
            this.Staff.DataPropertyName = "UserName";
            this.Staff.FillWeight = 110.7445F;
            this.Staff.HeaderText = "Staff";
            this.Staff.Name = "Staff";
            // 
            // CheckIn
            // 
            this.CheckIn.DataPropertyName = "CheckIn";
            this.CheckIn.FillWeight = 110.7445F;
            this.CheckIn.HeaderText = "Check In";
            this.CheckIn.Name = "CheckIn";
            // 
            // CheckOut
            // 
            this.CheckOut.DataPropertyName = "CheckOut";
            this.CheckOut.HeaderText = "Check Out";
            this.CheckOut.Name = "CheckOut";
            // 
            // btnViewAppointment
            // 
            this.btnViewAppointment.FillWeight = 110.7445F;
            this.btnViewAppointment.HeaderText = "View Appointment";
            this.btnViewAppointment.Name = "btnViewAppointment";
            this.btnViewAppointment.Text = "View Appointment";
            this.btnViewAppointment.UseColumnTextForButtonValue = true;
            // 
            // frmCheckIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 469);
            this.Controls.Add(this.panel1);
            this.Name = "frmCheckIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Check In Room";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.Label lblPax;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCheckIn;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChecked;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Room;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Staff;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckOut;
        private System.Windows.Forms.DataGridViewButtonColumn btnViewAppointment;
    }
}