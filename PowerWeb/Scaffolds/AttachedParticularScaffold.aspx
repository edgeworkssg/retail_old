<%@ Page Language="C#" Title="<%$Resources:dictionary, Attached Particular%>" Inherits="PowerPOS.AttachedParticularScaffold"
    MasterPageFile="~/PowerPOSMst.master" Theme="default" CodeBehind="AttachedParticularScaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<h2>AttachedParticular</h2>--%>
    <asp:Panel ID="pnlGrid" runat="server">
        <asp:Label ID="lblMessage" runat="server" CssClass="LabelMessage"></asp:Label>
        <br />
        <a href="AttachedParticularScaffold.aspx?id=0&membershipno=<%= MembershipNo %>" class="classname">
            <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Add New%>" /></a>
        <a href="MembershipScaffold.aspx" class="classname">Return</a>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="AttachedParticularID"
            PageSize="20" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="AttachedParticularID, MembershipNo"
                    DataNavigateUrlFormatString="AttachedParticularScaffold.aspx?id={0}&membershipno={1}" />
                <asp:BoundField DataField="FirstName" HeaderText="<%$Resources:dictionary,First Name %>"
                    SortExpression="FirstName"></asp:BoundField>
                <asp:BoundField DataField="LastName" HeaderText="<%$Resources:dictionary,Last Name %>"
                    SortExpression="LastName"></asp:BoundField>
                <asp:BoundField DataField="ChristianName" HeaderText="<%$Resources:dictionary,Christian Name %>"
                    SortExpression="ChristianName"></asp:BoundField>
                <asp:BoundField DataField="ChineseName" HeaderText="<%$Resources:dictionary,Chinese Name %>"
                    SortExpression="ChineseName"></asp:BoundField>
                <asp:BoundField DataField="Gender" HeaderText="<%$Resources:dictionary,Gender %>"
                    SortExpression="Gender"></asp:BoundField>
                <asp:BoundField DataField="Occupation" HeaderText="<%$Resources:dictionary,Occupation %>"
                    SortExpression="Occupation"></asp:BoundField>
                <asp:BoundField DataField="DateOfBirth" HeaderText="<%$Resources:dictionary,Date Of Birth %>"
                    SortExpression="DateOfBirth" DataFormatString="{0: dd MMM yyyy}"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, No Attached Particular%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                    <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal>
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %>"
                        CommandArgument="Next" CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <ajax:ScriptManager ID="ScriptManager1" runat="server">
        </ajax:ScriptManager>
        <cc1:CalendarExtender ID="cldDOB" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton1" TargetControlID="ctrlDateOfBirth">
        </cc1:CalendarExtender>
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <asp:Label ID="lblID" runat="server" Visible="false" />
        <table class="scaffoldEditTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td class="scaffoldEditItemCaption">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:Label ID="lblMembershipNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlFirstName" runat="server" MaxLength="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlLastName" runat="server" MaxLength="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Christian Name %>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlChristianName" runat="server" MaxLength="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Chinese Name %>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlChineseName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Gender %>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:DropDownList ID="ctrlGender" runat="server">
                        <asp:ListItem Value="M" Text="<%$Resources:dictionary, Male%>"></asp:ListItem>
                        <asp:ListItem Value="F" Text="<%$Resources:dictionary, Female%>"></asp:ListItem>
                    </asp:DropDownList>
                    <%--<asp:TextBox ID="ctrlGender" runat="server" MaxLength="1"></asp:TextBox>--%>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Occupation %>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlOccupation" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Date Of Birth %>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlDateOfBirth" runat="server" Width="118px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                        Style="height: 16px; width: 16px" />
                    <%--<subsonic:CalendarControl ID="ctrlDateOfBirth" runat="server"></subsonic:CalendarControl>--%>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="<%$ Resources:dictionary, Save %>"
                        OnClick="btnSave_Click"></asp:Button>&nbsp;
                    <input id="btnReturn" runat="server" type="button" onclick="location.href='AttachedParticularScaffold.aspx?membershipno=<%= MembershipNo %>'"
                        class="classname" value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
                        Text="<%$ Resources:dictionary, Delete %>" OnClick="btnDelete_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
