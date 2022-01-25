
<%@ Page Language="C#" Title="<%$Resources:dictionary,Department Scaffold %>" Inherits="Department_scaffold" MasterPageFile="~/PowerPOSMSt.master" Theme="default" Codebehind="Department_scaffold.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
	
	<h2><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal></h2>
	<asp:Panel id="pnlGrid" runat="server">
    <asp:GridView 
    ID="GridView1"
    runat="server" 
    AllowPaging="True" 
    AllowSorting="True"
    AutoGenerateColumns="False" 
    OnDataBound="GridView1_DataBound" 
    OnSorting="GridView1_Sorting"
	OnPageIndexChanging="GridView1_PageIndexChanging"
    DataKeyNames="DepartmentID" 
    PageSize="50"
    >
        <Columns>
	            <asp:HyperLinkField Text="<%$Resources:dictionary,Edit %>" DataNavigateUrlFields="DepartmentID" DataNavigateUrlFormatString="Department_scaffold.aspx?id={0}
" />
	            
	            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:dictionary,Department Name %>" SortExpression="DepartmentName"></asp:BoundField>
	            
	            <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary,Remark %>" SortExpression="Remark"></asp:BoundField>
	            
	            <asp:BoundField DataField="CreatedBy" HeaderText="<%$Resources:dictionary,Created By %>" SortExpression="CreatedBy"></asp:BoundField>
	            
	            <asp:BoundField DataField="CreatedOn" HeaderText="<%$Resources:dictionary,Created On %>" SortExpression="CreatedOn"></asp:BoundField>
	            
	            <asp:BoundField DataField="ModifiedBy" HeaderText="<%$Resources:dictionary,Modified By %>" SortExpression="ModifiedBy"></asp:BoundField>
	            
	            <asp:BoundField DataField="ModifiedOn" HeaderText="<%$Resources:dictionary,Modified On %>" SortExpression="ModifiedOn"></asp:BoundField>
	            
	            <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary,Deleted %>" SortExpression="Deleted"></asp:BoundField>
	            
        </Columns>
        <EmptyDataTemplate>
           <asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,No Department  %>"></asp:Literal> 
        </EmptyDataTemplate>
       <PagerTemplate>
                        <div style="border-top:1px solid #666666">            
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
    </asp:GridView>
	<a href="Department_scaffold.aspx?id=0"><asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Add New... %>"></asp:Literal></a>
	</asp:Panel>
	
	<asp:panel id="pnlEdit" Runat="server">
	<asp:Label ID="lblResult" runat="server"></asp:Label>	
	
	<table cellpadding="5" cellspacing="0" Width="600px">
			<tr>
			<td ><asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal></td>
			<td ><asp:Label id="lblID" runat="server" /></td>
		</tr>
	
		<tr>
			<td ><asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,Department Name %>"></asp:Literal></td>
			
		<td ><asp:TextBox ID="ctrlDepartmentName" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td ><asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Remark %>"></asp:Literal></td>
			
		<td ><asp:TextBox ID="ctrlRemark" runat="server" MaxLength="250" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td ><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Created By %>"></asp:Literal></td>
			
		<td ><asp:Label ID="ctrlCreatedBy" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td ><asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Created On %>"></asp:Literal></td>
			
		<td ><asp:Label ID="ctrlCreatedOn" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td ><asp:Literal ID = "Literal7" runat="server" Text="<%$Resources:dictionary,Modified By %>"></asp:Literal></td>
			
		<td ><asp:Label ID="ctrlModifiedBy" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td ><asp:Literal ID = "Literal8" runat="server" Text="<%$Resources:dictionary,Modified On %>"></asp:Literal></td>
			
		<td ><asp:Label ID="ctrlModifiedOn" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td ><asp:Literal ID = "Literal9" runat="server" Text="<%$Resources:dictionary,Deleted %>"></asp:Literal></td>
			
		<td ><asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" ></asp:CheckBox></td>
		</tr>
		
		<tr>
			<td colspan="2" align="left">
			<asp:Button id="btnSave" CssClass="scaffoldButton" runat="server" Text="<%$ Resources:dictionary, Save %>" OnClick="btnSave_Click"></asp:Button>&nbsp;
			<input type="button" onclick="location.href='Department_scaffold.aspx'" class="scaffoldButton" value="Return" />
			<asp:Button id="btnDelete" CssClass="scaffoldButton" runat="server" CausesValidation="False" Text="<%$ Resources:dictionary, Delete %>" OnClick="btnDelete_Click"></asp:Button></td>
		</tr>
	</table>
	</asp:panel>
</asp:Content>
