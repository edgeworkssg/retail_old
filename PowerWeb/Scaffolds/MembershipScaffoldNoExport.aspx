
<%@ Page Language="C#" Title="<%$Resources:dictionary,Membership Setup %>" Inherits="PowerPOS.MembershipScaffoldNoExport" MasterPageFile="~/PowerPOSMst.master" Theme="default" Codebehind="MembershipScaffoldNoExport.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
	
	<h2><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Membership Setup %>"></asp:Literal></h2>
	<asp:Panel id="pnlGrid" runat="server" Width="800px">
	<div >
        <asp:Label ID="lblResult0" runat="server"></asp:Label>
        <br />
        <input type="button" onclick="location.href='MembershipScaffold.aspx?id=0'" class="scaffoldButton" value="Add New" style="width: 87px" /><asp:Button 
            id="btnExportAll" CssClass="scaffoldButton" runat="server" 
            CausesValidation="False" Text="<%$ Resources:dictionary, Export %>" 
            OnClick="btnExportAll_Click" Width="88px" Visible="False" />
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="MembershipNo" 
            OnDataBound="GridView1_DataBound" 
            OnPageIndexChanging="GridView1_PageIndexChanging" 
            OnRowDataBound="GridView1_RowDataBound" 
            OnSorting="GridView1_Sorting" PageSize="20" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="MembershipNo" DataNavigateUrlFormatString="MembershipScaffold.aspx?id={0}
