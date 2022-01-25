<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAppCalendarDaily.ascx.cs"
    Inherits="PowerWeb.Appointment.UCAppCalendarDaily" %>
<style type="text/css">
    .ddl
    {
        width: 250px !important;
    }
    .ddl option
    {
        width: 400px !important;
    }
</style>

<script type="text/javascript" src="https://www.google.com/jsapi"></script>

<script type="text/javascript">
    google.load("visualization", "1", { packages: ["table"] });

    $(document).ready(function() {
        $("#tabs").tabs({
            activate: function(event, ui) {
                var active = $('#tabs').tabs('option', 'active');
                if (active == 0) {
                    PrevForm();
                } else if (active == 1) {
                    NextForm();
                }
            }
        });

        $("#<%= dialogForm.ClientID %>").dialog({
            autoOpen: false,
            height: 550,
            width: 700,
            modal: true
        });

        $("#<%= txtDate.ClientID %>").datepicker({ dateFormat: 'dd M yy' });
        $("#<%= txtStartDate.ClientID %>").datepicker({ dateFormat: 'dd M yy' });
        $("#<%= btnDelete.ClientID %>").button().click(function(event) {
            event.preventDefault();
            DeleteApp();
        });
        $("#<%= btnContinue.ClientID %>").button().click(function(event) {
            event.preventDefault();
            NextForm();
        });

        $("#<%= btnSaveApp.ClientID %>").button().click(function(event) {
            event.preventDefault();
            SaveApp();
        });

        $("#<%= btnSearchMember.ClientID %>").button().click(function(event) {
            event.preventDefault();
            SearchMember();
        });
        $("#<%= btnNewMember.ClientID %>").button().click(function(event) {
            event.preventDefault();
            NewMember();
        });
        LoadCalendar('TODAY');

        var isCreateAppointment = $("#<%= isCreateAppointment.ClientID %>").val();
        if (isCreateAppointment.toLowerCase() == 'true') {
            $('#divButton').show();
        } else {
            $('#divButton').hide();
        }
        
        var ddPointOfSaleID = $("#<%= DDPointOfSaleID.ClientID %>");
        ddPointOfSaleID.off();
        ddPointOfSaleID.on('change', function(evt) {
            LoadOutletSales();
        });
    });

    function SearchMember() {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "Services/AppointmentService.asmx/FetchMembership",
            data: '{"search": "' + escape($("#<%= txtCustomerNo.ClientID %>").val()) + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            success: function(result) {
                //alert(result.d);
                BindMember(result.d);
                HideLoading();
                $("#<%= txtCustomerNo.ClientID %>").val('');
            },
            error: function(err) {
                ServiceFailed(err);
            }
        });
    };
    
    function LoadOutletSales(callbackEvent) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "Services/AppointmentService.asmx/FetchOutletSales",
            data: '{"pointOfSaleID": "' + escape($("#<%= DDPointOfSaleID.ClientID %>").val()) + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            beforeSend: function() {
                $("#<%= ddlSalesPerson.ClientID %>").html('');
            },
            success: function(response) {
                var outletSales = JSON.parse(response.d);
                var htmlContent = '';
                $.each(outletSales, function(key, value) {
                    htmlContent += '<option value="' + value.UserName + '">' + value.UserName + '</option>';
                });
                $("#<%= ddlSalesPerson.ClientID %>").html(htmlContent);
                
                if(callbackEvent != undefined &&  typeof callbackEvent == 'function') {
                    setTimeout(function() {
                        callbackEvent();
                    }, 1);
                }
            },
            error: function(err) {
                ServiceFailed(err);
            },
            complete: function() {
                HideLoading();
            }
        });
    }
    
    function LoadResources(callbackEvent) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "Services/AppointmentService.asmx/FetchResources",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            beforeSend: function() {
                $("#<%= ddlResource.ClientID %>").html('');
            },
            success: function(response) {
                var resources = JSON.parse(response.d);
                var htmlContent = '';
                $.each(resources, function(key, value) {
                    htmlContent += '<option value=' + value.ResourceId + '>' + value.ResourceName + '</option>';
                });
                $("#<%= ddlResource.ClientID %>").html(htmlContent);
                
                if(callbackEvent != undefined &&  typeof callbackEvent == 'function') {
                    setTimeout(function() {
                        callbackEvent();
                    }, 1);
                }
            },
            error: function(err) {
                ServiceFailed(err);
            },
            complete: function() {
                HideLoading();
            }
        });
    }
    
    function LoadMembershipGroup(callbackEvent) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "Services/AppointmentService.asmx/FetchMembershipGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            beforeSend: function() {
                $("#<%= ddlMembershipGroup.ClientID %>").html('');
            },
            success: function(response) {
                var resources = JSON.parse(response.d);
                var htmlContent = '';
                $.each(resources, function(key, value) {
                    htmlContent += '<option value=' + value.MembershipGroupId + '>' + value.GroupName + '</option>';
                });
                $("#<%= ddlMembershipGroup.ClientID %>").html(htmlContent);
                
                if(callbackEvent != undefined &&  typeof callbackEvent == 'function') {
                    setTimeout(function() {
                        callbackEvent();
                    }, 1);
                }
            },
            error: function(err) {
                ServiceFailed(err);
            },
            complete: function() {
                HideLoading();
            }
        });
    }

    function ServiceFailed(xhr) {
        HideLoading();
        var errText = '';
        for (var member in xhr) {
            errText += member + ' : ' + xhr[member] + '\n';
        }
        console.log(errText);
        alert('Error occured!');
        return;
    };
    var table;
    var data;
    function BindMember(dataValues) {
        data = new google.visualization.DataTable(dataValues);

        if (data.getNumberOfRows() == 0) {
            $("#divMemberGrid").hide();
            $("#divMemberForm").hide();
            alert('No membership found');
        }
        else if (data.getNumberOfRows() == 1) {
            BindMembership(data.getValue(0, 0));
        }
        else {
            $("#divMemberGrid").show();
            $("#divMemberForm").hide();
            table = new google.visualization.Table(document.getElementById('divMemberTable'));
            table.draw(data,
            {
                showRowNumber: true,
                page: 'enable',
                pageSize: 5,
                pagingSymbols: {
                    prev: 'prev',
                    next: 'next'
                },
                //pagingButtonsConfiguration: 'auto'
            });
            google.visualization.events.addListener(table, 'select', membershipSelected);
        }
    }

    function membershipSelected(e) {
        var selectedData = table.getSelection(), row, item;
        row = selectedData[0].row;
        item = data.getValue(row, 0);
        BindMembership(item);
    };

    function BindMembership(membershipNo) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "Services/AppointmentService.asmx/FetchMembershipByMembershipNo",
            data: '{"membershipNo": "' + membershipNo + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            success: function(result) {
                var member = result.d;
                if (member.Status == "success") {
                    $("#divMemberGrid").hide();
                    $("#divMemberForm").show();
                    $("#<%= txtName.ClientID %>").val(member.NameToAppear);
                    $("#<%= txtNRIC.ClientID %>").val(member.NRIC);
                    $("#<%= txtMembershipNo.ClientID %>").val(member.MembershipNo);
                    $("#<%= txtMobile.ClientID %>").val(member.Mobile);
                    $("#<%= txtEmail.ClientID %>").val(member.Email);
                    $("#<%= txtAddress.ClientID %>").val(member.StreetName);
                    $("#<%= txtAddress2.ClientID %>").val(member.StreetName2);
                    $("#<%= ddlMembershipGroup.ClientID %>").val(member.MembershipGroupId);

                    DisableForm('true');
                }
                else {
                    alert(member.Status);
                }
                HideLoading();
            },
            error: function(err) {
                ServiceFailed(err);
            }
        });
    };

    function DisableForm(val) {
        if (val == 'true') {
            $("#<%= txtName.ClientID %>").attr('readonly', val);
            $("#<%= txtNRIC.ClientID %>").attr('readonly', val);
            $("#<%= txtMobile.ClientID %>").attr('readonly', val);
            $("#<%= txtEmail.ClientID %>").attr('readonly', val);
            $("#<%= txtAddress.ClientID %>").attr('readonly', val);
            $("#<%= txtAddress2.ClientID %>").attr('readonly', val);
            $("#<%= ddlMembershipGroup.ClientID %>").attr('disabled', 'disabled');
            $("#<%= trMembershipNo.ClientID %>").show();
        }
        else {
            $("#<%= txtName.ClientID %>").removeAttr('readonly');
            $("#<%= txtNRIC.ClientID %>").removeAttr('readonly');
            $("#<%= txtMobile.ClientID %>").removeAttr('readonly');
            $("#<%= txtEmail.ClientID %>").removeAttr('readonly');
            $("#<%= txtAddress.ClientID %>").removeAttr('readonly');
            $("#<%= txtAddress2.ClientID %>").removeAttr('readonly');
            $("#<%= ddlMembershipGroup.ClientID %>").removeAttr('disabled');
            $("#<%= trMembershipNo.ClientID %>").hide();
        }
    };

    function NewMember() {
        $("#divMemberGrid").hide();
        $("#divMemberForm").show();
        $("#<%= txtName.ClientID %>").val('');
        $("#<%= txtNRIC.ClientID %>").val('');
        $("#<%= txtMobile.ClientID %>").val('');
        $("#<%= txtEmail.ClientID %>").val('');
        $("#<%= txtAddress.ClientID %>").val('');
        $("#<%= txtAddress2.ClientID %>").val('');
        $("#<%= txtMembershipNo.ClientID %>").val('');
        $("#<%= ddlMembershipGroup.ClientID %>").prop('selectedIndex', 0);

        DisableForm('false');
    };

    function DeleteApp() {
        if (confirm("Are you sure want to delete this appointment") == true) {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "Services/AppointmentService.asmx/DeleteAppointment",
                data: '{"appointmentID": "' + escape($("#<%= hfAppID.ClientID %>").val()) + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                processdata: true,
                success: function(result) {
                    if (result.d == "success") {
                        LoadCalendar('FIND');
                    }
                    else {
                        alert(result.d);
                    }
                    HideLoading();
                },
                error: function(err) {
                    ServiceFailed(err);
                }
            });
        }
    };

    function NextForm() {
        $("#divAppDetail").hide();
        $("#divAppMember").show();
        $("#<%= btnSaveApp.ClientID %>").show();
        $("#<%= btnContinue.ClientID %>").hide();
        $("#divMemberGrid").hide();
        $("#divMemberForm").hide();
        if ($("#<%= txtMembershipNo.ClientID %>").val() != "") {
            BindMembership($("#<%= txtMembershipNo.ClientID %>").val());
        }
        $('#tabs').tabs("option", "active", 1);
    };

    function PrevForm() {
        $("#divAppDetail").show();
        $("#divAppMember").hide();
        $("#<%= btnSaveApp.ClientID %>").hide();
        $("#<%= btnContinue.ClientID %>").show();
        $("#divMemberGrid").hide();
        $("#divMemberForm").hide();
        $('#tabs').tabs("option", "active", 0);
    }

    function SaveApp() {
        ShowLoading();
        
        var data = {
            ID: $("#<%= hfAppID.ClientID %>").val(),
            SalesPerson: $("#<%= ddlSalesPerson.ClientID %>").val(),
            Service: $("#<%= ddlServices.ClientID %>").val(),
            Date: $("#<%= txtDate.ClientID %>").val(),
            Hour: $("#<%= ddlHour.ClientID %>").val(),
            Minute: $("#<%= ddlMinutes.ClientID %>").val(),
            AMPM: $("#<%= ddlAMPM.ClientID %>").val(),
            Duration: $("#<%= ddlDuration.ClientID %>").val(),
            Remark: $("#<%= txtDescription.ClientID %>").val(),
            MembershipNo: $("#<%= txtMembershipNo.ClientID %>").val(),
            Name: $("#<%= txtName.ClientID %>").val(),
            NRIC: $("#<%= txtNRIC.ClientID %>").val(),
            Mobile: $("#<%= txtMobile.ClientID %>").val(),
            Email: $("#<%= txtEmail.ClientID %>").val(),
            Address: $("#<%= txtAddress.ClientID %>").val(),
            Address2: $("#<%= txtAddress2.ClientID %>").val(),
            MembershipGroupID: $("#<%= ddlMembershipGroup.ClientID %>").val(),
            PointOfSaleID: $('#<%= DDPointOfSaleID.ClientID %>').val(),
            ResourceId: $("#<%= ddlResource.ClientID %>").val()
        };
        var isSuccess = false;
        if (data.MembershipNo == '') {
            if ($("#divMemberForm").is(":visible")) {
                if (data.Name == '') {
                    alert('Please fill Name!');
                    HideLoading();
                    return;
                }
                else if (data.Mobile == '') {
                    alert('Please fill Mobile!');
                    HideLoading();
                    return;
                }
                isSuccess = true;
            }
            else {
                alert('Please select member');
                HideLoading();
                return;
            }
        }
        else
            isSuccess = true;
        if (isSuccess) {
            //console.log(JSON.stringify(data));
            ShowLoading();
//            $('#<%= DDFilterPointOfSaleID.ClientID %>').val('0');
            $.ajax({
                type: "POST",
                url: "Services/AppointmentService.asmx/SaveAppointment",
                data: '{"data": "' + escape(JSON.stringify(data)) + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                processdata: true,
                success: function(result) {
                    if (result.d == "success") {
                        LoadCalendar('FIND');
                    }
                    else {
                        alert(result.d);
                    }
                    HideLoading();
                },
                error: function(err) {
                    ServiceFailed(err);
                }
            });
            HideLoading();
        }
    };

    function ResetForm() {
        $("#<%= ddlHour.ClientID %>").val('10');
        $("#<%= ddlMinutes.ClientID %>").val('00');
        $("#<%= ddlAMPM.ClientID %>").val('AM');
        $("#<%= txtMembershipNo.ClientID %>").val('');
        $("#<%= hfAppID.ClientID %>").val('0');
        $("#<%= txtDescription.ClientID %>").val('');
        $("#<%= ddlDuration.ClientID %>").val('30');
        $("#<%= txtCustomerNo.ClientID %>").val('');
        $("#<%= ddlServices.ClientID %>").prop('selectedIndex', 0);
        $("#<%= ddlSalesPerson.ClientID %>").prop('selectedIndex', 0);
        $("#<%= ddlResource.ClientID %>").prop('selectedIndex', 0);

        $("#<%= txtName.ClientID %>").val('');
        $("#<%= txtNRIC.ClientID %>").val('');
        $("#<%= txtMobile.ClientID %>").val('');
        $("#<%= txtEmail.ClientID %>").val('');
        $("#<%= txtAddress.ClientID %>").val('');
        $("#<%= txtAddress2.ClientID %>").val('');
        $("#<%= ddlMembershipGroup.ClientID %>").prop('selectedIndex', 0);
        $("#<%= txtMembershipNo.ClientID %>").val('');
    }

    function OpenForm(selectedID, selectedEmp, selectedDate, selectedTime) {
        //alert(selectedTime);
        ResetForm();
        PrevForm();
        LoadResources();
        LoadMembershipGroup();
        $("#<%= DDPointOfSaleID.ClientID %>").val($("#<%= DDFilterPointOfSaleID.ClientID %>").val());
                        
        if (selectedID == 0) {
            console.log();
            LoadOutletSales(function() {
                if (selectedEmp != "") $("#<%= ddlSalesPerson.ClientID %>").val(selectedEmp);
            });
            
            ShowLoading();        
            $("#<%= txtDate.ClientID %>").val(selectedDate);
            //if (selectedEmp != "") $("#<%= ddlSalesPerson.ClientID %>").val(selectedEmp);
            $("#<%= ddlHour.ClientID %>").val(selectedTime.split('_')[0]);
            $("#<%= ddlMinutes.ClientID %>").val(selectedTime.split('_')[1]);
            $("#<%= ddlAMPM.ClientID %>").val(selectedTime.split('_')[2]);
            $("#<%= txtMembershipNo.ClientID %>").val('');
            $("#<%= hfAppID.ClientID %>").val('0');
            $("#<%= btnDelete.ClientID %>").hide();
            HideLoading();
        }
        else {
            $("#<%= hfAppID.ClientID %>").val(selectedID);
            $("#<%= btnDelete.ClientID %>").show();
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "Services/AppointmentService.asmx/FetchAppointmentByID",
                data: '{"appointmentID": "' + selectedID + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                processdata: true,
                success: function(result) {
                    LoadOutletSales();
                    var app = result.d;
                    //setTimeout(function(){
                        if (app.Status == "success") {
                            selectedEmp = app.SalesPerson;
                            $("#<%= txtDate.ClientID %>").val(app.Date);
                            $("#<%= ddlSalesPerson.ClientID %>").val(app.SalesPerson);
                            $("#<%= ddlResource.ClientID %>").val(app.ResourceId);
                            $("#<%= ddlServices.ClientID %>").val(app.Service);
                            $("#<%= ddlHour.ClientID %>").val(app.Hour);
                            $("#<%= ddlMinutes.ClientID %>").val(app.Minute);
                            $("#<%= ddlAMPM.ClientID %>").val(app.AMPM);
                            $("#<%= ddlDuration.ClientID %>").val(app.Duration);
                            $("#<%= txtDescription.ClientID %>").val(app.Remark);
                            $("#<%= txtMembershipNo.ClientID %>").val(app.MembershipNo);
                            $("#<%= DDPointOfSaleID.ClientID %>").val($("#<%= DDFilterPointOfSaleID.ClientID %>").val());
                            
                            LoadOutletSales(function() {
                                if (selectedEmp != "") $("#<%= ddlSalesPerson.ClientID %>").val(selectedEmp);
                            });
                        }
                        else {
                            alert(app.Status);
                            $("#<%= dialogForm.ClientID %>").dialog("close");
                        }
                    //},800);
                },
                error: function(err) {
                    ServiceFailed(err);
                },
                complete: function() {
                    HideLoading();
                }
            });
        }
        $("#divAppDetail").show();
        $("#divAppMember").hide();
        $("#<%= btnSaveApp.ClientID %>").hide();
        $("#<%= btnContinue.ClientID %>").show();
        $("#divMemberGrid").hide();
        $("#divMemberForm").hide();
        $("#<%= dialogForm.ClientID %>").dialog("open");
    };

    function LoadCalendar(flag) {

        $("#<%= dialogForm.ClientID %>").dialog("close");
        // $('#<%= DDFilterPointOfSaleID.ClientID %>').val('TempData');
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "Services/AppointmentService.asmx/FetchCalendarTable",
            data: '{"date": "' + $("#<%= txtStartDate.ClientID %>").val() + '","flag": "' + flag + '", "pointOfSaleID": "' + $('#<%= DDFilterPointOfSaleID.ClientID %>').val() + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            success: function(result) {
                var app = result.d;
                if (app.Status == "success") {
                    $("#<%= txtStartDate.ClientID %>").val(app.Date);
                    $("#divCalendar").html(app.Content);
                    var demo, fixedTable;
                    fixedTable = function(el) {
                        var $body, $header, $sidebar;
                        $body = $(el).find('.fixedTable-body');
                        $sidebar = $(el).find('.fixedTable-sidebar table');
                        $header = $(el).find('.fixedTable-header table');
                        return $($body).scroll(function() {
                            $($sidebar).css('margin-top', -$($body).scrollTop());
                            return $($header).css('margin-left', -$($body).scrollLeft());
                        });
                    };
                    demo = new fixedTable($('#demo'));
                }
                else {
                    alert(app.Status);
                }
                HideLoading();
            },
            error: function(err) {
                ServiceFailed(err);
            }
        });
    }

    function FindDate() {
        LoadCalendar('FIND');
    };
    function TodayDate() {
        LoadCalendar('TODAY');
    };

    function PrevDate() {
        LoadCalendar('PREV');
    };

    function NextDate() {
        LoadCalendar('NEXT');
    };

    function ShowLoading() {
        //$('html, body').animate({ scrollTop: 0 }, 'fast');
        var over = '<div id="overlay">' +
            '<img id="loading" src="images/loading.gif">' +
            '</div>';
        $(over).appendTo('body');
        $('body').css({
            'overflow': 'hidden'
        });
    }

    function HideLoading() {
        $('#overlay').remove();
        $('body').css({
            'overflow': 'auto'
        });
    }

    function NewAppointmentClick() {
        var theDate = $("#<%= txtStartDate.ClientID %>").val();
        OpenForm('', '', theDate, '10_00_AM');
        LoadOutletSales();
        LoadResources();
        LoadMembershipGroup();
    }
    
