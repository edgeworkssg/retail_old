namespace WinPowerPOS.Reports
{
    partial class frmZ2Closing
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.Starttime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Endtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSystemRecorded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActualCollection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Variance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CounterCloseID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintBill = new System.Windows.Forms.DataGridViewButtonColumn();
            this.EJ = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnZ2Closing = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPOS = new System.Windows.Forms.Label();
            this.lblVariance = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalCollected = new System.Windows.Forms.Label();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblTotalRecorded = new System.Windows.Forms.Label();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.panel3.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(999, 690);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvList);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 88);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(979, 543);
            this.panel4.TabIndex = 32;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Starttime,
            this.Endtime,
            this.TotalSystemRecorded,
            this.ActualCollection,
            this.Variance,
            this.CounterCloseID,
            this.PrintBill,
            this.EJ});
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvList.Location = new System.Drawing.Point(0, 0);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.RowHeadersWidth = 23;
            this.dgvList.Size = new System.Drawing.Size(979, 543);
            this.dgvList.TabIndex = 30;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // Starttime
            // 
            this.Starttime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Starttime.DataPropertyName = "StartTime";
            this.Starttime.HeaderText = "Start Time";
            this.Starttime.Name = "Starttime";
            this.Starttime.ReadOnly = true;
            // 
            // Endtime
            // 
            this.Endtime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Endtime.DataPropertyName = "EndTime";
            this.Endtime.HeaderText = "End Time";
            this.Endtime.Name = "Endtime";
            this.Endtime.ReadOnly = true;
            // 
            // TotalSystemRecorded
            // 
            this.TotalSystemRecorded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TotalSystemRecorded.DataPropertyName = "TotalSystemRecorded";
            dataGridViewCellStyle4.Format = "C2";
            dataGridViewCellStyle4.NullValue = null;
            this.TotalSystemRecorded.DefaultCellStyle = dataGridViewCellStyle4;
            this.TotalSystemRecorded.HeaderText = "Total Recorded";
            this.TotalSystemRecorded.Name = "TotalSystemRecorded";
            this.TotalSystemRecorded.ReadOnly = true;
            // 
            // ActualCollection
            // 
            this.ActualCollection.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ActualCollection.DataPropertyName = "TotalActualCollected";
            dataGridViewCellStyle5.Format = "C2";
            dataGridViewCellStyle5.NullValue = null;
            this.ActualCollection.DefaultCellStyle = dataGridViewCellStyle5;
            this.ActualCollection.HeaderText = "Total Collected";
            this.ActualCollection.Name = "ActualCollection";
            this.ActualCollection.ReadOnly = true;
            // 
            // Variance
            // 
            this.Variance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Variance.DataPropertyName = "variance";
            dataGridViewCellStyle6.Format = "C2";
            dataGridViewCellStyle6.NullValue = null;
            this.Variance.DefaultCellStyle = dataGridViewCellStyle6;
            this.Variance.HeaderText = "+/-";
            this.Variance.Name = "Variance";
            this.Variance.ReadOnly = true;
            // 
            // CounterCloseID
            // 
            this.CounterCloseID.DataPropertyName = "CounterCloseID";
            this.CounterCloseID.HeaderText = "CounterCloseID";
            this.CounterCloseID.Name = "CounterCloseID";
            this.CounterCloseID.ReadOnly = true;
            this.CounterCloseID.Visible = false;
            // 
            // PrintBill
            // 
            this.PrintBill.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PrintBill.HeaderText = global::WinPowerPOS.Properties.Language.String1;
            this.PrintBill.Name = "PrintBill";
            this.PrintBill.ReadOnly = true;
            this.PrintBill.Text = "Print";
            this.PrintBill.UseColumnTextForButtonValue = true;
            this.PrintBill.Width = 80;
            // 
            // EJ
            // 
            this.EJ.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.EJ.HeaderText = global::WinPowerPOS.Properties.Language.String1;
            this.EJ.Name = "EJ";
            this.EJ.ReadOnly = true;
            this.EJ.Text = "EJ Report";
            this.EJ.UseColumnTextForButtonValue = true;
            this.EJ.Width = 80;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnZ2Closing);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 631);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(979, 49);
            this.panel3.TabIndex = 31;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(861, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 38);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnZ2Closing
            // 
            this.btnZ2Closing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZ2Closing.BackColor = System.Drawing.Color.Green;
            this.btnZ2Closing.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnZ2Closing.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZ2Closing.ForeColor = System.Drawing.Color.White;
            this.btnZ2Closing.Location = new System.Drawing.Point(750, 3);
            this.btnZ2Closing.Name = "btnZ2Closing";
            this.btnZ2Closing.Size = new System.Drawing.Size(105, 38);
            this.btnZ2Closing.TabIndex = 5;
            this.btnZ2Closing.Text = "Z2";
            this.btnZ2Closing.UseVisualStyleBackColor = false;
            this.btnZ2Closing.Click += new System.EventHandler(this.btnZ2Closing_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblPOS);
            this.panel2.Controls.Add(this.lblVariance);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.lblTotalCollected);
            this.panel2.Controls.Add(this.lblStartTime);
            this.panel2.Controls.Add(this.lblTotalRecorded);
            this.panel2.Controls.Add(this.lblEndTime);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(979, 72);
            this.panel2.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start Time";
            // 
            // lblPOS
            // 
            this.lblPOS.AutoSize = true;
            this.lblPOS.Location = new System.Drawing.Point(4, 11);
            this.lblPOS.Name = "lblPOS";
            this.lblPOS.Size = new System.Drawing.Size(10, 13);
            this.lblPOS.TabIndex = 0;
            this.lblPOS.Text = "-";
            // 
            // lblVariance
            // 
            this.lblVariance.AutoSize = true;
            this.lblVariance.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVariance.Location = new System.Drawing.Point(615, 50);
            this.lblVariance.Name = "lblVariance";
            this.lblVariance.Size = new System.Drawing.Size(16, 22);
            this.lblVariance.TabIndex = 26;
            this.lblVariance.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "End Time";
            // 
            // lblTotalCollected
            // 
            this.lblTotalCollected.AutoSize = true;
            this.lblTotalCollected.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCollected.Location = new System.Drawing.Point(615, 28);
            this.lblTotalCollected.Name = "lblTotalCollected";
            this.lblTotalCollected.Size = new System.Drawing.Size(16, 22);
            this.lblTotalCollected.TabIndex = 25;
            this.lblTotalCollected.Text = "-";
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartTime.Location = new System.Drawing.Point(101, 28);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(15, 20);
            this.lblStartTime.TabIndex = 3;
            this.lblStartTime.Text = "-";
            // 
            // lblTotalRecorded
            // 
            this.lblTotalRecorded.AutoSize = true;
            this.lblTotalRecorded.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalRecorded.Location = new System.Drawing.Point(615, 6);
            this.lblTotalRecorded.Name = "lblTotalRecorded";
            this.lblTotalRecorded.Size = new System.Drawing.Size(16, 22);
            this.lblTotalRecorded.TabIndex = 24;
            this.lblTotalRecorded.Text = "-";
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndTime.Location = new System.Drawing.Point(101, 48);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(15, 20);
            this.lblEndTime.TabIndex = 4;
            this.lblEndTime.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(459, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 22);
            this.label7.TabIndex = 23;
            this.label7.Text = "+/-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(459, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 22);
            this.label6.TabIndex = 20;
            this.label6.Text = "Total Recorded";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(459, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 22);
            this.label5.TabIndex = 21;
            this.label5.Text = "Total Collected";
            // 
            // frmZ2Closing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 690);
            this.Controls.Add(this.panel1);
            this.Name = "frmZ2Closing";
            this.Text = "Z2 Closing";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmZ2Closing_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPOS;
        private System.Windows.Forms.Label lblVariance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalCollected;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblTotalRecorded;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnZ2Closing;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Starttime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Endtime;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSystemRecorded;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActualCollection;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variance;
        private System.Windows.Forms.DataGridViewTextBoxColumn CounterCloseID;
        private System.Windows.Forms.DataGridViewButtonColumn PrintBill;
        private System.Windows.Forms.DataGridViewButtonColumn EJ;

    }
}