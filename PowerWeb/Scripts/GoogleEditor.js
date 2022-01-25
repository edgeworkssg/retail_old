//Default event handler for CalendarView and CalendarEventBarView
GoogleEditor = function(view, eventbar)
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
	if(this._editor == null || this._editor == 'undefined')
	{
		var d = document.createElement("DIV");
		d.id = "myGoogleEditor";
		d.style.position = 'absolute';
		d.style.width = "300px";
		d.style.height = "150px";
		d.style.display = "none";
		
		d.style.zIndex = "155";
		d.style.fontFamily = "Arial, Sans-Serif";
		d.style.lineHeight = "12pt";
		d.style.fontSize = "14px";
		
		d.innerHTML+='<img src="Images/iw_nw.gif" width="25px" height="25px" />';
		d.innerHTML+='<div style="position:absolute; top:0px; left:25px; border-top: solid 1px rgb(171,171,171); height:25px; width: 251px; background-color:#fff;"></div>';
		d.innerHTML+='<img src="Images/iw_ne.gif" width="25px" height="25px" style="position:absolute; left:276px; top:0px;" />';
		d.innerHTML+='<div style="position:absolute; height:131px; left:0px; top:25px; width:25px; border-left:solid 1px rgb(171,171,171); background-color:#fff;"></div>'
		d.innerHTML+='<div style="position:absolute; height:131px; left:275px; top:25px; width:25px; background-color:#fff; border-right:solid 1px rgb(171,171,171);"></div>';
		d.innerHTML+='<img src="Images/iw_sw.gif" width="25px" height="25px" style="position:absolute; left:0px; top:156px;" />';
		d.innerHTML+='<div style="position:absolute; top:155px; left:25px; border-bottom: solid 1px rgb(171,171,171); height:25px;background-color:#fff; width: 251px;"></div>';
		d.innerHTML+='<div style="position:absolute; top:25px; left:25px; height:130px;background-color:#fff; width: 250px;"></div>';
		d.innerHTML+='<img src="Images/iw_se.gif" width="25px" height="25px" style="position:absolute; left:276px; top:156px;" />';
		d.innerHTML+='<img src="Images/close.gif" style="position:absolute; top:5px; left:270px;" id="imClose" />';
		d.innerHTML+='<div id="divEditor" style="position:absolute; left:10px; top:25px; padding-top:5px;">What:&nbsp;<div style="overflow:auto; width:210px; position:absolute; top:0px; left:45px;"><input type="text" id="tbTitle" style="width:200px;" /></div></div>';
		d.innerHTML+='<div id="divTitle" style="position:absolute; left:10px; top:25px; width:200px; overflow:hidden; font-family: arial, sans-serif; font-weight:700; color:#2952A3;"></div>';
		d.innerHTML+='<div id="divDates" style="position:absolute; left:10px; top:50px;">';
		d.innerHTML+='<div id="lbDelete" style="position:absolute; left:130px; top:90px;">[<font style="color:blue; cursor:pointer;"><u>Delete</u></font>]</div>';
		d.innerHTML+='<div id="divSep" style="position:absolute; left:10px; top:110px; height:3px; border-top:solid 1px #2952A3; width:280px;"></div>'
		d.innerHTML+='<input type="button" id="btnCreate" style="position:absolute; left:10px; top:115px; width:100px;" value="Create Course" />'
		d.innerHTML+='<div id="lbEdit" style="position:absolute; left:130px; top:115px;"><font style="color:blue; cursor:pointer;"><a id="hlEditEvent">edit course&gt;&gt;</a></font></div>';
		
		//d.innerHTML+='<div id="lbAttend" style="position:absolute; left:130px; top:140px;"><font style="color:blue; cursor:pointer;"><a id="hlAttendEvent">Attendance</a></font></div>';
		
		document.body.appendChild(d);
		this._iTitle = $get("tbTitle");
		this._divTitle = $get("divTitle");
		this._divEditor = $get("divEditor");
		this._db = $get("lbDelete");
		this._cb = $get("imClose");
		this._btnCreate = $get("btnCreate");
		this._divSep = $get("divSep");
		this._divEdit = $get("lbEdit");
		
		this._hlEditEvent = $get("hlEditEvent");
		
		//this._divEdit = $get("lbAttend");
		//this._hlAttendEvent = $get("hlAttendEvent");
		//this._iDescription = $get("tbDescription");
		this._dates = $get("divDates");
		$addHandler(this._cb,'click', Function.createDelegate(this, this.CloseEditor));
		$addHandler(this._db,'click', Function.createDelegate(this, this.DeleteItem));
		$addHandler(this._btnCreate,'click', Function.createDelegate(this, this.CreateItem));
		$addHandler(this._hlEditEvent,'click', Function.createDelegate(this, this.LinkClick));
	//	$addHandler(this._hlAttendEvent,'click', Function.createDelegate(this, this.Attend));
		
		this._editor = d;
		this._itemUid = null;
		this._itemTitle = null;
		this._itemStartDate = null;
		this._itemEndDate = null;
		this._itemDescription = null;
		this._itemIsAllDay = null;
		this._itemExtensions = null;	
		this._itemsManager = null;
	}
}

