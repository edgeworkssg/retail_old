<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="AccountStatementReport" Title="Account Statement Report" Codebehind="AccountStatementReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" src="../Scripts/jquery-1.8.3.min.js"></script>
    <script>
        var _interval;
        $(document).ready(function() {
            _interval = setInterval("refreshPage()", 180000);
        });
    
        var newwindow;
        function poptastic(url)
        {
	        newwindow=window.open(url,'name','height=700,width=900,resizeable=1,scrollbars=1');
	        if (window.focus) {newwindow.focus()}
	    }

	    function refreshPage() {
	        clearInterval(_interval);
	        $('#ctl00_ContentPlaceHolder1_btnSearch').click();
	        //location.reload(true)
	    }
	    
    </script>
    
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    
    <table style="width: 800px" id="FieldsTable">
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        <tr>
            <td  style="width: 130px; height: 3px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" /></td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
            <td  style="height: 3px">
                <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal></td>
            <td style="height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
        </tr>      
        <tr>
            <td  style="width: 130px">
                <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                    Width="68px" /></td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" >
                    <asp:ListItem Value="1">January</asp:ListItem>
                    <asp:ListItem Value="2">February</asp:ListItem>
                    <asp:ListItem Value="3">March</asp:ListItem>
                    <asp:ListItem Value="4">April</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="6">June</asp:ListItem>
                    <asp:ListItem Value="7">July</asp:ListItem>
                    <asp:ListItem Value="8">August</asp:ListItem>
                    <asp:ListItem Value="9">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddYear" runat="server" Width="70px">
                
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr><td  style="width: 102px"> <asp:Literal ID = "Literal2" runat="server" Text="Search"></asp:Literal></td><td><asp:TextBox ID="txtSearch" runat="server" Width="172px"></asp:TextBox></td>
        <td></td><td></td></tr>
        <%--<tr><td colspan=2 style="height: 15px" >
            &nbsp;<asp:Button ID="btnSearch" class="classname" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear"  class="classname" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td align="right"  >
                <asp:LinkButton ID="lnkExport" class="classBlue"  runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>--%>
    </table>
        <table width="800px">
         <tr><td colspan=2 style="height: 15px" >
            &nbsp;<asp:Button ID="Button1" class="classname" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="Button2"  class="classname" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td align="right"  >
                <asp:LinkButton ID="LinkButton1" class="classBlue"  runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>
        </table>
    <asp:GridView ID="gvReport" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" DataKeyNames="MembershipNo" 
        PageSize="20" SkinID="scaffold" OnDataBound="gvReport_DataBound" 
        OnPageIndexChanging="gvReport_PageIndexChanging" 
        OnRowDataBound="gvReport_RowDataBound" OnSorting="gvReport_Sorting" 
        Width="800px" 
        onrowcommand="gvReport_RowCommand" 
        onselectedindexchanged="gvReport_SelectedIndexChanged" >
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <a ID="HyperLink1"
                     href="javascript:poptastic('AccountStatementDownload.aspx?id=<%# Eval("Membershipno")%>&startdate=<%# txtStartDate.Text %>&enddate=<%# txtEndDate.Text %>');">
                     <asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,View %>"></asp:Literal></a>
                </ItemTemplate>
            </asp:TemplateField>
        
            <asp:TemplateField>
                <ItemTemplate>
                    <a ID="HyperLink2"
                     href="javascript:poptastic('AccountStatementDownload.aspx?id=<%# Eval("Membershipno")%>&startdate=<%# txtStartDate.Text %>&enddate=<%# txtEndDate.Text %>&download=1');">
                     <asp:Literal ID = "SEARCHLbl2" runat="server" Text="Download"></asp:Literal></a>
                </ItemTemplate>            
            </asp:TemplateField>
            <asp:BoundField DataField="MembershipNo" HeaderText="Membership No" 
                SortExpression="MembershipNo" />
            <asp:BoundField DataField="NameToAppear" HeaderText="Name" 
                SortExpression="NameToAppear" />                                                            
            <asp:BoundField DataField="Home" HeaderText="Home" SortExpression="Home" />
            <asp:BoundField DataField="Mobile" HeaderText="Mobile" 
                SortExpression="Mobile" /> 
            <asp:BoundField DataField="OpeningBalance" HeaderText="Opening Balance" 
                SortExpression="Credit" />      
            <asp:BoundField DataField="Debit" HeaderText="Debit" SortExpression="Debit" />                             
            <asp:BoundField DataField="Credit" HeaderText="Credit" 
                SortExpression="Credit" />
            
            <asp:BoundField DataField="Balance" HeaderText="Balance" 
                SortExpression="Balance" />
        </Columns>
         <PagerTemplate>
                        <div style="border-top:1px solid #666666">            
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
        <EmptyDataTemplate>
            <asp:Literal ID = "literal4656" runat="server" Text="<%$Resources:dictionary,No Membership %>"></asp:Literal>
        </EmptyDataTemplate>
        <SelectedRowStyle ForeColor="#000066" />
    </asp:GridView>
</asp:Content>

