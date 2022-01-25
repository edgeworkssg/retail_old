namespace PowerInventory
{
    partial class frmStockOut
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
            this.SuspendLayout();
            // 
            // frmStockOut
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmStockOut";
            this.Tag = "STOCK ISSUE";
            this.Text = "STOCK ISSUE";
            this.Load += new System.EventHandler(this.frmStockOut_Load);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.ResumeLayout(false);

        }

        #endregion
    }
}