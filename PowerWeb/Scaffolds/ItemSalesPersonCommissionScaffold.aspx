
<%@ Page Language="C#" Title="Item Sales Person Commmission Setup" Inherits="ItemSalesPersonCommissionScaffold" MasterPageFile="~/PowerPOSMSt.master" Theme="default" Codebehind="ItemSalesPersonCommissionScaffold.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
	
    <asp:Panel id="pnlGrid" runat="server">
	<input class="classname" onclick="location.href='ItemSalesPersonCommissionScaffold.aspx?id=0'" type="button"
    value="Add New"/>
    <div style="height:7px;"></div>
    <asp:GridView 
    ID="GridView1"
    runat="server" 
    AllowPaging="True" 
    AllowSorting="True"
    AutoGenerateColumns="False" 
    OnDataBound="GridView1_DataBound" 
    OnSorting="GridView1_Sorting"
	OnPageIndexChanging="GridView1_PageIndexChanging"
    DataKeyNames="ID" 
    PageSize="50"
     SkinID="scaffold" 
    >
        <Columns>
	            <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="ItemSalesPersonCommissionScaffold.aspx?id={0}" />
	            
	            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
	            
	            <asp:BoundField DataField="ItemNo" HeaderText="Item No" SortExpression="ItemNo"></asp:BoundField>
	            
	            <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" 
                    SortExpression="SalesPerson"></asp:BoundField>
	         
	            <asp:BoundField DataField="Commission" HeaderText="Commission" 
                    SortExpression="Commission"></asp:BoundField>
	            
	            <asp:BoundField DataField="Remarks" HeaderText="Remarks" 
                    SortExpression="Remarks"></asp:BoundField>
	            
	            <asp:BoundField DataField="Deleted" HeaderText="Deleted" SortExpression="Deleted"></asp:BoundField>	            	        
	            
        </Columns>
        <EmptyDataTemplate>
            No Item Sales Person Commission
        </EmptyDataTemplate>
        <PagerTemplate>        
            <div style="border-top:1px solid #666666">
            <br />
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<< First" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="< Previous" CommandArgument="Prev" CommandName="Page"/>
                Page
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> of <asp:Label ID="lblPageCount" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="Next >" CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="Last >>" CommandArgument="Last" CommandName="Page"/>
        </PagerTemplate>
    </asp:GridView>	
	</asp:Panel>	
	<asp:panel id="pnlEdit" Runat="server">
	<asp:Label ID="lblResult" runat="server"></asp:Label>	
	
	    <ajax:ScriptManager ID="ScriptManager1" runat="server">
        </ajax:ScriptManager>
        <asp:HiddenField ID="hdfID" runat="server" Value="0" />
        <table ID="FieldsTable" cellpadding="5" cellspacing="0" Width="600px">
            <tr>
                <td>
                    Search Item</td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox> &nbsp;
                    <asp:Button ID="Button1" runat="server" Text="Search" onclick="Button1_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    Item</td>
                <td>
                    <asp:Label ID="lblSelectedItem" runat="server" Text=""></asp:Label>&nbsp;
                    </td>
            </tr>
            <tr>
                <td>
                    Sales Person</td>
                <td>
                    <asp:DropDownList ID="ddlSalesPerson" runat="server" Height="22px" 
                        Width="194px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Commission</td>
                <td>
                    <asp:TextBox ID="txtCommission" runat="server" MaxLength="250"></asp:TextBox>
                   <%-- <asp:RangeValidator ID="RangeValidator1" runat="server" 
                        ErrorMessage="Value must be between 0-100" ControlToValidate="txtCommission" MinimumValue="1" MaximumValue="100"></asp:RangeValidator>--%>
                </td>
            </tr>
            <tr>
                <td>
                    Created By</td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Created On</td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Modified By</td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Modified On</td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" 
                        OnClick="btnSave_Click" Text="Save" />
                    &nbsp;
                    <input class="classname" onclick="location.href='ItemSalesPersonCommissionScaffold.aspx'" 
                        type="button" value="Return" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                        CssClass="classname" OnClick="btnDelete_Click" Text="Delete" />
                </td>
            </tr>
        </table>
	</asp:panel>
</asp:Content>
