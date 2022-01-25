using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Configuration;

//Author: Graham Pilapil
namespace PowerPOS
{
    public partial class frmUploadItemPhotos : Form
    {
        private string UPLOAD_DESTINATION = ConfigurationManager.AppSettings["ItemPhotosFolder"];
        DataTable DT = new DataTable();
        private int UploadCount = 0;

        public frmUploadItemPhotos()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            backgroundWorker1.WorkerReportsProgress = true;
        }

        private void frmUploadItemPhotos_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable DTCategory = new DataTable();
                DTCategory.Load(SubSonic.DataService.GetReader(new QueryCommand("SELECT CategoryName From Category Order By CategoryName ")));

                DataRow row = DTCategory.NewRow();
                row["CategoryName"] = "ALL";
                DTCategory.Rows.InsertAt(row, 0);


                cmbCategory.DataSource = DTCategory;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryName";

                GetItems();
         
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void GetItems()
        {
            string SQL = "SELECT ItemNo, ItemName, CategoryName, '' as [ImagePath] ,'Browse' as [Browse], 'Remove' as [Remove] FROM Item WHERE Deleted=0 ";
            DT.Clear();
            //DT.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));
            DT = SubSonic.DataService.GetDataSet(new QueryCommand(SQL)).Tables[0];
            DT.Columns["ImagePath"].MaxLength = 1000;
            DT.PrimaryKey = new DataColumn[] { DT.Columns["ItemNo"] };

            string[] extensions = { "jpg", "png", "bmp", "jpeg" };
            if (!Directory.Exists(UPLOAD_DESTINATION))
                Directory.CreateDirectory(UPLOAD_DESTINATION);
            string[] files = Directory.GetFiles(UPLOAD_DESTINATION, "*.*").Where(f => extensions.Contains(f.Split('.').Last().ToLower())).ToArray();
            if (files.Length != 0)
            {
                foreach (string file in files)
                {
                    
                    string fileName = Path.GetFileName(file);
                    fileName = fileName.Remove(fileName.IndexOf("."));
                    /*Item item = new Item(fileName);
                    if (item != null)
                    {
                        row["ItemNo"] = item.ItemNo;
                        row["ItemName"] = item.ItemName;
                        row["CategoryName"] = item.CategoryName;
                        row["ImagePath"] = file;
                        row["Browse"] = "Browse";
                        DT.Rows.Add(row);
                    }*/
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (fileName == DT.Rows[i]["ItemNo"].ToString())
                        {
                            DT.Rows[i]["ImagePath"] = Path.GetFullPath(file);
                        }
                    }
                }
            }

            dataGridView1.DataSource = DT.DefaultView;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string filterSQL = "";

                if (txtItem.Text.Trim() != "")
                    filterSQL+= string.Format("ItemNo+ItemName LIKE '%{0}%'", txtItem.Text.Trim());
                if (cmbCategory.SelectedValue != null)
                {
                    if (cmbCategory.SelectedValue.ToString()!="ALL")
                    {
                        if(filterSQL != "")
                            filterSQL += " AND ";
                        
                        filterSQL += string.Format("CategoryName LIKE '{0}' ", cmbCategory.SelectedValue.ToString());
                    }                        
                }

                DataView view = DT.DefaultView;


                view.RowFilter = filterSQL;

                //DataTable filter = view.ToTable();
                //dataGridView1.DataSource = filter;

