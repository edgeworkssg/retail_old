
<%@ Page Language="C#" Title="EZLinkMsgLog Scaffold" Inherits="PowerPOS.EZLinkMsgLogScaffold" MasterPageFile="~/PowerPOSMSt.master" Theme="default" Codebehind="EZLinkMsgLogScaffold.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
	
	<h2>EZLinkMsgLog</h2>
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
    DataKeyNames="msgLogID" 
    PageSize="50"
    >
        <Columns>
	            <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="msgLogID" DataNavigateUrlFormatString="EZLinkMsgLogScaffold.aspx?id={0}
" />
	            
	            <asp:BoundField DataField="msgDate" HeaderText="Msg Date" SortExpression="msgDate"></asp:BoundField>
	            
	            <asp:BoundField DataField="msgContent" HeaderText="Msg Content" SortExpression="msgContent"></asp:BoundField>
	            
	            <asp:BoundField DataField="Deleted" HeaderText="Deleted" SortExpression="Deleted"></asp:BoundField>
	            
	            <asp:BoundField DataField="uniqueID" HeaderText="UniqueID" SortExpression="uniqueID"></asp:BoundField>
	            
        </Columns>
        <EmptyDataTemplate>
            No EZLinkMsgLog 
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
	<a href="EZLinkMsgLogScaffold.aspx?id=0">Add New...</a>
	</asp:Panel>
	
	<asp:panel id="pnlEdit" Runat="server">
	<asp:Label ID="lblResult" runat="server"></asp:Label>	
	
	<table cellpadding="5" cellspacing="0" Width="600px">
			<tr>
			<td >MsgLog</td>
			<td ><asp:Label id="lblID" runat="server" /></td>
		</tr>
	
		<tr>
			<td >Msg Date</td>
			
		<td ><subsonic:CalendarControl ID="ctrlMsgDate" runat="server" ></subsonic:CalendarControl></td>
		</tr>
		
		<tr>
			<td >Msg Content</td>
			
		<td ><asp:TextBox ID="ctrlMsgContent" runat="server" MaxLength="250" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td >Deleted</td>
			
		<td ><asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" ></asp:CheckBox></td>
		</tr>
		
		<tr>
			<td >UniqueID</td>
			
		<td ><asp:TextBox ID="ctrlUniqueID" runat="server" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td colspan="2" align="left">
			<asp:Button id="btnSave" CssClass="scaffoldButton" runat="server" Text="Save" OnClick="btnSave_Click"></asp:Button>&nbsp;
			<input type="button" onclick="location.href='EZLinkMsgLogScaffold.aspx'" class="scaffoldButton" value="Return" />
			<asp:Button id="btnDelete" CssClass="scaffoldButton" runat="server" CausesValidation="False" Text="Delete" OnClick="btnDelete_Click"></asp:Button></td>
		</tr>
	</table>
	</asp:panel>
</asp:Content>
