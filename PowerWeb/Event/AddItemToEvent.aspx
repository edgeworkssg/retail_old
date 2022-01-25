<%@ Page Language="C#" Theme="Default"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="AddItemToEvent" Title="<%$Resources:dictionary,Special Price Item %>" Codebehind="AddItemToEvent.aspx.cs" %>
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
            <asp:Literal id="Literal2" runat="server" Text="<%$Resources:dictionary,Select Event %>">
            </asp:Literal>
        </TD></TR>
        <TR><TD style="WIDTH: 104px" >
            <asp:Literal id="Literal3" runat="server" Text="<%$Resources:dictionary,Event %>"></asp:Literal></TD>
            <TD>
                &nbsp;<asp:DropDownList ID="ddEvent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddEvent_SelectedIndexChanged"
                    Width="452px">
                </asp:DropDownList></TD>            
        </TR>
        <TR><TD class="wl_pageheaderSub" colSpan=2>
            <asp:Literal id="AddItemtobeStockInLbl" runat="server" Text="<%$Resources:dictionary,Add Item %>">
            </asp:Literal>
        </TD></TR>
    <tr>
        <td  style="width: 104px">
            <asp:Literal id="ItemNameLbl" runat="server" 
                Text="<%$Resources:dictionary,Item Name %>"></asp:Literal></td>
        <td>
            <subsonic:DropDown ID="ddlName" runat="server" CausesValidation="True" 
                ShowPrompt="True" TableName="Item" TextField="ItemName" 
                ValidationGroup="AddItem" ValueField="ItemNo" Width="457px">
            </subsonic:DropDown>
        </td>
    </tr>
        <tr>
            <td  style="width: 104px">
                <asp:Literal ID="Literal1" runat="server" 
                    Text="<%$Resources:dictionary,Price %>"></asp:Literal>
            </td>
            <td>
                &nbsp;<asp:TextBox ID="txtPrice" runat="server" CausesValidation="True" 
                    ValidationGroup="AddItem" Width="162px"></asp:TextBox>
                &nbsp;<asp:RangeValidator ID="RangeValidator2" runat="server" 
                    ControlToValidate="txtPrice" 
                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535" 
                    MinimumValue="0" Type="Double"></asp:RangeValidator>
            </td>
        </tr>
    <TR>
        <TD  style="width: 104px">
        </TD>
        <td>
            <asp:Button ID="btnOk" runat="server" onclick="btnOk_Click" 
                Text="<%$ Resources:dictionary, Ok %>" ValidationGroup="AddItem" Width="41px" />
        </td>
    </TR>
    <TR>
        <TD class="wl_pageheaderSub" colSpan=2>
            <asp:Literal ID="InventoryDetailsLbl" runat="server" 
                Text="<%$Resources:dictionary,Inventory Details %>"></asp:Literal>
            </TD></TR>
        <tr>
            <td class="" colspan="2">
                <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="EventItemMapID" OnRowCancelingEdit="gvDetails_RowCancelingEdit" 
                    OnRowDataBound="gvDetails_RowDataBound" OnRowDeleting="gvDetails_RowDeleting" 
                    OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating" 
                    SkinID="scaffold" Width="100%">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                            ValidationGroup="EditLine"></asp:CommandField>
                        <asp:TemplateField HeaderText="<%$ Resources:dictionary,Item No %>">
                            <EditItemTemplate>
                                <asp:Label ID="lblItemNo" runat="server" Text='<%# Bind("ItemNo") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("ItemNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:dictionary,Item Name %>">
                            <EditItemTemplate>
                                <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>'>
                                </asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:dictionary,Price %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPrice" runat="server" 
                                    onkeydown="return DisableEnterPostBackOnTextBox(event)" 
                                    Text='<%# Bind("ItemPrice") %>' Width="86px"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator2" runat="server" 
                                    ControlToValidate="txtPrice" CssClass="LabelMessage" 
                                    ErrorMessage="*Invalid entry" MaximumValue="65000" MinimumValue="0" 
                                    Type="Double" ValidationGroup="EditLine"></asp:RangeValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("ItemPrice") %>'></asp:Label>
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

