using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmSignatureDelivery : Form
    {
        public bool IsSuccessful = false;
        string orderRefNo = "";
        public Bitmap bmp;
        Bitmap bmp2;
        private List<Point> strokes = new List<Point>();
        private List<List<Point>> totalStrokes = new List<List<Point>>();
        PictureBox p;

        public frmSignatureDelivery()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            //this.pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            //this.pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            //this.pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);

            //this.p.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            //this.p.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            //this.p.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);

            //create picturebox dynamically
            p = new PictureBox();
            p.Width = Screen.PrimaryScreen.Bounds.Width;
            p.Height = Screen.PrimaryScreen.Bounds.Height;
            p.Top = 0;
            p.Left = 0;
            
            //add mouse handler
            p.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            p.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            p.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            
            this.Controls.Add(p);
            
            //bitmaps
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp2 = new Bitmap(p.Width, p.Height);

            pictureBox1.Visible = false;
        }

        private Point? _Previous = null;
        private Pen _Pen = new Pen(Color.Black);
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _Previous = new Point(e.X, e.Y);
            strokes.Add(new Point(e.X, e.Y));
            pictureBox1_MouseMove(sender, e);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Previous != null)
            {
                if (pictureBox1.Image == null)
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                    }
                    pictureBox1.Image = bmp;
                }
                if (p.Image == null)
                {
                    using (Graphics g = Graphics.FromImage(bmp2))
                    {
                        g.Clear(Color.FromKnownColor(KnownColor.Control));
                    }
                    p.Image = bmp2;

                }
                using (Graphics g = Graphics.FromImage(p.Image))
                {
                    g.DrawLine(_Pen, _Previous.Value.X, _Previous.Value.Y, e.X, e.Y);
                }
                p.Invalidate();
                _Previous = new Point(e.X, e.Y);
                strokes.Add(new Point(e.X, e.Y));
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _Previous = null;
            //btnCapture_Click(sender, e);
            totalStrokes.Add(new List<Point>(strokes));
            strokes.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (totalStrokes.Count <= 0)
                    throw new Exception("(warning)You haven't signed anything.");

                {
                    //confirm signature
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                            ("Confirm", "Do you want to confirm?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        //capture the signature
                        btnCapture_Click(sender, e);

                        //set 
                        IsSuccessful = true;

                        string InputOutputFolder = Application.StartupPath + "\\Signature";
                        //if (!Directory.Exists(InputOutputFolder))
                        //    Directory.CreateDirectory(InputOutputFolder);

                        //string savingPath = Application.StartupPath + "\\Signature\\sign" + orderRefNo + ".jpg";

                        Bitmap bm = new Bitmap(this.bmp);
                        //bm.Save(savingPath, ImageFormat.Jpeg);

                        //close dialogue
                        this.Close();

                    }

                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel || myfrm.choice == "cancel")
                    {
                        myfrm.Dispose();
                        return;
                    }
                }
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                pictureBox1.Invalidate();
            }
            using (Graphics g = Graphics.FromImage(bmp2))
            {
                g.Clear(Color.FromKnownColor(KnownColor.Control));
                p.Invalidate();
            }

            //clear list
            strokes.Clear();
            totalStrokes.Clear();
        }


        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (totalStrokes.Count > 0)
            {
                if (pictureBox1.Image == null)
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                    }
                    pictureBox1.Image = bmp;
                }

                if (p.Image == null)
                {
                    using (Graphics g = Graphics.FromImage(bmp2))
                    {
                        g.Clear(Color.FromKnownColor(KnownColor.Control));
                    }
                    p.Image = bmp2;

                }

                //scaling points if size is bigger
                int leftLimitX = 0;
                int rightLimitX = 0;
                int upLimitY = 0;
                int downLimitY = 0;

                //set limit to the first point
                leftLimitX = totalStrokes[0][0].X;
                rightLimitX = totalStrokes[0][0].X;
                upLimitY = totalStrokes[0][0].Y;
                downLimitY = totalStrokes[0][0].Y;

                foreach (List<Point> line in totalStrokes)
                {
                    //find rectangle limit
                    for (int i = 0; i < line.Count - 1; ++i)
                    {
                        //width
                        if (line[i].X < leftLimitX)
                            leftLimitX = line[i].X;
                        else if (line[i].X > rightLimitX)
                            rightLimitX = line[i].X;

                        //height
                        if (line[i].Y < downLimitY)
                            downLimitY = line[i].Y;
                        else if (line[i].Y > upLimitY)
                            upLimitY = line[i].Y;

                    }
                }
                //check if width > height, vice versa
                int width = rightLimitX - leftLimitX;
                int height = upLimitY - downLimitY;
                double scale = 1; //default scale

                if (width >= height)
                    scale = (double)pictureBox1.Height / width; //the picturebox's height is smaller than the width
                else if (height > width)
                    scale = (double)pictureBox1.Height / height;

                if (scale > 1)
                    scale = 1;
    
                int newX, newY;
                List<List<Point>> newTotalStrokes = new List<List<Point>>();
                foreach (List<Point> line in totalStrokes)
                {
                    List<Point> newPoints = new List<Point>();
                    //scaling the points
                    for (int i = 0; i < line.Count - 1; ++i)
                    {
                        newX = Convert.ToInt32(line[i].X * scale);
                        newY = Convert.ToInt32(line[i].Y * scale);

                        newPoints.Add(new Point(newX, newY));
                    }
                    newTotalStrokes.Add(newPoints);
                }

                //translate points to fit the pictureBox
                int translateX = pictureBox1.Location.X - Convert.ToInt32(leftLimitX * scale) - 40; //40,100 = magic number??
                int translateY = pictureBox1.Location.Y - Convert.ToInt32(upLimitY * scale) + 100;

                List<List<Point>> newTotalStrokes2 = new List<List<Point>>();
                foreach (List<Point> line in newTotalStrokes)
                {
                    List<Point> newPoints = new List<Point>();
                    for (int i = 0; i < line.Count - 1; ++i)
                    {
                        newX = line[i].X + translateX;
                        newY = line[i].Y + translateY;

                        newPoints.Add(new Point(newX, newY));
                    }
                    newTotalStrokes2.Add(newPoints);
                }

                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    //draw signature
                    foreach (List<Point> line in newTotalStrokes2)
                    {
                        for (int i = 0; i < line.Count - 1; ++i)
                            g.DrawLine(_Pen, line[i], line[i + 1]);
                    }
                    pictureBox1.Invalidate();
                }

                //clear the list
                totalStrokes.Clear();


            }
        }

        private void frmSignature_Load(object sender, EventArgs e)
        {
            //p.Load();
        }
    }
}
