namespace PowerInventory
{
    partial class frmAddPromoItem
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPromoName = new System.Windows.Forms.Label();
            this.lblPromoCode = new System.Windows.Forms.Label();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.btnAddItems = new System.Windows.Forms.Button();
            this.btnCancelAdd = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "PROMO NAME";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(240, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "PROMO CODE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(429, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "QTY";
            // 
            // lblPromoName
            // 
            this.lblPromoName.AutoSize = true;
            this.lblPromoName.Location = new System.Drawing.Point(28, 48);
            this.lblPromoName.Name = "lblPromoName";
            this.lblPromoName.Size = new System.Drawing.Size(10, 13);
            this.lblPromoName.TabIndex = 3;
            this.lblPromoName.Text = "-";
            // 
            // lblPromoCode
            // 
            this.lblPromoCode.AutoSize = true;
            this.lblPromoCode.Location = new System.Drawing.Point(240, 48);
            this.lblPromoCode.Name = "lblPromoCode";
            this.lblPromoCode.Size = new System.Drawing.Size(10, 13);
            this.lblPromoCode.TabIndex = 4;
            this.lblPromoCode.Text = "-";
            // 
            // txtQty
            // 
            this.txtQty.Location = new System.Drawing.Point(432, 40);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(36, 20);
            this.txtQty.TabIndex = 5;
            this.txtQty.Text = "0";
            this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQty_KeyPress);
            // 
            // btnAddItems
            // 
            this.btnAddItems.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItems.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
            this.btnAddItems.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddItems.ForeColor = System.Drawing.Color.White;
            this.btnAddItems.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddItems.Location = new System.Drawing.Point(432, 84);
            this.btnAddItems.Name = "btnAddItems";
            this.btnAddItems.Size = new System.Drawing.Size(89, 41);
            this.btnAddItems.TabIndex = 25;
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
            this.btnCancelAdd.Location = new System.Drawing.Point(337, 84);
            this.btnCancelAdd.Name = "btnCancelAdd";
            this.btnCancelAdd.Size = new System.Drawing.Size(89, 41);
            this.btnCancelAdd.TabIndex = 24;
            this.btnCancelAdd.Text = "CANCEL";
            this.btnCancelAdd.UseVisualStyleBackColor = false;
            this.btnCancelAdd.Click += new System.EventHandler(this.btnCancelAdd_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(473, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "x";
            // 
            // lblQty
            // 
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new System.Drawing.Point(485, 44);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(13, 13);
            this.lblQty.TabIndex = 27;
            this.lblQty.Text = "0";
            // 
            // frmAddPromoItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 133);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAddItems);
            this.Controls.Add(this.btnCancelAdd);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.lblPromoCode);
            this.Controls.Add(this.lblPromoName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmAddPromoItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Promo Item";
            this.Load += new System.EventHandler(this.frmAddPromoItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPromoName;
        private System.Windows.Forms.Label lblPromoCode;
        private System.Windows.Forms.TextBox txtQty;
        internal System.Windows.Forms.Button btnAddItems;
        internal System.Windows.Forms.Button btnCancelAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblQty;
    }
}