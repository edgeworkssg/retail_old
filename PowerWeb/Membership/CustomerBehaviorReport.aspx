<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="CustomerBehaviorReport" Title="<%$Resources:dictionary,Customer Purchase Summary Report%>"
    CodeBehind="CustomerBehaviorReport.aspx.cs" %>
<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=700,width=650,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
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
        <%--  <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
       --%>
        <tr>
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
                <asp:Literal ID="lblMembershipGroup" runat="server" Text="<%$Resources:dictionary,Membership Group%>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlMembershipGroup" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Item Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtItemName" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Category %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddCategory" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtMembershipNo" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNameToAppear" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtLastName" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;<asp:Button ID="btnSearch" CssClass="classname" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" CssClass="classname" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td colspan="2" align="right">
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="100%" runat="server" AllowPaging="True" AllowSorting="True"
        OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting" OnPageIndexChanging="gvReport_PageIndexChanging"
        AutoGenerateColumns="False" SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound"
        PageSize="20">
        <Columns>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a id="HyperLink2" href="javascript:poptastic('../Membership/MembershipDetail.aspx?id=<%# Eval("MembershipNo")%>');">
                        <asp:Literal ID="SEARCHLbl2" runat="server" Text="<%$ Resources:dictionary,View %>" /></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="membershipno" HeaderText="<%$ Resources:dictionary,Membership No %>"
                SortExpression="membershipno" />
            <asp:BoundField DataField="nametoappear" SortExpression="nametoappear" HeaderText="<%$ Resources:dictionary,Name %>" />
            <asp:BoundField DataField="GroupName" SortExpression="groupname" HeaderText="<%$ Resources:dictionary,Membership Group %>" />
            <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="<%$ Resources:dictionary,Email %>" />
            <asp:BoundField DataField="mobile" SortExpression="mobile" HeaderText="<%$ Resources:dictionary,mobile %>" />
            <asp:BoundField DataField="SubscriptionDate" SortExpression="SubscriptionDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="<%$ Resources:dictionary,Subscription Date %>" />
            <asp:BoundField DataField="TotalPurchased" SortExpression="TotalPurchased" HeaderText="<%$ Resources:dictionary,Total Purchased %>" />
            <asp:BoundField DataField="TotalItemBought" SortExpression="TotalItemBought" HeaderText="<%$ Resources:dictionary,Total Item Bought %>" />
            <asp:BoundField DataField="NumberOfTransaction" HeaderText="<%$ Resources:dictionary,Number Of Transaction %>"
                SortExpression="NumberOfTransaction" />
            <asp:BoundField DataField="AvgAmountPerTransaction" HeaderText="<%$ Resources:dictionary,Avg Amount Per Transaction %>"
                SortExpression="AvgAmountPerTransaction" />
            <asp:BoundField DataField="AvgAmountPerItem" HeaderText="<%$ Resources:dictionary, Avg Amount Per Item %>"
                SortExpression="AvgAmountPerItem" />
            <asp:BoundField Visible="False" DataField="FirstName" SortExpression="FirstName"
                HeaderText="<%$ Resources:dictionary,First Name %>" />
            <asp:BoundField Visible="False" DataField="LastName" SortExpression="LastName" HeaderText="<%$ Resources:dictionary,Last Name %>" />
            <asp:BoundField Visible="False" DataField="StreetName" SortExpression="StreetName"
                HeaderText="<%$ Resources:dictionary,Address %>" />
            <asp:BoundField Visible="False" DataField="City" SortExpression="City" HeaderText="<%$ Resources:dictionary,City %>" />
            <asp:BoundField Visible="False" DataField="Country" SortExpression="Country" HeaderText="<%$ Resources:dictionary,Country %>" />
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
</asp:Content>
