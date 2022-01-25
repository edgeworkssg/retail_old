namespace PowerPOS
{
    partial class frmAddAppointment
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
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.cboSalesPersons = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboStart1 = new System.Windows.Forms.ComboBox();
            this.cboEnd1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboStart2 = new System.Windows.Forms.ComboBox();
            this.cboEnd2 = new System.Windows.Forms.ComboBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(128, 199);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Date";
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(94, 20);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(190, 21);
            this.dtpDate.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sales Person";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(94, 142);
            this.txtDesc.MaxLength = 500;
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDesc.Size = new System.Drawing.Size(190, 50);
            this.txtDesc.TabIndex = 4;
            // 
            // cboSalesPersons
            // 
            this.cboSalesPersons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSalesPersons.FormattingEnabled = true;
            this.cboSalesPersons.Location = new System.Drawing.Point(94, 52);
            this.cboSalesPersons.Name = "cboSalesPersons";
            this.cboSalesPersons.Size = new System.Drawing.Size(190, 21);
            this.cboSalesPersons.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Description";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Start Time";
            // 
            // cboStart1
            // 
            this.cboStart1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStart1.FormattingEnabled = true;
            this.cboStart1.Items.AddRange(new object[] {
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cboStart1.Location = new System.Drawing.Point(94, 80);
            this.cboStart1.Name = "cboStart1";
            this.cboStart1.Size = new System.Drawing.Size(66, 21);
            this.cboStart1.TabIndex = 8;
            // 
            // cboEnd1
            // 
            this.cboEnd1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnd1.FormattingEnabled = true;
            this.cboEnd1.Items.AddRange(new object[] {
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cboEnd1.Location = new System.Drawing.Point(94, 111);
            this.cboEnd1.Name = "cboEnd1";
            this.cboEnd1.Size = new System.Drawing.Size(64, 21);
            this.cboEnd1.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "End Time";
            // 
            // cboStart2
            // 
            this.cboStart2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStart2.FormattingEnabled = true;
            this.cboStart2.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.cboStart2.Location = new System.Drawing.Point(166, 79);
            this.cboStart2.Name = "cboStart2";
            this.cboStart2.Size = new System.Drawing.Size(64, 21);
            this.cboStart2.TabIndex = 11;
            // 
            // cboEnd2
            // 
            this.cboEnd2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnd2.FormattingEnabled = true;
            this.cboEnd2.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.cboEnd2.Location = new System.Drawing.Point(164, 111);
            this.cboEnd2.Name = "cboEnd2";
            this.cboEnd2.Size = new System.Drawing.Size(64, 21);
            this.cboEnd2.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(209, 199);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 13;
            this.button1.Text = "CLOSE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 241);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cboEnd2);
            this.Controls.Add(this.cboStart2);
            this.Controls.Add(this.cboEnd1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboStart1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboSalesPersons);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimizeBox = false;
            this.Name = "frmAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Appointment";
            this.Load += new System.EventHandler(this.frmAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.ComboBox cboSalesPersons;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboStart1;
        private System.Windows.Forms.ComboBox cboEnd1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboStart2;
        private System.Windows.Forms.ComboBox cboEnd2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1;
    }
}