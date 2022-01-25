<%@ Page Language="C#" AutoEventWireup="true" Inherits="MembershipDetail" Codebehind="MembershipDetail.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:Literal ID = "OrderDetailTitle" runat="server" Text="<%$Resources:dictionary,Member Detail %>"></asp:Literal></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
  <table cellpadding="5" cellspacing="0" Width="600px">
			<tr>
			<td  style="width: 104px">
               <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal> 
            </td>
			<td  style="width: 180px"><asp:TextBox ReadOnly=True ID="ctrlMembershipNo" runat="server" MaxLength="50" OnTextChanged="ctrlMembershipNo_TextChanged" ></asp:TextBox></td>
			<td  style="width: 92px"> <asp:Literal ID = "Literal28" runat="server" Text="<%$Resources:dictionary,Group %>"></asp:Literal></td>
			<td >
                &nbsp;<asp:TextBox ID="ctrlGroupName" runat="server" MaxLength="50" OnTextChanged="ctrlMembershipNo_TextChanged"
                    ReadOnly="True"></asp:TextBox></td>
		</tr>
        <tr>
            <td  style="width: 104px">
                <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Expiry Date %>"></asp:Literal></td>
            <td  style="width: 180px">
            <asp:TextBox ID="ctrlExpiryDate" runat="server" Width="121px" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
            <td  style="width: 92px">
               <asp:Literal ID = "Literal29" runat="server" Text="<%$Resources:dictionary,Remark %>"></asp:Literal> </td>
            <td >
            <asp:TextBox ID="ctrlRemarks" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox></td>
        </tr>
        <tr>
            <td class="wl_pageheaderSub" style="text-align: center" colspan="4">
               <asp:Literal ID = "Literal30" runat="server" Text="<%$Resources:dictionary,Personal Information %>"></asp:Literal></td>
        </tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal7" runat="server" Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal></td>
			
		<td  style="width: 180px">
            <asp:TextBox ID="ctrlNameToAppear" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ctrlLastName"
                ErrorMessage="*"></asp:RequiredFieldValidator></td>
		<td  style="width: 92px"><asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,NRIC/FIN %>"></asp:Literal> </td>
			<td >
                <asp:TextBox ID="ctrlNRIC" runat="server" MaxLength="50" ReadOnly="True"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ctrlNRIC"
                    ErrorMessage="*"></asp:RequiredFieldValidator></td>
		</tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal></td>
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlFirstName" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="ctrlFirstName"
                ErrorMessage="*"></asp:RequiredFieldValidator></td>
		<td  style="width: 92px"><asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal></td>
			<td ><asp:TextBox ID="ctrlLastName" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ctrlLastName"
                    ErrorMessage="*"></asp:RequiredFieldValidator></td>
		</tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal31" runat="server" 
                    Text="<%$ Resources:dictionary,Chinese Name %>"></asp:Literal></td>
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlFirstName0" 
                runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox>
            </td>
		<td  style="width: 92px">
                <asp:Literal ID = "Literal32" runat="server" 
                    Text="<%$Resources:dictionary,Christian Name %>"></asp:Literal></td>
			<td >
                <asp:TextBox ID="ctrlLastName0" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox>
                </td>
		</tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal9" runat="server" Text="<%$Resources:dictionary,Gender %>"></asp:Literal></td>
			
		<td  style="width: 180px">
            <asp:TextBox ID="ctrlGender" runat="server" ReadOnly="True" Width="147px"></asp:TextBox></td>
		<td  style="width: 92px"><asp:Literal ID = "Literal10" runat="server" Text="<%$Resources:dictionary,Date Of Birth  %>"></asp:Literal></td>
			<td >
            <asp:TextBox ID="ctrlDateOfBirth" runat="server" Width="118px" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
		</tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal11" runat="server" Text="<%$Resources:dictionary,Occupation %>"></asp:Literal></td>
			
		<td  style="width: 180px">
                <asp:TextBox ID="ctrlOccupation" runat="server" MaxLength="50" ReadOnly="True"></asp:TextBox></td>
		<td  style="width: 92px"></td>
			<td >
                &nbsp;</td>
		</tr>
        <tr>
            <td class="wl_pageheaderSub" colspan="4" style="text-align: center">
                <asp:Literal ID = "Literal12" runat="server" Text="<%$Resources:dictionary,Address Information %>"></asp:Literal></td>
        </tr>
        <tr>
            <td  style="width: 104px; height: 11px;">
                <asp:Literal ID = "Literal13" runat="server" Text="<%$Resources:dictionary,Address %>"></asp:Literal></td>
            <td  style="height: 11px;" colspan="3">
                <asp:TextBox ID="ctrlStreetName" runat="server" MaxLength="200"
                    Width="431px" ReadOnly="True" style="margin-bottom: 0px"></asp:TextBox>
                <br />
                <br />
                <asp:TextBox ID="ctrlStreetName2" runat="server" MaxLength="200"
                    Width="431px" ReadOnly="True"></asp:TextBox></td>
        </tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal15" runat="server" Text="<%$Resources:dictionary,Zip Code %>"></asp:Literal></td>
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlZipCode" runat="server" MaxLength="50" Width="150px" ReadOnly="True" ></asp:TextBox></td>
		<td  style="width: 92px"><asp:Literal ID = "Literal16" runat="server" Text="<%$Resources:dictionary,City %>"></asp:Literal></td>
			<td ><asp:TextBox ID="ctrlCity" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal14" runat="server" Text="<%$Resources:dictionary,Country %>"></asp:Literal>
            </td>
			
		<td  style="width: 180px">
                <asp:TextBox ID="ctrlCountry" runat="server" MaxLength="50" Width="150px" ReadOnly="True"></asp:TextBox></td>
		<td  style="width: 92px">&nbsp;</td>
			<td >&nbsp;</td>
		</tr>
        <tr>
            <td class="wl_pageheaderSub" style="text-align: center" colspan="4">
                <asp:Literal ID = "Literal17" runat="server" Text="<%$Resources:dictionary,Contact Information %>"></asp:Literal></td>
        </tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal18" runat="server" Text="<%$Resources:dictionary,Mobile %>"></asp:Literal></td>
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlMobile" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox></td>
		<td  style="width: 92px">
            <asp:Literal ID = "Literal19" runat="server" Text="<%$Resources:dictionary,Fax %>"></asp:Literal>&nbsp;</td>
			<td ><asp:TextBox ID="ctrlOffice" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox></td>
		</tr>
		
		<tr>
			<td  style="width: 104px"><asp:Literal ID = "Literal20" runat="server" Text="<%$Resources:dictionary,Home %>"></asp:Literal></td>			
			
		<td  style="width: 180px"><asp:TextBox ID="ctrlHome" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox></td>
		<td  style="width: 92px">
            <asp:Literal ID = "Literal21" runat="server" Text="<%$Resources:dictionary,e-mail %>"></asp:Literal></td>
			<td ><asp:TextBox ID="ctrlEmail" runat="server" MaxLength="50" ReadOnly="True" ></asp:TextBox></td>
		</tr>
		 <tr>
            <td class="wl_pageheaderSub" style="text-align: center" colspan="4">
                <asp:Literal ID = "Literal22" runat="server" 
                    Text="<%$ Resources:dictionary,Billy House Related Information %>" 
                    Visible="False"></asp:Literal></td>
        </tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal23" runat="server" 
                    Text="<%$ Resources:dictionary,Vita Mix Customer %>" Visible="False"></asp:Literal></td>
			
		<td  style="width: 180px">
            <asp:CheckBox ID="cbIsVitaMix" runat="server" />
            </td>
		<td  style="width: 92px">
            <asp:Literal ID = "Literal24" runat="server" 
                Text="<%$ Resources:dictionary,Water Filter Customer %>" Visible="False"></asp:Literal>&nbsp;</td>
			<td >
                <asp:CheckBox ID="cbIsWaterFilter" runat="server" Visible="False" />
            </td>
		</tr>
		
		<tr>
			<td  style="width: 104px">
                <asp:Literal ID = "Literal33" runat="server" 
                    Text="<%$ Resources:dictionary,Young Customer %>" Visible="False"></asp:Literal></td>
			
		<td  style="width: 180px">
            <asp:CheckBox ID="cbIsYoung" runat="server" Visible="False" />
            </td>
		<td  style="width: 92px">
                <asp:Literal ID = "Literal34" runat="server" 
                Text="<%$ Resources:dictionary,Juice Plus Customer %>" Visible="False"></asp:Literal></td>
			<td >
                <asp:CheckBox ID="cbIsJuicePlus" runat="server" Visible="False" />
            </td>
		</tr>
		<tr>
            <td class="wl_pageheaderSub" colspan="4" style="text-align: center">
                <asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,Remark %>"></asp:Literal></td>
        </tr>		
		<tr>
			<td  style="width: 104px; height: 2px;">Remarks</td>			
		    <td  colspan=3><asp:TextBox ID="TextBox1" runat="server" MaxLength="50" TextMode=MultiLine  ></asp:TextBox>
		    </td>		    
		</tr>
		<tr>
			<td colspan="4" align="left" style="height: 5px">
                &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                <asp:Label id="lblID" runat="server" Visible="False" /></td>
		</tr>
	</table>
    </div>
    </form>
</body>
</html>
