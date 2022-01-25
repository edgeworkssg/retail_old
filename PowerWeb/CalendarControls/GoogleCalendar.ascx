<%@ Control Language="C#" AutoEventWireup="true" Inherits="CalendarControls_GoogleCalendar" Codebehind="GoogleCalendar.ascx.cs" %>
<%@ Register Assembly="Mediachase.AjaxCalendar" Namespace="Mediachase.AjaxCalendar"
	TagPrefix="mc" %>



<table border="0" cellpadding="0" cellspacing="0" style="width:100%">
	<tr>
		<td>
			<asp:ImageButton runat="server" ID="ibPrev" ImageUrl="~/Images/g_btn_prev.gif" ImageAlign="AbsMiddle" />
			<asp:ImageButton runat="server" ID="ibNext" ImageUrl="~/Images/g_btn_next.gif" ImageAlign="AbsMiddle" />
			<asp:Button runat="server" ID="btnToday" Text="Today" Font-Size="13px" Font-Names="Arial, Sans-serif" />
			<asp:Label runat="server" ID="lbDatesRange" Text="Dates range" Font-Bold="true" Font-Underline="false" Font-Names="Arial" Font-Size="13px"></asp:Label>
		</td>
		<td align="right">
			<table border="0" cellpadding="2" cellspacing="0" style="height:100%; text-align:right;">
				<tr>
					<td style="padding-bottom:0px;">
						<div runat="server" id="divDayTop1" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:2px; margin-right:2px;"></div>
						<div runat="server" id="divDayTop2" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:1px; margin-right:1px;"></div>
						<div runat="server" id="divDay" style="background-color:#E8EEF7; padding:4px 5px 3px 5px;">
							<asp:LinkButton ForeColor="Black" runat="server" ID="lbDay" Text="Day" Font-Underline="false" Font-Names="arial" Font-Size="13px"></asp:LinkButton>
						</div>								
					</td>
					<td style="padding-bottom:0px">
						<div runat="server" id="divWeekTop1" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:2px; margin-right:2px;"></div>
						<div runat="server" id="divWeekTop2" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:1px; margin-right:1px;"></div>
						<div runat="server" id="divWeek" style="background-color:#E8EEF7; padding:4px 5px 3px 5px;">
							<asp:LinkButton ForeColor="Black" runat="server" ID="lbWeek" Text="Week" Font-Underline="false" Font-Names="arial" Font-Size="13px"></asp:LinkButton>
						</div>								
					</td>
					<td style="padding-bottom:0px">
						<div runat="server" id="divMonthTop1" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:2px; margin-right:2px;"></div>
						<div runat="server" id="divMonthTop2" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:1px; margin-right:1px;"></div>
						<div runat="server" id="divMonth" style="background-color:#E8EEF7; padding:4px 5px 3px 5px;">
							<asp:LinkButton ForeColor="Black" runat="server" ID="lbMonth" Text="Month" Font-Underline="false" Font-Names="arial" Font-Size="13px" ></asp:LinkButton>
						</div>								
					</td>
					<td style="padding-bottom:0px">
						<div runat="server" id="divYearTop1" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:2px; margin-right:2px;"></div>
						<div runat="server" id="divYearTop2" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:1px; margin-right:1px;"></div>
						<div runat="server" id="divYear" style="background-color:#E8EEF7; padding:4px 5px 3px 5px;">
							<asp:LinkButton  ForeColor="Black" runat="server" ID="lbYear" Text="Year" Font-Underline="false" Font-Names="arial" Font-Size="13px"></asp:LinkButton>
						</div>								
					</td>
					<td style="padding-bottom:0px">
						<div runat="server" id="divTaskTop1" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:2px; margin-right:2px;"></div>
						<div runat="server" id="divTaskTop2" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:1px; margin-right:1px;"></div>
						<div runat="server" id="divTask" style="background-color:#E8EEF7; padding:4px 5px 3px 5px;">
							<asp:LinkButton  ForeColor="Black" runat="server" ID="lbTask" Text="Task (day scale)" Font-Underline="false" Font-Names="arial" Font-Size="13px"></asp:LinkButton>
						</div>								
					</td>
					<td style="padding-bottom:0px">
						<div runat="server" id="divTaskHourTop1" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:2px; margin-right:2px;"></div>
						<div runat="server" id="divTaskHourTop2" style="font-size:1px; line-height:1px; background-color:#E8EEF7; height:1px; margin-left:1px; margin-right:1px;"></div>
						<div runat="server" id="divTaskHour" style="background-color:#E8EEF7; padding:4px 5px 3px 5px;">
							<asp:LinkButton  ForeColor="Black" runat="server" ID="lbTaskHour" Text="Task (hour scale)" Font-Underline="false" Font-Names="arial" Font-Size="13px"></asp:LinkButton>
						</div>								
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td colspan="2" style="font-family:Arial; font-size:11px;">
			<div style="font-size:1px; line-height:1px; background-color:#C3D9FF; height:1px; margin-left:2px; margin-right:2px;"></div>
			<div style="font-size:1px; line-height:1px; background-color:#C3D9FF; height:1px; margin-left:1px; margin-right:1px;"></div>
			<div style="background-color:#C3D9FF; padding-left:5px; padding-right:5px; padding-bottom:5px;">
			<mc:MediachaseAjaxCalendar runat="server" ID="gCalendar" 
				DrillDownEnabled="true">
				<DataSource ItemsWebServiceFullName="WebSample.Default" ItemsWebServicePath="~/Default.asmx" />
				<MultiDayView DayStartHour="0" DayEndHour="23" TodayBackgroundColor="#FFFFCC" 
					GridRowBorderStyle="Solid" GridRowBorderColor="#DDDDDD"
					GridRowBorderWidth="1" GridRowAlternativeBorderStyle="Dotted"
					GridRowAlternativeBorderColor="#DDDDDD" GridColumnSeparatorBorderColor="#DDDDDD"
					GridColumnSeparatorBorderStyle="Solid" GridColumnSeparatorBorderWidth="1"
					GridColumnSeparatorWidth="1" SelectionColor="#CCCCCC" HeaderTodayBackgroundColor="#88AACC"
					UseDefaultCreateHandler="false" > 
					<HeaderStyle Font-Names="Arial" Font-Size="11px" Height="15px" BackColor="#C3D9FF"  />
					<TimeColumnStyle Font-Names="Arial" Font-Size="11px" BackColor="#E8EEF7"/>
					<EventBarStyle BackColor="#E8EEF7" Height="0px" />
					<GridStyle BackColor="#FFFFFF" />
				</MultiDayView>
				<MonthView UseDefaultCreateHandler="false" TodayBackgroundColor="#FFFFCC" GridRowBorderStyle="Solid"
					GridRowBorderColor="#DDDDDD" GridRowBorderWidth="1" 
					GridColumnSeparatorBorderColor="#DDDDDD" GridColumnSeparatorBorderStyle="Solid"
					GridColumnSeparatorBorderWidth="1" GridColumnSeparatorWidth="0"
					SelectionColor="#CCCCCC">
					<HeaderStyle Font-Names="Arial" Font-Size="11px" Height="15px" BackColor="#C3D9FF"  />
					<GridStyle Font-Names="Verdana"  BackColor="White" />
				</MonthView>
				<YearView TodayBackgroundColor="#FFFFCC" GridRowBorderColor="#DDDDDD" GridRowBorderWidth="1" 
					GridColumnSeparatorBorderColor="#DDDDDD" GridColumnSeparatorBorderStyle="Solid"
					GridColumnSeparatorBorderWidth="1" GridColumnSeparatorWidth="0">
					<HeaderStyle Font-Names="Arial" Font-Size="11px" Height="15px" BackColor="#C3D9FF" />
					<GridStyle Font-Names="Verdana" BackColor="WHITE" />
				</YearView>
				<TaskView DaysCount="2" UseDefaultCreateHandler="false" ItemsPadding="5" TodayBackgroundColor="#FFFFCC"
					GridRowBorderColor="#0094FF" ItemHeight="20" >
					<GridStyle Height="0px" BackColor="#ffffff"  />
					<TitleColumnStyle Font-Names="Arial" Font-Size="11px" BackColor="#E8EEF7"  />
				</TaskView>
			</mc:MediachaseAjaxCalendar>
			</div>
			<div style="font-size:1px; line-height:1px; background-color:#C3D9FF; height:1px; margin-left:1px; margin-right:1px;"></div>
			<div style="font-size:1px; line-height:1px; background-color:#C3D9FF; height:1px; margin-left:2px; margin-right:2px;"></div>
		</td>
	</tr>
</table>	


<script type="text/javascript">
	function InitUpdateHandler()
	{
		var view = $find('<%=this.gCalendar.ClientViewId%>');
		if(view)
		{
		<%if(this.gCalendar.ViewType == Mediachase.AjaxCalendar.CalendarViewType.MultiDay){%>
		  var eventbar = $find('<%=this.gCalendar.ClientEventBarId%>');	
		  if(eventbar) 
		  {	
			s=null; 
			s = new GoogleEditor(view, eventbar);
		  }
		<%}else{%>
			s = new GoogleEditor(view, null);
		<%}%>
		}
		else
		{
			setTimeout(InitUpdateHandler, 500);
			return;
		}
	}
	InitUpdateHandler();
	</script>