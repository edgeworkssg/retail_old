namespace WinPowerPOS.MembershipForms
{
    partial class frmMembershipTap
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
            this.btnPutDeposit = new System.Windows.Forms.Button();
            this.btnRedeemDeposit = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCurrentTapAmount = new System.Windows.Forms.Label();
            this.lblMember = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnPutDeposit
            // 
            this.btnPutDeposit.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightblue;
            this.btnPutDeposit.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnPutDeposit.ForeColor = System.Drawing.Color.Black;
            this.btnPutDeposit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPutDeposit.Location = new System.Drawing.Point(31, 140);
            this.btnPutDeposit.Margin = new System.Windows.Forms.Padding(6);
            this.btnPutDeposit.Name = "btnPutDeposit";
            this.btnPutDeposit.Size = new System.Drawing.Size(136, 79);
            this.btnPutDeposit.TabIndex = 83;
            this.btnPutDeposit.Tag = "false";
            this.btnPutDeposit.Text = "Put Deposit";
            this.btnPutDeposit.UseVisualStyleBackColor = true;
            this.btnPutDeposit.Click += new System.EventHandler(this.btnPutDeposit_Click);
            // 
            // btnRedeemDeposit
            // 
            this.btnRedeemDeposit.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightyellowbutton;
            this.btnRedeemDeposit.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnRedeemDeposit.ForeColor = System.Drawing.Color.Black;
            this.btnRedeemDeposit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRedeemDeposit.Location = new System.Drawing.Point(179, 140);
            this.btnRedeemDeposit.Margin = new System.Windows.Forms.Padding(6);
            this.btnRedeemDeposit.Name = "btnRedeemDeposit";
            this.btnRedeemDeposit.Size = new System.Drawing.Size(137, 79);
            this.btnRedeemDeposit.TabIndex = 84;
            this.btnRedeemDeposit.Tag = "true";
            this.btnRedeemDeposit.Text = "Redeem Deposit";
            this.btnRedeemDeposit.UseVisualStyleBackColor = true;
            this.btnRedeemDeposit.Click += new System.EventHandler(this.btnPutDeposit_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(22, 37);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(190, 21);
            this.label7.TabIndex = 85;
            this.label7.Text = "Current Deposit Amount";
            // 
            // lblCurrentTapAmount
            // 
            this.lblCurrentTapAmount.AutoSize = true;
            this.lblCurrentTapAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentTapAmount.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTapAmount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurrentTapAmount.Location = new System.Drawing.Point(22, 73);
            this.lblCurrentTapAmount.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCurrentTapAmount.Name = "lblCurrentTapAmount";
            this.lblCurrentTapAmount.Size = new System.Drawing.Size(16, 21);
            this.lblCurrentTapAmount.TabIndex = 86;
            this.lblCurrentTapAmount.Text = "-";
            // 
            // lblMember
            // 
            this.lblMember.AutoSize = true;
            this.lblMember.BackColor = System.Drawing.Color.Transparent;
            this.lblMember.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMember.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMember.Location = new System.Drawing.Point(22, 7);
            this.lblMember.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblMember.Name = "lblMember";
            this.lblMember.Size = new System.Drawing.Size(16, 21);
            this.lblMember.TabIndex = 87;
            this.lblMember.Text = "-";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(98, 102);
            this.txtRemark.MaxLength = 49;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(238, 29);
            this.txtRemark.TabIndex = 88;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(22, 107);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 21);
            this.label1.TabIndex = 89;
            this.label1.Text = "Remark";
            // 
            // frmMembershipTap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(369, 250);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.lblMember);
            this.Controls.Add(this.lblCurrentTapAmount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnRedeemDeposit);
            this.Controls.Add(this.btnPutDeposit);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmMembershipTap";
            this.Text = "Deposit";
            this.Load += new System.EventHandler(this.frmMembershipTap_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnPutDeposit;
        internal System.Windows.Forms.Button btnRedeemDeposit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCurrentTapAmount;
        private System.Windows.Forms.Label lblMember;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label1;
    }
}