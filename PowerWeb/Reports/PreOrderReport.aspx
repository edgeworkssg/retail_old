<%@ Page Title="<%$Resources:dictionary, Pre Order Report%>" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="PreOrderReport.aspx.cs" Inherits="PowerWeb.Reports.PreOrderReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'DeliveryStatus', 'height=500,width=800,resizeable=1,scrollbars=1');
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
    <table width="700px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
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
                <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Item %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtItem" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Customer Name%>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtCustName" runat="server" Width="173px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary, Outstanding Bal%>"></asp:Literal>
            </td>
            <td>
                <asp:RadioButtonList ID="rblOutstandingBal" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Text="<%$Resources:dictionary, All%>" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:dictionary, Yes%>" Value="YES"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:dictionary, No%>" Value="NO"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Notification Status%>"></asp:Literal>
            </td>
            <td>
                <asp:RadioButtonList ID="rblNotification" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Text="<%$Resources:dictionary, All%>" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:dictionary, Yes%>" Value="YES"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:dictionary, No%>" Value="NO"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary, DeliveryStatus%>"></asp:Literal>
            </td>
            <td>
                <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Text="<%$Resources:dictionary, All%>" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:dictionary, Delivered%>" Value="Delivered"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:dictionary, Not Delivered%>" Value="Not Delivered"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                
            </td>
            <td>
                
            </td>
        </tr>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 700px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton></td><td align="right" style="height: 30px; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="LinkButton2" class="classBlue" runat="server" OnClick="lnkReadyToDeliver_Click">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Show Item Ready To Deliver %>" /></asp:LinkButton><asp:LinkButton ID="LinkButton5" class="classBlue" runat="server" OnClick="lnkDeliver_Click"
                    OnClientClick="javascript:return confirm('Are you sure you want to create Delivery Order for the selected item(s)?')">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary, Deliver%>" /></asp:LinkButton><asp:LinkButton ID="LinkButton6" class="classBlue" runat="server" OnClick="lnkNotify_Click"
                    OnClientClick="javascript:return confirm('Are you sure you want to send notification for the selected item(s)?')">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary, Notify%>" /></asp:LinkButton><asp:LinkButton ID="LinkButton7" class="classBlue" runat="server" OnClick="lnkDelivered_Click"
                    OnClientClick="javascript:return confirm('Are you sure you want to mark the selected item(s) as Delivered?')">
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary, Delivered%>" /></asp:LinkButton><asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton></td></tr><tr>
            <td colspan="4">
                <div style="color: Green"><asp:Literal ID="litSuccessMsg" runat="server" Text="" /></div>
                <div style="color: Red"><asp:Literal ID="litErrorMsg" runat="server" Text="" /></div>
            </td>
        </tr>
    </table>
    
    <asp:GridView ID="gvReport" Width="100%" runat="server" PageSize="20" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="<%$Resources:dictionary, Select%>">
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OrderDate" SortExpression="OrderDate" HeaderText="<%$Resources:dictionary, Date%>"
                DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" />
            <asp:TemplateField HeaderText="<%$Resources:dictionary, Preorder #%>" SortExpression="InvoiceNo">
                <ItemTemplate>
                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>" />
            <asp:TemplateField HeaderText="<%$Resources:dictionary, Item Name%>" SortExpression="ItemName">
                <ItemTemplate>
                    <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'>
                 </asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MembershipNo" SortExpression="MembershipNo" HeaderText="<%$Resources:dictionary, Membership No%>" />
            <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="<%$Resources:dictionary, Customer Name%>" />
            <asp:BoundField DataField="MobileNo" SortExpression="MobileNo" HeaderText="<%$Resources:dictionary, Mobile No%>" />
            <asp:BoundField DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="<%$Resources:dictionary, Price%>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="OrderQty" SortExpression="OrderQty" HeaderText="<%$Resources:dictionary, OrderQty%>" />
            <asp:TemplateField HeaderText="<%$Resources:dictionary, On Hand Qty%>" SortExpression="QtyOnHand">
                <ItemTemplate>
                    <asp:Label ID="lblQtyOnHand" runat="server" Text='<%# Eval("QtyOnHand") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:BoundField DataField="TotalPreOrderQty" SortExpression="TotalPreOrderQty" HeaderText="<%$Resources:dictionary, Pre Order Qty%>" />
            <asp:BoundField DataField="BalanceQty" SortExpression="BalanceQty" HeaderText="<%$Resources:dictionary, Balance Qty%>" />
            <asp:TemplateField HeaderText="<%$Resources:dictionary, Delivery Qty%>">
                <ItemTemplate>
                    <asp:TextBox ID="txtDeliveryQty" runat="server" Width="50px"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="<%$Resources:dictionary, Outstanding Qty%>" SortExpression="OutstandingQty">
                <ItemTemplate>
                    <asp:Label ID="lblOutstandingQty" runat="server" Text='<%# String.Format("{0:N0}", Eval("OutstandingQty")) %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:BoundField DataField="IsNotified" SortExpression="IsNotified" HeaderText="<%$Resources:dictionary, Notified%>" />
            <asp:TemplateField HeaderText="<%$Resources:dictionary, Delivery Status%>" SortExpression="DeliveryStatus">
                <ItemTemplate>
                    <asp:Label ID="lblDeliveryStatus" runat="server" Text='<%# Eval("DeliveryStatus") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="<%$Resources:dictionary, Partial Delivery%>">
                <ItemTemplate>
                    <a class="classname" href="javascript:poptastic('../Sales/TrackDelivery.aspx?RefNo=<%# Eval("OrderDetID")%>');"><asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, status%>" /></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DeliveredDate" SortExpression="DeliveredDate" HeaderText="<%$Resources:dictionary, Delivered%>" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary, Outlet%>"  />
            <asp:BoundField DataField="OutstandingBal" SortExpression="OutstandingBal" HeaderText="<%$Resources:dictionary, Balance Payment%>" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="PaidAmount" SortExpression="PaidAmount" HeaderText="<%$Resources:dictionary, Paid Amount%>" DataFormatString="{0:N2}" />
            <asp:TemplateField HeaderText="<%$Resources:dictionary, OrderHdrID%>" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblOrderHdrID" runat="server" Text='<%# Eval("OrderHdrID") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="<%$Resources:dictionary, OrderDetID%>" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblOrderDetID" runat="server" Text='<%# Eval("OrderDetID") %>'></asp:Label></ItemTemplate></asp:TemplateField></Columns><PagerTemplate>
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
