using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
class FixReEntrantDataGridView : DataGridView
{
    private bool reEntrent = false;
    protected override bool SetCurrentCellAddressCore(int columnIndex, int rowIndex, bool setAnchorCellAddress, bool validateCurrentCell, bool throughMouseClick)
    {
        bool rv = true;
        if (!reEntrent)
        {
            reEntrent = true;
            rv = base.SetCurrentCellAddressCore(columnIndex, rowIndex, setAnchorCellAddress, validateCurrentCell, throughMouseClick);
            reEntrent = false;
        }
        return rv;
    }
}

