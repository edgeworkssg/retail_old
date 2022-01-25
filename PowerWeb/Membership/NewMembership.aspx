<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="NewMembership.aspx.cs" Inherits="PowerWeb.NewMembership" Title="Registration & Indemnity Form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/css/tooltipster.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/css/themes/tooltipster-light.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
    <input type="file" name="FileItem" id="FileItem" class="hide" />
    <div class="page" id="panelForm">
        <div class="panel align-right crud-button-panel top" id="panelButtonForm" style="margin-left: 0px;">
            <button class="button" id="btnNew">
                <i class="fa fa-file"></i>&nbsp;New</button>
            <%--<button class="button" id="btnSave">
                <i class="fa fa-save"></i>&nbsp;Save</button>--%>
        </div>
        <div align="center">
            Welcome to Happy Willow!.<br />
            In accordance with our Play Rules, every entrant to Happy Willow is required to
            complete this Registration & Indemnity Form. We respect your privacy and all information
            collected are managed in accordance with our Data Privacy Statement.
        </div>
        <div class="col-2 section-row section-top border-bottom" style="padding-bottom: 30px;
            margin-bottom: -20px;">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">Parent's Particular</span></label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
        </div>
        <div class="col-2 top-row">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">Surname</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input title-case" data-parsley-required="true"  
                        id="ParentSurname" name="ParentSurname" />
                </div>
            </div>
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">Given Name</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input title-case" data-parsley-required="true" id="ParentGivenName"
                        name="ParentGivenName" />
                </div>
            </div>
        </div>
        <div class="col-2">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">NRIC/FIN/Pp No.</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input" data-parsley-required="true"  
                        id="ParentNRIC" name="ParentNRIC" />
                </div>
            </div>
        </div>
        <div class="col-2 clear-both no-padding-top" style="padding-top: 13px;">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">Home Address</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input title-case" data-parsley-required="true" id="ParentHomeAddress"
                        name="ParentHomeAddress" />
                </div>
            </div>
        </div>
        <div class="col-2 clear-both no-padding-top" style="padding-top: 13px;">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">Contact No.</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input" data-parsley-required="true"  
                        id="ParentContactNo" name="ParentContactNo" />
                </div>
            </div>
        </div>
        <div class="col-2 clear-both no-padding-top" style="padding-top: 13px;">
            <div class="form-group">
                <label class="datepicker">
                    <span style="font-size: 13px;">Email Address</span>
                </label>
                <div class="element-wrapper">
                    <input type="text" class="long-input email-case" data-parsley-type="email" id="ParentEmail"
                        name="ParentEmail" />
                </div>
                <div style="font-style: italic!important">
                    If you would like to receive updates on Happy Willow’s exciting activities and promotions,
                    kindly provide your email address here.
                </div>
            </div>
        </div>
        <div class="col-2 clear-both no-padding-top" style="padding-top: 13px;">
            <div class="form-group">
                <label class="datepicker">
                    <span style="font-size: 13px;">Passcode</span>
                </label>
                <div class="element-wrapper">
                    <input type="password" class="long-input data-parsley-type="required" data-parsley-minlength="4" id="Passcode"  maxlength="4"
                        name="Passcode" data-parsley-maxlength="4" data-parsley-type="number" data-parsley-error-message="Pass Code should be numeric and must be 4 digit. Please change your Pass Code"/
                        data-parsley-equalto="#ConfirmPasscode" data-parsley-equalto-error-message="Pass Code not same. Please check your Pass Code.">
                </div>
            </div>
        </div>
        <div class="col-2 clear-both no-padding-top" style="padding-top: 13px;">
            <div class="form-group">
               <label class="datepicker">
                    <span style="font-size: 13px;">Confirm Passcode</span>
                </label>
                <div class="element-wrapper">
                    <input type="password" class="long-input data-parsley-type="required" data-parsley-minlength="4" id="ConfirmPasscode" maxlength="4"
                        name="ConfirmPasscode" data-parsley-maxlength="4" data-parsley-equalto="#Passcode" data-parsley-error-message="Pass Code not same. Please check your Pass Code."/>
                </div>
            </div>
        </div>
        <!--
        <div class="col-2">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">Expiry Date</span></label>
                <div class="element-wrapper">
                    <input type="text" style="width: 100px;" class="long-input datepicker" data-parsley-required="true"
                        id="ExpiryDate" name="ExpiryDate" />
                    <%--<img class="btn-date" element="Child1DateOfBirth" src="<%= ResolveUrl("~/App_Themes/Default/image/Calendar_scheduleHS.png") %>" />--%>
                </div>
            </div>
        </div>
        -->
        <input type="hidden" style="width: 100px;" class="long-input datepicker" data-parsley-required="true"
            id="ExpiryDate" name="ExpiryDate" />
        <br />
        <br />
        <br />
        <div class="col-1 section-row top-row section-non-top border-bottom">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;"></span>
                </label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
            <div class="form-group">
                <label class="">
                    <span style="font-size: 15px; margin-bottom: 5px;">Child(ren)’s Particulars</span></label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
        </div>
        <div class="col-2">
            <%-- <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">1st Child Surname</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input" data-parsley-required="true"  
                        id="Child1Surname" name="Child1Surname" />
                </div>
            </div>--%>
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">1st Child Given Name</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input title-case" data-parsley-required="false" id="Child1GivenName"
                        name="Child1GivenName" />
                </div>
            </div>
        </div>
        <div class="col-2">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">Date of Birth</span></label>
                <div class="element-wrapper" id="date-wrapper-1">
                    <%--<input type="text" style="width: 100px;" class="long-input datepicker" data-parsley-required="true"
                        id="Child1DateOfBirth" name="Child1DateOfBirth" />--%>
                    <select name="Child1DateOfBirthDate" id="Child1DateOfBirthDate" class="datepicker-date"
                        style="width: 50px;">
                    </select>
                    <select name="Child1DateOfBirthMonth" id="Child1DateOfBirthMonth" class="datepicker-month"
                        style="width: 100px">
                    </select>
                    <select name="Child1DateOfBirthYear" id="Child1DateOfBirthYear" class="datepicker-year"
                        style="width: 70px">
                    </select>
                    <%--<img class="btn-date" element="Child1DateOfBirth" src="<%= ResolveUrl("~/App_Themes/Default/image/Calendar_scheduleHS.png") %>" />--%>
                </div>
            </div>
        </div>
        <div class="col-2 clear-both" style="padding-top: 13px;">
            <%--<div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">2nd Child Surname</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input" id="Child2Surname" name="Child2Surname"   />
                </div>
            </div>--%>
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">2nd Child Given Name</span></label>
                <div class="element-wrapper">
                    <input type="text" class="long-input title-case" id="Child2GivenName" name="Child2GivenName" />
                </div>
            </div>
        </div>
        <div class="col-2">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;">Date of Birth</span></label>
                <div class="element-wrapper" id="date-wrapper-2">
                    <%--<input type="text" style="width: 100px;" class="long-input datepicker" id="Child2DateOfBirth"
                        name="Child2DateOfBirth" />--%>
                    <select name="Child2DateOfBirthDate" id="Child2DateOfBirthDate" class="datepicker-date"
                        style="width: 50px;">
                    </select>
                    <select name="Child2DateOfBirthMonth" id="Child2DateOfBirthMonth" class="datepicker-month"
                        style="width: 100px">
                    </select>
                    <select name="Child2DateOfBirthYear" id="Child2DateOfBirthYear" class="datepicker-year"
                        style="width: 70px">
                    </select>
                    <%--<img class="btn-date" element="Child2DateOfBirth" src="<%= ResolveUrl("~/App_Themes/Default/image/Calendar_scheduleHS.png") %>" />--%>
                </div>
            </div>
        </div>
        <div class="col-1 section-row top-row section-non-top border-bottom">
            <div class="form-group">
                <label class="">
                    <span style="font-size: 13px;"></span>
                </label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
            <div class="form-group">
                <label class="">
                    <span style="font-size: 15px; margin-bottom: 5px;">How did you hear about us? Please
                        tick.</span></label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
        </div>
        <div style="margin-top: 25px;">
            <span>
                <input type="checkbox" id="chkMagazines" name="chkMagazines" />Magazines </span>
            <span>
                <input type="checkbox" id="chkOnlineSearch" name="chkOnlineSearch" />Online Search
            </span><span>
                <input type="checkbox" id="chkOnlineMedia" name="chkOnlineMedia" />Online Media
            </span><span>
                <input type="checkbox" id="chkFriends" name="chkFriends" />Friends </span><span>
                    <input type="checkbox" id="chkOther" name="chkOther" />Others </span>
        </div>
        <div class="footer clear-both border-top" style="margin-top: 55px;">
            <ul>
                <li>I have read and agree to abide by the Play Rules of Happy Willow during all visits
                    to Happy Willow.</li>
                <li>I understand and agree that all information provided may be used by Happy Willow
                    for its operational and marketing purposes.</li>
                <li>To the extent permitted by law, I shall not hold Happy Willow Pte Ltd, its directors,
                    staff and contractors liable for any injuries, damages or death resulting from the
                    use of its services and equipment at Happy Willow. I shall indemnify Happy Willow
                    Pte Ltd, its directors, staff and contractors against any loss of or damage to any
                    property or injury or death to any person as a result of a wilful or negligent act
                    by my child or I during any visit to Happy Willow.</li>
            </ul>
        </div>
        <div style="margin-left: 22px;">
            <div class="element-wrapper">
                <input type="checkbox" id="chkAgreement" name="chkAgreement" data-parsley-required="true" />
                I agree
            </div>
        </div>
        <div class="footer clear-both border-top" style="margin-top: 55px; text-align: right;">
            <button class="button" id="btnSave">
                <i class="fa fa-save"></i>&nbsp;Save</button>
        </div>
    </div>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/JQueryUI/jquery-ui.js") %>"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            app.setting.baseUrl = $('#BaseUrl').val();
        });
    </script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Parsley/js/parsley.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/AccountingJs/accounting.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/js/jquery.tooltipster.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Membership/new-membership.js") %>"></script>

</asp:Content>
