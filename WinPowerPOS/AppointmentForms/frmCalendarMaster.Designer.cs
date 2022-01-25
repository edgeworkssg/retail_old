namespace WinPowerPOS.AppointmentForms
{
    partial class frmCalendarMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCalendarMaster));
            this.myCalendar = new System.Windows.Forms.MonthCalendar();
            this.Tabel = new System.Windows.Forms.DataGridView();
            this.rbWeekly = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbUser = new System.Windows.Forms.RadioButton();
            this.detWeekly = new System.Windows.Forms.Panel();
            this.tSalesPerson = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSalesPerson = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.detWeekly.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // myCalendar
            // 
            this.myCalendar.Location = new System.Drawing.Point(12, 9);
            this.myCalendar.MaxSelectionCount = 1;
            this.myCalendar.Name = "myCalendar";
            this.myCalendar.ShowTodayCircle = false;
            this.myCalendar.TabIndex = 9;
            // 
            // Tabel
            // 
            this.Tabel.AllowUserToAddRows = false;
            this.Tabel.AllowUserToDeleteRows = false;
            this.Tabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabel.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.Tabel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.Tabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Tabel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tabel.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.Tabel.Location = new System.Drawing.Point(12, 33);
            this.Tabel.MultiSelect = false;
            this.Tabel.Name = "Tabel";
            this.Tabel.ReadOnly = true;
            this.Tabel.RowHeadersVisible = false;
            this.Tabel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Tabel.Size = new System.Drawing.Size(817, 362);
            this.Tabel.TabIndex = 10;
            this.Tabel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellClick);
            // 
            // rbWeekly
            // 
            this.rbWeekly.AutoSize = true;
            this.rbWeekly.Checked = true;
            this.rbWeekly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbWeekly.ForeColor = System.Drawing.Color.White;
            this.rbWeekly.Location = new System.Drawing.Point(6, 21);
            this.rbWeekly.Name = "rbWeekly";
            this.rbWeekly.Size = new System.Drawing.Size(154, 20);
            this.rbWeekly.TabIndex = 11;
            this.rbWeekly.TabStop = true;
            this.rbWeekly.Text = "Show all days weekly";
            this.rbWeekly.UseVisualStyleBackColor = true;
            this.rbWeekly.CheckedChanged += new System.EventHandler(this.rbWeekly_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbUser);
            this.groupBox1.Controls.Add(this.detWeekly);
            this.groupBox1.Controls.Add(this.rbWeekly);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.GreenYellow;
            this.groupBox1.Location = new System.Drawing.Point(251, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 136);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View Mode";
            // 
            // rbUser
            // 
            this.rbUser.AutoSize = true;
            this.rbUser.Enabled = false;
            this.rbUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbUser.ForeColor = System.Drawing.Color.White;
            this.rbUser.Location = new System.Drawing.Point(6, 82);
            this.rbUser.Name = "rbUser";
            this.rbUser.Size = new System.Drawing.Size(160, 20);
            this.rbUser.TabIndex = 13;
            this.rbUser.Text = "Show all Sales Person";
            this.rbUser.UseVisualStyleBackColor = true;
            this.rbUser.Visible = false;
            // 
            // detWeekly
            // 
            this.detWeekly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.detWeekly.Controls.Add(this.tSalesPerson);
            this.detWeekly.Controls.Add(this.label4);
            this.detWeekly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detWeekly.ForeColor = System.Drawing.Color.White;
            this.detWeekly.Location = new System.Drawing.Point(20, 42);
            this.detWeekly.Name = "detWeekly";
            this.detWeekly.Size = new System.Drawing.Size(270, 34);
            this.detWeekly.TabIndex = 12;
            // 
            // tSalesPerson
            // 
            this.tSalesPerson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tSalesPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tSalesPerson.FormattingEnabled = true;
            this.tSalesPerson.Location = new System.Drawing.Point(98, 3);
            this.tSalesPerson.Name = "tSalesPerson";
            this.tSalesPerson.Size = new System.Drawing.Size(167, 24);
            this.tSalesPerson.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Sales Person";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.lblSalesPerson);
            this.panel3.Controls.Add(this.Tabel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 180);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(841, 407);
            this.panel3.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(9, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Sales Person: ";
            // 
            // lblSalesPerson
            // 
            this.lblSalesPerson.AutoSize = true;
            this.lblSalesPerson.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesPerson.ForeColor = System.Drawing.Color.White;
            this.lblSalesPerson.Location = new System.Drawing.Point(117, 13);
            this.lblSalesPerson.Name = "lblSalesPerson";
            this.lblSalesPerson.Size = new System.Drawing.Size(13, 16);
            this.lblSalesPerson.TabIndex = 11;
            this.lblSalesPerson.Text = "-";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnRefresh);
            this.panel4.Controls.Add(this.myCalendar);
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(841, 180);
            this.panel4.TabIndex = 15;
            // 
            // btnRefresh
            // 
            this.btnRefresh.ForeColor = System.Drawing.Color.Black;
            this.btnRefresh.Location = new System.Drawing.Point(469, 151);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmCalendarMaster
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(841, 587);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmCalendarMaster";
            this.Text = "Google Calendar Demo Application";
            this.Load += new System.EventHandler(this.frmCalendarTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.detWeekly.ResumeLayout(false);
            this.detWeekly.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MonthCalendar myCalendar;
        private System.Windows.Forms.DataGridView Tabel;
        private System.Windows.Forms.RadioButton rbWeekly;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel detWeekly;
        private System.Windows.Forms.RadioButton rbUser;
        private System.Windows.Forms.ComboBox tSalesPerson;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblSalesPerson;
        private System.Windows.Forms.Label label5;


    }
}