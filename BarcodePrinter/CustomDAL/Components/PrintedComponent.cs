using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace BarcodePrinter
{
    public interface PrintedComponent
    {
        string Name
        { get; set; }
        int Left
        { get; set; }
        int Top
        { get; set; }
        int Width
        { get; set; }
        int Height
        { get; set; }
        Font PrintedFont
        { get; set; }

        void Print(ref PrintPageEventArgs e);
        void Paint(ref PaintEventArgs e);

        void SetValue(object Value);
    }
}
