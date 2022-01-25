
<%@ Page Language="C#" Title="EZlinkMessage Scaffold" Inherits="PowerPOS.EZlinkMessageScaffold" MasterPageFile="~/PowerPOSMSt.master" Theme="default" Codebehind="EZlinkMessageScaffold.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
	
	<h2>EZlinkMessage</h2>
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
    DataKeyNames="MsgName" 
    PageSize="50"
    >
        <Columns>
	            <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="MsgName" DataNavigateUrlFormatString="EZlinkMessageScaffold.aspx?id={0}
" />
	            
	            <asp:BoundField DataField="MsgContent" HeaderText="Msg Content" SortExpression="MsgContent"></asp:BoundField>
	            
	            <asp:BoundField DataField="Deleted" HeaderText="Deleted" SortExpression="Deleted"></asp:BoundField>
	            
	            <asp:BoundField DataField="CreatedOn" HeaderText="Created On" SortExpression="CreatedOn"></asp:BoundField>
	            
	            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"></asp:BoundField>
	            
	            <asp:BoundField DataField="ModifiedOn" HeaderText="Modified On" SortExpression="ModifiedOn"></asp:BoundField>
	            
	            <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" SortExpression="ModifiedBy"></asp:BoundField>
	            
	            <asp:BoundField DataField="UniqueID" HeaderText="UniqueID" SortExpression="UniqueID"></asp:BoundField>
	            
        </Columns>
        <EmptyDataTemplate>
            No EZlinkMessage 
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
	<a href="EZlinkMessageScaffold.aspx?id=0">Add New...</a>
	</asp:Panel>
	
	<asp:panel id="pnlEdit" Runat="server">
	<asp:Label ID="lblResult" runat="server"></asp:Label>	
	
	<table cellpadding="5" cellspacing="0" Width="600px">
			<tr>
			<td >Msg Name</td>
			<td ><asp:Label id="lblID" runat="server" /></td>
		</tr>
	
		<tr>
			<td >Msg Content</td>
			
		<td ><asp:TextBox ID="ctrlMsgContent" runat="server" TextMode="MultiLine" Height="100px" Width="500px" MaxLength="2147483647" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td >Deleted</td>
			
		<td ><asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" ></asp:CheckBox></td>
		</tr>
		
		<tr>
			<td >Created On</td>
			
		<td ><asp:Label ID="ctrlCreatedOn" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td >Created By</td>
			
		<td ><asp:Label ID="ctrlCreatedBy" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td >Modified On</td>
			
		<td ><asp:Label ID="ctrlModifiedOn" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td >Modified By</td>
			
		<td ><asp:Label ID="ctrlModifiedBy" runat="server" ></asp:Label></td>
		</tr>
		
		<tr>
			<td >UniqueID</td>
			
		<td ><asp:TextBox ID="ctrlUniqueID" runat="server" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td colspan="2" align="left">
			<asp:Button id="btnSave" CssClass="scaffoldButton" runat="server" Text="Save" OnClick="btnSave_Click"></asp:Button>&nbsp;
			<input type="button" onclick="location.href='EZlinkMessageScaffold.aspx'" class="scaffoldButton" value="Return" />
			<asp:Button id="btnDelete" CssClass="scaffoldButton" runat="server" CausesValidation="False" Text="Delete" OnClick="btnDelete_Click"></asp:Button></td>
		</tr>
	</table>
	</asp:panel>
</asp:Content>
