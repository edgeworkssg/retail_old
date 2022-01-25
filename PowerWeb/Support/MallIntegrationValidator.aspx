<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="MallIntegrationValidator.aspx.cs" Inherits="PowerWeb.Support.MallIntegrationValidator"
    Title="<%$Resources:dictionary, Interface File Submission%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <div style="width: 800px" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Interface File Submission%>"></asp:Literal>
    </div>
    <table width="800px" id="FilterTable">
        <tr>
            <td class="fieldname" colspan="2">
                <asp:Label ID="Label1" runat="server" Text="<%$Resources:dictionary, Download the interface file specification%> " />
                <asp:LinkButton ID="lnkDownload" runat="server" Text="<%$Resources:dictionary, here%>" OnClick="lnkDownload_Click" />
            </td>
        </tr>
        <tr>
            <td class="style3">
                <asp:Label ID="lblOutlet" runat="server" Text="<%$Resources:dictionary, Outlet%>" />
            </td>
            <td class="style3">
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="200px" AutoPostBack="true"
                    OnInit="ddlOutlet_Init" DataTextField="OutletName" DataValueField="OutletName"
                    OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;
                <asp:Label ID="lblOutletCode" runat="server" Text="<%$Resources:dictionary, Outlet Code :%> " />
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Label ID="lblPOS" runat="server" Text="<%$Resources:dictionary, POS%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlTenant" runat="server" Width="200px" DataTextField="PointOfSaleName"
                    DataValueField="PointOfSaleID" AutoPostBack="True" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;<asp:Label ID="lblPOSCode" runat="server" Text="<%$Resources:dictionary, POS Code :%> " />
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Interface Validation Status%>" />
            </td>
            <td>
                <asp:Label ID="lblValidationStatus" runat="server" Text="<%$Resources:dictionary, [Status]%>" />
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Validate File%>" />
            </td>
            <td>
                <asp:FileUpload ID="fufileValidator" runat="server" />
                &nbsp;&nbsp;<asp:Button ID="btnValidate" runat="server" Text="<%$Resources:dictionary, Validate%>" OnClick="btnValidate_Click" />
            </td>
        </tr>
        <tr>
            <td class="fieldname" colspan="2" style="text-align: center!important;">
                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="fieldname" colspan="2" style="text-align: center!important;">
                <asp:Label ID="lblSuccessStatus" runat="server" Font-Bold="True" Font-Size="Medium"
                    ForeColor="#00CC00"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="position: relative; z-index: 0;">
    </div>
</asp:Content>
