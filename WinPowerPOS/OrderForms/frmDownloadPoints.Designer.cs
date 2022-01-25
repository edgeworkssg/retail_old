namespace WinPowerPOS.OrderForms
{
    partial class frmDownloadPoints
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.bgDownloadPoints = new System.ComponentModel.BackgroundWorker();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.lblLoadingMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(307, 94);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(123, 44);
            this.btnCancel.TabIndex = 44;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // bgDownloadPoints
            // 
            this.bgDownloadPoints.WorkerSupportsCancellation = true;
            this.bgDownloadPoints.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDownloadPoints_DoWork);
            this.bgDownloadPoints.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgDownloadPoints_RunWorkerCompleted);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label36.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label36.Location = new System.Drawing.Point(101, 56);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(198, 16);
            this.label36.TabIndex = 47;
            this.label36.Text = "Downloading Points Data....";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label35.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label35.Location = new System.Drawing.Point(101, 72);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(214, 16);
            this.label35.TabIndex = 46;
            this.label35.Text = "This may take a few minutes...";
            // 
            // lblLoadingMessage
            // 
            this.lblLoadingMessage.AutoSize = true;
            this.lblLoadingMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblLoadingMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblLoadingMessage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblLoadingMessage.Location = new System.Drawing.Point(98, 25);
            this.lblLoadingMessage.Name = "lblLoadingMessage";
            this.lblLoadingMessage.Size = new System.Drawing.Size(232, 31);
            this.lblLoadingMessage.TabIndex = 45;
            this.lblLoadingMessage.Text = "PLEASE WAIT...";
            // 
            // frmDownloadPoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(435, 140);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.lblLoadingMessage);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmDownloadPoints";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmSalesPerson_Load);
            this.Shown += new System.EventHandler(this.frmDownloadPoints_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bgDownloadPoints;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label lblLoadingMessage;
    }
}