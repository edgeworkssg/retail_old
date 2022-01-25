<%@ Page Title="<%$Resources:dictionary, Membership Importer%>" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="MembershipImporter.aspx.cs" Inherits="PowerWeb.Support.MembershipImporter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <div style="width: 800px" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Membership Importer%>"></asp:Literal>
    </div>
    <table width="800px" id="FilterTable">
        <tr>
            <td class="fieldname" style="width: 300px">
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Export Blank Template%>" />
            </td>
            <td>
                <asp:Button ID="btnExportBlank" runat="server" Text="<%$Resources:dictionary, Export%>"
                    OnClick="btnExportBlank_Click" />
            </td>
        </tr>
        <tr >
            <td class="fieldname" style="width: 300px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Export Template for Members%>" />
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Group%>" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" OnInit="ddlDepartment_Init">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnExportItemDept" runat="server" Text="<%$Resources:dictionary, Export%>"
                                OnClick="btnExportItemDept_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="background-color:#dddbdc">
            <td class="fieldname">
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Import Data%>" />
            </td>
            <td>
                <asp:FileUpload ID="fuItemImporter" runat="server" />
                &nbsp;&nbsp;<asp:Button ID="btnImport" runat="server" Text="<%$Resources:dictionary, Import%>"
                    OnClick="btnImport_Click" />
            </td>
        </tr>
        <tr style="background-color:#ebebeb">
            <td class="fieldname" colspan="2" style="text-align: center!important;">
                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="position: relative; z-index: 0;">
    </div>
</asp:Content>
