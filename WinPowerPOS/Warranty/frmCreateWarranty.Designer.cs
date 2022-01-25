namespace WinPowerPOS.WarrantyForms
{
    partial class frmCreateWarranty
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpDateOfExpiry = new System.Windows.Forms.DateTimePicker();
            this.dtpDateOfPurchase = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.dgvWarranty = new System.Windows.Forms.DataGridView();
            this.SerialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductIdentificationNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateOfPurchase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateOfExpiry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtProductIdentificationNo = new System.Windows.Forms.TextBox();
            this.txtModelNo = new System.Windows.Forms.TextBox();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarranty)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dtpDateOfExpiry);
            this.panel1.Controls.Add(this.dtpDateOfPurchase);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtItemName);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.dgvWarranty);
            this.panel1.Controls.Add(this.txtProductIdentificationNo);
            this.panel1.Controls.Add(this.txtModelNo);
            this.panel1.Controls.Add(this.txtSerialNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(668, 460);
            this.panel1.TabIndex = 1;
            // 
            // dtpDateOfExpiry
            // 
            this.dtpDateOfExpiry.Location = new System.Drawing.Point(205, 170);
            this.dtpDateOfExpiry.Name = "dtpDateOfExpiry";
            this.dtpDateOfExpiry.Size = new System.Drawing.Size(341, 20);
            this.dtpDateOfExpiry.TabIndex = 24;
            // 
            // dtpDateOfPurchase
            // 
            this.dtpDateOfPurchase.Location = new System.Drawing.Point(205, 139);
            this.dtpDateOfPurchase.Name = "dtpDateOfPurchase";
            this.dtpDateOfPurchase.Size = new System.Drawing.Size(341, 20);
            this.dtpDateOfPurchase.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 20);
            this.label6.TabIndex = 22;
            this.label6.Text = "Date of Expiry";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Date of Purchase";
            // 
            // txtItemName
            // 
            this.txtItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName.Location = new System.Drawing.Point(204, 10);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.ReadOnly = true;
            this.txtItemName.Size = new System.Drawing.Size(341, 26);
            this.txtItemName.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 20);
            this.label5.TabIndex = 19;
            this.label5.Text = "Item";
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(569, 153);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 37);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "SAVE";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dgvWarranty
            // 
            this.dgvWarranty.AllowUserToAddRows = false;
            this.dgvWarranty.AllowUserToDeleteRows = false;
            this.dgvWarranty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWarranty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWarranty.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SerialNo,
            this.ItemNo,
            this.ModelNo,
            this.ProductIdentificationNo,
            this.DateOfPurchase,
            this.DateOfExpiry});
            this.dgvWarranty.Location = new System.Drawing.Point(3, 205);
            this.dgvWarranty.MultiSelect = false;
            this.dgvWarranty.Name = "dgvWarranty";
            this.dgvWarranty.ReadOnly = true;
            this.dgvWarranty.RowHeadersVisible = false;
            this.dgvWarranty.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWarranty.Size = new System.Drawing.Size(665, 252);
            this.dgvWarranty.TabIndex = 17;
            this.dgvWarranty.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWarranty_CellClick);
            // 
            // SerialNo
            // 
            this.SerialNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SerialNo.DataPropertyName = "SerialNo";
            this.SerialNo.HeaderText = "Serial No.";
            this.SerialNo.MinimumWidth = 100;
            this.SerialNo.Name = "SerialNo";
            this.SerialNo.ReadOnly = true;
            // 
            // ItemNo
            // 
            this.ItemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemNo.DataPropertyName = "ItemNo";
            this.ItemNo.HeaderText = "Item No";
            this.ItemNo.MinimumWidth = 100;
            this.ItemNo.Name = "ItemNo";
            this.ItemNo.ReadOnly = true;
            // 
            // ModelNo
            // 
            this.ModelNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ModelNo.DataPropertyName = "ModelNo";
            this.ModelNo.HeaderText = "Model No.";
            this.ModelNo.MinimumWidth = 100;
            this.ModelNo.Name = "ModelNo";
            this.ModelNo.ReadOnly = true;
            // 
            // ProductIdentificationNo
            // 
            this.ProductIdentificationNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ProductIdentificationNo.DataPropertyName = "ProductIdentification";
            this.ProductIdentificationNo.HeaderText = "Prod ID";
            this.ProductIdentificationNo.MinimumWidth = 100;
            this.ProductIdentificationNo.Name = "ProductIdentificationNo";
            this.ProductIdentificationNo.ReadOnly = true;
            // 
            // DateOfPurchase
            // 
            this.DateOfPurchase.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DateOfPurchase.DataPropertyName = "DateOfPurchase";
            dataGridViewCellStyle1.Format = "dd MMM yyyy";
            this.DateOfPurchase.DefaultCellStyle = dataGridViewCellStyle1;
            this.DateOfPurchase.HeaderText = "Date Of Purchase";
            this.DateOfPurchase.MinimumWidth = 100;
            this.DateOfPurchase.Name = "DateOfPurchase";
            this.DateOfPurchase.ReadOnly = true;
            // 
            // DateOfExpiry
            // 
            this.DateOfExpiry.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DateOfExpiry.DataPropertyName = "DateExpiry";
            dataGridViewCellStyle2.Format = "dd MMM yyyy";
            this.DateOfExpiry.DefaultCellStyle = dataGridViewCellStyle2;
            this.DateOfExpiry.HeaderText = "Date Of Expiry";
            this.DateOfExpiry.MinimumWidth = 100;
            this.DateOfExpiry.Name = "DateOfExpiry";
            this.DateOfExpiry.ReadOnly = true;
            // 
            // txtProductIdentificationNo
            // 
            this.txtProductIdentificationNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductIdentificationNo.Location = new System.Drawing.Point(205, 102);
            this.txtProductIdentificationNo.Name = "txtProductIdentificationNo";
            this.txtProductIdentificationNo.Size = new System.Drawing.Size(341, 26);
            this.txtProductIdentificationNo.TabIndex = 15;
            // 
            // txtModelNo
            // 
            this.txtModelNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModelNo.Location = new System.Drawing.Point(205, 70);
            this.txtModelNo.Name = "txtModelNo";
            this.txtModelNo.Size = new System.Drawing.Size(341, 26);
            this.txtModelNo.TabIndex = 14;
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerialNo.Location = new System.Drawing.Point(205, 38);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(341, 26);
            this.txtSerialNo.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Product Identification No.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Model No.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Serial No.";
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.Location = new System.Drawing.Point(570, 464);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(102, 47);
            this.btnDone.TabIndex = 19;
            this.btnDone.Text = "CONFIRM";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // frmCreateWarranty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 517);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.panel1);
            this.Name = "frmCreateWarranty";
            this.Text = "Create Warranty";
            this.Load += new System.EventHandler(this.frmCreateWarranty_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarranty)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView dgvWarranty;
        private System.Windows.Forms.TextBox txtProductIdentificationNo;
        private System.Windows.Forms.TextBox txtModelNo;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.DateTimePicker dtpDateOfExpiry;
        private System.Windows.Forms.DateTimePicker dtpDateOfPurchase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductIdentificationNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateOfPurchase;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateOfExpiry;
    }
}