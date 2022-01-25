<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="MembershipTransactionReport" Title="<%$Resources:dictionary,Customer Purchase Detail Report %>"
    CodeBehind="MembershipTransactionReport.aspx.cs" %>

<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
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
    <div style="height: 20px; width: 700px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="700px" id="FilterTable">
        <%--<tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        --%><tr>
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
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
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
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                </asp:DropDownList>
                <br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtMembershipNo" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,NRIC %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNRIC" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtLastName" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary,Group Name %>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="ddGroupName" runat="server" OrderField="GroupName" PromptValue="0"
                    ShowPrompt="True" TableName="MembershipGroup" TextField="GroupName" ValueField="MembershipGroupID"
                    Width="175px">
                </subsonic:DropDown>
            </td>
            <td>
                <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNameToAppear" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Item Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtItemName" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Category %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddCategory" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <%--<tr>
            <td><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal></td><td>
            <subsonic:DropDown ID="ddPOS" runat="server" OnInit="ddPOS_Init" PromptText="ALL"
                ShowPrompt="True" TableName="PointOfSale" TextField="PointOfSaleName" ValueField="PointOfSaleID"
                Width="170px">
            </subsonic:DropDown></td>
        <td><asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal></td><td>
            <asp:DropDownList ID="ddlOutlet" runat="server" Width="179px">
            </asp:DropDownList></td></tr>
        <tr>--%>
        <tr>
            <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="ddDept" runat="server" OnInit="ddDept_Init" PromptValue=""
                    TableName="Department" TextField="DepartmentName" ValueField="DepartmentID" Width="172px">
                </subsonic:DropDown>
            </td>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Order Ref No %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtOrderNo" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="litStaff" runat="server" Text="<%$Resources:dictionary,Staff %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStaff" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="litRemark" runat="server" Text="<%$Resources:dictionary,Remark %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="litLineInfo" runat="server" Text="<%$Resources:dictionary, Line Info%>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtLineInfo" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px">
                &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td colspan="2" align="right">
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="100%" runat="server" ShowFooter="true" PageSize="20"
        AllowPaging="True" AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" DataKeyNames="OrderRefNo" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField />
            <asp:TemplateField>
                <ItemTemplate>
                    <a id="HyperLink1" href="javascript:poptastic('../Order/OrderDetail.aspx?id=<%# Eval("OrderHdrID")%>');">
                        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,View %>"></asp:Literal></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OrderDate" SortExpression="OrderDate" HeaderText="<%$Resources:dictionary,Order Date %>" />
            <asp:TemplateField HeaderText="<%$Resources:dictionary,Membership No %>">
                <ItemTemplate>
                    <a id="HyperLink2" href="javascript:poptastic('../Membership/MembershipDetail.aspx?id=<%# Eval("MembershipNo")%>');">
                        <asp:Literal ID="SEARCHLbl2" runat="server" Text='<%# Eval("MembershipNo")%>'></asp:Literal></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MembershipNo" Visible="false" SortExpression="MembershipNo" />
            <asp:BoundField DataField="NameToAppear" HeaderText="<%$Resources:dictionary,Name %>"
                SortExpression="NameToAppear" />
            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary,ItemName %>" />
            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="<%$Resources:dictionary,Qty %>" />
            <asp:BoundField DataField="LineAmount" SortExpression="LineAmount" HeaderText="<%$Resources:dictionary,Amount %>" />
            <asp:BoundField DataField="CashierID" SortExpression="CashierID" HeaderText="<%$Resources:dictionary,Cashier %>" />
            <asp:BoundField DataField="SalesPersonID" SortExpression="SalesPersonID" HeaderText="<%$Resources:dictionary,Sales Person %>" />
            <asp:BoundField DataField="PointOfSaleName" SortExpression="PointOfSaleName" HeaderText="<%$Resources:dictionary,Point Of Sale %>" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary,Outlet%>" />
            <asp:BoundField DataField="email" SortExpression="email" HeaderText="<%$Resources:dictionary, Email%>" />
            <asp:BoundField DataField="Mobile" SortExpression="mobile" HeaderText="<%$Resources:dictionary, Mobile%>" />
            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$Resources:dictionary,Remark %>" />
            <asp:BoundField DataField="LineInfo" SortExpression="LineInfo" HeaderText="<%$Resources:dictionary, Line Info%>" />
            <asp:BoundField DataField="BalancePayment" SortExpression="BalancePayment" HeaderText="<%$Resources:dictionary, Balance Payment%>"
                DataFormatString="{0:$0.00}" />
            <asp:BoundField DataField="QtyOnHand" SortExpression="QtyOnHand" HeaderText="<%$Resources:dictionary, Qty On Hand%>" />
            <asp:BoundField Visible="false" DataField="OrderRefNo" SortExpression="OrderRefNo"
                HeaderText="<%$Resources:dictionary,Order RefNo %>" />
            <asp:BoundField Visible="False" DataField="PointOfSaleID" SortExpression="PointOfSaleID"
                HeaderText="<%$Resources:dictionary,Point Of Sale ID %>" />
            <asp:BoundField Visible="false" DataField="CashierID" SortExpression="CashierID"
                HeaderText="<%$Resources:dictionary,Cashier %>" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
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
</asp:Content>
