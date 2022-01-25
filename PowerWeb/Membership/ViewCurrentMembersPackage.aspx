<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="ViewCurrentMembersPackage" Title="<%$Resources:dictionary, View Members Package%>"
    CodeBehind="ViewCurrentMembersPackage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager2" runat="server" />
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate" />

    <script>
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=700,width=650,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }
    </script>

    <%--<ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>--%>
    &nbsp;&nbsp;
    <div style="height: 20px;" class="wl_pageheaderSub">
        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="600px" id="FieldsTable">
        <tr>
            <td>
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$Resources:dictionary,End Date %>" />
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtMembershipNo" runat="server" Width="170px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Membership Group %>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="ddGroupName" runat="server" OnInit="ddPOS_Init" PromptText="ALL"
                    ShowPrompt="True" TableName="MembershipGroup" TextField="GroupName" ValueField="MembershipGroupID"
                    Width="170px">
                </subsonic:DropDown>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNameToAppear" runat="server" Width="170px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,NRIC %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNRIC" runat="server" Width="170px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtLastName" runat="server" Width="170px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server" Width="170px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Point of Sale %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="170px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="ddPOS" runat="server" OnInit="ddPOS_Init" PromptText="ALL"
                    ShowPrompt="True" TableName="PointOfSale" TextField="PointOfSaleName" ValueField="PointOfSaleID"
                    Width="170px">
                </subsonic:DropDown>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px">
                &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td colspan="2" align="right">
                <asp:LinkButton ID="lnkExport" runat="server" class="classBlue" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" ShowFooter="True" Width="100%" runat="server" PageSize="25"
        AllowPaging="True" AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a id="HyperLink1" href="javascript:poptastic('../Membership/EditPoints.aspx?id=<%# Eval("MembershipNo")%>&pointid=<%# Eval("PointID")%>');">
                        <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Adjust Points%>" /> </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField Visible="False" DataField="MembershipNo" HeaderText="<%$Resources:dictionary, Membership No%>"
                SortExpression="MembershipNo" />
            <asp:TemplateField HeaderText="<%$Resources:dictionary, Membership No%>">
                <ItemTemplate>
                    <a id="HyperLink1" href="javascript:poptastic('../Membership/MembershipDetail.aspx?id=<%# Eval("MembershipNo")%>');">
                        <%# Eval("MembershipNo")%>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Package%>" SortExpression="PointID" />
            <asp:BoundField DataField="GroupName" HeaderText="<%$Resources:dictionary, Group%>" SortExpression="GroupName" />
            <asp:BoundField DataField="NametoAppear" HeaderText="<%$Resources:dictionary, Name%>" SortExpression="NameToAppear" />
            <asp:BoundField Visible="False" DataField="FirstName" HeaderText="<%$Resources:dictionary, First Name%>" SortExpression="FirstName" />
            <asp:BoundField Visible="False" DataField="LastName" HeaderText="<%$Resources:dictionary, Last Name%>" SortExpression="LastName" />
            <asp:BoundField DataField="NRIC" HeaderText="<%$Resources:dictionary, NRIC%>" SortExpression="NRIC" />
            <asp:BoundField DataField="TotalPoints" HeaderText="<%$Resources:dictionary, Remaining Time%>" SortExpression="TotalPoints" />
            <asp:BoundField ItemStyle-Width="1px" DataField="PointType" HeaderText="" SortExpression="PointType"
                Visible="false" />
            <asp:BoundField DataField="BreakdownPrice" HeaderText="<%$Resources:dictionary, Breakdown Price%>" SortExpression="BreakdownPrice" />
            <asp:BoundField DataField="TotalValue" HeaderText="<%$Resources:dictionary, Package Value%>" SortExpression="TotalValue" />
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
