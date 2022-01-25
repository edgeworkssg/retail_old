<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        SubSonic.DataService.Provider.ConnectionStringName = "PowerPOS";
        PowerWeb.BLL.Helper.Utility.DecryptDefaultConnString();
        // Code that runs on application startup
        if (PowerPOS.AppSetting.GetSettingFromDBAndConfigFile("PointOfSaleID") != null)
        {
            int posID = 1;
            int.TryParse(ConfigurationManager.AppSettings["PointOfSaleID"], out posID);            
            Application["PointOfSaleID"] = posID;            
        }
        else
        {
            Application["PointOfSaleID"] = 1;
        }
        PowerPOS.Container.PointOfSaleInfo.MembershipPrefixCode = "S";
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        // Make sure AttachedParticular table exists.
        PowerPOS.MembershipController.CheckAttachedParticularTable();

        PowerPOS.DbUtilityController.UpdateDBStructure();

        PowerPOS.DbUtilityController.SetIsStockOutRunningFalse();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        PowerPOS.AttributesLabelController.FetchProductAttributeLabel();
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
</script>
