using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Data;
using GenericReport.LocalDAL;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;

namespace GenericReport.NewPrint
{
    public class A5Controller
    {
        public static ReportClass GetInvoice(POSController pos, bool reprint)
        {
            try
            {
                bool ShowPrice = true;
                bool ShowPayment = true;

                A5 Printer = new A5();

                string strLogoPath = ConfigurationSettings.AppSettings["LogoPath"];
                string strReportLogoName = ConfigurationSettings.AppSettings["LogoFileName"];
                strLogoPath += "\\" + strReportLogoName;

                //LoadLogo(Printer, strLogoPath);

                int LogoWidth = 0, LogoHeight = 0;

                PrintingInfo Infos = new PrintingInfo();
                Infos.AssignData("Receipt", pos, strLogoPath, out LogoWidth, out LogoHeight, reprint);

                Printer.SetDataSource((DataTable)Infos.DocumentInfo);
                if (ShowPrice && ShowPayment)
                {
                    Printer.Subreports["PaymentType"].SetDataSource((DataTable)Infos.PaymentInfo);
                }

                Printer.Header.ReportObjects["companylogo"].Width = LogoWidth;
                Printer.Header.ReportObjects["companylogo"].Height = LogoHeight;

                int StartPage = 0;
                int Endpage = 0;

                return Printer;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                System.Windows.Forms.MessageBox.Show("Error woi!!");
                return null;
            }
        }

        private static void HidePrice(A5 Printer)
        {
            Printer.Header.ReportObjects["tDisc"].ObjectFormat.EnableSuppress = false;
            Printer.Header.ReportObjects["tUnitPrice"].ObjectFormat.EnableSuppress = false;
            Printer.Header.ReportObjects["tLineTotal"].ObjectFormat.EnableSuppress = false;
            Printer.Details.ReportObjects["DiscPercent1"].ObjectFormat.EnableSuppress = false;
            Printer.Details.ReportObjects["UnitPrice1"].ObjectFormat.EnableSuppress = false;
            Printer.Details.ReportObjects["LineTotal1"].ObjectFormat.EnableSuppress = false;

            Printer.Header.ReportObjects["tQty"].Left = 7080;
            Printer.Details.ReportObjects["Quantity1"].Left = 7080;
            Printer.Details.ReportObjects["tDesc"].Width = 4480;
            Printer.Details.ReportObjects["Description1"].Width = 4480;
        }

        private static void LoadLogo(A5 Printer,string LogoURL)
        {
            Image imgLogo;

            if (File.Exists(LogoURL))
            {
                imgLogo = new Image();
                string name = "imgLogo";
                imgLogo.ImageName = name;
                imgLogo.ImageMIMEType = MIMEType.JPEG;
                imgLogo.ImageSizing = Sizing.FitProportional;
                imgLogo.ImagePath = LogoURL;
                imgLogo.ImageHeight = 1.8;
                imgLogo.ImageWidth = 6.0;
                imgLogo.posImg.x = 0.0;
                imgLogo.posImg.y = 0.0;
            }
            else
                imgLogo = null;

            bool bImageAlreadyEmbedded = false;
            // check if the image that the user wants to add has been embedded to the report yet or not
            for (int i = 0; i < Printer.Section1 .ReportObjects.Count; i++)
            {
                if (Printer.Section1.ReportObjects[i].Name == imgLogo.ImageName)
                {
                    //Printer.a [i] = ; 
                    bImageAlreadyEmbedded = true;
                    break;
                }
            }

            //if (bImageAlreadyEmbedded == true)
            //    listPageImages.Add(img);
            //else
            //    MessageBox.Show("Image has yet to be embedded to the report. Please add image for embedding first.", "ERROR");

        }
    }
}
