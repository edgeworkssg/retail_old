using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
using PowerPOS.Container;

namespace PowerPOS
{   
    public partial class AttributesLabelController
    {
        public const int MaxAttributes = 7;

        public static void FetchProductAttributeLabel()
        {
            AttributesLabelCollection arb = new AttributesLabelCollection();
            arb.OrderByAsc(AttributesLabel.Columns.AttributesNo);
            arb.Load();

            for (int i = 0; i < arb.Count; i++)
            {
                switch (arb[i].AttributesNo)
                {
                    case 1:
                        ProductAttributeInfo.Attributes1 = arb[i].Label;
                        break;
                    case 2:
                        ProductAttributeInfo.Attributes2 = arb[i].Label;
                        break;
                    case 3:
                        ProductAttributeInfo.Attributes3 = arb[i].Label;
                        break;
                    case 4:
                        ProductAttributeInfo.Attributes4 = arb[i].Label;
                        break;
                    case 5:
                        ProductAttributeInfo.Attributes5 = arb[i].Label;
                        break;
                    case 6:
                        ProductAttributeInfo.Attributes6 = arb[i].Label;
                        break;
                    case 7:
                        ProductAttributeInfo.Attributes7 = arb[i].Label;
                        break;
                    case 8:
                        ProductAttributeInfo.Attributes8 = arb[i].Label;
                        break;
                }
            }

        }

        public static Dictionary<int, string> GetAttributesLabel()
        {
            Dictionary<int, string> Rst = new Dictionary<int, string>();

            AttributesLabelCollection AttLbl = new AttributesLabelCollection();
            AttLbl.Load();
            for (int Counter = 0; Counter < AttLbl.Count; Counter++)
            {
                if (AttLbl[Counter].AttributesNo > 0 && AttLbl[Counter].AttributesNo <= MaxAttributes)
                    Rst.Add(AttLbl[Counter].AttributesNo, AttLbl[Counter].Label);
            }

            return Rst;
        }

        public static bool SaveAttributesLabel(int attributeno, string lbl)
        {
            bool result = false;
            AttributesLabel at = new AttributesLabel(attributeno);
            if (at != null && at.IsLoaded && at.AttributesNo == attributeno)
            {
                //edit
                at.Label = lbl;
                at.Save(UserInfo.username);
                return true;
            }
            else
            {
                //insert
                AttributesLabel atNew = new AttributesLabel();
                atNew.AttributesNo = attributeno;
                atNew.Label = lbl;
                atNew.Save(UserInfo.username);
                return true;
            }
            return result;

            

        }

    }
}
