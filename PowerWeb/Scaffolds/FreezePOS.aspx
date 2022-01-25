<%@ Page Language="C#" Title="<%$Resources:dictionary, Freeze POS%>" Inherits="FreezePOS" MasterPageFile="~/PowerPOSMSt.master"
    Theme="default" CodeBehind="FreezePOS.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="PointOfSaleID"
            PageSize="50" SkinID="scaffold" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
            <Columns>
                <asp:BoundField Visible="false" DataField="PointOfSaleID" HeaderText="<%$Resources:dictionary, PointOfSaleID%>"
                    SortExpression="PointOfSaleID" />
                <asp:BoundField DataField="PointOfSaleName" HeaderText="<%$Resources:dictionary, Point Of Sale%>" SortExpression="PointOfSaleName" />
                <asp:BoundField DataField="PointOfSaleDescription" HeaderText="<%$Resources:dictionary,Point Of Sale Description %>"
                    SortExpression="PointOfSaleDescription" />
                <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary, Outlet%>" SortExpression="OutletName" />
                <asp:CheckBoxField DataField="IsFrozen" HeaderText="<%$Resources:dictionary,Frozen %>" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnFreeze" runat="server" Text='<%# Bind("Frozen") %>' CommandName="FreezePOS"
                            CommandArgument='<%# Bind("PointOfSaleID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Point Of Sale%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, %><< First"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, %>< Previous"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                   <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, of%>" /> 
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, %>Next >" CommandArgument="Next"
                        CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, %>Last >>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
