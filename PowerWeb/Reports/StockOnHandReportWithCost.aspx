<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="StockOnHandReportWithCost" Title="Stock Balance Report (With Cost)" Codebehind="StockOnHandReportWithCost.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <table  width="600px" id="FilterTable">
        <tr style="height:20px;"><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        <tr><td> <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal></td><td><asp:TextBox ID="txtSearch" runat="server" Width="172px"></asp:TextBox></td>
        <td>
                <asp:CheckBox ID="cbShowCostPrice" runat="server" Text="Display Cost Price" />
            </td></tr>
        <tr><td>
            <asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,Inventory Location %>"></asp:Literal></td><td>
            <subsonic:DropDown ID="ddlInventoryLocation" runat="server" 
                    PromptText="--Please Select--" ShowPrompt="True"
                TableName="InventoryLocation" TextField="InventoryLocationName" ValueField="InventoryLocationID"
                Width="177px" PromptValue="0">
            </subsonic:DropDown></td>
        <td align="right">
                <asp:LinkButton class="classBlue" ID="btnExportStockTakeForm" runat="server" 
                    Text="<%$ Resources:dictionary, Export Stock Take Form%>" 
                    onclick="LinkButton1_Click"></asp:LinkButton>
            </td></tr>
       <%-- <tr>
            <td>
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align=right class="classname">
            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>
        <tr><td colspan=2><asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
        </td></tr>--%>
    </table> 
    <table id="search_ExportTable" style="vertical-align:middle;width:600px;height:40px;" border="0" cellpadding="2" cellspacing="0">
            <tr>                
                <td style="height:30px;width:50%; background-color:#FFFFFF; left:0;vertical-align :middle ;" >
                    <asp:LinkButton  ID="LinkButton1"  class="classname"  runat="server"  OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:dictionary, Search%>" /> </asp:LinkButton><div class="divider">   </div>
                    <asp:LinkButton  ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click" ><asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton></td><td align="right" style="height:30px;width:50%; background-color:#FFFFFF;padding-right:0px;vertical-align:middle;right:0px;">
                     <asp:LinkButton  ID="LinkButton3" class="classBlue"  runat="server" OnClick="lnkExport_Click" >  
                     <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton></td></tr><tr>
                <td colspan="4" style="height:25px;"> 
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label></td></tr></table><asp:GridView ID="gvReport"
            Width="800px" 
            runat="server" 
            AllowPaging="True" 
            AllowSorting="True"
            OnDataBound="gvReport_DataBound" 
            OnSorting="gvReport_Sorting"
	        OnPageIndexChanging="gvReport_PageIndexChanging" 
            AutoGenerateColumns="False"             
             PageSize=20
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" 
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="DepartmentName" SortExpression="DepartmentName" HeaderText="Department"  />
            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />                                    
            <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="<%$Resources:dictionary,ItemNo %>" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary,Item %>" />            
            <asp:BoundField DataField="Attributes1" SortExpression="Attributes1" HeaderText="Attributes1" />            
            <asp:BoundField DataField="Attributes2" SortExpression="Attributes2" HeaderText="Attributes2" />            
            <asp:BoundField DataField="Attributes3" SortExpression="Attributes3" HeaderText="Attributes3" />            
            <asp:BoundField DataField="Attributes4" SortExpression="Attributes4" HeaderText="Attributes4" />            
            <asp:BoundField DataField="Attributes5" SortExpression="Attributes5" HeaderText="Attributes5" />            
            <asp:BoundField DataField="Attributes6" SortExpression="Attributes6" HeaderText="Attributes6" />            
            <asp:BoundField DataField="Attributes7" SortExpression="Attributes7" HeaderText="Attributes7" />            
            <asp:BoundField DataField="Attributes8" SortExpression="Attributes8" HeaderText="Attributes8" />            
            <asp:BoundField DataField="OnHand" SortExpression="OnHand" HeaderText="<%$Resources:dictionary,On Hand %>" />                        
            <asp:BoundField DataField="CostOfGoods" SortExpression="CostOfGoods" HeaderText="<%$Resources:dictionary,Avg Unit Cost %>" />                        
            <asp:BoundField DataField="TotalCost" SortExpression="TotalCost" HeaderText="<%$Resources:dictionary,Total Cost %>" />                                    
            <asp:BoundField DataField="RetailPrice" HeaderText="Retail Price" />
        </Columns>
         <PagerTemplate>                   
        <div style=" text-align:center; border:1px solid #666666">            
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
        <EmptyDataTemplate>Search does not produce any result</EmptyDataTemplate></asp:GridView></asp:Content>