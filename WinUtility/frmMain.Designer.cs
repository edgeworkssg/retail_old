namespace WinUtility
{
    partial class frmMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRestoreDB = new System.Windows.Forms.Button();
            this.tTableName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClearData = new System.Windows.Forms.Button();
            this.btnRetailTouchScreen = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRestoreDB);
            this.groupBox1.Location = new System.Drawing.Point(14, 45);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Size = new System.Drawing.Size(228, 235);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generic Tools";
            // 
            // btnRestoreDB
            // 
            this.btnRestoreDB.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRestoreDB.Location = new System.Drawing.Point(5, 21);
            this.btnRestoreDB.Name = "btnRestoreDB";
            this.btnRestoreDB.Size = new System.Drawing.Size(218, 40);
            this.btnRestoreDB.TabIndex = 0;
            this.btnRestoreDB.Text = "Restore / Create DB";
            this.btnRestoreDB.UseVisualStyleBackColor = true;
            this.btnRestoreDB.Click += new System.EventHandler(this.btnRestoreDB_Click);
            // 
            // tTableName
            // 
            this.tTableName.Location = new System.Drawing.Point(127, 13);
            this.tTableName.Margin = new System.Windows.Forms.Padding(4);
            this.tTableName.Name = "tTableName";
            this.tTableName.Size = new System.Drawing.Size(164, 24);
            this.tTableName.TabIndex = 9;
            this.tTableName.Validated += new System.EventHandler(this.tTableName_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Table Name :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.btnClearData);
            this.groupBox2.Controls.Add(this.btnRetailTouchScreen);
            this.groupBox2.Location = new System.Drawing.Point(252, 45);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Size = new System.Drawing.Size(228, 235);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Retail";
            // 
            // btnClearData
            // 
            this.btnClearData.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnClearData.Location = new System.Drawing.Point(5, 61);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(218, 40);
            this.btnClearData.TabIndex = 1;
            this.btnClearData.Text = "Clear All Training Data";
            this.btnClearData.UseVisualStyleBackColor = true;
            this.btnClearData.Click += new System.EventHandler(this.btnClearData_Click);
            // 
            // btnRetailTouchScreen
            // 
            this.btnRetailTouchScreen.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRetailTouchScreen.Location = new System.Drawing.Point(5, 21);
            this.btnRetailTouchScreen.Name = "btnRetailTouchScreen";
            this.btnRetailTouchScreen.Size = new System.Drawing.Size(218, 40);
            this.btnRetailTouchScreen.TabIndex = 0;
            this.btnRetailTouchScreen.Text = "Create Touch Screen";
            this.btnRetailTouchScreen.UseVisualStyleBackColor = true;
            this.btnRetailTouchScreen.Click += new System.EventHandler(this.btnRetailTouchScreen_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(5, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(218, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "Clear All Training Data";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 293);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tTableName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmMain";
            this.Text = "Utility";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRestoreDB;
        private System.Windows.Forms.TextBox tTableName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRetailTouchScreen;
        private System.Windows.Forms.Button btnClearData;
        private System.Windows.Forms.Button button1;
    }
}