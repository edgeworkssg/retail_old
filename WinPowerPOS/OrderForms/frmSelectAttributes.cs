using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS
{
    public partial class frmSelectAttributes : Form
    {
        public bool IsSuccess = false;
        public List<string> SelectedAttributes = new List<string>();

        public frmSelectAttributes(string itemNo, List<string> selAttribs)
        {
            InitializeComponent();
            try
            {
                LoadData(itemNo, selAttribs);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public void LoadData(string itemNo, List<string> selAttribs)
        {
            SelectedAttributes = selAttribs;
            var allAttGroup = new ItemAttributesMapController().FetchAll().Where(o => o.ItemNo == itemNo).ToList();
            flAttribGroup.Controls.Clear();
            foreach (var attGrp in allAttGroup)
            {
                Button b = new Button();
                b.Width = 100;
                b.Height = 40;
                b.BackColor = System.Drawing.Color.Orange;
                b.Font = new Font("Verdana", 9, FontStyle.Bold);
                b.Name = attGrp.AttributesGroup.AttributesGroupCode;
                b.Text = attGrp.AttributesGroup.AttributesGroupName;
                b.Click += new EventHandler(b_Click);
                flAttribGroup.Controls.Add(b);
            }

            if (flAttribGroup.Controls.Count > 0)
            {
                var firstButton = (Button)flAttribGroup.Controls[0];
                b_Click(firstButton, new EventArgs());
            }
            else
            {
                this.Close();
            }
            LoadSelected();
        }

        public void LoadSelected()
        {
            for (int i = 0; i < flAttributes.Controls.Count; i++)
            {
                var btn = (Button)flAttributes.Controls[i];

                if (SelectedAttributes.Contains(btn.Name))
                    btn.BackColor = Color.YellowGreen;
                else
                    btn.BackColor = Color.SlateBlue;
            }
        }

        protected void b_Click(object sender, EventArgs e)
        {
            Button theButton = (Button)sender;
            string groupName = theButton.Name;

            var allAttribute = new AttributeController().FetchAll().Where(o => o.AttributesGroupCode == groupName).ToList();
            flAttributes.Controls.Clear();

            foreach (var attGrp in allAttribute)
            {
                Button btnAttribute = new Button();
                btnAttribute.Width = 110;
                btnAttribute.Height = 45;
                btnAttribute.BackColor = System.Drawing.Color.SlateBlue;
                btnAttribute.Font = new Font("Verdana", 9, FontStyle.Bold);
                btnAttribute.Name = attGrp.AttributesCode;
                btnAttribute.Text = attGrp.AttributesName;
                btnAttribute.Click += new EventHandler(btnAttribute_Click);
                flAttributes.Controls.Add(btnAttribute);
            }
            LoadSelected();
        }

        void btnAttribute_Click(object sender, EventArgs e)
        {
            string attributeCode = ((Button)sender).Name;

            if (SelectedAttributes.Contains(attributeCode))
                SelectedAttributes.RemoveAll(o => o == attributeCode);
            else
                SelectedAttributes.Add(attributeCode);
            LoadSelected();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccess = false;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            IsSuccess = true;
            this.Close();
        }
    }
}
