namespace PowerInventory
{
    partial class frmStockTake
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblWait = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // pnlLoading
            // 
            this.pnlLoading.Location = new System.Drawing.Point(-99, -89);
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblWait
            // 
            this.lblWait.AutoSize = true;
            this.lblWait.BackColor = System.Drawing.Color.Black;
            this.lblWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWait.ForeColor = System.Drawing.Color.Yellow;
            this.lblWait.Location = new System.Drawing.Point(321, 350);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(232, 31);
            this.lblWait.TabIndex = 103;
            this.lblWait.Text = "PLEASE WAIT...";
            // 
            // frmStockTake
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.Add(this.lblWait);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "frmStockTake";
            this.Tag = "STOCK TAKE";
            this.Text = "frmStockTake";
            this.Load += new System.EventHandler(this.frmStockTake_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStockTake_FormClosing);
            this.Controls.SetChildIndex(this.btnSaveToDisk, 0);
            this.Controls.SetChildIndex(this.btnPrint, 0);
            this.Controls.SetChildIndex(this.lblWait, 0);
            this.Controls.SetChildIndex(this.tbControl, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnDeleteChecked, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblWait;
    }
}