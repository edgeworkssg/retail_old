using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using PowerPOS.Container;
using System.Runtime.InteropServices;

namespace POSDevices
{

    
    
    public class FlyTechCashDrawer
    {

        [DllImport("inpout32.dll")]
        private static extern UInt32 IsInpOutDriverOpen();
        [DllImport("inpout32.dll")]
        private static extern void Out32(short PortAddress, short Data);
        [DllImport("inpout32.dll")]
        private static extern char Inp32(short PortAddress);

        [DllImport("inpout32.dll")]
        private static extern void DlPortWritePortUshort(short PortAddress, ushort Data);
        [DllImport("inpout32.dll")]
        private static extern ushort DlPortReadPortUshort(short PortAddress);

        [DllImport("inpout32.dll")]
        private static extern void DlPortWritePortUlong(int PortAddress, uint Data);
        [DllImport("inpout32.dll")]
        private static extern uint DlPortReadPortUlong(int PortAddress);

        [DllImport("inpoutx64.dll")]
        private static extern bool GetPhysLong(ref int PortAddress, ref uint Data);
        [DllImport("inpoutx64.dll")]
        private static extern bool SetPhysLong(ref int PortAddress, ref uint Data);


        [DllImport("inpoutx64.dll", EntryPoint = "IsInpOutDriverOpen")]
        private static extern UInt32 IsInpOutDriverOpen_x64();
        [DllImport("inpoutx64.dll", EntryPoint = "Out32")]
        private static extern void Out32_x64(short PortAddress, short Data);
        [DllImport("inpoutx64.dll", EntryPoint = "Inp32")]
        private static extern char Inp32_x64(short PortAddress);

        [DllImport("inpoutx64.dll", EntryPoint = "DlPortWritePortUshort")]
        private static extern void DlPortWritePortUshort_x64(short PortAddress, ushort Data);
        [DllImport("inpoutx64.dll", EntryPoint = "DlPortReadPortUshort")]
        private static extern ushort DlPortReadPortUshort_x64(short PortAddress);

        [DllImport("inpoutx64.dll", EntryPoint = "DlPortWritePortUlong")]
        private static extern void DlPortWritePortUlong_x64(int PortAddress, uint Data);
        [DllImport("inpoutx64.dll", EntryPoint = "DlPortReadPortUlong")]
        private static extern uint DlPortReadPortUlong_x64(int PortAddress);

        [DllImport("inpoutx64.dll", EntryPoint = "GetPhysLong")]
        private static extern bool GetPhysLong_x64(ref int PortAddress, ref uint Data);
        [DllImport("inpoutx64.dll", EntryPoint = "SetPhysLong")]
        private static extern bool SetPhysLong_x64(ref int PortAddress, ref uint Data);
        



        private string PORT1_OPEN="10";
        private string PORT1_CLOSE="00";
        private string PORT1_SBIT = "08";

        private string PORT2_OPEN="30";
        private string PORT2_CLOSE="00";
        private string PORT2_SBIT = "00";

        private string IN = "482";

        private string OUT = "482";

        private string IN1 = "A07";
        private string PORT1_OPEN1 = "04";
        private string PORT1_CLOSE1 = "10";
        private string PORT1_SBIT1 = "20";

        public void LoadValuesFromSetting()
        {

        }


        public void OpenDrawer(string port)
        {
            try
            {

                short iPort = Convert.ToInt16(OUT, 16);

                short data = 0;
                if (port == "2")
                    data = Convert.ToInt16(PORT2_OPEN, 16);
                else
                    data = Convert.ToInt16(PORT1_OPEN, 16);
                Out32(iPort, data);

                System.Threading.Thread.Sleep(50);
                data = 0;
                if (port == "2")
                    data = Convert.ToInt16(PORT2_CLOSE, 16);
                else
                    data = Convert.ToInt16(PORT1_CLOSE, 16);
                Out32(iPort, data);



            }
            catch (Exception e) { }

            try
            {
                OpenDrawer8780(port);
            }
            catch (Exception ex) { }
        }

        public string OpenDrawer8780(string port)
        {
            try
            {

                short iPort = Convert.ToInt16(IN1, 16);

                short data = 0;
                if (port == "2")
                    data = Convert.ToInt16(PORT2_OPEN, 16);
                else
                    data = Convert.ToInt16(PORT1_OPEN1, 16);
                Out32(iPort, data);

                System.Threading.Thread.Sleep(50);
                data = 0;
                if (port == "2")
                    data = Convert.ToInt16(PORT2_CLOSE, 16);
                else
                    data = Convert.ToInt16(PORT1_CLOSE1, 16);
                Out32(iPort, data);

                return "";
            }
            catch (Exception e)
            {
                throw new Exception("Open drawer failed!", e);
            }
        }

        public void CloseDrawer(string port)
        {
            try
            {
                short iPort = Convert.ToInt16(OUT,16);
                short data = 0;
                
                data = 0;
                if (port == "2")
                    data = Convert.ToInt16(PORT2_CLOSE,16);
                else
                    data = Convert.ToInt16(PORT1_CLOSE,16);
                Out32(iPort, data);

            }
            catch (Exception e)
            {
                throw new Exception("Open drawer failed!", e);
            }
        }

        public char checkStatus()
        {
            try
            {
                short iPort = Convert.ToInt16(IN, 16);
                char tmp = Inp32(iPort);
                return tmp;
            }
            catch (Exception e)
            {
                throw new Exception("Open drawer failed!", e);
            }
        }

        public bool isCashDrawerOpen()
        {
            char tmp = checkStatus();
            char tmp8750 = checkStatus8750();
            if (tmp == 'O' || tmp8750 == '2')
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool isCashDrawerOpen(out char tmp8750)
        {
            char tmp = checkStatus();
            tmp8750 = checkStatus8750();
            if (tmp == 'O' || tmp8750 == '2' || tmp8750 == '0')
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public char checkStatus8750()
        {
            try
            {
                short iPort = Convert.ToInt16(IN1, 16);
                char tmp = Inp32(iPort);
                return tmp;
            }
            catch (Exception e)
            {
                throw new Exception("Open drawer failed!", e);
            }
        }

        public bool isCashDrawerOpen8750()
        {
            char tmp = checkStatus8750();
            if (tmp == '2')
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}
