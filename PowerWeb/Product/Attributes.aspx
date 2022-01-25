<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="Attributes.aspx.cs" Inherits="PowerWeb.Product.Attributes" Title="Edit Attributes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        Sorry, No data to be edited
        <asp:Button ID="BtnBack" runat="server" Text="Back" 
            PostBackUrl="ProductMaster.aspx" />
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <table id="Table1" runat="server" width="100%">
            <tr>
                <td class="scaffoldEditItemCaption" style="width: 100px">
                    <asp:Literal ID="Literal1" runat="server" Text="Item No" />
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="tItemNo" runat="server" Text="P01P" 
                        ValidationGroup="EditHeader" ReadOnly="True" />
                </td>
            </tr>
            <tr>
                <td class="scaffoldEditItemCaption" style="width: 100px; height: 26px;">
                    <asp:Literal ID="Literal4" runat="server" Text="Item Name" />
                </td>
                <td class="scaffoldEditItem" style="height: 26px">
                    <asp:TextBox ID="tItemName" runat="server" Width="400px" 
                        Text="" ValidationGroup="EditHeader" />
                </td>
            </tr>
            <tr id="ErrBox">
                <td colspan ="2" style="color: #FF0000; font-weight: bold">
                    <asp:Literal ID="tError" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="cmdReturn" runat="server" CssClass="scaffoldButton" 
                        Text="Back" Width="114px" PostBackUrl="ProductMaster.aspx" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlSubItem" runat="server">
            <asp:GridView 
                ID="GridView2" SkinID="scaffold" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" 
                PageSize="20" Width="100%" 
                ShowFooter="True" onrowcommand="GridView2_RowCommand" 
                DataSourceID="ObjectDataSource1" onrowdeleting="GridView2_RowDeleting">
                
                <Columns>
                    <asp:TemplateField HeaderText="Attribute Group Code" ItemStyle-Width="150px" 
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="viewAttGroupCode" Text='<%# Bind("AttributesGroupCode")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Literal runat="server" ID="editAttGroupCode" Text='<%# Bind("AttributesGroupCode")%>' />
                        </EditItemTemplate>
                        <ItemStyle Width="150px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Attribute Group Name" ItemStyle-Width="100%" 
                        ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%# Eval("AttributesGroupName")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="editItemName" Text='<%# Bind("AttributesGroupName") %>' Width="100%" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="insertAttributesGroup" runat="server" 
                                DataSourceID="odsAttributesGroupName" DataTextField="AttributesGroupName" 
                                DataValueField="AttributesGroupCode">
                            </asp:DropDownList>
<%--                            <asp:TextBox runat="server" ID="insertItemName" Text='<%# Bind("ItemName") %>' Width="100%" />--%>
                        </FooterTemplate>
                        <ItemStyle Width="100%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="Delete" ImageUrl="~/Images/o_delete.gif" CommandName="Delete" Width="20px" Height="20px" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" ID="Update" ImageUrl="~/images/o_save.gif" CommandName="Update" Width="20px" Height="20px" />
                            <asp:ImageButton runat="server" ID="Cancel" ImageUrl="~/Images/o_delete.gif" CommandName="Cancel" Width="20px" Height="20px" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Button runat="server" ID="Insert" Text="Insert" CommandName="InsertNew" />
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                
                <EmptyDataTemplate>
                    <table id="Table1" runat="server" width="100%">
                        <tr>
                            <td class="scaffoldEditItemCaption" style="width: 100px">
                                <asp:Literal ID="Literal5" runat="server" Text="Attributes Group Name" />
                            </td>
                            <td class="scaffoldEditItem">
                                <asp:DropDownList ID="newAttributesGroup" runat="server" 
                                    DataSourceID="odsAttributesGroupName" DataTextField="AttributesGroupName" 
                                    DataValueField="AttributesGroupCode">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="scaffoldEditItem">
                                <asp:Button runat="server" ID="NoDataInsert" Text="Insert" CommandName="NoDataInsert" />
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                
            </asp:GridView>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                DeleteMethod="Sub_Delete" InsertMethod="Sub_Insert" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="FetchByItemNo" 
                TypeName="PowerPOS.AttributesMapController">
                <DeleteParameters>
                    <asp:ControlParameter ControlID="tItemNo" Name="ItemNo" PropertyName="Text" 
                        Type="String" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="tItemNo" Name="ItemNo" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
                <InsertParameters>
                    <asp:ControlParameter ControlID="tItemNo" Name="ItemNo" PropertyName="Text" 
                        Type="String" />
                </InsertParameters>
            </asp:ObjectDataSource>
            
            <asp:ObjectDataSource ID="odsAttributesGroupName" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="FetchAll" 
                TypeName="PowerWeb.Product.ODSAttributesGroupController"></asp:ObjectDataSource>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
