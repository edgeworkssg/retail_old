<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="ItemImporter.aspx.cs" Inherits="PowerWeb.Support.ItemImporter" Title="<%$Resources:dictionary, Item Importer%>" %>

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
            <td class="fieldname">
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,  Applicable To%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlApplicableTo" runat="server" Width="147px" OnSelectedIndexChanged="ddlApplicableTo_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Value="Product Master" Text="<%$Resources:dictionary, Product Master%>"></asp:ListItem>
                    <asp:ListItem Value="Outlet" Text="<%$Resources:dictionary, Outlet%>"></asp:ListItem>
                    <asp:ListItem Value="Outlet Group" Text="<%$Resources:dictionary, Outlet Group%>"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlOutletList" runat="server" Width="147px"  Enabled="false">
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
        <tr >
            <td class="fieldname" style="width: 300px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Export Template for Items%>" />
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Department%>" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" Width="75px" OnInit="ddlDepartment_Init"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Category%>" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCategory" runat="server" Width="75px" OnInit="ddlCategory_Init">
                            </asp:DropDownList>
                        </td>
                        <td>
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