GoogleEditor.prototype.LinkClick = function()
{
	if(this._iTitle!=null && this._hlEditEvent!=null)
	{
		if(this._iTitle.value.length>0)
		{
			this._hlEditEvent.href = this._hlEditEvent.href+"&Title="+this._iTitle.value
		}
	}
}

//GoogleEditor.prototype.Attend = function()
//{
//	if(this._iTitle!=null && this._h1AttendEvent!=null)
//	{
//		if(this._iTitle.value.length>0)
//		{
//			this._h1AttendEvent.href = this._h1AttendEvent.href+"&Title="+this._iTitle.value
//		}
//	}
//}

GoogleEditor.prototype.OnItemCreated = function(sender, args)
{
	this._editor.style.display = "block";
	var dh = 0;
	if(document.documentElement.scrollHeight>document.documentElement.clientHeight)
		dh = document.documentElement.scrollTop;
	if(args.MouseEvent.clientY-75>0)
			this._editor.style.top = args.MouseEvent.clientY-75+dh+"px";
	else	
		this._editor.style.top = "0px";
	if(args.MouseEvent.clientX-150>0)	
		this._editor.style.left = args.MouseEvent.clientX-150+"px";
	else
		this._editor.style.left = "0px";
	this._divEditor.style.display = "block";
	this._divTitle.style.display = "none";
	this._iTitle.value = "";
	this._divEditor.style.top = "70px";
	this._db.style.display = "none";
	
	//this._hlAttendEvent.style.display = "none";
	
	
	this._divSep.style.display = "none";
	this._btnCreate.style.display = "block";
	this._dates.style.top = "20px";
	//this._divEdit.style.left = "110px";
	var sd = args.StartDate;
	var ed = args.EndDate;
	if((ed.getTime()-sd.getTime())/1000/60<=30 && (sd.getHours()!=23 || sd.getMinutes()<30))
	{
		ed = new Date(sd.getTime()+59*60*1000)
	}						
	if(sd.getDate()==ed.getDate() && sd.getMonth()==ed.getMonth() && sd.getFullYear()==ed.getFullYear())
	{
		this._dates.innerHTML = sd.toLocaleDateString()+", "+sd.toLocaleTimeString()+" - "+ed.toLocaleTimeString()
	}
	else
	{
		this._dates.innerHTML = sd.toLocaleDateString()+ ", "+ sd.toLocaleTimeString() + " - " +	ed.toLocaleDateString()+ ", "+ ed.toLocaleTimeString();
	}
	this._itemTitle = "";
	this._itemStartDate = args.StartDate;
	this._itemEndDate = ed;//args.EndDate;
	this._itemDescription = "";
	this._itemIsAllDay = args.IsAllDay;
	this._itemExtensions = null;	
	this._itemsManager = args.ItemsManager;	
	
	if(hostPath==null || hostPath=='undefined')
		this._hlEditEvent.href = "http://localhost/MCAjaxCalendar/EditorPages/GoogleEventEdit.aspx?";	
	else
		this._hlEditEvent.href = hostPath+"EditorPages/GoogleEventEdit.aspx?";	
	
	var sm = sd.getMonth()+1;
	var em = ed.getMonth()+1;
	var s = "StartDate="+ sm +"\/"+sd.getDate()+"\/"+sd.getFullYear()+" "+sd.getHours()+":"+sd.getMinutes();
	var e = "&EndDate="+ em +"\/"+ed.getDate()+"\/"+ed.getFullYear()+" "+ed.getHours()+":"+ed.getMinutes();
	var ad = "&IsAllDay="+this._itemIsAllDay.toString();
	this._hlEditEvent.href+=s+e+ad;
	
}

