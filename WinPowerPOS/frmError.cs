using System;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS
{
	public partial class frmError : Form
	{
		public frmError(Exception exception, string info)
		{
			InitializeComponent();
            Logger.writeLog(exception);
			var message = exception.Message;

			lblMessage.Text = (string.IsNullOrEmpty(info))
								  ? message
								  : string.Format("{0} ({1})", message, info);
		}

		public static void ShowError(Exception exception)
		{
            Logger.writeLog(exception);
			ShowError(null, exception, "");
		}

		public static void ShowError(IWin32Window owner, Exception exception)
		{
            Logger.writeLog(exception);
			ShowError(owner, exception, "");
		}

		public static void ShowError(IWin32Window owner, Exception exception, string info)
		{
            Logger.writeLog(exception);
			new frmError(exception, info).ShowDialog();
		}
	}
}
