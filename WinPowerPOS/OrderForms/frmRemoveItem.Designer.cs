namespace WinPowerPOS.OrderForms
{
    partial class frmRemoveItem
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
            this.txtDisc = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlItemQuantity = new System.Windows.Forms.Panel();
            this.lblItemName = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.pnlItemQuantity.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDisc
            // 
            this.txtDisc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDisc.Location = new System.Drawing.Point(15, 28);
            this.txtDisc.Name = "txtDisc";
            this.txtDisc.Size = new System.Drawing.Size(302, 26);
            this.txtDisc.TabIndex = 0;
            this.txtDisc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDisc_KeyDown);
            // 
            // btnSet
            // 
            this.btnSet.BackColor = System.Drawing.Color.Transparent;
            this.btnSet.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSet.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSet.ForeColor = System.Drawing.Color.White;
            this.btnSet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSet.Location = new System.Drawing.Point(173, 101);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(98, 47);
            this.btnSet.TabIndex = 2;
            this.btnSet.Text = "REMOVE";
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(277, 101);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(98, 47);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(277, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "Please Scan Item Barcode To Remove";
            // 
            // pnlItemQuantity
            // 
            this.pnlItemQuantity.Controls.Add(this.txtQuantity);
            this.pnlItemQuantity.Controls.Add(this.lblItemName);
            this.pnlItemQuantity.Location = new System.Drawing.Point(15, 61);
            this.pnlItemQuantity.Name = "pnlItemQuantity";
            this.pnlItemQuantity.Size = new System.Drawing.Size(499, 34);
            this.pnlItemQuantity.TabIndex = 31;
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.BackColor = System.Drawing.Color.Transparent;
            this.lblItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblItemName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblItemName.Location = new System.Drawing.Point(15, 9);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(95, 16);
            this.lblItemName.TabIndex = 31;
            this.lblItemName.Text = "lblItemName";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantity.Location = new System.Drawing.Point(409, 3);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(80, 26);
            this.txtQuantity.TabIndex = 1;
            this.txtQuantity.Enter += new System.EventHandler(this.txtQuantity_Enter);
            // 
            // frmRemoveItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(523, 160);
            this.Controls.Add(this.pnlItemQuantity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtDisc);
            this.Name = "frmRemoveItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remove Item";
            this.Load += new System.EventHandler(this.frmRemoveItem_Load);
            this.pnlItemQuantity.ResumeLayout(false);
            this.pnlItemQuantity.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnSet;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtDisc;
        private System.Windows.Forms.Panel pnlItemQuantity;
        private System.Windows.Forms.Label lblItemName;
        public System.Windows.Forms.TextBox txtQuantity;
    }
}