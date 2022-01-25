<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="DeliveryList" Title="<%$Resources:dictionary, Delivery List%>"
    CodeBehind="DeliveryList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript"> 
	        var ele = document.getElementById("HelpBox");
	        if(ele.style.display == "block") {
    		        ele.style.display = "none";
  	        }
	        else {
		        ele.style.display = "block";
	        }
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
    <div id="HelpBox" style="position: absolute; display: none; left: 50px; width: 300px;
        background-color: Gray; padding: 10px 10px 10px 10px;">
        <a href="javascript:toggle();" style="position: inherit; right: 10px; top: 10px;">
            <img id="btnCloseHelp" src="../Images/close.gif" />
        </a>
        <h1>
            <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, HELP%>" /></h1>
        <p>
            <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary, Search the items to be delivered out from your warehouse%>" />
            <br />
            <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Tick the selected items scroll to the bottom of the page and press the%>" />
            <b><asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Do Stock Out%>" /></b><asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, button%>" /> 
            <br />
        </p>
    </div>
    <div style="height: 20px; width: 1024px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="1024px">
        <%--  <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
       --%>
        <tr>
            <td style="width: 200px; height: 3px">
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td style="width: 200px; height: 3px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td style="width: 200px">
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Reference Number%>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtRefNo" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Point Of Sale%>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="ddPOS" runat="server" TableName="PointOfSale" TextField="PointOfSaleName"
                    ValueField="PointOfSaleID" Width="170px" OnInit="ddPOS_Init">
                </subsonic:DropDown>
            </td>
        </tr>
        <tr>
            <td style="width: 200px">
                
                <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Search%>" />
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            </td>
            <td style="width: 102px">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <%--  <tr>
        <td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" />
            <button type="button" causesvalidation="false" onclick="javascript:toggle();">Help</button>
            </td>
            <td colspan=2 align=right class="ExportButton">
            <asp:LinkButton ID="lnkSelectAll" runat="server" OnClick="lnkSelectAll_Click" Text="Select All"></asp:LinkButton>
                , <asp:LinkButton ID="lnkSelectNone" runat="server" OnClick="lnkSelectNone_Click" Text="Select None"></asp:LinkButton>
                , <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
                </td>
        </tr>--%>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 1024px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search%>"
                    class="classname" OnClick="btnSearch_Click" />
                <%--  <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Search%>" /> </asp:LinkButton>--%>
                <div class="divider">
                </div>
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear%>"
                    class="classname" OnClick="btnClear_Click" />
                <%--<asp:LinkButton  ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click" >
                    <asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>--%>
                <div class="divider">
                </div>
                <button type="button" causesvalidation="false" class="classname" onclick="javascript:toggle();">
                <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Help%>" />
                    </button>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="lnkSelectAll" class="classBlue" runat="server" OnClick="lnkSelectAll_Click"
                    Text="<%$Resources:dictionary, Select All%>"></asp:LinkButton>
                <asp:LinkButton ID="lnkSelectNone" class="classBlue" runat="server" OnClick="lnkSelectNone_Click"
                    Text="<%$Resources:dictionary, Select None%>"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
            </td>
            <%-- <td colspan="4" style="height:25px;"> 
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label></td>--%>
        </tr>
        <tr>
            <td colspan="4">
                <div>
                    <asp:Label ID="lblResult" runat="server"></asp:Label>
                    <br />
                    <asp:GridView ID="gvReport" Width="100%" runat="server" SkinID="scaffold" AutoGenerateColumns="False"
                        AllowSorting="True" OnRowDataBound="gvReport_RowDataBound" OnDataBound="gvReport_DataBound"
                        OnSorting="gvReport_Sorting">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OrderDetID" HeaderText="<%$Resources:dictionary, Ref No%>" SortExpression="OrderDetID" />
                            <asp:BoundField DataField="OrderDate" HeaderText="<%$Resources:dictionary, Date%>" SortExpression="OrderDate"
                                DataFormatString="{0:D}" />
                            <asp:BoundField DataField="DeliveryAddress" HeaderText="<%$Resources:dictionary, Delivery Address%>" SortExpression="DeliveryAddress" />
                            <asp:BoundField DataField="StoreReference" HeaderText="<%$Resources:dictionary, Store Reference%>" SortExpression="StoreReference" />
                            <asp:BoundField DataField="ModeOfDelivery" HeaderText="<%$Resources:dictionary, Mode Of Delivery%>" SortExpression="ModeOfDelivery" />
                            <asp:BoundField DataField="MembershipNo" HeaderText="<%$Resources:dictionary, Member No%>" SortExpression="MembershipNo" />
                            <asp:BoundField DataField="MemberName" HeaderText="<%$Resources:dictionary, Member Name%>" SortExpression="MemberName" />
                            <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>" SortExpression="ItemNo" />
                            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Item Name%>" SortExpression="ItemName" />
                            <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:dictionary, Qty%>" SortExpression="Quantity" />
                            <asp:BoundField DataField="DeliveryStatus" HeaderText="<%$Resources:dictionary, Status%>" SortExpression="CASE WHEN ID.OrderDetID IS NOT NULL AND ID.OrderDetID <> '' THEN 'Delivered' ELSE 'Pending' END" />
                            <asp:BoundField DataField="DeliveryRemark" HeaderText="<%$Resources:dictionary, Remark%>" SortExpression="DeliveryRemark" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="<%$Resources:dictionary, Do Stock Out%>"
                    Width="110px" Height="35px" />
            </td>
            <td colspan="2" align="right" class="ExportButton">
            </td>
        </tr>
    </table>
</asp:Content>
