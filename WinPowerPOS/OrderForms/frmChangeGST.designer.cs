namespace WinPowerPOS.OrderForms
{
    partial class frmChangeGST
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExcGST = new System.Windows.Forms.Button();
            this.btnAbsorbGST = new System.Windows.Forms.Button();
            this.btnIncGST = new System.Windows.Forms.Button();
            this.btnNoGST = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Change GST on Sales";
            // 
            // btnExcGST
            // 
            this.btnExcGST.BackColor = System.Drawing.Color.Transparent;
            this.btnExcGST.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnExcGST.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnExcGST.ForeColor = System.Drawing.Color.White;
            this.btnExcGST.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExcGST.Location = new System.Drawing.Point(17, 70);
            this.btnExcGST.Margin = new System.Windows.Forms.Padding(6);
            this.btnExcGST.Name = "btnExcGST";
            this.btnExcGST.Size = new System.Drawing.Size(167, 60);
            this.btnExcGST.TabIndex = 6;
            this.btnExcGST.Text = "Exc - GST";
            this.toolTip1.SetToolTip(this.btnExcGST, "Unit Price: 100.00  Result : 107.00 GST : 7.00");
            this.btnExcGST.UseVisualStyleBackColor = false;
            this.btnExcGST.Click += new System.EventHandler(this.btnExcGST_Click);
            // 
            // btnAbsorbGST
            // 
            this.btnAbsorbGST.BackColor = System.Drawing.Color.Transparent;
            this.btnAbsorbGST.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnAbsorbGST.CausesValidation = false;
            this.btnAbsorbGST.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnAbsorbGST.ForeColor = System.Drawing.Color.White;
            this.btnAbsorbGST.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAbsorbGST.Location = new System.Drawing.Point(357, 70);
            this.btnAbsorbGST.Margin = new System.Windows.Forms.Padding(6);
            this.btnAbsorbGST.Name = "btnAbsorbGST";
            this.btnAbsorbGST.Size = new System.Drawing.Size(167, 59);
            this.btnAbsorbGST.TabIndex = 7;
            this.btnAbsorbGST.Text = "Absorb - GST";
            this.toolTip1.SetToolTip(this.btnAbsorbGST, "Unit Price: 100.00  Result : 93.46 GST : 0.00");
            this.btnAbsorbGST.UseVisualStyleBackColor = false;
            this.btnAbsorbGST.Click += new System.EventHandler(this.btnAbsorbGST_Click);
            // 
            // btnIncGST
            // 
            this.btnIncGST.BackColor = System.Drawing.Color.Transparent;
            this.btnIncGST.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnIncGST.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnIncGST.ForeColor = System.Drawing.Color.White;
            this.btnIncGST.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnIncGST.Location = new System.Drawing.Point(187, 70);
            this.btnIncGST.Margin = new System.Windows.Forms.Padding(6);
            this.btnIncGST.Name = "btnIncGST";
            this.btnIncGST.Size = new System.Drawing.Size(167, 59);
            this.btnIncGST.TabIndex = 8;
            this.btnIncGST.Text = "Inc - GST";
            this.toolTip1.SetToolTip(this.btnIncGST, "Unit Price: 100.00  Result : 100.00 GST : 6.54");
            this.btnIncGST.UseVisualStyleBackColor = false;
            this.btnIncGST.Click += new System.EventHandler(this.btnIncGST_Click);
            // 
            // btnNoGST
            // 
            this.btnNoGST.BackColor = System.Drawing.Color.Transparent;
            this.btnNoGST.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.btnNoGST.CausesValidation = false;
            this.btnNoGST.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnNoGST.ForeColor = System.Drawing.Color.White;
            this.btnNoGST.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNoGST.Location = new System.Drawing.Point(527, 70);
            this.btnNoGST.Margin = new System.Windows.Forms.Padding(6);
            this.btnNoGST.Name = "btnNoGST";
            this.btnNoGST.Size = new System.Drawing.Size(167, 59);
            this.btnNoGST.TabIndex = 9;
            this.btnNoGST.Text = "No - GST";
            this.toolTip1.SetToolTip(this.btnNoGST, "Unit Price: 100.00  Result : 100.00 GST : 0.00");
            this.btnNoGST.UseVisualStyleBackColor = false;
            this.btnNoGST.Click += new System.EventHandler(this.btnNoGST_Click);
            // 
            // frmChangeGST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 157);
            this.Controls.Add(this.btnNoGST);
            this.Controls.Add(this.btnIncGST);
            this.Controls.Add(this.btnExcGST);
            this.Controls.Add(this.btnAbsorbGST);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChangeGST";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change GST on Sales";
            this.Load += new System.EventHandler(this.frmChangeGST_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnExcGST;
        internal System.Windows.Forms.Button btnAbsorbGST;
        internal System.Windows.Forms.Button btnIncGST;
        internal System.Windows.Forms.Button btnNoGST;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}