using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using WinPowerPOS.OrderForms;
using System.ComponentModel;

namespace WinPowerPOS
{
    public partial class FormController
    {
        public partial class FormNames
        {
            public static string AttendanceReport = "frmAttendanceReport";
            public static string AttendanceModule = "frmAttendance";
            public static string FeatureSetup = "frmFeatureSetup";
            public static string OutstandingInstallmentReport = "frmOutstandingInstallmentReport";
            public static string DeliveryList = "frmDeliveryList";
            public static string ExtraChargeSetup = "frmExtraChargeSetup";
        }

        public static Dictionary<string, Form> AllForms = new Dictionary<string, Form>();

        public static void ShowInvoice(Guid AppointmentId, BackgroundWorker SyncSalesThread, BackgroundWorker SyncAppointmentThread)
        {
            //Load the form
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm is frmOrderTaking)
                {
                    MessageBox.Show("Another Sales Invoice is already open. Please settle the previous transaction first");
                }
            }
            if (AllForms.ContainsKey("frmOrderTaking"))
            {
                MessageBox.Show("Another Sales Invoice is already open. Please settle the previous transaction first");
                return;
            }


            frmOrderTaking fOrderTaking = new frmOrderTaking(AppointmentId);
            fOrderTaking.SyncSalesThread = SyncSalesThread;
            fOrderTaking.ShowDialog();
            //in the form will load members and items from appointment then show to the user
            //run sales invoice like usual 
            //the update process of appointment's orderhdrid will be handled in the pos.ConfirmOrder;
            if (fOrderTaking.isSuccessful)
            {
                Appointment app = new Appointment(AppointmentId);
                app.OrderHdrID = fOrderTaking.OrderHdrID;
                //app.Save();
                string status = "";
                if (!AppointmentController.SendAppointment(app, SyncAppointmentThread, out status))
                {
                    MessageBox.Show(status);
                    return;
                }
            }
            fOrderTaking.Dispose();
        }

        public static bool ShowInvoiceWithReturn(Guid AppointmentId, BackgroundWorker SyncSalesThread, BackgroundWorker SyncAppointmentThread)
        {
            //Load the form
            try
            {
                FormCollection fc = Application.OpenForms;

                foreach (Form frm in fc)
                {
                    if (frm is frmOrderTaking)
                    {
                        //MessageBox.Show("Another Sales Invoice is already open. Please settle the previous transaction first");
                        throw new Exception("Another Sales Invoice is already open. Please settle the previous transaction first");
                    }
                }
                if (AllForms.ContainsKey("frmOrderTaking"))
                {
                    //MessageBox.Show("Another Sales Invoice is already open. Please settle the previous transaction first");
                    //return;
                    throw new Exception("Another Sales Invoice is already open. Please settle the previous transaction first");
                }


                frmOrderTaking fOrderTaking = new frmOrderTaking(AppointmentId);
                fOrderTaking.SyncSalesThread = SyncSalesThread;
                fOrderTaking.ShowDialog();
                //in the form will load members and items from appointment then show to the user
                //run sales invoice like usual 
                //the update process of appointment's orderhdrid will be handled in the pos.ConfirmOrder;
                if (fOrderTaking.isSuccessful)
                {
                    Appointment app = new Appointment(AppointmentId);
                    app.OrderHdrID = fOrderTaking.OrderHdrID;
                    app.IsServerUpdate = true;
                    app.Save();
                    string status = "";
                    /*if (!AppointmentController.SendAppointment(app, SyncAppointmentThread, out status))
                    {
                        //MessageBox.Show(status);
                        throw new Exception(status);
                    }*/
                    

                    fOrderTaking.Dispose();
                    return true;
                }
                fOrderTaking.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static void ShowQuotation(string OrderHdrID, BackgroundWorker SyncQuotationThread)
        {
            //Load the form
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm is frmOrderQuotation)
                {
                    MessageBox.Show("Another Quotation is already open. Please settle the previous transaction first");
                }
            }
            if (AllForms.ContainsKey("frmOrderQuotation"))
            {
                MessageBox.Show("Another Quotation is already open. Please settle the previous transaction first");
                return;
            }


            frmOrderQuotation fOrderQuotation = new frmOrderQuotation();
            fOrderQuotation.OrderHdrID = OrderHdrID;
            fOrderQuotation.SyncQuotationThread = SyncQuotationThread;
            fOrderQuotation.ShowDialog();
            fOrderQuotation.Dispose();
        }

        public static bool ShowInvoiceFromAttendance(string membershipNo, string itemNo, DateTime startTime, DateTime endTime, BackgroundWorker SyncSalesThread)
        {
            //Load the form
            try
            {
                frmOrderTaking fOrderTaking = new frmOrderTaking();

                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    if (frm is frmOrderTaking)
                    {
                        fOrderTaking = (frmOrderTaking)frm;
                        if (fOrderTaking.AddAttendanceItemToOrder(membershipNo, itemNo, startTime, endTime))
                        {
                            fOrderTaking.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                            fOrderTaking.Show();
                            fOrderTaking.Activate();
                            fOrderTaking.BringToFront();
                            return true;
                        }
                    }
                }

                fOrderTaking.SyncSalesThread = SyncSalesThread;
                fOrderTaking.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                fOrderTaking.Show();
                fOrderTaking.Activate();
                fOrderTaking.BringToFront();
                fOrderTaking.AddAttendanceItemToOrder(membershipNo, itemNo, startTime, endTime);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static void ShowForm(string FormName, Form Inst)
        {
            if (AllForms.ContainsKey(FormName))
            {
                if (AllForms[FormName] == null || AllForms[FormName].IsDisposed)
                {
                    AllForms.Remove(FormName);

                    if (Inst == null || Inst.IsDisposed)
                        return;
                    else
                        AllForms.Add(FormName, Inst);
                }
                else
                {
                    Inst = AllForms[FormName];
                }
            }
            else
            {
                if (Inst == null || Inst.IsDisposed)
                    return;
                else
                    AllForms.Add(FormName, Inst);
            }

            Inst.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Inst.Show();
            Inst.Activate();
            Inst.BringToFront();
        }

        public static void CloseForm(string FormName, Form Inst)
        {
            if (AllForms.ContainsKey(FormName))
            {
                if (!(AllForms[FormName] == null || AllForms[FormName].IsDisposed))
                {
                    AllForms[FormName].Dispose();
                }
                AllForms.Remove(FormName);
            }
        }

    }
}
