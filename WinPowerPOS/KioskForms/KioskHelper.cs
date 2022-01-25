using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPowerPOS.KioskForms
{
    public static class KioskHelper
    {
        public static bool isNewWeightLessThan(decimal newWeight, decimal lastWeight, decimal weightTolerance)
        {
            if (newWeight < lastWeight - weightTolerance)
            {
                return true;
            }
            return false;
        }

        public static bool isNewWeightMoreThan(decimal newWeight, decimal lastWeight, decimal weightTolerance)
        {
            if (newWeight > lastWeight + weightTolerance)
            {
                return true;
            }
            return false;
        }

        public static bool isNewWeightEquals(decimal newWeight, decimal lastWeight, decimal weightTolerance)
        {
            if (Math.Abs(newWeight - lastWeight) <= weightTolerance)
            {
                return true;
            }
            return false;
        }


    }

    public enum BarcodeScanState
    {
        ScanItem,
        ScanVerifyNRIC,
        ScanVerifyStaffCard,
        ScanStaffCard,
        ScanReceipt
    }

    public enum IndicatorType
    {
        Wnot0,
        Wx,
        Nx,
        D
    }
}
