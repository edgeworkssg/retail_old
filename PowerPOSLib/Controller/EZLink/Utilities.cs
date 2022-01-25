using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PowerPOS.EZLink
{
    /// <summary>
    /// Class contains generic useful functions used by other classes
    /// </summary>
    class Utilities
    {
        /// <summary>
        /// Convert integer to Binary Coded Decimat format.
        /// <para>e.g. 128 = byte[] {0x12, 0x28}</para>
        /// </summary>
        /// <param name="val">Integer to be converted</param>
        /// <param name="byteArrayLength">Indicate length of the result bytes array, 0 for arbitraty length</param>
        /// <returns>Bytes array converted</returns>
        public static byte[] intToBytesBCD(int val, int byteArrayLength)
        {
            int byteCount = 0;
            int temp = val;
            byte[] retval;

            if (byteArrayLength == 0)
            {
                while (temp > 0)
                {
                    temp /= 100; //2 digits = 1 byte
                    ++byteCount;
                }

                retval = new byte[byteCount];
            }
            else if (byteArrayLength > 0)
            {
                retval = new byte[byteArrayLength];
            }
            else
            {
                return null;
            }

            for (int i = (retval.Length - 1); i >= 0; i--)
            {
                retval[i] = (byte)(val % 10);
                val /= 10;
                retval[i] |= (byte)((val % 10) << 4);
                val /= 10;
            }

            return retval;
        }

        /// <summary>
        /// Convert byte array to hexadecimal string format.
        /// <para>e.g. byte[] {0x12, 0x28} = "12 28"</para>
        /// </summary>
        /// <param name="data">Byte array to be converted</param>
        /// <returns>String converted</returns>
        public static string bytesArrayToHex(byte[] data)
        {
            string retval = string.Empty;

            try
            {
                retval = BitConverter.ToString(data).Replace("-", " ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return retval;
        }

        /// <summary>
        /// Convert ASCII string to bytes array.
        /// <para>e.g. "00" = byte[] {0x30, 0x30}</para>
        /// </summary>
        /// <param name="str">String to be converted, in ASCII</param>
        /// <returns>Bytes array</returns>
        public static byte[] asciiStrToBytesArray(string str)
        {
            return (Encoding.ASCII.GetBytes(str));
        }

        /// <summary>
        /// Convert bytes array to ASCII encoded string
        /// </summary>
        /// <param name="data">Bytes array to be converted</param>
        /// <returns>ASCII string converted from byte array</returns>
        public static string bytesArrayToASCIIString(byte[] data)
        {
            string retval = string.Empty;

            try
            {
                retval = Encoding.ASCII.GetString(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return retval;
        }

        /// <summary>
        /// Convert bytes array in BCD format to integer
        /// <para>E.g. {0x01, 0x28} => 128</para>
        /// </summary>
        /// <param name="data">Bytes array in BCD format</param>
        /// <returns>Converted integer</returns>
        public static int bytesBCDToInt(byte[] data)
        {
            int retval = 0;

            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    retval *= 100;

                    retval += (((int)(data[i] & 0xf0) >> 4) * 10) + ((int)(data[i] & 0x0f));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                retval = 0;
            }

            return retval;
        }

        /// <summary>
        /// Remove extra space between words in a string
        /// <para>e.g. "A    b" => "A b"</para>
        /// </summary>
        /// <param name="str">string to be trimmed</param>
        /// <returns>Trimmed string</returns>
        public static string removeExtraSpaceBtwnWords(string str)
        {
            return (Regex.Replace(str, @"\s+", " "));
        }

        /// <summary>
        /// Format decimal to currenty format
        /// <para>e.g. 1.23 => "$1.23"</para>
        /// </summary>
        /// <param name="d">Decimal to be formatted</param>
        /// <returns>Formatted currency string</returns>
        public static string decimalToCurrencyFormat(decimal d)
        {
            return (String.Format("{0:C}", d));
        }

        /// <summary>
        /// Convert Hexadecimal String to Bytes array
        /// <para>e.g. "01 02" => {0x01, 0x02}</para>
        /// </summary>
        /// <param name="hexStr">String in hexadecimal format</param>
        /// <returns>Byte array representing "hexStr"</returns>
        public static byte[] hexStrToBytesArray(string hexStr)
        {
            string[] strings = hexStr.Split(' ');
            byte[] retval = new byte[strings.Length];

            try
            {
                for (int i = 0; i < retval.Length; i++)
                {
                    retval[i] = (byte)Convert.ToInt32(strings[i], 16);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return retval;
        }

        /// <summary>
        /// Convert Hexadecimal String to Bytes array
        /// <para>e.g. "01 02" => {0x01, 0x02}</para>
        /// </summary>
        /// <param name="hexStr">String in hexadecimal format</param>
        /// <returns>Byte array representing "hexStr"</returns>
        public static byte[] hexStrToBytesArray1(string hexStr)
        {
            string[] strings = hexStr.Split(' ');
            byte[] retval = new byte[strings.Length];

            try
            {
                for (int i = 0; i < retval.Length; i++)
                {
                    retval[i] = (byte)Convert.ToInt32(strings[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return retval;
        }

        /// <summary>
        /// Convert String to currency format string
        /// <para>e.g. "123" => "$1.23"</para>
        /// </summary>
        /// <param name="str">numeric string</param>
        /// <returns>String in currency format</returns>
        public static string stringToCurrencyFormat(string str)
        {
            string retval = string.Empty;

            try
            {
                decimal amt = 0;
                decimal.TryParse(str, out amt);

                if (amt > 0)
                {
                    amt /= 100;
                }

                retval = decimalToCurrencyFormat(amt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return retval;
        }
    }
}
