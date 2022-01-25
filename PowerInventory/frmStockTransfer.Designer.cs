namespace PowerInventory
{
    partial class frmStockTransfer
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
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockTransfer));
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // pnlLoading
            // 
            //this.pnlLoading.Location = new System.Drawing.Point(-99, -89);

            // 
            // frmStockTransfer
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            //resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Name = "frmStockTransfer";
            this.Tag = "TRANSFER";
            this.Load += new System.EventHandler(this.frmStockTransfer_Load);
            this.ResumeLayout(false);

        }

        #endregion
    }
}