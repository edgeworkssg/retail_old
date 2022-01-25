<%@ Page Language="C#" Theme="Default"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="PromotionLocationMap" Title="<%$Resources:dictionary,Promotion Location Map %>" Codebehind="PromoLocationMap.aspx.cs" %>
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
            <asp:Literal id="AddItemtobeStockInLbl" runat="server" Text="<%$Resources:dictionary,Add Location %>">
            </asp:Literal>
        </TD></TR>
    <tr>
        <td  style="width: 104px">
            <asp:Literal id="ItemNameLbl" runat="server" 
                Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal></td>
        <td>
            <subsonic:DropDown ID="ddlName" runat="server" CausesValidation="True" 
                ShowPrompt="True" TableName="PointOfSale" TextField="PointOfSaleName" 
                ValidationGroup="AddItem" ValueField="PointOfSaleID" Width="457px" 
                WhereField="Deleted" WhereValue="false">
            </subsonic:DropDown>
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
                Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
            </TD></TR>
        <tr>
            <td class="" colspan="2">
                <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="PromoLocationMapID" OnRowCancelingEdit="gvDetails_RowCancelingEdit" 
                    OnRowDataBound="gvDetails_RowDataBound" OnRowDeleting="gvDetails_RowDeleting" 
                    OnRowUpdating="gvDetails_RowUpdating" 
                    SkinID="scaffold" Width="100%">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" 
                            ValidationGroup="EditLine"></asp:CommandField>
                        <asp:BoundField DataField="PointOfSaleName" HeaderText="Point Of Sale" />
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

