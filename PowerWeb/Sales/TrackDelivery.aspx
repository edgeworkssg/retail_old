<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrackDelivery.aspx.cs" Inherits="PowerWeb.Sales.TrackDelivery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Track Delivery</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h4>
            Delivery Status for 
            <asp:Literal ID="litItem" runat="server"></asp:Literal>
            (Preorder# 
            <asp:Literal ID="litInvNo" runat="server"></asp:Literal>)
        </h4>
        <div style="color: Green"><asp:Literal ID="litSuccessMsg" runat="server" Text="" /></div>
        <div style="color: Red"><asp:Literal ID="litErrorMsg" runat="server" Text="" /></div>
        <asp:GridView ID="gvReport" Width="100%" runat="server" PageSize="20" AllowPaging="True"
            AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
            OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
            SkinID="scaffold" onrowcommand="gvReport_RowCommand">
            <Columns>
                <asp:BoundField DataField="DeliveryNo" SortExpression="DeliveryNo" HeaderText="No" />
                <asp:BoundField DataField="DeliveryDate" SortExpression="DeliveryDate" HeaderText="Delivery Date"
                    DataFormatString="{0:dd MMM yyyy}" />
                <asp:BoundField DataField="DeliveryQty" SortExpression="DeliveryQty" HeaderText="Delivery Qty" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="DeliveryStatus" SortExpression="DeliveryStatus" HeaderText="Delivery Status" />
                <asp:TemplateField HeaderText="Delivered">
                    <ItemTemplate>
                        <asp:Button ID="btnDelivered" runat="server" Text="Delivered" CssClass="classname"  CommandName="Delivered" CommandArgument='<%#Eval("DOHDRID")%>' 
                            OnClientClick="return confirm('Are you sure you want to mark this DO as Delivered?')" />
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
    </div>
    </form>
</body>
</html>
