namespace WinPowerPOS.EditBillForms
{
    partial class frmSelectVoidReason
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectVoidReason));
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbVoidReason = new System.Windows.Forms.ComboBox();
            this.chkReturnInventory = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleDescription = null;
            this.btnConfirm.AccessibleName = null;
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbVoidReason
            // 
            this.cmbVoidReason.AccessibleDescription = null;
            this.cmbVoidReason.AccessibleName = null;
            resources.ApplyResources(this.cmbVoidReason, "cmbVoidReason");
            this.cmbVoidReason.BackgroundImage = null;
            this.cmbVoidReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVoidReason.FormattingEnabled = true;
            this.cmbVoidReason.Name = "cmbVoidReason";
            // 
            // chkReturnInventory
            // 
            this.chkReturnInventory.AccessibleDescription = null;
            this.chkReturnInventory.AccessibleName = null;
            resources.ApplyResources(this.chkReturnInventory, "chkReturnInventory");
            this.chkReturnInventory.BackgroundImage = null;
            this.chkReturnInventory.Checked = true;
            this.chkReturnInventory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReturnInventory.Name = "chkReturnInventory";
            this.chkReturnInventory.UseVisualStyleBackColor = true;
            // 
            // frmSelectVoidReason
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.ControlBox = false;
            this.Controls.Add(this.chkReturnInventory);
            this.Controls.Add(this.cmbVoidReason);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.label1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectVoidReason";
            this.Load += new System.EventHandler(this.frmSelectVoidReason_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnConfirm;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbVoidReason;
        private System.Windows.Forms.CheckBox chkReturnInventory;
    }
}