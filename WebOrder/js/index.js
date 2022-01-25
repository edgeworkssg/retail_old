/// <reference path="jquery-1.10.2-vsdoc.js" />
/// <reference path="sammy.js" />
/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="date.js" />
/// <reference path="setting.js" />

var timeoutHandler;

// Declare SammyJS. Executed in Document ready
var sammyApp = $.sammy(function () {
    this.get('', function () {
        // if the Hash change, call fnLoadPage to "refresh" the page
        fnLoadPage();
    });
});

// Document ready
$(function () {
    $("#btnSignIn").click(function (e) {
        e.preventDefault();
        InventoryWebWS.fnLogin($("#username").val(), $("#password").val(), function (result) {
            if (result.result == true) {
                var user = {
                    username: result.UserName,
                    displayName: result.DisplayName,
                    role: result.Role,
                    departmentID: result.DeptID,
                    pointOfSaleID: result.PointOfSaleID,
                    locationID: result.InventoryLocation.InventoryLocationID,
                    locationName: result.InventoryLocation.InventoryLocationName,
                    InventoryLocationCollection: result.InventoryLocationCollection,
                    privileges: result.Privileges,
                    userToken: result.UserToken,
                    isSupplier: result.isSupplier !== undefined ? result.isSupplier : false,
                    isRestrictedSupplierList: result.isRestrictedSupplierList !== undefined ? result.isRestrictedSupplierList : false,
                    isUseUserPortal: result.isUseUserPortal !== undefined ? result.isUseUserPortal : false,
                    isUseTransferApproval: result.isUseTransferApproval !== undefined ? result.isUseTransferApproval : false
                };

                fnSetUserSession(user);

                if ($("#remember").is(':checked')) {
                    localStorage.setItem("user", JSON.stringify(user));
                };

                localStorage.companyName = result.CompanyName;
                $("#headerTitle").text(localStorage.companyName);

                DAL.fnDropAllTables(function () {
                    fnInitializeEquipPDA(function (num_of_records) {
                        console.log(num_of_records + ' records have been added to database.')
                        $("#loginModal").modal('hide');
                        fnLoadPage();
                    });
                });
            }
            else {
                $("#loginModal").modal('hide');
                BootstrapDialog.alert(result.status, function () {
                    $("#loginModal").modal('show');
                });
                $("#password").select();
            };
        });
    });

    DAL.fnOpenDB(function () {
        settings.fnLoad(function () {
            if (localStorage.getItem("user")) {
                var user = JSON.parse(localStorage.getItem("user"));
                if (user.userToken) {
                    InventoryWebWS.fnCheckUserToken(user.userToken + "", function (result) {
                        if (result.valid) {
                            fnSetUserSession(user);
                        }
                        else {
                            sessionStorage.clear();
                        };
                    });
                }
                else {
                    sessionStorage.clear();
                };
            };

            $("#headerTitle").text(localStorage.getItem("companyName"));
        });
    });

    // Manually handle the navbar menu click
    $('#mainMenu, .menu-settings-narrow, .menu-settings-wide').delegate('ul.dropdown-menu > li > a', 'click', function () {
        if ($(this).attr('href') != "#") {
            sammyApp.setLocation($(this).attr('href'));
        };
        $(this).closest('.dropdown-menu').trigger('click');
        hideNavBar();
        return false;
    });

    sammyApp.run();

});

function fnSetUserSession(user) {
    sessionStorage.username = user.username;
    sessionStorage.displayName = user.displayName;
    sessionStorage.role = user.role;
    sessionStorage.departmentID = user.departmentID;
    sessionStorage.PointOfSaleID = user.pointOfSaleID;
    sessionStorage.locationID = user.locationID;
    sessionStorage.locationName = user.locationName;   
    sessionStorage.privileges = JSON.stringify(user.privileges);
    sessionStorage.userToken = user.userToken;
    sessionStorage.InventoryLocationCollection = JSON.stringify(user.InventoryLocationCollection);
    sessionStorage.isSupplier = user.isSupplier;
    sessionStorage.isRestrictedSupplierList = user.isRestrictedSupplierList;
    sessionStorage.isUseUserPortal = user.isUseUserPortal;
    sessionStorage.isUseTransferApproval = user.isUseTransferApproval;
    
    $(".userDisplayName").html("<span class='glyphicon glyphicon-user'></span> " + sessionStorage.displayName + " <span class='badge notif-count'></span>");
}

