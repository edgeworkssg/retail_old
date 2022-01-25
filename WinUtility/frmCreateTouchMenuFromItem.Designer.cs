namespace WinUtility
{
    partial class frmCreateTouchMenuFromItem
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
            this.txtDepartmentName = new System.Windows.Forms.TextBox();
            this.txtGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.Location = new System.Drawing.Point(38, 12);
            this.txtDepartmentName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Size = new System.Drawing.Size(217, 24);
            this.txtDepartmentName.TabIndex = 0;
            this.txtDepartmentName.Visible = false;
            // 
            // txtGenerate
            // 
            this.txtGenerate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGenerate.Location = new System.Drawing.Point(0, 0);
            this.txtGenerate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGenerate.Name = "txtGenerate";
            this.txtGenerate.Size = new System.Drawing.Size(300, 101);
            this.txtGenerate.TabIndex = 1;
            this.txtGenerate.Text = "Generate Touch Screen";
            this.txtGenerate.UseVisualStyleBackColor = true;
            this.txtGenerate.Click += new System.EventHandler(this.txtGenerate_Click);
            // 
            // frmCreateTouchMenuFromItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 101);
            this.Controls.Add(this.txtGenerate);
            this.Controls.Add(this.txtDepartmentName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmCreateTouchMenuFromItem";
            this.Text = "Create Touch Menu From Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDepartmentName;
        private System.Windows.Forms.Button txtGenerate;
    }
}