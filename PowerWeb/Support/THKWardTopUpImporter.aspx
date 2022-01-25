<%@ Page Title="<%$Resources:dictionary, Ward Top Up Importer%>" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="THKWardTopUpImporter.aspx.cs" Inherits="PowerWeb.Support.THKWardTopUpImporter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <div style="width: 800px" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Ward Top Up Importer%>"></asp:Literal>
    </div>
    <table width="800px" id="FilterTable">
        <tr>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Inventory Location%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlInventoryLocation" runat="server" Width="150px" OnInit="ddlInventoryLocation_Init">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 300px">
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Export Blank Template%>" />
            </td>
            <td>
                <asp:Button ID="btnExportBlank" runat="server" Text="<%$Resources:dictionary, Export%>"
                    OnClick="btnExportBlank_Click" />
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 300px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Export Template for Items%>" />
            </td>
            <td>
                <table>
                    <tr>
                        
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnExportItemDept" runat="server" Text="<%$Resources:dictionary, Export%>"
                                OnClick="btnExportItemWardTopUp_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Import Data%>" />
            </td>
            <td>
                <asp:FileUpload ID="fuItemImporter" runat="server" />
                &nbsp;&nbsp;<asp:Button ID="btnImport" runat="server" Text="<%$Resources:dictionary, Import%>"
                    OnClick="btnImport_Click" />
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