</script>

<asp:Panel ID="pnlCalendar" runat="server">
    <div>
        <input type="hidden" id="isCreateAppointment" runat="server" />
        <table style="width: 100%" id="FieldsTable">
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Appointment System%>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 130px; height: 3px">
                    <asp:Label ID="LabelX01" runat="server" Text="<%$Resources:dictionary, Point of Sale%>" /></asp:Label>
                </td>
                <td style="width: 130px; height: 3px">
                    <asp:DropDownList CssClass="ddl" ID="DDFilterPointOfSaleID" runat="server" Width="250px"
                        DataValueField="Value" DataTextField="Text" OnInit="ddlPointOFSaleID_Init">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 130px; height: 3px">
                    <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                        Text="<%$Resources:dictionary, Date%>" />
                </td>
                <td style="height: 3px">
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                    &nbsp;
                    <button id="btnFind" class="classname" runat="server" value="<<" causesvalidation="false"
                        onclick="FindDate(); return false;">
                        <asp:Literal ID="Literal39"  runat="server" Text="<%$Resources:dictionary, Find%>" /></button>
                </td>
            </tr>
            <tr>
                <td style="width: 130px; height: 3px">
                </td>
                <td style="height: 3px">
                    <button id="btnPrev" class="classname" runat="server" value="<<" causesvalidation="false"
                        onclick="PrevDate(); return false;">
                        <<</button>
                    &nbsp;
                    <button id="btnToday" class="classname" runat="server" value="Today" causesvalidation="false"
                        onclick="TodayDate(); return false;">
                        <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Today%>" /></button>
                    &nbsp;
                    <button id="btnNext" class="classname" runat="server" value=">>" causesvalidation="false"
                        onclick="NextDate(); return false;">
                        >></button>
                </td>
            </tr>
            <tr id="trNewAppointment" runat="server">
                <td style="width: 130px; height: 3px">
                </td>
                <td style="height: 3px">
                    <button id="btnNew" runat="server" causesvalidation="false" value="New Appointment"
                        title="New Appointment" class="classname" style="min-width: 155px!important;"
                        onclick="NewAppointmentClick(); return false;">
                        <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, New Appointment%>" /></button>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <div id="divCalendar">
        </div>
    </div>
