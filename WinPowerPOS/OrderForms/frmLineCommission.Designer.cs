namespace WinPowerPOS.OrderForms
{
    partial class frmLineCommission
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbSalesPerson = new System.Windows.Forms.ComboBox();
            this.txtLineRemark = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLineInfo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOldReceiptNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSalesPerson2 = new System.Windows.Forms.ComboBox();
            this.lblSalesPerson2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(155, 428);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(167, 59);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(334, 428);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(169, 59);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(22, 17);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 13);
            this.label10.TabIndex = 46;
            this.label10.Text = "SALES PERSON";
            // 
            // cmbSalesPerson
            // 
            this.cmbSalesPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSalesPerson.FormattingEnabled = true;
            this.cmbSalesPerson.Location = new System.Drawing.Point(25, 36);
            this.cmbSalesPerson.Margin = new System.Windows.Forms.Padding(6);
            this.cmbSalesPerson.Name = "cmbSalesPerson";
            this.cmbSalesPerson.Size = new System.Drawing.Size(479, 32);
            this.cmbSalesPerson.TabIndex = 0;
            this.cmbSalesPerson.SelectedIndexChanged += new System.EventHandler(this.cmbSalesPerson_SelectedIndexChanged);
            // 
            // txtLineRemark
            // 
            this.txtLineRemark.Location = new System.Drawing.Point(26, 214);
            this.txtLineRemark.Margin = new System.Windows.Forms.Padding(6);
            this.txtLineRemark.MaxLength = 32000;
            this.txtLineRemark.Multiline = true;
            this.txtLineRemark.Name = "txtLineRemark";
            this.txtLineRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLineRemark.Size = new System.Drawing.Size(479, 124);
            this.txtLineRemark.TabIndex = 2;
            this.txtLineRemark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLineRemark_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(23, 195);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "REMARK";
            // 
            // cmbLineInfo
            // 
            this.cmbLineInfo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLineInfo.FormattingEnabled = true;
            this.cmbLineInfo.Location = new System.Drawing.Point(25, 150);
            this.cmbLineInfo.Margin = new System.Windows.Forms.Padding(6);
            this.cmbLineInfo.Name = "cmbLineInfo";
            this.cmbLineInfo.Size = new System.Drawing.Size(479, 32);
            this.cmbLineInfo.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(22, 131);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 52;
            this.label2.Text = "LINE INFO";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOldReceiptNo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(26, 347);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 74);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SALES RETURN";
            // 
            // txtOldReceiptNo
            // 
            this.txtOldReceiptNo.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.txtOldReceiptNo.Location = new System.Drawing.Point(144, 28);
            this.txtOldReceiptNo.Name = "txtOldReceiptNo";
            this.txtOldReceiptNo.Size = new System.Drawing.Size(259, 22);
            this.txtOldReceiptNo.TabIndex = 3;
            this.txtOldReceiptNo.Leave += new System.EventHandler(this.txtOldReceiptNo_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(17, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "Receipt Number :";
            // 
            // cmbSalesPerson2
            // 
            this.cmbSalesPerson2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSalesPerson2.FormattingEnabled = true;
            this.cmbSalesPerson2.Location = new System.Drawing.Point(25, 93);
            this.cmbSalesPerson2.Margin = new System.Windows.Forms.Padding(6);
            this.cmbSalesPerson2.Name = "cmbSalesPerson2";
            this.cmbSalesPerson2.Size = new System.Drawing.Size(479, 32);
            this.cmbSalesPerson2.TabIndex = 54;
            // 
            // lblSalesPerson2
            // 
            this.lblSalesPerson2.AutoSize = true;
            this.lblSalesPerson2.BackColor = System.Drawing.Color.Transparent;
            this.lblSalesPerson2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblSalesPerson2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSalesPerson2.Location = new System.Drawing.Point(22, 74);
            this.lblSalesPerson2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSalesPerson2.Name = "lblSalesPerson2";
            this.lblSalesPerson2.Size = new System.Drawing.Size(130, 13);
            this.lblSalesPerson2.TabIndex = 55;
            this.lblSalesPerson2.Text = "2ND SALES PERSON";
            // 
            // frmLineCommission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 498);
            this.Controls.Add(this.lblSalesPerson2);
            this.Controls.Add(this.cmbSalesPerson2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbLineInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLineRemark);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbSalesPerson);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLineCommission";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Line Commission";
            this.Load += new System.EventHandler(this.frmLineCommission_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbSalesPerson;
        private System.Windows.Forms.TextBox txtLineRemark;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbLineInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtOldReceiptNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSalesPerson2;
        private System.Windows.Forms.Label lblSalesPerson2;
    }
}