using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;

namespace WinUtility
{
    public partial class tryTablePlan : Form
    {
        private bool isDragging = false;
        private int currentX, currentY;

        public tryTablePlan()
        {
            InitializeComponent();
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;

            currentX = e.X;
            currentY = e.Y;
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {

                ((Button)sender).Top = ((Button)sender).Top + (e.Y - currentY);
                ((Button)sender).Left = ((Button)sender).Left + (e.X - currentX);
            }

        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
        
        Font f = new Font("Verdana", 11, FontStyle.Bold);
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics g = e.Graphics;
            PictureBox myPic = ((PictureBox)sender);
            //g.DrawString("03", f,Brushes.Black, new PointF(myPic.Location.X, myPic.Location.Y));
            g.DrawString(myPic.Tag.ToString(), f, Brushes.White, new PointF(10,10));
        }

        private void button1_Click(object sender, EventArgs e)
        {            
        }
    }
}