function fnGenerateMenu() {
    if (fnIsAuthenticated()) {
        $("#mainMenu").removeClass("hidden");
        $(".menu-login").addClass("hidden");
        $(".menu-logout").removeClass("hidden");
        $(".userDisplayName").removeClass("hidden");
    }
    else {
        $("#mainMenu").addClass("hidden");
        $(".menu-login").removeClass("hidden");
        $(".menu-logout").addClass("hidden");
        $(".userDisplayName").addClass("hidden");
    };

    fnShowOrHideMenu("Goods Ordering", ".menu-goods-ordering");
    fnShowOrHideMenu("Order Approval", ".menu-order-approval");
    fnShowOrHideMenu("Goods Receive", ".menu-goods-receive");
    fnShowOrHideMenu("Stock Return", ".menu-stock-return");
    fnShowOrHideMenu("Return Approval", ".menu-return-approval");
    fnShowOrHideMenu("Stock Adjustment", ".menu-stock-adjustment");
    fnShowOrHideMenu("Adjustment Approval", ".menu-adjustment-approval");
    fnShowOrHideMenu("Stock Transfer", ".menu-stock-transfer");
    fnShowOrHideMenu("Stock Take Document", ".menu-stock-take-doc");
    fnShowOrHideMenu("Stock Take", ".menu-stock-take");
    fnShowOrHideMenu("Stock Take Approval", ".menu-stock-take-approval");
    fnShowOrHideMenu("Clinic Operation Hours", ".menu-clinic-operation-hours");
    fnShowOrHideMenu("Drug Importer", ".menu-item-importer");
    fnShowOrHideMenu("Cost Importer", ".menu-cost-importer");
    fnShowOrHideMenu("Freeze/Unfreeze Clinic", ".menu-freeze-clinic");
    fnShowOrHideMenu("Email Notification", ".menu-email-notification");
    fnShowOrHideMenu("Second Screen Setup", ".menu-second-screen-setup");
    fnShowOrHideMenu("Receive Transfer", ".menu-receive-transfer");
    fnShowOrHideMenu("Transfer Approval", ".menu-transfer-approval");

    fnShowOrHideMenu("Print Stock Take", ".menu-report-stocktakedocument");
    fnShowOrHideMenu("Stock Balance Report", ".menu-report-stockbalancebyoutlet");
    fnShowOrHideMenu("Stock Card Report", ".menu-report-stockcardreport");
    fnShowOrHideMenu("Inventory Activity Report", ".menu-report-inventorytransactionreport");
    fnShowOrHideMenu("Discrepancies Report", ".menu-report-stockdiscrepanciesreport");
    fnShowOrHideMenu("Adjustment Report", ".menu-report-adjustmentreport");
    fnShowOrHideMenu("Transfer Report", ".menu-report-transferreport");
    fnShowOrHideMenu("Transfer W/O Cost Report", ".menu-report-transferreportwocost");
    fnShowOrHideMenu("Clinic Upload Status Report", ".menu-report-clinicuploadstatusreport");
    fnShowOrHideMenu("Upload History Report", ".menu-report-uploadhistoryreport");
    fnShowOrHideMenu("Dispensing Report", ".menu-report-dispensingreport");
    fnShowOrHideMenu("Clinic Drug Ordering Status Report", ".menu-report-clinicdrugorderingstatusreport");
    fnShowOrHideMenu("Recipe Setup", ".menu-ingredient-setup");
    fnShowOrHideMenu("UOM Conversion Setup", ".menu-uom-conversion-setup");
    fnShowOrHideMenu("Production", ".menu-item-cooking");
    fnShowOrHideMenu("Outlet Optimal Stock", ".menu-outlet-optimal-stock");
    fnShowOrHideMenu("Stock Transfer Variance Report", ".menu-report-stocktransfervariancereport");
    fnShowOrHideMenu("Stock Return Variance Report", ".menu-report-stockreturnvariancereport");
    fnShowOrHideMenu("Stock Order Variance Report", ".menu-report-stockorderreport");


    var dropdownList = $("#mainMenu li.dropdown");
    for (var i = 0; i < dropdownList.length; i++) {
        if ($(dropdownList[i]).find("ul.dropdown-menu li:not('.hidden')").length > 0)
            $(dropdownList[i]).removeClass("hidden");
        else
            $(dropdownList[i]).addClass("hidden");
    };
}

