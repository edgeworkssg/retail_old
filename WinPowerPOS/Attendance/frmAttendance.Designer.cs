namespace WinPowerPOS.Attendance
{
    partial class frmAttendance
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgActive = new System.Windows.Forms.DataGridView();
            this.dgvcMembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcMembershipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTime = new System.Windows.Forms.Label();
            this.txtScan = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblCardNo = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTotalActive = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgCheckOut = new System.Windows.Forms.DataGridView();
            this.dgvcOCardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOMemberName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcODuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerData = new System.Windows.Forms.Timer(this.components);
            this.btnReport = new System.Windows.Forms.Button();
            this.btnCreateInvoice = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgActive)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCheckOut)).BeginInit();
            this.SuspendLayout();
            // 
            // dgActive
            // 
            this.dgActive.AllowUserToAddRows = false;
            this.dgActive.AllowUserToDeleteRows = false;
            this.dgActive.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgActive.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcMembershipNo,
            this.dgvcMembershipName,
            this.dgvcStartTime,
            this.dgvcDuration});
            this.dgActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgActive.Location = new System.Drawing.Point(3, 3);
            this.dgActive.Name = "dgActive";
            this.dgActive.ReadOnly = true;
            this.dgActive.RowHeadersVisible = false;
            this.dgActive.Size = new System.Drawing.Size(558, 440);
            this.dgActive.TabIndex = 0;
            // 
            // dgvcMembershipNo
            // 
            this.dgvcMembershipNo.DataPropertyName = "MembershipNo";
            this.dgvcMembershipNo.HeaderText = "Card No";
            this.dgvcMembershipNo.Name = "dgvcMembershipNo";
            this.dgvcMembershipNo.ReadOnly = true;
            // 
            // dgvcMembershipName
            // 
            this.dgvcMembershipName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcMembershipName.DataPropertyName = "MembershipName";
            this.dgvcMembershipName.HeaderText = "Member Name";
            this.dgvcMembershipName.MinimumWidth = 150;
            this.dgvcMembershipName.Name = "dgvcMembershipName";
            this.dgvcMembershipName.ReadOnly = true;
            // 
            // dgvcStartTime
            // 
            this.dgvcStartTime.DataPropertyName = "LoginTime";
            dataGridViewCellStyle16.Format = "dd MMM yyyy HH:mm:ss";
            dataGridViewCellStyle16.NullValue = null;
            this.dgvcStartTime.DefaultCellStyle = dataGridViewCellStyle16;
            this.dgvcStartTime.HeaderText = "Start";
            this.dgvcStartTime.Name = "dgvcStartTime";
            this.dgvcStartTime.ReadOnly = true;
            this.dgvcStartTime.Width = 150;
            // 
            // dgvcDuration
            // 
            this.dgvcDuration.DataPropertyName = "Duration";
            dataGridViewCellStyle17.Format = "HH\'h\' mm\'m\'";
            dataGridViewCellStyle17.NullValue = null;
            this.dgvcDuration.DefaultCellStyle = dataGridViewCellStyle17;
            this.dgvcDuration.HeaderText = "Duration";
            this.dgvcDuration.Name = "dgvcDuration";
            this.dgvcDuration.ReadOnly = true;
            this.dgvcDuration.Width = 150;
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(606, 24);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(225, 63);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "00:00:00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtScan
            // 
            this.txtScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScan.Location = new System.Drawing.Point(590, 116);
            this.txtScan.Name = "txtScan";
            this.txtScan.Size = new System.Drawing.Size(216, 22);
            this.txtScan.TabIndex = 5;
            this.txtScan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtScan_KeyDown);
            this.txtScan.Leave += new System.EventHandler(this.txtScan_Leave);
            this.txtScan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtScan_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(591, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Scan Membership Card";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblDuration);
            this.groupBox1.Controls.Add(this.lblEnd);
            this.groupBox1.Controls.Add(this.lblStart);
            this.groupBox1.Controls.Add(this.lblCardNo);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(590, 152);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 143);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Last Check Out for Today";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(129, 113);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(81, 16);
            this.lblDuration.TabIndex = 18;
            this.lblDuration.Text = "00h 00m 00s";
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(129, 85);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(68, 16);
            this.lblEnd.TabIndex = 17;
            this.lblEnd.Text = "00 : 00 : 00";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(129, 57);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(68, 16);
            this.lblStart.TabIndex = 16;
            this.lblStart.Text = "00 : 00 : 00";
            // 
            // lblCardNo
            // 
            this.lblCardNo.AutoSize = true;
            this.lblCardNo.Location = new System.Drawing.Point(129, 29);
            this.lblCardNo.Name = "lblCardNo";
            this.lblCardNo.Size = new System.Drawing.Size(12, 16);
            this.lblCardNo.TabIndex = 15;
            this.lblCardNo.Text = "-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 16);
            this.label6.TabIndex = 14;
            this.label6.Text = "Duration :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "End :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Start :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Card No :";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblTotalActive);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Location = new System.Drawing.Point(590, 301);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 118);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistic";
            // 
            // lblTotalActive
            // 
            this.lblTotalActive.AutoSize = true;
            this.lblTotalActive.Location = new System.Drawing.Point(129, 29);
            this.lblTotalActive.Name = "lblTotalActive";
            this.lblTotalActive.Size = new System.Drawing.Size(15, 16);
            this.lblTotalActive.TabIndex = 15;
            this.lblTotalActive.Text = "0";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 29);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(117, 16);
            this.label18.TabIndex = 7;
            this.label18.Text = "Total Active Card :";
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(812, 113);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(38, 28);
            this.btnGo.TabIndex = 12;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // timerClock
            // 
            this.timerClock.Enabled = true;
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(572, 505);
            this.tabControl1.TabIndex = 13;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgActive);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(564, 446);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Active";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgCheckOut);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(564, 473);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Check Out";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgCheckOut
            // 
            this.dgCheckOut.AllowUserToAddRows = false;
            this.dgCheckOut.AllowUserToDeleteRows = false;
            this.dgCheckOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCheckOut.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcOCardNo,
            this.dgvcOMemberName,
            this.dgvcOStartTime,
            this.dgvcODuration,
            this.dgvcOEndTime});
            this.dgCheckOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCheckOut.Location = new System.Drawing.Point(3, 3);
            this.dgCheckOut.Name = "dgCheckOut";
            this.dgCheckOut.ReadOnly = true;
            this.dgCheckOut.RowHeadersVisible = false;
            this.dgCheckOut.Size = new System.Drawing.Size(558, 467);
            this.dgCheckOut.TabIndex = 4;
            // 
            // dgvcOCardNo
            // 
            this.dgvcOCardNo.DataPropertyName = "MembershipNo";
            this.dgvcOCardNo.HeaderText = "Card No";
            this.dgvcOCardNo.Name = "dgvcOCardNo";
            this.dgvcOCardNo.ReadOnly = true;
            // 
            // dgvcOMemberName
            // 
            this.dgvcOMemberName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcOMemberName.DataPropertyName = "MembershipName";
            this.dgvcOMemberName.HeaderText = "Member Name";
            this.dgvcOMemberName.MinimumWidth = 150;
            this.dgvcOMemberName.Name = "dgvcOMemberName";
            this.dgvcOMemberName.ReadOnly = true;
            // 
            // dgvcOStartTime
            // 
            this.dgvcOStartTime.DataPropertyName = "LoginTime";
            dataGridViewCellStyle18.Format = "dd MMM yyyy HH:mm:ss";
            dataGridViewCellStyle18.NullValue = null;
            this.dgvcOStartTime.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgvcOStartTime.HeaderText = "Start";
            this.dgvcOStartTime.Name = "dgvcOStartTime";
            this.dgvcOStartTime.ReadOnly = true;
            this.dgvcOStartTime.Width = 150;
            // 
            // dgvcODuration
            // 
            this.dgvcODuration.DataPropertyName = "Duration";
            dataGridViewCellStyle19.Format = "HH\'h\' mm\'m\' ss\'s\'";
            this.dgvcODuration.DefaultCellStyle = dataGridViewCellStyle19;
            this.dgvcODuration.HeaderText = "Duration";
            this.dgvcODuration.Name = "dgvcODuration";
            this.dgvcODuration.ReadOnly = true;
            this.dgvcODuration.Width = 150;
            // 
            // dgvcOEndTime
            // 
            this.dgvcOEndTime.DataPropertyName = "LogoutTime";
            dataGridViewCellStyle20.Format = "dd MMM yyyy HH:mm:ss";
            this.dgvcOEndTime.DefaultCellStyle = dataGridViewCellStyle20;
            this.dgvcOEndTime.HeaderText = "End";
            this.dgvcOEndTime.Name = "dgvcOEndTime";
            this.dgvcOEndTime.ReadOnly = true;
            this.dgvcOEndTime.Width = 150;
            // 
            // timerData
            // 
            this.timerData.Enabled = true;
            this.timerData.Interval = 30000;
            this.timerData.Tick += new System.EventHandler(this.timerData_Tick);
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReport.BackColor = System.Drawing.Color.White;
            this.btnReport.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnReport.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnReport.ForeColor = System.Drawing.Color.Black;
            this.btnReport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnReport.Location = new System.Drawing.Point(722, 477);
            this.btnReport.Margin = new System.Windows.Forms.Padding(0);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(131, 43);
            this.btnReport.TabIndex = 85;
            this.btnReport.Text = "REPORT";
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnCreateInvoice
            // 
            this.btnCreateInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateInvoice.BackColor = System.Drawing.Color.White;
            this.btnCreateInvoice.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnCreateInvoice.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCreateInvoice.ForeColor = System.Drawing.Color.White;
            this.btnCreateInvoice.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCreateInvoice.Location = new System.Drawing.Point(722, 426);
            this.btnCreateInvoice.Margin = new System.Windows.Forms.Padding(0);
            this.btnCreateInvoice.Name = "btnCreateInvoice";
            this.btnCreateInvoice.Size = new System.Drawing.Size(131, 43);
            this.btnCreateInvoice.TabIndex = 86;
            this.btnCreateInvoice.Text = "CREATE INVOICE";
            this.btnCreateInvoice.UseVisualStyleBackColor = false;
            this.btnCreateInvoice.Click += new System.EventHandler(this.btnCreateInvoice_Click);
            // 
            // frmAttendance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 529);
            this.Controls.Add(this.btnCreateInvoice);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtScan);
            this.Controls.Add(this.lblTime);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAttendance";
            this.Text = "Attendance";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAttendance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgActive)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCheckOut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgActive;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.TextBox txtScan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblCardNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTotalActive;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Timer timerClock;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer timerData;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.DataGridView dgCheckOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOCardNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOMemberName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcODuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOEndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcMembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcMembershipName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcDuration;
        private System.Windows.Forms.Button btnCreateInvoice;
    }
}