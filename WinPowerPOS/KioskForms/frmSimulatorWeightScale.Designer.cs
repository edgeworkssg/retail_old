namespace WinPowerPOS.KioskForms
{
    partial class frmSimulatorWeightScale
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
            this.chConnect = new System.Windows.Forms.CheckBox();
            this.lCurrentWeight = new System.Windows.Forms.Label();
            this.bChange = new System.Windows.Forms.Button();
            this.tWeight = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chConnect
            // 
            this.chConnect.AutoSize = true;
            this.chConnect.Location = new System.Drawing.Point(12, 12);
            this.chConnect.Name = "chConnect";
            this.chConnect.Size = new System.Drawing.Size(66, 17);
            this.chConnect.TabIndex = 0;
            this.chConnect.Text = "Connect";
            this.chConnect.UseVisualStyleBackColor = true;
            this.chConnect.CheckedChanged += new System.EventHandler(this.chConnect_CheckedChanged);
            // 
            // lCurrentWeight
            // 
            this.lCurrentWeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lCurrentWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lCurrentWeight.Font = new System.Drawing.Font("Calibri", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCurrentWeight.Location = new System.Drawing.Point(11, 44);
            this.lCurrentWeight.Name = "lCurrentWeight";
            this.lCurrentWeight.Size = new System.Drawing.Size(251, 73);
            this.lCurrentWeight.TabIndex = 1;
            this.lCurrentWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bChange
            // 
            this.bChange.AutoSize = true;
            this.bChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bChange.Location = new System.Drawing.Point(188, 131);
            this.bChange.Name = "bChange";
            this.bChange.Size = new System.Drawing.Size(75, 26);
            this.bChange.TabIndex = 2;
            this.bChange.Text = "Add";
            this.bChange.UseVisualStyleBackColor = true;
            this.bChange.Click += new System.EventHandler(this.bChange_Click);
            // 
            // tWeight
            // 
            this.tWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tWeight.Location = new System.Drawing.Point(12, 133);
            this.tWeight.Name = "tWeight";
            this.tWeight.Size = new System.Drawing.Size(158, 22);
            this.tWeight.TabIndex = 3;
            this.tWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // frmSimulatorWeightScale
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(272, 166);
            this.ControlBox = false;
            this.Controls.Add(this.tWeight);
            this.Controls.Add(this.bChange);
            this.Controls.Add(this.lCurrentWeight);
            this.Controls.Add(this.chConnect);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSimulatorWeightScale";
            this.ShowIcon = false;
            this.Text = "Simulator Weight Scale";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chConnect;
        private System.Windows.Forms.Label lCurrentWeight;
        private System.Windows.Forms.Button bChange;
        private System.Windows.Forms.TextBox tWeight;
    }
}