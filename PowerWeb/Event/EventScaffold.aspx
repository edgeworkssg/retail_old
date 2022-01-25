
<%@ Page Language="C#" Title="Special Event Setup" Inherits="PowerPOS.SpecialEventScaffold" MasterPageFile="~/PowerPOSMSt.master" Theme="default" Codebehind="EventScaffold.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
	
	<h2>SpecialEvent</h2>
	<asp:Panel id="pnlGrid" runat="server">	
	<input type="button" onclick="location.href='EventScaffold.aspx?id=0'" class="scaffoldButton" value="Add New" />
    <asp:GridView 
    ID="GridView1"
    runat="server" 
    AllowPaging="True" 
    AllowSorting="True"
    AutoGenerateColumns="False" 
    OnDataBound="GridView1_DataBound" 
    OnSorting="GridView1_Sorting"
	OnPageIndexChanging="GridView1_PageIndexChanging"
    DataKeyNames="EventId" 
    PageSize="20"
    SkinID="scaffold"
    >
        <Columns>
	            <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="EventId" DataNavigateUrlFormatString="EventScaffold.aspx?id={0}
" />
	            
	            <asp:BoundField DataField="EventName" HeaderText="Event Name" SortExpression="EventName"></asp:BoundField>
	            
	            <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate"></asp:BoundField>
	            
	            <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate"></asp:BoundField>
	            
	            <asp:BoundField DataField="CreatedOn"  Visible="false" HeaderText="Created On" SortExpression="CreatedOn"></asp:BoundField>
	            
	            <asp:BoundField DataField="CreatedBy" Visible="false"  HeaderText="Created By" SortExpression="CreatedBy"></asp:BoundField>
	            
	            <asp:BoundField DataField="ModifiedOn" Visible="false"  HeaderText="Modified On" SortExpression="ModifiedOn"></asp:BoundField>
	            
	            <asp:BoundField DataField="ModifiedBy" Visible="false"  HeaderText="Modified By" SortExpression="ModifiedBy"></asp:BoundField>
	            
	            <asp:BoundField DataField="Deleted" HeaderText="Deleted" SortExpression="Deleted"></asp:BoundField>
	            
	            <asp:BoundField DataField="UniqueID" HeaderText="UniqueID" SortExpression="UniqueID"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld1" Visible="false"  HeaderText="User Fld1" SortExpression="UserFld1"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld2" Visible="false"  HeaderText="User Fld2" SortExpression="UserFld2"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld3" Visible="false"  HeaderText="User Fld3" SortExpression="UserFld3"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld4"  Visible="false" HeaderText="User Fld4" SortExpression="UserFld4"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld5" Visible="false"  HeaderText="User Fld5" SortExpression="UserFld5"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld6" Visible="false"  HeaderText="User Fld6" SortExpression="UserFld6"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld7" Visible="false"  HeaderText="User Fld7" SortExpression="UserFld7"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld8" Visible="false"  HeaderText="User Fld8" SortExpression="UserFld8"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld9" Visible="false"  HeaderText="User Fld9" SortExpression="UserFld9"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFld10" Visible="false"  HeaderText="User Fld10" SortExpression="UserFld10"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFloat1"  Visible="false" HeaderText="User Float1" SortExpression="UserFloat1"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFloat2" Visible="false"  HeaderText="User Float2" SortExpression="UserFloat2"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFloat3" Visible="false"  HeaderText="User Float3" SortExpression="UserFloat3"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFloat4" Visible="false"  HeaderText="User Float4" SortExpression="UserFloat4"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFloat5" Visible="false"  HeaderText="User Float5" SortExpression="UserFloat5"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFlag1" Visible="false"  HeaderText="User Flag1" SortExpression="UserFlag1"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFlag2" Visible="false"  HeaderText="User Flag2" SortExpression="UserFlag2"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFlag3" Visible="false"  HeaderText="User Flag3" SortExpression="UserFlag3"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFlag4"  Visible="false" HeaderText="User Flag4" SortExpression="UserFlag4"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserFlag5" Visible="false"  HeaderText="User Flag5" SortExpression="UserFlag5"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserInt1" Visible="false"  HeaderText="User Int1" SortExpression="UserInt1"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserInt2" Visible="false"  HeaderText="User Int2" SortExpression="UserInt2"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserInt3" Visible="false"  HeaderText="User Int3" SortExpression="UserInt3"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserInt4" Visible="false"  HeaderText="User Int4" SortExpression="UserInt4"></asp:BoundField>
	            
	            <asp:BoundField DataField="UserInt5" Visible="false"  HeaderText="User Int5" SortExpression="UserInt5"></asp:BoundField>
	            
        </Columns>
        <EmptyDataTemplate>
            No SpecialEvent 
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
	
	<table cellpadding="5" cellspacing="0" Width="600px">
			<tr>
			<td >Event </td>
			<td ><asp:Label id="lblID" runat="server" /></td>
		</tr>
	
		<tr>
			<td >Event Name</td>
			
		<td ><asp:TextBox ID="ctrlEventName" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td >Start Date</td>
			
		<td ><subsonic:CalendarControl ID="ctrlStartDate" runat="server" ></subsonic:CalendarControl></td>
		</tr>
		
		<tr>
			<td >End Date</td>
			
		<td ><subsonic:CalendarControl ID="ctrlEndDate" runat="server" ></subsonic:CalendarControl></td>
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
			<input type="button" onclick="location.href='EventScaffold.aspx'" class="scaffoldButton" value="Return" />
			<asp:Button id="btnDelete" CssClass="scaffoldButton" runat="server" CausesValidation="False" Text="Delete" OnClick="btnDelete_Click"></asp:Button></td>
		</tr>
	</table>
	</asp:panel>
</asp:Content>
