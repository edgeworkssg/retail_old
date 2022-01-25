namespace WinPowerPOS.OrderForms
{
    partial class frmBreakPointValue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBreakPointValue));
            this.txtGetpoint = new System.Windows.Forms.TextBox();
            this.txtBreakPointPrice = new System.Windows.Forms.TextBox();
            this.lblGetPoint = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblItemName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtGetpoint
            // 
            this.txtGetpoint.AccessibleDescription = null;
            this.txtGetpoint.AccessibleName = null;
            resources.ApplyResources(this.txtGetpoint, "txtGetpoint");
            this.txtGetpoint.BackgroundImage = null;
            this.txtGetpoint.Name = "txtGetpoint";
            // 
            // txtBreakPointPrice
            // 
            this.txtBreakPointPrice.AccessibleDescription = null;
            this.txtBreakPointPrice.AccessibleName = null;
            resources.ApplyResources(this.txtBreakPointPrice, "txtBreakPointPrice");
            this.txtBreakPointPrice.BackgroundImage = null;
            this.txtBreakPointPrice.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtBreakPointPrice.Name = "txtBreakPointPrice";
            // 
            // lblGetPoint
            // 
            this.lblGetPoint.AccessibleDescription = null;
            this.lblGetPoint.AccessibleName = null;
            resources.ApplyResources(this.lblGetPoint, "lblGetPoint");
            this.lblGetPoint.BackColor = System.Drawing.Color.Transparent;
            this.lblGetPoint.Name = "lblGetPoint";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // btnSave
            // 
            this.btnSave.AccessibleDescription = null;
            this.btnSave.AccessibleName = null;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.AccessibleDescription = null;
            this.BtnClear.AccessibleName = null;
            resources.ApplyResources(this.BtnClear, "BtnClear");
            this.BtnClear.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.BtnClear.ForeColor = System.Drawing.Color.White;
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.AccessibleDescription = null;
            this.lblMessage.AccessibleName = null;
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.Font = null;
            this.lblMessage.Name = "lblMessage";
            // 
            // lblItemName
            // 
            this.lblItemName.AccessibleDescription = null;
            this.lblItemName.AccessibleName = null;
            resources.ApplyResources(this.lblItemName, "lblItemName");
            this.lblItemName.BackColor = System.Drawing.Color.Transparent;
            this.lblItemName.Name = "lblItemName";
            // 
            // frmBreakPointValue
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.Controls.Add(this.lblItemName);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblGetPoint);
            this.Controls.Add(this.txtBreakPointPrice);
            this.Controls.Add(this.txtGetpoint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = null;
            this.Name = "frmBreakPointValue";
            this.Load += new System.EventHandler(this.frmBreakPointValue_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGetpoint;
        private System.Windows.Forms.TextBox txtBreakPointPrice;
        private System.Windows.Forms.Label lblGetPoint;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblItemName;
    }
}