namespace PowerInventory
{
    partial class frmConfirmStockTakeDateTime
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
            this.dtpInventoryDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtHH = new System.Windows.Forms.TextBox();
            this.txtMM = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dtpInventoryDate
            // 
            this.dtpInventoryDate.CustomFormat = "dd MMMM yyyy";
            this.dtpInventoryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInventoryDate.Location = new System.Drawing.Point(141, 82);
            this.dtpInventoryDate.Margin = new System.Windows.Forms.Padding(8);
            this.dtpInventoryDate.Name = "dtpInventoryDate";
            this.dtpInventoryDate.Size = new System.Drawing.Size(328, 40);
            this.dtpInventoryDate.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(539, 33);
            this.label1.TabIndex = 5;
            this.label1.Text = "Please Confirm Stock Take Date && Time";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(469, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 251);
            this.label2.TabIndex = 6;
            this.label2.Text = "WARNING: Setting wrong DATE && TIME can cause your inventory balance to be incorr" +
                "ect";
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = global::PowerInventory.Properties.Resources.blueButton;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(37, 236);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(192, 54);
            this.btnSave.TabIndex = 102;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtHH
            // 
            this.txtHH.Location = new System.Drawing.Point(141, 144);
            this.txtHH.Name = "txtHH";
            this.txtHH.Size = new System.Drawing.Size(100, 40);
            this.txtHH.TabIndex = 103;
            // 
            // txtMM
            // 
            this.txtMM.Location = new System.Drawing.Point(267, 144);
            this.txtMM.Name = "txtMM";
            this.txtMM.Size = new System.Drawing.Size(100, 40);
            this.txtMM.TabIndex = 104;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 33);
            this.label3.TabIndex = 105;
            this.label3.Text = "TIME";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 33);
            this.label4.TabIndex = 106;
            this.label4.Text = "DATE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(241, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 33);
            this.label5.TabIndex = 107;
            this.label5.Text = ":";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(139, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(215, 33);
            this.label6.TabIndex = 108;
            this.label6.Text = "*24 hour format";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(267, 236);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(195, 54);
            this.btnCancel.TabIndex = 109;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmConfirmStockTakeDateTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 321);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMM);
            this.Controls.Add(this.txtHH);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpInventoryDate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "frmConfirmStockTakeDateTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmConfirmStockTakeDateTime_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DateTimePicker dtpInventoryDate;
        private System.Windows.Forms.TextBox txtHH;
        private System.Windows.Forms.TextBox txtMM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        protected System.Windows.Forms.Button btnCancel;

    }
}