                dataGridView1.DataSource = view;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will clear all imported data.. Do you want to continue?", "CLEAR",MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                txtItem.Text = "";
                cmbCategory.SelectedIndex = 0;
                txtItem.Focus();
                //this.dataGridView1.DataSource = null;
                progressBar.Value = 0;
                lblStatus.Text = "";
                GetItems();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex ==6)
                {
                    using (OpenFileDialog open = new OpenFileDialog())
                    {
                        open.Filter = "JPG Files (*.jpg)|*.jpg|BMP Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png";
                        open.ShowDialog();
                        if (open.FileName != "")
                        {
                            DataRow FindRow = DT.Rows.Find(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                            if (FindRow != null)
                                FindRow["ImagePath"] = open.FileName;
                        }
                    }
                }
                else if (e.ColumnIndex == 5)
                {
                    DataRow FindRow = DT.Rows.Find(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                    if (FindRow != null)
                    {
                        FindRow["ImagePath"] = "";
                    }
                    dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string[] extensions = { "jpg", "png", "bmp", "jpeg" };

                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.ShowDialog();
                    if (dialog.SelectedPath != "")
                    {
                        string[] files = Directory.GetFiles(dialog.SelectedPath, "*.*").Where(f => extensions.Contains(f.Split('.').Last().ToLower())).ToArray();
                        if (files.Length != 0)
                        {
                            DT.Clear();

                            foreach (string file in files)
                            {
                                DataRow row = DT.NewRow();
                                string fileName = Path.GetFileName(file);
                                fileName = fileName.Remove(fileName.IndexOf("."));
                                Item item = new Item(fileName);
                                if (item != null && item.ItemName != "")
                                {
                                    row["ItemNo"] = item.ItemNo;
                                    row["ItemName"] = item.ItemName;
                                    row["CategoryName"] = item.CategoryName;
                                    row["ImagePath"] = file;
                                    row["Browse"] = "Browse";
                                    DT.Rows.Add(row);
                                }
                            }
                            if (DT != null)
                                if (!backgroundWorker1.IsBusy)
                                {
                                    btnClear.Enabled = false;
                                    btnFilter.Enabled = false;
                                    btnImport.Enabled = false;
                                    btnUpload.Enabled = false;
                                    progressBar.Value = 0;
                                    backgroundWorker1.RunWorkerAsync();
                                }
                            //dataGridView1.DataSource = DT.DefaultView;
                        }

                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            //GetItems();
            ////dataGridView1.DataSource = null;
            //dataGridView1.DataSource = DT.DefaultView;
            //this.Text = "Upload Item Photos - MANUAL";
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                btnClear.Enabled = false;
                btnFilter.Enabled = false;
                btnImport.Enabled = false;
                btnUpload.Enabled = false;
                progressBar.Value = 0;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (DT != null)
                {
                    DataView view = DT.Copy().DefaultView;
                    view.RowFilter = "ImagePath <> ''";
                    DataTable dtUpload = view.ToTable();
                    
                    if (dtUpload.Rows.Count != 0)
                    {
                        UploadCount = dtUpload.Rows.Count;
                        int Counter = 0;
                        decimal progressValue = 0;
                        
                        if (!Directory.Exists(UPLOAD_DESTINATION))
                            Directory.CreateDirectory(UPLOAD_DESTINATION);
                        if(!UPLOAD_DESTINATION[UPLOAD_DESTINATION.Length-1].ToString().Equals(@"\"))
                            UPLOAD_DESTINATION+= @"\";
                        foreach (DataRow row in dtUpload.Rows)
                        {
                            Counter++;
                            string FileName = Path.GetFileNameWithoutExtension(row["ImagePath"].ToString());
                            string Source = row["ImagePath"].ToString();
                            string Destination = UPLOAD_DESTINATION + Path.GetFileName(row["ImagePath"].ToString().Replace(FileName, row["ItemNo"].ToString()));
           
                            File.Copy(Source, Destination, true);

                            progressValue = ((decimal)Counter / (decimal)dtUpload.Rows.Count) * 100;
                            backgroundWorker1.ReportProgress((int)progressValue, string.Format("Uploading {0} of {1}..", Counter, dtUpload.Rows.Count));
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Selected!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lblStatus.Text = e.UserState.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 
            MessageBox.Show(string.Format("{0} image(s) Succesfully uploaded..", UploadCount));
            btnClear.Enabled = true;
            btnFilter.Enabled = true;
            btnImport.Enabled = true;
            btnUpload.Enabled = true;
            lblStatus.Text = "Done.";
            UploadCount = 0;
            GetItems();
            progressBar.Value = 0;
            lblStatus.Text = "";
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow Row in dataGridView1.Rows)
            {
                if (!String.IsNullOrEmpty(Row.Cells[3].Value.ToString()))
                {
                    try
                    {
                        Row.Cells[4].Value = ResizeImage(Image.FromFile(Row.Cells[3].Value.ToString()), new Size(40, 40));
                    }
                    catch { }
                }
                else 
                {
                    Row.Cells[4].Value = null;
                }
            }
        }
        private Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

  

        
    }
}
