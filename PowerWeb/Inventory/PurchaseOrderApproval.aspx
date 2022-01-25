<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="PurchaseOrderApproval.aspx.cs" Inherits="PowerWeb.Inventory.PurchaseOrderApproval"
    Title="<%$Resources:dictionary, Purchase Order Approval%>" %>

<asp:Content ID="head" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        div.vertical-line
        {
            width: 1px; /* Line width */
            background-color: black; /* Line color */
            height: 100%; /* Override in-line if you want specific height. */
            float: left; /* Causes the line to float to left of content. 
    You can instead use position:absolute or display:inline-block
    if this fits better with your design */
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=500,width=800,resizeable=1,scrollbars=1');
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
    <table width="800px" id="FieldsTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width: 102px; height: 3px">
                <asp:Literal ID="rdbRange" runat="server" Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server" Width="150px"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td style="height: 3px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server" Width="150px"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltr01" runat="server" Text="<%$Resources:dictionary, User%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlUser" runat="server" Width="150px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Location%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlLocation" runat="server" Width="150px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="height: 19px">
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Supplier%>" />
            </td>
            <td style="height: 19px">
                <asp:DropDownList ID="ddlSupplier" runat="server" Width="150px">
                </asp:DropDownList>
            </td>
            <td style="height: 19px">
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Remarks%>" />
            </td>
            <td style="height: 19px">
                <asp:TextBox ID="txtRemarks" runat="server" Width="150px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary, Status%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                    <asp:ListItem Value="ALL" Text="ALL" />
                    <asp:ListItem Value="Submitted" Text="Submitted" />
                    <asp:ListItem Value="Approved" Text="Approved" />
                    <asp:ListItem Value="Rejected" Text="Rejected" />
                    <asp:ListItem Value="Partially Received" Text="Partially Received" />
                    <asp:ListItem Value="Canceled" Text="Canceled" />
                    <asp:ListItem Value="Open" Text="Open" />
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, PO Number%>" />
            </td>
            <td>
                <asp:TextBox ID="txtPONumber" runat="server" Width="150px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;<asp:Button ID="btnSearch" class="classname" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" class="classname" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td colspan="2" align="right" class="ExportButton">
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvPurchaseOrder" ShowFooter="True" Width="100%" runat="server"
        AllowPaging="True" AllowSorting="false" DataKeyNames="PurchaseOrderHdrRefNo"
        AutoGenerateColumns="False" SkinID="scaffold" PageSize="20" OnDataBound="gvPurchaseOrder_DataBound"
        OnPageIndexChanging="gvPurchaseOrder_PageIndexChanging" OnRowCommand="gvPurchaseOrder_RowCommand"
        OnRowDataBound="gvPurchaseOrder_RowDataBound" OnSorting="gvPurchaseOrder_Sorting"
        OnRowCreated="gvPurchaseOrder_RowCreated">
        <Columns>
            <asp:BoundField DataField="No" SortExpression="No" HeaderText="<%$Resources:dictionary, No%>" />
            <asp:BoundField DataField="PurchaseOrderDate" SortExpression="PurchaseOrderDate"
                HeaderText="<%$Resources:dictionary, Date%>" />
            <asp:BoundField DataField="CustomRefNo" SortExpression="CustomRefNo" HeaderText="<%$Resources:dictionary, Ref No%>" />
            <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="<%$Resources:dictionary, User%>" />
            <asp:BoundField DataField="InventoryLocationName" SortExpression="InventoryLocationName"
                HeaderText="<%$Resources:dictionary, Location%>" />
            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$Resources:dictionary, Remark%>" />
            <asp:BoundField DataField="SupplierName" SortExpression="SupplierName" HeaderText="<%$Resources:dictionary, Supplier%>" />
            <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$Resources:dictionary, Status%>" />
            <asp:CheckBoxField DataField="IsEmailSent" SortExpression="IsEmailSent"" HeaderText="<%$Resources:dictionary, Is Email Sent%>" />
            <asp:BoundField DataField="TotalAmount" SortExpression="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
            <asp:TemplateField HeaderText="" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <div id="divEdit" runat="server">
                        <a class="classname" id="hlEdit" href="javascript:poptastic('EditPO.aspx?RefNo=<%# Eval("PurchaseOrderHdrRefNo")%>');">
                            <asp:Literal ID="ltr02" runat="server" Text="<%$Resources:dictionary, Edit%>" />
                        </a>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <asp:Button ID="btnApprove" runat="server" class="classname" Text="<%$Resources:dictionary, Approve%>"
                        CommandName="Approve" CommandArgument='<%# Eval("PurchaseOrderHdrRefNo") %>'
                        OnClientClick="return confirm('Are you sure you want to approve this PO?')" />
                    <asp:Button ID="btnClose" runat="server" class="classname" Text="<%$Resources:dictionary, Close%>"
                        CommandName="Close" CommandArgument='<%# Eval("PurchaseOrderHdrRefNo") %>'
                        OnClientClick="return confirm('Are you sure you want to close this PO?')" />
                    <div id="divMail" runat="server">
                        <a class="classname" id="hlEdit" href="javascript:poptastic('MailPO.aspx?RefNo=<%# Eval("PurchaseOrderHdrRefNo")%>');">
                            <asp:Literal ID="ltr03" runat="server" Text="<%$Resources:dictionary, Email To Supplier%>" />
                        </a>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <asp:Button ID="btnReject" runat="server" class="classname" Text="<%$Resources:dictionary, Reject%>"
                        CommandName="Reject" CommandArgument='<%# Eval("PurchaseOrderHdrRefNo") %>' OnClientClick="return confirm('Are you sure you want to reject this PO?')" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <asp:Button ID="btnCancel" runat="server" class="classname" Text="<%$Resources:dictionary, Cancel%>"
                        CommandName="CancelPO" CommandArgument='<%# Eval("PurchaseOrderHdrRefNo") %>'
                        OnClientClick="return confirm('Are you sure you want to cancel this PO?')" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <a class="classname" id="HyperLink1" href="javascript:poptastic('POAttachment.aspx?RefNo=<%# Eval("PurchaseOrderHdrRefNo")%>');">
                        <asp:Literal ID="litAttachment" runat="server" Text="Attachment" />
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action" ItemStyle-Width="130px" HeaderStyle-Width="130px">
                <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <a class="classname" id="HyperLink1" href="javascript:poptastic('PrintPurchaseOrder.aspx?RefNo=<%# Eval("PurchaseOrderHdrRefNo")%>');">
                        <asp:Literal ID="ltr04" runat="server" Text="<%$Resources:dictionary, View/Print PO%>" />
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
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
</asp:Content>
