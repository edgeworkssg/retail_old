namespace WinPowerPOS.MembershipForms
{
    partial class frmNewMembersList
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
            this.MembershipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameToAppear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BirthDae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMember
            // 
            this.dgvMember.AllowUserToAddRows = false;
            this.dgvMember.AllowUserToDeleteRows = false;
            this.dgvMember.AllowUserToResizeColumns = false;
            this.dgvMember.AllowUserToResizeRows = false;
            this.dgvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MembershipNo,
            this.GroupName,
            this.NameToAppear,
            this.NRIC,
            this.ExpiryDate,
            this.BirthDae,
            this.FirstName,
            this.LastName,
            this.EMail});
            this.dgvMember.Location = new System.Drawing.Point(12, 49);
            this.dgvMember.Name = "dgvMember";
            this.dgvMember.ReadOnly = true;
            this.dgvMember.RowHeadersWidth = 20;
            this.dgvMember.Size = new System.Drawing.Size(777, 503);
            this.dgvMember.TabIndex = 27;
            this.dgvMember.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMember_CellContentClick);
            // 
            // MembershipNo
            // 
            this.MembershipNo.DataPropertyName = "MembershipNo";
            this.MembershipNo.HeaderText = "No";
            this.MembershipNo.Name = "MembershipNo";
            // 
            // GroupName
            // 
            this.GroupName.DataPropertyName = "GroupName";
            this.GroupName.HeaderText = "Group";
            this.GroupName.Name = "GroupName";
            // 
            // NameToAppear
            // 
            this.NameToAppear.DataPropertyName = "NameToAppear";
            this.NameToAppear.HeaderText = "Name To Appear";
            this.NameToAppear.Name = "NameToAppear";
            this.NameToAppear.Width = 140;
            // 
            // NRIC
            // 
            this.NRIC.DataPropertyName = "NRIC";
            this.NRIC.HeaderText = "NRIC";
            this.NRIC.Name = "NRIC";
            // 
            // ExpiryDate
            // 
            this.ExpiryDate.DataPropertyName = "ExpiryDate";
            this.ExpiryDate.HeaderText = "ExpiryDate";
            this.ExpiryDate.Name = "ExpiryDate";
            // 
            // BirthDae
            // 
            this.BirthDae.DataPropertyName = "DateOfBirth";
            this.BirthDae.HeaderText = "Birthday";
            this.BirthDae.Name = "BirthDae";
            // 
            // FirstName
            // 
            this.FirstName.DataPropertyName = "FirstName";
            this.FirstName.HeaderText = "First Name";
            this.FirstName.Name = "FirstName";
            this.FirstName.Width = 140;
            // 
            // LastName
            // 
            this.LastName.DataPropertyName = "lastName";
            this.LastName.HeaderText = "Last Name";
            this.LastName.Name = "LastName";
            this.LastName.Width = 140;
            // 
            // EMail
            // 
            this.EMail.DataPropertyName = "email";
            this.EMail.HeaderText = "E-Mail";
            this.EMail.Name = "EMail";
            this.EMail.Width = 140;
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClear.CausesValidation = false;
            this.btnClear.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClear.Location = new System.Drawing.Point(696, 7);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(93, 36);
            this.btnClear.TabIndex = 30;
            this.btnClear.Text = "CLOSE";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmNewMembersList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgndlong;
            this.ClientSize = new System.Drawing.Size(798, 564);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.dgvMember);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmNewMembersList";
            this.Text = "frmNewMembersList";
            this.Load += new System.EventHandler(this.frmNewMembersList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMember;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembershipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameToAppear;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BirthDae;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMail;
        internal System.Windows.Forms.Button btnClear;
    }
}