namespace WinPowerPOS.MaintenanceForms
{
    partial class frmSupport
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
            this.btnBackupDB = new System.Windows.Forms.Button();
            this.btnClearTrainingData = new System.Windows.Forms.Button();
            this.btnClearSalesAndInventory = new System.Windows.Forms.Button();
            this.btnViewDBLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBackupDB
            // 
            this.btnBackupDB.BackColor = System.Drawing.Color.LightCyan;
            this.btnBackupDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackupDB.Location = new System.Drawing.Point(12, 11);
            this.btnBackupDB.Name = "btnBackupDB";
            this.btnBackupDB.Size = new System.Drawing.Size(132, 54);
            this.btnBackupDB.TabIndex = 0;
            this.btnBackupDB.Text = "BACK UP DB";
            this.btnBackupDB.UseVisualStyleBackColor = false;
            this.btnBackupDB.Click += new System.EventHandler(this.btnBackupDB_Click);
            // 
            // btnClearTrainingData
            // 
            this.btnClearTrainingData.BackColor = System.Drawing.Color.LawnGreen;
            this.btnClearTrainingData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearTrainingData.Location = new System.Drawing.Point(317, 11);
            this.btnClearTrainingData.Name = "btnClearTrainingData";
            this.btnClearTrainingData.Size = new System.Drawing.Size(132, 54);
            this.btnClearTrainingData.TabIndex = 1;
            this.btnClearTrainingData.Text = "CLEAR ALL ITEM DATA";
            this.btnClearTrainingData.UseVisualStyleBackColor = false;
            this.btnClearTrainingData.Click += new System.EventHandler(this.btnClearTrainingData_Click);
            // 
            // btnClearSalesAndInventory
            // 
            this.btnClearSalesAndInventory.BackColor = System.Drawing.Color.LightPink;
            this.btnClearSalesAndInventory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSalesAndInventory.Location = new System.Drawing.Point(164, 11);
            this.btnClearSalesAndInventory.Name = "btnClearSalesAndInventory";
            this.btnClearSalesAndInventory.Size = new System.Drawing.Size(132, 54);
            this.btnClearSalesAndInventory.TabIndex = 3;
            this.btnClearSalesAndInventory.Text = "CLEAR SALES && INVENTORY";
            this.btnClearSalesAndInventory.UseVisualStyleBackColor = false;
            this.btnClearSalesAndInventory.Click += new System.EventHandler(this.btnClearSalesAndInventory_Click);
            // 
            // btnViewDBLog
            // 
            this.btnViewDBLog.BackColor = System.Drawing.Color.LightYellow;
            this.btnViewDBLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewDBLog.Location = new System.Drawing.Point(568, 11);
            this.btnViewDBLog.Name = "btnViewDBLog";
            this.btnViewDBLog.Size = new System.Drawing.Size(132, 54);
            this.btnViewDBLog.TabIndex = 4;
            this.btnViewDBLog.Text = "View DB Log";
            this.btnViewDBLog.UseVisualStyleBackColor = false;
            this.btnViewDBLog.Click += new System.EventHandler(this.btnViewDBLog_Click);
            // 
            // frmSupport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 415);
            this.Controls.Add(this.btnViewDBLog);
            this.Controls.Add(this.btnClearSalesAndInventory);
            this.Controls.Add(this.btnClearTrainingData);
            this.Controls.Add(this.btnBackupDB);
            this.Name = "frmSupport";
            this.Text = "Support Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBackupDB;
        private System.Windows.Forms.Button btnClearTrainingData;
        private System.Windows.Forms.Button btnClearSalesAndInventory;
        private System.Windows.Forms.Button btnViewDBLog;
    }
}