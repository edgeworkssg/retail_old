namespace WinPowerPOS
{
    partial class frmSalesReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.Open = new System.Windows.Forms.DataGridViewButtonColumn();
            this.OpenA4 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CounterCloseID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSystemRecorded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalActualCollected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Variance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cashier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Supervisor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepositBagNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.print = new System.Windows.Forms.DataGridViewButtonColumn();
            this.email = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(73, 8);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(202, 20);
            this.dtpStartDate.TabIndex = 7;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(353, 8);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(200, 20);
            this.dtpEndDate.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(295, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "End Date";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(791, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(79, 32);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(872, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(79, 32);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Open,
            this.OpenA4,
            this.EndTime,
            this.CounterCloseID,
            this.TotalSystemRecorded,
            this.TotalActualCollected,
            this.Variance,
            this.Cashier,
            this.Supervisor,
            this.DepositBagNo,
            this.print,
            this.email});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvReport.Location = new System.Drawing.Point(12, 42);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.Size = new System.Drawing.Size(939, 508);
            this.dgvReport.TabIndex = 6;
            this.dgvReport.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvReport_RowPrePaint);
            this.dgvReport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellContentClick);
            // 
            // Open
            // 
            this.Open.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Open.HeaderText = "";
            this.Open.Name = "Open";
            this.Open.ReadOnly = true;
            this.Open.Text = "Print";
            this.Open.UseColumnTextForButtonValue = true;
            this.Open.Width = 70;
            // 
            // OpenA4
            // 
            this.OpenA4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OpenA4.HeaderText = "";
            this.OpenA4.Name = "OpenA4";
            this.OpenA4.ReadOnly = true;
            this.OpenA4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.OpenA4.Text = "Print A4";
            this.OpenA4.UseColumnTextForButtonValue = true;
            this.OpenA4.Width = 5;
            // 
            // EndTime
            // 
            this.EndTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EndTime.DataPropertyName = "EndTime";
            this.EndTime.HeaderText = "Closing Time";
            this.EndTime.Name = "EndTime";
            this.EndTime.ReadOnly = true;
            // 
            // CounterCloseID
            // 
            this.CounterCloseID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CounterCloseID.DataPropertyName = "CounterCloseID";
            this.CounterCloseID.HeaderText = "Ref No";
            this.CounterCloseID.Name = "CounterCloseID";
            this.CounterCloseID.ReadOnly = true;
            // 
            // TotalSystemRecorded
            // 
            this.TotalSystemRecorded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TotalSystemRecorded.DataPropertyName = "TotalSystemRecorded";
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = "0";
            this.TotalSystemRecorded.DefaultCellStyle = dataGridViewCellStyle2;
            this.TotalSystemRecorded.HeaderText = "Total Recorded";
            this.TotalSystemRecorded.Name = "TotalSystemRecorded";
            this.TotalSystemRecorded.ReadOnly = true;
            // 
            // TotalActualCollected
            // 
            this.TotalActualCollected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TotalActualCollected.DataPropertyName = "TotalActualCollected";
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = "0";
            this.TotalActualCollected.DefaultCellStyle = dataGridViewCellStyle3;
            this.TotalActualCollected.HeaderText = "Total Collected";
            this.TotalActualCollected.Name = "TotalActualCollected";
            this.TotalActualCollected.ReadOnly = true;
            // 
            // Variance
            // 
            this.Variance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Format = "C2";
            dataGridViewCellStyle4.NullValue = "0";
            this.Variance.DefaultCellStyle = dataGridViewCellStyle4;
            this.Variance.HeaderText = "Surplus/Defi";
            this.Variance.Name = "Variance";
            this.Variance.ReadOnly = true;
            // 
            // Cashier
            // 
            this.Cashier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cashier.DataPropertyName = "Cashier";
            this.Cashier.HeaderText = "Cashier";
            this.Cashier.Name = "Cashier";
            this.Cashier.ReadOnly = true;
            // 
            // Supervisor
            // 
            this.Supervisor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Supervisor.DataPropertyName = "Supervisor";
            this.Supervisor.HeaderText = "Verifier";
            this.Supervisor.Name = "Supervisor";
            this.Supervisor.ReadOnly = true;
            // 
            // DepositBagNo
            // 
            this.DepositBagNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DepositBagNo.DataPropertyName = "DepositBagNo";
            this.DepositBagNo.HeaderText = "DepositBagNo";
            this.DepositBagNo.Name = "DepositBagNo";
            this.DepositBagNo.ReadOnly = true;
            // 
            // print
            // 
            this.print.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.print.HeaderText = "";
            this.print.Name = "print";
            this.print.ReadOnly = true;
            this.print.Text = "Print Receipts";
            this.print.UseColumnTextForButtonValue = true;
            this.print.Visible = false;
            this.print.Width = 110;
            // 
            // email
            // 
            this.email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.email.HeaderText = "";
            this.email.Name = "email";
            this.email.ReadOnly = true;
            this.email.Text = "email";
            this.email.UseColumnTextForButtonValue = true;
            this.email.Visible = false;
            this.email.Width = 110;
            // 
            // frmSalesReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 562);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.dgvReport);
            this.DoubleBuffered = true;
            this.Name = "frmSalesReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSalesReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.DataGridViewButtonColumn Open;
        private System.Windows.Forms.DataGridViewButtonColumn OpenA4;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CounterCloseID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSystemRecorded;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalActualCollected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variance;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cashier;
        private System.Windows.Forms.DataGridViewTextBoxColumn Supervisor;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepositBagNo;
        private System.Windows.Forms.DataGridViewButtonColumn print;
        private System.Windows.Forms.DataGridViewButtonColumn email;
    }
}