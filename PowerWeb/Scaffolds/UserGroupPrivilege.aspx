<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="UserGroupPrivilege" Title="<%$Resources:dictionary,Assign Privileges %>"
    CodeBehind="UserGroupPrivilege.aspx.cs" %>

<asp:Content ID="ContentHeader1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .accordionHeader
        {
            font-family: Arial,Helvetica,sans-serif;
            color: #FFF;
            font-size: 16px;
            width: 100%;
            padding: 3px 3px 3px 5px;
            height: 23px;
            background: #51A2E4;
            border: solid 1px #87B3D1;
            cursor: pointer;
            background: #4d9ee0; /* Old browsers */
            background: -moz-linear-gradient(top,  #4d9ee0 0%, #69b8f3 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#4d9ee0), color-stop(100%,#69b8f3)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* IE10+ */
            background: linear-gradient(to bottom,  #4d9ee0 0%,#69b8f3 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#4d9ee0', endColorstr='#69b8f3',GradientType=0 ); /* IE6-9 */
        }
        .accordionHeaderSelected
        {
            font-family: Arial,Helvetica,sans-serif;
            color: #FFF;
            font-size: 16px;
            width: 100%;
            padding: 3px 3px 3px 5px;
            height: 23px;
            background: #4A9BDD;
            border: solid 1px #357FB7;
            cursor: pointer;
        }
        .accordionContent
        {
            width: 100%;
            background: #E1E1E1;
            border: solid 1px #D1D1D1;
            padding: 5px;
            width: 788px !important;
            overflow: hidden !important;
        }
        .ctl00_ContentPlaceHolder1_MyAccordion
        {
            overflow: hidden !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server" />
    <table width="800px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Group Name %>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="DropDown1" runat="server" TableName="UserGroup" TextField="GroupName"
                    ValueField="GroupID" WhereField="Deleted" WhereValue="false" OnSelectedIndexChanged="DropDown1_SelectedIndexChanged"
                    ShowPrompt="true" AutoPostBack="True">
                </subsonic:DropDown>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Select All%>"
                    Width="130px" ID="BtnSelectAll" OnClick="BtnSelectAll_Click" /><div class="divider">
                    </div>
                <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Clear Selection%>"
                    ID="BtnClearSelection" OnClick="BtnClearSelection_Click" Width="130px" />
            </td>
        </tr>
    </table>
    <br />
    <%--    <asp:Panel ID="Panel1" runat="server">
    </asp:Panel>--%>
    <ajax:UpdatePanel ID="PanelPrivileges" runat="server" EnableViewState="true">
        <ContentTemplate>
            <asp:Button ID="Button1" runat="server" CssClass="classname" OnClick="Button1_Click"
                Text="<%$Resources:dictionary,Submit %>" />
            <br />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
            <br />
            <div id="divPrivileges" runat="server">
            </div>
            <br />
        </ContentTemplate>
    </ajax:UpdatePanel>
    <%-- <br />
    <asp:CheckBoxList ID="cblPrivileges" runat="server" RepeatColumns="3">
    </asp:CheckBoxList>--%>
    <br />
    <%--<asp:Button ID="Button1" runat="server" CssClass="classname" OnClick="Button1_Click"
        Text="<%$Resources:dictionary,Submit %>" />--%>
</asp:Content>
