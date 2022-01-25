<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="SalesReturnReport.aspx.cs" Title="<%$Resources:dictionary, Sale Return Report%>" Inherits="PowerWeb.Reports.SalesReturnReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FilterTable">
        <tr style="height: 20px;">
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
                <asp:Label ID="lblYear" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlPOS" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Search %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <%--<tr><td colspan=2 >
            &nbsp;<asp:Button ID="Button1" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align=right class="ExportButton">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>
            --%>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 600px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 25px;">
                <asp:Literal ID="litMessage" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <table width="600px">
        <tr>
            <td colspan="4" style="height: 451px">
                <asp:GridView ID="gvReport" Width="800px" runat="server" AutoGenerateColumns="False"
                    AllowPaging="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
                    ShowFooter="True" OnPageIndexChanging="gvReport_PageIndexChanging" SkinID="scaffold"
                    PageSize="20" DataKeyNames="OrderHdrID" OnRowDataBound="gvReport_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ReceiptNo" HeaderText="<%$Resources:dictionary, Receipt No%>" />
                        <asp:BoundField DataField="Returned Receipt No" HeaderText="<%$Resources:dictionary, Returned Receipt No%>" />
                        <asp:BoundField DataField="Receipt Date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="<%$Resources:dictionary, Receipt Date%>" />
                        <asp:BoundField DataField="PointOfSaleName" HeaderText="<%$Resources:dictionary, Point Of Sale%>" />
                        <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary, Category%>" Visible="False" />
                        <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>" />
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Item Name%>" Visible="False" />
                        <asp:BoundField DataField="Attributes1" HeaderText="<%$Resources:dictionary, Attributes1%>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:dictionary, Amount%>" />
                        <asp:BoundField DataField="MembershipNo" HeaderText="<%$Resources:dictionary, Membership No%>" />
                        <asp:BoundField DataField="NameToAppear" HeaderText="<%$Resources:dictionary, Membership Name%>" />
                        <asp:TemplateField HeaderText="<%$Resources:dictionary, Cheque Issued%>" Visible="False">
                            <ItemTemplate>
                                <asp:LinkButton OnClick="lnkStatus_Click" ID="lnkStatus" Text='<%# Eval("Cheque Issued") %>'
                                    runat="server"></asp:LinkButton><asp:Label ID="lblCheque" runat="server" Text='<%# Eval("Cheque Issued") %>'></asp:Label></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" ForeColor="Black" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Cheque No" HeaderText="<%$Resources:dictionary, Cheque No%>" Visible="False" />
                        <asp:BoundField DataField="SalesPerson" HeaderText="<%$Resources:dictionary, Sales Person%>" />
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
            </td>
        </tr>
    </table>
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup" CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="150px" Width="250px"
        Style="display: none; overflow: auto; border: solid 3px #000;">
        <div>
            <h4>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Edit Cheque No%>" /></h4>
            <asp:HiddenField ID="hdnReceiptNo" Value="" runat="server" />
        </div>
        <table width="250px">
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Cheque No%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtChequeNo" Width="150px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td align="center" style="padding-left: 30px;" colspan="2">
                    <asp:Button ID="btnUpdate" OnClick="btnUpdate_Click" CssClass="btn" runat="server"
                        Text="<%$Resources:dictionary, Update%>" />
                    <asp:Button ID="btnCancel" CssClass="btn" runat="server" Text="<%$Resources:dictionary, Cancel%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
