<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="CashRecordingReport" Title="<%$Resources:dictionary,Cash Recording Report %>"
    CodeBehind="CashRecordingReport.aspx.cs" %>

<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Format="dd MMM yyyy" PopupButtonID="ImageButton2"
        TargetControlID="txtEndDate" Animated="False">
    </cc1:CalendarExtender>
    <table width="600px" id="FilterTable">
        <tr style="height: 20px;">
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                    runat="server" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Text="<%$Resources:dictionary, Use Start Date%>" Checked="True" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                    runat="server" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Text="<%$Resources:dictionary, Use End Date%>" Checked="True" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Ref No %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtRefNo" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Type %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlType" runat="server" Width="179px">
                    <asp:ListItem Value="" Text=""></asp:ListItem>
                    <asp:ListItem Value="Opening Balance" Text="<%$Resources:dictionary, Opening Balance%>"></asp:ListItem>
                    <asp:ListItem Value="Cash In" Text="<%$Resources:dictionary, Cash In%>"></asp:ListItem>
                    <asp:ListItem Value="Cash Out" Text="<%$Resources:dictionary, Cash Out%>"> </asp:ListItem>
                    <asp:ListItem Value="Closing Balance" Text="<%$Resources:dictionary, Closing Balance%>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Supervisor %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtSupervisor" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Cashier %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtCashier" runat="server" Width="173px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="ddDept" runat="server" OnInit="ddDept_Init" PromptValue=""
                    TableName="Department" TextField="DepartmentName" ValueField="DepartmentID" Width="172px">
                </subsonic:DropDown>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <%-- <tr><td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align=right valign="middle" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
            <td align="right" colspan="2" valign="middle">
            </td>
        </tr>--%>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 600px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 25px;">
                <asp:Literal ID="litMessage" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="100%" runat="server" ShowFooter="true" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" DataKeyNames="CashRecRefNo"
        PageSize="20" AutoGenerateColumns="False" SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField />
            <asp:BoundField DataField="CashRecordingTime" SortExpression="CashRecordingTime"
                HeaderText="<%$Resources:dictionary,Date/Time %>" />
            <asp:BoundField DataField="CashRecRefNo" SortExpression="CashRecRefNo" HeaderText="<%$Resources:dictionary,Ref No %>" />
            <asp:BoundField DataField="CashRecordingTypeName" SortExpression="CashRecordingTypeName"
                HeaderText="<%$Resources:dictionary,Type %>" />
            <asp:BoundField DataField="CashierName" SortExpression="CashierName" HeaderText="<%$Resources:dictionary,Cashier %>" />
            <asp:BoundField DataField="SupervisorName" SortExpression="SupervisorName" HeaderText="<%$Resources:dictionary,Supervisor %>" />
            <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="<%$Resources:dictionary,Amount %>" />
            <asp:BoundField DataField="PointOfSaleName" SortExpression="PointOfSaleName" HeaderText="<%$Resources:dictionary,Point Of Sale %>" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary,Outlet %>" />
            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$Resources:dictionary, Remark%>" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList
                    ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                    ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server"
                        CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next"
                        CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                    CommandArgument="Last" CommandName="Page" />
            </div>
        </PagerTemplate>
    </asp:GridView>
</asp:Content>
