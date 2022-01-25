using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Notifier.Properties;
using System.IO;
using System.Globalization;

/*
 * Arguments detail:
 * -p: execute profit/loss report
 * -t: execute sales tally report
 * -n: execute no sales stores report
 * 
 * ~sd: set start date properties (~sd:MM/dd/yyyy)
 * ~ed: set end date properties (~ed:MM/dd/yyyy)
 * ~co: set country properties (~co:ALL or <name of country>)
 * ~re: set region properties (~re:ALL or <name of region>)
 * ~pos: set point of sales properties (~pos:<pos id>)
 */


namespace Notifier
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Notifier Application Starting...");
                List<string> InvokeModule = new List<string>();
                List<string> Parameters = new List<string>();

                if (args.Contains("/?"))
                {
                    Console.WriteLine("Welcome to Mail Notifier application for Philips.");
                    Console.WriteLine("Please read below HELP for further information to execute this application:");
                    Console.WriteLine();
                    Console.WriteLine("1. Execute this application using some argument behind console comment, look bottom example:");
                    Console.WriteLine("   C:\\>Notifier.exe -p -t -n -m");
                    Console.WriteLine("2. There were some arguments to execute supported report module, below explanation for arguments:");
                    Console.WriteLine("   -p   Execute Profit/Loss Report");
                    Console.WriteLine("   -t   Execute Sales Tally Report");
                    Console.WriteLine("   -n   Execute No Sales Stores Report");
                    Console.WriteLine("   -m   Execute Monthly Sales Report");
                    Console.WriteLine("3. There were some arguments to set report fields, below explanation for arguments:");
                    Console.WriteLine("   ~sd: set start date properties for profit/loss report and sales tally report (~sd:MM/dd/yyyy)");
                    Console.WriteLine("   ~ed: set end date properties for profit/loss report and sales tally report (~ed:MM/dd/yyyy)");
                    Console.WriteLine("   ~dd: set date properties for no sales stores report and sales tally report (~ed:MM/dd/yyyy)");
                    Console.WriteLine("   ~co: set country properties for no sales stores report (~co:ALL or <name of country>)");
                    Console.WriteLine("   ~re: set region properties for no sales stores report (~re:ALL or <name of region>)");
                    Console.WriteLine("   ~pos: set point of sales properties for no sales stores report (~pos:<pos id>)");
                    Console.WriteLine("   ~d: set date properties for no sales stores report to get [today date - some number of days] (~d:<number of days>)");
                    Console.WriteLine("   ~m: set start date and end date properties for profit/loss report and sales tally report to get start date as first day of some number of months and end date as last day of some number of months [today date - some number of days] (~m:<number of months>)");
                    Console.WriteLine("   ~mo: set month properties for monthly sale report (~mo:<number of month 1 - 12> or <0 for current month> or <-number for number months ago>)");
                    Console.WriteLine("   ~yr: set year properties for monthly sale report (~yr:<number of year 4 digits> or <0 for current year> or <-number for number years ago>)");
                    Console.WriteLine("4. Find EmailNotification Table to set mail resipient for each module, supported module name below:");
                    Console.WriteLine("   a. ProfitLossReport: Execute Profit/Loss Report");
                    Console.WriteLine("   b. SalesTallyReport: Execute Sales Tally Report");
                    Console.WriteLine("   c. NoSalesStoresReport: Execute No Sales Stores Report");
                    Console.WriteLine("   d. MonthlySaleReport: Execute Monthly Sale Report");
                    Console.WriteLine();
                    Console.WriteLine("=== GOOD LUCK ===");
                }
                else
                {
                    #region Separate Module Call and Parameters
                    foreach (string item in args)
                    {
                        if (item.StartsWith("-"))
                        {
                            InvokeModule.Add(item);
                        }
                        else if (item.StartsWith("~"))
                        {
                            Parameters.Add(item);
                        }
                    }
                    #endregion

                    #region Set Parameter
                    foreach (string item in Parameters)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            string paramIndex = item.Split(':')[0];
                            switch (paramIndex)
                            {
                                case "~sd":
                                    if (InvokeModule.Contains("-p"))
                                        Notifier.Module.ProfitLossReport.Instance.StartDate = DateTime.ParseExact(string.Format("{0} 00:00:00", item.Split(':')[1]), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    if (InvokeModule.Contains("-t"))
                                        Notifier.Module.SalesTallyReport.Instance.StartDate = DateTime.ParseExact(string.Format("{0} 00:00:00", item.Split(':')[1]), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    break;
                                case "~ed":
                                    if (InvokeModule.Contains("-p"))
                                        Notifier.Module.ProfitLossReport.Instance.EndDate = DateTime.ParseExact(string.Format("{0} 23:59:59", item.Split(':')[1]), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    if (InvokeModule.Contains("-t"))
                                        Notifier.Module.SalesTallyReport.Instance.EndDate = DateTime.ParseExact(string.Format("{0} 23:59:59", item.Split(':')[1]), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    break;
                                case "~dd":
                                    if (InvokeModule.Contains("-n"))
                                        Notifier.Module.NoSalesStoresReport.Instance.Date = DateTime.ParseExact(string.Format("{0} 23:59:59", item.Split(':')[1]), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    break;
                                case "~co":
                                    if (InvokeModule.Contains("-n"))
                                        Notifier.Module.NoSalesStoresReport.Instance.Country = item.Split(':')[1];
                                    break;
                                case "~re":
                                    if (InvokeModule.Contains("-n"))
                                        Notifier.Module.NoSalesStoresReport.Instance.Region = item.Split(':')[1];
                                    break;
                                case "~pos":
                                    if (InvokeModule.Contains("-n"))
                                        Notifier.Module.NoSalesStoresReport.Instance.PointOfSale = int.Parse(item.Split(':')[1]);
                                    break;
                                case "~m":
                                    if (InvokeModule.Contains("-p"))
                                    {
                                        Notifier.Module.ProfitLossReport.Instance.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0).AddMonths(-1 * (int.Parse(item.Split(':')[1])));
                                        Notifier.Module.ProfitLossReport.Instance.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0).AddMonths(-1 * (int.Parse(item.Split(':')[1]) - 1)).AddSeconds(-1);
                                    }
                                    if (InvokeModule.Contains("-t"))
                                    {
                                        Notifier.Module.SalesTallyReport.Instance.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0).AddMonths(-1 * (int.Parse(item.Split(':')[1])));
                                        Notifier.Module.SalesTallyReport.Instance.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0).AddMonths(-1 * (int.Parse(item.Split(':')[1]) - 1)).AddSeconds(-1);
                                    }
                                    break;
                                case "~d":
                                    if (InvokeModule.Contains("-n"))
                                        Notifier.Module.NoSalesStoresReport.Instance.Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59).AddDays(-1d * double.Parse(item.Split(':')[1]));
                                    break;
                                case "~mo":
                                    if (InvokeModule.Contains("-m"))
                                        Notifier.Module.MonthlySalesReport.Instance.Month = item.Split(':')[1];
                                    break;
                                case "~yr":
                                    if (InvokeModule.Contains("-m"))
                                        Notifier.Module.MonthlySalesReport.Instance.Year = item.Split(':')[1];
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region Invoke Module
                    foreach (string item in InvokeModule)
                    {
                        switch (item)
                        {
                            case "-t":
                                Console.WriteLine("Sale Tally Report Module is Running...");
                                if (Notifier.Module.SalesTallyReport.Instance.VerificationReportData())
                                    Console.WriteLine("Sales wasn't tally. Notifier mail sent.");
                                else
                                    Console.WriteLine("Sales was tally. No mail sent.");
                                break;
                            case "-p":
                                Console.WriteLine("Profit/Loss Report Module is Running...");
                                if (Notifier.Module.ProfitLossReport.Instance.VerificationReportData())
                                    Console.WriteLine("Profit/loss retrieved data successfully. Notifier mail sent.");
                                else
                                    Console.WriteLine("Profit/loss had no data. No mail sent.");
                                break;
                            case "-n":
                                Console.WriteLine("No Sales Stores Report Module is Running....");
                                if (Notifier.Module.NoSalesStoresReport.Instance.VerificationReportData())
                                    Console.WriteLine("There are stores that have no sales the last 3 days. Notifier email Sent.");
                                else
                                    Console.WriteLine("there is no store that has no sales the last 3 days. No email Sent.");
                                break;
                            case "-m":
                                Console.WriteLine("Monthly Sale Report Module is Running....");
                                if (Notifier.Module.MonthlySalesReport.Instance.VerificationReportData())
                                    Console.WriteLine("Monthly sale retrieved data successfully. Notifier mail sent.");
                                else
                                    Console.WriteLine("Monthly sale had no data. No mail sent.");
                                break;

                            #region DO NOT REMOVE THIS PART
                            default:
                                Console.WriteLine("Please execute with /? for more HELP. (e.g. Notifier.exe /?)");
                                break;
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            catch (Notifier.Module.MailBaseException mbex)
            {
                Console.WriteLine(string.Format("Notifier Application Stopped by Error From {2}. [{0} <|||> {1}]", mbex.Message, mbex.StackTrace, mbex.GetType().Name));
            }
            catch (Notifier.Module.SalesTallyReportException strex)
            {
                Console.WriteLine(string.Format("Notifier Application Stopped by Error From {2}. [{0} <|||> {1}]", strex.Message, strex.StackTrace, strex.GetType().Name));
            }
            catch (Notifier.Module.NoSalesStoresReportException nssrex)
            {
                Console.WriteLine(string.Format("Notifier Application Stopped by Error From {2}. [{0} <|||> {1}]", nssrex.Message, nssrex.StackTrace, nssrex.GetType().Name));
            }
            catch (Notifier.Module.ProfitLossReportException plrex)
            {
                Console.WriteLine(string.Format("Notifier Application Stopped by Error From {2}. [{0} <|||> {1}]", plrex.Message, plrex.StackTrace, plrex.GetType().Name));
            }
            catch (Notifier.Module.MonthlySalesReportException msrex)
            {
                Console.WriteLine(string.Format("Notifier Application Stopped by Error From {2}. [{0} <|||> {1}]", msrex.Message, msrex.StackTrace, msrex.GetType().Name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Notifier Application Stopped by Error From {2}. [{0} <|||> {1}]", ex.Message, ex.StackTrace, ex.GetType().Name));
            }

            if (Settings.Default.IsTestingPurpose)
            {
                Console.ReadKey();
            }
        }
    }
}
