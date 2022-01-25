namespace WinPowerPOS.PromoAdmin
{
    partial class frmViewPromo
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
            this.dgvPromotions = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotions)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPromotions
            // 
            this.dgvPromotions.AllowUserToAddRows = false;
            this.dgvPromotions.AllowUserToDeleteRows = false;
            this.dgvPromotions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPromotions.Location = new System.Drawing.Point(12, 26);
            this.dgvPromotions.Name = "dgvPromotions";
            this.dgvPromotions.ReadOnly = true;
            this.dgvPromotions.Size = new System.Drawing.Size(678, 234);
            this.dgvPromotions.TabIndex = 0;
            // 
            // frmViewPromo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(702, 264);
            this.Controls.Add(this.dgvPromotions);
            this.Name = "frmViewPromo";
            this.Text = "Promotions";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPromotions;
    }
}