<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="MembershipReport" Title="<%$Resources:dictionary,Membership Search %>"
    CodeBehind="MembershipReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartExpiryDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndExpiryDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton3" TargetControlID="txtStartBirthDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton4" TargetControlID="txtEndBirthDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton5" TargetControlID="txtStartSubsDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton6" TargetControlID="txtEndSubsDate">
    </cc1:CalendarExtender>
    <div style="height: 20px; width: 650px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="650px" id="FilterTable">
        <%-- <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
       --%>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Start Subscription Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStartSubsDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartSubsDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Subscription Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndSubsDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndSubsDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <tr>
            <td>
                Start Expiry Date
            </td>
            <td>
                <asp:TextBox ID="txtStartExpiryDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartExpiryDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
            </td>
            <td>
                End Expiry Date
            </td>
            <td>
                <asp:TextBox ID="txtEndExpiryDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndExpiryDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Start Birth Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStartBirthDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartBirthDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,End Birth Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndBirthDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndBirthDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary, Birthday Month %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="120px">
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
                <br />
                <asp:CheckBox ID="cbUseBirthDayMonth" runat="server" Text="<%$ Resources:dictionary, Use Birthday Month %>" />
            </td>
            <td>
                <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Gender %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlGender" runat="server">
                    <asp:ListItem Value="">ALL</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                    <asp:ListItem Value="F">Female</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="From Card No"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtFromMembershipNo" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal16" runat="server" Text="To Card No"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtToMembershipNo" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,NRIC %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNRIC" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNameToAppear" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 15px">
                <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Card Type %>"></asp:Literal>
            </td>
            <td style="height: 15px">
                <subsonic:DropDown ID="ddGroupName" runat="server" OrderField="GroupName" PromptValue="0"
                    ShowPrompt="True" TableName="MembershipGroup" TextField="GroupName" ValueField="MembershipGroupID"
                    Width="175px">
                </subsonic:DropDown>
            </td>
            <td>
                <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Address %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStreetName" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 15px">
                <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:dictionary,Mobile %>"></asp:Literal>
            </td>
            <td style="height: 15px">
                <asp:TextBox ID="txtMobileNo" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal18" runat="server" Text="<%$Resources:dictionary,Home %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtHomeNo" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 15px">
                Staff
            </td>
            <td style="height: 15px">
                <asp:DropDownList ID="ddlStylist" runat="server" Width="170px">
                </asp:DropDownList>
            </td>
            <td style="height: 15px">
                <asp:Literal ID="Literal7" runat="server" Text="Email"></asp:Literal>
            </td>
            <td style="height: 15px">
                <asp:TextBox ID="txtEmail" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="650px">
        <tr>
            <td colspan="2" style="height: 15px">
                &nbsp;<asp:Button ID="btnSearch" class="classname" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" class="classname" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td style="width: 50%;">
            </td>
            <td align="right" style="height: 30px;">
                <asp:LinkButton ID="lnkExport" runat="server" class="classBlue" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" DataKeyNames="MembershipNo" PageSize="20" SkinID="scaffold"
        OnDataBound="gvReport_DataBound" OnPageIndexChanging="gvReport_PageIndexChanging"
        OnRowDataBound="gvReport_RowDataBound" OnSorting="gvReport_Sorting" Width="800px">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="MembershipNo" DataNavigateUrlFormatString="../Scaffolds/MembershipScaffold.aspx?id={0}
            " Text="<%$Resources:dictionary,Edit %>" />
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
            <asp:BoundField DataField="SalesPersonID" HeaderText="Staff" SortExpression="SalesPersonID" />
            <asp:BoundField Visible="false" DataField="IsVitaMix" HeaderText="<%$Resources:dictionary,Vita Mix Customer %>"
                SortExpression="IsVitaMix" />
            <asp:BoundField Visible="false" DataField="IsWaterFilter" HeaderText="<%$Resources:dictionary,Water Filter Customer %>"
                SortExpression="IsWaterFilter" />
            <asp:BoundField Visible="false" DataField="IsYoung" HeaderText="<%$Resources:dictionary,Young Customer %>"
                SortExpression="IsYoung" />
            <asp:BoundField Visible="false" DataField="IsJuicePlus" HeaderText="<%$Resources:dictionary,Juice Plus Customer %>"
                SortExpression="IsJuicePlus" />
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
                SortExpression="DateOfBirth" />
            <asp:BoundField DataField="Occupation" HeaderText="<%$Resources:dictionary,Occupation %>"
                SortExpression="Occupation" />
            <asp:BoundField DataField="remarks" HeaderText="<%$Resources:dictionary,Remark  %>"
                SortExpression="remarks" />
            <asp:BoundField DataField="SubscriptionDate" HeaderText="Subscription Date" SortExpression="SubscriptionDate"
                DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate"
                DataFormatString="{0:dd MMM yyyy}" />
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
        <EmptyDataTemplate>
            <asp:Literal ID="literal4656" runat="server" Text="<%$Resources:dictionary,No Membership %>"></asp:Literal>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
