<%@ Page Title="Recipe Importer" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="RecipeImporter.aspx.cs" Inherits="PowerWeb.Support.RecipeImporter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
        <div style="width: 800px" class="wl_pageheaderSub">
            <asp:Literal ID="SEARCHLbl" runat="server" Text="Recipes Importer"></asp:Literal>
        </div>
    <table width="800px" id="FilterTable">
        <tr>
            <td class="fieldname" style="width: 300px">
                Export Blank Template
            </td>
            <td>
                <asp:Button ID="btnExportBlank" runat="server" Text="Export" OnClick="btnExportBlank_Click" />
            </td>
        </tr>
        <tr>
            <td class="fieldname" tyle="width: 300px">
                Export Template for Items
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            Department
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" Width="75px" 
                                OnInit="ddlDepartment_Init" AutoPostBack="True" 
                                onselectedindexchanged="ddlDepartment_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                            Category
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCategory" runat="server" Width="75px" 
                                OnInit="ddlCategory_Init">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnExportItemDept" runat="server" Text="Export" 
                                OnClick="btnExportItemDept_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trImportData" runat="server">
            <td class="fieldname">
                Import Data
            </td>
            <td>
                <asp:FileUpload ID="fuItemImporter" runat="server" />
                &nbsp;&nbsp;<asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" />
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
