<%@ Page Language="C#" AutoEventWireup="true" Inherits="EditorPages_OutlookEventEdit" Codebehind="OutlookEventEdit.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Event</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:600px; font-family:Tahoma; font-size:8pt;">
		<div style="background-color:#DCEBFE; height:1px; line-height:1px; font-size:1px; margin-right:2px; margin-left:2px;"></div>
		<div style="background-color:#DCEBFE; height:1px; line-height:1px; font-size:1px; margin-right:1px; margin-left:1px;"></div>
		<div style="background-image:url(../Images/o_ch0bg.gif); background-repeat:repeat-x; height:18px; font-family:Tahoma; font-size:12px; padding:3px; " >
			<asp:LinkButton runat="server" ID="lbSave" ForeColor="Black" Font-Underline="false">
				<img style="border:0; vertical-align:middle;" src="../Images/o_save.gif" />
				Save and close
			</asp:LinkButton>
			&nbsp;
			<asp:LinkButton runat="server" ID="lbDelete" ForeColor="Black" Font-Underline="false">
				<img style="border:0; vertical-align:middle;" src="../Images/o_delete.gif" />
				Delete
			</asp:LinkButton>
			&nbsp;
			<img style="border:0; vertical-align:middle;" src="../Images/o_hsep.gif" />
			&nbsp;
			<asp:LinkButton runat="server" ID="lbCancel" ForeColor="Black" Font-Underline="false">
				Cancel
			</asp:LinkButton>
		</div>
		<div style="background-color:#88AEE4; border-left:solid 1px #3B619C; border-right:solid 1px #3B619C; height:1px; line-height:1px; font-size:1px; margin-right:1px; margin-left:1px;"></div>
		<div style="background-color:#3B619C; height:1px; line-height:1px; font-size:1px; margin-right:2px; margin-left:2px;"></div>
		
		<div style="background-color:#F9FAF9; height:1px; line-height:1px; font-size:1px; margin-right:2px; margin-left:2px;"></div>
		<div style="background-color:#F9FAF9; height:1px; line-height:1px; font-size:1px; margin-right:1px; margin-left:1px;"></div>
		<div style="background-color:#F9FAF9; padding:5px;">
			<asp:Label runat="server" ID="lbError" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
			<table border="0" cellpadding="2" cellspacing="0" style="border-collapse:collapse; width:100%">
				<tr>
					<td>
						<table border="0" cellpadding="3" cellspacing="0">
							<tr>
								<td style="width:80px;">
									Subject:
								</td>
								<td>
									<asp:TextBox runat="server" ID="tbTitle" Width="500px"></asp:TextBox>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td >
						<table border="0" cellpadding="3" cellspacing="0">
							<tr>
								<td style="width:80px;">
									Place: 
								</td>
								<td>
									<asp:DropDownList runat="server" ID="tbPlace" Width="500px"></asp:DropDownList>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<div style="background-color:#ACA899; height:1px; line-height:1px; font-size:1px;"></div>
					</td>
				</tr>
				<tr>
					<td>
						<table cellpadding="3" cellspacing="0" >
							<tr>
								<td style="text-align:right;">
									Start: <asp:TextBox runat="server" ID="tbStartDate" Width="80px"></asp:TextBox><br /><br />
									End: <asp:TextBox runat="server" ID="tbEndDate" Width="80px"></asp:TextBox>
								</td>
								<td>
								<asp:DropDownList runat="server" ID="ddStartHour" ></asp:DropDownList> :
								<asp:DropDownList runat="server" ID="ddStartMinute" ></asp:DropDownList>
								<asp:CheckBox runat="server" ID="cbIsAllDay" Text="All Day" /><br /><br />
								<asp:DropDownList runat="server" ID="ddEndHour" ></asp:DropDownList> :
								<asp:DropDownList runat="server" ID="ddEndMinute" ></asp:DropDownList>
								</td>
							</tr>
						</table>

					</td>
				</tr>
				<tr>
					<td>
						<div style="background-color:#ACA899; height:1px; line-height:1px; font-size:1px;"></div>
					</td>
				</tr>
				<tr>
					<td>
						<div style="position:relative; padding-top:10px;">
						<div style="height:1px; line-height:1px; font-size:1px; margin-left:2px; margin-right:2px; background-color:#D0D0BF;"></div>
						<div style="height:1px; line-height:1px; font-size:1px; margin-left:1px; margin-right:1px; border-left:solid 1px #D0D0BF; border-right:solid 1px #D0D0BF;"></div>
						
						<div style="border-left:solid 1px #D0D0BF; border-right:solid 1px #D0D0BF; padding:3px;">
						<table border="0" cellpadding="3" cellspacing="0" style="table-layout:fixed; overflow:hidden;" >
							<tr>
								<td style="width:100px; ">
									<asp:RadioButtonList runat="server" ID="rblRecType" AutoPostBack="true">
										<asp:ListItem Text="Not recurring" Value="0"></asp:ListItem>
										<asp:ListItem Text="Daily" Value="1"></asp:ListItem>
										<asp:ListItem Text="Weekly" Value="2"></asp:ListItem>
										<asp:ListItem Text="Monthly" Value="3"></asp:ListItem>
										<asp:ListItem Text="Yearly" Value="4"></asp:ListItem>
									</asp:RadioButtonList>
								</td>
								<td style="width:1px; border-left: solid 1px #ACA899; ">
								&nbsp;
								</td>
								<td runat="server" id="tdRecDaily" visible="false">
									<asp:RadioButton runat="server" ID="rbRecDailyDay" Text="Every " GroupName="DailyRec" />
									<asp:TextBox runat="server" ID="tbDailyRec" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox> day(s)<br /><br />
									<asp:RadioButton runat="server" ID="rbRecDailyWorkingDays" Text="Every working day" GroupName="DailyRec" />
								</td>
								<td runat="server" id="tdRecWeekly" visible="false">
									Repeat every <asp:TextBox runat="server" ID="tbWeeklyRec" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox> week on<br />
									<asp:CheckBox runat="server" ID="cbRecWeeklySunday" Text="Sunday" />
									<asp:CheckBox runat="server" ID="cbRecWeeklyMonday" Text="Monday" />
									<asp:CheckBox runat="server" ID="cbRecWeeklyTuesday" Text="Tuesday" />
									<asp:CheckBox runat="server" ID="cbRecWeeklyWednesday" Text="Wednesday" /><br />
									<asp:CheckBox runat="server" ID="cbRecWeeklyThursday" Text="Thursday" />
									<asp:CheckBox runat="server" ID="cbRecWeeklyFriday" Text="Friday" />
									<asp:CheckBox runat="server" ID="cbRecWeeklySaturday" Text="Saturday" />
								</td>
								<td runat="server" id="tdRecMonthly" visible="false" >
									<table border="0" cellpadding="3" cellspacing="0">
										<tr>
											<td>
												<asp:RadioButton runat="server" ID="rbRecMonthDayNum" Text="on " GroupName="MonthlyRec" />
												<asp:TextBox runat="server" ID="tbRecMonthDayNum" Width="40px"></asp:TextBox> -th day of every
											</td>
											<td rowspan="2">
												<asp:TextBox runat="server" ID="tbRecMonthFreq" Width="40px"></asp:TextBox> -th month
											</td>	
										</tr>
										<tr>
											<td>
												<asp:RadioButton runat="server" ID="rbRecMonthWeekDay" Text="on " GroupName="MonthlyRec" /> 
												<asp:DropDownList runat="server" ID="ddRecMonthWeekNum">
													<asp:ListItem Text="First" Value="0"></asp:ListItem>
													<asp:ListItem Text="Second" Value="1"></asp:ListItem>
													<asp:ListItem Text="Third" Value="2"></asp:ListItem>
													<asp:ListItem Text="Fourth" Value="3"></asp:ListItem>
													<asp:ListItem Text="Last" Value="4"></asp:ListItem>
												</asp:DropDownList> 
												<asp:DropDownList runat="server" ID="ddRecMonthWeekDay">
													<asp:ListItem Text="Sun" Value="1"></asp:ListItem>
													<asp:ListItem Text="Mon" Value="2"></asp:ListItem>
													<asp:ListItem Text="Tue" Value="4"></asp:ListItem>
													<asp:ListItem Text="Wed" Value="8"></asp:ListItem>
													<asp:ListItem Text="Thu" Value="16"></asp:ListItem>
													<asp:ListItem Text="Fri" Value="32"></asp:ListItem>
													<asp:ListItem Text="Sat" Value="64"></asp:ListItem> 
												</asp:DropDownList> of every
											</td>
										</tr>
									</table>
								</td>
								<td runat="server" id="tdRecYearly">
									<table border="0" cellpadding="3" cellspacing="0">
										<tr>
											<td>
												<asp:RadioButton runat="server" ID="rbRecYearDay" Text="on " GroupName="YearlyRec" />
												<asp:TextBox runat="server" ID="tbRecYearDayNum" Width="40px"></asp:TextBox> -th day of
											</td>
											<td rowspan="2">
												<asp:DropDownList runat="server" ID="ddRecYearMonth">
													<asp:ListItem Text="January" Value="1"></asp:ListItem>
													<asp:ListItem Text="February" Value="2"></asp:ListItem>
													<asp:ListItem Text="March" Value="3"></asp:ListItem>
													<asp:ListItem Text="April" Value="4"></asp:ListItem>
													<asp:ListItem Text="May" Value="5"></asp:ListItem>
													<asp:ListItem Text="June" Value="6"></asp:ListItem>
													<asp:ListItem Text="July" Value="7"></asp:ListItem>
													<asp:ListItem Text="August" Value="8"></asp:ListItem>
													<asp:ListItem Text="September" Value="9"></asp:ListItem>
													<asp:ListItem Text="October" Value="10"></asp:ListItem>
													<asp:ListItem Text="November" Value="11"></asp:ListItem>
													<asp:ListItem Text="December" Value="12"></asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td>
												<asp:RadioButton runat="server" ID="rbRecYearWeekDay" Text="on " GroupName="YearlyRec" /> 
												<asp:DropDownList runat="server" ID="ddRecYearWeekNum">
													<asp:ListItem Text="First" Value="0"></asp:ListItem>
													<asp:ListItem Text="Second" Value="1"></asp:ListItem>
													<asp:ListItem Text="Third" Value="2"></asp:ListItem>
													<asp:ListItem Text="Fourth" Value="3"></asp:ListItem>
													<asp:ListItem Text="Last" Value="4"></asp:ListItem>
												</asp:DropDownList> 
												<asp:DropDownList runat="server" ID="ddRecYearWeekDay">
													<asp:ListItem Text="Sun" Value="1"></asp:ListItem>
													<asp:ListItem Text="Mon" Value="2"></asp:ListItem>
													<asp:ListItem Text="Tue" Value="4"></asp:ListItem>
													<asp:ListItem Text="Wed" Value="8"></asp:ListItem>
													<asp:ListItem Text="Thu" Value="16"></asp:ListItem>
													<asp:ListItem Text="Fri" Value="32"></asp:ListItem>
													<asp:ListItem Text="Sat" Value="64"></asp:ListItem> 
												</asp:DropDownList> of
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
						</div>
							<div style="height:1px; line-height:1px; font-size:1px; margin-left:1px; margin-right:1px; border-left:solid 1px #D0D0BF; border-right:solid 1px #D0D0BF;"></div>
							<div style="height:1px; line-height:1px; font-size:1px; margin-left:2px; margin-right:2px; background-color:#D0D0BF;"></div>
							<span style="position:absolute; left:10px; top:3px; background-color:#F9FAF9; padding-left:3px; padding-right:3px;">
								<b>
									Recurrence:
								</b>
							</span>
						</div>
					</td>
				</tr>
				<tr runat="server" id="trRecEndType">
					<td>
						<div style="position:relative; padding-top:10px;">
							<div style="height:1px; line-height:1px; font-size:1px; margin-left:2px; margin-right:2px; background-color:#D0D0BF;"></div>
							<div style="height:1px; line-height:1px; font-size:1px; margin-left:1px; margin-right:1px; border-left:solid 1px #D0D0BF; border-right:solid 1px #D0D0BF;"></div>
							
							<div style="border-left:solid 1px #D0D0BF; border-right:solid 1px #D0D0BF; padding:3px;">
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td valign="top" style="width:150px; padding-top:5px;">
										Starts: <asp:TextBox runat="server" ID="tbRecStartDate" Width="80px" Font-Names="Arial, Sans-Serif" Font-Size="12px" ></asp:TextBox>
									</td>
									<td>
										<asp:RadioButton runat="server" ID="rbRecNeverEnd" Text="Never" GroupName="RET" Font-Names="Arial, Sans-Serif" Font-Size="12px" /><br />
										<div style="height:3px;"></div>
										<asp:RadioButton runat="server" ID="rbRecEndDate" Text="Until" GroupName="RET" Font-Names="Arial, Sans-Serif" Font-Size="12px" /> <asp:TextBox runat="server" ID="tbRecEndDate"  Width="80px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox><br />
										<div style="height:3px;"></div>
										<asp:RadioButton runat="server" ID="rbRecOccNum" Text="End after" GroupName="RET" Font-Names="Arial, Sans-Serif" Font-Size="12px" /> <asp:TextBox runat="server" ID="tbRecOccNum" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox> occurences
									</td>
								</tr>
							</table>
							</div>
							
							<div style="height:1px; line-height:1px; font-size:1px; margin-left:1px; margin-right:1px; border-left:solid 1px #D0D0BF; border-right:solid 1px #D0D0BF;"></div>
							<div style="height:1px; line-height:1px; font-size:1px; margin-left:2px; margin-right:2px; background-color:#D0D0BF;"></div>
							<span style="position:absolute; left:10px; top:3px; background-color:#F9FAF9; padding-left:3px; padding-right:3px;">
								<b>
									Recurrence range:
								</b>
							</span>
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<div style="background-color:#ACA899; height:1px; line-height:1px; font-size:1px;"></div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:TextBox runat="server" ID="tbDescription" TextMode="MultiLine" Width="560px" Height="200px"></asp:TextBox>
					</td>
				</tr>
			</table>
		</div>
		<div style="background-color:#F9FAF9; height:1px; line-height:1px; font-size:1px; margin-right:1px; margin-left:1px;"></div>
		<div style="background-color:#F9FAF9; height:1px; line-height:1px; font-size:1px; margin-right:2px; margin-left:2px;"></div>
    </div>
    </form>
</body>
</html>
