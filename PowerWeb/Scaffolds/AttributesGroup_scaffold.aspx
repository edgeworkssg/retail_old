<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="AttributesGroup_scaffold.aspx.cs" Inherits="PowerWeb.Scaffolds.AttributesGroup_scaffold" Title="Attributes Group" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:Panel id="pnlGrid" runat="server">
	<a href="AttributesGroup_scaffold.aspx?id=0">Add New...</a>
	<br /><br />
    <asp:GridView 
    ID="GridView1"
    runat="server" 
    AllowPaging="True" 
    AllowSorting="True"
    AutoGenerateColumns="False" 
    OnDataBound="GridView1_DataBound" 
    OnSorting="GridView1_Sorting"
	OnPageIndexChanging="GridView1_PageIndexChanging"
    DataKeyNames="AttributesGroupCode" 
    SkinID=scaffold
    PageSize="50" Width="100%"
    >
        <Columns>
	            <asp:HyperLinkField ItemStyle-Width="30px" Text="Edit" DataNavigateUrlFields="AttributesGroupCode" DataNavigateUrlFormatString="AttributesGroup_scaffold.aspx?id={0}" />
	            
	            <asp:HyperLinkField ItemStyle-Width="30px" Text="Detail" DataNavigateUrlFields="AttributesGroupCode" DataNavigateUrlFormatString="Attributes_scaffold.aspx?groupid={0}" />
	            
	            <asp:BoundField DataField="AttributesGroupName" HeaderText="Attributes Group Name" SortExpression="AttributesGroupName"></asp:BoundField>
	            
	            <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="PrintOnBill" HeaderText="Print On Bill" SortExpression="PrintOnBill"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="UserField1" HeaderText="User Field1" SortExpression="UserField1"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="UserField2" HeaderText="User Field2" SortExpression="UserField2"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="UserField3" HeaderText="User Field3" SortExpression="UserField3"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="UserField4" HeaderText="User Field4" SortExpression="UserField4"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="UserField5" HeaderText="User Field5" SortExpression="UserField5"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="CreatedOn" HeaderText="Created On" SortExpression="CreatedOn"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="ModifiedOn" HeaderText="Modified On" SortExpression="ModifiedOn"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="ModifiedBy" HeaderText="Modified By" SortExpression="ModifiedBy"></asp:BoundField>
	            
	            <asp:BoundField Visible="false" DataField="Deleted" HeaderText="Deleted" SortExpression="Deleted"></asp:BoundField>
	            
        </Columns>
        <EmptyDataTemplate>
            No AttributesGroup 
        </EmptyDataTemplate>
        <PagerTemplate>
        
            <div style="border-top:1px solid #666666">
            <br />
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<< First" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="< Previous" CommandArgument="Prev" CommandName="Page"/>
                Page
                <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> of <asp:Label ID="lblPageCount" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="Next >" CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="Last >>" CommandArgument="Last" CommandName="Page"/>
        </PagerTemplate>
    </asp:GridView>	
	</asp:Panel>
	
	<asp:panel id="pnlEdit" Runat="server">
	<asp:Label ID="lblResult" runat="server"></asp:Label>	
	
	<table class="scaffoldEditTable" cellpadding="5" cellspacing="0" Width="600px">
			<tr>
			<td class="scaffoldEditItemCaption">Attributes Group Code</td>
			<td class="scaffoldEditItem"><asp:Label id="lblID" runat="server" /></td>
		</tr>
	
		<tr>
			<td class="scaffoldEditItemCaption">Attributes Group Name</td>
			
		<td class="scaffoldEditItem"><asp:TextBox ID="ctrlAttributesGroupName" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">Print On Bill</td>
			
		<td class="scaffoldEditItem"><asp:CheckBox ID="ctrlPrintOnBill" runat="server" Checked="False" ></asp:CheckBox></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">User Field1</td>
			
		<td class="scaffoldEditItem"><asp:TextBox ID="ctrlUserField1" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">User Field2</td>
			
		<td class="scaffoldEditItem"><asp:TextBox ID="ctrlUserField2" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">User Field3</td>
			
		<td class="scaffoldEditItem"><asp:TextBox ID="ctrlUserField3" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">User Field4</td>
			
		<td class="scaffoldEditItem"><asp:TextBox ID="ctrlUserField4" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">User Field5</td>
			
		<td class="scaffoldEditItem"><asp:TextBox ID="ctrlUserField5" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">Created On</td>
			
		<td class="scaffoldEditItem"><asp:Label ID="ctrlCreatedOn" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">Created By</td>
			
		<td class="scaffoldEditItem"><asp:Label ID="ctrlCreatedBy" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">Modified On</td>
			
		<td class="scaffoldEditItem"><asp:Label ID="ctrlModifiedOn" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">Modified By</td>
			
		<td class="scaffoldEditItem"><asp:Label ID="ctrlModifiedBy" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td class="scaffoldEditItemCaption">Deleted</td>
			
		<td class="scaffoldEditItem"><asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" ></asp:CheckBox></td>
		</tr>
		
		<tr>
			<td colspan="2" align="left">
			<asp:Button id="btnSave" CssClass="scaffoldButton" runat="server" Text="Save" OnClick="btnSave_Click"></asp:Button>&nbsp;
			<input type="button" onclick="location.href='AttributesGroup_scaffold.aspx'" class="scaffoldButton" value="Return" />
			<asp:Button id="btnDelete" CssClass="scaffoldButton" runat="server" CausesValidation="False" Text="Delete" OnClick="btnDelete_Click"></asp:Button></td>
		</tr>
	</table>
	</asp:panel>
</asp:Content>
