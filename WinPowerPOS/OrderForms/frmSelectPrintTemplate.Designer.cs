namespace WinPowerPOS.OrderForms
{
    partial class frmSelectPrintTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectPrintTemplate));
            this.btnSales = new System.Windows.Forms.Button();
            this.btnDelivery = new System.Windows.Forms.Button();
            this.lblDeliveryWarning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSales
            // 
            this.btnSales.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSales.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSales.BackgroundImage")));
            this.btnSales.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSales.ForeColor = System.Drawing.Color.White;
            this.btnSales.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSales.Location = new System.Drawing.Point(52, 34);
            this.btnSales.Margin = new System.Windows.Forms.Padding(4);
            this.btnSales.Name = "btnSales";
            this.btnSales.Size = new System.Drawing.Size(133, 74);
            this.btnSales.TabIndex = 24;
            this.btnSales.Text = "SALES";
            this.btnSales.UseVisualStyleBackColor = true;
            this.btnSales.Click += new System.EventHandler(this.btnSales_Click);
            // 
            // btnDelivery
            // 
            this.btnDelivery.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelivery.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnDelivery.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelivery.ForeColor = System.Drawing.Color.Orange;
            this.btnDelivery.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDelivery.Location = new System.Drawing.Point(193, 34);
            this.btnDelivery.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelivery.Name = "btnDelivery";
            this.btnDelivery.Size = new System.Drawing.Size(133, 74);
            this.btnDelivery.TabIndex = 25;
            this.btnDelivery.Tag = "1";
            this.btnDelivery.Text = "DELIVERY";
            this.btnDelivery.UseVisualStyleBackColor = true;
            this.btnDelivery.Click += new System.EventHandler(this.btnDelivery_Click);
            // 
            // lblDeliveryWarning
            // 
            this.lblDeliveryWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeliveryWarning.BackColor = System.Drawing.Color.Gainsboro;
            this.lblDeliveryWarning.Location = new System.Drawing.Point(12, 122);
            this.lblDeliveryWarning.Name = "lblDeliveryWarning";
            this.lblDeliveryWarning.Size = new System.Drawing.Size(355, 32);
            this.lblDeliveryWarning.TabIndex = 26;
            // 
            // frmSelectPrintTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(379, 163);
            this.Controls.Add(this.lblDeliveryWarning);
            this.Controls.Add(this.btnSales);
            this.Controls.Add(this.btnDelivery);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectPrintTemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Invoice Method";
            this.Load += new System.EventHandler(this.frmSelectPrintTemplate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnSales;
        internal System.Windows.Forms.Button btnDelivery;
        private System.Windows.Forms.Label lblDeliveryWarning;
    }
}