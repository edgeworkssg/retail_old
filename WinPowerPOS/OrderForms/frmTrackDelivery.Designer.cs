namespace WinPowerPOS.OrderForms
{
    partial class frmTrackDelivery
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtStoreReference = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.txtDeliveryMode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkTaken = new System.Windows.Forms.CheckBox();
            this.chkAdvise = new System.Windows.Forms.CheckBox();
            this.CmbSHift = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Delivery Address";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(158, 193);
            this.txtAddress.MaxLength = 200;
            this.txtAddress.MinimumSize = new System.Drawing.Size(100, 80);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(265, 120);
            this.txtAddress.TabIndex = 6;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(256, 374);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(167, 59);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnDelivery_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(75, 374);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(169, 59);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 0;
            this.toolTip1.ToolTipTitle = "Date Blocking";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 346);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Store Reference";
            this.label2.Visible = false;
            // 
            // txtStoreReference
            // 
            this.txtStoreReference.Location = new System.Drawing.Point(158, 345);
            this.txtStoreReference.MaxLength = 50;
            this.txtStoreReference.Name = "txtStoreReference";
            this.txtStoreReference.Size = new System.Drawing.Size(228, 20);
            this.txtStoreReference.TabIndex = 8;
            this.txtStoreReference.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Delivery Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(158, 107);
            this.txtRemarks.MaxLength = 200;
            this.txtRemarks.MinimumSize = new System.Drawing.Size(100, 80);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(265, 80);
            this.txtRemarks.TabIndex = 5;
            // 
            // txtDeliveryMode
            // 
            this.txtDeliveryMode.Location = new System.Drawing.Point(158, 319);
            this.txtDeliveryMode.MaxLength = 50;
            this.txtDeliveryMode.Name = "txtDeliveryMode";
            this.txtDeliveryMode.Size = new System.Drawing.Size(265, 20);
            this.txtDeliveryMode.TabIndex = 7;
            this.txtDeliveryMode.Text = " ";
            this.txtDeliveryMode.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 320);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Mode Of Delivery";
            this.label4.Visible = false;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(158, 7);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(265, 21);
            this.cmbType.TabIndex = 0;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.label5.Location = new System.Drawing.Point(12, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Type";
            // 
            // chkTaken
            // 
            this.chkTaken.AutoSize = true;
            this.chkTaken.Enabled = false;
            this.chkTaken.Location = new System.Drawing.Point(222, 57);
            this.chkTaken.Name = "chkTaken";
            this.chkTaken.Size = new System.Drawing.Size(57, 17);
            this.chkTaken.TabIndex = 3;
            this.chkTaken.Text = "Taken";
            this.chkTaken.UseVisualStyleBackColor = true;
            // 
            // chkAdvise
            // 
            this.chkAdvise.AutoSize = true;
            this.chkAdvise.Enabled = false;
            this.chkAdvise.Location = new System.Drawing.Point(158, 57);
            this.chkAdvise.Name = "chkAdvise";
            this.chkAdvise.Size = new System.Drawing.Size(58, 17);
            this.chkAdvise.TabIndex = 2;
            this.chkAdvise.Text = "Advise";
            this.chkAdvise.UseVisualStyleBackColor = true;
            // 
            // CmbSHift
            // 
            this.CmbSHift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbSHift.FormattingEnabled = true;
            this.CmbSHift.Location = new System.Drawing.Point(158, 80);
            this.CmbSHift.Name = "CmbSHift";
            this.CmbSHift.Size = new System.Drawing.Size(265, 21);
            this.CmbSHift.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.label6.Location = new System.Drawing.Point(12, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 16);
            this.label6.TabIndex = 20;
            this.label6.Text = "Shift";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "Delivery Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd MMM yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(158, 34);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(265, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // frmTrackDelivery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 446);
            this.Controls.Add(this.chkTaken);
            this.Controls.Add(this.chkAdvise);
            this.Controls.Add(this.CmbSHift);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDeliveryMode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStoreReference);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmTrackDelivery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delivery";
            this.Load += new System.EventHandler(this.frmTrackDelivery_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddress;
        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStoreReference;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.TextBox txtDeliveryMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkTaken;
        private System.Windows.Forms.CheckBox chkAdvise;
        private System.Windows.Forms.ComboBox CmbSHift;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}