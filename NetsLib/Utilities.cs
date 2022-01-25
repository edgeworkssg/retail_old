using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Text.RegularExpressions;
using NBit;

namespace PowerPOS.Nets
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
                //Logger.writeLog("Error Converting bytes to Hex" +ex.ToString());
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

        public static byte[] asciiStrToBytesArrayLittleEndian(string str)
        {
            return ReverseBytes(Encoding.ASCII.GetBytes(str));
        }

        public static byte[] ReverseBytes(byte[] inArray)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(inArray);

            return inArray;
        }

        static void Swap(ref byte a, ref byte b) {
            var temp = a;
            a = b;
            b = temp;
        }

        public static string reverseString(string input)
        {
            string output = "";
            if (input.Length % 2 != 0)
                return output;

            if (input.Length <= 2)
            {
                output = input;
                return output;
            }

            for (int i = input.Length / 2; i > 0; i--)
            {
                output += input.Substring(i * 2 - 2, 2);
            }

            return output;
            

        }

        static byte[] FlipInt16(byte[] rawData) {
            for (var i = 0; i < rawData.Length; i += 2) // Step two for 2x8 bits=16
                Swap(ref rawData[i], ref rawData[i + 1]);
            return rawData;
        }

        static byte[] FlipInt32(byte[] rawData) {
            for (var i = 0; i < rawData.Length; i += 4)  {// Step four for 4x8 bits=32
                Swap(ref rawData[i + 0], ref rawData[i + 2]);
                Swap(ref rawData[i + 1], ref rawData[i + 3]);
            }
            return rawData;
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
                //Logger.writeLog("Error Converting bytes to ASCII" + ex.ToString());
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
                //Logger.writeLog("Error Converting bytes to Int" + ex.ToString());

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
                //Logger.writeLog("Error Converting Hex to Array" + ex.ToString());
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
                //Logger.writeLog("Error Converting string to Currency" + ex.ToString());
            }

            return retval;
        }

        /// <summary>
        /// Convert an integer to a string of hexidecimal numbers.
        /// </summary>
        /// <param name="n">The int to convert to Hex representation</param>
        /// <param name="len">number of digits in the hex string. Pads with leading zeros.</param>
        /// <returns></returns>
        public static String IntToHexString(Int64 n, int len)
        {
            char[] ch = new char[len--];
            for (int i = len; i >= 0; i--)
            {
                ch[len - i] = ByteToHexChar((byte)((uint)(n >> 4 * i) & 15));
            }
            return new String(ch);
        }

        /// <summary>
        /// Convert a byte to a hexidecimal char
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static char ByteToHexChar(byte b)
        {
            if (b < 0 || b > 15)
                throw new Exception("IntToHexChar: input out of range for Hex value");
            return b < 10 ? (char)(b + 48) : (char)(b + 55);
        }

        /// <summary>
        /// Convert a hexidecimal string to an base 10 integer
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int HexStringToInt(String str)
        {
            int value = 0;
            for (int i = 0; i < str.Length; i++)
            {
                value += HexCharToInt(str[i]) << ((str.Length - 1 - i) * 4);
            }
            return value;
        }

        /// <summary>
        /// Convert a hex char to it an integer.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static int HexCharToInt(char ch)
        {
            if (ch < 48 || (ch > 57 && ch < 65) || ch > 70)
                throw new Exception("HexCharToInt: input out of range for Hex value");
            return (ch < 58) ? ch - 48 : ch - 55;
        }

        /// <summary>
        /// Convert string hex to bytesarray.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string CalculateCRC(byte[] input)
        {
            var std = Crc.Crc32;
            var crc = std.Compute(input);
            return crc.ToString("X");
        }

        public static string AddTransparency(string input)
        {
            //string result = "";
            //Change byte 0x10 to 0x10 0x10
            int ct = 0;
            while (ct < input.Length/2 )
            {
                
                string tmp = input.Substring(ct * 2, 2);
                if (tmp == "10")
                {
                    input = input.Remove(ct * 2, 2);
                    input = input.Insert(ct * 2, "1010");
                    ct += 2;
                    continue;
                }
                ct++;

            }
            //Change byte 0x04 to 0x10 0x01
            ct = 0;
            while (ct < input.Length/2 )
            {
                
                string tmp = input.Substring(ct * 2, 2);
                if (tmp == "04")
                {
                    input = input.Remove(ct * 2, 2);
                    input = input.Insert(ct * 2, "1001");
                    ct += 2;
                    continue;
                }
                ct++;

            }
            
            //Change byte 0x02 to 0x10 0x00
            ct = 0;
            while (ct < input.Length / 2)
            {

                string tmp = input.Substring(ct * 2, 2);
                if (tmp == "02")
                {
                    input = input.Remove(ct * 2, 2);
                    input = input.Insert(ct * 2, "1000");
                    ct += 2;
                    continue;
                }
                ct++;

            }
            return input;
        }

        public static string RemoveTransparency(string input)
        {
            //string result = "";
            //Change byte 0x10 to 0x10 0x10
            int ct = 0;
            while (ct < input.Length / 2 - 1 )
            {

                string tmp = input.Substring(ct * 2, 4);
                if (tmp == "1010")
                {
                    input = input.Remove(ct * 2, 4);
                    input = input.Insert(ct * 2, "10");
                }
                if (tmp == "1001")
                {
                    input = input.Remove(ct * 2, 4);
                    input = input.Insert(ct * 2, "04");
                }
                if (tmp == "1000")
                {
                    input = input.Remove(ct * 2, 4);
                    input = input.Insert(ct * 2, "02");
                }
                ct++;

            }
            //Change byte 0x04 to 0x10 0x01
            /*ct = 0;
            while (ct < input.Length / 2-1)
            {

                string tmp = input.Substring(ct * 2, 4);
                if (tmp == "1001")
                {
                    input = input.Remove(ct * 2, 4);
                    input = input.Insert(ct * 2, "04");
                }
                ct++;

            }

            //Change byte 0x02 to 0x10 0x00
            ct = 0;
            while (ct < input.Length / 2 - 1)
            {

                string tmp = input.Substring(ct * 2, 4);
                if (tmp == "1000")
                {
                    input = input.Remove(ct * 2, 4);
                    input = input.Insert(ct * 2, "02");
                }
                ct++;

            }*/
            return input;
        }


        
        
    }
}
