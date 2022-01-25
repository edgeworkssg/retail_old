namespace WinPowerPOS.OrderForms
{
    partial class frmSelectOtherPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectOtherPayment));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblNumOfItems = new System.Windows.Forms.Label();
            this.lblRefNo = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // lblNumOfItems
            // 
            resources.ApplyResources(this.lblNumOfItems, "lblNumOfItems");
            this.lblNumOfItems.BackColor = System.Drawing.Color.Transparent;
            this.lblNumOfItems.Name = "lblNumOfItems";
            // 
            // lblRefNo
            // 
            resources.ApplyResources(this.lblRefNo, "lblRefNo");
            this.lblRefNo.BackColor = System.Drawing.Color.Transparent;
            this.lblRefNo.Name = "lblRefNo";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSelectOtherPayment
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblNumOfItems);
            this.Controls.Add(this.lblRefNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmSelectOtherPayment";
            this.Load += new System.EventHandler(this.frmSelectPayment_Load);
            this.Click += new System.EventHandler(this.btnMakePayment_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblNumOfItems;
        private System.Windows.Forms.Label lblRefNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Button btnCancel;
    }
}