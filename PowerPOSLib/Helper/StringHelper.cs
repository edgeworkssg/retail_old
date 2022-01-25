using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PowerPOSLib.Helper
{
    public class StringHelper
    {
        public static string ConvertToProperCase(string text)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(text.ToLower());
        }
    }
}
