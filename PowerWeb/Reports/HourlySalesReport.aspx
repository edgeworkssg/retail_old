<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="HourlySalesReport" Title="<%$Resources:dictionary, Hourly Sales Report%>"
    CodeBehind="HourlySalesReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FilterTable">
        <tr style="height: 20px;">
            <td colspan="4" class="wl_pageheaderSub" style="height: 20px;">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width: 102px; height: 3px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br/>
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
                <br/>
            </td>
            <td style="height: 3px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br/>
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
                <br/>
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
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
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Point Of Sale%>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddPOS" runat="server" Width="179px">
                </asp:DropDownList>
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
    <asp:GridView ID="gvReport" ShowFooter="True" Width="100%" runat="server" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="true" SkinID="scaffold"
        OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
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
