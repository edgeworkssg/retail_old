<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="ProfitLossReport" Title="<%$Resources:dictionary,Profit/Loss Report %>"
    CodeBehind="ProfitLossReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <div style="height: 20px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="600px" id="FilterTable">
        <%--  <tr style="height:20px;"><td colspan=4 class="wl_pageheaderSub" style="height:20px;"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>--%>
        <tr>
            <td style="width: 105px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />&nbsp;
            </td>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td style="width: 105px">
                <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                    Width="68px" />
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="122px">
                    <asp:ListItem Value="1" Text="<%$Resources:dictionary,January %>"></asp:ListItem>
                    <asp:ListItem Value="2" Text="<%$Resources:dictionary,February %>"></asp:ListItem>
                    <asp:ListItem Value="3" Text="<%$Resources:dictionary,March %>"></asp:ListItem>
                    <asp:ListItem Value="4" Text="<%$Resources:dictionary,April %>"></asp:ListItem>
                    <asp:ListItem Value="5" Text="<%$Resources:dictionary,May %>"></asp:ListItem>
                    <asp:ListItem Value="6" Text="<%$Resources:dictionary,June %>"></asp:ListItem>
                    <asp:ListItem Value="7" Text="<%$Resources:dictionary,July %>"></asp:ListItem>
                    <asp:ListItem Value="8" Text="<%$Resources:dictionary,August %>"></asp:ListItem>
                    <asp:ListItem Value="9" Text="<%$Resources:dictionary,September %>"></asp:ListItem>
                    <asp:ListItem Value="10" Text="<%$Resources:dictionary,October %>"></asp:ListItem>
                    <asp:ListItem Value="11" Text="<%$Resources:dictionary,November %>"></asp:ListItem>
                    <asp:ListItem Value="12" Text="<%$Resources:dictionary,December %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlYear" runat="server" Width="70px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 105px">
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Include Pre Order %>"></asp:Literal>
            </td>
            <td>
                <asp:CheckBox ID="cbIncludePreOrder" Checked="false" runat="server" />
            </td>
            <td>
            </td>
            <td>
                
            </td>
        </tr>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 600px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 25px;">
                <asp:Literal ID="litMessage" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlGrid" runat="server">
        <asp:GridView ID="gvReport" Width="800px" runat="server" ShowFooter="True" AllowPaging="True"
            AllowSorting="True" DataKeyNames="PLDate" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
            OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
            <Columns>
                <asp:TemplateField HeaderText="Date" SortExpression="PLDate">
                    <ItemTemplate>
                        <a id="HyperLink1" target="_blank" href="<%# String.Format("ProfitLossReportDaily.aspx?id={0}&Outlet={1}&IncludePreOrder={2}", DateTime.Parse(Eval("PLDate").ToString()).ToString("dd MMM yyyy"), Eval("OutletName"), Eval("IncludePreOrder")) %>">
                            <%# DateTime.Parse(Eval("PLDate").ToString()).ToString("dd MMM yyyy")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PLDate" Visible="false" HeaderText="<%$ Resources:dictionary,Date %>"
                    SortExpression="PLDate" />
                <asp:BoundField DataField="NoOfTransaction" HeaderText="<%$ Resources:dictionary,No Of Transaction %>"
                    SortExpression="NoOfTransaction" />
                <asp:BoundField DataField="GrossSales" HeaderText="<%$ Resources:dictionary,Gross Sales %>"
                    SortExpression="GrossSales" />
                <asp:BoundField DataField="DiscountSales" HeaderText="<%$ Resources:dictionary,Discount %>"
                    SortExpression="DiscountSales" />
                <asp:BoundField DataField="DiscountPercentage" HeaderText="<%$ Resources:dictionary,Disc (%) %>"
                    SortExpression="DiscountPercentage" />
                <asp:BoundField DataField="NettSalesBeforeGST" HeaderText="<%$ Resources:dictionary,Nett Sales %>"
                    SortExpression="NettSalesBeforeGST" />
                <asp:BoundField DataField="GSTAmount" HeaderText="<%$ Resources:dictionary,GST Amount %>"
                    SortExpression="GSTAmount" />
                <asp:BoundField DataField="NettSalesAfterGST" HeaderText="<%$ Resources:dictionary,Nett Sales (Without GST) %>"
                    SortExpression="NettSalesAfterGST" />
                <asp:BoundField DataField="CostofGoodsSold" HeaderText="<%$ Resources:dictionary,Cost of Goods Sold %>"
                    SortExpression="CostofGoodsSold" />
                <asp:BoundField DataField="ProfitLoss" HeaderText="<%$ Resources:dictionary,P/L %>"
                    SortExpression="ProfitLoss" />
                <asp:BoundField DataField="ProfitLossPercentage" HeaderText="<%$ Resources:dictionary,P/L(%) %>"
                    SortExpression="ProfitLossPercentage" />
                <asp:BoundField DataField="OutletName" HeaderText="<%$ Resources:dictionary,Outlet %>"
                    SortExpression="OutletName" />
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
            <EmptyDataTemplate>
                <asp:Literal ID="ltNoData" runat="server" Text="<%$Resources:dictionary,No Data Found%>" /></EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Label ID="lblIncomplete" runat="server" Text="<%$Resources:dictionary,**The report P&L shows as INCOMPLETE because there are inventory item being sold but there are insufficient quantity to deduct the sold item. Kindly check Undeducted Sales Report under Inventory section and adjust the inventory accordingly. %>"
        Style="font-weight: 700; color: #FF0000" Visible="False"></asp:Label><asp:Panel ID="pnlChart"
            runat="server" Visible="false">
            <iframe id="chartFrame1" runat="server" width="100%" height="400" scrolling="yes" />
        </asp:Panel>
</asp:Content>
