using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPowerPOS.KioskForms
{
    public interface IObserver
    {
        void UpdateLog();
        void ExecuteCommand(string command);
    }
}
