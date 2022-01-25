<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="DailySalesReport.aspx.cs" Inherits="PowerWeb.Reports.DailySalesReport"
    Title="<%$Resources:dictionary,Daily Sales Report %>" %>
<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="800px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 105px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server" Width="141px"></asp:TextBox><asp:ImageButton
                    ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                &nbsp;
            </td>
            <td class="fieldname">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                &nbsp;
                <br />
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 105px">
                <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                    Width="68px" />
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="100px">
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
                <asp:DropDownList ID="ddlYear" runat="server" Width="60px">
                </asp:DropDownList>
            </td>
            <td class="fieldname" colspan="2">
                <asp:CheckBox ID="chkShowAmountBeforeGST" runat="server" Text="<%$Resources:dictionary,Show Amount Before GST%>" />
            </td>
        </tr>
            <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" CssClass="classname" />
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" CssClass="classname" />
            </td>
            <td colspan="2" align="right">
                <asp:LinkButton ID="lnkExport" runat="server" CssClass="classBlue" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="800px" runat="server" ShowFooter="True" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
            <asp:BoundField HeaderText="<%$Resources:dictionary, Sales Date %>" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false"
                DataField="SalesDate" SortExpression="SalesDate" />
            <asp:BoundField DataField="ShopLevel" HeaderText="<%$Resources:dictionary,Shop Level %>" SortExpression="ShopLevel" />
            <asp:BoundField DataField="ShopNo" HeaderText="<%$Resources:dictionary,Shop No %>" SortExpression="ShopNo" />                            
            <asp:BoundField DataField="PointOfSaleName" HeaderText="<%$Resources:dictionary,Point Of Sale %>" SortExpression="PointOfSaleName" />            
            <asp:TemplateField HeaderText="Actual Sales (Cash/Nets/Card/Vouchers)" AccessibleHeaderText="TotalSales">
                <FooterTemplate>
                    <asp:Label ID="TotalSales" runat="server" Text="0"></asp:Label>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="TotalSales" runat="server" Text="0"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TotalTransSTR" HeaderText="<%$Resources:dictionary,Total Trans %>" SortExpression="TotalTrans" />                
            <asp:TemplateField HeaderText="Points Redeem" AccessibleHeaderText="TotalPointsRedeem">
                <FooterTemplate>
                    <asp:Label ID="TotalPointsRedeem" runat="server" Text="0"></asp:Label>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="TotalPointsRedeem" runat="server" Text="0"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Installment" AccessibleHeaderText="TotalInstallment">
                <FooterTemplate>
                    <asp:Label ID="TotalInstallment" runat="server" Text="0"></asp:Label>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="TotalInstallment" runat="server" Text="0"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TotalCustomer" HeaderText="<%$Resources:dictionary,Customer %>" SortExpression="TotalCustomer" />
            <asp:BoundField DataField="ShopArea" HeaderText="<%$Resources:dictionary,Shop Area (sqft) %>" SortExpression="ShopArea" />         
            <asp:BoundField DataField="Attribute1" HeaderText="<%$Resources:dictionary,Attribute1 %>" SortExpression="Attribute1" />
            <asp:BoundField DataField="Attribute2" HeaderText="<%$Resources:dictionary,Attribute2 %>" SortExpression="Attribute2" />                        
            <asp:BoundField DataField="TotalSalesSTR" HeaderText="<%$ Resources:dictionary,Total Sales %>" SortExpression="TotalSales" />
            <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary,Outlet %>" SortExpression="OutletName" />
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
