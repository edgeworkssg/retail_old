<%@ Page Language="C#" Title="<%$Resources:dictionary, Update Delivery Details%>" Inherits="PowerPOS.UpdateDeliveryDetails"
    MasterPageFile="~/PowerPOSMst.master" Theme="default" CodeBehind="UpdateDeliveryDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldDeliveryDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="ctrlDeliveryDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldSearchInvDateFrom" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="imbSearchInvDateFrom" TargetControlID="txtSearchInvDateFrom">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldSearchInvDateTo" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="imbSearchInvDateTo" TargetControlID="txtSearchInvDateTo">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldSearchDODateFrom" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="imbSearchDODateFrom" TargetControlID="txtSearchDODateFrom">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldSearchDODateTo" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="imbSearchDODateTo" TargetControlID="txtSearchDODateTo">
    </cc1:CalendarExtender>
    <asp:Panel ID="pnlGrid" runat="server">
        <table id="FilterTable" width="700px">
            <tr>
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Customer Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtSearchCustName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <%--<tr>
                <td>
                    Customer No
                </td>
                <td>
                    <asp:TextBox ID="txtSearchCustNo" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Customer Contact No%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtSearchContactNo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary,Invoice No %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtSearchInvoiceNo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary,Outlet %>" />
                </td>
                <td>
                    <subsonic:DropDown ID="ddlSearchOutlet" runat="server" ShowPrompt="True" TableName="Outlet"
                        ValueField="OutletName" PromptText="ALL">
                    </subsonic:DropDown>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Invoice Date%>" />
                </td>
                <td>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Start Date%>" />
                    <asp:TextBox ID="txtSearchInvDateFrom" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="imbSearchInvDateFrom" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                        Style="height: 16px; width: 16px" />
                    &nbsp;&nbsp;&nbsp; <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, End Date%>" />
                    <asp:TextBox ID="txtSearchInvDateTo" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="imbSearchInvDateTo" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                        Style="height: 16px; width: 16px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary,Delivery Date %>" />
                </td>
                <td>
                    <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, Start Date%>" />
                    <asp:TextBox ID="txtSearchDODateFrom" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="imbSearchDODateFrom" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                        Style="height: 16px; width: 16px" />
                    &nbsp;&nbsp;&nbsp; <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, End Date%>" />
                    <asp:TextBox ID="txtSearchDODateTo" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="imbSearchDODateTo" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                        Style="height: 16px; width: 16px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Delivery Details%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlSearchDeliveryDetails" runat="server">
                        <asp:ListItem Value="ALL" Text="<%$Resources:dictionary, ALL%>"></asp:ListItem>
                        <asp:ListItem Value="ASSIGNED" Text="<%$Resources:dictionary, Assigned%>"></asp:ListItem>
                        <asp:ListItem Value="UNASSIGNED" Text="<%$Resources:dictionary, Unassigned%>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div style="height: 5px;">
        </div>
        <asp:Button ID="btnSearch" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Search%>" OnClick="btnSearch_Click">
        </asp:Button>
        <%--<input class="classname" onclick="location.href='UpdateDeliveryDetails.aspx?id=0'"
            type="button" value="Add New" />--%>
        <div style="height: 20px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="OrderNumber"
            PageSize="50" OnRowDataBound="GridView1_RowDataBound" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="OrderNumber" DataNavigateUrlFormatString="UpdateDeliveryDetails.aspx?id={0}" />
                <asp:BoundField DataField="CustomInvoiceNo" HeaderText="<%$Resources:dictionary, Invoice No%>" SortExpression="CustomInvoiceNo">
                </asp:BoundField>
                <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary, Outlet%>" SortExpression="OutletName">
                </asp:BoundField>
                <asp:BoundField DataField="OrderDate" HeaderText="<%$Resources:dictionary, Invoice Date%>" SortExpression="OrderDate"
                    DataFormatString="{0:dd MMM yyyy HH:mm:ss}"></asp:BoundField>
                <asp:BoundField DataField="PurchaseOrderRefNo" HeaderText="<%$Resources:dictionary, Delivery No%>" SortExpression="PurchaseOrderRefNo">
                </asp:BoundField>
                <asp:BoundField DataField="RecipientName" HeaderText="<%$Resources:dictionary, Recipient Name%>" SortExpression="RecipientName">
                </asp:BoundField>
                <asp:BoundField DataField="MobileNo" HeaderText="<%$Resources:dictionary, Mobile No%>" SortExpression="MobileNo">
                </asp:BoundField>
                <asp:BoundField DataField="HomeNo" HeaderText="<%$Resources:dictionary, Home No%>" SortExpression="HomeNo">
                </asp:BoundField>
                <asp:TemplateField HeaderText="<%$Resources:dictionary, Delivery Address%>" SortExpression="DeliveryAddress">
                    <ItemTemplate>
                        <asp:Literal ID="litDeliveryAddress" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:dictionary, Delivery Date & Time%>" SortExpression="DeliveryDate">
                    <ItemTemplate>
                        <asp:Literal ID="litDeliveryDate" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary, Remark%>" SortExpression="Remark"></asp:BoundField>
                <%--<asp:BoundField DataField="DeliveryAddress" HeaderText="Delivery Address" SortExpression="DeliveryAddress">
                </asp:BoundField>
                <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date & Time" SortExpression="DeliveryDate">
                </asp:BoundField>
                <asp:BoundField DataField="TimeSlotFrom" HeaderText="Time Slot From" SortExpression="TimeSlotFrom">
                </asp:BoundField>
                <asp:BoundField DataField="TimeSlotTo" HeaderText="Time Slot To" SortExpression="TimeSlotTo">
                </asp:BoundField>
                <asp:BoundField DataField="SalesOrderRefNo" HeaderText="Sales Order Ref No" SortExpression="SalesOrderRefNo">
                </asp:BoundField>
                <asp:BoundField DataField="PurchaseOrderRefNo" HeaderText="Purchase Order Ref No"
                    SortExpression="PurchaseOrderRefNo"></asp:BoundField>
                <asp:BoundField DataField="MembershipNo" HeaderText="Membership No" SortExpression="MembershipNo">
                </asp:BoundField>
                <asp:BoundField DataField="PostalCode" HeaderText="Postal Code" SortExpression="PostalCode">
                </asp:BoundField>
                <asp:BoundField DataField="UnitNo" HeaderText="Unit No" SortExpression="UnitNo">
                </asp:BoundField>--%>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr01"  runat="server" Text="<%$Resources:dictionary, No Delivery Order%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="ltr011"  runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ltr0112"  runat="server" Text="<%$Resources:dictionary, of%>" />
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
        <table id="FieldsTable" cellpadding="5" cellspacing="0">
            <tr>
                <td>
                    <asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary,Invoice No %>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlInvoiceNo" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, Outlet%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlOutlet" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Invoice Date%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlInvoiceDate" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal14"  runat="server" Text="<%$Resources:dictionary, Delivery Order No%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlPurchaseOrderRefNo" runat="server" Enabled="false"></asp:TextBox>
                    <asp:Label ID="lblID" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, Recipient Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlRecipientName" runat="server" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, Mobile No%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlMobileNo" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Home No%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlHomeNo" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, Postal Code%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlPostalCode" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:Button ID="btnPostalCode" runat="server" Text="Update Address" OnClick="btnPostalCode_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal19"  runat="server" Text="<%$Resources:dictionary, Delivery Address%>" />
                </td>
                <td>
                    <ajax:UpdatePanel ID="upnlDeliveryAddress" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="ctrlDeliveryAddress" runat="server" TextMode="MultiLine" Height="100px"
                                Width="500px" MaxLength="2147483647">
                            </asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <ajax:AsyncPostBackTrigger ControlID="btnPostalCode" EventName="Click" />
                        </Triggers>
                    </ajax:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal20"  runat="server" Text="<%$Resources:dictionary, Unit No%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlUnitNo" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal21"  runat="server" Text="<%$Resources:dictionary, Delivery Date%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlDeliveryDate" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                        Style="height: 16px; width: 16px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal22"  runat="server" Text="<%$Resources:dictionary, Delivery Time%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ctrlDeliveryTime" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal23"  runat="server" Text="<%$Resources:dictionary, Remark%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlRemark" runat="server" TextMode="MultiLine" Height="100px" Width="500px"
                        MaxLength="2147483647"></asp:TextBox>
                </td>
            </tr>
            <%-- <tr>
                <td class="scaffoldEditItemCaption">
                    Person Assigned
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlPersonAssigned" runat="server" Width="50px"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td class="scaffoldEditItemCaption">
                    Time Slot From
                </td>
                <td class="scaffoldEditItem">
                    <subsonic:CalendarControl ID="ctrlTimeSlotFrom" runat="server"></subsonic:CalendarControl>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    Time Slot To
                </td>
                <td class="scaffoldEditItem">
                    <subsonic:CalendarControl ID="ctrlTimeSlotTo" runat="server"></subsonic:CalendarControl>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    Status
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlStatus" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    Sales Order Ref No
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="ctrlSalesOrderRefNo" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td class="scaffoldEditItemCaption">
                    Deleted
                </td>
                <td class="scaffoldEditItem">
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    Created On
                </td>
                <td class="scaffoldEditItem">
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    Modified On
                </td>
                <td class="scaffoldEditItem">
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    Created By
                </td>
                <td class="scaffoldEditItem">
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    Modified By
                </td>
                <td class="scaffoldEditItem">
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption">
                    Membership No
                </td>
                <td class="scaffoldEditItem">
                    <asp:DropDownList ID="ctrlMembershipNo" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            --%>
        </table>
        <asp:GridView ID="GridView2" runat="server" AllowPaging="false" AllowSorting="False"
            Width="100%" AutoGenerateColumns="False" DataKeyNames="DetailsID" PageSize="50"
            SkinID="scaffold">
            <Columns>
                <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>"></asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Item Name%>"></asp:BoundField>
                <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:dictionary, Quantity%>"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Delivery Order%>" />
            </EmptyDataTemplate>
        </asp:GridView>
        <br />
        <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Save%>" OnClick="btnSave_Click">
        </asp:Button>&nbsp;
        <input id="btnReturn" runat="server" type="button" onclick="location.href='UpdateDeliveryDetails.aspx'" class="classname"
            value="<%$Resources:dictionary, Return%>" />
        <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
            Text="<%$Resources:dictionary, Cancel Delivery%>" OnClick="btnDelete_Click"></asp:Button>
    </asp:Panel>
</asp:Content>
