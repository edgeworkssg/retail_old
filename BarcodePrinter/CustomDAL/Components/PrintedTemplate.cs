using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BarcodePrinter
{
    [Serializable]
    public class PrintedTemplate
    {
        public string Name = "";

        public List<PrintedComponent> Values;
        public PrintedTemplate(string TemplateName)
        {
            Values = new List<PrintedComponent>();
            Name = TemplateName;
        }

        public void SetData(DataRow Rw)
        {
            for (int Counter = 0; Counter < Values.Count; Counter++)
            {
                if (Rw.Table.Columns.Contains(Values[Counter].Name))
                {
                    Values[Counter].SetValue(Rw[Values[Counter].Name]);
                }
            }
        }

        /// <summary>
        /// Will add into (replace if exist) the temporary component list
        /// </summary>
        /// <param name="Value"></param>
        public void AddComponent(PrintedComponent Value)
        {
            DeleteComponent(Value.Name);

            Values.Add(Value);
        }

        public void DeleteComponent(string Name)
        {
            foreach (PrintedComponent oneValue in Values)
            {
                if (oneValue.Name == Name)
                {
                    Values.Remove(oneValue);
                    break;
                }
            }
        }

        public PrintedComponent LoadComponentInformation(string ComponentName)
        {
            foreach (PrintedComponent oneValue in Values)
            {
                if (oneValue.Name == ComponentName)
                    return oneValue;
            }

            return null;
        }
    }
}
