<%@ Control Language="C#" AutoEventWireup="true" Inherits="CalendarControls_OutlookCalendar" Codebehind="OutlookCalendar.ascx.cs" %>
<%@ Register Assembly="Mediachase.AjaxCalendar" Namespace="Mediachase.AjaxCalendar"
	TagPrefix="mc" %>


<table cellpadding="0" cellspacing="3" border="0" width="100%" style="background-color:#C4DAFA;">
	<tr style="background-image:url(Images/o_ch0bg.gif); background-repeat:repeat-x; height:25px; font-family:Tahoma; font-size:12px;">
		<td>
		&nbsp;
		</td>
		<td style="padding-left:5px; vertical-align:middle; vertical-align:middle;">
			<asp:LinkButton runat="server" ID="lbToday" Text="Today" Font-Underline="false" ForeColor="Black"></asp:LinkButton>
			&nbsp;
			<img style="border:0; vertical-align:middle;" src="Images/o_hsep.gif" />
			&nbsp;
			<asp:LinkButton runat="server" ID="lbDayView" Font-Underline="false" ForeColor="Black">
				<img style="border:0; vertical-align:middle;" src="Images/o_dayview.gif" />
				Day
			</asp:LinkButton>
			&nbsp;
			<asp:LinkButton runat="server" ID="lbWorkWeekView"  Font-Underline="false" ForeColor="Black">
				<img style="border:0; vertical-align:middle;" src="Images/o_workweekview.gif" />
				Work Week
			</asp:LinkButton>
			&nbsp;
			<asp:LinkButton runat="server" ID="lbWeekView" Font-Underline="false" ForeColor="Black">
				<img style="border:0; vertical-align:middle;" src="Images/o_weekview.gif" />
				Week
			</asp:LinkButton>
			&nbsp;
			<asp:LinkButton runat="server" ID="lbMonthView"  Font-Underline="false" ForeColor="Black">
				<img style="border:0; vertical-align:middle;" src="Images/o_monthview.gif" />
				Month
			</asp:LinkButton>
			&nbsp;
			<asp:LinkButton runat="server" ID="lbYearView"  Font-Underline="false" ForeColor="Black">
				<img style="border:0; vertical-align:middle;" src="Images/o_yearview.gif" />
				Year
			</asp:LinkButton>
			&nbsp;
			<asp:LinkButton runat="server" ID="lbTaskView"  Font-Underline="false" ForeColor="Black">
				<img style="border:0; vertical-align:middle;" src="Images/o_taskview.gif" />
				Task (day scale)
			</asp:LinkButton>
			&nbsp;
			<asp:LinkButton runat="server" ID="lbTaskHourView"  Font-Underline="false" ForeColor="Black">
				<img style="border:0; vertical-align:middle;" src="Images/o_taskview.gif" />
				Task (hour scale)
			</asp:LinkButton>
		</td>
	</tr>
	<tr style="background-image:url(Images/o_chbg.gif); background-repeat:repeat-x; height:25px;" >
		<td style="padding-left:3px; font-family:Tahoma; color:White; font-size:16px; font-weight:bold;" >
		Calendar
		</td>
		<td align="right" style="font-family:Tahoma; font-size:14px; color:White; padding-right:5px;">
			<asp:Label runat="server" ID="lbDatesRange" Text="Dates Range."></asp:Label>
			<img border="0" src="Images/o_calendar_icon.gif" />
		</td>
	</tr>
	<tr>
		<td style="width:150px; vertical-align:top; background-color:White; text-align:center; padding:3px;">
				<asp:Calendar runat="server" ID="cSmall" SelectionMode="Day" BorderWidth="0" 
					Font-Names="Tahoma" Font-Size="8pt" NextMonthText="<img src='Images/o_btn_next.gif' border='0'>" 
					PrevMonthText="<img src='Images/o_btn_prev.gif' border='0'>"  >
					<TitleStyle  BackColor="#9EBEF5" />
					<DayStyle Font-Underline="false" />
					<OtherMonthDayStyle  ForeColor="#ACA899"/>
					<TodayDayStyle BorderColor="#BB5503" BorderStyle="solid" BorderWidth="1px" />
					<SelectedDayStyle BackColor="#FBE694"   ForeColor="Black" />
				</asp:Calendar>
		</td>
		<td id="cCont" style="background-color:#ECE9D8; font-family:Arial; font-size:11px;" >
			<mc:MediachaseAjaxCalendar runat="server" ID="oCalendar"  
				DrillDownEnabled="false" ViewMode="Day" ViewType="MultiDay" >
				<DataSource ItemsWebServiceFullName="WebSample.Default" ItemsWebServicePath="~/Default.asmx" />
				<MultiDayView DayStartHour="0" DayEndHour="23" UseDefaultCreateHandler="False"
					TodayBackgroundColor="#FFF4BC" GridRowBorderStyle="Solid" GridRowBorderColor="#EAD098"
					GridRowBorderWidth="1" GridRowAlternativeBorderStyle="Solid" 
					GridRowAlternativeBorderColor="#F3E4B1" GridColumnSeparatorBorderColor="#EAD098"
					GridColumnSeparatorBorderStyle = "Solid" GridColumnSeparatorBorderWidth="2"
					GridColumnSeparatorWidth="0" SelectionColor="Blue" EventBarBottomPadding="0"
					HeaderTodayBackgroundColor="#F7D277" HeaderDateFormat="d MMMM" > 
					<HeaderStyle Font-Names="Arial" Font-Size="11px" Height="15px" BackColor="#ECE9D8" ForeColor="#000" HorizontalAlign="Center"  />
					<TimeColumnStyle Font-Names="Tahoma" Font-Size="Small" BackColor="#ECE9D8" HorizontalAlign="Center" Width="60px" Font-Bold="true" />
					<GridStyle BackColor="#FFFFD5" Height="400px" />
					<EventBarStyle BackColor="#ACA899" Height="0px" />
				</MultiDayView>
				<MonthView UseDefaultCreateHandler="False" AbbreviatedDayNames="True" AddNotRenderedItemsCount="True"
				 TodayBackgroundColor="#FCD671" SelectedMonthBackgroundColor="#FFFFD5" GridRowBorderStyle="Solid"
				 GridRowBorderColor="Black" GridRowBorderWidth="1" SelectionColor="Blue"
				 GridColumnSeparatorBorderColor="Black" GridColumnSeparatorBorderStyle="Solid"
				 GridColumnSeparatorBorderWidth="1" GridColumnSeparatorWidth="0">
					<HeaderStyle Font-Names="Arial" Font-Size="11px" Height="15px" BackColor= "#ECE9D8" ForeColor="Black" HorizontalAlign="Center"  />
					<GridStyle Font-Names="Verdana" BackColor="#FFF4BC" Height="400px" />
					<PopUpStyle BackColor="LightSlateGray" BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
				</MonthView>
				<YearView UseDefaultCreateHandler="False" AbbreviatedMonthNames="True"
					TodayBackgroundColor="#FCD671" SelectedYearBackgroundColor="#FFFFD5" 
					GridRowBorderStyle="Solid"	GridRowBorderColor="Black" GridRowBorderWidth="1" 
					GridColumnSeparatorBorderColor="Black" GridColumnSeparatorBorderStyle="Solid"
					GridColumnSeparatorBorderWidth="1" GridColumnSeparatorWidth="0" >
					<HeaderStyle Font-Names="Arial" Font-Size="11px" Height="15px" BackColor="#ECE9D8" HorizontalAlign="Center" />
					<GridStyle Font-Names="Verdana" BackColor="#FFF4BC" Height="400px" />
					<PopUpStyle BackColor="LightSlateGray" BorderStyle="Solid" />
				</YearView>
				<TaskView UseDefaultCreateHandler="False" TodayBackgroundColor="#FCD671" SelectionColor="Blue"
					ItemsPadding="5" DaysCount="30" GridColumnSeparatorBorderColor="#EAD098" GridColumnSeparatorAlternativeBorderColor="#EAD098"
					GridRowBorderColor="#EAD098" GridColumnSeparatorBorderWidth="2"
					HeaderTodayBackgroundColor="#FCD671">
					<GridStyle BackColor="#FFFFD5" Height="0px"/>
					<HeaderStyle BackColor="#ECE9D8" />
					<TitleColumnStyle BackColor="#ECE9D8" Font-Names="Tahoma" Font-Size="11px" />
				</TaskView>
			</mc:MediachaseAjaxCalendar>
		</td>
	</tr>
