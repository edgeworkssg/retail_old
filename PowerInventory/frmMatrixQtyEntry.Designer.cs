namespace PowerInventory
{
    partial class frmMatrixQtyEntry
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
            this.btnAddItems = new System.Windows.Forms.Button();
            this.btnCancelAdd = new System.Windows.Forms.Button();
            this.dgvMatrix = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblItemName = new System.Windows.Forms.Label();
            this.lblItemNo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatrix)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddItems
            // 
            this.btnAddItems.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItems.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnAddItems.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddItems.ForeColor = System.Drawing.Color.White;
            this.btnAddItems.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddItems.Location = new System.Drawing.Point(530, 283);
            this.btnAddItems.Name = "btnAddItems";
            this.btnAddItems.Size = new System.Drawing.Size(89, 41);
            this.btnAddItems.TabIndex = 23;
            this.btnAddItems.Text = "SAVE";
            this.btnAddItems.UseVisualStyleBackColor = false;
            this.btnAddItems.Click += new System.EventHandler(this.btnAddItems_Click);
            // 
            // btnCancelAdd
            // 
            this.btnCancelAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelAdd.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
            this.btnCancelAdd.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelAdd.ForeColor = System.Drawing.Color.White;
            this.btnCancelAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelAdd.Location = new System.Drawing.Point(435, 283);
            this.btnCancelAdd.Name = "btnCancelAdd";
            this.btnCancelAdd.Size = new System.Drawing.Size(89, 41);
            this.btnCancelAdd.TabIndex = 22;
            this.btnCancelAdd.Text = "CANCEL";
            this.btnCancelAdd.UseVisualStyleBackColor = false;
            this.btnCancelAdd.Click += new System.EventHandler(this.btnCancelAdd_Click);
            // 
            // dgvMatrix
            // 
            this.dgvMatrix.AllowUserToAddRows = false;
            this.dgvMatrix.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMatrix.Location = new System.Drawing.Point(15, 86);
            this.dgvMatrix.Name = "dgvMatrix";
            this.dgvMatrix.Size = new System.Drawing.Size(604, 191);
            this.dgvMatrix.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 25;
            this.label1.Text = "Item Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(364, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 26;
            this.label2.Text = "Item No";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 27;
            this.label3.Text = "Description";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(115, 49);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(0, 16);
            this.lblDescription.TabIndex = 28;
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemName.Location = new System.Drawing.Point(115, 13);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(0, 16);
            this.lblItemName.TabIndex = 29;
            // 
            // lblItemNo
            // 
            this.lblItemNo.AutoSize = true;
            this.lblItemNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemNo.Location = new System.Drawing.Point(432, 13);
            this.lblItemNo.Name = "lblItemNo";
            this.lblItemNo.Size = new System.Drawing.Size(0, 16);
            this.lblItemNo.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(25, 283);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(207, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "*Note = Grey color meaning item is deleted";
            // 
            // frmMatrixQtyEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 336);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblItemNo);
            this.Controls.Add(this.lblItemName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvMatrix);
            this.Controls.Add(this.btnAddItems);
            this.Controls.Add(this.btnCancelAdd);
            this.Name = "frmMatrixQtyEntry";
            this.Text = "Enter Quantity";
            this.Load += new System.EventHandler(this.frmMatrixItem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatrix)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnAddItems;
        internal System.Windows.Forms.Button btnCancelAdd;
        private System.Windows.Forms.DataGridView dgvMatrix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.Label lblItemNo;
        private System.Windows.Forms.Label label4;
    }
}