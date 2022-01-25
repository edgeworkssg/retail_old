using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POSDevices;
using System.Configuration;

namespace WincorTest
{
    public partial class Form1 : Form
    {
        PriceDisplay myDisplay;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            myDisplay.SendCommandToDisplay(PriceDisplay.ConvertToChar(ConfigurationManager.AppSettings["FirstLineCommand"]));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            myDisplay.SendCommandToDisplay(PriceDisplay.ConvertToChar(ConfigurationManager.AppSettings["SecondLineCommand"]));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myDisplay.SendCommandToDisplay(textBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myDisplay = new PriceDisplay(ConfigurationManager.AppSettings["PriceDisplayPrinter"], ConfigurationManager.AppSettings["ClearScreen"]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myDisplay.ClearScreen();
        }
    }
}
