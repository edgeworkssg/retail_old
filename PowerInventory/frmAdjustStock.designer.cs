namespace PowerInventory
{
    partial class frmAdjustStock
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
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdjustStock));
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

            // 
            // frmAdjustStock
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            //resources.ApplyResources(this, "$this");
            this.Name = "frmAdjustStock";
            this.Tag = "ADJUSTMENT";
            this.Load += new System.EventHandler(this.frmAdjustStock_Load);
            this.ResumeLayout(false);

        }

        #endregion
    }
}