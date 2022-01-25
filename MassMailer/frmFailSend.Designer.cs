namespace MassMailer
{
    partial class frmFailSend
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
            this.dgvMember = new System.Windows.Forms.DataGridView();
            this.btnSend = new System.Windows.Forms.Button();
            this.pnlSendOrder = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label18 = new System.Windows.Forms.Label();
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).BeginInit();
            this.pnlSendOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMember
            // 
            this.dgvMember.AllowUserToAddRows = false;
            this.dgvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MembershipNo,
            this.NameToAppear,
            this.EMail});
            this.dgvMember.Location = new System.Drawing.Point(12, 12);
            this.dgvMember.Name = "dgvMember";
            this.dgvMember.RowHeadersWidth = 20;
            this.dgvMember.Size = new System.Drawing.Size(523, 287);
            this.dgvMember.TabIndex = 27;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.Green;
            this.btnSend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSend.Location = new System.Drawing.Point(418, 304);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(117, 41);
            this.btnSend.TabIndex = 31;
            this.btnSend.Text = "RESEND >";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // pnlSendOrder
            // 
            this.pnlSendOrder.BackColor = System.Drawing.Color.White;
            this.pnlSendOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSendOrder.Controls.Add(this.pictureBox2);
            this.pnlSendOrder.Controls.Add(this.label18);
            this.pnlSendOrder.Location = new System.Drawing.Point(12, 47);
            this.pnlSendOrder.Name = "pnlSendOrder";
            this.pnlSendOrder.Size = new System.Drawing.Size(523, 239);
            this.pnlSendOrder.TabIndex = 71;
            this.pnlSendOrder.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MassMailer.Properties.Resources.progressbar_long_green;
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(114, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(307, 63);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(79, 114);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(386, 26);
            this.label18.TabIndex = 1;
            this.label18.Text = "SENDING EMAILS. PLEASE WAIT";
            // 
            // MembershipNo
            // 
            this.MembershipNo.DataPropertyName = "MembershipNo";
            this.MembershipNo.HeaderText = "Membership No";
            this.MembershipNo.Name = "MembershipNo";
            this.MembershipNo.Width = 140;
            // 
            // NameToAppear
            // 
            this.NameToAppear.DataPropertyName = "NameToAppear";
            this.NameToAppear.HeaderText = "Name To Appear";
            this.NameToAppear.Name = "NameToAppear";
            this.NameToAppear.Width = 180;
            // 
            // EMail
            // 
            this.EMail.DataPropertyName = "email";
            this.EMail.HeaderText = "E-Mail";
            this.EMail.Name = "EMail";
            this.EMail.Width = 180;
            // 
            // btnClear
            // 
            this.btnClear.CausesValidation = false;
            this.btnClear.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.DarkRed;
            this.btnClear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClear.Location = new System.Drawing.Point(312, 305);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 40);
            this.btnClear.TabIndex = 72;
            this.btnClear.Text = "CLOSE";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCopy.ForeColor = System.Drawing.Color.Green;
            this.btnCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCopy.Location = new System.Drawing.Point(12, 305);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(117, 41);
            this.btnCopy.TabIndex = 73;
            this.btnCopy.Text = "COPY";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // frmFailSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 357);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.dgvMember);
            this.Controls.Add(this.pnlSendOrder);
            this.Name = "frmFailSend";
            this.Text = "Re-Send Fail";
            this.Load += new System.EventHandler(this.frmFailSend_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).EndInit();
            this.pnlSendOrder.ResumeLayout(false);
            this.pnlSendOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMember;
        internal System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel pnlSendOrder;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMail;
        internal System.Windows.Forms.Button btnClear;
        internal System.Windows.Forms.Button btnCopy;
    }
}