</table>

<script type="text/javascript">
	
	function CorrectTaskHeader()
	{
		var c = document.getElementById("cCont");
		if(c!=null && navigator.userAgent.indexOf("MSIE")>-1)
		{
			if(c.childNodes[0].childNodes[0].childNodes[0].tagName.toUpperCase()=="DIV")
				c.childNodes[0].childNodes[0].childNodes[1].childNodes[0].childNodes[0].childNodes[1].childNodes[0].style.top=null;
			else
				c.childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].style.top=null;
		}
	}
	
	OutlookEventHandler = function(view, eventbar)
	{
		if(view)
		{
			view.add_itemUpdating(Function.createDelegate(this, this.OnItemUpdating));
			view.add_itemCreated(Function.createDelegate(this, this.OnItemCreated));
		}
		if(eventbar)
		{
			eventbar.add_itemUpdating(Function.createDelegate(this, this.OnItemUpdating));
			eventbar.add_itemCreated(Function.createDelegate(this, this.OnItemCreated));
		}
	}

	OutlookEventHandler.prototype.OnItemUpdating = function(sender, args)
	{
		if(args.MouseEvent.type=='mouseup' && sender.get_ActionName()=="")
		{
			if(hostPath==null || hostPath=='undefined')
				window.location.href = window.location.href;
			else
				window.location.href = hostPath+"EditorPages/OutlookEventEdit.aspx?EventId="+args.Uid;
		}
	}
	
	OutlookEventHandler.prototype.OnItemCreated = function(sender, args)
	{
		if(args.MouseEvent.type=='mouseup' && sender.get_ActionName()=="")
		{
			var sd = args.StartDate;
			var ed = args.EndDate;	
			var link = "";
			if(hostPath==null || hostPath=='undefined')
				link = "http://localhost/MCAjaxCalendar/EditorPages/OutlookEventEdit.aspx?";
			else
			    link = hostPath+"EditorPages/OutlookEventEdit.aspx?";	
			var sm = sd.getMonth()+1;
			var em = ed.getMonth()+1;
			var s = "StartDate="+ sm +"\/"+sd.getDate()+"\/"+sd.getFullYear()+" "+sd.getHours()+":"+sd.getMinutes();
			var e = "&EndDate="+ em +"\/"+ed.getDate()+"\/"+ed.getFullYear()+" "+ed.getHours()+":"+ed.getMinutes();
			var ad = "&IsAllDay="+args.IsAllDay.toString();
			link+=s+e+ad;
			window.location.href = link;
		}
	}
	
	
	function InitUpdateHandler()
	{
		var view = $find('<%=this.oCalendar.ClientViewId%>');
		if(view)
		{
		<%if(this.oCalendar.ViewType == Mediachase.AjaxCalendar.CalendarViewType.MultiDay){%>
		  var eventbar = $find('<%=this.oCalendar.ClientEventBarId%>');	
		  if(eventbar) 
		  {	
			s=null; 
			s = new OutlookEventHandler(view, eventbar);
		  }
		<%}else{%>
			s = new OutlookEventHandler(view, null);
		<%}%>
		<%if(this.oCalendar.ViewType == Mediachase.AjaxCalendar.CalendarViewType.Task){%>
		CorrectTaskHeader();
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


