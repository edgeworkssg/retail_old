<%@ Page Title="<%$ Resources:dictionary, Merge Similar Item%>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="MergeSimilarItem.aspx.cs" Inherits="PowerWeb.Product.MergeSimilarItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/css/themes/tooltipster-light.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.css") %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="800px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="style4">
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Barcode %>"></asp:Literal>
            </td>
            <td align="left" class="style4">
                <asp:TextBox ID="txtBarcode" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="Button1" runat="server" CssClass="classname" OnClick="btnSearch_Click"
                    Text="<%$ Resources:dictionary, Search %>" />
                <asp:Button ID="Button2" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
                <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Merge Similar Item%>"
                    Width="130px" ID="BtnMerge" OnClick="BtnMerge_Click" />
                <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Return%>"
                    Width="130px" ID="BtnReturn" OnClick="BtnReturn_Click" />
            </td>
        </tr>
        <tr style="background-color: #ebebeb">
            <td class="fieldname" colspan="2" style="text-align: center!important;">
                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvReport" Width="80%" runat="server" AllowPaging="True" AllowSorting="True"
        OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting" OnPageIndexChanging="gvReport_PageIndexChanging"
        SkinID="scaffold" PageSize="50" OnRowDataBound="gvReport_RowDataBound" ShowFooter="True"
        AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No %>" />
            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Item Name %>" />
            <asp:BoundField DataField="Barcode" HeaderText="<%$Resources:dictionary,Barcode %>" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                    ID="lblPageCount" runat="server"></asp:Label>
                <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                    CommandArgument="Next" CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                    CommandArgument="Last" CommandName="Page" />
            </div>
        </PagerTemplate>
    </asp:GridView>
    <div id="tempPanel" runat="server" style="display: none;">
         <asp:Button ID="BtnMergeReal" OnClick="BtnMergeReal_Click" runat="server" CausesValidation="false" />
    </div>
    <div id="dialog-warning" title="Warning" style="display: none;">
        <div class="panel" id="panelCopyPromo">
           <asp:Label ID="labelWarning" runat="server"></asp:Label>
        </div>
    </div>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js")%>"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            app.setting.baseUrl = $('#BaseUrl').val();
        });


        $("#dialog-warning").dialog({
            autoOpen: false,
            height: 400,
            width: 600,
            modal: true,
            buttons: {
                "Yes": function(evt) {
                    evt.preventDefault();
                    $("[id*=BtnMergeReal]").click();
                    $(this).dialog("close");
                },
                Cancel: function(evt) {
                    evt.preventDefault();
                    $(this).dialog("close");
                }
            },
            close: function(evt) {
                evt.preventDefault();

            }
        }); 
    </script>

</asp:Content>