</asp:Panel>
<div id="dialogForm" runat="server" title="<%$Resources:dictionary, Appointment Detail%>" style="display: none">
    <div style="min-height: 350px">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1"><asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Appointment Detail%>" /></a></li>
                <li><a href="#tabs-2"><asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Member Detail%>" /></a></li>
            </ul>
            <div id="tabs-1">
                <div id="divAppDetail" style="min-height: 340px;">
                    <table class="tblWiz">
                        <tr>
                            <td>
                                <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Point Of Sale%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ddl" ID="DDPointOfSaleID" runat="server" Width="250px"
                                    DataValueField="Value" DataTextField="Text" OnInit="ddlPointOFSaleID_Init">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Provider (staff)%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <%--<asp:DropDownList CssClass="ddl" ID="ddlSalesPerson" runat="server" Width="250px"
                                    DataValueField="UserName" DataTextField="DisplayName" OnInit="ddlSalesPerson_Init">
                                </asp:DropDownList>--%>
                                <asp:DropDownList CssClass="ddl" ID="ddlSalesPerson" runat="server" Width="250px"
                                    DataValueField="UserName" DataTextField="DisplayName">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Room%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ddl" ID="ddlResource" runat="server" Width="250px" DataValueField="ResourceId"
                                    DataTextField="ResourceName">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Service%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ddl" ID="ddlServices" runat="server" Width="250px" DataValueField="ItemNo"
                                    DataTextField="ItemName" OnInit="ddlServices_Init">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, Day/Time%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDate" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlHour" runat="server" Width="60px">
                                                <asp:ListItem Value="01" Text="01" />
                                                <asp:ListItem Value="02" Text="02" />
                                                <asp:ListItem Value="03" Text="03" />
                                                <asp:ListItem Value="04" Text="04" />
                                                <asp:ListItem Value="05" Text="05" />
                                                <asp:ListItem Value="06" Text="06" />
                                                <asp:ListItem Value="07" Text="07" />
                                                <asp:ListItem Value="08" Text="08" />
                                                <asp:ListItem Value="09" Text="09" />
                                                <asp:ListItem Value="10" Text="10" />
                                                <asp:ListItem Value="11" Text="11" />
                                                <asp:ListItem Value="12" Text="12" />
                                            </asp:DropDownList>
                                            &nbsp; :
                                            <asp:DropDownList ID="ddlMinutes" runat="server" Width="60px">
                                                <asp:ListItem Value="00" Text="00" />
                                                <asp:ListItem Value="30" Text="30" />
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:DropDownList ID="ddlAMPM" runat="server" Width="60px">
                                                <asp:ListItem Value="AM" Text="AM" />
                                                <asp:ListItem Value="PM" Text="PM" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Duration%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDuration" runat="server" Width="150px">
                                    <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                    <asp:ListItem Value="60" Text="60"></asp:ListItem>
                                    <asp:ListItem Value="90" Text="90"></asp:ListItem>
                                    <asp:ListItem Value="120" Text="120"></asp:ListItem>
                                    <asp:ListItem Value="150" Text="150"></asp:ListItem>
                                    <asp:ListItem Value="180" Text="180"></asp:ListItem>
                                    <asp:ListItem Value="210" Text="210"></asp:ListItem>
                                    <asp:ListItem Value="240" Text="240"></asp:ListItem>
                                    <asp:ListItem Value="270" Text="270"></asp:ListItem>
                                    <asp:ListItem Value="300" Text="300"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Remark%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" Height="56px" TextMode="MultiLine"
                                    Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="tabs-2">
                <div id="divAppMember" style="display: none;">
                    <table class="tblWiz">
                        <tr>
                            <td style="width: 170px">
                                <asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Search Customer%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomerNo" runat="server" Width="230px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px">
                            </td>
                            <td>
                            </td>
                            <td>
                                <input id="btnSearchMember" runat="server" type="submit" value="<%$Resources:dictionary, Search%>" />
                                <input id="btnNewMember" runat="server" type="submit" value="<%$Resources:dictionary, New Member%>" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divMemberGrid" style="width: 100%; display: none;">
                    <div id="divMemberTable">
                    </div>
                </div>
                <div id="divMemberForm" style="display: none;">
                    <table class="tblWiz">
                        <tr id="trMembershipNo" runat="server">
                            <td style="width: 170px" valign="top">
                                <asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, Membership No%>" />
                            </td>
                            <td valign="top">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtMembershipNo" runat="server" Width="230px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px" valign="top">
                                <asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Name%>" /><span class="required">*</span>
                            </td>
                            <td valign="top">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" Width="230px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px" valign="top">
                                <asp:Literal ID="Literal14"  runat="server" Text="<%$Resources:dictionary, NRIC%>" />
                            </td>
                            <td valign="top">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtNRIC" runat="server" Width="230px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px" valign="top">
                                <asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, Mobile%>" /><span class="required">*</span>
                            </td>
                            <td valign="top">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" Width="230px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px">
                                <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, Email%>" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="230px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Address%>" />
                            </td>
                            <td valign="top">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" Width="230px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px">
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress2" runat="server" Width="230px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px" valign="top">
                                <asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, Group%>" />
                            </td>
                            <td valign="top">
                                :
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ddl" ID="ddlMembershipGroup" runat="server" Width="230px"
                                    DataValueField="MembershipGroupId" DataTextField="GroupName">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div id="divButton" align="right">
        <input id="btnDelete" runat="server" type="submit" value="<%$Resources:dictionary, Delete%>" />
        <input id="btnContinue" runat="server" type="submit" value="<%$Resources:dictionary, Continue%>" />
        <input id="btnSaveApp" runat="server" type="submit" value="<%$Resources:dictionary,Save Appointment %>" />
    </div>
    <asp:HiddenField ID="hfAppID" runat="server" />
</div>

<script type="text/javascript">
    (function() {
        var demo, fixedTable;

        fixedTable = function(el) {
            var $body, $header, $sidebar;
            $body = $(el).find('.fixedTable-body');
            $sidebar = $(el).find('.fixedTable-sidebar table');
            $header = $(el).find('.fixedTable-header table');
            return $($body).scroll(function() {
                $($sidebar).css('margin-top', -$($body).scrollTop());
                return $($header).css('margin-left', -$($body).scrollLeft());
            });
        };

        demo = new fixedTable($('#demo'));

    }).call(this);    
</script>

