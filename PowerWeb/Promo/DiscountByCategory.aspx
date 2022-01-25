<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="DiscountByCategory" Title="Discount By Category" Codebehind="DiscountByCategory.aspx.cs" %>
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
                    ErrorMessage="Please specify campaign name"></asp:RequiredFieldValidator></td>
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
            <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Category to Assign Discount to %>"></asp:Literal></td></tr>
            <tr><td >
                <asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Category %>"></asp:Literal></td>
                <td style="width: 183px">
                    <subsonic:DropDown ID="ddsCategory" runat="server" ValueField="Remarks" TextField="CategoryName" TableName="Category" DataValueField="Remarks" DataTextField="CategoryName" Width="167px">
                    </subsonic:DropDown></td>
                <td ></td>
                <td>
                    &nbsp;</td></tr>
    <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal7" runat="server" Text="<%$Resources:dictionary,Discount %>"></asp:Literal></td></tr>
    <tr>
        <td  style="height: 26px">
            <asp:Literal ID = "Literal9" runat="server" Text="<%$Resources:dictionary,Discount %>"></asp:Literal></td>
        <td style="width: 183px; height: 26px;">
            <asp:TextBox ID="txtDiscount" runat="server" Width="39px"></asp:TextBox><strong>%</strong>&nbsp;
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtDiscount"
                ErrorMessage="<%$ Resources:dictionary, 0-100% %>" MaximumValue="100" MinimumValue="0" Type="Double"></asp:RangeValidator></td>
        <td  style="height: 26px">
            </td>
        <td style="height: 26px">
            &nbsp;</td>
    </tr>
    <tr>
        <td >
        </td>
        <td style="width: 183px">
        </td>
        <td >
        </td>
        <td>
        </td>
    </tr>
    <tr>
         <td class="wl_pageheaderSub" colspan="4">
            <asp:Literal ID = "Literal8" runat="server" Text="<%$Resources:dictionary,Membership %>"></asp:Literal></td>
    </tr>
    <tr>
        <td colspan="4">
            &nbsp;<asp:RadioButton ID="rbMember" runat="server" Checked="True" Text="<%$ Resources:dictionary, Promo are only for  members %>"
                Width="206px" GroupName="membership" />
            <asp:RadioButton ID="rbBoth" runat="server" Text="<%$ Resources:dictionary, Promo for both member and non member %>" GroupName="membership" /></td>
    </tr>
    <tr>
        <td colspan="4" style="height: 5px">
            <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label></td>
    </tr>
        <tr><td colspan=4 >
            &nbsp;<asp:Button ID="btnCreate" runat="server" Text="<%$ Resources:dictionary, Create %>" OnClick="btnCreate_Click1"  />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"  /></td></tr>
    </table>
    <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" DataKeyNames="PromoCampaignHdrID" DataSourceID="odsView" SkinID="scaffold">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField DataField="PromoCampaignHdrID" HeaderText="<%$Resources:dictionary,ID %>" />
            <asp:BoundField DataField="DateFrom" HeaderText="<%$Resources:dictionary,Date From %>" />
            <asp:BoundField DataField="DateTo" HeaderText="<%$Resources:dictionary,Date To %>" />
            <asp:BoundField DataField="PromoCampaignName" HeaderText="<%$Resources:dictionary,Campaign Name %>" />
            <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />
            <asp:BoundField DataField="PromoDiscount" HeaderText="<%$Resources:dictionary,Discount %>" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsView" runat="server" DeleteMethod="DeletePromotionCampaign" SelectMethod="ViewDiscountByCategory" TypeName="PowerPOS.PromotionAdminController">
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
    <asp:HiddenField ID="ViewStartDate" runat="server" />
    <asp:HiddenField ID="ViewEndDate" runat="server" />
</asp:Content>