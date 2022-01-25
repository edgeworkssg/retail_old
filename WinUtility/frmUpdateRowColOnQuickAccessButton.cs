using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinUtility
{
    public partial class frmUpdateRowColOnQuickAccessButton : Form
    {
        public frmUpdateRowColOnQuickAccessButton()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pull the buttons
            //ItemCollection itemCols = cat[i].ItemRecords();
            QuickAccessCategoryCollection itemCols = new QuickAccessCategoryCollection();            
            itemCols.Load();
            int col = 0, row = 0;
            for (int j = 0; j < itemCols.Count; j++)
            {
                //Create Quick Access Button
                QuickAccessButtonCollection qbutton = new QuickAccessButtonCollection();
                qbutton.Where(QuickAccessButton.Columns.QuickAccessCategoryID, itemCols[j].QuickAccessCategoryId);
                qbutton.Load();
                row = 0; col = 0;
                for (int p = 0; p < qbutton.Count; p++)
                {

                    qbutton[p].Col = col;
                    qbutton[p].Row = row;
                    col++;
                    if (col > 5)
                    {
                        col = 0;
                        row++;
                    }
                    qbutton[p].Save();
                }                
            }
            MessageBox.Show("Done!");
        }
    }
}
