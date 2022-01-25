namespace WinPowerPOS.OrderForms
{
    partial class frmPaymentIntegration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaymentIntegration));
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPaymentType = new System.Windows.Forms.Label();
            this.lblPaymentAmount = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.btnChangePaymentType = new System.Windows.Forms.Button();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.lblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRetry
            // 
            this.btnRetry.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnRetry.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnRetry.ForeColor = System.Drawing.Color.White;
            this.btnRetry.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRetry.Location = new System.Drawing.Point(290, 152);
            this.btnRetry.Margin = new System.Windows.Forms.Padding(4);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(125, 50);
            this.btnRetry.TabIndex = 85;
            this.btnRetry.Tag = "1";
            this.btnRetry.Text = "RETRY";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // btnManual
            // 
            this.btnManual.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnManual.BackgroundImage")));
            this.btnManual.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnManual.ForeColor = System.Drawing.Color.White;
            this.btnManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnManual.Location = new System.Drawing.Point(150, 152);
            this.btnManual.Margin = new System.Windows.Forms.Padding(4);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(125, 50);
            this.btnManual.TabIndex = 84;
            this.btnManual.Text = "MANUAL";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(5, 152);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 50);
            this.btnCancel.TabIndex = 83;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 86;
            this.label1.Text = "Payment Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 20);
            this.label2.TabIndex = 87;
            this.label2.Text = "Amount";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(133, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 20);
            this.label3.TabIndex = 88;
            this.label3.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(133, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 20);
            this.label4.TabIndex = 89;
            this.label4.Text = ":";
            // 
            // lblPaymentType
            // 
            this.lblPaymentType.AutoSize = true;
            this.lblPaymentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentType.Location = new System.Drawing.Point(152, 13);
            this.lblPaymentType.Name = "lblPaymentType";
            this.lblPaymentType.Size = new System.Drawing.Size(55, 20);
            this.lblPaymentType.TabIndex = 90;
            this.lblPaymentType.Text = "NETS";
            // 
            // lblPaymentAmount
            // 
            this.lblPaymentAmount.AutoSize = true;
            this.lblPaymentAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentAmount.Location = new System.Drawing.Point(152, 42);
            this.lblPaymentAmount.Name = "lblPaymentAmount";
            this.lblPaymentAmount.Size = new System.Drawing.Size(44, 20);
            this.lblPaymentAmount.TabIndex = 91;
            this.lblPaymentAmount.Text = "0.00";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 74);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(403, 41);
            this.lblStatus.TabIndex = 93;
            this.lblStatus.Text = "[status]";
            // 
            // worker
            // 
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
            // 
            // btnChangePaymentType
            // 
            this.btnChangePaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChangePaymentType.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightyellowbutton;
            this.btnChangePaymentType.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnChangePaymentType.ForeColor = System.Drawing.Color.Black;
            this.btnChangePaymentType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChangePaymentType.Location = new System.Drawing.Point(5, 152);
            this.btnChangePaymentType.Margin = new System.Windows.Forms.Padding(4);
            this.btnChangePaymentType.Name = "btnChangePaymentType";
            this.btnChangePaymentType.Size = new System.Drawing.Size(125, 50);
            this.btnChangePaymentType.TabIndex = 94;
            this.btnChangePaymentType.Text = "CHANGE PAYMENT";
            this.btnChangePaymentType.UseVisualStyleBackColor = true;
            this.btnChangePaymentType.Visible = false;
            this.btnChangePaymentType.Click += new System.EventHandler(this.btnChangePaymentType_Click);
            // 
            // pbLoading
            // 
            this.pbLoading.Image = global::WinPowerPOS.Properties.Resources.progressbar_long_green;
            this.pbLoading.Location = new System.Drawing.Point(12, 118);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(402, 15);
            this.pbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLoading.TabIndex = 97;
            this.pbLoading.TabStop = false;
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(12, 74);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(403, 41);
            this.lblError.TabIndex = 98;
            this.lblError.Text = "[status]";
            this.lblError.Visible = false;
            // 
            // frmPaymentIntegration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 215);
            this.ControlBox = false;
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblPaymentAmount);
            this.Controls.Add(this.lblPaymentType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.btnManual);
            this.Controls.Add(this.btnChangePaymentType);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmPaymentIntegration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payment Integration";
            this.Shown += new System.EventHandler(this.frmPaymentIntegration_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnRetry;
        internal System.Windows.Forms.Button btnManual;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPaymentType;
        private System.Windows.Forms.Label lblPaymentAmount;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker worker;
        internal System.Windows.Forms.Button btnChangePaymentType;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.Label lblError;
    }
}