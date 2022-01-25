namespace WinPowerPOS.OrderForms
{
    partial class frmSalesPerson
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
            this.cmbSalesPerson = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlPasswordInput = new System.Windows.Forms.Panel();
            this.btnKeyboard = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.pnlPasswordInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnKeyboard)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSalesPerson
            // 
            this.cmbSalesPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSalesPerson.FormattingEnabled = true;
            this.cmbSalesPerson.Location = new System.Drawing.Point(28, 54);
            this.cmbSalesPerson.Margin = new System.Windows.Forms.Padding(6);
            this.cmbSalesPerson.Name = "cmbSalesPerson";
            this.cmbSalesPerson.Size = new System.Drawing.Size(479, 32);
            this.cmbSalesPerson.TabIndex = 41;
            this.cmbSalesPerson.SelectedValueChanged += new System.EventHandler(this.cmbSalesPerson_SelectedValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(22, 17);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "SALES PERSON";
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(160, 157);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(167, 59);
            this.btnOk.TabIndex = 43;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(338, 157);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(169, 59);
            this.btnCancel.TabIndex = 44;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlPasswordInput
            // 
            this.pnlPasswordInput.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.pnlPasswordInput.Controls.Add(this.btnKeyboard);
            this.pnlPasswordInput.Controls.Add(this.label1);
            this.pnlPasswordInput.Controls.Add(this.txtPassword);
            this.pnlPasswordInput.Location = new System.Drawing.Point(28, 95);
            this.pnlPasswordInput.Name = "pnlPasswordInput";
            this.pnlPasswordInput.Size = new System.Drawing.Size(479, 42);
            this.pnlPasswordInput.TabIndex = 45;
            this.pnlPasswordInput.Visible = false;
            // 
            // btnKeyboard
            // 
            this.btnKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyboard.ErrorImage = null;
            this.btnKeyboard.Image = global::WinPowerPOS.Properties.Resources.Best_Keyboard_Apps;
            this.btnKeyboard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnKeyboard.Location = new System.Drawing.Point(332, 6);
            this.btnKeyboard.Name = "btnKeyboard";
            this.btnKeyboard.Size = new System.Drawing.Size(35, 35);
            this.btnKeyboard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnKeyboard.TabIndex = 35;
            this.btnKeyboard.TabStop = false;
            this.btnKeyboard.Click += new System.EventHandler(this.btnKeyboard_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "PASSWORD";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(101, 6);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(225, 29);
            this.txtPassword.TabIndex = 30;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // frmSalesPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(535, 231);
            this.Controls.Add(this.pnlPasswordInput);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbSalesPerson);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmSalesPerson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Person";
            this.Load += new System.EventHandler(this.frmSalesPerson_Load);
            this.pnlPasswordInput.ResumeLayout(false);
            this.pnlPasswordInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnKeyboard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSalesPerson;
        private System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlPasswordInput;
        private System.Windows.Forms.PictureBox btnKeyboard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
    }
}