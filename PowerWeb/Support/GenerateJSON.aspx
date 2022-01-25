<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="GenerateJSON.aspx.cs" Inherits="PowerWeb.Support.GenereateJSON" Title="<%$Resources:dictionary, Generate Salesman App JSON File%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <div style="width: 800px" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Item Importer%>"></asp:Literal>
    </div>
    <table width="800px" id="FilterTable">
        <tr>
            <td class="fieldname" style="width: 300px">
                <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Export Blank Template%>" />
            </td>
            <td>
                <asp:Button ID="btnExportBlank" runat="server" Text="<%$Resources:dictionary, Export%>" OnClick="btnExportBlank_Click" />
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Generate JSON File%>" />
            </td>
            <td>
                <asp:FileUpload ID="fuItemImporter" runat="server" />
                &nbsp;&nbsp;<asp:Button ID="btnImport" runat="server" Text="<%$Resources:dictionary, Generate%>" OnClick="btnImport_Click" />
            </td>
        </tr>
        <tr>
            <td class="fieldname" colspan="2" style="text-align: center!important;">
                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="position: relative; z-index: 0;">
    </div>
</asp:Content>
