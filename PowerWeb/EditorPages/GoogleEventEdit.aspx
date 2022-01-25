<%@ Page Language="C#" AutoEventWireup="true" Inherits="EditorPages_GoogleEventEdit" Codebehind="GoogleEventEdit.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Event</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:600px;">
		<div style="font-size:1px; line-height:1px; background-color:#C3D9FF; height:1px; margin-left:2px; margin-right:2px;"></div>
	    <div style="font-size:1px; line-height:1px; background-color:#C3D9FF; height:1px; margin-left:1px; margin-right:1px;"></div>
		<div style="background-color:#C3D9FF; padding:10px; font-family:Arial, Sans-Serif; font-size:12px;">
			<asp:LinkButton runat="server" ID="lbBack" Text="<< Back to calendar"></asp:LinkButton>
			<asp:Button runat="server" ID="btnSave" Text="Save" />
			<asp:Button runat="server" ID="btnCancel" Text="Cancel" />
			<asp:Button runat="server" ID="btnDelete" Text="Delete" />
			<br />
			<asp:Label runat="server" ID="lbError" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
			<br />
			<div style="background-color:White; padding:10px;">
				<div style="font-size:1px; line-height:1px; background-color:#D2E6D2; height:1px; margin-left:2px; margin-right:2px;"></div>
				<div style="font-size:1px; line-height:1px; background-color:#D2E6D2; height:1px; margin-left:1px; margin-right:1px;"></div>
					<div style="background-color:#D2E6D2;">
						<table cellpadding="5" cellspacing="0" border="0">	
							<tr>
								<td>
									<b>What</b>
								</td>
								<td>
									<asp:TextBox runat="server" ID="tbTitle" Width="300px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox>
								</td>	
							</tr>
							<tr>
								<td>
									<b>When</b>
								</td>
								<td>
									<asp:TextBox runat="server" ID="tbStartDate" Width="80px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox>
									<asp:DropDownList runat="server" ID="ddStartHour" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:DropDownList>
									<asp:DropDownList runat="server" ID="ddStartMinute" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:DropDownList>
									to
									<asp:TextBox runat="server" ID="tbEndDate" Width="80px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox>
									<asp:DropDownList runat="server" ID="ddEndHour" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:DropDownList>
									<asp:DropDownList runat="server" ID="ddEndMinute" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:DropDownList>
									<asp:CheckBox runat="server" ID="cbIsAllDay" Text="All Day" Font-Names="Arial, Sans-Serif" Font-Size="12px" AutoPostBack="true" />
								</td>
							</tr>
							<tr>
								<td>
									<b>
										Repeats:
									</b>
								</td>
								<td>
									<%--<mc:gRecEditor runat="server" ID="recEditor"  />--%>
									<table border="0" cellpadding="0" cellspacing="5" width="100%">
										<tr>
											<td style="border-bottom: solid 2px #E0F1E0; padding:3px;">
												<asp:DropDownList runat="server" ID="ddRecType" AutoPostBack="true" Font-Names="Arial, Sans-Serif" Font-Size="12px" >	
													<asp:ListItem Text="Does not repeat" Value="0"></asp:ListItem>
													<asp:ListItem Text="Daily" Value="1"></asp:ListItem>
													<asp:ListItem Text="Every week day (Mon-Fri)" Value="2"></asp:ListItem>
													<asp:ListItem Text="Every Mon., Wed., and Fri." Value="3"></asp:ListItem>
													<asp:ListItem Text="Every Tues., and Thurs." Value="4"></asp:ListItem>
													<asp:ListItem Text="Weekly" Value="5"></asp:ListItem>
													<asp:ListItem Text="Monthly" Value="6"></asp:ListItem>
													<asp:ListItem Text="Yearly" Value="7"></asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										
										<tr runat="server" id="trDailyRec" visible="false">
											<td style="border-bottom: solid 2px #E0F1E0; padding: 3px;">
												<table border="0" cellpadding="0" cellspacing="0" width="100%">
													<tr>
														<td style="border-bottom: solid 2px #E0F1E0; padding: 0px 3px 3px 3px;">
															<div style="padding:10px; background-color:#B0D4B0; width:120px; text-align:center; border: solid 1px #737B73;">
																Every <asp:Label runat="server" ID="lbDailyRec" Text="i-th"></asp:Label> day(s)
															</div>
														</td>
													</tr>
													<tr>
														<td style="padding:3px;">
															Every <asp:TextBox runat="server" ID="tbDailyRec" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox> day(s)
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr runat="server" id="trWorkingDaysRec" visible="false">
											<td style="border-bottom: solid 2px #E0F1E0; padding: 0px 3px 3px 3px;">
												<div style="padding:10px; background-color:#B0D4B0; text-align:center; border: solid 1px #737B73;">
													Weekly on weekdays<asp:Label runat="server" ID="lbWorkingDaysRecEnd" Text=""></asp:Label> 
												</div>
											</td>
										</tr>
										<tr runat="server" id="tr135Rec" visible="false">
											<td style="border-bottom: solid 2px #E0F1E0; padding: 0px 3px 3px 3px;">
												<div style="padding:10px; background-color:#B0D4B0; text-align:center; border: solid 1px #737B73;">
													Weekly on Monday, Wednesday, Friday<asp:Label runat="server" ID="lb135RecEnd" Text=""></asp:Label> 
												</div>
											</td>
										</tr>
										<tr runat="server" id="tr24Rec" visible="false">
											<td style="border-bottom: solid 2px #E0F1E0; padding: 0px 3px 3px 3px;">
												<div style="padding:10px; background-color:#B0D4B0; text-align:center; border: solid 1px #737B73;">
													Weekly on Tuesday, Thursday<asp:Label runat="server" ID="lb24RecEnd" Text=""></asp:Label> 
												</div>
											</td>
										</tr>
										<tr runat="server" id="trWeeklyRec" visible="false">
											<td style="padding: 0px 3px 3px 3px;">
												<table border="0" cellpadding="0" cellspacing="0" width="100%">
													<tr>
														<td style="border-bottom: solid 2px #E0F1E0; padding-bottom:3px; ">
															<div style="padding:10px; background-color:#B0D4B0; text-align:center; border: solid 1px #737B73;">
																Weekly<asp:Label runat="server" ID="lbWeeklyRec" Text=""></asp:Label> 
																<asp:Label runat="server" ID="lbWeeklyRecEnd" Text=""></asp:Label>
															</div>
														</td>
													</tr>
														<tr>
															<td style="border-bottom: solid 2px #E0F1E0;padding:3px" >
																<b>Repeat every</b><asp:TextBox runat="server" ID="tbWeeklyRec" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox> week
															</td>
														</tr>
													<tr>
														<td style="border-bottom: solid 2px #E0F1E0;padding:3px">
															<b>Repeat On:</b><br />
															<asp:CheckBox runat="server" ID="cbWeeklySunday" Text="Sunday" />
															<asp:CheckBox runat="server" ID="cbWeeklyMonday" Text="Monday" />
															<asp:CheckBox runat="server" ID="cbWeeklyTuesday" Text="Tuesday" />
															<asp:CheckBox runat="server" ID="cbWeeklyWednesday" Text="Wednesday" /><br />
															<asp:CheckBox runat="server" ID="cbWeeklyThursday" Text="Thursday" />
															<asp:CheckBox runat="server" ID="cbWeeklyFriday" Text="Friday" />
															<asp:CheckBox runat="server" ID="cbWeeklySaturday" Text="Saturday" />
														</td>
													</tr>
												</table>

											</td>
										</tr>
										<tr runat="server" id="trMonthlyRec" visible="false">
											<td style="padding: 0px 3px 3px 3px;">
												<table border="0" cellpadding="0" cellspacing="0" width="100%">
													<tr>
														<td style="border-bottom: solid 2px #E0F1E0; padding-bottom:3px; ">
															<div style="padding:10px; background-color:#B0D4B0; text-align:center; border: solid 1px #737B73;">
																Monthly<asp:Label runat="server" ID="lbMonthlyRec" Text=""></asp:Label>
																<asp:Label runat="server" ID="lbMonthlyRecEnd" Text=""></asp:Label>
															</div>
														</td>
													</tr>
													<tr>
														<td style="border-bottom: solid 2px #E0F1E0;padding:3px" >
															<b>Repeat every</b> <asp:TextBox runat="server" ID="tbMonthlyRec" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox> month
														</td>
													</tr>
													<tr>
														<td>
															<b>Repeat By:</b><br />
															<asp:RadioButton runat="server" ID="rbMonthlyRecMonthDay" Text="day of the month" GroupName="RMR" Font-Names="Arial, Sans-Serif" Font-Size="12px" />
															<asp:RadioButton runat="server" ID="rbMonthlyRecWeekDay" Text="day of the week" GroupName="RMR" Font-Names="Arial, Sans-Serif" Font-Size="12px" />
														</td>
													</tr>
												</table>	
											</td>
										</tr>
										<tr runat="server" id="trYearlyRec">
											<td >
												<table cellpadding="0" cellspacing="0" border="0" width="100%">
													<tr>
														<td style="border-bottom: solid 2px #E0F1E0; padding: 0px 3px 3px 3px;">
															<div style="padding:10px; background-color:#B0D4B0; text-align:center; border: solid 1px #737B73;">
																Yearly<asp:Label runat="server" ID="lbYearlyRecEnd" Text="" ></asp:Label> 
															</div>
														</td>
													</tr>
													<tr>
														<td style="border-bottom: solid 2px #E0F1E0; padding: 0px 3px 3px 3px;">
															<table cellpadding="3" cellspacing="0" border="0" width="100%">
																<tr>
																	<td style="width:200px;">
																		<asp:RadioButton runat="Server" ID="rbYearlyRecMonthDay" Text="Every " GroupName="YRP" Font-Names="Arial, Sans-Serif" Font-Size="12px"/><asp:TextBox runat="server" ID="tbYearlyRecMonthDay" Width="40px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox> of
																	</td>
																	<td rowspan="2">
																		<asp:DropDownList runat="server" ID="ddYearlyRecMonth" Font-Names="Arial, Sans-Serif" Font-Size="12px">
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
																		<asp:RadioButton runat="server" ID="rbYearlyRecWeekDay" Text="on " GroupName="YRP" Font-Names="Arial, Sans-Serif" Font-Size="12px"/>
																		<asp:DropDownList runat="server" ID="ddYearlyRecWeekNum" Font-Names="Arial, Sans-Serif" Font-Size="12px">
																			<asp:ListItem Text="First" Value="0"></asp:ListItem>
																			<asp:ListItem Text="Second" Value="1"></asp:ListItem>
																			<asp:ListItem Text="Third" Value="2"></asp:ListItem>
																			<asp:ListItem Text="Fourth" Value="3"></asp:ListItem>
																			<asp:ListItem Text="Last" Value="4"></asp:ListItem>
																		</asp:DropDownList>&nbsp;
																		<asp:DropDownList runat="server" ID="ddYearlyRecWeekDay" Font-Names="Arial, Sans-Serif" Font-Size="12px">
																			<asp:ListItem Text="Sun" Value="1"></asp:ListItem>
																			<asp:ListItem Text="Mon" Value="2"></asp:ListItem>
																			<asp:ListItem Text="Tue" Value="4"></asp:ListItem>
																			<asp:ListItem Text="Wed" Value="8"></asp:ListItem>
																			<asp:ListItem Text="Thu" Value="16"></asp:ListItem>
																			<asp:ListItem Text="Fri" Value="32"></asp:ListItem>
																			<asp:ListItem Text="Sat" Value="64"></asp:ListItem>
																		</asp:DropDownList> in
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr runat="server" id="trRecEndType">
											<td style="border-bottom: solid 2px #E0F1E0; padding:3px;">
												<table border="0" cellpadding="0" cellspacing="0" width="100%">
													<tr>
														<td valign="middle">
															<b>Range:</b><br />
															Starts <asp:TextBox runat="server" ID="tbRecStartDate" Width="80px" Font-Names="Arial, Sans-Serif" Font-Size="12px" ></asp:TextBox>
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
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<b>Where</b>
								</td>
								<td>
									<asp:DropDownList runat="server" ID="tbPlace" Width="300px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:DropDownList>
								</td>	
							</tr>
							<tr>
								<td>
									<b>Course Type</b>
								</td>
								<td>
									<asp:DropDownList runat="server" ID="ddlCourseCat" Width="300px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:DropDownList>
								</td>	
							</tr>
							<tr>
								<td>
									<b>Description</b>
								</td>
								<td>
									<asp:TextBox runat="server" ID="tbDescription" Width="300px" TextMode="MultiLine" Height="150px" Font-Names="Arial, Sans-Serif" Font-Size="12px"></asp:TextBox>
								</td>	
							</tr>
						</table>
					</div>
				<div style="font-size:1px; line-height:1px; background-color:#D2E6D2; height:1px; margin-left:1px; margin-right:1px;"></div>
				<div style="font-size:1px; line-height:1px; background-color:#D2E6D2; height:1px; margin-left:2px; margin-right:2px;"></div>
			</div>
		</div>
		<div style="font-size:1px; line-height:1px; background-color:#C3D9FF; height:1px; margin-left:1px; margin-right:1px;"></div>
		<div style="font-size:1px; line-height:1px; background-color:#C3D9FF; height:1px; margin-left:2px; margin-right:2px;"></div>
		
    </div>
    </form>
</body>
</html>