" Text="<%$Resources:dictionary,Edit %>" />
                <asp:BoundField DataField="MembershipNo" 
                    HeaderText="<%$Resources:dictionary,Membership No %>" 
                    SortExpression="MembershipNo" />
                <asp:BoundField DataField="GroupName" 
                    HeaderText="<%$Resources:dictionary,Card Type %> " SortExpression="GroupName" />
                <asp:BoundField DataField="NameToAppear" 
                    HeaderText="<%$Resources:dictionary,Name %>" SortExpression="NameToAppear" />
                <asp:BoundField DataField="LastName" 
                    HeaderText="<%$Resources:dictionary,Surname %>" SortExpression="LastName" />
                <asp:BoundField DataField="SalesPersonID" HeaderText="Stylist" 
                    SortExpression="SalesPersonID" />
                <asp:BoundField DataField="Gender" 
                    HeaderText="<%$Resources:dictionary,Gender %>" SortExpression="Gender" 
                    Visible="False" />
                <asp:BoundField DataField="DateOfBirth" DataFormatString="{0:dd MMM yyyy}" 
                    HeaderText="<%$Resources:dictionary,DOB %>" SortExpression="DateOfBirth" />
                <asp:BoundField DataField="Email" HeaderText="<%$Resources:dictionary,Email %>" 
                    SortExpression="Email" />
                <asp:BoundField DataField="NRIC" HeaderText="<%$Resources:dictionary,NRIC %>" 
                    SortExpression="NRIC" />
                <asp:BoundField DataField="Occupation" 
                    HeaderText="<%$Resources:dictionary,occupation %>" SortExpression="occupation" 
                    Visible="False" />
                <asp:BoundField DataField="StreetName" 
                    HeaderText="<%$Resources:dictionary,Address 1 %>" SortExpression="StreetName" />
                <asp:BoundField DataField="StreetName2" 
                    HeaderText="<%$Resources:dictionary,Address 2 %>" 
                    SortExpression="StreetName2" />
                <asp:BoundField DataField="ZipCode" 
                    HeaderText="<%$Resources:dictionary,Zip Code %>" SortExpression="ZipCode" />
                <asp:BoundField DataField="City" HeaderText="<%$Resources:dictionary,City %>" 
                    SortExpression="City" />
                <asp:BoundField DataField="Country" 
                    HeaderText="<%$Resources:dictionary,Country %>" SortExpression="Country" />
                <asp:BoundField DataField="Mobile" 
                    HeaderText="<%$Resources:dictionary,Mobile %>" SortExpression="Mobile" />
                <asp:BoundField DataField="Office" HeaderText="<%$Resources:dictionary,Fax %>" 
                    SortExpression="Office" Visible="False" />
                <asp:BoundField DataField="Home" HeaderText="<%$Resources:dictionary,Home %>" 
                    SortExpression="Home" />
                <asp:BoundField DataField="ExpiryDate" DataFormatString="{0:dd MMM yyyy}" 
                    HeaderText="<%$Resources:dictionary,Expiry Date %>" 
                    SortExpression="ExpiryDate" />
                <asp:BoundField DataField="Remarks" 
                    HeaderText="<%$Resources:dictionary,Remarks %>" SortExpression="Remarks" />
                <asp:BoundField DataField="userfld1" HeaderText="Child 1 Name" 
                    SortExpression="userfld1" Visible="False" />
                <asp:BoundField DataField="userfld3" HeaderText="Child 2 Name" 
                    SortExpression="userfld3" Visible="False" />
                <asp:BoundField DataField="userfld5" HeaderText="Child 3 Name" 
                    SortExpression="userfld5" Visible="False" />
                <asp:BoundField DataField="userfld7" HeaderText="Child 4 Name" 
                    SortExpression="userfld7" Visible="False" />
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="SEARCHLbl" runat="server" 
                    Text="<%$Resources:dictionary,No Membership  %>"></asp:Literal>
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top:1px solid #666666">
                    <asp:Button ID="btnFirst" runat="server" CommandArgument="First" 
                        CommandName="Page" CssClass="scaffoldButton" 
                        Text="<%$Resources:dictionary,<< First %>" />
                    <asp:Button ID="btnPrev" runat="server" CommandArgument="Prev" 
                        CommandName="Page" CssClass="scaffoldButton" 
                        Text="<%$Resources:dictionary,< Previous%>" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" 
                        Css 
                        OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal>
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CommandArgument="Next" 
                        CommandName="Page" CssClass="scaffoldButton" 
                        Text="<%$Resources:dictionary,Next > %> " />
                    <asp:Button ID="btnLast" runat="server" CommandArgument="Last" 
                        CommandName="Page" CssClass="scaffoldButton" 
                        Text="<%$Resources:dictionary,Last >> %> " />
                </div>
            </PagerTemplate>
        </asp:GridView>
        </div>        
	</asp:Panel>
	
	<asp:panel id="pnlEdit" Runat="server">
	 <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" 
        MinimumPrefixLength="1"
        CompletionSetCount="1"  
        ServicePath="../Synchronization/Synchronization.asmx" 
        ServiceMethod="GetNameList" 
        TargetControlID="txtChristianName"></cc1:AutoCompleteExtender>
    <cc1:CalendarExtender ID="cldDOB" runat="server" Animated="False" Format="dd MMM yyyy" PopupButtonID="ImageButton1" TargetControlID="ctrlDateOfBirth">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldExpiryDate" runat="server" Animated="False" Format="dd MMM yyyy" PopupButtonID="ImageButton2" TargetControlID="ctrlExpiryDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldSubscriptionDate" runat="server" Animated="False" Format="dd MMM yyyy" PopupButtonID="ImageButton3" TargetControlID="txtSubscriptionDate">
    </cc1:CalendarExtender>        
	<asp:Label ID="lblResult" runat="server"></asp:Label>&nbsp;
	
	<table cellpadding="5" cellspacing="0" Width="600px">
			<tr>
			<td  style="width: 104px">
               <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal> 
            </td>
			<td  style="width: 180px">
                <asp:TextBox ID="ctrlMembershipNo" runat="server" MaxLength="50" ></asp:TextBox></td>
			<td  style="width: 92px"> <asp:Literal ID = "Literal28" runat="server" Text="<%$Resources:dictionary,Group %>"></asp:Literal></td>
			<td ><asp:DropDownList ID="ctrlMembershipGroupId" runat="server" Width="153px" ></asp:DropDownList></td>
		</tr>
        <tr>
            <td  style="width: 104px">
                <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Expiry Date %>"></asp:Literal></td>
            <td  style="width: 180px">
            <asp:TextBox ID="ctrlExpiryDate" runat="server" Width="121px"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
            <td  style="width: 92px">
                <asp:Literal ID="Literal8" runat="server" 
                    Text="<%$Resources:dictionary,Subscription Date %>"></asp:Literal>
               </td>
            <td >
                <asp:TextBox ID="txtSubscriptionDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td class="wl_pageheaderSub" style="text-align: center" colspan="4">
               <asp:Literal ID = "Literal30" runat="server" Text="<%$Resources:dictionary,Personal Information %>"></asp:Literal></td>
        </tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID="Literal7" runat="server" 
                    Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal>
            </td>
			
		<td  style="width: 180px">
            <asp:TextBox ID="ctrlNameToAppear" runat="server" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="ctrlNameToAppear" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
		<td  style="width: 92px"><asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,NRIC/FIN %>"></asp:Literal> </td>
			<td >
                <asp:TextBox ID="ctrlNRIC" runat="server" MaxLength="50"></asp:TextBox>
                </td>
		</tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal></td>
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlFirstName" runat="server" MaxLength="50" ></asp:TextBox>
            </td>
		<td  style="width: 92px"><asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal></td>
			<td ><asp:TextBox ID="ctrlLastName" runat="server" MaxLength="50" ></asp:TextBox>
                </td>
		</tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID="Literal35" runat="server" 
                    Text="<%$Resources:dictionary,Chinese Name %>"></asp:Literal>
            </td>
			
		<td  style="width: 180px">
            <asp:TextBox ID="txtChineseName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
		<td  style="width: 92px">
                <asp:Literal ID="Literal36" runat="server" 
                    Text="<%$Resources:dictionary,Christian Name %>"></asp:Literal>
            </td>
			<td >
                <asp:TextBox ID="txtChristianName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
		</tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal9" runat="server" Text="<%$Resources:dictionary,Gender %>"></asp:Literal></td>
			
		<td  style="width: 180px">
            <asp:DropDownList ID="ctrlGender" runat="server">
                <asp:ListItem>M</asp:ListItem>
                <asp:ListItem>F</asp:ListItem>
            </asp:DropDownList></td>
		<td  style="width: 92px"><asp:Literal ID = "Literal10" runat="server" Text="<%$Resources:dictionary,Date Of Birth  %>"></asp:Literal></td>
			<td >
            <asp:TextBox ID="ctrlDateOfBirth" runat="server" Width="118px"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" 
                    ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" 
                    style="height: 16px; width: 16px" /></td>
		</tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal11" runat="server" Text="<%$Resources:dictionary,Occupation %>"></asp:Literal></td>
			
		<td  style="width: 180px">
                <asp:TextBox ID="ctrlOccupation" runat="server" MaxLength="50"></asp:TextBox></td>
		<td  style="width: 92px">Stylist</td>
			<td >
                <asp:DropDownList ID="ctrlStylist" runat="server" Width="153px">
                </asp:DropDownList>
            </td>
		</tr>
        <tr>
            <td class="wl_pageheaderSub" colspan="4" style="text-align: center">
                <asp:Literal ID = "Literal" runat="server" Text="<%$Resources:dictionary,Address Information %>"></asp:Literal></td>
        </tr>
        <tr>
            <td  style="width: 104px; height: 11px;">
                <asp:Literal ID = "Literal13" runat="server" Text="<%$Resources:dictionary,Address %>"></asp:Literal></td>
            <td  style="height: 11px;" colspan="3">
                <asp:TextBox ID="ctrlStreetName" runat="server" MaxLength="200"
                    Width="431px" style="margin-bottom: 0px"></asp:TextBox>
                <br />
                <br />
                <asp:TextBox ID="ctrlStreetName2" runat="server" MaxLength="200"
                    Width="431px"></asp:TextBox></td>
        </tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal15" runat="server" Text="<%$Resources:dictionary,Zip Code %>"></asp:Literal></td>
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlZipCode" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
		<td  style="width: 92px"><asp:Literal ID = "Literal16" runat="server" Text="<%$Resources:dictionary,City %>"></asp:Literal></td>
			<td ><asp:TextBox ID="ctrlCity" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal14" runat="server" Text="<%$Resources:dictionary,Country %>"></asp:Literal>
            </td>
			
		<td  style="width: 180px">
                <asp:TextBox ID="ctrlCountry" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
		<td  style="width: 92px">&nbsp;</td>
			<td >&nbsp;</td>
		</tr>
        <tr>
            <td class="wl_pageheaderSub" style="text-align: center" colspan="4">
                <asp:Literal ID = "Literal17" runat="server" Text="<%$Resources:dictionary,Contact Information %>"></asp:Literal></td>
        </tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal18" runat="server" Text="<%$Resources:dictionary,Mobile %>"></asp:Literal></td>
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlMobile" runat="server" MaxLength="50"  ></asp:TextBox></td>
		<td  style="width: 92px">
            <asp:Literal ID = "Literal19" runat="server" Text="<%$Resources:dictionary,Fax %>"></asp:Literal>
            &nbsp;</td>
			<td ><asp:TextBox ID="ctrlOffice" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal20" runat="server" Text="<%$Resources:dictionary,Home %>"></asp:Literal></td>			
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlHome" runat="server" MaxLength="50" ></asp:TextBox></td>
		<td  style="width: 92px">
            <asp:Literal ID = "Literal21" runat="server" Text="<%$Resources:dictionary,e-mail %>"></asp:Literal></td>
			<td ><asp:TextBox ID="ctrlEmail" runat="server" MaxLength="50" ></asp:TextBox></td>
		</tr>
		 <tr>
            <td class="wl_pageheaderSub" style="text-align: center; height: 23px;" colspan="4">
                <asp:Literal ID = "Literal37" runat="server" 
                    Text="<%$ Resources:dictionary,Additional Information %>" 
                    Visible="True"></asp:Literal></td>
        </tr>
		
		    <tr>
                <td colspan="4" style="text-align: left">
                    <asp:PlaceHolder ID="AdditionalInfoHolder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
		
		<tr visible="false">
			<td visible="false" class="wl_pageheaderSub" style="text-align: center;" colspan="4">
                <asp:Literal ID = "Literal12" runat="server" 
                    Text="<%$ Resources:dictionary,Billy House Related Information %>" 
                    Visible="False"></asp:Literal></td>
			
		</tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal23" runat="server" 
                    Text="<%$ Resources:dictionary,Vita Mix Customer %>" Visible="False"></asp:Literal></td>
			
		<td  style="width: 180px">
            <asp:CheckBox ID="cbIsVitaMix" runat="server" Visible="False" />
            </td>
		<td  style="width: 92px">
                <asp:Literal ID = "Literal24" runat="server" 
                Text="<%$ Resources:dictionary,Water Filter Customer %>" Visible="False"></asp:Literal>
                &nbsp;</td>
			<td >
                <asp:CheckBox ID="cbIsWaterFilter" runat="server" Visible="False" />
            </td>
		</tr>
		<tr>
            <td  style="width: 104px;">
                <asp:Literal ID = "Literal33" runat="server" 
                    Text="<%$ Resources:dictionary,Young Customer %>" Visible="False"></asp:Literal></td>
            <td  style="width: 180px">
                <asp:CheckBox ID="cbIsYoung" runat="server" Visible="False" />
            </td>
            <td  style="width: 92px">
                <asp:Literal ID="Literal34" runat="server" 
                    Text="<%$ Resources:dictionary,Juice Plus Customer %>" Visible="False"></asp:Literal>
            </td>
            <td >
                <asp:CheckBox ID="cbIsJuicePlus" runat="server" Visible="False" />
            </td>
        </tr>		
		<tr>
			<td class="wl_pageheaderSub" style="text-align: center;" colspan="4">
                <asp:Literal ID="Literal22" runat="server" 
                    Text="<%$Resources:dictionary,Remark %>"></asp:Literal>
            </td>			
		</tr>
		<tr>
			<td  style="width: 104px; height: 2px;">
                Remarks</td>
		    <td  colspan="3">
                <asp:TextBox ID="ctrlRemarks" runat="server" Height="118px" MaxLength="50" 
                    TextMode="MultiLine" Width="427px"></asp:TextBox>
            </td>
		</tr>		           		
	        <tr>
                <td align="left" colspan="4">
                    &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" CssClass="scaffoldButton" 
                        OnClick="btnSave_Click" Text="<%$ Resources:dictionary, Save %>" />
                    &nbsp;
                    <input class="scaffoldButton" onclick="location.href='MembershipScaffold.aspx'" 
                        type="button" value="Return" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                        CssClass="scaffoldButton" OnClick="btnDelete_Click" 
                        Text="<%$ Resources:dictionary, Delete %>" />
                    <asp:Button ID="btnExportDetails" runat="server" CausesValidation="False" 
                        CssClass="scaffoldButton" Text="<%$ Resources:dictionary, Export %>" 
                        Visible="False" />
                    <asp:Label ID="lblID" runat="server" Visible="False" />
                </td>
            </tr>
	</table>
	</asp:panel>
</asp:Content>