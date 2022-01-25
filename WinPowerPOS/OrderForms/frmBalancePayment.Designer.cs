namespace WinPowerPOS.OrderForms
{
    partial class frmBalancePayment
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
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtFinalAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOriginalAmount = new System.Windows.Forms.Label();
            this.lblRoundingAmount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnConfirm.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfirm.Location = new System.Drawing.Point(158, 88);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(114, 42);
            this.btnConfirm.TabIndex = 52;
            this.btnConfirm.Text = "OK";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 16);
            this.label8.TabIndex = 54;
            this.label8.Text = "Final Amount";
            // 
            // txtFinalAmount
            // 
            this.txtFinalAmount.Location = new System.Drawing.Point(158, 8);
            this.txtFinalAmount.Name = "txtFinalAmount";
            this.txtFinalAmount.Size = new System.Drawing.Size(114, 20);
            this.txtFinalAmount.TabIndex = 53;
            this.txtFinalAmount.TextChanged += new System.EventHandler(this.txtFinalAmount_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 16);
            this.label1.TabIndex = 56;
            this.label1.Text = "Original Amount";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 16);
            this.label2.TabIndex = 58;
            this.label2.Text = "Rounding Amount";
            // 
            // lblOriginalAmount
            // 
            this.lblOriginalAmount.AutoSize = true;
            this.lblOriginalAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblOriginalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOriginalAmount.Location = new System.Drawing.Point(155, 35);
            this.lblOriginalAmount.Name = "lblOriginalAmount";
            this.lblOriginalAmount.Size = new System.Drawing.Size(36, 16);
            this.lblOriginalAmount.TabIndex = 59;
            this.lblOriginalAmount.Text = "0.00";
            // 
            // lblRoundingAmount
            // 
            this.lblRoundingAmount.AutoSize = true;
            this.lblRoundingAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblRoundingAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoundingAmount.Location = new System.Drawing.Point(155, 61);
            this.lblRoundingAmount.Name = "lblRoundingAmount";
            this.lblRoundingAmount.Size = new System.Drawing.Size(36, 16);
            this.lblRoundingAmount.TabIndex = 60;
            this.lblRoundingAmount.Text = "0.00";
            // 
            // frmBalancePayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 142);
            this.ControlBox = false;
            this.Controls.Add(this.lblRoundingAmount);
            this.Controls.Add(this.lblOriginalAmount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtFinalAmount);
            this.Controls.Add(this.btnConfirm);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBalancePayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Balance Payment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtFinalAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOriginalAmount;
        private System.Windows.Forms.Label lblRoundingAmount;
    }
}