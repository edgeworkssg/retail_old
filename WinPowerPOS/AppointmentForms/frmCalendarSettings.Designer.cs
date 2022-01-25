namespace WinPowerPOS.AppointmentForms
{
    partial class frmCalendarSettings
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
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCalID = new System.Windows.Forms.Label();
            this.Tabel = new System.Windows.Forms.DataGridView();
            this.dgvcSelect = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvcCalendarID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcCalendarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisplayName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayName.Location = new System.Drawing.Point(12, 9);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(522, 27);
            this.lblDisplayName.TabIndex = 0;
            this.lblDisplayName.Text = "<User Displayed Name>";
            this.lblDisplayName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Calendar ID :";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.Tabel);
            this.panel1.Controls.Add(this.lblCalID);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(12, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(522, 291);
            this.panel1.TabIndex = 2;
            // 
            // lblCalID
            // 
            this.lblCalID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCalID.AutoEllipsis = true;
            this.lblCalID.ForeColor = System.Drawing.Color.White;
            this.lblCalID.Location = new System.Drawing.Point(104, 13);
            this.lblCalID.Name = "lblCalID";
            this.lblCalID.Size = new System.Drawing.Size(234, 16);
            this.lblCalID.TabIndex = 2;
            this.lblCalID.Text = "GivenByGoogle@Gmail.com";
            // 
            // Tabel
            // 
            this.Tabel.AllowUserToAddRows = false;
            this.Tabel.AllowUserToDeleteRows = false;
            this.Tabel.AllowUserToResizeColumns = false;
            this.Tabel.AllowUserToResizeRows = false;
            this.Tabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabel.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.Tabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Tabel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tabel.ColumnHeadersVisible = false;
            this.Tabel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcSelect,
            this.dgvcCalendarID,
            this.dgvcCalendarName});
            this.Tabel.Location = new System.Drawing.Point(16, 46);
            this.Tabel.MultiSelect = false;
            this.Tabel.Name = "Tabel";
            this.Tabel.ReadOnly = true;
            this.Tabel.RowHeadersVisible = false;
            this.Tabel.Size = new System.Drawing.Size(484, 223);
            this.Tabel.TabIndex = 3;
            this.Tabel.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabel_CellContentClick);
            // 
            // dgvcSelect
            // 
            this.dgvcSelect.HeaderText = "";
            this.dgvcSelect.Name = "dgvcSelect";
            this.dgvcSelect.ReadOnly = true;
            this.dgvcSelect.Text = "Select";
            this.dgvcSelect.UseColumnTextForButtonValue = true;
            this.dgvcSelect.Width = 75;
            // 
            // dgvcCalendarID
            // 
            this.dgvcCalendarID.DataPropertyName = "Value";
            this.dgvcCalendarID.HeaderText = "ID";
            this.dgvcCalendarID.Name = "dgvcCalendarID";
            this.dgvcCalendarID.ReadOnly = true;
            this.dgvcCalendarID.Visible = false;
            // 
            // dgvcCalendarName
            // 
            this.dgvcCalendarName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcCalendarName.DataPropertyName = "Displayed";
            this.dgvcCalendarName.HeaderText = "Calendar";
            this.dgvcCalendarName.Name = "dgvcCalendarName";
            this.dgvcCalendarName.ReadOnly = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(344, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 34);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(425, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 34);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmCalendarSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.ClientSize = new System.Drawing.Size(546, 342);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblDisplayName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimizeBox = false;
            this.Name = "frmCalendarSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "x";
            this.Load += new System.EventHandler(this.frmCalendarSettings_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCalID;
        private System.Windows.Forms.DataGridView Tabel;
        private System.Windows.Forms.DataGridViewButtonColumn dgvcSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcCalendarID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcCalendarName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}