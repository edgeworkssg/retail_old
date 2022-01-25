<%@ Page Language="C#" Title="<%$Resources:dictionary, Feedback Form%>" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="PowerWeb.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="controls">
        <style>
            #controls table tr td input[type="text"]
            {
                width: 300px;
            }
            #controls table tr td textarea
            {
                width: 420px;
            }
        </style>
        <br />
        <span><asp:Literal ID="lbl" runat="server" Text="<%$Resources:dictionary,Fields with * are required. %>"/></span>
        <br />
        <br />
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <caption>
                <asp:Literal runat="server" ID="lblMsg" /></caption>
            <tr>
                <td>
                    <span><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Company * %>"/></span>
                </td>
                <td>
                    <asp:TextBox ID="txtCompany" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Name * %>"/> </span>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span><asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Email * %>"/> </span>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" />
                    <asp:RegularExpressionValidator runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ControlToValidate="txtEmail" ErrorMessage="<%$Resources:dictionary, Invalid Email%>" />
                </td>
            </tr>
            <tr>
                <td>
                    <span><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Subject %>" /></span>
                </td>
                <td>
                    <asp:TextBox ID="txtSubject" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Severity * %>"/> </span>
                </td>
                <td>
                    <asp:DropDownList ID="cboSeverity" runat="server" Width="420px">
                        <asp:ListItem Text="<%$Resources:dictionary, Low.%>" Value="low" />
                        <asp:ListItem Text="<%$Resources:dictionary, Important. %>" Value="important" />
                        <asp:ListItem Text="<%$Resources:dictionary, Critical. %>" Value="critical" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    <span><asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Description * %>" /></span>
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="10" />
                </td>
            </tr>
            <tr>
                <td>
                    <span><asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Rate Our Service * %>"/></span>
                </td>
                <td>
                    <asp:DropDownList ID="cboRate" runat="server" Width="80px">
                        <asp:ListItem Text="1" Value="1" />
                        <asp:ListItem Text="2" Value="2" />
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="4" Value="4" />
                        <asp:ListItem Text="5" Value="5" />
                    </asp:DropDownList>
                    <span><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Rate Our %>"/></span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button CssClass="classname" runat="server" ID="btnSend" Text="<%$Resources:dictionary, SEND%>" OnClick="btnSend_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
