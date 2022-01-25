namespace AppointmentBook
{
	partial class AppointmentBookControl
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
			this.components = new System.ComponentModel.Container();
			this.vScrollBar = new System.Windows.Forms.VScrollBar();
			this.hScrollBar = new System.Windows.Forms.HScrollBar();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// vScrollBar
			// 
			this.vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.vScrollBar.LargeChange = 40;
			this.vScrollBar.Location = new System.Drawing.Point(463, 89);
			this.vScrollBar.Name = "vScrollBar";
			this.vScrollBar.Size = new System.Drawing.Size(20, 175);
			this.vScrollBar.TabIndex = 0;
			this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
			// 
			// hScrollBar
			// 
			this.hScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.hScrollBar.Location = new System.Drawing.Point(98, 268);
			this.hScrollBar.Name = "hScrollBar";
			this.hScrollBar.Size = new System.Drawing.Size(365, 20);
			this.hScrollBar.TabIndex = 1;
			this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 3000;
			this.toolTip.AutoPopDelay = 3000;
			this.toolTip.InitialDelay = 3000;
			this.toolTip.ReshowDelay = 3000;
			// 
			// AppointmentBookControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.hScrollBar);
			this.Controls.Add(this.vScrollBar);
			this.Name = "AppointmentBookControl";
			this.Size = new System.Drawing.Size(483, 288);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.VScrollBar vScrollBar;
		private System.Windows.Forms.HScrollBar hScrollBar;
		private System.Windows.Forms.ToolTip toolTip;

	}
}
