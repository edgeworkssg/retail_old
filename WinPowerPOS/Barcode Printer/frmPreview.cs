using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BarcodePrinter;

namespace WinPowerPOS.BarcodePrinter
{
    public partial class frmPreview : Form
    {
        #region #) Settings: listSource Data Source
        private string[] AvailableSource = new string[] 
            {
                "ItemNo"
                , "ItemName"
                , "CategoryName"
                , "ItemDepartmentID" 
                , "Barcode"
                , "RetailPrice"
                , "Attributes1"
                , "Attributes2"
                , "Attributes3"
                , "Attributes4"
                , "Attributes5"
                , "Attributes6"
                , "Attributes7"
                , "Attributes8"
                , "BarcodeText"
            };
        #endregion

        BarcodePrinterController MasterController = null;

        private bool isPostBack = false; /// (^_^)

        public frmPreview()
        {
            InitializeComponent();
            listSource.SelectedIndexChanged+=new EventHandler(listSource_SelectedIndexChanged);
            listSelected.SelectedIndexChanged+=new EventHandler(listSelected_SelectedIndexChanged);
        }

        private void frmPreview_Load(object sender, EventArgs e)
        {
            openTemplateDialog.InitialDirectory = Application.StartupPath;

            LoadTemplate("Default");

            pictureBox1.Paint += MasterController.HandlerPaintBarcodeLabel;

            listSource.Items.Clear();
            listSource.Items.AddRange(AvailableSource);
        }
       
        private void openTemplateDialog_FileOk(object sender, CancelEventArgs e)
        {
            LoadTemplate(new Uri(openTemplateDialog.FileName));
            pictureBox1.Refresh();
        }

        private void LoadSelectedList()
        {
            listSelected.Items.Clear();
            listSelected.Items.AddRange(MasterController.GetAllComponent());
        }
        private void LoadTemplate(string TemplateName)
        {
            MasterController = new BarcodePrinterController(TemplateName);
            tTemplateName.Text = MasterController.TemplateName;

            LoadSelectedList();

            pictureBox1.Paint += MasterController.HandlerPaintBarcodeLabel;
        }
        private void LoadTemplate(Uri TemplateFile)
        {
            MasterController = new BarcodePrinterController(TemplateFile);
            tTemplateName.Text = MasterController.TemplateName;

            LoadSelectedList();

            pictureBox1.Paint += MasterController.HandlerPaintBarcodeLabel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedIndex >= 0 && cmbType.Items[cmbType.SelectedIndex].ToString() == "Static Text")
                    tName.Text = tStatic.Text;

                if (tName.Text == "") return;

                int Left = 0
                    , Top = 0
                    , Width = 0
                    , Height = 0;

                Left = (int)tSizeLeft.Value;
                Top = (int)tSizeTop.Value;
                Width = (int)tSizeWidth.Value;
                Height = (int)tSizeHeight.Value;

                //#region *) Parse: (int) Left, Top, Width, Height
                //if (!int.TryParse(tSizeLeft.tex, out Left))
                //    throw new Exception("(error)Cannot read Left information");
                //if (!int.TryParse(tSizeTop.Text, out Top))
                //    throw new Exception("(error)Cannot read Top information");
                //if (!int.TryParse(tSizeWidth.Text, out Width))
                //    throw new Exception("(error)Cannot read Width information");
                //if (!int.TryParse(tSizeHeight.Text, out Height))
                //    throw new Exception("(error)Cannot read Height information");
                //#endregion

                if (cmbType.Items[cmbType.SelectedIndex].ToString() == "Barcode")
                    MasterController.AddBarcode(tName.Text, Left, Top, Width, Height, StringAlignment.Center);
                else if (cmbType.Items[cmbType.SelectedIndex].ToString() == "Text")
                    MasterController.AddText(tName.Text, Left, Top, Width, Height, StringAlignment.Center, (Font)tFont.Tag);
                else if (cmbType.Items[cmbType.SelectedIndex].ToString() == "Currency")
                    MasterController.AddCurrency(tName.Text, Left, Top, Width, Height, StringAlignment.Far, (Font)tFont.Tag);
                else if (cmbType.Items[cmbType.SelectedIndex].ToString() == "Static Text")
                    MasterController.AddStaticText(tStatic.Text, Left, Top, Width, Height, StringAlignment.Far, (Font)tFont.Tag);

                cmbType.Refresh();
                LoadSelectedList();
                pictureBox1.Refresh();
            }
            catch (Exception X)
            {
                MessageBox.Show(X.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tName.Text == "") return;

            MasterController.DeleteComponent(tName.Text);

            LoadSelectedList();
            pictureBox1.Refresh();
        }

