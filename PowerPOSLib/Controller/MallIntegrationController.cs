using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.IO;

namespace PowerPOS
{
    public partial class MallIntegrationController
    {
        OrderHdr oHdr;
        OrderDetCollection oDet;
        POSController pos;
        decimal amountPayable;
        private ExchangeLog log;

        
        public bool GenerateFile
            (out string status)
        {
            status = "";
            try
            {
                string tenantID = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.MallIntegration_TenantID);
                if (tenantID == "")
                {
                    status = "Error. Tenant ID is Empty."; 
                    return false;
                }
                string outputDirectory = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.MallIntegration_OutputDirectory);
                if (outputDirectory == "")
                {
                    status = "Error. Output Directory is Empty.";
                    return false;
                }

                outputDirectory = outputDirectory + "/" + pos.GetOrderDate().ToString("yyyyMMdd");
                
                #region *) Check: if Archive directory exists or not. If not, then create one
                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);
                #endregion

                string outputFileName = tenantID + "_" + pos.GetOrderDate().ToString("yyyyMMdd") + "_" + pos.GetOrderDate().ToString("HHmmss") + ".txt";

                string arguments = tenantID + "|" + pos.GetUnsavedCustomRefNo() + "|";

                if (pos.myOrderHdr.NettAmount > 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (pos.recDet.Count >= i + 1)
                        {

                            ReceiptDet rd = pos.recDet[i];
                            arguments = arguments + rd.Amount.ToString("N2") + "|";

                            switch (rd.PaymentType.ToUpper())
                            {
                                case "CASH": arguments = arguments + "1|"; break;
                                case "VISA": arguments = arguments + "2|"; break;
                                case "MASTER": arguments = arguments + "3|"; break;
                                case "AMEX": arguments = arguments + "4|"; break;
                                case "NETS": arguments = arguments + "5|"; break;
                                case "EASYLINK": arguments = arguments + "6|"; break;
                                case "VOUCHER": arguments = arguments + "7|"; break;
                                default: arguments = arguments + "8|"; break;
                            }

                        }
                        else
                        {
                            arguments = arguments + "0.00|0|";
                        }
                    }
                    //GST    
                    arguments = arguments + pos.GetGSTAmount().ToString("N2") + "|";
                    //Discount
                    arguments = arguments + pos.CalculateTotalDiscount().ToString("N2") +"|";

                }
                else
                {
                    decimal totalOrderDet = 0;
                    decimal totalGST = 0;
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        totalOrderDet = totalOrderDet + od.Amount;
                        totalGST = totalGST + (decimal)od.GSTAmount;
                    }
                    if (totalOrderDet < 0)
                    {
                        arguments = arguments + (totalOrderDet).ToString("N2") + "|";
                        arguments = arguments + "1|";
                        arguments = arguments + "0.00|0|";
                        arguments = arguments + "0.00|0|";
                        //GST    
                        arguments = arguments + (totalGST).ToString("N2") + "|";

                        //Discount
                        arguments = arguments + "0.00|";
                    }
                    else
                    {
                        arguments = arguments + (totalOrderDet).ToString("N2") + "|";
                        arguments = arguments + "0|";
                        arguments = arguments + "0.00|0|";
                        arguments = arguments + "0.00|0|";
                        //GST    
                        arguments = arguments + "0.00|";

                        //Discount
                        arguments = arguments + "0.00|";
                    }
                }
                arguments = arguments + pos.GetOrderDate().ToString("yyyyMMdd") + "|";
                arguments = arguments + pos.GetOrderDate().ToString("HH") + "|";
                arguments = arguments + pos.GetOrderDate().ToString("mm") + "|";
                arguments = arguments + pos.GetOrderDate().ToString("ss") + "|";

                //membership
                arguments = arguments + "0|" + (pos.GetMemberInfo().MembershipNo == "WALK-IN" ? "0" : pos.GetMemberInfo().MembershipNo);
                using (FileStream stream = new FileStream(outputDirectory + "\\" + outputFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    TextWriter tw = new StreamWriter(stream);
                    tw.Write(arguments);
                    tw.Flush();
                    tw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool GenerateVoid
            (out string status)
        {
            status = "";
            try
            {
                string tenantID = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.MallIntegration_TenantID);
                if (tenantID == "")
                {
                    status = "Error. Tenant ID is Empty.";
                    return false;
                }
                string outputDirectory = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.MallIntegration_OutputDirectory);
                if (outputDirectory == "")
                {
                    status = "Error. Output Directory is Empty.";
                    return false;
                }

                outputDirectory = outputDirectory + "/" + pos.GetOrderDate().ToString("yyyyMMdd");

                #region *) Check: if Archive directory exists or not. If not, then create one
                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);
                #endregion

                string outputFileName = tenantID + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".txt";

                string arguments = tenantID  + "|" + pos.GetUnsavedCustomRefNo() + "V|";

                if (pos.myOrderHdr.NettAmount > 0 )
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (pos.recDet.Count >= i + 1)
                        {
                            ReceiptDet rd = pos.recDet[i];
                            arguments = arguments + (rd.Amount * -1).ToString("N2") + "|";
                            switch (rd.PaymentType.ToUpper())
                            {
                                case "CASH": arguments = arguments + "1|"; break;
                                case "VISA": arguments = arguments + "2|"; break;
                                case "MASTER": arguments = arguments + "3|"; break;
                                case "AMEX": arguments = arguments + "4|"; break;
                                case "NETS": arguments = arguments + "5|"; break;
                                case "EASYLINK": arguments = arguments + "6|"; break;
                                case "VOUCHER": arguments = arguments + "7|"; break;
                                default: arguments = arguments + "8|"; break;
                            }

                        }
                        else
                        {
                            arguments = arguments +  "0.00|0|";
                        }
                    }
                    //GST    
                    arguments = arguments + (pos.GetGSTAmount() * -1).ToString("N2") + "|";

                    //Discount
                    arguments = arguments + (pos.CalculateTotalDiscount() * -1).ToString("N2") +"|";
                }
                else
                {
                    decimal totalOrderDet= 0;
                    decimal totalGST = 0;
                    foreach(OrderDet od in pos.myOrderDet)
                    {
                        totalOrderDet = totalOrderDet + od.Amount;
                        totalGST = totalGST + (decimal)od.GSTAmount;
                    }
                    if (totalOrderDet < 0)
                    {
                        arguments = arguments + (totalOrderDet * -1).ToString("N2") + "|";
                        arguments = arguments + "1|";
                        arguments = arguments + "0.00|0|";
                        arguments = arguments + "0.00|0|";
                        //GST    
                        arguments = arguments + (totalGST * -1).ToString("N2") + "|";

                        //Discount
                        arguments = arguments + "0.00|";
                    }
                    else
                    {
                        arguments = arguments + (totalOrderDet * -1).ToString("N2") + "|";
                        arguments = arguments + "0|";
                        arguments = arguments + "0.00|0|";
                        arguments = arguments + "0.00|0|";
                        //GST    
                        arguments = arguments + (totalGST * -1).ToString("N2") + "|";

                        //Discount
                        arguments = arguments + "0.00|";
                    }
                    
                }
                
                arguments = arguments + DateTime.Now.ToString("yyyyMMdd") + "|";
                arguments = arguments + DateTime.Now.ToString("HH") + "|";
                arguments = arguments + DateTime.Now.ToString("mm") + "|";
                arguments = arguments + DateTime.Now.ToString("ss") + "|";

                //membership
                arguments = arguments + "0|" + (pos.GetMemberInfo().MembershipNo == "WALK-IN" ? "0" : pos.GetMemberInfo().MembershipNo);
                using (FileStream stream = new FileStream(outputDirectory + "\\" + outputFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    TextWriter tw = new StreamWriter(stream);
                    tw.Write(arguments);
                    tw.Flush();
                    tw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        
        public MallIntegrationController(string OrderHdrID) 
        {            
            try
            {
                if (OrderHdrID != null && OrderHdrID.ToString() != "")
                {
                    pos = new POSController(OrderHdrID);
                    //Get the order
                    oHdr = new OrderHdr(OrderHdrID);                    
                    if (!oHdr.IsNew) 
                        oDet = oHdr.OrderDetRecords();                    

                }                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);                
            }
        }

        
    }
}
