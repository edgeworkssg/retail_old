<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="ItemGroupPriceDiscount" Title=" <%$Resources:dictionary,Item Group Price/Discount %>"
    CodeBehind="ItemGroupPriceDiscount.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Promotion Name %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width: 122px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Campaign Name %>"></asp:Literal>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtCampaignName" runat="server" Width="172px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCampaignName"
                    ErrorMessage="<%$ Resources:dictionary, Please specify campaign name %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Campaign Schedule %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width: 122px">
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
            </td>
            <td style="width: 180px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td style="width: 87px">
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td style="width: 243px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
    </table>
    <table width="600px" id="FieldsTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Item Group %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 37px">
                <asp:RadioButton ID="rbExisting" runat="server" Text="<%$ Resources:dictionary, Use Existing Item Group %>"
                    Checked="True" GroupName="ItemGrp" Width="182px" />
                <subsonic:DropDown ID="ddItemGroup" runat="server" ValueField="ItemGroupID" TextField="ItemGroupName"
                    TableName="ItemGroup" DataValueField="ItemNo" DataTextField="ItemName" Width="255px"
                    OnSelectedIndexChanged="ddItemGroup_SelectedIndexChanged" AutoPostBack="True"
                    ShowPrompt="False">
                </subsonic:DropDown>
                <br />
                <asp:GridView ID="gvViewItemGroup" runat="server" AutoGenerateColumns="False" SkinID="scaffold"
                    Width="300px" OnRowCommand="gvItem_RowCommand" OnRowDeleting="gvItem_RowDeleting"
                    DataSourceID="ObjectDataSource1">
                    <Columns>
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Item Name %>" />
                        <asp:BoundField DataField="UnitQty" HeaderText="<%$Resources:dictionary,Quantity %>" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
                    InsertMethod="Insert" SelectMethod="FetchItemGroupMapByName" TypeName="PowerPOS.ItemGroupController"
                    UpdateMethod="Update">
                    <DeleteParameters>
                        <asp:Parameter Name="ItemGroupId" Type="Object" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="ItemGroupId" Type="Int32" />
                        <asp:Parameter Name="ItemGroupName" Type="String" />
                        <asp:Parameter Name="UniqueItems" Type="Boolean" />
                        <asp:Parameter Name="CreatedBy" Type="String" />
                        <asp:Parameter Name="CreatedOn" Type="DateTime" />
                        <asp:Parameter Name="ModifiedBy" Type="String" />
                        <asp:Parameter Name="ModifiedOn" Type="DateTime" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddItemGroup" Name="ItemGroupId" PropertyName="SelectedValue"
                            Type="Object" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="ItemGroupName" Type="String" />
                        <asp:Parameter Name="UniqueItems" Type="Boolean" />
                        <asp:Parameter Name="CreatedBy" Type="String" />
                        <asp:Parameter Name="CreatedOn" Type="DateTime" />
                        <asp:Parameter Name="ModifiedBy" Type="String" />
                        <asp:Parameter Name="ModifiedOn" Type="DateTime" />
                    </InsertParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:RadioButton ID="rbNew" runat="server" Text="<%$ Resources:dictionary, Create New Group %>"
                    GroupName="ItemGrp" Width="183px" />
                <br />
                <strong>
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Group Name %>"></asp:Literal>
                </strong>
                <asp:TextBox ID="txtGroupName" runat="server" Width="151px"></asp:TextBox><br />
                <br />
                <strong><asp:Literal ID = "Literal12" runat="server" Text="<%$Resources:dictionary,Barcode %>"></asp:Literal> </strong>
                    <asp:TextBox ID="txtBarcode" runat="server" Width="151px"></asp:TextBox><br />
                <br />
                <strong>
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Item:  %>"></asp:Literal>&nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; </strong>&nbsp;<subsonic:DropDown ID="ddsItem" runat="server"
                        ValueField="ItemNo" TextField="ItemName" TableName="Item" DataValueField="ItemNo"
                        DataTextField="ItemName" Width="344px">
                    </subsonic:DropDown>
                <br />
                <strong>
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Qty: %>"></asp:Literal>&nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>
                <asp:TextBox ID="txtUnitQty" runat="server" Width="97px">1</asp:TextBox>
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtUnitQty"
                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535"
                    MinimumValue="0" Type="Integer"></asp:RangeValidator><br />
                &nbsp;<asp:Button ID="btnAddItem" runat="server" Text="<%$ Resources:dictionary, Include Item to Group %>"
                    CausesValidation="False" OnClick="btnAddItem_Click" /><br />
            </td>
            <td colspan="2">
                <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="False" SkinID="scaffold"
                    Width="300px" AutoGenerateDeleteButton="True" OnRowCommand="gvItem_RowCommand"
                    OnRowDeleting="gvItem_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>"
                            SortExpression="ItemNo" />
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Item Name %>" />
                        <asp:BoundField DataField="UnitQty" HeaderText="<%$Resources:dictionary,Quantity %>" />
                        <asp:BoundField DataField="Price" HeaderText="<%$Resources:dictionary,Retail Price %>" />
                    </Columns>
                </asp:GridView>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Price / Discount %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButton ID="rbPrice" runat="server" Text="<%$ Resources:dictionary, Price&nbsp;&nbsp;&nbsp;&nbsp; %>"
                    GroupName="PriceGrp" Width="99px" />
            </td>
            <td style="width: 180px">
                <asp:TextBox ID="txtPromoAmount" runat="server" Width="77px">0</asp:TextBox>
                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtPromoAmount"
                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535"
                    MinimumValue="0" Type="Currency"></asp:RangeValidator>
            </td>
            <td style="width: 87px">
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="height: 16px;">
                <asp:RadioButton ID="rbDiscount" runat="server" Text="<%$ Resources:dictionary, Discount %>"
                    Checked="True" GroupName="PriceGrp" Width="99px" />
            </td>
            <td style="width: 180px; height: 16px;">
                <asp:TextBox ID="txtPromoDiscount" runat="server" Width="48px">0</asp:TextBox>
                <strong>%
                    <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtPromoDiscount"
                        ErrorMessage="<%$ Resources:dictionary, 0-100% %>" MaximumValue="100" MinimumValue="0"
                        Type="Double"></asp:RangeValidator></strong>
            </td>
            <td style="height: 16px; width: 87px;">
            </td>
            <td style="height: 16px">
            </td>
        </tr>
        <tr>
            <td class="wl_pageheaderSub" colspan="4">
                <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Membership %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 37px">
                &nbsp;<asp:RadioButton ID="rbMember" runat="server" Checked="True" Text="<%$ Resources:dictionary, Promo are only for  members %>"
                    Width="206px" GroupName="membership" />
                <asp:RadioButton ID="rbBoth" runat="server" Text="<%$ Resources:dictionary, Promo for both member and non member %>"
                    GroupName="membership" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                &nbsp;<asp:Button ID="btnCreate" runat="server" Text="<%$ Resources:dictionary, Create %>"
                    OnClick="btnCreate_Click" />
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Current Promotions List %>"></asp:Literal>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" DataKeyNames="PromoCampaignHdrID"
        DataSourceID="odsView" SkinID="scaffold">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField Visible="False" DataField="PromoCampaignHdrID" HeaderText="<%$Resources:dictionary,ID %>" />
            <asp:BoundField DataField="DateFrom" HeaderText="<%$Resources:dictionary,Date From %>" />
            <asp:BoundField DataField="DateTo" HeaderText="<%$Resources:dictionary,Date To %>" />
            <asp:BoundField DataField="PromoCampaignName" HeaderText="<%$Resources:dictionary,Campaign Name %>" />
            <asp:BoundField DataField="ItemGroupName" HeaderText="<%$Resources:dictionary,Item Group %>" />
            <asp:BoundField DataField="PromoPrice" HeaderText="<%$Resources:dictionary,Promo Price %>" />
            <asp:BoundField DataField="PromoDiscount" HeaderText="<%$Resources:dictionary,Promo Discount(%) %>" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsView" runat="server" DeleteMethod="DeletePromotionCampaign"
        SelectMethod="ViewItemGroupPriceDiscount" TypeName="PowerPOS.PromotionAdminController"
        OnSelecting="odsView_Selecting">
        <DeleteParameters>
            <asp:Parameter Name="PromoCampaignHdrID" Type="Object" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="ViewStartDate" DefaultValue="DateTime.min" Name="startDate"
                PropertyName="Value" Type="DateTime" />
            <asp:ControlParameter ControlID="ViewEndDate" Name="endDate" PropertyName="Value"
                Type="DateTime" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:HiddenField ID="ViewStartDate" runat="server" />
    <asp:HiddenField ID="ViewEndDate" runat="server" />
</asp:Content>
