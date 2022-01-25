using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Notifier.Helper
{
    public class Logger
    {
        public static void WriteLogToFile(string message)
        {
            string fileLocation = string.Format(@"{0}\{1}Log.txt", Directory.GetParent(Assembly.GetAssembly(typeof(Logger)).Location).FullName, Assembly.GetAssembly(typeof(Logger)).GetName().Name);

            if (!File.Exists(fileLocation))
            {
                FileStream fs = File.Create(fileLocation);
                fs.Close();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(File.ReadAllText(fileLocation));
            sb.AppendLine(message);

            File.WriteAllText(fileLocation, sb.ToString());
        }

        public static void WriteLogToFile(string format, params string[] parameters)
        {
            WriteLogToFile(string.Format(format, parameters));
        }
    }
}
