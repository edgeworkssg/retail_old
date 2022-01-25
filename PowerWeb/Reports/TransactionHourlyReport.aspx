<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="TransactionHourlyReport.aspx.cs" Inherits="PowerWeb.Reports.TransactionHourlyReport" Title="<%$Resources:dictionary, Transaction Hourly Report%>" %>
<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="800px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="fieldname">
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
            <td class="fieldname">
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
           <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
        </tr>
        <tr>
            <td class="fieldname">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td class="fieldname">
                &nbsp;</td>
            <td>
                <asp:CheckBox ID="chkShowBeforeGST" runat="server" Text="<%$Resources:dictionary, Show Amount Before GST%>" />            
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px">
                &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" CssClass="classname" />
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" CssClass="classname" />
            </td>
            <td colspan="2" align="right">
                <asp:LinkButton ID="lnkExport" runat="server" CssClass="classBlue" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvReport" Width="800px" runat="server" PageSize="20" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False" ShowFooter="True"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField DataField="OrderDate" HeaderText="<%$Resources:dictionary,Order Date %>" />
            <asp:BoundField DataField="OrderHour" HeaderText="<%$Resources:dictionary, Order Hour%>" />
            <asp:BoundField DataField="TotalAmount" HeaderText="<%$Resources:dictionary, Total Amount%>" />
            <asp:BoundField DataField="TotalTrans" HeaderText="<%$Resources:dictionary, Total Trans%>" />
            <asp:BoundField DataField="Outlet" HeaderText="<%$Resources:dictionary, Outlet%>" />
            <asp:BoundField DataField="PointOfSale" HeaderText="<%$Resources:dictionary, Point Of Sale%>" />
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
</asp:Content>
