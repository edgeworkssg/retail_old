<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="TransactionDetailReport" Title="<%$Resources:dictionary,Transaction Detail Report %>" Codebehind="TransactionDetailReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 

    <script>
    var newwindow;
    function poptastic(url)
    {
	    newwindow=window.open(url,'name','height=700,width=650');
	    if (window.focus) {newwindow.focus()}
    }
    </script>
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FilterTable" >
        <tr  style="height:20px;"><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        <tr>
            <td>
                <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal></td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                <asp:DropDownList ID="ddStartHour" runat="server">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddStartMinute" runat="server">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                </asp:DropDownList>
                <br />
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use Start Date %>" /></td>
            <td>
                <asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal></td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                <asp:DropDownList ID="ddEndHour" runat="server">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddEndMinute" runat="server">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                </asp:DropDownList>
                <br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use End Date %>" /></td>
        </tr>
        <tr><td><asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,Item Name %>"></asp:Literal></td><td><asp:TextBox ID="txtItemName" runat="server" Width="172px"></asp:TextBox></td>
        <td>
            <asp:Literal ID = "Literal8" runat="server" 
                Text="<%$Resources:dictionary,Category %>"></asp:Literal></td><td>
                <asp:DropDownList ID="ddCategory" runat="server" Width="179px">
                </asp:DropDownList></td></tr>
        <tr><td>
                <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal></td><td>
                <subsonic:DropDown ID="ddDept" runat="server" OnInit="ddDept_Init" ShowPrompt="true" PromptValue="0" PromptText = "--ALL--"
                    TableName="ItemDepartment" TextField="DepartmentName" ValueField="ItemDepartmentID" Width="172px">
                </subsonic:DropDown></td>
        <td><asp:Literal ID = "lblOutlet" runat="server" 
                Text="<%$ Resources:dictionary,Outlet %>"></asp:Literal></td><td>
            <asp:DropDownList ID="ddlOutlet" runat="server" Width="179px" AutoPostBack="True" OnSelectedIndexChanged="ddDept_SelectedIndexChanged">
            </asp:DropDownList></td></tr>
        <tr>
            <td>
                <asp:Literal ID = "lblPOS" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal></td>
            <td>
            <subsonic:DropDown ID="ddPOS" runat="server" OnInit="ddPOS_Init" PromptText="ALL"
                ShowPrompt="True" TableName="PointOfSale" TextField="PointOfSaleName" ValueField="PointOfSaleID"
                Width="170px">
            </subsonic:DropDown></td>
            <td>
                <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Cashier ID %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtCashier" runat="server" Width="173px"></asp:TextBox>
            </td>
        </tr>
        <%--<tr><td colspan=2 style="height: 15px" >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align="right" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>--%>
    </table> 
     <table id="search_ExportTable" style="vertical-align:middle;width:600px;height:40px;" border="0" cellpadding="2" cellspacing="0">
            <tr>                
                <td style="height:30px;width:50%; background-color:#FFFFFF; left:0;vertical-align :middle ;" >
                    <asp:LinkButton  ID="LinkButton1"  class="classname"  runat="server"  OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:dictionary, Search%>" /> </asp:LinkButton><div class="divider">   </div>
                    <asp:LinkButton  ID="LinkButton4" class="classname" runat="server" ><asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton></td><td align="right" style="height:30px;width:50%; background-color:#FFFFFF;padding-right:0px;vertical-align:middle;right:0px;">
                     <asp:LinkButton  ID="LinkButton3" class="classBlue"  runat="server" OnClick="lnkExport_Click" >  
                     <asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton></td></tr><tr>
                <td colspan="4" style="height:25px;"> 
                <asp:Literal ID="litMessage" runat="server" Text="" /> </td>
            </tr>
     </table>     
    <asp:GridView ID="gvReport"
            Width="100%" 
            runat="server" 
            PageSize=20
            AllowPaging="True" 
            AllowSorting="True"
            OnDataBound="gvReport_DataBound" 
            OnSorting="gvReport_Sorting"
	        OnPageIndexChanging="gvReport_PageIndexChanging"
	        DataKeyNames="OrderRefNo" 
            AutoGenerateColumns="False"             
            ShowFooter="True" 
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField />
            <asp:TemplateField>
                <ItemTemplate>                  
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OrderDetDate" SortExpression="OrderDetDate" HeaderText="<%$Resources:dictionary,Order Date %>" />            
            <asp:BoundField DataField="Customize" SortExpression="Customize" 
                HeaderText="<%$Resources:dictionary,Order RefNo %>" />            
            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />            
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary,Item Name %>" />
            <asp:BoundField DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="Unit Price"   />            
            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="<%$Resources:dictionary,Qty %>"   />            
            <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="<%$Resources:dictionary,Amount %>"   />
            <asp:BoundField DataField="GSTAmountPerItem" HeaderText="GST" 
                SortExpression="GSTAmountPerItem" />
            <asp:BoundField DataField="amountBefGST" HeaderText="Amount Bef GST" 
                SortExpression="amountBefGST" />
            <asp:BoundField DataField="Disc" SortExpression="Disc" 
                HeaderText="<%$Resources:dictionary,Discount %>"/>
            <asp:BoundField DataField="DiscPercent" HeaderText="Disc. Percent" 
                SortExpression="DiscPercent" />
            <asp:BoundField DataField="PointOfSaleName" SortExpression="PointOfSaleName" HeaderText="<%$Resources:dictionary,Point Of Sale %>" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary,Outlet%>" />            
            <asp:BoundField Visible="False" DataField="PointOfSaleID" SortExpression="PointOfSaleID" HeaderText="<%$Resources:dictionary,Point Of Sale ID %>" />
        </Columns>
         <PagerTemplate>
                        <div style="border-top:1px solid #666666">            
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
        
    </asp:GridView>    
</asp:Content>

