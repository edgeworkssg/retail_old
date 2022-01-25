<%@ Page Title="<%$Resources:dictionary,Campaign Promo Report %>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="CampaignPromoReport.aspx.cs" Inherits="PowerWeb.Reports.CampaignPromoReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FilterTable">
        <tr style="height: 20px;">
            <td colspan="4" class="wl_pageheaderSub" style="height: 20px;">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td style="width: 200px">
                <asp:TextBox ID="txtStartDate" runat="server">
                </asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server">
                </asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButton ID="rdbYear" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Year %>"
                    Width="68px" />
            </td>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlYear" runat="server" Width="70px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 600px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 25px;">
                <asp:Literal ID="litMessage" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="100%" ShowFooter="true" runat="server" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <a id="hyperLink1" href="javascript:poptastic('CampaignReportDetail.aspx?PromoCode=<%# Eval("PromoCampaignHdrID")%>');">
                        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,View %>"></asp:Literal></a></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PromoCode" SortExpression="PromoCode" HeaderText="<%$Resources:dictionary, Promo Code%>" />
            <asp:BoundField DataField="PromoCampaignName" SortExpression="PromoCampaignName"
                HeaderText="<%$Resources:dictionary,Promo Name %>" />
            <asp:BoundField DataField="PromoUsedWithinPeriod" SortExpression="PromoUsedWithinPeriod"
                ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:dictionary, Promo Used Within Period%>" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="TotalSold" SortExpression="TotalSold" HeaderText="<%$Resources:dictionary, Total Sold Within Period%>"
                DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="DateFrom" SortExpression="DateFrom" HeaderText="<%$Resources:dictionary, Promo Date From%>"
                DataFormatString="{0:dd MMM yyyy hh:mm tt}" />
            <asp:BoundField DataField="DateTo" SortExpression="DateTo" HeaderText="<%$Resources:dictionary, Promo Date To%>"
                DataFormatString="{0:dd MMM yyyy hh:mm tt}" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary,Outlet %>" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList
                    ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                    ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server"
                        CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next"
                        CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                    CommandArgument="Last" CommandName="Page" Visible="false" />
            </div>
        </PagerTemplate>
        <EmptyDataTemplate>
            <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, (No Data Found)%>" /></EmptyDataTemplate>
    </asp:GridView>

    <script type="text/javascript">
        var newwindow;
        function poptastic(url) {
            var e = document.getElementById("<%=ddlOutlet.ClientID%>");
            var strOutlet = e.options[e.selectedIndex].value;

            var startdate = document.getElementById("<%=txtStartDate.ClientID%>").value
            var enddate = document.getElementById("<%=txtEndDate.ClientID%>").value
            var search = document.getElementById("<%=txtSearch.ClientID%>").value

            url = url + "&StartDate=" + startdate + "&EndDate=" + enddate + "&Outlet=" + strOutlet + "&Search=" + search;

            newwindow = window.open(url, 'name', 'height=1000,width=850,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }
    </script>

</asp:Content>
