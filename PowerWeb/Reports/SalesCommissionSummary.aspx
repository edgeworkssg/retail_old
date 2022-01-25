<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="SalesCommissionSummary" Title="<%$Resources:dictionary,Sales Commission Summary %>"
    CodeBehind="SalesCommissionSummary.aspx.cs" %>

<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=600,width=800,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }
    </script>
    
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FilterTable">
        <tr style="background: none; height: 3px">
            <td colspan="4">
                <div style="height: 20px;" class="wl_pageheaderSub">
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
                </div>
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
                <asp:Literal ID="Literal1a" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server" Width="150px"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <%--<tr>
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
                <td>
                </td>
                <td>
                </td>
        </tr>--%>
        <tr>
            <td style="width: 102px">
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Staff%>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlStaff" runat="server" Width="179px" OnInit="ddlStaff_Init">
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
                </asp:LinkButton>
                </asp:LinkButton><div class="divider">
                 <asp:LinkButton ID="LinkButton3" class="classname" runat="server" OnClick="btnShowUnpaid_Click">
                    <asp:Literal ID="Literal4" runat="server" Text="Show Unpaid" />
                </asp:LinkButton>
                <div class="divider" style="margin-right:20px;"></div>
                <asp:LinkButton ID="LinkButton2" class="classname" runat="server" OnClick="btnGenerate_Click"
                    OnClientClick="return confirm('Are you sure you want to generate this commission?')">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:dictionary, Generate%>"/>
                </asp:LinkButton><div class="divider">
                </div>
                <%--<asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>--%>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <%--<asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>--%>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 25px;">
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" runat="server" AllowPaging="True" OnDataBound="gvReport_DataBound"
        OnSorting="gvReport_Sorting" PageSize="20" OnPageIndexChanging="gvReport_PageIndexChanging"
        AutoGenerateColumns="False" SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound"
        ShowFooter="True" Width="800px" onrowcommand="gvReport_RowCommand">
        <Columns>
            <asp:BoundField DataField="Month" SortExpression="Month" HeaderText="Month" />
            <asp:BoundField DataField="Staff" SortExpression="Staff" HeaderText="Staff" />
            <asp:BoundField DataField="Salary" SortExpression="Salary" HeaderText="Salary" DataFormatString="{0:0.00}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="OtherAllowance" SortExpression="OtherAllowance" HeaderText="Other Allowance"
                DataFormatString="{0:0.00}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Commission" SortExpression="Commission" HeaderText="Commission"
                DataFormatString="{0:0.00}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total" SortExpression="Total" HeaderText="Total" DataFormatString="{0:0.00}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Details" ItemStyle-Width="130px" HeaderStyle-Width="130px">
                <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <a class="classname" id="HyperLink1" href="javascript:poptastic('SalesCommissionDetails.aspx?Month=<%# Eval("Month")%>&Staff=<%# Eval("Staff")%>');">
                        <asp:Literal ID="ltr04" runat="server" Text="Details" />
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <asp:Button ID="btnPay" runat="server" class="classname" Text="Pay"
                        CommandName="Pay" CommandArgument='<%# Eval("ID")%>' OnClientClick="return confirm('Are you sure you want to pay this salary?')" />
                </ItemTemplate>
            </asp:TemplateField>
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
