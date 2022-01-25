namespace WinPowerPOS.OrderForms
{
    partial class frmVoidRemark
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVoidRemark));
            this.txtVoidReason = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnKeyboard = new System.Windows.Forms.PictureBox();
            this.chkReturnInventory = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnKeyboard)).BeginInit();
            this.SuspendLayout();
            // 
            // txtVoidReason
            // 
            this.txtVoidReason.AccessibleDescription = null;
            this.txtVoidReason.AccessibleName = null;
            resources.ApplyResources(this.txtVoidReason, "txtVoidReason");
            this.txtVoidReason.BackgroundImage = null;
            this.txtVoidReason.Font = null;
            this.txtVoidReason.Name = "txtVoidReason";
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
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // btnKeyboard
            // 
            this.btnKeyboard.AccessibleDescription = null;
            this.btnKeyboard.AccessibleName = null;
            resources.ApplyResources(this.btnKeyboard, "btnKeyboard");
            this.btnKeyboard.BackgroundImage = null;
            this.btnKeyboard.ErrorImage = null;
            this.btnKeyboard.Font = null;
            this.btnKeyboard.Image = global::WinPowerPOS.Properties.Resources.Best_Keyboard_Apps;
            this.btnKeyboard.ImageLocation = null;
            this.btnKeyboard.Name = "btnKeyboard";
            this.btnKeyboard.TabStop = false;
            this.btnKeyboard.Click += new System.EventHandler(this.btnKeyboard_Click);
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
            // frmVoidRemark
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.ControlBox = false;
            this.Controls.Add(this.chkReturnInventory);
            this.Controls.Add(this.btnKeyboard);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtVoidReason);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = null;
            this.Name = "frmVoidRemark";
            this.Load += new System.EventHandler(this.frmVoidRemark_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmVoidRemark_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.btnKeyboard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtVoidReason;
        internal System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox btnKeyboard;
        private System.Windows.Forms.CheckBox chkReturnInventory;
    }
}