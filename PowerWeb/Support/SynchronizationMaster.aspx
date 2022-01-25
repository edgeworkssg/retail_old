<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="SynchronizationMaster.aspx.cs" Inherits="PowerWeb.Support.SynchronizationMaster" Title="<%$Resources:dictionary, Integration Manager%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <div style="width: 800px" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="Sync Products & Sale"></asp:Literal>
    </div>
    <table width="800px" id="FilterTable">
        <tr>
            <td class="fieldname" style="width: 300px">
                <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Import  %>" />
            </td>
            <td>
                <asp:Button ID="btnSyncProducts" runat="server" Text="<%$Resources:dictionary, Import Products%>" OnClick="btnImportProducts_Click" />
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 300px">
                <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Post Invoice%>" />
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" 
                    Text="<%$Resources:dictionary, Post Invoice%>" OnClick="btnSendSales_Click" 
                    Width="343px" />
            </td>
        </tr>
        
        
    </table>
    <div style="position: relative; z-index: 0;">
    </div>
</asp:Content>
