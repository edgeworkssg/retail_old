namespace WinPowerPOS.MaintenanceForms
{
    partial class frmMagentoSync
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
            this.lbCategory = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSyncCategory = new System.Windows.Forms.Button();
            this.btnSyncItem = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pnlWait = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlWait.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCategory
            // 
            this.lbCategory.FormattingEnabled = true;
            this.lbCategory.Location = new System.Drawing.Point(12, 38);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.Size = new System.Drawing.Size(218, 274);
            this.lbCategory.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Category";
            // 
            // btnSyncCategory
            // 
            this.btnSyncCategory.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnSyncCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncCategory.ForeColor = System.Drawing.Color.White;
            this.btnSyncCategory.Location = new System.Drawing.Point(249, 38);
            this.btnSyncCategory.Name = "btnSyncCategory";
            this.btnSyncCategory.Size = new System.Drawing.Size(112, 59);
            this.btnSyncCategory.TabIndex = 2;
            this.btnSyncCategory.Text = "Sync Category";
            this.btnSyncCategory.UseVisualStyleBackColor = true;
            this.btnSyncCategory.Click += new System.EventHandler(this.btnSyncCategory_Click);
            // 
            // btnSyncItem
            // 
            this.btnSyncItem.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnSyncItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncItem.ForeColor = System.Drawing.Color.White;
            this.btnSyncItem.Location = new System.Drawing.Point(249, 103);
            this.btnSyncItem.Name = "btnSyncItem";
            this.btnSyncItem.Size = new System.Drawing.Size(112, 62);
            this.btnSyncItem.TabIndex = 3;
            this.btnSyncItem.Text = "Sync Item";
            this.btnSyncItem.UseVisualStyleBackColor = true;
            this.btnSyncItem.Click += new System.EventHandler(this.btnSyncItem_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsg.Location = new System.Drawing.Point(416, 38);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(277, 276);
            this.txtMsg.TabIndex = 21;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(249, 171);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 62);
            this.button1.TabIndex = 22;
            this.button1.Text = "Check Store";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(249, 239);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 62);
            this.button2.TabIndex = 23;
            this.button2.Text = "Check Payment List";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pnlWait
            // 
            this.pnlWait.BackgroundImage = global::WinPowerPOS.Properties.Resources.longyellowbackground2;
            this.pnlWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlWait.Controls.Add(this.label6);
            this.pnlWait.Location = new System.Drawing.Point(213, 57);
            this.pnlWait.Name = "pnlWait";
            this.pnlWait.Size = new System.Drawing.Size(409, 167);
            this.pnlWait.TabIndex = 24;
            this.pnlWait.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(102, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(205, 31);
            this.label6.TabIndex = 0;
            this.label6.Text = "PLEASE WAIT";
            // 
            // frmMagentoSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 344);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnSyncItem);
            this.Controls.Add(this.btnSyncCategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbCategory);
            this.Controls.Add(this.pnlWait);
            this.Name = "frmMagentoSync";
            this.Text = "Magento Sync";
            this.Load += new System.EventHandler(this.frmMagentoSync_Load);
            this.pnlWait.ResumeLayout(false);
            this.pnlWait.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lbCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSyncCategory;
        private System.Windows.Forms.Button btnSyncItem;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel pnlWait;
        private System.Windows.Forms.Label label6;
    }
}