namespace AppointmentBook
{
    partial class DateControl
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
            this.btnNextDay = new System.Windows.Forms.Button();
            this.btnPreviousDay = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // btnNextDay
            // 
            this.btnNextDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNextDay.Location = new System.Drawing.Point(234, 0);
            this.btnNextDay.Margin = new System.Windows.Forms.Padding(4);
            this.btnNextDay.Name = "btnNextDay";
            this.btnNextDay.Size = new System.Drawing.Size(46, 51);
            this.btnNextDay.TabIndex = 8;
            this.btnNextDay.Text = ">";
            this.btnNextDay.UseVisualStyleBackColor = true;
            this.btnNextDay.Click += new System.EventHandler(this.btnNextDay_Click);
            // 
            // btnPreviousDay
            // 
            this.btnPreviousDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPreviousDay.Location = new System.Drawing.Point(1, 0);
            this.btnPreviousDay.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreviousDay.Name = "btnPreviousDay";
            this.btnPreviousDay.Size = new System.Drawing.Size(46, 51);
            this.btnPreviousDay.TabIndex = 7;
            this.btnPreviousDay.Text = "<";
            this.btnPreviousDay.UseVisualStyleBackColor = true;
            this.btnPreviousDay.Click += new System.EventHandler(this.btnPreviousDay_Click);
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(54, 2);
            this.lblDate.Name = "lblDate";
            this.lblDate.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Size = new System.Drawing.Size(164, 20);
            this.lblDate.TabIndex = 9;
            this.lblDate.Text = "Tuesday";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpDate
            // 
            this.dtpDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpDate.CustomFormat = "ddd, dd/MMM/yyyy";
            this.dtpDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(54, 25);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(173, 22);
            this.dtpDate.TabIndex = 10;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // DateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.btnNextDay);
            this.Controls.Add(this.btnPreviousDay);
            this.Name = "DateControl";
            this.Size = new System.Drawing.Size(282, 55);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnNextDay;
		private System.Windows.Forms.Button btnPreviousDay;
		private System.Windows.Forms.Label lblDate;
		private System.Windows.Forms.DateTimePicker dtpDate;
	}
}
