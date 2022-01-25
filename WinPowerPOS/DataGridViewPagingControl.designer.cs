namespace WinPowerPOS
{
	partial class DataGridViewPagingControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnFirstPage = new System.Windows.Forms.Button();
			this.btnPreviousPage = new System.Windows.Forms.Button();
			this.btnNextPage = new System.Windows.Forms.Button();
			this.btnLastPage = new System.Windows.Forms.Button();
			this.btnPage = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnFirstPage
			// 
			this.btnFirstPage.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnFirstPage.Location = new System.Drawing.Point(0, 0);
			this.btnFirstPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnFirstPage.Name = "btnFirstPage";
			this.btnFirstPage.Size = new System.Drawing.Size(48, 48);
			this.btnFirstPage.TabIndex = 0;
			this.btnFirstPage.Text = "|<";
			this.btnFirstPage.UseVisualStyleBackColor = true;
			this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
			// 
			// btnPreviousPage
			// 
			this.btnPreviousPage.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnPreviousPage.Location = new System.Drawing.Point(52, 0);
			this.btnPreviousPage.Margin = new System.Windows.Forms.Padding(4);
			this.btnPreviousPage.Name = "btnPreviousPage";
			this.btnPreviousPage.Size = new System.Drawing.Size(48, 48);
			this.btnPreviousPage.TabIndex = 1;
			this.btnPreviousPage.Text = "<";
			this.btnPreviousPage.UseVisualStyleBackColor = true;
			this.btnPreviousPage.Click += new System.EventHandler(this.btnPreviousPage_Click);
			// 
			// btnNextPage
			// 
			this.btnNextPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNextPage.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnNextPage.Location = new System.Drawing.Point(319, 0);
			this.btnNextPage.Margin = new System.Windows.Forms.Padding(4);
			this.btnNextPage.Name = "btnNextPage";
			this.btnNextPage.Size = new System.Drawing.Size(48, 48);
			this.btnNextPage.TabIndex = 2;
			this.btnNextPage.Text = ">";
			this.btnNextPage.UseVisualStyleBackColor = true;
			this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
			// 
			// btnLastPage
			// 
			this.btnLastPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLastPage.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnLastPage.Location = new System.Drawing.Point(371, 0);
			this.btnLastPage.Margin = new System.Windows.Forms.Padding(4);
			this.btnLastPage.Name = "btnLastPage";
			this.btnLastPage.Size = new System.Drawing.Size(48, 48);
			this.btnLastPage.TabIndex = 3;
			this.btnLastPage.Text = ">|";
			this.btnLastPage.UseVisualStyleBackColor = true;
			this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
			// 
			// btnPage
			// 
			this.btnPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnPage.Enabled = false;
			this.btnPage.Location = new System.Drawing.Point(108, 0);
			this.btnPage.Margin = new System.Windows.Forms.Padding(4);
			this.btnPage.Name = "btnPage";
			this.btnPage.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.btnPage.Size = new System.Drawing.Size(203, 48);
			this.btnPage.TabIndex = 5;
			this.btnPage.Text = "Page 1 of 100";
			this.btnPage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnPage.UseVisualStyleBackColor = true;
			// 
			// DataGridViewPagingControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnPage);
			this.Controls.Add(this.btnLastPage);
			this.Controls.Add(this.btnNextPage);
			this.Controls.Add(this.btnPreviousPage);
			this.Controls.Add(this.btnFirstPage);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "DataGridViewPagingControl";
			this.Size = new System.Drawing.Size(419, 49);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnFirstPage;
		private System.Windows.Forms.Button btnPreviousPage;
		private System.Windows.Forms.Button btnNextPage;
		private System.Windows.Forms.Button btnLastPage;
		private System.Windows.Forms.Button btnPage;
	}
}
