<%@ Page Title="Serial No Checking" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="ItemTagReport.aspx.cs" Inherits="PowerWeb.Reports.ItemTagReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .style4
        {
            width: 100px;
            height: 26px;
        }
        .style5
        {
            height: 26px;
        }
        .style6
        {
            height: 25px;
        }
        .style7
        {
            height: 25px;
        }
        .style8
        {
            width: 50%;
            left: 0;
            height: 30px;
        }
        .style9
        {
            width: 50%;
            right: 0px;
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 20px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="600px" id="FilterTable">
        <tr>
            <td class="style4">
                Search
            </td>
            <td class="style5">
                <asp:TextBox ID="txtSearch" runat="server" Width="150px"></asp:TextBox>
                <asp:Button ID="btnSearchItem" runat="server" Width="95px" Text="Search" class="classname"
                    OnClick="btnSearchItem_Click" />
            </td>
        </tr>
        <tr>
            <td class="style4">
                Item
            </td>
            <td class="style5">
                <asp:DropDownList ID="ddlItem" runat="server" DataTextField="ItemName" DataValueField="ItemNo"
                    Width="250px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style6">
                Serial No
            </td>
            <td class="style7">
                <asp:TextBox ID="txtSerialNo" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style6">
                &nbsp;
            </td>
            <td class="style7">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style6" colspan="2">
                <asp:LinkButton ID="btnSearch" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:dictionary, Search%>" />&nbsp;
                    <div class="divider"></div>&nbsp;&nbsp;
                </asp:LinkButton><asp:LinkButton ID="btnClear" class="classname" runat="server" OnClick="btnClear_Click">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td class="style6" colspan="2">
                <asp:Literal ID="lblMessage" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <div id="divResult" runat="server" visible="false">
        <div style="height: 20px;" class="wl_pageheaderSub">
            <asp:Literal ID="Literal1" runat="server" Text="RESULT"></asp:Literal></div>
        <table width="600px">
            <asp:HiddenField ID="hfItemNo" runat="server" />
            <tr>
                <td>
                    Item
                </td>
                <td style="width: 5px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblItemName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Serial No
                </td>
                <td style="width: 5px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblSerialNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Current Status
                </td>
                <td style="width: 5px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Location
                </td>
                <td style="width: 5px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblLocation" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <div style="height: 20px;" class="wl_pageheaderSub">
            <asp:Literal ID="Literal2" runat="server" Text="HISTORY"></asp:Literal></div>
        <asp:GridView ID="gvReport" ShowFooter="True" Width="100%" runat="server" AllowPaging="True"
            AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
            OnPageIndexChanging="gvReport_PageIndexChanging" DataKeyNames="ItemNo" AutoGenerateColumns="False"
            SkinID="scaffold" PageSize="20">
            <Columns>
                <asp:BoundField DataField="InventoryDate" SortExpression="InventoryDate" HeaderText="<%$Resources:dictionary, Date%>"
                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                <asp:BoundField DataField="Movement" SortExpression="Movement" HeaderText="Movement" />
                <asp:BoundField DataField="FromLocation" SortExpression="FromLocation" HeaderText="From Location" />
                <asp:BoundField DataField="ToLocation" SortExpression="ToLocation" HeaderText="To Location" />
            </Columns>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList
                        ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                        ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server"
                            CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next"
                            CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                        CommandArgument="Last" CommandName="Page" />
                </div>
            </PagerTemplate>
        </asp:GridView>
    </div>
</asp:Content>
