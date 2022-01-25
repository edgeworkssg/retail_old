using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public class SpecialBarcodeController
    {
        public static int ScanBarcode(string barcode, out ViewItem theItem, out decimal price, out decimal quantity, out string recordedDigit,  out string status)
        {
            int result = -1;
            theItem = null;
            price = 0;
            quantity = 0;
            recordedDigit = "";
            status = "";

            try
            {
                int checkDigit = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.BarcodeCheckDigit) + "").GetIntValue();
                string checkDigitValue = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.BarcodeCheckValue) + "");

                int itemStart = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.ItemDigitStart) + "").GetIntValue();
                int itemLength = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.ItemDigitLength) + "").GetIntValue();

                int priceStart = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.PriceDigitStart) + "").GetIntValue();
                int priceLength = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.PriceDigitLength) + "").GetIntValue();
                int recordedStart = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.RecordedDigitStart) + "").GetIntValue();
                int recordedLength = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.RecordedDigitLength) + "").GetIntValue();

                int quantityStart = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.QuantityDigitStart) + "").GetIntValue();
                int quantityLength = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.QuantityDigitLength) + "").GetIntValue();
                int integerDigit = (AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.IntegerDigitLength) + "").GetIntValue();

                bool isWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.UseSpecialBarcodeForQuantity), false);

                int minLength = checkDigit + itemLength;
                if (isWeight)
                    minLength = minLength + quantityLength;
                else
                    minLength = minLength + priceLength;

                string barcodeCheckDigit;
                try
                {
                    barcodeCheckDigit = barcode.Substring(checkDigit - 1, checkDigitValue.Length);
                }
                catch
                {
                    throw new Exception("Barcode check digit or check digit value setting is incorrect. Please check the settings.");
                }

                if (!barcodeCheckDigit.ToUpper().Equals((checkDigitValue+"").ToUpper()))
                {
                    result = 1;
                    throw new Exception("Barcode check digit is mismatched");
                }

                if (barcode.Length < minLength)
                    throw new Exception("Special Barcode Length is Wrong. Please check the settings.");

                string itemNo = barcode.Substring(itemStart - 1, itemLength);

                theItem = new ItemController().FetchByBarcode(itemNo);
                if (theItem != null && theItem.IsLoaded && !theItem.IsNew)
                {
                    if (isWeight)
                    {
                        string qtyStr = barcode.Substring(quantityStart - 1, quantityLength);
                        if (qtyStr.Length > 2)
                        {
                            //int integerDigit = 2;
                            if (integerDigit == 0) integerDigit = 2;
                            decimal integerValue = qtyStr.Substring(0, integerDigit).GetDecimalValue();
                            string decimalValueStr = qtyStr.Substring(integerDigit, (qtyStr.Length - integerDigit));
                            decimal decimalValue = decimalValueStr.GetDecimalValue();

                            for (int i = 0; i < decimalValueStr.Length; i++)
                                decimalValue = decimalValue / 10;
                            quantity = integerValue + decimalValue;
                        }
                        else
                        {
                            quantity = qtyStr.GetDecimalValue();
                        }
                        price = theItem.RetailPrice;
                    }
                    else
                    {
                        price = barcode.Substring(priceStart-1, priceLength).GetDecimalValue();
                        price = price / 100;
                        quantity = 1;
                    }

                    recordedDigit = barcode.Substring(recordedStart - 1, recordedLength);

                    result = 0;
                }
                else
                {
                    throw new Exception("Barcode does not exist in the system.");
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
            }

            return result;
        }
    }
}
