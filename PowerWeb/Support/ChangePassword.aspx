<%@ Page Title="<%$Resources:dictionary, Change Password%>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="PowerWeb.Support.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblResult" runat="server"></asp:Label>
    <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
        <tr>
            <td>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Display Name%>" />
            </td>
            <td>
                <asp:Literal ID="lblDisplayName" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Old Password %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtOldPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,New Password %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtCfmPassword"
                    ControlToValidate="txtNewPassword" ErrorMessage="<%$ Resources:dictionary, Password must match %>"></asp:CompareValidator>
            </td>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Re-Type Password %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCfmPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="<%$ Resources:dictionary, Save %>"
                    OnClick="btnSave_Click"></asp:Button>&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
