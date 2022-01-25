namespace WinUtility
{
    partial class frmCostPriceUpdater
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboDatabaseNames = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtItemNo = new System.Windows.Forms.TextBox();
            this.txtCostPrice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtChangeTo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCommit = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database Name";
            // 
            // cboDatabaseNames
            // 
            this.cboDatabaseNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabaseNames.FormattingEnabled = true;
            this.cboDatabaseNames.Location = new System.Drawing.Point(132, 33);
            this.cboDatabaseNames.Name = "cboDatabaseNames";
            this.cboDatabaseNames.Size = new System.Drawing.Size(201, 21);
            this.cboDatabaseNames.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Item No.";
            // 
            // txtItemNo
            // 
            this.txtItemNo.Location = new System.Drawing.Point(152, 84);
            this.txtItemNo.Name = "txtItemNo";
            this.txtItemNo.Size = new System.Drawing.Size(169, 20);
            this.txtItemNo.TabIndex = 3;
            this.txtItemNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItemNo_KeyDown);
            this.txtItemNo.Leave += new System.EventHandler(this.txtItemNo_Leave);
            // 
            // txtCostPrice
            // 
            this.txtCostPrice.Location = new System.Drawing.Point(152, 110);
            this.txtCostPrice.Name = "txtCostPrice";
            this.txtCostPrice.ReadOnly = true;
            this.txtCostPrice.Size = new System.Drawing.Size(169, 20);
            this.txtCostPrice.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(83, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Cost Price";
            // 
            // txtChangeTo
            // 
            this.txtChangeTo.Location = new System.Drawing.Point(152, 136);
            this.txtChangeTo.Name = "txtChangeTo";
            this.txtChangeTo.Size = new System.Drawing.Size(169, 20);
            this.txtChangeTo.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(83, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Change To";
            // 
            // btnCommit
            // 
            this.btnCommit.Location = new System.Drawing.Point(89, 212);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(106, 42);
            this.btnCommit.TabIndex = 8;
            this.btnCommit.Text = "COMMIT";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(218, 212);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(106, 42);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmCostPriceUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 286);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.txtChangeTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCostPrice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtItemNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboDatabaseNames);
            this.Controls.Add(this.label1);
            this.Name = "frmCostPriceUpdater";
            this.Text = "Inventory Cost Price Updater";
            this.Load += new System.EventHandler(this.frmCostPriceUpdater_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDatabaseNames;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtItemNo;
        private System.Windows.Forms.TextBox txtCostPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtChangeTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Button btnClear;

    }
}