<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="SetupProductPromotion.aspx.cs" Title="<%$Resources:dictionary,Product Promotion Setup %>"
    Inherits="PowerWeb.Promo.PromoGroupPriceDiscount" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/css/themes/tooltipster-light.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Chosen/chosen.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/Style.css") %>" />
    <style type="text/css">
        .input-disabled
        {
            background-color: #EBEBE4;
            border: 1px solid #ABADB3;
            padding: 2px 1px;
        }
    </style>
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtPromoDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy hh:mm tt"
        PopupButtonID="ImageButton3" TargetControlID="txtCtrlStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy hh:mm tt"
        PopupButtonID="ImageButton4" TargetControlID="txtCtrlEndDate">
    </cc1:CalendarExtender>
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
    <asp:Panel ID="pnlIndex" runat="server">
        <table width="600px" id="FieldsTable">
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="172px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Valid On %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Valid On %>"></asp:Literal>
                </td>
                <td style="width: 220px">
                    <asp:TextBox ID="txtPromoDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOutlet" runat="server" Width="172px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="search_ExportTable" style="vertical-align: middle; width: 600px; height: 40px;"
            border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                    <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                        <asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                    </asp:LinkButton><div class="divider">
                    </div>
                    <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClearFilter_Click">
                        <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Clear%>" />
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="height: 25px;">
                    <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
            </tr>
        </table>
        <input class="classname" onclick="location.href='SetupProductPromotion.aspx?id=0'"
            type="button" width="130px" value="Add New" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="Select All" Width="130px" ID="BtnSelectAll"
            OnClick="BtnSelectAll_Click" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="Clear Selection" ID="BtnClearSelection"
            OnClick="BtnClearSelection_Click" Width="130px" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="Delete Selected" ID="BtnDeleteSelection"
            OnClick="BtnDeleteSelection_Click" Width="130px" /><div class="divider">
            </div>
        <input type="button" id="btnCopyPromo" value="Copy Outlet Promotion" class="classname" />
        &nbsp;
        <div style="height: 10px;">
        </div>
        <asp:GridView ID="GridView1" SkinID="scaffold" runat="server" AllowPaging="True"
            AllowSorting="True" OnSorting="GridView1_Sorting" AutoGenerateColumns="False"
            DataKeyNames="PromoCampaignHdrID" PageSize="20" Width="100%" OnRowDataBound="GridView1_RowDataBound"
            OnDataBound="GridView1_DataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
            <Columns>
                <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="PromoCampaignHdrID" DataNavigateUrlFormatString="SetupProductPromotion.aspx?id={0}" />
                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPromoCampaignHdrIDGV" runat="server" Text='<%# Eval("PromoCampaignHdrID")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PromoCode" HeaderText="<%$Resources:dictionary,Promo Code %>"
                    SortExpression="PromoCode" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                </asp:BoundField>
                <asp:BoundField DataField="PromoCampaignName" HeaderText="<%$Resources:dictionary,Promo Name %>"
                    SortExpression="PromoCampaignName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                </asp:BoundField>
                <asp:BoundField DataField="Priority" HeaderText="<%$Resources:dictionary,Priority %>"
                    SortExpression="Priority"></asp:BoundField>
                <asp:BoundField DataField="DateFrom" HeaderText="<%$Resources:dictionary,Start Date%>"
                    SortExpression="DateFrom" DataFormatString="{0:dd MMM yyyy hh:mm tt}"></asp:BoundField>
                <asp:BoundField DataField="DateTo" HeaderText="<%$Resources:dictionary,End Date%>"
                    SortExpression="DateTo" DataFormatString="{0:dd MMM yyyy hh:mm tt}"></asp:BoundField>
                <asp:BoundField DataField="IsRestrictHour" HeaderText="Hour" SortExpression="IsRestrictHour">
                </asp:BoundField>
                <asp:BoundField DataField="ApplicableDays" HeaderText="Days" SortExpression="ApplicableDays">
                </asp:BoundField>
                <asp:BoundField DataField="ForNonMembersAlso" HeaderText="<%$Resources:dictionary,Applicable to %>"
                    SortExpression="ForNonMembersAlso" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                </asp:BoundField>
                <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category %>"
                    SortExpression="CategoryName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                </asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Product Name %>"
                    SortExpression="ItemName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                </asp:BoundField>
                <asp:BoundField DataField="UnitQty" HeaderText="<%$Resources:dictionary,Qty %>" SortExpression="UnitQty">
                </asp:BoundField>
                <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary,Outlet %>"
                    SortExpression="OutletName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="literal5" runat="server" Text="<%$Resources:dictionary,No Product Created Yet %>"></asp:Literal>
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
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
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <asp:HiddenField ID="hdPromoCampaignHdrID" runat="server" />
        <table cellpadding="5" cellspacing="0" width="1000" id="FieldsTable1">
            <tr>
                <td colspan="4" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Promo Details %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Promo Code %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtPromoCode" runat="server" Width="172px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPromoCode"
                        ErrorMessage="*Please input Promo Code" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Barcode %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtBarcode" runat="server" Width="172px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Promo Name %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtPromoName" runat="server" Width="172px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPromoName"
                        ErrorMessage="*Please input Promo Name" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Priority%>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtPriority" runat="server" Width="172px" Text="1" onkeydown="return jsDecimals(event);"></asp:TextBox>
                    <asp:Label ID="Literal22" runat="server" Font-Size="Smaller" Text="*No 1 is the highest priority"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtCtrlStartDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPromoName"
                        ErrorMessage="*Please input Start Date" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
                <td style="width: 87px">
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
                </td>
                <td style="width: 243px">
                    <asp:TextBox ID="txtCtrlEndDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPromoName"
                        ErrorMessage="*Please input End Date" SetFocusOnError="True"></asp:RequiredFieldValidator><br />
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Restrict to the hour of the day only%>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:CheckBox ID="cbIsRestrictHourOnly" runat="server" OnCheckedChanged="cbIsRestrictHourOnly_OnCheckedChanged"
                        AutoPostBack="true"></asp:CheckBox>
                    &nbsp;
                </td>
                <td style="width: 87px">
                    <asp:Literal ID="Literal19" runat="server" Text="<%$Resources:dictionary,Start%>"></asp:Literal>
                    <asp:TextBox ID="txtRestrictHourStart" Text="00:00 AM" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td style="width: 243px">
                    <asp:Literal ID="Literal21" runat="server" Text="<%$Resources:dictionary,End %>"></asp:Literal>
                    <asp:TextBox ID="txtRestrictHourEnd" Text="00:00 AM" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal20" runat="server" Text="<%$Resources:dictionary,Days Applicable %>"></asp:Literal>
                </td>
                <td colspan="3" style="background-color: #dddbdc">
                    <asp:CheckBoxList ID="cbDaysApplicable" runat="server" RepeatDirection="Horizontal"
                        Style="background-color: #dddbdc">
                        <asp:ListItem Text="M" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Tu" Value="2" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="W" Value="3" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Th" Value="4" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="F" Value="5" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Sa" Value="6" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Su" Value="7" Selected="True"></asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr style="background-color: #ebebeb">
                <td style="width: 122px">
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary,Applicable to%>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:RadioButton ID="rbAll" GroupName="rbApplicable" runat="server" Text="<%$Resources:dictionary,All %>"
                        Checked="true"></asp:RadioButton>
                </td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rbMemberOnly" GroupName="rbApplicable" runat="server" Text="<%$Resources:dictionary, Member Only%>">
                                </asp:RadioButton>
                            </td>
                            <td id="rowMemberGroup">
                                <asp:DropDownCheckBoxes ID="ddlMemberGroup" runat="server" AddJQueryReference="false" UseButtons="False" UseSelectAllNode="True">
                                    <Texts SelectBoxCaption="Select Member Group" />
                                    <Style2 DropDownBoxBoxWidth="200" SelectBoxWidth="175" />
                                </asp:DropDownCheckBoxes>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary,Item %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    <input type="button" id="btnAddItem" value="Add Item" class="classname" />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="gvDetails" SkinID="scaffold" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="PromoCampaignDetID"
                        OnPageIndexChanging="gvDetails_OnPageIndexChanging" OnRowDataBound="gvDetails_RowDataBound"
                        Width="100%" OnRowDeleting="gvDetails_OnRowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$Resources:dictionary,Edit%>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="editBtn" runat="server" ForeColor="Black" data="" OnClientClick="return false;"
                                        action="edit_detail">Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="true" CausesValidation="false" HeaderText="<%$Resources:dictionary,Delete%>" />
                            <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category%>"
                                ReadOnly="true"></asp:BoundField>
                            <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No%>"
                                ReadOnly="true"></asp:BoundField>
                            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Product Name%>"
                                ReadOnly="true"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$Resources:dictionary, Qty%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblQtyGV" runat="server" Text='<%# Bind("UnitQty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtQtyGV" runat="server" Text='<%# Bind("UnitQty") %>' Width="50px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:dictionary,Any%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblAnyQtyGV" runat="server" Text='<%# Bind("AnyQty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAnyQtyGV" runat="server" Text='<%# Bind("AnyQty") %>' Width="50px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:dictionary,Retail Price%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRetailPriceGV" runat="server" Text='<%# (Convert.ToDecimal(Eval("RetailPrice"))).ToString("N2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:dictionary, Promo Price%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblPromoPriceGV" runat="server" Text='<%# (Convert.ToDecimal(Eval("PromoPrice"))).ToString("N2") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPromoPriceGV" runat="server" Text='<%# Bind("PromoPrice") %>'
                                        Width="50px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Discount (%)">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscPercentGV" runat="server" Text='<%# (Convert.ToDecimal(Eval("DiscPercent"))).ToString("N2") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDiscPercentGV" runat="server" Text='<%# Bind("DiscPercent") %>'
                                        Width="50px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Discount ($)">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscAmountGV" runat="server" Text='<%# (Convert.ToDecimal(Eval("DiscAmount"))).ToString("N2") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDiscAmountGV" runat="server" Text='<%# Bind("DiscAmount") %>'
                                        Width="50px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPromoCampaignDetIDGVDetails" runat="server" Text='<%# Eval("PromoCampaignDetID")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeletedGVDetails" runat="server" Text='<%# Eval("Deleted")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Literal ID="literal5" runat="server" Text="No Promo Details created yet"></asp:Literal></EmptyDataTemplate>
                    </asp:GridView>
                    <asp:Label ID="lblResultDetail" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="wl_pageheaderSub">
                    <input id="chkAllOutlet" type="checkbox" value="true" />              
                    <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                
                    <asp:GridView ID="gvOutlet" SkinID="scaffold" runat="server" AllowSorting="True"
                        AutoGenerateColumns="False" DataKeyNames="OutletName" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" onchange="javascript:outletCheckChange();"  />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" onchange="javascript:outletCheckChange();"  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary,Outlet%>"
                                HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="PointOfSaleName" HeaderText="<%$Resources:dictionary,Point of Sale %>"
                                HtmlEncode="false"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Literal ID="literal5" runat="server" Text="<%$Resources:dictionary,No Outlet %>"></asp:Literal></EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        CausesValidation="true" Text="<%$ Resources:dictionary, Save %>" />
                    &nbsp;
                    <input class="classname" onclick="location.href='SetupProductPromotion.aspx'" type="button"
                        value="Return" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$ Resources:dictionary, Delete %>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div id="tempPanel" runat="server" style="display: none;">
        <asp:Button ID="btnAddDetails" runat="server" OnClick="btnAddDetails_Click" CausesValidation="false" />
        <asp:HiddenField ID="tmpIsEdit" runat="server" />
        <asp:HiddenField ID="tmpDetailId" runat="server" />
        <asp:HiddenField ID="tmpPromoCampaignDetID" runat="server" />
        <asp:HiddenField ID="tmpCategoryName" runat="server" />
        <asp:HiddenField ID="tmpItemNo" runat="server" />
        <asp:HiddenField ID="tmpItemGroup" runat="server" />
        <asp:HiddenField ID="tmpQty" runat="server" />
        <asp:HiddenField ID="tmpAnyQty" runat="server" />
        <asp:HiddenField ID="tmpPromoPrice" runat="server" />
        <asp:HiddenField ID="tmpDiscPercent" runat="server" />
        <asp:HiddenField ID="tmpDiscAmount" runat="server" />
        <asp:HiddenField ID="tmpRetailPrice" runat="server" />
        <asp:HiddenField ID="tmpUnitPrice" runat="server" />
        <asp:HiddenField ID="tmpOutletFrom" runat="server" />
        <asp:HiddenField ID="tmpOutletTo" runat="server" />
        <asp:Button ID="btnCopyPromotion" runat="server" OnClick="btnCopyPromotion_Click"
            CausesValidation="false" />
    </div>
    <div id="dialog-CopyPromoForm" title="Copy Active Promo Between Outlet" style="display: none;">
        <div class="panel" id="panelCopyPromo">
            <fieldset>
                <legend>Copy All Active Promotion</legend>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        From Outlet
                    </div>
                    <select id="ListOutletFrom" data-placeholder="Choose an Outlet" style="width: 400px"
                        class="chosen-select">
                    </select>
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        To Outlet
                    </div>
                    <select id="ListOutletTo" data-placeholder="Choose an Outlet" style="width: 400px"
                        class="chosen-select">
                    </select>
                </div>
            </fieldset>
            &nbsp;<label id="lblWarningCopyPromo" style="display: none; font-size: smaller; color: Red;" /></div>
    </div>
    <div id="dialog-form" title="Add Product" style="display: none;">
        <div class="panel" id="panelUpdateProduct">
            <input type="hidden" id="hdPromoCampaignDetID" value="0" />
            <input type="hidden" id="hdDetailID" value="" />
            <fieldset>
                <legend>Item</legend>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        <input type="radio" id="rbBarcode" name="rbItem" class="element-label" checked="true" />
                        Barcode &nbsp;
                    </div>
                    <input type="text" id="txtBarcodeItem" />
                    &nbsp;
                    <input type="button" id="btnScan" value="Scan" />
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        <input type="radio" id="rbProduct" name="rbItem" />
                        Product
                    </div>
                    <%--<select id="ListItem" data-placeholder="Choose an Item" style="width: 600px" class="chosen-select">
                    </select>--%>
                    <input type="text" id="txtSelectedItem" style="width: 500px" disabled="disabled" />
                    <input type="button" id="btnSearchItem" value="Search" />
                    <input type="hidden" id="hdSelectedItemNo" />
                    <input type="hidden" id="hdSelectedItemName" />
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        <input type="radio" id="rbCategory" name="rbItem" />
                        Category
                    </div>
                    <select id="ListCategory" class="chosen-select" style="width: 600px">
                    </select>
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        <input type="radio" id="rbItemGroup" name="rbItem" />
                        Item Group
                    </div>
                    <select id="ListItemGroup" class="chosen-select" style="width: 600px">
                    </select>
                </div>
            </fieldset>
            <fieldset>
                <legend>Qty</legend>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 220px; float: left;">
                        <div style="width: 150px; float: left;">
                            <label class="element-label" for="Qty">
                                Qty
                            </label>
                        </div>
                        <input type="text" id="txtQty" style="width: 50px; float: left;" onkeypress="javascript: return isMoneyKey(this);" />
                    </div>
                    <div style="width: 220px; float: left;">
                        <div style="width: 150px; float: left;">
                            <label class="element-label" for="Any">
                                Any
                            </label>
                        </div>
                        <input type="text" id="txtAny" style="width: 50px; float: left;" class="input-disabled"
                            onkeypress="javascript: return isMoneyKey(this);" />
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>Price</legend>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Retail Price &nbsp;
                    </div>
                    <input type="hidden" id="hdUnitPrice" />
                    <input type="text" id="txtRetailPrice" readonly="readonly" class="input-disabled" />
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        <input type="radio" id="rbPromoPrice" name="rbPrice" checked="true" />
                        Promo Price
                    </div>
                    <input type="text" id="txtPromoPrice" onkeypress="javascript: return isMoneyKey(this);" />
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        <input type="radio" id="rbDiscPercent" name="rbPrice" />
                        Disc(%)
                    </div>
                    <input type="text" id="txtDiscPercent" onkeypress="javascript: return isMoneyKey(this);" />
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        <input type="radio" id="rbDiscAmount" name="rbPrice" />
                        Disc Amount ($)
                    </div>
                    <input type="text" id="txtDiscAmount" onkeypress="javascript: return isMoneyKey(this);" />
                </div>
            </fieldset>
            &nbsp;<label id="lblWarningItemForm" style="display: none; font-size: smaller; color: Red;" /></div>
    </div>
    <div id="dialog-searchform" title="Search Product" style="display: none;">
        <div class="panel" id="panelSearchProduct">
            <fieldset>
                <legend>Search </legend>
                <div class="form-group" style="padding: 5px;">
                    Search &nbsp;
                    <input type="text" id="txtDialogSearch" style="width: 300px" />
                    &nbsp;
                    <input type="button" id="btnDialogSearch" value="Search" />
                    <input type="hidden" id="SavedFilter" />
                </div>
                <br />
                <div style="overflow: auto; max-height: 300px">
                    <table id="TableSearch" style="width: 100%">
                        <thead>
                            <tr>
                                <th>
                                    Item No
                                </th>
                                <th>
                                    Item Name
                                </th>
                                <th>
                                </th>
                            </tr>
                        </thead>
                        <tbody id="tbodyResult">
                        </tbody>
                        <tbody id="tbodyNotFound" style="display: none">
                            <tr>
                                <td colspan="3" align="center">
                                    <br />
                                    No result found
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div id="divShowLoadMore" style="display: none" style="width: 100%">
                        <button type="button" class="btn btn-default form-control" id="btnLoadMore">
                            Load more...</button>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Chosen/chosen.jquery.min.js")%>"></script>

    <script type="text/javascript">
        $(document).ready(function() {

            app.setting.baseUrl = $('#BaseUrl').val();

            if ($('#ctl00_ContentPlaceHolder1_rbAll').is(":checked"))
                $('#rowMemberGroup').hide();
            else
                $('#rowMemberGroup').show();

            $('#ctl00_ContentPlaceHolder1_rbMemberOnly').change(function() {
                $('#rowMemberGroup').show();
            });

            $('#ctl00_ContentPlaceHolder1_rbAll').change(function() {
                $('#rowMemberGroup').hide();
            });

            $('#chkAllOutlet').change(function() {
                var val = $('#chkAllOutlet:checked').val();
                if (!val)
                    val = false;
                for (var i = 2; i < 100; i++) {
                    var str = "" + i;
                    var pad = "00";
                    var ans = pad.substring(0, pad.length - str.length) + str;
                    var ctrl = $("#ctl00_ContentPlaceHolder1_gvOutlet_ctl" + ans + "_CheckBox1");
                    if (!ctrl.attr('id'))
                        break;
                    ctrl.prop("checked", val);
                }
            });
        });
        outletCheckChange();		
        function outletCheckChange() {

            var isAllChecked = true;
            for (var i = 2; i < 100; i++) {
                var str = "" + i;
                var pad = "00";
                var ans = pad.substring(0, pad.length - str.length) + str;
                var ctrl = $("#ctl00_ContentPlaceHolder1_gvOutlet_ctl" + ans + "_CheckBox1");
                if (!ctrl.attr('id'))
                    break;
                var val = ctrl.prop("checked");
                if (!val) {
                    isAllChecked = false;
                    break;
                }
            }
            $('#chkAllOutlet').prop("checked", isAllChecked);       
        };
               
        
    </script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Promo/promo-group.js")%>"></script>

</asp:Content>
