<%@ Page Title="<%$Resources:dictionary,SMF Report %>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="SMFReport.aspx.cs" Inherits="PowerWeb.Reports.SMFReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="700px">
        <tr>
            <td style="width: 105px; vertical-align:top;">
                <asp:CheckBox ID="IsUseStartDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />&nbsp;
                <br />
                <asp:DropDownList ID="ddlStartTimeHour" runat="server" /><asp:DropDownList ID="ddlStartTimeMinute" runat="server" /><asp:DropDownList ID="ddlStartTimeSecond" runat="server" />
            </td>
            <td style="vertical-align:top;">
                <asp:CheckBox ID="IsUseEndDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, End Date %>" />
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                <br />
                <asp:DropDownList ID="ddlEndTimeHour" runat="server" /><asp:DropDownList ID="ddlEndTimeMinute" runat="server" /><asp:DropDownList ID="ddlEndTimeSecond" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Search%>" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><%--   <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
                    Text="Search" />--%> </td><td colspan="2">
                <asp:LinkButton ID="lnkExport" runat="server" class="classBlue" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td></tr></table><table>
        <tr>
            <td colspan="4">
                <asp:GridView ID="gvReport" Width="800px" runat="server" AllowPaging="True" AllowSorting="false"
                    OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting" ShowFooter="true"
                    OnPageIndexChanging="gvReport_PageIndexChanging" SkinID="scaffold" PageSize="20"
                    OnRowDataBound="gvReport_RowDataBound">
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
            </td>
        </tr>
    </table>
</asp:Content>
