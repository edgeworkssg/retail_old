<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="StockOut" Title="<%$ Resources:dictionary, Stock Out %>" CodeBehind="StockOut.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ScriptManager id="ScriptManager1" runat="server" />
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="btnInventoryDate" TargetControlID="txtInventoryDate">
    </cc1:CalendarExtender>
 <div style="height:20px;width:1024px;" class="wl_pageheaderSub"> <asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal> </div>
   
    <table width="1024px">
       <%-- <tr><td colspan="4" class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="Basic Information"></asp:Literal></td></tr>
        --%><tr>
            <td  style="width: 150px; height: 3px">
                <asp:Literal ID = "Literal3" runat="server" Text="Ref No" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtRefNo" runat="server" ReadOnly="true" Width="180px" />
            </td>
            <td  style="width: 150px; height: 3px">
                <asp:Literal ID = "Literal1" runat="server" Text="Date" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtInventoryDate" runat="server" />
                <asp:ImageButton ID="btnInventoryDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>      
        <tr>
            <td  style="width: 102px; height: 3px">
                <asp:Literal ID = "Literal2" runat="server" Text="Location" />
            </td>
            <td style="height: 3px">
                <subsonic:DropDown ID="cmbLocation" runat="server" ShowPrompt="True" TableName="InventoryLocation"
                    TextField="InventoryLocationName" ValueField="InventoryLocationID" PromptText="--Select Location--" PromptValue="0" Width="180px" />
            </td>
            <td  style="height: 3px">
                <asp:Literal ID = "Literal4" runat="server" Text="Reason" />
            </td>
            <td style="height: 3px">
                <asp:DropDownList ID="cmbReason" runat="server" Width="180px" />
            </td>
        </tr>     
        <tr>
            <td  style="width: 102px; height: 3px">
                <asp:Literal ID = "Literal6" runat="server" Text="Remarks" />
            </td>
            <td style="height: 50px">
                <asp:TextBox ID="txtRemarks" Height="50px" Width="180px" runat="server" TextMode="MultiLine" />
            </td>
            <td  style="height: 3px">
                &nbsp;
            </td>
            <td style="height: 3px">
                &nbsp;
            </td>
        </tr>     
<%--        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal5" runat="server" Text="Details"></asp:Literal></td></tr>--%>
        <tr>
            <td colspan=4>
                <div>
                    <asp:Label ID="lblResult" runat="server"></asp:Label>
                    <br />
                    <asp:GridView ID="gvReport" runat="server" Width="100%" ShowFooter="True"
                            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                            OnPageIndexChanging="gvReport_PageIndexChanging"	        
                            OnRowDataBound="gvReport_RowDataBound" 
                            OnDataBound="gvReport_DataBound" 
                            OnSorting="gvReport_Sorting" 
                            SkinID="scaffold" PageSize="20">
                        <Columns>
                            <%--<asp:CheckBoxField HeaderText = "AA" DataField="Deleted"  />--%>
                            <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="Item No"  />
                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Item Name" />
                            <asp:BoundField DataField="OnHand" SortExpression="OnHand" HeaderText="Bal Qty"  />
                            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="Qty"  />
                            <asp:BoundField DataField="FactoryPrice" SortExpression="FactoryPrice" HeaderText="Cost Price"  />
                            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark"  />
                        </Columns>
                        <PagerTemplate>
                            <div style="border-top:1px solid #666666">
                                <asp:Button ID="btnFirst" runat="server" CommandArgument="First" 
                                    CommandName="Page" CssClass="scaffoldButton" 
                                    Text="<%$Resources:dictionary,<< First %>" />
                                <asp:Button ID="btnPrev" runat="server" CommandArgument="Prev" 
                                    CommandName="Page" CssClass="scaffoldButton" 
                                    Text="<%$Resources:dictionary,< Previous%>" />
                                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" 
                                    Css 
                                    OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal>
                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                <asp:Button ID="btnNext" runat="server" CommandArgument="Next" 
                                    CommandName="Page" CssClass="scaffoldButton" 
                                    Text="<%$Resources:dictionary,Next > %> " />
                                <asp:Button ID="btnLast" runat="server" CommandArgument="Last" 
                                    CommandName="Page" CssClass="scaffoldButton" 
                                    Text="<%$Resources:dictionary,Last >> %> " />
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 3px">
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click" Width="75px" Height="35px" />
            </td>
            <td style="height: 3px">
                &nbsp;
            </td>
            <td style="height: 3px">
                &nbsp;
            </td>
            <td style="height: 3px;">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>