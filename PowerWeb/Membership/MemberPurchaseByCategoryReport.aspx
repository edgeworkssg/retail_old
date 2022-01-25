<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="MemberPurchaseByCategoryReport" Title="<%$Resources:dictionary,Customer Purchase By Category Report %>"
    CodeBehind="MemberPurchaseByCategoryReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
    var newwindow;
    function poptastic(url)
    {
	    newwindow=window.open(url,'name','height=700,width=650,resizeable=1,scrollbars=1');
	    if (window.focus) {newwindow.focus()}
    }
    </script>

    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <div style="height: 20px; width: 700px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="700px" id="FilterTable">
        <%-- <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        --%><tr>
            <td style="width: 102px; height: 3px">
            </td>
            <td style="height: 3px">
                <asp:CheckBox ID="cbFilterByDate" runat="server" Text="<%$Resources:dictionary, Filter By Date%>" />
            </td>
            <td style="height: 3px">
            </td>
            <td style="height: 3px">
            </td>
        </tr>
        <tr>
            <td style="width: 102px; height: 3px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td style="height: 3px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                    Width="68px" />
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="122px">
                    <asp:ListItem Value="1" Text="<%$Resources:dictionary,January %>"></asp:ListItem>
                    <asp:ListItem Value="2" Text="<%$Resources:dictionary,February %>"></asp:ListItem>
                    <asp:ListItem Value="3" Text="<%$Resources:dictionary,March %>"></asp:ListItem>
                    <asp:ListItem Value="4" Text="<%$Resources:dictionary,April %>"></asp:ListItem>
                    <asp:ListItem Value="5" Text="<%$Resources:dictionary,May %>"></asp:ListItem>
                    <asp:ListItem Value="6" Text="<%$Resources:dictionary,June %>"></asp:ListItem>
                    <asp:ListItem Value="7" Text="<%$Resources:dictionary,July %>"></asp:ListItem>
                    <asp:ListItem Value="8" Text="<%$Resources:dictionary,August %>"></asp:ListItem>
                    <asp:ListItem Value="9" Text="<%$Resources:dictionary,September %>"></asp:ListItem>
                    <asp:ListItem Value="10" Text="<%$Resources:dictionary,October %>"></asp:ListItem>
                    <asp:ListItem Value="11" Text="<%$Resources:dictionary,November %>"></asp:ListItem>
                    <asp:ListItem Value="12" Text="<%$Resources:dictionary,December %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblYear" runat="server"></asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
            </td>
            <td align="right" colspan="2">
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td colspan="4" style="height: 451px">
                <asp:GridView ID="gvReport" ShowFooter="True" Width="800px" runat="server" AllowPaging="True"
                    AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
                    OnPageIndexChanging="gvReport_PageIndexChanging" SkinID="scaffold" PageSize="20"
                    OnRowDataBound="gvReport_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <a id="HyperLink2" href="javascript:poptastic('../Membership/MembershipDetail.aspx?id=<%# Eval("MembershipNo")%>');">
                                    <asp:Literal ID="SEARCHLbl2" runat="server" Text="<%$ Resources:dictionary,View %>" /></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <div style="border-top: 1px solid #666666">
                            <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                                CommandArgument="First" CommandName="Page" />
                            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                                CommandArgument="Prev" CommandName="Page" />
                            <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                            <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                                ID="lblPageCount" runat="server"></asp:Label>
                            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                                CommandArgument="Next" CommandName="Page" />
                            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                                CommandArgument="Last" CommandName="Page" />
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
