<%@ Page Language="C#" Theme="Default"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="ProductInquiry" Title="Product Inquiry" Codebehind="ProductInquiry.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script>
    var newwindow;
    function poptastic(url)
    {
	    newwindow=window.open(url,'name','height=700,width=800,resizeable=1,scrollbars=1');
	    if (window.focus) {newwindow.focus()}
    }
    </script>
    <asp:Panel id="pnlGrid" runat="server">	
    <table style="width: 476px">
        <tr><td class="scaffoldEditItemCaption" style="width: 147px">
            <asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal></td><td class="scaffoldEditItem" style="width: 229px" >
            <asp:TextBox ID="txtItemNo" runat="server" Width="147px"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="<%$ Resources:dictionary, Search %>" /></td></tr>
        <tr>
            <td class="scaffoldEditItem" colspan="2">
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label></td>
        </tr>
        <tr><td colspan="2" align=right class="ExportButton"><asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td></tr>
    </table>
    <asp:GridView 
    ID="GridView1" 
    SkinID="scaffold"
    runat="server" 
    AllowPaging="True" 
    AllowSorting="True"
    AutoGenerateColumns="False"         
    DataKeyNames="ItemNo" 
    PageSize="20" Width="100%" OnRowDataBound="GridView1_RowDataBound" 
    ondatabound="GridView1_DataBound" onpageindexchanging="GridView1_PageIndexChanging" 
    >
        <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                    <a id="HyperLink1" 
                     href="javascript:poptastic('ViewItem.aspx?id=<%# Eval("ItemNo")%>');">
                     <asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,View %>"></asp:Literal></a>
                    </ItemTemplate>
                </asp:TemplateField>
                         
	            <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>"></asp:BoundField>	            
	            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Product Name %>"></asp:BoundField>	            	            
	            <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category %>"></asp:BoundField>	            	            	            	            
	            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:dictionary,Department %>"></asp:BoundField>	            	            	            	            
	            <asp:BoundField DataField="RetailPrice" HeaderText="<%$Resources:dictionary,Retail Price %>"></asp:BoundField>	            	            	            	            	            	            
	            <asp:BoundField DataField="SPP" HeaderText="<%$Resources:dictionary,SPP %>"></asp:BoundField>
	            <asp:BoundField DataField="P3" HeaderText="Staff 20% SPP"></asp:BoundField>	
	            <asp:BoundField DataField="P4" HeaderText="Staff 40% RCP"></asp:BoundField>            	            	            	            	            	            
	            <asp:BoundField DataField="FactoryPrice" HeaderText="Factory Price" Visible="false"></asp:BoundField>      	            	            	            	            	            
	            <asp:BoundField DataField="IsInInventory" HeaderText="<%$Resources:dictionary,Inventory Item %>"></asp:BoundField>	            	            	            
	            <asp:BoundField DataField="IsNonDiscountable" HeaderText="<%$Resources:dictionary,Non Discountable %>"></asp:BoundField>	            	            	            	            	            	            	            	            	            	            
	            <asp:BoundField DataField="Attributes1"  HeaderText=""></asp:BoundField>	            
	            <asp:BoundField DataField="Attributes2"  HeaderText=""></asp:BoundField>	            
	            <asp:BoundField DataField="Attributes3"  HeaderText=""></asp:BoundField>	            
	            <asp:BoundField DataField="Attributes4"  HeaderText=""></asp:BoundField>	            
	            <asp:BoundField DataField="Attributes5"  HeaderText=""></asp:BoundField>	            	            	         	            	            
	            <asp:BoundField DataField="PointType" HeaderText="Point Type"></asp:BoundField>
	            <asp:BoundField DataField="PointAmount" HeaderText="Point Amount"></asp:BoundField>
	            <asp:BoundField  DataField="Barcode" HeaderText="<%$Resources:dictionary,Barcode %>"></asp:BoundField>	            	            	            
	            <asp:BoundField DataField="IsServiceItem" HeaderText="<%$Resources:dictionary,Service Item %>"></asp:BoundField>	            	            	            
	            <asp:BoundField DataField="GSTRule" HeaderText="<%$Resources:dictionary,GST Rule %>"></asp:BoundField>
	            <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary,Remark %>"></asp:BoundField>	           	            	            	            	            
	            <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary,Deleted %>"></asp:BoundField>	            
        </Columns>
        <EmptyDataTemplate>
            <asp:Literal ID = "literal5" runat="server" Text="<%$Resources:dictionary,No Product Created Yet %>"></asp:Literal>
        </EmptyDataTemplate>
       <PagerTemplate>
            <div style="border-top:1px solid #666666">
            <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
    </asp:GridView>	
	</asp:Panel>
</asp:Content>

