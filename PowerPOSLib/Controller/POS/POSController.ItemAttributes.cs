using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SubSonic;
using PowerPOS.Container;
using System.Data;
using System.Linq;
using System.IO;

namespace PowerPOS
{
    public partial class POSController
    {
        public string SelectedAttribute = "";

        public List<OrderDetAttribute> myOrderDetAttribute = new List<OrderDetAttribute>();

        public List<string> GetAttributesOfOrderDet(string orderDetID)
        {
            List<string> retVal = new List<string>();

            retVal = (from odt in myOrderDetAttribute
                      where odt.OrderDetID == orderDetID
                      select odt.Attribute.AttributesName).ToList();

            return retVal;
        }

        public List<string> GetReceiptAttributesOfOrderDet(string orderDetID)
        {
            List<string> retVal = new List<string>();

            retVal = (from odt in myOrderDetAttribute
                      where odt.OrderDetID == orderDetID
                            && odt.Attribute.BillPrint
                      select odt.Attribute.AttributesName).ToList();

            return retVal;
        }

        public void AssignAttribute(string orderDetID, List<string> attributes)
        {
            myOrderDetAttribute.RemoveAll(o => o.OrderDetID == orderDetID);

            foreach (var attrib in attributes)
            {
                myOrderDetAttribute.Add(new OrderDetAttribute
                {
                    AttributesCode = attrib,
                    OrderDetID = orderDetID
                });
            }
        }

        public string GetItemNoOfLine(string line)
        {
            return myOrderDet.Where(o => o.OrderDetID == line).FirstOrDefault().ItemNo;
        }

        public void ApplyDefaultAttribute(OrderDet oDet)
        {
            try
            {
                var theItem = oDet.Item;
                Attribute attrib = null;
                bool assignDefaultAttrib = false;

                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\DefaultItemAttribute.xml");
                DataTable defaultAttrib = ds.Tables[0];

                for (int i = 0; i < defaultAttrib.Rows.Count; i++)
                {
                    if (defaultAttrib.Rows[i]["ItemNo"].ToString() == theItem.ItemNo)
                    {
                        attrib = new Attribute(defaultAttrib.Rows[i]["Attribute"].ToString());
                        if (attrib != null && !attrib.IsNew)
                            assignDefaultAttrib = true;
                        break;
                    }
                }

                if (assignDefaultAttrib && attrib != null && !string.IsNullOrEmpty(attrib.AttributesCode))
                {
                    var theItemAttribMap = theItem.ItemAttributesMapRecords().Where(o => o.AttributesGroupCode == attrib.AttributesGroupCode).FirstOrDefault();
                    if (theItemAttribMap != null)
                    {
                        List<string> newAttribs = new List<string>();
                        newAttribs.Add(attrib.AttributesCode);
                        AssignAttribute(oDet.OrderDetID, newAttribs);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
