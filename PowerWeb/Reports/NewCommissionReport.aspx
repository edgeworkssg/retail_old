<%@ Page Title="Commission Report" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="NewCommissionReport.aspx.cs" Inherits="PowerWeb.Reports.NewCommissionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="800px" id="FilterTable">
        <tr style="height: 20px;">
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server" Width="150px"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use Start Date %>" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server" Width="150px"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,UserName %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlUser" runat="server" OnInit="ddlUser_Init" Width="150px"
                    DataTextField="DisplayName" DataValueField="UserName">
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 800px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton></td><td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton></td></tr><tr>
            <td colspan="4" style="height: 25px;">
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label></td></tr></table><asp:GridView ID="gvReport" runat="server" AllowPaging="True" OnDataBound="gvReport_DataBound"
        OnSorting="gvReport_Sorting" PageSize="20" OnPageIndexChanging="gvReport_PageIndexChanging"
        AutoGenerateColumns="False" SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound"
        ShowFooter="True" Width="800px">
        <Columns>
            <asp:BoundField DataField="Sales" SortExpression="Sales" HeaderText="Sales Name" />
            <asp:BoundField DataField="SchemeName" SortExpression="SchemeName" HeaderText="Scheme Name" />            
            <asp:BoundField DataField="TotalQuantity" SortExpression="TotalQuantity" HeaderText="Sales Total Quantity" DataFormatString="{0:0.00}">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="TotalAmount" SortExpression="TotalAmount" HeaderText="Sales Total Amount" DataFormatString="{0:0.00}">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>      
            <asp:BoundField DataField="TotalCommission" SortExpression="TotalCommission" HeaderText="Total Commission"
                DataFormatString="{0:0.00}">
                <FooterStyle HorizontalAlign="Right" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
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
</asp:Content>
