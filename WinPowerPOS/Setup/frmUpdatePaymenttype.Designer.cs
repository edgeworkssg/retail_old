namespace WinPowerPOS.OrderForms
{
    partial class frmUpdatePaymenttype
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
            this.label10 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvPaymenttype = new System.Windows.Forms.DataGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnType = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaymenttype)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(15, 12);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "Payment Type";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(321, 305);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 38);
            this.btnSave.TabIndex = 43;
            this.btnSave.Text = "&SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(414, 305);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 38);
            this.btnCancel.TabIndex = 44;
            this.btnCancel.Text = "&CLOSE";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvPaymenttype
            // 
            this.dgvPaymenttype.AllowUserToAddRows = false;
            this.dgvPaymenttype.AllowUserToDeleteRows = false;
            this.dgvPaymenttype.AllowUserToResizeRows = false;
            this.dgvPaymenttype.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvPaymenttype.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPaymenttype.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPaymenttype.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPaymenttype.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPaymenttype.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvPaymenttype.Location = new System.Drawing.Point(8, 21);
            this.dgvPaymenttype.Name = "dgvPaymenttype";
            this.dgvPaymenttype.RowHeadersVisible = false;
            this.dgvPaymenttype.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPaymenttype.Size = new System.Drawing.Size(479, 228);
            this.dgvPaymenttype.TabIndex = 45;
            this.dgvPaymenttype.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPaymenttype_RowEnter);
            this.dgvPaymenttype.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPaymenttype_RowLeave);
            this.dgvPaymenttype.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPaymenttype_CellMouseEnter);
            this.dgvPaymenttype.SelectionChanged += new System.EventHandler(this.dgvPaymenttype_SelectionChanged);
            this.dgvPaymenttype.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPaymenttype_CellContentClick);
            // 
            // colID
            // 
            this.colID.DataPropertyName = "ID";
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Width = 200;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colName.Width = 275;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dgvPaymenttype);
            this.groupBox1.Location = new System.Drawing.Point(12, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 266);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDelete.Location = new System.Drawing.Point(12, 305);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(91, 38);
            this.btnDelete.TabIndex = 47;
            this.btnDelete.Text = "&DELETE";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnType
            // 
            this.btnType.BackColor = System.Drawing.Color.Transparent;
            this.btnType.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnType.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnType.ForeColor = System.Drawing.Color.White;
            this.btnType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnType.Location = new System.Drawing.Point(105, 305);
            this.btnType.Margin = new System.Windows.Forms.Padding(6);
            this.btnType.Name = "btnType";
            this.btnType.Size = new System.Drawing.Size(204, 38);
            this.btnType.TabIndex = 50;
            this.btnType.Text = "&ADD NEW PAYMENT TYPE";
            this.btnType.UseVisualStyleBackColor = false;
            this.btnType.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmUpdatePaymenttype
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(517, 354);
            this.Controls.Add(this.btnType);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label10);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmUpdatePaymenttype";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Payment Type";
            this.Load += new System.EventHandler(this.frmUpdatePaymenttype_Load_1);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUpdatePaymenttype_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaymenttype)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvPaymenttype;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        internal System.Windows.Forms.Button btnType;
    }
}