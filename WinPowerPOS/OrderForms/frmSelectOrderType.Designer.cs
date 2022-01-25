namespace WinPowerPOS.OrderForms
{
    partial class frmSelectOrderType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectOrderType));
            this.btnCashCarry = new System.Windows.Forms.Button();
            this.btnPreorder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCashCarry
            // 
            this.btnCashCarry.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCashCarry.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCashCarry.BackgroundImage")));
            this.btnCashCarry.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCashCarry.ForeColor = System.Drawing.Color.White;
            this.btnCashCarry.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCashCarry.Location = new System.Drawing.Point(52, 34);
            this.btnCashCarry.Margin = new System.Windows.Forms.Padding(4);
            this.btnCashCarry.Name = "btnCashCarry";
            this.btnCashCarry.Size = new System.Drawing.Size(133, 74);
            this.btnCashCarry.TabIndex = 26;
            this.btnCashCarry.Text = "CASH AND CARRY";
            this.btnCashCarry.UseVisualStyleBackColor = true;
            this.btnCashCarry.Click += new System.EventHandler(this.btnCashCarry_Click);
            // 
            // btnPreorder
            // 
            this.btnPreorder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPreorder.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnPreorder.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnPreorder.ForeColor = System.Drawing.Color.Orange;
            this.btnPreorder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPreorder.Location = new System.Drawing.Point(193, 34);
            this.btnPreorder.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreorder.Name = "btnPreorder";
            this.btnPreorder.Size = new System.Drawing.Size(133, 74);
            this.btnPreorder.TabIndex = 27;
            this.btnPreorder.Tag = "1";
            this.btnPreorder.Text = "PREORDER";
            this.btnPreorder.UseVisualStyleBackColor = true;
            this.btnPreorder.Click += new System.EventHandler(this.btnPreorder_Click);
            // 
            // frmSelectOrderType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(379, 154);
            this.Controls.Add(this.btnCashCarry);
            this.Controls.Add(this.btnPreorder);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectOrderType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Order Type";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnCashCarry;
        internal System.Windows.Forms.Button btnPreorder;
    }
}