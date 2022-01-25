namespace WinPowerPOS.Delivery
{
    partial class frmDeliveryList
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tRefNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Tabel = new System.Windows.Forms.DataGridView();
            this.dgvcRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcDONo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOrderDetID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcMembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcMembershipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcRecipientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Deliver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.tSearch);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tRefNo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(854, 153);
            this.panel1.TabIndex = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(156, 92);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(133, 43);
            this.btnEdit.TabIndex = 105;
            this.btnEdit.Text = "EDIT DO";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 82);
            this.groupBox1.TabIndex = 104;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Delivery Date";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(137, 19);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            this.dtpStartDate.Size = new System.Drawing.Size(246, 22);
            this.dtpStartDate.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Start Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "End Date";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(137, 47);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.Size = new System.Drawing.Size(246, 22);
            this.dtpEndDate.TabIndex = 7;
            // 
            // btnPrint
            // 
            this.btnPrint.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(15, 92);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(133, 43);
            this.btnPrint.TabIndex = 103;
            this.btnPrint.Text = "PRINT DO";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(706, 92);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(133, 43);
            this.btnSearch.TabIndex = 102;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tSearch
            // 
            this.tSearch.Location = new System.Drawing.Point(593, 50);
            this.tSearch.Name = "tSearch";
            this.tSearch.Size = new System.Drawing.Size(246, 22);
            this.tSearch.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(465, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Search";
            // 
            // tRefNo
            // 
            this.tRefNo.Location = new System.Drawing.Point(593, 22);
            this.tRefNo.Name = "tRefNo";
            this.tRefNo.Size = new System.Drawing.Size(246, 22);
            this.tRefNo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(465, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reference Number";
            // 
            // Tabel
            // 
            this.Tabel.AllowUserToAddRows = false;
            this.Tabel.AllowUserToDeleteRows = false;
            this.Tabel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tabel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcRefNo,
            this.dgvcDONo,
            this.dgvcOrderDetID,
            this.dgvcOrderDate,
            this.dgvcMembershipNo,
            this.dgvcMembershipName,
            this.dgvcRecipientName,
            this.dgvcItemNo,
            this.dgvcItemName,
            this.Deliver,
            this.dgvcQuantity});
            this.Tabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabel.Location = new System.Drawing.Point(7, 160);
            this.Tabel.Margin = new System.Windows.Forms.Padding(4);
            this.Tabel.Name = "Tabel";
            this.Tabel.ReadOnly = true;
            this.Tabel.RowHeadersVisible = false;
            this.Tabel.Size = new System.Drawing.Size(854, 262);
            this.Tabel.TabIndex = 1;
            // 
            // dgvcRefNo
            // 
            this.dgvcRefNo.DataPropertyName = "OrderNumber";
            this.dgvcRefNo.HeaderText = "OrderNumber";
            this.dgvcRefNo.Name = "dgvcRefNo";
            this.dgvcRefNo.ReadOnly = true;
            this.dgvcRefNo.Visible = false;
            this.dgvcRefNo.Width = 125;
            // 
            // dgvcDONo
            // 
            this.dgvcDONo.DataPropertyName = "CustomDONo";
            this.dgvcDONo.HeaderText = "Delivery No";
            this.dgvcDONo.Name = "dgvcDONo";
            this.dgvcDONo.ReadOnly = true;
            // 
            // dgvcOrderDetID
            // 
            this.dgvcOrderDetID.DataPropertyName = "CustomOrderRefNo";
            this.dgvcOrderDetID.HeaderText = "Sales Order Ref No";
            this.dgvcOrderDetID.Name = "dgvcOrderDetID";
            this.dgvcOrderDetID.ReadOnly = true;
            // 
            // dgvcOrderDate
            // 
            this.dgvcOrderDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcOrderDate.DataPropertyName = "DeliveryDate";
            dataGridViewCellStyle1.Format = "dd/MM/yyyy";
            dataGridViewCellStyle1.NullValue = null;
            this.dgvcOrderDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvcOrderDate.HeaderText = "Date";
            this.dgvcOrderDate.Name = "dgvcOrderDate";
            this.dgvcOrderDate.ReadOnly = true;
            this.dgvcOrderDate.Width = 62;
            // 
            // dgvcMembershipNo
            // 
            this.dgvcMembershipNo.DataPropertyName = "MembershipNo";
            this.dgvcMembershipNo.HeaderText = "Member No";
            this.dgvcMembershipNo.Name = "dgvcMembershipNo";
            this.dgvcMembershipNo.ReadOnly = true;
            this.dgvcMembershipNo.Width = 125;
            // 
            // dgvcMembershipName
            // 
            this.dgvcMembershipName.DataPropertyName = "NameToAppear";
            this.dgvcMembershipName.HeaderText = "Member Name";
            this.dgvcMembershipName.Name = "dgvcMembershipName";
            this.dgvcMembershipName.ReadOnly = true;
            this.dgvcMembershipName.Visible = false;
            this.dgvcMembershipName.Width = 175;
            // 
            // dgvcRecipientName
            // 
            this.dgvcRecipientName.DataPropertyName = "RecipientName";
            this.dgvcRecipientName.HeaderText = "Recipient Name";
            this.dgvcRecipientName.Name = "dgvcRecipientName";
            this.dgvcRecipientName.ReadOnly = true;
            // 
            // dgvcItemNo
            // 
            this.dgvcItemNo.DataPropertyName = "ItemNo";
            this.dgvcItemNo.HeaderText = "Item No";
            this.dgvcItemNo.Name = "dgvcItemNo";
            this.dgvcItemNo.ReadOnly = true;
            // 
            // dgvcItemName
            // 
            this.dgvcItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcItemName.DataPropertyName = "ItemName";
            this.dgvcItemName.HeaderText = "Item Name";
            this.dgvcItemName.MinimumWidth = 50;
            this.dgvcItemName.Name = "dgvcItemName";
            this.dgvcItemName.ReadOnly = true;
            // 
            // Deliver
            // 
            this.Deliver.DataPropertyName = "IsDelivered";
            this.Deliver.HeaderText = "Deliver";
            this.Deliver.Name = "Deliver";
            this.Deliver.ReadOnly = true;
            // 
            // dgvcQuantity
            // 
            this.dgvcQuantity.DataPropertyName = "Quantity";
            this.dgvcQuantity.HeaderText = "Quantity";
            this.dgvcQuantity.Name = "dgvcQuantity";
            this.dgvcQuantity.ReadOnly = true;
            // 
            // frmDeliveryList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(868, 429);
            this.Controls.Add(this.Tabel);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmDeliveryList";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Text = "Delivery List";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmDeliveryList_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView Tabel;
        private System.Windows.Forms.TextBox tRefNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        protected System.Windows.Forms.Button btnSearch;
        protected System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.GroupBox groupBox1;
        protected System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcRefNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcDONo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOrderDetID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcMembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcMembershipName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcRecipientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Deliver;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcQuantity;
    }
}