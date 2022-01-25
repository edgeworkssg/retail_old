<%@ Page Title="<%$Resources:dictionary,Aggregated Sales Report %>" Language="C#"
    MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="AggregatedSalesReport.aspx.cs"
    Inherits="AggregatedSalesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <table width="700px" id="FilterTable">
        <tr>
            <td style="width: 102px; height: 3px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td style="height: 3px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                    Width="68px" />
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="122px">
                    <asp:ListItem Value="1" Text="<%$Resources:dictionary,January %>">
                    </asp:ListItem>
                    <asp:ListItem Value="2" Text="<%$Resources:dictionary,February %>">
                    </asp:ListItem>
                    <asp:ListItem Value="3" Text="<%$Resources:dictionary,March %>">
                    </asp:ListItem>
                    <asp:ListItem Value="4" Text="<%$Resources:dictionary,April %>">
                    </asp:ListItem>
                    <asp:ListItem Value="5" Text="<%$Resources:dictionary,May %>">
                    </asp:ListItem>
                    <asp:ListItem Value="6" Text="<%$Resources:dictionary,June %>">
                    </asp:ListItem>
                    <asp:ListItem Value="7" Text="<%$Resources:dictionary,July %>">
                    </asp:ListItem>
                    <asp:ListItem Value="8" Text="<%$Resources:dictionary,August %>">
                    </asp:ListItem>
                    <asp:ListItem Value="9" Text="<%$Resources:dictionary,September %>">
                    </asp:ListItem>
                    <asp:ListItem Value="10" Text="<%$Resources:dictionary,October %>">
                    </asp:ListItem>
                    <asp:ListItem Value="11" Text="<%$Resources:dictionary,November %>">
                    </asp:ListItem>
                    <asp:ListItem Value="12" Text="<%$Resources:dictionary,December %>">
                    </asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlYear" runat="server" Width="70px">
                </asp:DropDownList>
            </td>
            <td style="width: 102px">
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Search%>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtItemName" runat="server" Width="172px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Sales Channel %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlPOS" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Order Type %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlTransactionType" runat="server">
                    <asp:ListItem Value="" Text="-- Please select --"></asp:ListItem>
                    <asp:ListItem Value="Normal sales" Text="Normal sales (ZORS)"></asp:ListItem>
                    <asp:ListItem Value="Refund with Product Return" Text="Refund with Product Return (ZRES)"></asp:ListItem>
                    <asp:ListItem Value="Refund without Product Return" Text="Refund without Product Return (ZCRS)"></asp:ListItem>
                    <asp:ListItem Value="Sample" Text="Sample (ZSML)"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        
    </table>
    <div style="height: 20px;" class="wl_pageheaderSub">
        <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,History Log %>"></asp:Literal>
    </div>
    <asp:GridView ID="gvHistory" ShowFooter="false" Width="700px" runat="server" AllowPaging="false"
        AllowSorting="false" AutoGenerateColumns="False" SkinID="scaffold" OnRowCommand="gvHistory_RowCommand">
        <Columns>
            <asp:ButtonField ButtonType="Button" CommandName="Use" Text="Use" />
            <asp:BoundField DataField="LogDate" HeaderText="<%$Resources:dictionary, Log Date%>" DataFormatString="{0:dd MMM yyyy HH:mm:ss}" />
            <asp:BoundField DataField="StartDate" HeaderText="<%$Resources:dictionary,Start Date %>" />
            <asp:BoundField DataField="EndDate" HeaderText="<%$Resources:dictionary,End Date %>" />
            <asp:BoundField DataField="PointOfSale" HeaderText="<%$Resources:dictionary,Sales Channel %>" />
            <asp:BoundField DataField="TransactionType" HeaderText="<%$Resources:dictionary,Order Type %>" />
            <asp:BoundField DataField="Filename" HeaderText="<%$Resources:dictionary, Filename%>" />
        </Columns>
    </asp:GridView>
    <table id="search_ExportTable" style="vertical-align: middle; width: 700px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:dictionary, Clear%>" />
                </asp:LinkButton>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px; text-align:right">
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Export%>" />
                </asp:LinkButton>
                <asp:LinkButton ID="lnkZXV3" class="classBlue" runat="server" OnClick="lnkZXV3_Click">
                    <asp:Literal ID="Literal6" runat="server" Text="Generate ZXV3" />
                </asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 25px; color:Red">
                <asp:Label ID="litMessage" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" ShowFooter="True" Width="100%" runat="server" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" DataKeyNames="ItemNo" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
            <asp:BoundField DataField="Attributes1" HeaderText="SAP material code" SortExpression="Attributes1" />
            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />
            <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary,Item Name %>" />
            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="<%$Resources:dictionary,Qty %>" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="<%$Resources:dictionary,Unit Price %>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="TotalAmtWoGST" SortExpression="TotalAmtWoGST" HeaderText="<%$Resources:dictionary, Total Amount (w/o GST) %>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="DiscAmtWoGST" SortExpression="DiscAmtWoGST" HeaderText="<%$Resources:dictionary, Discount Amount (w/o GST) %>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="NettAmtWoGST" SortExpression="NettAmtWoGST" HeaderText="<%$Resources:dictionary, Nett Amount (w/o GST) %>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="GSTAmount" SortExpression="GSTAmount" HeaderText="<%$Resources:dictionary, GST %>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="TotalAmtInclGST" SortExpression="TotalAmtInclGST" HeaderText="<%$Resources:dictionary, Total Amount (inclusive GST) %>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="DiscountPercent" SortExpression="DiscountPercent" HeaderText="<%$Resources:dictionary, Discount (%) %>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="PaymentType" SortExpression="PaymentType" HeaderText="<%$Resources:dictionary, Payment Type %>" />
            <asp:BoundField DataField="PointOfSaleName" SortExpression="PointOfSaleName" HeaderText="<%$Resources:dictionary, Sales Channel %>" />
            <asp:BoundField DataField="CustomerCode" SortExpression="CustomerCode" HeaderText="SAP Customer number" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary, Outlet Name %>" />
            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$Resources:dictionary, Remark %>" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList
                    ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
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
