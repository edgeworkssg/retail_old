<%@ Page Title="Inventory Data Recording" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="InventoryDataRecording.aspx.cs" Inherits="PowerWeb.Reports.InventoryDataRecording" %>

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
    <table width="600px" id="FilterTable">
        <tr>
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
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Checked="True" Text="Use Start Date" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="Use End Date" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal6" runat="server" Text="Inventory Location"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddInventoryLocation" runat="server" CausesValidation="True"
                    Width="182px">
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <%-- <tr><td colspan=2 style="height: 15px" >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
        <td colspan=2 align="right" valign="bottom" class="ExportButton">
            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
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
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton></td><td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton></td></tr><%--                     <tr>
                <td colspan="4" style="height:25px;"> 
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label></td></tr>--%> </table><asp:GridView ID="gvReport" Width="100%" runat="server" AllowPaging="True" PageSize="20"
        AllowSorting="True" OnDataBound="gvReport_DataBound" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" 
                DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" /><asp:BoundField DataField="Val1" />
            <asp:BoundField DataField="Val2" />
            <asp:BoundField DataField="Val3" />
            <asp:BoundField DataField="Val4" />
            <asp:BoundField DataField="Val5" />
            <asp:BoundField DataField="Val6" />
            <asp:BoundField DataField="Val7" />
            <asp:BoundField DataField="Val8" />
            <asp:BoundField DataField="Val9" />
            <asp:BoundField DataField="Val10" />
            <asp:BoundField DataField="InventoryLocationName" />
            <asp:BoundField DataField="CreatedBy" HeaderText="Recorded By" />
        </Columns>
    </asp:GridView>
</asp:Content>
