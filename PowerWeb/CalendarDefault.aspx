<%@ Page Language="C#" AutoEventWireup="true" Inherits="CalendarDefault" Codebehind="CalendarDefault.aspx.cs" %>
<%@ Register Src="~/CalendarControls/GoogleCalendar.ascx"  TagName="GCalendar" TagPrefix="mc"%>
<%@ Register Src="~/CalendarControls/OutlookCalendar.ascx"  TagName="OCalendar" TagPrefix="mc"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Course Management System</title>
</head>

<body>
	<script type="text/javascript">
		var hostPath = '<%=GetAppPath()%>';
    </script>
    <form id="form1" runat="server">
    <ajax:ScriptManager runat="server" ID="sm" EnablePageMethods="true" >
		<Scripts>
			<ajax:ScriptReference Path="~/Scripts/GoogleEditor.js" />
		</Scripts>
    </ajax:ScriptManager>
    <div>
		<table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
			<tr>
				<td>
				
				</td>
				<td align="right">
					<b>Calendar Skin Type:</b>
					<asp:DropDownList runat="server" ID="ddSkinType" AutoPostBack="true">
						<asp:ListItem Text="Google" Value="Google"></asp:ListItem>
						<asp:ListItem Text="Outlook" Value="Outlook"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td></td>
				<td align="right">
					<b>Admin Tasks</b>
					<asp:DropDownList runat="server" ID="drpAdminTasks" AutoPostBack="true" 
                        onselectedindexchanged="drpAdminTasks_SelectedIndexChanged">
                        <asp:ListItem Value="  ">
                        </asp:ListItem>
						<asp:ListItem Text="Create Room" Value="Room"></asp:ListItem>
                        <asp:ListItem Text="Create Building" Value="Building"></asp:ListItem>
						<asp:ListItem Text="Mark Attendance" Value="Attendance"></asp:ListItem>
					    
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td style="padding-top:10px;" colspan="2">
					<mc:GCalendar runat="server" ID="gCalendar"   /> 
					<mc:OCalendar runat="server" ID="oCalendar" Visible="false" />
				</td>
			</tr>
		</table>
    </div>
    </form>
</body>
</html>
