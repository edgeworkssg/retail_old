namespace PowerInventory
{
    partial class frmStockOnHandMatrix
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvMatrix = new System.Windows.Forms.DataGridView();
            this.btnCancelAdd = new System.Windows.Forms.Button();
            this.lblItemName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblItemNo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatrix)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 32;
            this.label3.Text = "Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(361, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 31;
            this.label2.Text = "Item No";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 30;
            this.label1.Text = "Item Name";
            // 
            // dgvMatrix
            // 
            this.dgvMatrix.AllowUserToAddRows = false;
            this.dgvMatrix.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMatrix.Location = new System.Drawing.Point(12, 90);
            this.dgvMatrix.Name = "dgvMatrix";
            this.dgvMatrix.ReadOnly = true;
            this.dgvMatrix.Size = new System.Drawing.Size(604, 191);
            this.dgvMatrix.TabIndex = 29;
            // 
            // btnCancelAdd
            // 
            this.btnCancelAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelAdd.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancelAdd.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelAdd.ForeColor = System.Drawing.Color.White;
            this.btnCancelAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelAdd.Location = new System.Drawing.Point(527, 289);
            this.btnCancelAdd.Name = "btnCancelAdd";
            this.btnCancelAdd.Size = new System.Drawing.Size(89, 41);
            this.btnCancelAdd.TabIndex = 28;
            this.btnCancelAdd.Text = "CLOSE";
            this.btnCancelAdd.UseVisualStyleBackColor = false;
            this.btnCancelAdd.Click += new System.EventHandler(this.btnCancelAdd_Click);
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemName.Location = new System.Drawing.Point(101, 17);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(0, 16);
            this.lblItemName.TabIndex = 33;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(104, 53);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(0, 16);
            this.lblDescription.TabIndex = 34;
            // 
            // lblItemNo
            // 
            this.lblItemNo.AutoSize = true;
            this.lblItemNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemNo.Location = new System.Drawing.Point(421, 17);
            this.lblItemNo.Name = "lblItemNo";
            this.lblItemNo.Size = new System.Drawing.Size(0, 16);
            this.lblItemNo.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(22, 284);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(207, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "*Note = Grey color meaning item is deleted";
            // 
            // frmStockOnHandMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 339);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblItemNo);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblItemName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvMatrix);
            this.Controls.Add(this.btnCancelAdd);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStockOnHandMatrix";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Details";
            this.Load += new System.EventHandler(this.frmStockOnHandMatrix_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatrix)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvMatrix;
        internal System.Windows.Forms.Button btnCancelAdd;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblItemNo;
        private System.Windows.Forms.Label label4;
    }
}