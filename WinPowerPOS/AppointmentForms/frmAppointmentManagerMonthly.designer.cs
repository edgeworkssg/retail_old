namespace WinPowerPOS.AppointmentForms
{
    partial class frmAppointmentManagerMonthly
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
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newAppointmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteAppointmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.createInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.editAppointmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddAppointment = new System.Windows.Forms.Button();
            this.lblUserGroup = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.appointmentBookControl = new AppointmentBook.AppointmentBookMonthly();
            this.dateControl = new AppointmentBook.DateControlMonthly();
            this.btnWeekly = new System.Windows.Forms.Button();
            this.btnMonthly = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.cmbUser = new WinPowerPOS.CheckedComboBox();
            this.cmbUserGroup = new WinPowerPOS.CheckedComboBox();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pgb1 = new System.Windows.Forms.ProgressBar();
            this.contextMenu.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newAppointmentToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteAppointmentToolStripMenuItem,
            this.toolStripSeparator3,
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator1,
            this.createInvoiceToolStripMenuItem,
            this.showInvoiceToolStripMenuItem,
            this.toolStripSeparator4,
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(169, 6);
            // 
            // deleteAppointmentToolStripMenuItem
            // 
            this.deleteAppointmentToolStripMenuItem.Name = "deleteAppointmentToolStripMenuItem";
            this.deleteAppointmentToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteAppointmentToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.deleteAppointmentToolStripMenuItem.Text = "Delete";
            this.deleteAppointmentToolStripMenuItem.Click += new System.EventHandler(this.deleteAppointmentToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(169, 6);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
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
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(169, 6);
            // 
            // editAppointmentToolStripMenuItem
            // 
            this.editAppointmentToolStripMenuItem.Name = "editAppointmentToolStripMenuItem";
            this.editAppointmentToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.editAppointmentToolStripMenuItem.Text = "Properties";
            this.editAppointmentToolStripMenuItem.Click += new System.EventHandler(this.editAppointmentToolStripMenuItem_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(954, 11);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(125, 48);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnAddAppointment
            // 
            this.btnAddAppointment.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnAddAppointment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddAppointment.ForeColor = System.Drawing.Color.White;
            this.btnAddAppointment.Location = new System.Drawing.Point(4, 11);
            this.btnAddAppointment.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddAppointment.Name = "btnAddAppointment";
            this.btnAddAppointment.Size = new System.Drawing.Size(231, 48);
            this.btnAddAppointment.TabIndex = 6;
            this.btnAddAppointment.Text = "NEW APPOINTMENT...";
            this.btnAddAppointment.UseVisualStyleBackColor = true;
            this.btnAddAppointment.Click += new System.EventHandler(this.btnAddAppointment_Click);
            // 
            // lblUserGroup
            // 
            this.lblUserGroup.AutoSize = true;
            this.lblUserGroup.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserGroup.Location = new System.Drawing.Point(609, 43);
            this.lblUserGroup.Name = "lblUserGroup";
            this.lblUserGroup.Size = new System.Drawing.Size(79, 16);
            this.lblUserGroup.TabIndex = 9;
            this.lblUserGroup.Text = "User Group";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(609, 14);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(36, 16);
            this.lblUser.TabIndex = 11;
            this.lblUser.Text = "User";
            // 
            // appointmentBookControl
            // 
            this.appointmentBookControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.appointmentBookControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.appointmentBookControl.ContextMenuStrip = this.contextMenu;
            this.appointmentBookControl.Data = appointmentBookData1;
            this.appointmentBookControl.Day = new System.DateTime(2014, 10, 30, 0, 0, 0, 0);
            this.appointmentBookControl.Location = new System.Drawing.Point(12, 68);
            this.appointmentBookControl.Name = "appointmentBookControl";
            this.appointmentBookControl.Options = appointmentBookControlOptions1;
            this.appointmentBookControl.Size = new System.Drawing.Size(1067, 400);
            this.appointmentBookControl.TabIndex = 0;
            this.appointmentBookControl.TabStop = false;
            // 
            // dateControl
            // 
            this.dateControl.BackColor = System.Drawing.SystemColors.Control;
            this.dateControl.Day = new System.DateTime(2014, 1, 28, 0, 0, 0, 0);
            this.dateControl.Format = "dddd";
            this.dateControl.Location = new System.Drawing.Point(389, 11);
            this.dateControl.Name = "dateControl";
            this.dateControl.Size = new System.Drawing.Size(214, 48);
            this.dateControl.TabIndex = 7;
            this.dateControl.Changed += new System.EventHandler(this.dateControl_Changed);
            // 
            // btnWeekly
            // 
            this.btnWeekly.Location = new System.Drawing.Point(869, 12);
            this.btnWeekly.Name = "btnWeekly";
            this.btnWeekly.Size = new System.Drawing.Size(75, 25);
            this.btnWeekly.TabIndex = 14;
            this.btnWeekly.Text = "Weekly";
            this.btnWeekly.UseVisualStyleBackColor = true;
            this.btnWeekly.Visible = false;
            this.btnWeekly.Click += new System.EventHandler(this.btnWeekly_Click);
            // 
            // btnMonthly
            // 
            this.btnMonthly.Enabled = false;
            this.btnMonthly.Location = new System.Drawing.Point(869, 43);
            this.btnMonthly.Name = "btnMonthly";
            this.btnMonthly.Size = new System.Drawing.Size(75, 25);
            this.btnMonthly.TabIndex = 15;
            this.btnMonthly.Text = "Monthly";
            this.btnMonthly.UseVisualStyleBackColor = true;
            this.btnMonthly.Visible = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(243, 11);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(86, 48);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "DELETE";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cmbUser
            // 
            this.cmbUser.CheckOnClick = true;
            this.cmbUser.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbUser.DropDownHeight = 1;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.IntegralHeight = false;
            this.cmbUser.Location = new System.Drawing.Point(698, 40);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(165, 21);
            this.cmbUser.TabIndex = 10;
            this.cmbUser.ValueSeparator = ", ";
            this.cmbUser.DropDownClosed += new System.EventHandler(this.cmbUser_DropDownClosed);
            // 
            // cmbUserGroup
            // 
            this.cmbUserGroup.CheckOnClick = true;
            this.cmbUserGroup.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbUserGroup.DropDownHeight = 1;
            this.cmbUserGroup.FormattingEnabled = true;
            this.cmbUserGroup.IntegralHeight = false;
            this.cmbUserGroup.Location = new System.Drawing.Point(698, 13);
            this.cmbUserGroup.Name = "cmbUserGroup";
            this.cmbUserGroup.Size = new System.Drawing.Size(165, 21);
            this.cmbUserGroup.TabIndex = 8;
            this.cmbUserGroup.ValueSeparator = ", ";
            this.cmbUserGroup.DropDownClosed += new System.EventHandler(this.cmbUserGroup_DropDownClosed);
            // 
            // pnlProgress
            // 
            this.pnlProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Controls.Add(this.pgb1);
            this.pnlProgress.Location = new System.Drawing.Point(450, 190);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(200, 100);
            this.pnlProgress.TabIndex = 71;
            this.pnlProgress.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(55, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
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
            // frmAppointmentManagerMonthly
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1100, 480);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.btnWeekly);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnMonthly);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.appointmentBookControl);
            this.Controls.Add(this.cmbUser);
            this.Controls.Add(this.btnAddAppointment);
            this.Controls.Add(this.cmbUserGroup);
            this.Controls.Add(this.lblUserGroup);
            this.Controls.Add(this.dateControl);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "frmAppointmentManagerMonthly";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Appointment Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAppointmentManagerWeekly_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAppointmentManagerWeekly_KeyDown);
            this.contextMenu.ResumeLayout(false);
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AppointmentBook.AppointmentBookMonthly appointmentBookControl;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem newAppointmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAppointmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createInvoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showInvoiceToolStripMenuItem;
        private AppointmentBook.DateControlMonthly dateControl;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddAppointment;
        private System.Windows.Forms.ToolStripMenuItem editAppointmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private CheckedComboBox cmbUserGroup;
        private System.Windows.Forms.Label lblUserGroup;
        private System.Windows.Forms.Label lblUser;
        private CheckedComboBox cmbUser;
        private System.Windows.Forms.Button btnWeekly;
        private System.Windows.Forms.Button btnMonthly;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgb1;
    }
}