function fnShowOrHideMenu(privilegeName, menuId) {
    if (fnIsAuthorized(privilegeName))
        $(menuId).removeClass("hidden")
    else
        $(menuId).addClass("hidden");

    //if (sessionStorage.privileges) {
    //    var userPrivileges = JSON.parse(sessionStorage.privileges);
    //    (userPrivileges.indexOf(privilegeName) > -1) ? $(menuId).removeClass("hidden") : $(menuId).addClass("hidden");
    //}
    //else {
    //    $(menuId).addClass("hidden");
    //};
}

function fnIsAuthenticated(isAskForLogin) {
    if (sessionStorage.username == null) {
        if (isAskForLogin == true) $("#loginModal").modal({ backdrop: 'static', keyboard: false });
        return false;
    };
    return true;
}

function fnIsAuthorized(privilegeName) {
    if (sessionStorage.privileges) {
        var userPrivileges = JSON.parse(sessionStorage.privileges);
        return (userPrivileges.indexOf(privilegeName) > -1) ? true : false;
    }
    else {
        return false;
    };
}

function fnExitFromApp() {
    BootstrapDialog.confirm('Are you sure you want to Exit this application?', function (ok) {
        if (ok) {
            navigator.app.exitApp();
        };
    });
}

function fnLogin(toggleNavBar) {
    fnIsAuthenticated(true);
    //if (toggleNavBar == true) hideNavBar();
}

function fnLogout(toggleNavBar) {
    BootstrapDialog.confirm('Are you sure you want to Log out?', function (ok) {
        if (ok) {
            sessionStorage.clear();
            localStorage.removeItem("user");
            window.location.href = "index.html";
        };
    });
}

function hideNavBar() {
    if ($(".navbar-toggle").css("display") != "none") {
        $(".navbar-toggle").click();
    }
}

function fnLoadPage() {
    fnGenerateMenu();
    if (window.location.hash == "#settings") {
        fnLoadSettings();
    }
    else {
        var name = window.location.hash.substring(1);
        fnLoadContent(name);
    };
}

function fnLoadContent(name, toggleNavBar) {
    if (fnIsAuthenticated(true)) {
        if (!name) name = settings.defaultPage;  // Get default page to load from setting.js
        
        if ($("a[href='#" + name + "']").parent().hasClass("hidden") == false) {
            $("#content").load(name + ".html?" + new Date().getTime(), function (response, status, xhr) {
                if (status == "error") {
                    var msg = "Sorry but there was an error: ";
                    $("#content").html(msg + xhr.status + " " + xhr.statusText);
                };
            });
        }
        else {
            $("#content").html("Sorry but you are not authorized to view this page.");
        };

        fnGetNotificationCount();
    };
    //if (toggleNavBar == true) hideNavBar();
}

function fnLoadSettings(toggleNavBar) {
    $("#content").load("settings.html?" + new Date().getTime());
    //if (toggleNavBar == true) hideNavBar();
}

function fnLoadReport(ReportName) {
    var urlReport = settings.crReportLocation + "?r=" + ReportName + ".rpt&ut=" + sessionStorage.userToken;
    window.open(urlReport, "_blank", "width=700,height=500,location=0");
}

function fnGetNotificationCount() {
    try {
        DAL.fnGetNotifications(function (result) {
            if (result.status == "") {
                $(".notif-container").html("<li class='pull-right'><a href='#' onclick='event.preventDefault(); fnGetNotificationCount();'><small><span class='glyphicon glyphicon-refresh'></span> refresh</small></a></li>");
                if (result.notifications.length > 0) {
                    $(".notif-count").html(result.notifications.length);
                    for (var i = 0; i < result.notifications.length; i++) {
                        //$(".notif-container").append("<li><a href='#" + result.notifications[i].menu + "' onclick=\"fnLoadContent('" + result.notifications[i].menu + "', true)\">" + result.notifications[i].message + "</a></li>");
                        $(".notif-container").append("<li><a href='#" + result.notifications[i].menu + "'>" + result.notifications[i].message + "</a></li>");
                    };
                }
                else {
                    $(".notif-count").html("");
                    $(".notif-container").append("<li><a href='#' onclick='event.preventDefault()'>No notification</a></li>");
                };
            };
        });
    } finally {
        if (timeoutHandler) clearTimeout(timeoutHandler);
        timeoutHandler = setTimeout(fnGetNotificationCount, 1000 * 60 * 10); // 10 minutes
    };
}