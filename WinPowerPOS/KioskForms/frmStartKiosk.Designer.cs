namespace WinPowerPOS.KioskForms
{
    partial class frmStartKiosk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStartKiosk));
            this.btnSetting = new System.Windows.Forms.Button();
            this.lblDisabledMode = new System.Windows.Forms.Label();
            this.bwWM = new System.ComponentModel.BackgroundWorker();
            this.btnLog = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timerAllAvailable = new System.Windows.Forms.Timer(this.components);
            this.ButtonLanguageElement = new System.Windows.Forms.Integration.ElementHost();
            this.btnLang = new WinPowerPOS.KioskForms.LangButton();
            this.ButtonStartElement = new System.Windows.Forms.Integration.ElementHost();
            this.btnStart = new WinPowerPOS.KioskForms.StartKioskButton();
            this.hostVideo = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlVideo = new WinPowerPOS.KioskForms.StartVideoControl();
            this.hostScanner = new System.Windows.Forms.Integration.ElementHost();
            this.ctrlScanner = new WinPowerPOS.KioskForms.ScanBarcode();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSetting
            // 
            this.btnSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnSetting.Image")));
            this.btnSetting.Location = new System.Drawing.Point(932, 676);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(80, 80);
            this.btnSetting.TabIndex = 2;
            this.btnSetting.TabStop = false;
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // lblDisabledMode
            // 
            this.lblDisabledMode.BackColor = System.Drawing.Color.Black;
            this.lblDisabledMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDisabledMode.Font = new System.Drawing.Font("Calibri", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisabledMode.ForeColor = System.Drawing.Color.White;
            this.lblDisabledMode.Location = new System.Drawing.Point(123, 497);
            this.lblDisabledMode.Name = "lblDisabledMode";
            this.lblDisabledMode.Size = new System.Drawing.Size(745, 170);
            this.lblDisabledMode.TabIndex = 3;
            this.lblDisabledMode.Text = "This machine is temporarily out of service";
            this.lblDisabledMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDisabledMode.Visible = false;
            // 
            // bwWM
            // 
            this.bwWM.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwWM_DoWork);
            // 
            // btnLog
            // 
            this.btnLog.AutoSize = true;
            this.btnLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLog.Location = new System.Drawing.Point(932, 12);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(76, 39);
            this.btnLog.TabIndex = 6;
            this.btnLog.TabStop = false;
            this.btnLog.Text = "LOG";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(509, 12);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(417, 482);
            this.txtLog.TabIndex = 5;
            this.txtLog.TabStop = false;
            this.txtLog.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Black;
            this.lblStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(12, 737);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(147, 23);
            this.lblStatus.TabIndex = 7;
            // 
            // timerAllAvailable
            // 
            this.timerAllAvailable.Enabled = true;
            this.timerAllAvailable.Interval = 1000;
            this.timerAllAvailable.Tick += new System.EventHandler(this.timerAllAvailable_Tick);
            // 
            // ButtonLanguageElement
            // 
            this.ButtonLanguageElement.Location = new System.Drawing.Point(689, 543);
            this.ButtonLanguageElement.Name = "ButtonLanguageElement";
            this.ButtonLanguageElement.Size = new System.Drawing.Size(102, 100);
            this.ButtonLanguageElement.TabIndex = 8;
            this.ButtonLanguageElement.Text = "elementHost3";
            this.ButtonLanguageElement.Child = this.btnLang;
            // 
            // ButtonStartElement
            // 
            this.ButtonStartElement.BackColor = System.Drawing.Color.Black;
            this.ButtonStartElement.Location = new System.Drawing.Point(363, 543);
            this.ButtonStartElement.Name = "ButtonStartElement";
            this.ButtonStartElement.Size = new System.Drawing.Size(298, 100);
            this.ButtonStartElement.TabIndex = 0;
            this.ButtonStartElement.TabStop = false;
            this.ButtonStartElement.Text = "elementHost1";
            this.ButtonStartElement.Child = this.btnStart;
            // 
            // hostVideo
            // 
            this.hostVideo.BackColor = System.Drawing.Color.Black;
            this.hostVideo.Location = new System.Drawing.Point(0, 0);
            this.hostVideo.Name = "hostVideo";
            this.hostVideo.Size = new System.Drawing.Size(1024, 768);
            this.hostVideo.TabIndex = 1;
            this.hostVideo.Text = "elementHost2";
            this.hostVideo.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.hostVideo_ChildChanged);
            this.hostVideo.Child = this.ctrlVideo;
            // 
            // hostScanner
            // 
            this.hostScanner.Location = new System.Drawing.Point(328, 301);
            this.hostScanner.Name = "hostScanner";
            this.hostScanner.Size = new System.Drawing.Size(445, 215);
            this.hostScanner.TabIndex = 9;
            this.hostScanner.Text = "elementHost1";
            this.hostScanner.Visible = false;
            this.hostScanner.Child = this.ctrlScanner;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(-100, 230);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(100, 20);
            this.txtBarcode.TabIndex = 10;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // frmStartKiosk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.hostScanner);
            this.Controls.Add(this.lblDisabledMode);
            this.Controls.Add(this.ButtonLanguageElement);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.ButtonStartElement);
            this.Controls.Add(this.hostVideo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmStartKiosk";
            this.Text = "frmStartKiosk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmStartKiosk_Load);
            this.Activated += new System.EventHandler(this.frmStartKiosk_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStartKiosk_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost ButtonStartElement;
        private System.Windows.Forms.Integration.ElementHost hostVideo;
        private StartKioskButton btnStart;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Label lblDisabledMode;
        private System.ComponentModel.BackgroundWorker bwWM;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer timerAllAvailable;
        private StartVideoControl ctrlVideo;
        private System.Windows.Forms.Integration.ElementHost ButtonLanguageElement;
        private LangButton btnLang;
        private System.Windows.Forms.Integration.ElementHost hostScanner;
        private ScanBarcode ctrlScanner;
        private System.Windows.Forms.TextBox txtBarcode;
    }
}