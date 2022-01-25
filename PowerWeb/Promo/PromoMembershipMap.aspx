<%@ Page Language="C#" Theme="Default"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="PromotionMembershipMap" Title="<%$Resources:dictionary,Promotion Membership Map %>" Codebehind="PromoMembershipMap.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <ajax:UpdateProgress runat="server" id="progress1" AssociatedUpdatePanelID="abc" DisplayAfter=1 >        
        <progresstemplate>
            <IMG src="../App_Themes/Default/image/indicator_mozilla_blu.gif" /> <SPAN style="COLOR: #0000ff"><B>Update in progress...</B><BR /></SPAN>
        </progresstemplate>
    </ajax:UpdateProgress>         
    <ajax:UpdatePanel runat="server" ID="abc">
    <ContentTemplate>
    <asp:Label id="lblErrorMsg" runat="server" Text="" CssClass="LabelMessage"></asp:Label> 
    <TABLE width=600><TBODY>
        <TR><TD class="wl_pageheaderSub" colSpan=2>
            <asp:Literal id="Literal2" runat="server" Text="<%$Resources:dictionary,Select Promotion %>">
            </asp:Literal>
        </TD></TR>
        <TR><TD style="WIDTH: 104px" >
            <asp:Literal id="Literal3" runat="server" Text="<%$Resources:dictionary,Promo %>"></asp:Literal></TD>
            <TD>
                <asp:DropDownList ID="ddPromo" runat="server" Height="22px" AutoPostBack=true  
                    onselectedindexchanged="ddlPromo_SelectedIndexChanged" Width="456px">
                </asp:DropDownList>
            </TD>            
        </TR>
        <TR><TD class="wl_pageheaderSub" colSpan=2>
            <asp:Literal id="AddItemtobeStockInLbl" runat="server" Text="<%$Resources:dictionary,Add Membership Group %>">
            </asp:Literal>
        </TD></TR>
    <tr>
        <td  style="width: 104px">
            <asp:Literal id="ItemNameLbl" runat="server" 
                Text="<%$Resources:dictionary,Membership Group %>"></asp:Literal></td>
        <td>
            <subsonic:DropDown ID="ddlMembershipGroup" runat="server" CausesValidation="True" 
                ShowPrompt="True" TableName="MembershipGroup" TextField="GroupName" 
                ValidationGroup="AddItem" ValueField="MembershipGroupID" Width="457px" 
                WhereField="Deleted" WhereValue="false">
            </subsonic:DropDown>
        </td>
    </tr>
    <TR>
        <TD  style="width: 104px">
            <asp:RadioButton ID="rbUsePrice" runat="server" GroupName="PriceDisc" 
                Checked="True" />
            <asp:Literal ID="ItemNameLbl0" runat="server" 
                Text="<%$Resources:dictionary,Promo Price %>"></asp:Literal>
        </TD>
        <td>
            <asp:TextBox ID="txtMembershipPrice" runat="server" Width="253px"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                ControlToValidate="txtMembershipPrice" CssClass="LabelMessage" 
                ErrorMessage="*Invalid entry" MaximumValue="65535" MinimumValue="0" 
                ToolTip="must be a number" Type="Currency" ValidationGroup="EditLine"></asp:RangeValidator>
        </td>
    </TR>
    <TR>
        <TD  style="width: 104px">
            <asp:RadioButton ID="rbUseDisc" runat="server" GroupName="PriceDisc" />
            <asp:Literal ID="Literal1" runat="server" 
                Text="<%$Resources:dictionary,Promo Disc(%) %>"></asp:Literal>
        </TD>
        <td>
            <asp:TextBox ID="txtPromoDiscount" runat="server" Width="253px"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator3" runat="server" 
                ControlToValidate="txtMembershipPrice" CssClass="LabelMessage" 
                ErrorMessage="*Invalid entry" MaximumValue="100" MinimumValue="0" 
                ToolTip="must be a percentage" Type="Currency" ValidationGroup="EditLine"></asp:RangeValidator>
        </td>
    </TR>
    <TR>
        <TD  style="width: 104px">
            </TD>
        <td>
            <asp:Button ID="btnOk" runat="server" onclick="btnOk_Click" 
                Text="<%$ Resources:dictionary, Ok %>" ValidationGroup="AddItem" Width="41px" />
        </td>
        </TR>
        <tr>
            <td class="wl_pageheaderSub" colspan="2">
                <asp:Literal ID="InventoryDetailsLbl" runat="server" 
                    Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="" colspan="2">
                <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="PromoMembershipID" 
                    OnRowCancelingEdit="gvDetails_RowCancelingEdit"                      
                    OnRowDataBound="gvDetails_RowDataBound" OnRowDeleting="gvDetails_RowDeleting" 
                    OnRowUpdating="gvDetails_RowUpdating" SkinID="scaffold" Width="100%" 
                    onrowediting="gvDetails_RowEditing">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ValidationGroup="EditLine">
                        </asp:CommandField>                        
                        <asp:BoundField DataField="GroupName" ReadOnly="true" HeaderText="Group Name" 
                            SortExpression="GroupName" />
                        <asp:BoundField DataField="UseMembershipPrice" ReadOnly="true" HeaderText="Use Price/Discount" 
                           SortExpression="UseMembershipPrice" />
                            <asp:TemplateField HeaderText="<%$ Resources:dictionary,Price %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPrice" runat="server" 
                                    onkeydown="return DisableEnterPostBackOnTextBox(event)" 
                                    Text='<%# Bind("MembershipPrice") %>' Width="86px"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator2" runat="server" 
                                    ControlToValidate="txtPrice" CssClass="LabelMessage" 
                                    ErrorMessage="*Invalid entry" MaximumValue="65000" MinimumValue="0" 
                                    Type="Double" ValidationGroup="EditLine"></asp:RangeValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("MembershipPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                                                    
                            <asp:TemplateField HeaderText="<%$ Resources:dictionary,Disc(%) %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDisc" runat="server" 
                                    onkeydown="return DisableEnterPostBackOnTextBox(event)" 
                                    Text='<%# Bind("MembershipDiscount") %>' Width="86px"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator3" runat="server" 
                                    ControlToValidate="txtDisc" CssClass="LabelMessage" 
                                    ErrorMessage="*Invalid entry" MaximumValue="65000" MinimumValue="0" 
                                    Type="Double" ValidationGroup="EditLine"></asp:RangeValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDisc" runat="server" Text='<%# Bind("MembershipDiscount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                            
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        </TBODY>
            </TABLE>
        &nbsp;
</ContentTemplate> 
        </ajax:UpdatePanel>        
</asp:Content>

