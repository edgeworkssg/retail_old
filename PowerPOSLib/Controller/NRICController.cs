using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public class NRICController
    {

        private static readonly int[] Multiples = { 2, 7, 6, 5, 4, 3, 2 };

        public static bool ICVerification(string nric, 
            out int birthYear,
            out bool isForeigner, 
            out string status)
        {
            bool isValid = false;
            birthYear = 0;
            isForeigner = false;
            status = "";
            nric = (nric + "").Trim().ToUpper();

            try
            {
                if (nric.Length != 9)
                    throw new Exception("Invalid length of NRIC " + nric);

                if (nric.Substring(0, 1) != "S" &&
                    nric.Substring(0, 1) != "T" &&
                    nric.Substring(0, 1) != "F" &&
                    nric.Substring(0, 1) != "G")
                    throw new Exception("Invalid first character of NRIC " + nric);

                if (!char.IsLetter(nric[8]))
                    throw new Exception("Invalid last character of NRIC " + nric);

                int numericNric = 0;
                int total = 0;
                int count = 0;
                if (!int.TryParse(nric.Substring(1, nric.Length - 2), out numericNric))
                    throw new Exception("Invalid character of NRIC ");

                while (numericNric != 0)
                {
                    total += numericNric % 10 * Multiples[Multiples.Length - (1 + count++)];

                    numericNric /= 10;
                }

                char first = nric[0];
                char last = nric[8];

                char[] outputs;
                if (first == 'S')
                {
                    outputs = new char[] { 'J', 'Z', 'I', 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A' };
                }
                else
                {
                    outputs = new char[] { 'G', 'F', 'E', 'D', 'C', 'B', 'A', 'J', 'Z', 'I', 'H' };
                }

                isValid = (last == outputs[total % 11]);


                if (nric[0] == 'S' || nric[0] == 'T')
                {
                    //born before 2000
                    if (nric[0] == 'S')
                    {
                        if (nric[1] == '0' || nric[1] == '1')
                        {
                            birthYear = 1967;
                        }
                        else
                        {
                            string strYear = nric.Substring(1, 2);
                            int yr = int.Parse(strYear);
                            birthYear = 1900 + yr;
                        }
                    }
                    else
                    {
                        string strYear = nric.Substring(1, 2);
                        int yr = int.Parse(strYear);
                        birthYear = 2000 + yr;
                    }
                }
                else if (nric[0] == 'F' || nric[0] == 'G')
                {
                    birthYear = 2017;
                }

                isForeigner = nric[0] != 'S' && nric[0] != 'T';
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message;
                //Logger.writeLog(ex);
            }


            return isValid;
        }

        /*public static bool isNAN(char c)
        {
            return !Character.isDigit(c);
        }

        public static int valueOf(char c)
        {
            return (((int)c) - 48);
        }

        public static char at(String from, int index)
        {
            return from.charAt(index);
        }*/
    }
}