        private void listSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listSource.Items.Count <= 0) return;
            if (listSource.SelectedIndex < 0) return;

            if (!isPostBack)
            {
                LoadComponentInformation(listSource.Items[listSource.SelectedIndex].ToString());

                if (listSelected.Items.Count > 0)
                {
                    isPostBack = true;
                    listSelected.SelectedIndex = -1;
                    for (int Counter = 0; Counter < listSelected.Items.Count; Counter++)
                    {
                        if (listSelected.Items[Counter].ToString() == tName.Text)
                        {
                            listSelected.SelectedIndex = Counter;
                            break;
                        }
                    }
                    isPostBack = false;
                }
            }
        }

        private void listSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listSelected.Items.Count <= 0) return;
            if (listSelected.SelectedIndex < 0) return;
            
            if (!isPostBack)
            {
                //listSource.SelectedValue = listSelected.SelectedValue;
                

                if (listSource.Items.Count > 0)
                {
                    isPostBack = true;
                    listSource.SelectedIndex = -1;
                    tName.Text = listSelected.SelectedItem.ToString();
                    for (int Counter = 0; Counter < listSource.Items.Count; Counter++)
                    {
                        if (listSource.Items[Counter].ToString() == tName.Text)
                        {
                            listSource.SelectedIndex = Counter;
                            break;
                        }
                    }
                    isPostBack = false;
                }
                LoadComponentInformation(listSelected.Items[listSelected.SelectedIndex].ToString());

            }
        }

        private void LoadComponentInformation(string ComponentName)
        {
           // if (tName.Text == ComponentName) return;

            tName.Text = ComponentName;
            tStatic.Text = "";
            PrintedComponent Rst = MasterController.LoadComponentInformation(ComponentName);
            if (Rst == null)
            {
                cmbType.SelectedItem = "Text";
                tSizeLeft.Value = 0;
                tSizeTop.Value = 0;
                tSizeWidth.Value = 0;
                tSizeHeight.Value = 0;
                Font SelectedFont = new Font("Arial Narrow", 8);
                tFont.Tag = SelectedFont;
                tFont.Text = SelectedFont.FontFamily + " [Size: " + SelectedFont.Size.ToString("N0") + "]";
            }
            else
            {
                if (Rst is BarcodeComponent)
                    cmbType.SelectedItem = "Barcode";
                else if (Rst is TextComponent)
                    cmbType.SelectedItem = "Text";
                else if (Rst is CurrencyComponent)
                    cmbType.SelectedItem = "Currency";
                else if (Rst is StaticComponent)
                {
                    tStatic.Text = tName.Text;
                    cmbType.SelectedItem = "Static Text";
                }
                cmbType.Refresh();
                try
                {
                    tSizeLeft.Value = Rst.Left;
                    tSizeTop.Value = Rst.Top;
                    tSizeWidth.Value = Rst.Width;
                    tSizeHeight.Value = Rst.Height;
                    Font SelectedFont = Rst.PrintedFont;
                    tFont.Tag = SelectedFont;
                    tFont.Text = SelectedFont.FontFamily + " [Size: " + SelectedFont.Size.ToString("N0") + "]";
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openTemplateDialog.ShowDialog();
        }

        private void tTemplateName_Validated(object sender, EventArgs e)
        {
            MasterController.TemplateName = tTemplateName.Text.Trim();
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = (Font)tFont.Tag;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Font SelectedFont = fontDialog1.Font;
                tFont.Tag = SelectedFont;
                tFont.Text = SelectedFont.FontFamily + " [Size: " + SelectedFont.Size.ToString("N0") + "]";
            }
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            Font SelectedFont = fontDialog1.Font;
            tFont.Tag = SelectedFont;
            tFont.Text = SelectedFont.FontFamily + " [Size: " + SelectedFont.Size.ToString("N0") + "]";
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedIndex < 0) return;

            if (cmbType.Items[cmbType.SelectedIndex].ToString() == "Static Text")
            {
                tStatic.ReadOnly = false;
                //LoadComponentInformation(tStatic.Text);
            }
            else
            {
                tStatic.ReadOnly = true;
                //if (listSource.SelectedIndex > 0)
                //    LoadComponentInformation(listSource.Items[listSource.SelectedIndex].ToString());
            }

        }

        private void cmbType_Click(object sender, EventArgs e)
        {
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            btnDelete.PerformClick();
        }

    }
}
