<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="MultitierDiscount" Title=" <%$Resources:dictionary,Multi Tier Discount %>" Codebehind="MultitierDiscount.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
<table width=600>
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Promotion Name %>"></asp:Literal></td></tr>
        <tr><td  style="width: 112px"><asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Campaign Name %>"></asp:Literal></td>
            <td colspan=3><asp:TextBox ID="txtCampaignName" runat="server" Width="172px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCampaignName"
                    ErrorMessage="<%$ Resources:dictionary, Please specify campaign name %>"></asp:RequiredFieldValidator></td>
        </tr>
         <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,Campaign Schedule %>"></asp:Literal></td></tr>
    <tr>
        <td  style="width: 112px">
            <asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal></td>
        <td style="width: 183px">
            <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
        </td>
        <td  style="width: 83px">
            <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal></td>
        <td>
            <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
    </tr> 
    <tr><td colspan=4 class="wl_pageheaderSub">
        <asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Minimum Quantity %>"></asp:Literal></td></tr>
    <tr>
        <td  style="width: 112px" >
            <asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Item %>"></asp:Literal></td>
        <td colspan=3 >
            <subsonic:DropDown ID="ddsItemNo" runat="server" DataTextField="ItemName" DataValueField="Remarks"
                TableName="Item" TextField="ItemName" ValueField="ItemNo" Width="369px">
            </subsonic:DropDown></td>
        
    </tr>
    <tr>
        <td  style="width: 112px">
            <asp:Literal ID = "Literal7" runat="server" Text="<%$Resources:dictionary,Minimum Qty %>"></asp:Literal></td>
        <td style="width: 183px">
            <asp:TextBox ID="txtMinQty" runat="server" Width="66px">0</asp:TextBox>
            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtMinQty"
                ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535" MinimumValue="0" Type="Integer"></asp:RangeValidator></td>
        <td  style="width: 83px">
        </td>
        <td>
        </td>
    </tr>
            <tr><td colspan=4 class="wl_pageheaderSub">
                <asp:Literal ID = "Literal8" runat="server" Text="<%$Resources:dictionary,Discount Tiers %>"></asp:Literal></td></tr>
            <tr><td colspan=4 >
                <table>
                <tr><td>
                <br />
                <strong><asp:Literal ID = "Literal9" runat="server" Text="<%$Resources:dictionary,Threshold Qty %>"></asp:Literal> &nbsp;&nbsp; </strong>
                    <asp:TextBox ID="txtThreshold" runat="server" Width="50px" ValidationGroup="ar">0</asp:TextBox>
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtThreshold"
                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535" MinimumValue="0" Type="Integer"
                    ValidationGroup="ar"></asp:RangeValidator><br />
                <strong><asp:Literal ID = "Literal10" runat="server" Text="<%$Resources:dictionary,Discount Tier: %>"></asp:Literal>&nbsp; &nbsp;</strong><asp:TextBox ID="txtDiscount" runat="server" Width="51px" ValidationGroup="ar">0</asp:TextBox>
                <strong>%
                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtDiscount"
                        ErrorMessage="<%$ Resources:dictionary, 0-100% %>" MaximumValue="100" MinimumValue="0" Type="Double" ValidationGroup="ar"></asp:RangeValidator></strong><br />
                <asp:Button ID="btnAddItem" runat="server" Text="<%$ Resources:dictionary, Add Discount %>" OnClick="btnAddItem_Click" ValidationGroup="ar"  />
                </td><td>
                <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="False" SkinID="scaffold" AutoGenerateDeleteButton="True" OnRowCommand="gvItem_RowCommand" OnRowDeleting="gvItem_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="TresholdQty" HeaderText="<%$Resources:dictionary,TresholdQty %>" SortExpression="TresholdQty" />
                        <asp:BoundField DataField="Discount" HeaderText="<%$Resources:dictionary,Discount %>" SortExpression="Discount" />
                    </Columns>
                </asp:GridView><br />
                </td>
                </tr> 
                </table> 
                </td></tr>                    
                
    <tr>
        <td  colspan="4" style="height: 15px">
            &nbsp;</td>
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
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click"  /></td></tr>
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal12" runat="server" Text="<%$Resources:dictionary,Current Promotions List %>"></asp:Literal></td></tr>
    </table>
    <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" DataKeyNames="PromoCampaignHdrID" DataSourceID="odsView" SkinID="scaffold" Width="597px">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField Visible=False DataField="PromoCampaignHdrID" HeaderText="<%$Resources:dictionary,ID %>" />
            <asp:BoundField DataField="DateFrom" HeaderText="<%$Resources:dictionary,Date From %>" />
            <asp:BoundField DataField="DateTo" HeaderText="<%$Resources:dictionary,Date To %>" />
            <asp:BoundField DataField="PromoCampaignName" HeaderText="<%$Resources:dictionary,Campaign Name %>" />
            <asp:BoundField DataField="MinQuantity" HeaderText="<%$Resources:dictionary,Minimum Quantity %>" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsView" runat="server" DeleteMethod="DeletePromotionCampaign" SelectMethod="ViewMultiTierDiscount" TypeName="PowerPOS.PromotionAdminController">
        <DeleteParameters>
            <asp:Parameter Name="PromoCampaignHdrID" Type="Object" />
        </DeleteParameters>
       <SelectParameters>
            <asp:ControlParameter ControlID="ViewStartDate" DefaultValue="DateTime.min" Name="startDate"
                PropertyName="Value" Type="DateTime" />
            <asp:ControlParameter ControlID="ViewEndDate" Name="endDate" PropertyName="Value"
                Type="DateTime" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource><asp:HiddenField ID="ViewStartDate" runat="server" /><asp:HiddenField ID="ViewEndDate" runat="server" />
</asp:Content>

