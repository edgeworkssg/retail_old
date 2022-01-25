<%@ Page Language="C#" AutoEventWireup="true" Inherits="EditPoints" Codebehind="EditPoints.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:Literal ID = "OrderDetailTitle" runat="server" Text="<%$Resources:dictionary,Order Detail %>"></asp:Literal></title>
</head>
<body>
	
    <form id="form1" runat="server">
     <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <div>
  <table cellpadding="5" cellspacing="0" Width="400px">
        <tr>
            <td class="wl_pageheaderSub" style="text-align: center" colspan="2">
               <asp:Literal ID = "Literal30" runat="server" Text="<%$Resources:dictionary,Adjust Points %>"></asp:Literal></td>
        </tr>
			<tr>
			<td  style="width: 104px">
               <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal> 
            </td>
			<td  style="width: 180px">
                <asp:Label ID="lblMembershipNo" runat="server" Text="Label"></asp:Label></td>			
		</tr>
        <tr>
            <td  style="width: 104px">
                <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal></td>
            <td  style="width: 180px">
                <asp:Label ID="lblNameToAppear" runat="server" Text="Label"></asp:Label></td>            
        </tr>
      <tr>
          <td  style="width: 104px">
              <asp:Literal ID="Literal31" runat="server" Text="Point/Package"></asp:Literal></td>
          <td  style="width: 180px">
              <asp:Label ID="lblItemName" runat="server" Text="Label"></asp:Label></td>
      </tr>
      <tr>
          <td  style="width: 104px">
              <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Current Point %>"></asp:Literal></td>
          <td  style="width: 180px">
              <asp:Label ID="lblCurrentPoint" runat="server" Text="Label"></asp:Label></td>
      </tr>
      <tr>
          <td  style="width: 104px">
              <asp:Literal ID="Literal3" runat="server" Text="New Point" /></td>
          <td  style="width: 180px">
              <asp:TextBox ID="txtPoints" runat="server" MaxLength="50"></asp:TextBox>
              <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtPoints"
                  ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535"
                  MinimumValue="0" Type="Currency"></asp:RangeValidator></td>
      </tr>
      <tr>
          <td colspan=2 align="center">
              <asp:Button ID="btnAdjust" runat="server" Text="<%$Resources:dictionary,Adjustment Point %>" Enabled="False" OnClick="btnAdjust_Click" />              
          </td>                  
      </tr>
      <tr>
          <td align="center" colspan="2" style="height: 26px">
              <asp:Label ID="lblError" runat="server"></asp:Label></td>
      </tr>
	</table>
    </div>
    </form>
</body>
</html>
