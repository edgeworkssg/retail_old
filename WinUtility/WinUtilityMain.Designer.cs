namespace WinUtility
{
    partial class WinUtilityMain
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnUpdateAdjustedInventory = new System.Windows.Forms.Button();
            this.btnAdjustCostPrice = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(39, 47);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(525, 337);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnUpdateAdjustedInventory);
            this.tabPage1.Controls.Add(this.btnAdjustCostPrice);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(517, 311);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Updater";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnUpdateAdjustedInventory
            // 
            this.btnUpdateAdjustedInventory.Location = new System.Drawing.Point(164, 21);
            this.btnUpdateAdjustedInventory.Name = "btnUpdateAdjustedInventory";
            this.btnUpdateAdjustedInventory.Size = new System.Drawing.Size(143, 48);
            this.btnUpdateAdjustedInventory.TabIndex = 1;
            this.btnUpdateAdjustedInventory.Text = "Update Adjusted Inventory";
            this.btnUpdateAdjustedInventory.UseVisualStyleBackColor = true;
            this.btnUpdateAdjustedInventory.Click += new System.EventHandler(this.btnUpdateAdjustedInventory_Click);
            // 
            // btnAdjustCostPrice
            // 
            this.btnAdjustCostPrice.Location = new System.Drawing.Point(15, 21);
            this.btnAdjustCostPrice.Name = "btnAdjustCostPrice";
            this.btnAdjustCostPrice.Size = new System.Drawing.Size(143, 48);
            this.btnAdjustCostPrice.TabIndex = 0;
            this.btnAdjustCostPrice.Text = "Update Inventory Cost Price";
            this.btnAdjustCostPrice.UseVisualStyleBackColor = true;
            this.btnAdjustCostPrice.Click += new System.EventHandler(this.btnAdjustCostPrice_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(517, 311);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Misc";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // WinUtilityMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 416);
            this.Controls.Add(this.tabControl1);
            this.Name = "WinUtilityMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Collection of Utility Forms";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnAdjustCostPrice;
        private System.Windows.Forms.Button btnUpdateAdjustedInventory;
    }
}