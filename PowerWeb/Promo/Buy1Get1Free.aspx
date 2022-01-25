<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="Buy1Get1Free" Title=" <%$Resources:dictionary,Buy 1 Get 1 Free %>" Codebehind="Buy1Get1Free.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
<table width=600>
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Promotion Name %>"></asp:Literal></td></tr>
        <tr><td ><asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Campaign Name %>"></asp:Literal></td>
            <td colspan=3><asp:TextBox ID="txtCampaignName" runat="server" Width="172px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCampaignName"
                    ErrorMessage="<%$ Resources:dictionary, Please specify campaign name %>"></asp:RequiredFieldValidator></td>
        </tr>
         <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,Campaign Schedule %>"></asp:Literal></td></tr>
    <tr>
        <td >
            <asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal></td>
        <td>
            <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
        <td >
            <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal></td>
        <td>
            <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
    </tr>     
            <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Purchased Item %>"></asp:Literal></td></tr>
            <tr><td >
                <asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Purchased Item %>"></asp:Literal></td>
                <td  colspan=3>
                    <subsonic:DropDown ID="ddsItem" runat="server" ValueField="ItemNo" TextField="ItemName" TableName="Item" DataValueField="ItemNo" DataTextField="ItemName" Width="394px">
                    </subsonic:DropDown></td>
            </tr>
    <tr>
        <td >
            <asp:Literal ID = "Literal7" runat="server" Text="<%$Resources:dictionary,Unit Quantity %>"></asp:Literal></td>
        <td colspan=3>
                    <asp:TextBox ID="txtUnitQty" runat="server" Width="54px">1</asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtUnitQty"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535" MinimumValue="0" Type="Integer"></asp:RangeValidator></td>
    </tr>
    <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal8" runat="server" Text="<%$Resources:dictionary,Free Item %>"></asp:Literal></td></tr>
    <tr>
        <td  >
            <asp:Literal ID = "Literal9" runat="server" Text="<%$Resources:dictionary,Free Item %>"></asp:Literal></td>
        <td colspan=3>
            <subsonic:DropDown ID="ddsFreeItem" runat="server" ValueField="ItemNo" TextField="ItemName" TableName="Item" DataValueField="ItemNo" DataTextField="ItemName" Width="397px">
            </subsonic:DropDown></td>
    </tr>
    <tr>
        <td >
            <asp:Literal ID = "Literal10" runat="server" Text="<%$Resources:dictionary,Free Quantity %>"></asp:Literal></td>
        <td colspan=3>
            <asp:TextBox ID="txtFreeQty" runat="server" Width="55px">1</asp:TextBox>
            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtFreeQty"
                ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535" MinimumValue="0" Type="Integer"></asp:RangeValidator></td>
    </tr>
    <tr>
        <td class="wl_pageheaderSub" colspan="4">
            <asp:Literal ID = "Literal11" runat="server" Text="<%$Resources:dictionary,Membership %>"></asp:Literal></td>
    </tr>
    <tr>
        <td colspan="4">
            &nbsp;<asp:RadioButton ID="rbMember" runat="server" Checked="True" Text="<%$ Resources:dictionary, Promo are only for  members %>"
                Width="206px" GroupName="membership" />
            <asp:RadioButton ID="rbBoth" runat="server" Text="<%$ Resources:dictionary, Promo for both member and non member %>" GroupName="membership" /></td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label></td>
    </tr>
        <tr><td colspan=4 >
            &nbsp;<asp:Button ID="btnCreate" runat="server" Text="<%$ Resources:dictionary, Create %>" OnClick="btnCreate_Click"  />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"  /></td></tr>
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal12" runat="server" Text="<%$Resources:dictionary,Current Promotions List %>"></asp:Literal></td></tr>
    </table>
    <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" DataKeyNames="PromoCampaignHdrID" DataSourceID="odsView" SkinID="scaffold">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField Visible=False DataField="PromoCampaignHdrID" HeaderText="<%$Resources:dictionary,ID %>" />
            <asp:BoundField DataField="DateFrom" HeaderText="<%$Resources:dictionary,Date From %>" />
            <asp:BoundField DataField="DateTo" HeaderText="<%$Resources:dictionary,Date To %>" />
            <asp:BoundField DataField="PromoCampaignName" HeaderText="<%$Resources:dictionary,Cmpgn Name %>" />
            <asp:BoundField DataField="PurchasedItemName" HeaderText="<%$Resources:dictionary,Purchased Item %>" />
            <asp:BoundField DataField="UnitQty" HeaderText="<%$Resources:dictionary,Unit Qty %>" />
            <asp:BoundField DataField="FreeItemName" HeaderText="<%$Resources:dictionary,Free Item %>" />
            <asp:BoundField DataField="FreeQty" HeaderText="<%$Resources:dictionary,Free Qty %>" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsView" runat="server" DeleteMethod="DeletePromotionCampaign" SelectMethod="ViewBuyXGetYFree" TypeName="PowerPOS.PromotionAdminController">
        <DeleteParameters>
            <asp:Parameter Name="PromoCampaignHdrID" Type="Object" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="ViewStartDate" Name="startDate" PropertyName="Value"
                Type="DateTime" />
            <asp:ControlParameter ControlID="ViewEndDate" Name="endDate" PropertyName="Value"
                Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:HiddenField ID="ViewStartDate" runat="server" /><asp:HiddenField ID="ViewEndDate" runat="server" />
</asp:Content>

