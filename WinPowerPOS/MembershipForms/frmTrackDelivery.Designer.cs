namespace WinPowerPOS.MembershipForms
{
    partial class frmTrackDelivery
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvTrackDelivery = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label34 = new System.Windows.Forms.Label();
            this.DeliveryOrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delivered = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DOHdrID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDetID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Print = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackDelivery)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.dgvTrackDelivery);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(507, 315);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(9, 256);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(489, 50);
            this.panel3.TabIndex = 11;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(347, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 48);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvTrackDelivery
            // 
            this.dgvTrackDelivery.AllowUserToAddRows = false;
            this.dgvTrackDelivery.AllowUserToDeleteRows = false;
            this.dgvTrackDelivery.AllowUserToResizeColumns = false;
            this.dgvTrackDelivery.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrackDelivery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrackDelivery.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeliveryOrderNo,
            this.DeliveryDate,
            this.DeliveryQty,
            this.DeliveryStatus,
            this.Delivered,
            this.DOHdrID,
            this.OrderDetID,
            this.Print});
            this.dgvTrackDelivery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTrackDelivery.Location = new System.Drawing.Point(9, 50);
            this.dgvTrackDelivery.MultiSelect = false;
            this.dgvTrackDelivery.Name = "dgvTrackDelivery";
            this.dgvTrackDelivery.ReadOnly = true;
            this.dgvTrackDelivery.RowHeadersVisible = false;
            this.dgvTrackDelivery.Size = new System.Drawing.Size(489, 256);
            this.dgvTrackDelivery.TabIndex = 1;
            this.dgvTrackDelivery.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTrackDelivery_DataBindingComplete);
            this.dgvTrackDelivery.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrackDelivery_CellContentClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label34);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(9, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(489, 41);
            this.panel2.TabIndex = 0;
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.label34.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.White;
            this.label34.Location = new System.Drawing.Point(0, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(489, 41);
            this.label34.TabIndex = 23;
            this.label34.Text = "Track Delivery";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeliveryOrderNo
            // 
            this.DeliveryOrderNo.DataPropertyName = "DeliveryNo";
            this.DeliveryOrderNo.HeaderText = "No";
            this.DeliveryOrderNo.Name = "DeliveryOrderNo";
            this.DeliveryOrderNo.ReadOnly = true;
            // 
            // DeliveryDate
            // 
            this.DeliveryDate.DataPropertyName = "DeliveryDate";
            this.DeliveryDate.HeaderText = "Delivery Date";
            this.DeliveryDate.Name = "DeliveryDate";
            this.DeliveryDate.ReadOnly = true;
            // 
            // DeliveryQty
            // 
            this.DeliveryQty.DataPropertyName = "DeliveryQty";
            this.DeliveryQty.HeaderText = "Delivery Qty";
            this.DeliveryQty.Name = "DeliveryQty";
            this.DeliveryQty.ReadOnly = true;
            // 
            // DeliveryStatus
            // 
            this.DeliveryStatus.DataPropertyName = "DeliveryStatus";
            this.DeliveryStatus.HeaderText = "Delivery Status";
            this.DeliveryStatus.Name = "DeliveryStatus";
            this.DeliveryStatus.ReadOnly = true;
            // 
            // Delivered
            // 
            this.Delivered.DataPropertyName = "Delivered";
            this.Delivered.HeaderText = "Delivered";
            this.Delivered.Name = "Delivered";
            this.Delivered.ReadOnly = true;
            this.Delivered.Text = "Delivered";
            this.Delivered.UseColumnTextForButtonValue = true;
            // 
            // DOHdrID
            // 
            this.DOHdrID.DataPropertyName = "DOHdrID";
            this.DOHdrID.HeaderText = "DOHdrID";
            this.DOHdrID.Name = "DOHdrID";
            this.DOHdrID.ReadOnly = true;
            this.DOHdrID.Visible = false;
            // 
            // OrderDetID
            // 
            this.OrderDetID.DataPropertyName = "OrderDetID";
            this.OrderDetID.HeaderText = "OrderDetID";
            this.OrderDetID.Name = "OrderDetID";
            this.OrderDetID.ReadOnly = true;
            this.OrderDetID.Visible = false;
            // 
            // Print
            // 
            this.Print.HeaderText = "Print";
            this.Print.Name = "Print";
            this.Print.ReadOnly = true;
            this.Print.Text = "Print";
            this.Print.UseColumnTextForButtonValue = true;
            // 
            // frmTrackDelivery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.longdarkbg;
            this.ClientSize = new System.Drawing.Size(507, 315);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Name = "frmTrackDelivery";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Track Delivery";
            this.Load += new System.EventHandler(this.frmTrackDelivery_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackDelivery)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvTrackDelivery;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryOrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryStatus;
        private System.Windows.Forms.DataGridViewButtonColumn Delivered;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOHdrID;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDetID;
        private System.Windows.Forms.DataGridViewButtonColumn Print;
    }
}