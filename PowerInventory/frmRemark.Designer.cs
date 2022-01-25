namespace PowerInventory.OrderForms
{
    partial class frmRemark
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRemark));
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblComment = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtRemark
            // 
            resources.ApplyResources(this.txtRemark, "txtRemark");
            this.txtRemark.Name = "txtRemark";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblComment
            // 
            resources.ApplyResources(this.lblComment, "lblComment");
            this.lblComment.BackColor = System.Drawing.Color.Transparent;
            this.lblComment.Name = "lblComment";
            // 
            // frmRemark
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PowerInventory.Properties.Resources.menubackgndlong;
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtRemark);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmRemark";
            this.Load += new System.EventHandler(this.frmRemark_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnConfirm;
        internal System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label lblComment;
    }
}