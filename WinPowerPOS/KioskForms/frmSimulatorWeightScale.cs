using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOSLib.Controller;

namespace WinPowerPOS.KioskForms
{
    public partial class frmSimulatorWeightScale : Form
    {
        private bool isConnected;

        private int _currentWeight;


        public event WeightChangedHandler WeightChanged;

        public event StatusChangedHandler StatusChanged;


        public frmSimulatorWeightScale()
        {
            InitializeComponent();

            init();
        }

        public void init()
        {
            _currentWeight = 0;
            lCurrentWeight.Text = _currentWeight.ToString();
            tWeight.Text = "0";

            isConnected = false;
            chConnect.Checked = isConnected;
        }

        private void chConnect_CheckedChanged(object sender, EventArgs e)
        {
            isConnected = chConnect.Checked;

            if (StatusChanged != null)
                StatusChanged(null, new StatusChangedArgs(isConnected ? "Connected" : "Disconnected"));
        }

        private void bChange_Click(object sender, EventArgs e)
        {
            int tmp = 0;
            int.TryParse(tWeight.Text, out tmp);

            _currentWeight += tmp;

            if (WeightChanged != null)
                WeightChanged(null, new WeightChangedArgs(_currentWeight, tmp));

            tWeight.Text = "0";
            lCurrentWeight.Text = _currentWeight.ToString();
        }

        public string getStatus()
        {
            if (isConnected)
                return "Connected";
            else
                return "Disconnected";
        }

        public int getWeight()
        {
            return _currentWeight;
        }
    }
}
