<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="TransactionReport" Title="<%$Resources:dictionary,Transaction Report %>"
    CodeBehind="TransactionReport.aspx.cs" %>

<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=700,width=650,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }
    </script>

    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                <asp:DropDownList ID="ddStartHour" runat="server">
                     <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddStartMinute" runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="ddStartSecond" runat="server">
                </asp:DropDownList>
                <br />
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use Start Date %>" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                <asp:DropDownList ID="ddEndHour" runat="server">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddEndMinute" runat="server">
                </asp:DropDownList>
                 <asp:DropDownList ID="ddEndSecond" runat="server">
                </asp:DropDownList>
                <br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Order Ref No %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtOrderNo" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Cashier ID %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtCashier" runat="server" Width="173px"></asp:TextBox>
            </td>
        </tr>
            <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
        <tr>
            <td>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Payment Mode%>" />
            </td>
            <td>
                <asp:TextBox ID="txtPaymentMode" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Remarks%>" />
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <%--<tr><td colspan=2 style="height: 15px" >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align="right" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>--%>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="chkShowVoidedTransaction" runat="server" Text="<%$Resources:dictionary, Show Voided Transaction%>" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
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
                    <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
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
    <asp:GridView ID="gvReport" Width="100%" runat="server" PageSize="20" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" DataKeyNames="OrderRefNo" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField />
            <asp:TemplateField>
                <ItemTemplate>
                    <a id="HyperLink1" href="javascript:poptastic('../Order/OrderDetail.aspx?id=<%# Eval("OrderHdrID")%>');">
                        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,View %>"></asp:Literal>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OrderDate" SortExpression="OrderDate" HeaderText="<%$Resources:dictionary, Date%>"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
            <asp:BoundField DataField="OrderRefNo" SortExpression="OrderRefNo" HeaderText="<%$Resources:dictionary, Ref No%>" />
            <asp:BoundField DataField="CashierID" SortExpression="CashierID" HeaderText="<%$Resources:dictionary,Cashier %>" />
            <asp:BoundField DataField="TransactionCount" SortExpression="TransactionCount" HeaderText="<%$Resources:dictionary, Transaction Count%>" />
            <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="<%$Resources:dictionary,Nett Amount %>"
                DataFormatString="{0:N2}" />              
            <asp:BoundField DataField="GST" DataFormatString="{0:N2}" HeaderText="<%$Resources:dictionary, GST%>" SortExpression="gst" />
            <asp:BoundField DataField="AmountBefGST" DataFormatString="{0:N2}" HeaderText="<%$Resources:dictionary, Amount bef GST%>"
                SortExpression="amountBefGST" />
            <asp:BoundField DataField="DiscountAmount" HeaderText="<%$Resources:dictionary, Discount%>" SortExpression="DiscountAmount" />
              <asp:BoundField DataField="GrossAmount" SortExpression="GrossAmount" HeaderText="Gross Amount"
                DataFormatString="{0:N2}" />
            <asp:BoundField DataField="PaymentType" SortExpression="PaymentType" HeaderText="<%$Resources:dictionary, Payment Mode%>" />
            <asp:BoundField DataField="CashReceived" SortExpression="CashReceived" HeaderText="<%$Resources:dictionary, Cash Received%>" />
            <asp:BoundField DataField="Change" SortExpression="Change" HeaderText="<%$Resources:dictionary, Change%>" />
            <asp:BoundField DataField="MembershipNo" SortExpression="MembershipNo" HeaderText="<%$Resources:dictionary, Card No%>" />
            <asp:BoundField DataField="NameToAppear" SortExpression="NameToAppear" HeaderText="<%$Resources:dictionary, Name%>" />
            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$Resources:dictionary, Remark%>" />
            <asp:BoundField DataField="PointOfSaleName" SortExpression="PointOfSaleName" HeaderText="<%$Resources:dictionary,Point Of Sale %>" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary,Outlet%>" />
            <asp:BoundField Visible="true" DataField="IsVoided" SortExpression="IsVoided" HeaderText="<%$Resources:dictionary, Is Voided%>"
                ControlStyle-Width="1">
                <ControlStyle Width="1px"></ControlStyle>
            </asp:BoundField>
            <asp:BoundField Visible="false" DataField="PointOfSaleID" SortExpression="PointOfSaleID"
                HeaderText="<%$Resources:dictionary,Point Of Sale ID %>" />
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
