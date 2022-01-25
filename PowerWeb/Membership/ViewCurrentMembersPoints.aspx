<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="ViewCurrentMembersPoints" Title="<%$Resources:dictionary,View Members Points%>"
    CodeBehind="ViewCurrentMembersPoints.aspx.cs" %>

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

    &nbsp;&nbsp;
    <div style="height: 20px;" class="wl_pageheaderSub">
        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="600px" id="FieldsTable">
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
            <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkIncludeZeroPoints" runat="server" Text="<%$Resources:dictionary,Show completed point package %>" />
            </td>
            <td>
            </td>
            <td>
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
    <asp:GridView ID="gvReport" Width="100%" runat="server" PageSize="25" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a id="HyperLink1" href="javascript:poptastic('../Membership/ViewPointsHistory.aspx?id=<%# Eval("MembershipNo")%>&pointid=<%# Eval("PointID")%>');">
                        <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, View Details%>" />
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a id="HyperLink1" href="javascript:poptastic('../Membership/EditPoints.aspx?id=<%# Eval("MembershipNo")%>&pointid=<%# Eval("PointID")%>');">
                        <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Adjust Points%>" />
                    </a>
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
            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Point/Packages%>"
                SortExpression="PointID" />
            <asp:BoundField DataField="GroupName" HeaderText="<%$Resources:dictionary, Group%>"
                SortExpression="GroupName" />
            <asp:BoundField DataField="NametoAppear" HeaderText="<%$Resources:dictionary, Name%>"
                SortExpression="NameToAppear" />
            <asp:BoundField Visible="False" DataField="FirstName" HeaderText="<%$Resources:dictionary, First Name%>"
                SortExpression="FirstName" />
            <asp:BoundField Visible="False" DataField="LastName" HeaderText="<%$Resources:dictionary, Last Name%>"
                SortExpression="LastName" />
            <asp:BoundField DataField="NRIC" HeaderText="<%$Resources:dictionary, NRIC%>" SortExpression="NRIC" />
            <asp:BoundField DataField="ExpiryDate" HeaderText="<%$Resources:dictionary, Expiry Date%>" SortExpression="ExpiryDate" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="TotalPoints" HeaderText="<%$Resources:dictionary, Total Points%>"
                SortExpression="TotalPoints" />
            <asp:BoundField ItemStyle-Width="1px" DataField="PointType" HeaderText="" SortExpression="PointType" />
            <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="BreakdownPrice" HeaderText="<%$Resources:dictionary, Breakdown Price%>"
                SortExpression="BreakdownPrice" />
            <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="TotalValue" HeaderText="<%$Resources:dictionary, Total Value%>"
                SortExpression="TotalValue" />
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
