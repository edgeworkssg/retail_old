using AppointmentBook;

namespace WinPowerPOS.AppointmentForms
{
	partial class frmAppointmentManager2
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
            AppointmentBook.Model.AppointmentBookData appointmentBookData1 = new AppointmentBook.Model.AppointmentBookData();
            AppointmentBook.AppointmentBookControlOptions appointmentBookControlOptions1 = new AppointmentBook.AppointmentBookControlOptions();
            AppointmentBook.Model.AppointmentBookData appointmentBookData2 = new AppointmentBook.Model.AppointmentBookData();
            AppointmentBook.AppointmentBookControlOptions appointmentBookControlOptions2 = new AppointmentBook.AppointmentBookControlOptions();
            this.btnClose = new System.Windows.Forms.Button();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newAppointmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteAppointmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.createInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.editAppointmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddAppointment = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbSearch = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateControl = new AppointmentBook.DateControl();
            this.appointmentBookControl = new AppointmentBook.AppointmentBookControl();
            this.appointmentBookResourceControl = new AppointmentBook.AppointmentBookResourceControl();
            this.btnAppointmentView = new System.Windows.Forms.Button();
            this.btnRoomView = new System.Windows.Forms.Button();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.contextMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1024, 413);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 48);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newAppointmentToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteAppointmentToolStripMenuItem,
            this.toolStripMenuItem1,
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.createInvoiceToolStripMenuItem,
            this.showInvoiceToolStripMenuItem,
            this.toolStripMenuItem3,
            this.editAppointmentToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(173, 204);
            // 
            // newAppointmentToolStripMenuItem
            // 
            this.newAppointmentToolStripMenuItem.Name = "newAppointmentToolStripMenuItem";
            this.newAppointmentToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.newAppointmentToolStripMenuItem.Text = "New Appointment";
            this.newAppointmentToolStripMenuItem.Click += new System.EventHandler(this.newAppointmentToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // deleteAppointmentToolStripMenuItem
            // 
            this.deleteAppointmentToolStripMenuItem.Name = "deleteAppointmentToolStripMenuItem";
            this.deleteAppointmentToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteAppointmentToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.deleteAppointmentToolStripMenuItem.Text = "Delete";
            this.deleteAppointmentToolStripMenuItem.Click += new System.EventHandler(this.deleteAppointmentToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(169, 6);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(169, 6);
            // 
            // createInvoiceToolStripMenuItem
            // 
            this.createInvoiceToolStripMenuItem.Name = "createInvoiceToolStripMenuItem";
            this.createInvoiceToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.createInvoiceToolStripMenuItem.Text = "Create Invoice";
            this.createInvoiceToolStripMenuItem.Click += new System.EventHandler(this.createInvoiceToolStripMenuItem_Click);
            // 
            // showInvoiceToolStripMenuItem
            // 
            this.showInvoiceToolStripMenuItem.Name = "showInvoiceToolStripMenuItem";
            this.showInvoiceToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.showInvoiceToolStripMenuItem.Text = "Show Invoice";
            this.showInvoiceToolStripMenuItem.Click += new System.EventHandler(this.showInvoiceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(169, 6);
            // 
            // editAppointmentToolStripMenuItem
            // 
            this.editAppointmentToolStripMenuItem.Name = "editAppointmentToolStripMenuItem";
            this.editAppointmentToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.editAppointmentToolStripMenuItem.Text = "Properties";
            this.editAppointmentToolStripMenuItem.Click += new System.EventHandler(this.editAppointmentToolStripMenuItem_Click);
            // 
            // btnAddAppointment
            // 
            this.btnAddAppointment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddAppointment.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnAddAppointment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddAppointment.ForeColor = System.Drawing.Color.White;
            this.btnAddAppointment.Location = new System.Drawing.Point(21, 413);
            this.btnAddAppointment.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddAppointment.Name = "btnAddAppointment";
            this.btnAddAppointment.Size = new System.Drawing.Size(200, 48);
            this.btnAddAppointment.TabIndex = 3;
            this.btnAddAppointment.Text = "NEW APPOINTMENT...";
            this.btnAddAppointment.UseVisualStyleBackColor = true;
            this.btnAddAppointment.Click += new System.EventHandler(this.btnAddAppointment_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.cbSearch);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(539, 413);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(345, 48);
            this.panel1.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(257, -1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 48);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSearch
            // 
            this.cbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearch.FormattingEnabled = true;
            this.cbSearch.Location = new System.Drawing.Point(133, 21);
            this.cbSearch.Name = "cbSearch";
            this.cbSearch.Size = new System.Drawing.Size(123, 24);
            this.cbSearch.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(2, 21);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(128, 22);
            this.txtSearch.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Appointment Search";
            // 
            // dateControl
            // 
            this.dateControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateControl.BackColor = System.Drawing.SystemColors.Control;
            this.dateControl.Day = new System.DateTime(2014, 1, 28, 0, 0, 0, 0);
            this.dateControl.Format = "dddd";
            this.dateControl.Location = new System.Drawing.Point(225, 414);
            this.dateControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateControl.MaximumSize = new System.Drawing.Size(400, 50);
            this.dateControl.MinimumSize = new System.Drawing.Size(309, 46);
            this.dateControl.Name = "dateControl";
            this.dateControl.Size = new System.Drawing.Size(309, 46);
            this.dateControl.TabIndex = 4;
            this.dateControl.Changed += new System.EventHandler(this.dateControl_Changed);
            // 
            // appointmentBookControl
            // 
            this.appointmentBookControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.appointmentBookControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.appointmentBookControl.ContextMenuStrip = this.contextMenu;
            this.appointmentBookControl.Data = appointmentBookData1;
            this.appointmentBookControl.Day = new System.DateTime(2014, 1, 24, 0, 0, 0, 0);
            this.appointmentBookControl.Location = new System.Drawing.Point(21, 14);
            this.appointmentBookControl.Margin = new System.Windows.Forms.Padding(4);
            this.appointmentBookControl.Name = "appointmentBookControl";
            this.appointmentBookControl.Options = appointmentBookControlOptions1;
            this.appointmentBookControl.Size = new System.Drawing.Size(1123, 390);
            this.appointmentBookControl.TabIndex = 0;
            this.appointmentBookControl.TabStop = false;
            // 
            // appointmentBookResourceControl
            // 
            this.appointmentBookResourceControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.appointmentBookResourceControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.appointmentBookResourceControl.ContextMenuStrip = this.contextMenu;
            this.appointmentBookResourceControl.Data = appointmentBookData2;
            this.appointmentBookResourceControl.Day = new System.DateTime(2014, 1, 24, 0, 0, 0, 0);
            this.appointmentBookResourceControl.Location = new System.Drawing.Point(21, 14);
            this.appointmentBookResourceControl.Margin = new System.Windows.Forms.Padding(4);
            this.appointmentBookResourceControl.Name = "appointmentBookResourceControl";
            this.appointmentBookResourceControl.Options = appointmentBookControlOptions2;
            this.appointmentBookResourceControl.Size = new System.Drawing.Size(1123, 390);
            this.appointmentBookResourceControl.TabIndex = 6;
            // 
            // btnAppointmentView
            // 
            this.btnAppointmentView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppointmentView.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.btnAppointmentView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAppointmentView.ForeColor = System.Drawing.Color.White;
            this.btnAppointmentView.Location = new System.Drawing.Point(894, 412);
            this.btnAppointmentView.Name = "btnAppointmentView";
            this.btnAppointmentView.Size = new System.Drawing.Size(130, 48);
            this.btnAppointmentView.TabIndex = 5;
            this.btnAppointmentView.Text = "APPOINTMENT VIEW";
            this.btnAppointmentView.UseVisualStyleBackColor = true;
            this.btnAppointmentView.Click += new System.EventHandler(this.btnAppointmentView_Click);
            // 
            // btnRoomView
            // 
            this.btnRoomView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRoomView.BackgroundImage = global::WinPowerPOS.Properties.Resources.lightorange;
            this.btnRoomView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRoomView.ForeColor = System.Drawing.Color.White;
            this.btnRoomView.Location = new System.Drawing.Point(894, 413);
            this.btnRoomView.Name = "btnRoomView";
            this.btnRoomView.Size = new System.Drawing.Size(130, 48);
            this.btnRoomView.TabIndex = 4;
            this.btnRoomView.Text = "ROOM VIEW";
            this.btnRoomView.UseVisualStyleBackColor = true;
            this.btnRoomView.Click += new System.EventHandler(this.btnRoomView_Click);
            // 
            // pnlProgress
            // 
            this.pnlProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Location = new System.Drawing.Point(482, 190);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(200, 100);
            this.pnlProgress.TabIndex = 72;
            this.pnlProgress.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(55, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Please Wait...";
            // 
            // pgb1
            // 
            this.pgb1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pgb1.Location = new System.Drawing.Point(25, 42);
            this.pgb1.Maximum = 300;
            this.pgb1.Name = "pgb1";
            this.pgb1.Size = new System.Drawing.Size(159, 23);
            this.pgb1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgb1.TabIndex = 0;
            // 
            // frmAppointmentManager2
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1165, 480);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.btnAppointmentView);
            this.Controls.Add(this.btnRoomView);
            this.Controls.Add(this.dateControl);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.appointmentBookControl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnAddAppointment);
            this.Controls.Add(this.appointmentBookResourceControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(720, 480);
            this.Name = "frmAppointmentManager2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Appointment Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAppointmentManager2_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAppointmentManager2_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAppointmentManager2_KeyDown);
            this.contextMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private AppointmentBookControl appointmentBookControl;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem newAppointmentToolStripMenuItem;
		private System.Windows.Forms.Button btnAddAppointment;
		private System.Windows.Forms.ToolStripMenuItem deleteAppointmentToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editAppointmentToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private DateControl dateControl;
        private System.Windows.Forms.ToolStripMenuItem createInvoiceToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem showInvoiceToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbSearch;
        private AppointmentBookResourceControl appointmentBookResourceControl;
        private System.Windows.Forms.Button btnAppointmentView;
        private System.Windows.Forms.Button btnRoomView;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
	}
}