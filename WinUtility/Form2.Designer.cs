namespace WinUtility
{
    partial class Form2
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.bw1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.txtHardwareName = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNetsComPort = new System.Windows.Forms.TextBox();
            this.btnNetsCheckStatus = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(360, 29);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(659, 156);
            this.textBox1.TabIndex = 3;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(6, 20);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(186, 20);
            this.txtPort.TabIndex = 10;
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(198, 17);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(82, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = "Connect";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 156);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weighing Machine";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(198, 127);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(82, 23);
            this.button7.TabIndex = 12;
            this.button7.Text = "Dispose";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // bw1
            // 
            this.bw1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw1_DoWork);
            this.bw1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw1_RunWorkerCompleted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button14);
            this.groupBox2.Controls.Add(this.button13);
            this.groupBox2.Controls.Add(this.button12);
            this.groupBox2.Controls.Add(this.button11);
            this.groupBox2.Controls.Add(this.button10);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.txtHardwareName);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Location = new System.Drawing.Point(337, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 156);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hardware";
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(112, 104);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(124, 23);
            this.button14.TabIndex = 17;
            this.button14.Text = "New Object";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(242, 75);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(124, 23);
            this.button13.TabIndex = 16;
            this.button13.Text = "Dispose";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(112, 75);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(124, 23);
            this.button12.TabIndex = 15;
            this.button12.Text = "Disable Timer";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(242, 46);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(124, 23);
            this.button11.TabIndex = 14;
            this.button11.Text = "Disable";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(242, 17);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(124, 23);
            this.button10.TabIndex = 13;
            this.button10.Text = "Enable";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(112, 46);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(124, 23);
            this.button8.TabIndex = 12;
            this.button8.Text = "Enable Timer";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // txtHardwareName
            // 
            this.txtHardwareName.Location = new System.Drawing.Point(6, 19);
            this.txtHardwareName.Name = "txtHardwareName";
            this.txtHardwareName.Size = new System.Drawing.Size(100, 20);
            this.txtHardwareName.TabIndex = 10;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(112, 17);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(124, 23);
            this.button9.TabIndex = 11;
            this.button9.Text = "Open Com Port";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtNetsComPort);
            this.groupBox3.Controls.Add(this.btnNetsCheckStatus);
            this.groupBox3.Location = new System.Drawing.Point(711, 191);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 156);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "NETS";
            this.groupBox3.Visible = false;
            // 
            // txtNetsComPort
            // 
            this.txtNetsComPort.Location = new System.Drawing.Point(6, 19);
            this.txtNetsComPort.Name = "txtNetsComPort";
            this.txtNetsComPort.Size = new System.Drawing.Size(100, 20);
            this.txtNetsComPort.TabIndex = 10;
            // 
            // btnNetsCheckStatus
            // 
            this.btnNetsCheckStatus.Location = new System.Drawing.Point(112, 17);
            this.btnNetsCheckStatus.Name = "btnNetsCheckStatus";
            this.btnNetsCheckStatus.Size = new System.Drawing.Size(82, 23);
            this.btnNetsCheckStatus.TabIndex = 11;
            this.btnNetsCheckStatus.Text = "Check Status";
            this.btnNetsCheckStatus.UseVisualStyleBackColor = true;
            this.btnNetsCheckStatus.Click += new System.EventHandler(this.btnNetsCheckStatus_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1655, 564);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form2";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker bw1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox txtHardwareName;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtNetsComPort;
        private System.Windows.Forms.Button btnNetsCheckStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
    }
}

