<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="MembershipAttendanceReport" Title="<%$Resources:dictionary,Membership Attendance Report %>"
    CodeBehind="MembershipAttendanceReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <div style="height: 20px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="600px" id="FieldsTable">
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtMembershipNo" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNameToAppear" runat="server" Width="173px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddPOS" runat="server" Width="179px"></asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px">
                &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td colspan="2" align="right" valign="middle">
                <asp:LinkButton ID="lnkExport" runat="server" class="classBlue" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="800px" runat="server" ShowFooter="True" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        PageSize="20" OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField DataField="AttendanceDate" HeaderText="Date" SortExpression="AttendanceDate" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="LoginTime" HeaderText="Login Time" SortExpression="LoginTime" DataFormatString="{0:HH:mm:ss}" />
            <asp:BoundField DataField="LogoutTime" HeaderText="Logout Time" SortExpression="LogoutTime" DataFormatString="{0:HH:mm:ss}" />
            <asp:BoundField DataField="Duration" HeaderText="Duration" SortExpression="Duration" ItemStyle-Wrap="false"   />
            <asp:BoundField DataField="MembershipNo" HeaderText="<%$Resources:dictionary,No%>"
                SortExpression="MembershipNo" />
            <asp:BoundField DataField="GroupName" HeaderText="<%$Resources:dictionary,Card Type %> "
                SortExpression="GroupName" />
            <asp:BoundField DataField="NameToAppear" HeaderText="<%$Resources:dictionary,Name %>"
                SortExpression="NameToAppear" />
            <asp:BoundField DataField="Mobile" HeaderText="<%$Resources:dictionary,Mobile %>"
                SortExpression="Mobile" />
            <asp:BoundField DataField="Home" HeaderText="<%$Resources:dictionary,Home %>" SortExpression="Home" />
            <asp:BoundField DataField="StreetName" HeaderText="<%$Resources:dictionary,Address1 %>"
                SortExpression="StreetName" />
            <asp:BoundField DataField="StreetName2" HeaderText="<%$Resources:dictionary,Address2 %>"
                SortExpression="StreetName2" />
            <asp:BoundField DataField="ZipCode" HeaderText="<%$Resources:dictionary,Postal Code %>"
                SortExpression="ZipCode" />
            <asp:BoundField DataField="NRIC" HeaderText="<%$Resources:dictionary,NRIC %>" SortExpression="NRIC" />
            <%--<asp:BoundField Visible="false" DataField="IsVitaMix" HeaderText="<%$Resources:dictionary,Vita Mix Customer %>"
                SortExpression="IsVitaMix" />
            <asp:BoundField Visible="false" DataField="IsWaterFilter" HeaderText="<%$Resources:dictionary,Water Filter Customer %>"
                SortExpression="IsWaterFilter" />
            <asp:BoundField Visible="false" DataField="IsYoung" HeaderText="<%$Resources:dictionary,Young Customer %>"
                SortExpression="IsYoung" />
            <asp:BoundField Visible="false" DataField="IsJuicePlus" HeaderText="<%$Resources:dictionary,Juice Plus Customer %>"
                SortExpression="IsJuicePlus" />--%>
            <asp:BoundField DataField="email" HeaderText="<%$Resources:dictionary,email %>" SortExpression="email" />
            <asp:BoundField DataField="ChineseName" HeaderText="<%$Resources:dictionary,Chinese Name %>"
                SortExpression="ChineseName" />
            <asp:BoundField DataField="FirstName" HeaderText="<%$Resources:dictionary,First Name %>"
                SortExpression="FirstName" />
            <asp:BoundField DataField="LastName" HeaderText="<%$Resources:dictionary,Last Name %>"
                SortExpression="LastName" />
            <asp:BoundField DataField="ChristianName" HeaderText="<%$Resources:dictionary,Christian Name %>"
                SortExpression="ChristianName" />
            <asp:BoundField DataField="DateOfBirth" HeaderText="<%$Resources:dictionary,Date of Birth %>"
                SortExpression="DateOfBirth" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="Occupation" HeaderText="<%$Resources:dictionary,Occupation %>"
                SortExpression="Occupation" />
            <asp:BoundField DataField="remarks" HeaderText="<%$Resources:dictionary,Remark  %>"
                SortExpression="remarks" />
            <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary,Deleted %>"
                SortExpression="Deleted" />
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
