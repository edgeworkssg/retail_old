namespace WinPowerPOS.Setup
{
    partial class frmExtraChargeSetup
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tPaymentType = new System.Windows.Forms.TextBox();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.dgvcDel = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvcEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvcPaymentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcExtraChargeType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcExtraChargeAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcDel,
            this.dgvcEdit,
            this.dgvcPaymentType,
            this.dgvcExtraChargeType,
            this.dgvcExtraChargeAmount});
            this.dataGridView1.Location = new System.Drawing.Point(267, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(360, 315);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Payment Type";
            // 
            // tPaymentType
            // 
            this.tPaymentType.Location = new System.Drawing.Point(93, 12);
            this.tPaymentType.Name = "tPaymentType";
            this.tPaymentType.Size = new System.Drawing.Size(141, 20);
            this.tPaymentType.TabIndex = 2;
            this.tPaymentType.Validating += new System.ComponentModel.CancelEventHandler(this.tPaymentType_Validating);
            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Items.AddRange(new object[] {
            "Amount",
            "Percent"});
            this.cmbMode.Location = new System.Drawing.Point(93, 38);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(141, 21);
            this.cmbMode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mode";
            // 
            // tAmount
            // 
            this.tAmount.Location = new System.Drawing.Point(93, 65);
            this.tAmount.Name = "tAmount";
            this.tAmount.Size = new System.Drawing.Size(141, 20);
            this.tAmount.TabIndex = 6;
            this.tAmount.Validating += new System.ComponentModel.CancelEventHandler(this.tAmount_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Amount";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(159, 91);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 35);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "UPDATE >>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // dgvcDel
            // 
            this.dgvcDel.HeaderText = "";
            this.dgvcDel.Name = "dgvcDel";
            this.dgvcDel.ReadOnly = true;
            this.dgvcDel.Text = "X";
            this.dgvcDel.UseColumnTextForButtonValue = true;
            this.dgvcDel.Width = 40;
            // 
            // dgvcEdit
            // 
            this.dgvcEdit.HeaderText = "";
            this.dgvcEdit.Name = "dgvcEdit";
            this.dgvcEdit.ReadOnly = true;
            this.dgvcEdit.Text = "Edit";
            this.dgvcEdit.UseColumnTextForButtonValue = true;
            this.dgvcEdit.Width = 40;
            // 
            // dgvcPaymentType
            // 
            this.dgvcPaymentType.DataPropertyName = "PaymentType";
            this.dgvcPaymentType.HeaderText = "Payment Type";
            this.dgvcPaymentType.Name = "dgvcPaymentType";
            this.dgvcPaymentType.ReadOnly = true;
            this.dgvcPaymentType.Width = 127;
            // 
            // dgvcExtraChargeType
            // 
            this.dgvcExtraChargeType.DataPropertyName = "Mode";
            this.dgvcExtraChargeType.HeaderText = "Mode";
            this.dgvcExtraChargeType.Name = "dgvcExtraChargeType";
            this.dgvcExtraChargeType.ReadOnly = true;
            this.dgvcExtraChargeType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgvcExtraChargeAmount
            // 
            this.dgvcExtraChargeAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcExtraChargeAmount.DataPropertyName = "Amount";
            this.dgvcExtraChargeAmount.HeaderText = "Amount";
            this.dgvcExtraChargeAmount.Name = "dgvcExtraChargeAmount";
            this.dgvcExtraChargeAmount.ReadOnly = true;
            // 
            // frmExtraChargeSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 339);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tAmount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbMode);
            this.Controls.Add(this.tPaymentType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmExtraChargeSetup";
            this.Text = "Extra Charge Setup";
            this.Load += new System.EventHandler(this.frmExtraChargeSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tPaymentType;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.DataGridViewButtonColumn dgvcDel;
        private System.Windows.Forms.DataGridViewButtonColumn dgvcEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcPaymentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcExtraChargeType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcExtraChargeAmount;
    }
}