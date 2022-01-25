namespace WinUtility
{
    partial class frmRecreateStock
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
            this.btnBackup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCreateTempTable = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRegenerate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClearUp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(135, 12);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 0;
            this.btnBackup.Text = "GO";
            this.btnBackup.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Backup Database";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Create Temp Table";
            // 
            // btnCreateTempTable
            // 
            this.btnCreateTempTable.Location = new System.Drawing.Point(135, 41);
            this.btnCreateTempTable.Name = "btnCreateTempTable";
            this.btnCreateTempTable.Size = new System.Drawing.Size(75, 23);
            this.btnCreateTempTable.TabIndex = 2;
            this.btnCreateTempTable.Text = "GO";
            this.btnCreateTempTable.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Clear Up";
            // 
            // btnRegenerate
            // 
            this.btnRegenerate.Location = new System.Drawing.Point(135, 99);
            this.btnRegenerate.Name = "btnRegenerate";
            this.btnRegenerate.Size = new System.Drawing.Size(75, 23);
            this.btnRegenerate.TabIndex = 4;
            this.btnRegenerate.Text = "GO";
            this.btnRegenerate.UseVisualStyleBackColor = true;
            this.btnRegenerate.Click += new System.EventHandler(this.btnRegenerate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Regenerate";
            // 
            // btnClearUp
            // 
            this.btnClearUp.Location = new System.Drawing.Point(135, 70);
            this.btnClearUp.Name = "btnClearUp";
            this.btnClearUp.Size = new System.Drawing.Size(75, 23);
            this.btnClearUp.TabIndex = 6;
            this.btnClearUp.Text = "GO";
            this.btnClearUp.UseVisualStyleBackColor = true;
            // 
            // frmRecreateStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnClearUp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRegenerate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCreateTempTable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBackup);
            this.Name = "frmRecreateStock";
            this.Text = "Re-Create Stock";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCreateTempTable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRegenerate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClearUp;
    }
}