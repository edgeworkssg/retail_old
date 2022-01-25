<%@ Page Language="C#" Title="<%$Resources:dictionary,Membership Setup %>" Inherits="PowerPOS.MembershipScaffold"
    MasterPageFile="~/PowerPOSMst.master" Theme="default" CodeBehind="MembershipScaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager2" runat="server">
    </ajax:ScriptManager>
    <asp:Panel ID="pnlGrid" runat="server" Width="800px">
        <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton1" TargetControlID="txtEndExpiryDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton2" TargetControlID="txtStartBirthDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton3" TargetControlID="txtStartBirthDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton4" TargetControlID="txtStartExpiryDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton5" TargetControlID="txtStartSubsDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton6" TargetControlID="txtEndSubsDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton7" TargetControlID="txtEndBirthDate">
        </cc1:CalendarExtender>
        <div style="height: 20px; width: 650px;" class="wl_pageheaderSub">
            <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
        </div>
        <table width="650px" id="FilterTable">
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Start Subscription Date %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtStartSubsDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                    &nbsp;<asp:CheckBox ID="cbUseStartSubsDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
                </td>
                <td>
                    <asp:Literal ID="Literal25" runat="server" Text="<%$Resources:dictionary,End Subscription Date %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEndSubsDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                    &nbsp;<asp:CheckBox ID="cbUseEndSubsDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Start Expiry Date%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtStartExpiryDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                    &nbsp;<asp:CheckBox ID="cbUseStartExpiryDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
                </td>
                <td>
                    <asp:Literal ID="Literal46" runat="server" Text="<%$Resources:dictionary, End Expiry Date %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtEndExpiryDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                    &nbsp;<asp:CheckBox ID="cbUseEndExpiryDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal26" runat="server" Text="<%$Resources:dictionary,Start Birth Date %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtStartBirthDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                    &nbsp;<asp:CheckBox ID="cbUseStartBirthDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
                </td>
                <td>
                    <asp:Literal ID="Literal27" runat="server" Text="<%$Resources:dictionary,End Birth Date %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEndBirthDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                    &nbsp;<asp:CheckBox ID="cbUseEndBirthDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal29" runat="server" Text="<%$Resources:dictionary, Birthday Month %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Width="120px">
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
                    <br />
                    <asp:CheckBox ID="cbUseBirthDayMonth" runat="server" Text="<%$ Resources:dictionary, Use Birthday Month %>" />
                </td>
                <td>
                    <asp:Literal ID="Literal31" runat="server" Text="<%$Resources:dictionary,Gender %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlGender" runat="server">
                        <asp:ListItem Value="">ALL</asp:ListItem>
                        <asp:ListItem Value="M" Text="<%$Resources:dictionary, Male%>"></asp:ListItem>
                        <asp:ListItem Value="F" Text="<%$Resources:dictionary, Female%>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal32" runat="server" Text="<%$Resources:dictionary, From Card No%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFromMembershipNo" runat="server" Width="172px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal38" runat="server" Text="<%$Resources:dictionary, To Card No%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtToMembershipNo" runat="server" Width="172px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal39" runat="server" Text="<%$Resources:dictionary,NRIC %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtNRIC" runat="server" Width="172px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal40" runat="server" Text="<%$Resources:dictionary,Name %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtNameToAppear" runat="server" Width="172px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 15px">
                    <asp:Literal ID="Literal41" runat="server" Text="<%$Resources:dictionary,Card Type %>"></asp:Literal>
                </td>
                <td style="height: 15px">
                    <subsonic:DropDown ID="ddGroupName" runat="server" OrderField="GroupName" PromptValue="0"
                        ShowPrompt="True" TableName="MembershipGroup" TextField="GroupName" ValueField="MembershipGroupID"
                        Width="175px">
                    </subsonic:DropDown>
                </td>
                <td>
                    <asp:Literal ID="Literal42" runat="server" Text="<%$Resources:dictionary,Address %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtStreetName" runat="server" Width="172px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 15px">
                    <asp:Literal ID="Literal43" runat="server" Text="<%$Resources:dictionary,Mobile %>"></asp:Literal>
                </td>
                <td style="height: 15px">
                    <asp:TextBox ID="txtMobileNo" runat="server" Width="172px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="Literal44" runat="server" Text="<%$Resources:dictionary,Home %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtHomeNo" runat="server" Width="172px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 15px">
                    <asp:Literal ID="Literal47" runat="server" Text="<%$Resources:dictionary, Staff%>" />
                </td>
                <td style="height: 15px">
                    <asp:DropDownList ID="ddlStylist" runat="server" Width="170px">
                    </asp:DropDownList>
                </td>
                <td style="height: 15px">
                    <asp:Literal ID="Literal45" runat="server" Text="<%$Resources:dictionary, Email%>"></asp:Literal>
                </td>
                <td style="height: 15px">
                    <asp:TextBox ID="txtEmail" runat="server" Width="172px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="650px">
            <tr>
                <td style="height: 15px">
                    &nbsp;<asp:Button ID="btnSearch" class="classname" runat="server" Text="<%$ Resources:dictionary, Search %>"
                        OnClick="btnSearch_Click" />
                    <asp:Button ID="btnClear" runat="server" class="classname" Text="<%$ Resources:dictionary, Clear %>"
                        OnClick="btnClear_Click" />&nbsp;
                    <input id="btnAddNew" runat="server" type="button" onclick="location.href='MembershipScaffold.aspx?id=0'"
                        class="classname" value="<%$Resources:dictionary, Add New%>" style="width: 87px" />
                </td>
                <td style="width: 50%;">
                </td>
                <td align="right" style="height: 30px;">
                    <asp:LinkButton ID="lnkExport" runat="server" class="classBlue" OnClick="lnkExport_Click"
                        Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="MembershipNo" PageSize="20" SkinID="scaffold"
            OnDataBound="GridView1_DataBound" OnPageIndexChanging="GridView1_PageIndexChanging"
            OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting" Width="800px">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="MembershipNo" DataNavigateUrlFormatString="MembershipScaffold.aspx?id={0}"
                    Text="<%$Resources:dictionary,Edit %>" />
                <asp:HyperLinkField DataNavigateUrlFields="MembershipNo" DataNavigateUrlFormatString="AttachedParticularScaffold.aspx?membershipno={0}"
                    Text="<%$Resources:dictionary, Attached Particular%>" />
                <asp:BoundField DataField="MembershipNo" HeaderText="<%$Resources:dictionary,No%>"
                    SortExpression="MembershipNo" />
                <asp:BoundField DataField="GroupName" HeaderText="<%$Resources:dictionary,Card Type %> "
                    SortExpression="GroupName" />
                <asp:BoundField DataField="NameToAppear" HeaderText="<%$Resources:dictionary,Name %>"
                    SortExpression="NameToAppear" />
                <asp:BoundField DataField="ChineseName" HeaderText="<%$Resources:dictionary,Chinese Name %>"
                    SortExpression="ChineseName" />
                <asp:BoundField DataField="FirstName" HeaderText="<%$Resources:dictionary,First Name %>"
                    SortExpression="FirstName" />
                <asp:BoundField DataField="LastName" HeaderText="<%$Resources:dictionary,Last Name %>"
                    SortExpression="LastName" />
                <asp:BoundField DataField="ChristianName" HeaderText="<%$Resources:dictionary,Christian Name %>"
                    SortExpression="ChristianName" />
                <asp:BoundField DataField="Gender" HeaderText="<%$Resources:dictionary,Gender %>"
                    SortExpression="Gender" />
                <asp:BoundField DataField="SalesPersonID" HeaderText="<%$Resources:dictionary, Staff%>"
                    SortExpression="SalesPersonID" />
                <asp:BoundField DataField="DateOfBirth" HeaderText="<%$Resources:dictionary,DOB %>"
                    DataFormatString="{0:dd MMM yyyy}" SortExpression="DateOfBirth" />
                <asp:BoundField DataField="Email" HeaderText="<%$Resources:dictionary,Email %>" SortExpression="Email" />
                <asp:BoundField DataField="NRIC" HeaderText="<%$Resources:dictionary,NRIC %>" SortExpression="NRIC" />
                <asp:BoundField DataField="StreetName" HeaderText="<%$Resources:dictionary,Address 1 %>"
                    SortExpression="StreetName" />
                <asp:BoundField DataField="StreetName2" HeaderText="<%$Resources:dictionary,Address 2 %>"
                    SortExpression="StreetName2" />
                <asp:BoundField DataField="ZipCode" HeaderText="<%$Resources:dictionary,Zip Code %>"
                    SortExpression="ZipCode" />
                <asp:BoundField DataField="City" HeaderText="<%$Resources:dictionary,City %>" SortExpression="City" />
                <asp:BoundField DataField="Country" HeaderText="<%$Resources:dictionary,Country %>"
                    SortExpression="Country" />
                <asp:BoundField DataField="Nationality" HeaderText="<%$Resources:dictionary,Nationality %>"
                    SortExpression="Nationality" />
                <asp:BoundField DataField="Mobile" HeaderText="<%$Resources:dictionary,Mobile %>"
                    SortExpression="Mobile" />
                <asp:BoundField DataField="Office" HeaderText="<%$Resources:dictionary,Fax %>" SortExpression="Office"
                    Visible="False" />
                <asp:BoundField DataField="Home" HeaderText="<%$Resources:dictionary,Home %>" SortExpression="Home" />
                <asp:BoundField DataField="Occupation" HeaderText="<%$Resources:dictionary,Occupation %>"
                    SortExpression="Occupation" />
                <asp:BoundField DataField="SubscriptionDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="<%$Resources:dictionary,Subscription Date %>"
                    SortExpression="SubscriptionDate" />
                <asp:BoundField DataField="ExpiryDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="<%$Resources:dictionary,Expiry Date %>"
                    SortExpression="ExpiryDate" />
                <asp:BoundField DataField="Remarks" HeaderText="<%$Resources:dictionary,Remarks %>"
                    SortExpression="Remarks" />
                <asp:BoundField DataField="userfld1" HeaderText="<%$Resources:dictionary, Child 1 Name%>"
                    SortExpression="userfld1" Visible="False" />
                <asp:BoundField DataField="userfld2" HeaderText="<%$Resources:dictionary, Child 2 Name%>"
                    SortExpression="userfld2" Visible="False" />
                <asp:BoundField DataField="userfld3" HeaderText="<%$Resources:dictionary, Child 3 Name%>"
                    SortExpression="userfld3" Visible="False" />
                <asp:BoundField DataField="userfld4" HeaderText="<%$Resources:dictionary, Child 4 Name%>"
                    SortExpression="userfld4" Visible="False" />
                <asp:BoundField DataField="userfld5" SortExpression="userfld5" Visible="False" />
                <asp:BoundField DataField="userfld6" SortExpression="userfld6" Visible="False" />
                <asp:BoundField DataField="userfld7" SortExpression="userfld7" Visible="False" />
                <asp:BoundField DataField="userfld8" SortExpression="userfld8" Visible="False" />
                <asp:BoundField DataField="userfld9" SortExpression="userfld7" Visible="False" />
                <asp:BoundField DataField="userfld10" SortExpression="userfld8" Visible="False" />
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
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" MinimumPrefixLength="1"
            CompletionSetCount="1" ServicePath="../Synchronization/Synchronization.asmx"
            ServiceMethod="GetNameList" TargetControlID="txtChristianName">
        </cc1:AutoCompleteExtender>
        <cc1:CalendarExtender ID="cldDOB" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton8" TargetControlID="ctrlDateOfBirth">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="cldExpiryDate" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton9" TargetControlID="ctrlExpiryDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="cldSubscriptionDate" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton10" TargetControlID="txtSubscriptionDate">
        </cc1:CalendarExtender>
        <asp:Label ID="lblResult" runat="server" CssClass="LabelMessage"></asp:Label>&nbsp;
        <table cellpadding="5" cellspacing="0" id="FieldsTable" width="600px">
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlMembershipNo" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal28" runat="server" Text="<%$Resources:dictionary,Group %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ctrlMembershipGroupId" runat="server" Width="153px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Expiry Date %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlExpiryDate" runat="server" Width="121px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Subscription Date %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSubscriptionDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                </td>
            </tr>
        </table>
        <table width="700px">
            <tr style="height: 22px;">
                <td class="wl_pageheaderSubSub" style="text-align: center; height: 22px;">
                    <asp:Literal ID="Literal30" runat="server" Text="<%$Resources:dictionary,Personal Information %>"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="700px" id="FieldsTable1">
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlNameToAppear" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ctrlNameToAppear"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,NRIC/FIN %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlNRIC" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlFirstName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlLastName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal35" runat="server" Text="<%$Resources:dictionary,Chinese Name %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtChineseName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal36" runat="server" Text="<%$Resources:dictionary,Christian Name %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtChristianName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Gender %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:DropDownList ID="ctrlGender" runat="server">
                        <asp:ListItem Text="<%$Resources:dictionary,Male %>" Value="M"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:dictionary,Female %>" Value="F"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Date Of Birth  %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlDateOfBirth" runat="server" Width="118px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                        Style="height: 16px; width: 16px" />
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Occupation %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlOccupation" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal49" runat="server" Text="<%$Resources:dictionary, Staff%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ctrlStylist" runat="server" Width="153px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal50" runat="server" Text="<%$Resources:dictionary,Nationality %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <select id="ctrlNationality" runat="server">
                        <option value="Afghan">Afghan</option>
                        <option value="Albanian">Albanian</option>
                        <option value="Algerian">Algerian</option>
                        <option value="American">American</option>
                        <option value="Andorran">Andorran</option>
                        <option value="Angolan">Angolan</option>
                        <option value="Antiguans">Antiguans</option>
                        <option value="Argentinean">Argentinean</option>
                        <option value="Armenian">Armenian</option>
                        <option value="Australian">Australian</option>
                        <option value="austrian">Austrian</option>
                        <option value="Azerbaijani">Azerbaijani</option>
                        <option value="Bahamian">Bahamian</option>
                        <option value="Bahraini">Bahraini</option>
                        <option value="Bangladeshi">Bangladeshi</option>
                        <option value="Barbadian">Barbadian</option>
                        <option value="Barbudans">Barbudans</option>
                        <option value="Batswana">Batswana</option>
                        <option value="Belarusian">Belarusian</option>
                        <option value="Belgian">Belgian</option>
                        <option value="Belizean">Belizean</option>
                        <option value="Beninese">Beninese</option>
                        <option value="Bhutanese">Bhutanese</option>
                        <option value="Bolivian">Bolivian</option>
                        <option value="Bosnian">Bosnian</option>
                        <option value="Brazilian">Brazilian</option>
                        <option value="British">British</option>
                        <option value="Bruneian">Bruneian</option>
                        <option value="Bulgarian">Bulgarian</option>
                        <option value="Burkinabe">Burkinabe</option>
                        <option value="Burmese">Burmese</option>
                        <option value="Burundian">Burundian</option>
                        <option value="Cambodian">Cambodian</option>
                        <option value="Cameroonian">Cameroonian</option>
                        <option value="Canadian">Canadian</option>
                        <option value="Cape Verdean">Cape Verdean</option>
                        <option value="Central African">Central African</option>
                        <option value="Chadian">Chadian</option>
                        <option value="Chilean">Chilean</option>
                        <option value="Chinese">Chinese</option>
                        <option value="Colombian">Colombian</option>
                        <option value="Comoran">Comoran</option>
                        <option value="Congolese">Congolese</option>
                        <option value="Costa Rican">Costa Rican</option>
                        <option value="Croatian">Croatian</option>
                        <option value="Cuban">Cuban</option>
                        <option value="Cypriot">Cypriot</option>
                        <option value="Czech">Czech</option>
                        <option value="Danish">Danish</option>
                        <option value="Djibouti">Djibouti</option>
                        <option value="Dominican">Dominican</option>
                        <option value="Dutch">Dutch</option>
                        <option value="East Timorese">East Timorese</option>
                        <option value="Ecuadorean">Ecuadorean</option>
                        <option value="Egyptian">Egyptian</option>
                        <option value="Emirian">Emirian</option>
                        <option value="Equatorial Guinean">Equatorial Guinean</option>
                        <option value="Eritrean">Eritrean</option>
                        <option value="Estonian">Estonian</option>
                        <option value="Ethiopian">Ethiopian</option>
                        <option value="Fijian">Fijian</option>
                        <option value="Filipino">Filipino</option>
                        <option value="Finnish">Finnish</option>
                        <option value="French">French</option>
                        <option value="Gabonese">Gabonese</option>
                        <option value="Gambian">Gambian</option>
                        <option value="Georgian">Georgian</option>
                        <option value="German">German</option>
                        <option value="Ghanaian">Ghanaian</option>
                        <option value="Greek">Greek</option>
                        <option value="Grenadian">Grenadian</option>
                        <option value="Guatemalan">Guatemalan</option>
                        <option value="Guinea-Bissauan">Guinea-Bissauan</option>
                        <option value="Guinean">Guinean</option>
                        <option value="Guyanese">Guyanese</option>
                        <option value="Haitian">Haitian</option>
                        <option value="Herzegovinian">Herzegovinian</option>
                        <option value="Honduran">Honduran</option>
                        <option value="Hungarian">Hungarian</option>
                        <option value="Icelander">Icelander</option>
                        <option value="Indian">Indian</option>
                        <option value="Indonesian">Indonesian</option>
                        <option value="Iranian">Iranian</option>
                        <option value="Iraqi">Iraqi</option>
                        <option value="Irish">Irish</option>
                        <option value="Israeli">Israeli</option>
                        <option value="Italian">Italian</option>
                        <option value="Ivorian">Ivorian</option>
                        <option value="Jamaican">Jamaican</option>
                        <option value="Japanese">Japanese</option>
                        <option value="Jordanian">Jordanian</option>
                        <option value="Kazakhstani">Kazakhstani</option>
                        <option value="Kenyan">Kenyan</option>
                        <option value="kittian and Nevisian">Kittian and Nevisian</option>
                        <option value="Kuwaiti">Kuwaiti</option>
                        <option value="kyrgyz">Kyrgyz</option>
                        <option value="Laotian">Laotian</option>
                        <option value="Latvian">Latvian</option>
                        <option value="Lebanese">Lebanese</option>
                        <option value="Liberian">Liberian</option>
                        <option value="Libyan">Libyan</option>
                        <option value="Liechtensteiner">Liechtensteiner</option>
                        <option value="Lithuanian">Lithuanian</option>
                        <option value="Luxembourger">Luxembourger</option>
                        <option value="Macedonian">Macedonian</option>
                        <option value="Malagasy">Malagasy</option>
                        <option value="Malawian">Malawian</option>
                        <option value="Malaysian">Malaysian</option>
                        <option value="Maldivan">Maldivan</option>
                        <option value="Malian">Malian</option>
                        <option value="Maltese">Maltese</option>
                        <option value="Marshallese">Marshallese</option>
                        <option value="Mauritanian">Mauritanian</option>
                        <option value="Mauritian">Mauritian</option>
                        <option value="Mexican">Mexican</option>
                        <option value="Micronesian">Micronesian</option>
                        <option value="Moldovan">Moldovan</option>
                        <option value="Monacan">Monacan</option>
                        <option value="Mongolian">Mongolian</option>
                        <option value="Moroccan">Moroccan</option>
                        <option value="Mosotho">Mosotho</option>
                        <option value="Motswana">Motswana</option>
                        <option value="Mozambican">Mozambican</option>
                        <option value="Namibian">Namibian</option>
                        <option value="Nauruan">Nauruan</option>
                        <option value="Nepalese">Nepalese</option>
                        <option value="New Zealander">New Zealander</option>
                        <option value="Ni-Vanuatu">Ni-Vanuatu</option>
                        <option value="Nicaraguan">Nicaraguan</option>
                        <option value="Nigerien">Nigerien</option>
                        <option value="North Korean">North Korean</option>
                        <option value="Northern Irish">Northern Irish</option>
                        <option value="Norwegian">Norwegian</option>
                        <option value="Omani">Omani</option>
                        <option value="Pakistani">Pakistani</option>
                        <option value="Palauan">Palauan</option>
                        <option value="Panamanian">Panamanian</option>
                        <option value="Papua New Guinean">Papua New Guinean</option>
                        <option value="Paraguayan">Paraguayan</option>
                        <option value="Peruvian">Peruvian</option>
                        <option value="Polish">Polish</option>
                        <option value="Portuguese">Portuguese</option>
                        <option value="Qatari">Qatari</option>
                        <option value="Romanian">Romanian</option>
                        <option value="Russian">Russian</option>
                        <option value="Rwandan">Rwandan</option>
                        <option value="Saint Lucian">Saint Lucian</option>
                        <option value="Salvadoran">Salvadoran</option>
                        <option value="Samoan">Samoan</option>
                        <option value="San Mrinese">San Marinese</option>
                        <option value="Sao Tomean">Sao Tomean</option>
                        <option value="Saudi">Saudi</option>
                        <option value="Scottish">Scottish</option>
                        <option value="Senegalese">Senegalese</option>
                        <option value="Serbian">Serbian</option>
                        <option value="Seychellois">Seychellois</option>
                        <option value="Sierra Leonean">Sierra Leonean</option>
                        <option value="Singaporean" selected="selected">Singaporean</option>
                        <option value="Slovakian">Slovakian</option>
                        <option value="Slovenian">Slovenian</option>
                        <option value="Solomon Islander">Solomon Islander</option>
                        <option value="Somali">Somali</option>
                        <option value="South Arican">South African</option>
                        <option value="South Korean">South Korean</option>
                        <option value="Spanish">Spanish</option>
                        <option value="Sri lLnkan">Sri Lankan</option>
                        <option value="Sdanese">Sudanese</option>
                        <option value="Srinamer">Surinamer</option>
                        <option value="Sazi">Swazi</option>
                        <option value="Swedish">Swedish</option>
                        <option value="Swiss">Swiss</option>
                        <option value="Syrian">Syrian</option>
                        <option value="Taiwanese">Taiwanese</option>
                        <option value="Tajik">Tajik</option>
                        <option value="Tanzanian">Tanzanian</option>
                        <option value="Thai">Thai</option>
                        <option value="Togolese">Togolese</option>
                        <option value="Tongan">Tongan</option>
                        <option value="Trinidadian or Tobagonian">Trinidadian or Tobagonian</option>
                        <option value="Tunisian">Tunisian</option>
                        <option value="Turkish">Turkish</option>
                        <option value="Tuvaluan">Tuvaluan</option>
                        <option value="Ugandan">Ugandan</option>
                        <option value="Ukrainian">Ukrainian</option>
                        <option value="Uruguayan">Uruguayan</option>
                        <option value="Uzbekistani">Uzbekistani</option>
                        <option value="Venezuelan">Venezuelan</option>
                        <option value="Vietnamese">Vietnamese</option>
                        <option value="Welsh">Welsh</option>
                        <option value="Yemenite">Yemenite</option>
                        <option value="Zambian">Zambian</option>
                        <option value="Zimbabwean">Zimbabwean</option>
                    </select>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal53" runat="server" Text="<%$Resources:dictionary,Pass Code %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPassCode" runat="server" MaxLength="4" Visible="false" CssClass="txtPassCode"></asp:TextBox>
                    <asp:Button ID="btnChangePassCode" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnChangePassCode_Click" Text="<%$ Resources:dictionary, Change Pass Code %>" />
                </td>
            </tr>
        </table>
        <table width="700px">
            <tr>
                <td class="wl_pageheaderSubSub" style="text-align: center">
                    <asp:Literal ID="Literal" runat="server" Text="<%$Resources:dictionary,Address Information %>"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="700px" id="FieldsTable2">
            <tr>
                <td style="width: 104px; height: 11px;">
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary,Address %>"></asp:Literal>
                </td>
                <td style="height: 11px;" colspan="3">
                    <asp:TextBox ID="ctrlStreetName" runat="server" MaxLength="200" Width="431px" Style="margin-bottom: 0px"></asp:TextBox>
                    <br />
                    <br />
                    <asp:TextBox ID="ctrlStreetName2" runat="server" MaxLength="200" Width="431px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary,Zip Code %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlZipCode" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal16" runat="server" Text="<%$Resources:dictionary,City %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlCity" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Country %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlCountry" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                </td>
                <td style="width: 92px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <table width="700px">
            <tr>
                <td class="wl_pageheaderSubSub" style="text-align: center">
                    <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:dictionary,Contact Information %>"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="700px" id="FieldsTable3">
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$Resources:dictionary,Mobile %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlMobile" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal19" runat="server" Text="<%$Resources:dictionary,Fax %>"></asp:Literal>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="ctrlOffice" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal20" runat="server" Text="<%$Resources:dictionary,Home %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="ctrlHome" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal21" runat="server" Text="<%$Resources:dictionary,e-mail %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlEmail" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:CheckBox ID="chkIsEmailCompulsory" runat="server" Checked="false" Text="<%$Resources:dictionary, Is Compulsory%>" />
                </td>
            </tr>
            <tr>
                <td class="wl_pageheaderSubSub" style="text-align: center;" colspan="4">
                    <asp:Literal ID="Literal37" runat="server" Text="<%$ Resources:dictionary,Additional Information %>"
                        Visible="True"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="700px" id="FilterTable">
            <tr>
                <td style="text-align: left">
                    <asp:PlaceHolder ID="AdditionalInfoHolder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
        <table width="700px">
            <tr style="visibility: hidden;">
                <td visible="false" style="text-align: center;" colspan="4">
                    <asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:dictionary,Billy House Related Information %>"
                        Visible="False"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 104px">
                    <asp:Literal ID="Literal23" runat="server" Text="<%$ Resources:dictionary,Vita Mix Customer %>"
                        Visible="False"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:CheckBox ID="cbIsVitaMix" runat="server" Visible="False" />
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal24" runat="server" Text="<%$ Resources:dictionary,Water Filter Customer %>"
                        Visible="False"></asp:Literal>
                    &nbsp;
                </td>
                <td>
                    <asp:CheckBox ID="cbIsWaterFilter" runat="server" Visible="False" />
                </td>
            </tr>
            <tr>
                <td style="width: 104px;">
                    <asp:Literal ID="Literal33" runat="server" Text="<%$ Resources:dictionary,Young Customer %>"
                        Visible="False"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:CheckBox ID="cbIsYoung" runat="server" Visible="False" />
                </td>
                <td style="width: 92px">
                    <asp:Literal ID="Literal34" runat="server" Text="<%$ Resources:dictionary,Juice Plus Customer %>"
                        Visible="False"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="cbIsJuicePlus" runat="server" Visible="False" />
                </td>
            </tr>
        </table>
        <table width="700px">
            <tr>
                <td class="wl_pageheaderSubSub" style="text-align: center;">
                    <asp:Literal ID="Literal51" runat="server" Text="<%$Resources:dictionary,Tag No %>"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="700px"  id="FieldsTable5">
            <tr class="wl_lightRaw">
                <td style="width: 104px">
                    <asp:Literal ID="Literal52" runat="server" Text="<%$Resources:dictionary,Tag No %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtTagNo" runat="server" MaxLength="50"></asp:TextBox>
                </td> 
                 <td rowspan="2">
                    &nbsp;
                </td>            
            </tr>
        </table>
        <table width="700px">
            <tr>
                <td class="wl_pageheaderSubSub" style="text-align: center;">
                    <asp:Literal ID="Literal22" runat="server" Text="<%$Resources:dictionary,Remark %>"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="700px" id="FieldsTable4">
            <tr>
                <td style="width: 104px; height: 2px;">
                    <asp:Literal ID="Literal48" runat="server" Text="<%$Resources:dictionary, Remark%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlRemarks" runat="server" Height="118px" MaxLength="50" TextMode="MultiLine"
                        Width="427px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" CssClass="classname btnSave" OnClick="btnSave_Click"
                        Text="<%$ Resources:dictionary, Save %>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='MembershipScaffold.aspx'"
                        type="button" value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$ Resources:dictionary, Delete %>" />
                    <asp:Button ID="btnExportDetails" runat="server" CausesValidation="False" CssClass="classname"
                        Text="<%$ Resources:dictionary, Export %>" Visible="False" />
                    <asp:Label ID="lblID" runat="server" CssClass="LabelMessage" Visible="False" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <script type="text/javascript">
        $(".btnSave").on("click", function() {
            if ($(".txtPassCode") && $(".txtPassCode").val().length == 0) {
                return confirm("Pass Code is empty. Are you sure you want to continue?");
            }
        });
    </script>
</asp:Content>
