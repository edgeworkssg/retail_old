<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="ViewActivePromo" Title="<%$Resources:dictionary,View Active Promo%>"
    CodeBehind="ViewActivePromo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
    var newwindow;
    function poptastic(url)
    {
	    newwindow=window.open(url,'name','height=500,width=450');
	    if (window.focus) {newwindow.focus()}
    }
    </script>

    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FieldsTable">
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
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Campaign Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtCampaignName" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Campaign Type %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddCampaignType" runat="server" Width="159px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px">
                &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td colspan="2" align="right" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="800px" runat="server" PageSize="20" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        DataKeyNames="PromoCampaignHdrID" SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound"
        OnRowCancelingEdit="gvReport_RowCancelingEdit" OnRowEditing="gvReport_RowEditing"
        OnRowUpdating="gvReport_RowUpdating">
        <Columns>
            <asp:BoundField />
            <asp:TemplateField>
                <ItemTemplate>
                    <a id="HyperLink1" href="javascript:poptastic('../Promo/PromoDetail.aspx?id=<%# Eval("PromoCampaignHdrID")%>');">
                        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Edit %>"></asp:Literal></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ReadOnly="true" DataField="PromoCampaignHdrID" HeaderText="<%$Resources:dictionary, PromoCampaignHdrID%>"
                Visible="false" SortExpression="PromoCampaignHdrID" />
            <asp:BoundField ReadOnly="true" DataField="PromoCampaignName" HeaderText="<%$Resources:dictionary, Campaign Name%>"
                SortExpression="PromoCampaignName" />
            <asp:BoundField ReadOnly="true" DataField="CampaignType" HeaderText="<%$Resources:dictionary, Campaign Type%>"
                SortExpression="CampaignType" />
            <asp:BoundField DataField="DateFrom" HeaderText="<%$Resources:dictionary, Start Date%>" SortExpression="DateFrom" />
            <asp:BoundField DataField="DateTo" HeaderText="<%$Resources:dictionary, End Date%>" SortExpression="DateTo" />
            <asp:BoundField DataField="ForNonMembersAlso" HeaderText="<%$Resources:dictionary, For Members Only?%>" SortExpression="ForNonMembersAlso" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                    ID="lblPageCount" runat="server"></asp:Label>
                <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                    CommandArgument="Next" CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                    CommandArgument="Last" CommandName="Page" />
            </div>
        </PagerTemplate>
    </asp:GridView>
</asp:Content>
