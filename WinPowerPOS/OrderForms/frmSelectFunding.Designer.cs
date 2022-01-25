namespace WinPowerPOS.OrderForms
{
    partial class frmSelectFunding
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectFunding));
            this.btnNoFunding = new System.Windows.Forms.Button();
            this.btnPAMed = new System.Windows.Forms.Button();
            this.btnSMF = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNoFunding
            // 
            this.btnNoFunding.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNoFunding.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNoFunding.BackgroundImage")));
            this.btnNoFunding.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnNoFunding.ForeColor = System.Drawing.Color.White;
            this.btnNoFunding.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNoFunding.Location = new System.Drawing.Point(8, 8);
            this.btnNoFunding.Margin = new System.Windows.Forms.Padding(8);
            this.btnNoFunding.Name = "btnNoFunding";
            this.btnNoFunding.Size = new System.Drawing.Size(133, 74);
            this.btnNoFunding.TabIndex = 27;
            this.btnNoFunding.Text = "No Funding";
            this.btnNoFunding.UseVisualStyleBackColor = true;
            this.btnNoFunding.Click += new System.EventHandler(this.btnNoFunding_Click);
            // 
            // btnPAMed
            // 
            this.btnPAMed.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPAMed.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnPAMed.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnPAMed.ForeColor = System.Drawing.Color.White;
            this.btnPAMed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPAMed.Location = new System.Drawing.Point(157, 8);
            this.btnPAMed.Margin = new System.Windows.Forms.Padding(8);
            this.btnPAMed.Name = "btnPAMed";
            this.btnPAMed.Size = new System.Drawing.Size(133, 74);
            this.btnPAMed.TabIndex = 28;
            this.btnPAMed.Tag = "1";
            this.btnPAMed.Text = "PA Medifund";
            this.btnPAMed.UseVisualStyleBackColor = true;
            this.btnPAMed.Click += new System.EventHandler(this.btnPAMed_Click);
            // 
            // btnSMF
            // 
            this.btnSMF.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSMF.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnSMF.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSMF.ForeColor = System.Drawing.Color.White;
            this.btnSMF.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSMF.Location = new System.Drawing.Point(306, 8);
            this.btnSMF.Margin = new System.Windows.Forms.Padding(8);
            this.btnSMF.Name = "btnSMF";
            this.btnSMF.Size = new System.Drawing.Size(133, 74);
            this.btnSMF.TabIndex = 29;
            this.btnSMF.Tag = "1";
            this.btnSMF.Text = "SMF";
            this.btnSMF.UseVisualStyleBackColor = true;
            this.btnSMF.Click += new System.EventHandler(this.btnSMF_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.btnNoFunding);
            this.flowLayoutPanel1.Controls.Add(this.btnPAMed);
            this.flowLayoutPanel1.Controls.Add(this.btnSMF);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(9, 31);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(447, 90);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(465, 153);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // frmSelectFunding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(465, 153);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectFunding";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Funding Method";
            this.Load += new System.EventHandler(this.frmSelectFunding_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnNoFunding;
        internal System.Windows.Forms.Button btnSMF;
        internal System.Windows.Forms.Button btnPAMed;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;



    }
}