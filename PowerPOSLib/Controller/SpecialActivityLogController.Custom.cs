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

namespace PowerPOS
{
    /// <summary>
    /// Controller class for SpecialActivityLog
    /// </summary>
    public partial class SpecialActivityLogController
    {
        public const decimal DISCOUNT_LIMIT = 15;
    }        
}
