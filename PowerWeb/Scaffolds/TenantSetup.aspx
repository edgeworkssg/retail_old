<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="TenantSetup.aspx.cs" Inherits="PowerWeb.Scaffolds.TenantSetup" Title="<%$Resources:dictionary, Tenant Setup%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<h2>PointOfSale</h2>--%><ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <asp:Panel ID="pnlGrid" runat="server">
        <table width="800px" id="FilterTable">
            <tr>
                <td colspan="4" class="wl_pageheaderSub">
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="fieldname">
                    <asp:Label ID="lblOutletFilter" runat="server" Text="<%$Resources:dictionary, Outlet%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlFilterOutlet" runat="server" DataTextField="OutletName"
                        DataValueField="OutletName" Width="200px" OnInit="ddlFilterOutlet_Init" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ddlFilterOutlet_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;
                </td>
                <td class="fieldname">
                    <asp:Label ID="lblInterface" runat="server" Text="<%$Resources:dictionary, Interface Validation Status%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlFilterValidationStatus" runat="server" Width="200px">
                        <asp:ListItem Text="<%$Resources:dictionary, ALL%>" Value="ALL" Selected="True" />
                        <asp:ListItem Text="<%$Resources:dictionary, Passed%>" Value="Passed" />
                        <asp:ListItem Text="<%$Resources:dictionary, Pending%>" Value="Pending" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="fieldname">
                    <asp:Label ID="lblShopLevel" runat="server" Text="<%$Resources:dictionary, Shop Level%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlFilterRetailerLevel" runat="server" 
                        AutoPostBack="True" OnInit="ddlFilterRetailerLevel_Init" 
                        OnSelectedIndexChanged="ddlFilterRetailerLevel_SelectedIndexChanged" 
                        Width="200px">
                    </asp:DropDownList>
                </td>
                <td class="fieldname">
                    <asp:Label ID="lblOutletFilter2" runat="server" Text="<%$Resources:dictionary, Shop No%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlFilterShopNo" runat="server" 
                        OnInit="ddlFilterShopNo_Init" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="fieldname">
                    <asp:Label ID="lblOutletFilter4" runat="server" Text="<%$Resources:dictionary, Search%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblOutletFilter3" runat="server" Text="<%$Resources:dictionary, Business Expiration%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlBusinessExpiration" runat="server" Width="200px">
                        <asp:ListItem Text="<%$Resources:dictionary, ALL%>" Value="ALL" />
                        <asp:ListItem Text="<%$Resources:dictionary, Active%>" Value="Active"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:dictionary, Expired%>" Value="Expired" />
                        <asp:ListItem Text="<%$Resources:dictionary, 1 Month%>" Value="1 Month" />
                        <asp:ListItem Text="<%$Resources:dictionary, 2 Month%>" Value="2 Month" />
                        <asp:ListItem Text="<%$Resources:dictionary, 3 Month%>" Value="3 Month" />
                        <asp:ListItem Text="<%$Resources:dictionary, 6 Month%>" Value="6 Month" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="fieldname">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:CheckBox ID="chkShowDeletedItems" runat="server" 
                        Text="<%$Resources:dictionary, Show Deleted Items%>" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <input id="btnAddNew" runat="server" class="classname" onclick="location.href='TenantSetup.aspx?id=0'" 
                        type="button" value="<%$Resources:dictionary, Add New%>" oninit="btnAddNew_Init" />&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="height: 15px">
                    <asp:Button ID="btnSearch" runat="server" CssClass="classname" 
                        OnClick="btnSearch_Click" Text="<%$ Resources:dictionary, Search %>" />
                    <asp:Button ID="btnClear" runat="server" CssClass="classname" 
                        OnClick="btnClear_Click" Text="<%$ Resources:dictionary, Clear %>" />
                    &nbsp;
                </td>
                <td align="right" colspan="2">
                    <asp:LinkButton ID="lnkExport" runat="server" CssClass="classBlue" 
                        OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="PointOfSaleID"
            PageSize="50" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="PointOfSaleID" DataNavigateUrlFormatString="TenantSetup.aspx?id={0}" />
                <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary, Outlet%>" SortExpression="OutletName" />
                <asp:BoundField DataField="ShopLevel" HeaderText="<%$Resources:dictionary, Shop Level%>" SortExpression="ShopLevel" />
                <asp:BoundField DataField="ShopNo" HeaderText="<%$Resources:dictionary, Shop No%>" SortExpression="ShopNo" />
                <asp:BoundField DataField="TenantCode" HeaderText="<%$Resources:dictionary, Tenant Code%>" SortExpression="TenantCode" />
                <asp:BoundField DataField="TenantName" HeaderText="<%$Resources:dictionary, Tenant Name%>" SortExpression="TenantName" />
                <asp:BoundField DataField="TenantDescription" HeaderText="<%$Resources:dictionary, Tenant Description%>" SortExpression="TenantDescription" />
                <asp:BoundField DataField="BusinessStartDate" HeaderText="<%$Resources:dictionary, Business Start Date%>" SortExpression="BusinessStartDate"
                    DataFormatString="{0:dd MMM yyyy}" />
                <asp:BoundField DataField="BusinessEndDate" HeaderText="<%$Resources:dictionary, Business End Date%>" SortExpression="BusinessEndDate"
                    DataFormatString="{0:dd MMM yyyy}" />
                <asp:BoundField DataField="InterfaceValidationStatus" HeaderText="<%$Resources:dictionary, Interface Validation Status%>"
                    SortExpression="InterfaceValidationStatus" />
                <asp:BoundField DataField="RetailerContactNo" HeaderText="<%$Resources:dictionary, Retailer Contact No%>" SortExpression="RetailerContactNo" />
                <asp:BoundField DataField="RetailerContactPerson" HeaderText="<%$Resources:dictionary, Retailer Contact Person%>"
                    SortExpression="RetailerContactPerson" />
                <asp:BoundField DataField="RetailerEmail" HeaderText="<%$Resources:dictionary, Retailer Email%>" SortExpression="RetailerEmail" />
                <asp:CheckBoxField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>" SortExpression="Deleted" />
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Tenant%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="Literal29"  runat="server" Text="<%$Resources:dictionary, of%>" />
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Next >%>" CommandArgument="Next"
                        CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Last >>%>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="800px">
            <tr id="trOutletName" runat="server">
                <td class="style4">
                    <asp:Label ID="lblOutlet" runat="server" Text="<%$Resources:dictionary, Outlet Name%>" />
                </td>
                <td class="style4">
                    <asp:DropDownList ID="ddlOutlet" runat="server" Width="175px" 
                        AutoPostBack="True" onselectedindexchanged="ddlOutlet_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trRetailerLevel" runat="server">
                <td>
                    <asp:Literal ID="Literal28"  runat="server" Text="<%$Resources:dictionary, Shop Level%>" /></td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlRetailerLevel" runat="server" Width="175px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlRetailerLevel_SelectedIndexChanged" />
                            </td>
                            <td>
                                <asp:Literal ID="Literal27"  runat="server" Text="<%$Resources:dictionary, Shop No %>" />&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlShopNo" runat="server" Width="175px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trBusinessStartDate" runat="server">
                <td>
                    <asp:Literal ID="Literal26"  runat="server" Text="<%$Resources:dictionary, Business Start/End Date%>" />
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
                                    PopupButtonID="imgBusinessStartDate" TargetControlID="txtBusinessStartDate">
                                </cc1:CalendarExtender>
                                <asp:TextBox ID="txtBusinessStartDate" runat="server" Width="175px"></asp:TextBox>
                                <asp:ImageButton ID="imgBusinessStartDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
                                    PopupButtonID="imgBusinessEndDate" TargetControlID="txtBusinessEndDate">
                                </cc1:CalendarExtender>
                                <asp:TextBox ID="txtBusinessEndDate" runat="server" Width="175px"></asp:TextBox>
                                <asp:ImageButton ID="imgBusinessEndDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPOSID" runat="server" Text="<%$Resources:dictionary, Point Of Sale ID%>" />
                </td>
                <td>
                    <asp:Label ID="lblPOSPrefix" runat="server" />
                    <asp:Label ID="lblID" runat="server" />
                </td>
            </tr>
            <tr id="trTenantMachineID" runat="server">
                <td>
                    <asp:Label ID="lblTenantCode" runat="server" Text="<%$Resources:dictionary, Tenant Code%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtTenantMachineID" runat="server" Width="175px" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr id="trPOSName" runat="server">
                <td>
                    <asp:Label ID="lblPointOfSaleName" runat="server" Text="<%$Resources:dictionary, Point Of Sale Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPointOfSale" runat="server" MaxLength="50" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trTenantCompanyName" runat="server">
                <td>
                    <asp:Label ID="lblTenantCompany" runat="server" Text="<%$Resources:dictionary, Tenant Company Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtTenantCompanyName" runat="server" MaxLength="50" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trPOSDesc" runat="server">
                <td>
                    <asp:Label ID="lblPOSDesc" runat="server" Text="<%$Resources:dictionary, Point Of Sale Description%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPointOfSaleDesc" runat="server" MaxLength="250" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trDepartment" runat="server" visible="false">
                <td class="style5">
                    <asp:Literal ID="Literal25"  runat="server" Text="<%$Resources:dictionary, Department%>" />
                </td>
                <td class="style5">
                    <asp:DropDownList ID="ddlDept" runat="server" Width="175px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trQuickAccessGroup" runat="server" visible="false">
                <td>
                    <asp:Literal ID="Literal24"  runat="server" Text="<%$Resources:dictionary, Quick Access Group%>" />
                </td>
                <td>
                    <subsonic:DropDown TableName="QuickAccessGroup" OrderField="QuickAccessGroupName"
                        runat="server" ID="ddlQuickAccess" ValueField="QuickAccessGroupID" TextField="QuickAccessGroupName"
                        Width="175px">
                    </subsonic:DropDown>
                </td>
            </tr>
            <tr id="trPhoneNo" runat="server" visible="false">
                <td>
                    <asp:Literal ID="Literal23"  runat="server" Text="<%$Resources:dictionary, Phone No%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="15" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trMembershipCode" runat="server" visible="false">
                <td>
                    <asp:Literal ID="Literal22"  runat="server" Text="<%$Resources:dictionary, Membership Code%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtMembershipCode" runat="server" MaxLength="15" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trPriceScheme" runat="server" visible="false">
                <td>
                    <asp:Literal ID="Literal21"  runat="server" Text="<%$Resources:dictionary, Price Scheme ID%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPriceSchemeID" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trAPIKey" runat="server">
                <td>
                    <asp:Literal ID="Literal20"  runat="server" Text="<%$Resources:dictionary, API Key%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtAPIKey" runat="server" Width="175px" ReadOnly="True"></asp:TextBox>
                    &nbsp;
                    <asp:Button ID="btnGenerateAPIKey" runat="server" Text="Generate" OnClick="btnGenerateAPIKey_Click"
                        Width="80px" CssClass="classname" />
                </td>
            </tr>
            <tr id="trInterfaceValidationStatus" runat="server">
                <td>
                    <asp:Literal ID="Literal19"  runat="server" Text="<%$Resources:dictionary, Interface Validation Status%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlValidationStatus" runat="server" Width="175px">
                        <asp:ListItem Value="Pending" Text="<%$Resources:dictionary, Pending%>" Selected="True" />
                        <asp:ListItem Value="Passed" Text="<%$Resources:dictionary, Passed%>" />
                        <asp:ListItem Value="Failed" Text="<%$Resources:dictionary, Failed%>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trRetailerContactPerson" runat="server">
                <td>
                    <asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, Retailer Contact Person%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtRetailerContactPerson" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trRetailerContactNo" runat="server">
                <td>
                    <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Retailer Contact No%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtRetailerContactNo" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trRetailerEmail" runat="server">
                <td>
                    <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, Retailer Email%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtRetailerEmail" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trRetailerDesignation" runat="server">
                <td>
                    <asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, Retailer Designation%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtRetailerDesignation" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trInterfaceDevTeam" runat="server">
                <td class="style4">
                    <asp:Literal ID="Literal14"  runat="server" Text="<%$Resources:dictionary, Interface Dev Team%>" />
                </td>
                <td class="style4">
                    <asp:DropDownList ID="ddlInterfaceDevTeam" runat="server" Width="175px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trVendorContactPersonName" runat="server">
                <td>
                    <asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Vendor Contact Person Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtVendorContactName" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trVendorContactNo" runat="server">
                <td>
                    <asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, Vendor Contact No%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtVendorContactNo" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trVendorEmail" runat="server">
                <td>
                    <asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Vendor Email%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtVendorEmail" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trPOSType" runat="server">
                <td>
                    <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, POS Type%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlPOSType" runat="server" Width="175px">
                        <asp:ListItem Value="Cash Register" Text="<%$Resources:dictionary, Cash Register%>"></asp:ListItem>
                        <asp:ListItem Value="POS" Text="<%$Resources:dictionary, POS%>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trPOSBrand" runat="server">
                <td>
                    <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, POS Brand%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPOSBrand" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trPOSOS" runat="server">
                <td>
                    <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, POS OS%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPOSOS" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trPOSSoftware" runat="server">
                <td>
                    <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, POS Software%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPOSSoftware" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trNoOfPOS" runat="server">
                <td>
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, No Of POS%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtNoOfPOS" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trOption" runat="server">
                <td>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Option%>" />
                </td>
                <td>
                    <asp:RadioButtonList ID="rblOption" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem>A</asp:ListItem>
                        <asp:ListItem>B</asp:ListItem>
                        <asp:ListItem>C</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trDeleted" runat="server">
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Deleted%>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" />
                </td>
            </tr>
            <tr id="trCreatedOn" runat="server" visible="false">
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Created On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trCreatedBy" runat="server" visible="false">
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Created By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trModifiedOn" runat="server" visible="false">
                <td>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Modified On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trModifiedBy" runat="server" visible="false">
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Modified By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$Resources:dictionary, Save%>" />
                    &nbsp;<input id="btnReturn" runat="server" class="classname" onclick="location.href='TenantSetup.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headContent">
    <style type="text/css">
        .style4
        {
            height: 32px;
        }
        .style5
        {
            height: 20px;
        }
    </style>
</asp:Content>