GoogleEditor.prototype.CreateItem = function()
{
	if(this._editor.style.display=='block')
	{
		if(this._iTitle.value.trim=="")
			this._itemTitle = "&nbsp;"
		else
			this._itemTitle = this._iTitle.value;
		this._itemsManager.CreateItem(this._itemTitle, this._itemStartDate, this._itemEndDate, "&nbsp;", this._itemIsAllDay, null,null);
		this.CloseEditor();
		this._itemUid = null;
		this._itemTitle = null;
		this._itemStartDate = null;
		this._itemEndDate = null;
		this._itemDescription = null;
		this._itemIsAllDay = null;
		this._itemExtensions = null;	
		this._itemsManager = null;
	}	
}


GoogleEditor.prototype.OnItemUpdating = function(sender, args)
{
	if(args.MouseEvent.type=='mouseup' && sender.get_ActionName()=="")
	{
		var dh = 0;
		if(document.documentElement.scrollHeight>document.documentElement.clientHeight)
			dh = document.documentElement.scrollTop;
		this._editor.style.display = "block";
		if(args.MouseEvent.clientY-75>0)
			this._editor.style.top = args.MouseEvent.clientY-75+dh+"px";
		else	
			this._editor.style.top = "0px";
		if(args.MouseEvent.clientX-150>0)	
			this._editor.style.left = args.MouseEvent.clientX-150+"px";
		else
			this._editor.style.left = "0px";
			
		this._divEditor.style.display = "none";
		this._divTitle.style.display = "block";
		this._dates.style.top = "50px";
		this._btnCreate.style.display = "none";
		this._db.style.display = "block";
		this._divSep.style.display = "block";
		//this._divEdit.style.left = "10px";
		if(args.Title.trim()!="&nbsp;")
			this._divTitle.innerHTML = args.Title;
		else
			this._divTitle.innerHTML = "(No Subject)";
		var sd = args.StartDate;
		var ed = args.EndDate;						
		if(sd.getDate()==ed.getDate() && sd.getMonth()==ed.getMonth() && sd.getFullYear()==ed.getFullYear())
		{
			this._dates.innerHTML = sd.toLocaleDateString()+", "+sd.toLocaleTimeString()+" - "+ed.toLocaleTimeString()
		}
		else
		{
			this._dates.innerHTML = sd.toLocaleDateString()+ ", "+ sd.toLocaleTimeString() + " - " +	ed.toLocaleDateString()+ ", "+ ed.toLocaleTimeString();
		}						
		this._itemUid = args.Uid;
		if(hostPath==null || hostPath=='undefined')
		{
			this._hlEditEvent.href = "http://localhost/MCAjaxCalendar/EditorPages/GoogleEventEdit.aspx?EventId=" + args.Uid;
			//this._hlAttendEvent.href = "http://localhost/MCAjaxCalendar/EditorPages/AttendanceSheet.aspx?EventId=" + args.Uid;
		}
		else
		{
			
			this._hlEditEvent.href = hostPath + "EditorPages/GoogleEventEdit.aspx?EventId=" + args.Uid;
			//this._hlAttendEvent.href = hostPath +"EditorPages/AttendanceSheet.aspx?EventId=" + args.Uid;
		}
		this._itemTitle = args.Title;
		this._itemStartDate = args.StartDate;
		this._itemEndDate = args.EndDate;
		this._itemDescription = args.Description;
		this._itemIsAllDay = args.IsAllDay;
		this._itemExtensions = args.Extensions;	
		this._itemsManager = args.ItemsManager;	
	}
	
}

GoogleEditor.prototype.DeleteItem = function()
{
	if(this._editor.style.display=='block')
	{
		this._itemsManager.DeleteItem(this._itemUid,null);
		this.CloseEditor();
		this._itemUid = null;
		this._itemTitle = null;
		this._itemStartDate = null;
		this._itemEndDate = null;
		this._itemDescription = null;
		this._itemIsAllDay = null;
		this._itemExtensions = null;	
		this._itemsManager = null;
	}
}


GoogleEditor.prototype.CloseEditor = function()
{
	if(this._editor && this._editor.style.display=="block")
	{	
		this._editor.style.display="none";
		this._iTitle.value = "";
		this._dates.innerHTML = "";
	}
}

