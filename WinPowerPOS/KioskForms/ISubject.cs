using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPowerPOS.KioskForms
{
    public interface ISubject
    {
        void Attach(IObserver obj);
        void Detach(IObserver obj);
        void Notify();
        void SendCommand(string command);
    }
}
