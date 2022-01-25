namespace WinPowerPOS.KioskForms
{
    partial class frmItemList
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
            this.hostItemList = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlItemList = new WinPowerPOS.KioskForms.ItemList();
            this.SuspendLayout();
            // 
            // hostItemList
            // 
            this.hostItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hostItemList.Location = new System.Drawing.Point(0, 0);
            this.hostItemList.Name = "hostItemList";
            this.hostItemList.Size = new System.Drawing.Size(698, 498);
            this.hostItemList.TabIndex = 0;
            this.hostItemList.Text = "elementHost1";
            this.hostItemList.Child = this.ctrlItemList;
            // 
            // frmItemList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 498);
            this.ControlBox = false;
            this.Controls.Add(this.hostItemList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmItemList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmItemList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost hostItemList;
        private ItemList ctrlItemList;
    }
}