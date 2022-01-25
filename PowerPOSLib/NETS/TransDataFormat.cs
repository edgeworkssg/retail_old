using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Class to store information contained in trans data in the response returned by NETS machine after debit
    /// </summary>
    class TransDataFormat
    {
        private Dictionary<string, string> details = new Dictionary<string, string>();

        /// <summary>
        /// Constructor, will proceeed with dissecting data and store into "details" <see cref="Dictionary"/> 
        /// </summary>
        /// <param name="data">Mesasge data retrieved from response, in String</param>
        /// <param name="cepasVersion">Cepas Version retrieve from response, used to indicate which way to extract information, as both information contain different fields</param>
        public TransDataFormat(string data, string cepasVersion)
        {
            int idx = 0;
            string temp = string.Empty;
            string key = string.Empty;
            int length = 0;

            if (cepasVersion.Equals(NETSConstants.MessageData.CEPASVersion.CEPAS_1.Value))
            {
                #region //Transaction Type
                length = 2;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Transaction Type";

                switch (temp)
                {
                    case ("01"):
                        addKeyValueToDetails(key, temp);
                        break;
                        
                    case ("96"):
                        addKeyValueToDetails(key, "# # #");
                        break;
                }
                #endregion

                #region //Transaction Amount
                length = 12;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Transaction Amount";

                temp = Utilities.stringToCurrencyFormat(temp);

                addKeyValueToDetails(key, temp);
                #endregion

                #region //Sign Cert (CSC)
                length = 4;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Sign Cert (CSC)";

                addKeyValueToDetails(key, temp);
                #endregion

                #region //Transaction Date
                length = 6;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Transaction Date";

                DateTime dt = DateTime.Now;
                DateTime.TryParseExact(temp, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                addKeyValueToDetails(key, dt.ToString(NETSConstants.DISPLAY_DATE_FORMAT));
                #endregion

                #region //Transaction Time
                length = 6;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Transaction Time";

                dt = DateTime.Now;
                DateTime.TryParseExact(temp, "HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                addKeyValueToDetails(key, dt.ToString("HH:mm:ss"));
                #endregion

                #region //Card Trans Counter (CTC)
                length = 6;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Card Trans Counter (CTC)";

                addKeyValueToDetails(key, temp);
                #endregion

                #region //TRP for CEPAS 1.0
                length = 6;
                temp = data.Substring(idx, length);
                idx += length;
                key = "TRP for CEPAS 1.0";

                addKeyValueToDetails(key, temp);
                #endregion

                #region //MAC
                length = 6;
                temp = data.Substring(idx, length);
                idx += length;
                key = "MAC";

                addKeyValueToDetails(key, temp);
                #endregion

                #region //Card Balance
                length = 12;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Card Balance";

                temp = Utilities.stringToCurrencyFormat(temp);

                addKeyValueToDetails(key, temp);
                #endregion

                #region //RFU
                length = 4;
                temp = data.Substring(idx, length);
                idx += length;
                key = "RFU";

                addKeyValueToDetails(key, temp);
                #endregion
            }
            else if (cepasVersion.Equals(NETSConstants.MessageData.CEPASVersion.CEPAS_2.Value))
            {
                string transactionAmountSideNote = string.Empty;

                #region //Transaction Type
                length = 2;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Transaction Type";

                switch (temp)
                {
                    case ("01"):
                        addKeyValueToDetails(key, "Normal C/L Purchase");
                        transactionAmountSideNote = "C/L Purchase Amount without Service Fee";
                        break;

                    case ("03"):
                        addKeyValueToDetails(key, "Top Up Credited");
                        transactionAmountSideNote = "Top Up Amount without Top Up Fee";
                        break;

                    case ("13"):
                        addKeyValueToDetails(key, "Top Up Not Credited");
                        break;

                    case ("23"):
                        addKeyValueToDetails(key, "Top Up Unconfirmed");
                        break;

                    case ("33"):
                        addKeyValueToDetails(key, "Host Declined");
                        break;
                }
                #endregion

                #region //Transaction Amount
                length = 12;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Transaction Amount";

                temp = Utilities.stringToCurrencyFormat(temp);

                addKeyValueToDetails(key, temp);
                #endregion

                #region //Transaction Date
                length = 6;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Transaction Date";

                DateTime dt = DateTime.Now;
                DateTime.TryParseExact(temp, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                addKeyValueToDetails(key, dt.ToString(NETSConstants.DISPLAY_DATE_FORMAT));
                #endregion

                #region //Transaction Time
                length = 6;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Transaction Time";

                dt = DateTime.Now;
                DateTime.TryParseExact(temp, "HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                addKeyValueToDetails(key, dt.ToString("HH:mm:ss"));
                #endregion

                #region //Prior Card Balance
                length = 12;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Prior Card Balance";

                temp = Utilities.stringToCurrencyFormat(temp);

                addKeyValueToDetails(key, temp);
                #endregion

                #region //Post Card Balance
                length = 12;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Post Card Balance";

                temp = Utilities.stringToCurrencyFormat(temp);

                addKeyValueToDetails(key, temp);
                #endregion

                #region //Post Auto Load Balance
                length = 12;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Post Auto Load Balance";

                temp = Utilities.stringToCurrencyFormat(temp);

                addKeyValueToDetails(key, temp);
                #endregion

                #region //Post Card Status
                length = 2;
                temp = data.Substring(idx, length);
                idx += length;
                key = "Post Card Status";

                switch (temp)
                {
                    case ("00"):
                        addKeyValueToDetails(key, "Purse Disabled + ATU Disabled");
                        break;

                    case ("01"):
                        addKeyValueToDetails(key, "Purse Enabled + ATU Disabled");
                        break;

                    case ("02"):
                        addKeyValueToDetails(key, "Purse Disabled + ATU Enabled");
                        break;

                    case ("03"):
                        addKeyValueToDetails(key, "Purse Enabled + ATU Enabled");
                        break;
                }
                #endregion
            }
        }

        /// <summary>
        /// Reset this trans data
        /// </summary>
        public void reset()
        {
            details.Clear();
        }

        /// <summary>
        /// Check if "details" Dictionary contains any value
        /// </summary>
        /// <returns>True if "details" Dictionay does not have any key value pair in it, otherwise False</returns>
        public bool isEmpty()
        {
            return (details.Count == 0);
        }

        /// <summary>
        /// Validate key and value pair and check for key's existense to determine whether to add new key or replace existing value
        /// </summary>
        /// <param name="key">String</param>
        /// <param name="value">String</param>
        protected void addKeyValueToDetails(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            if (string.IsNullOrEmpty(value))
            {
                value = "-NA-";
            }

            if (details.ContainsKey(key))
            {
                details[key] = value;
            }
            else
            {
                details.Add(key, value);
            }
        }

        /// <summary>
        /// Convert "details" Dictionary to presentable string for viewing purpose
        /// </summary>
        /// <returns></returns>
        public override string  ToString()
        {
            string retval = Environment.NewLine + "\t*******************************" + Environment.NewLine;

            foreach (KeyValuePair<string, string> kvp in details)
            {
                retval += "\t" + kvp.Key + " : " + kvp.Value + Environment.NewLine;
            }

            retval += "\t*******************************" + Environment.NewLine;

            return retval;
        }
    }
}
