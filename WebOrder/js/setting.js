/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.2-vsdoc.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="common_functions.js" />

settings = {
    _loaded: false,
    numOfRecords: 15, // specify number of records to get at once
    dateDisplayFormat: "d MMM yyyy",
    dateDisplayFormatWithTime: "d-MMM-yyyy HH:mm:ss",
    dateValueFormat: "yyyy-MM-dd HH:mm:ss",
    defaultPage: "goodsordering",
    fnLoad: function (callback) {
        // Default values
        localStorage.serviceURL = connection.serverAddress + '/synchronization/MobileWS.asmx';
        settings.crReportLocation = connection.serverAddress + "/CRReport/CRReport.aspx";

        settings._loaded = true;
        if (callback) callback();

        //DAL.fnCreateTablesIfNotExist(function () {
        //    DAL.fnLoadAllSetting(function (data) {
        //        if (data != null) {
        //            for (i = 0; i < data.rows.length; i++) {
        //                //if (data.rows.item(i).PropName == 'companyName') {
        //                //    localStorage.companyName = data.rows.item(i).propValue;
        //                //};
        //                if (data.rows.item(i).PropName == 'serviceURL') {
        //                    localStorage.serviceURL = data.rows.item(i).propValue;
        //                };
        //                //if (data.rows.item(i).PropName == 'PointOfSaleID') {
        //                //    sessionStorage.PointOfSaleID = data.rows.item(i).propValue;
        //                //};
        //            }
        //        };

        //        settings._loaded = true;
        //        if (callback) callback(); 
        //    });
        //});
    },
    fnSave: function (callback) {
        //localStorage.companyName = $("#companyName").val();
        localStorage.serviceURL = $("#serviceURL").val();
        //sessionStorage.PointOfSaleID = $("#PointOfSaleID").val();
        
        //DAL.fnSaveSetting("companyName", localStorage.companyName, function () {
            DAL.fnSaveSetting("serviceURL", localStorage.serviceURL, function () {
                //DAL.fnSaveSetting("PointOfSaleID", sessionStorage.PointOfSaleID, function () {
                    if (callback) callback();
                //});
            });
        //});
    }    
}

