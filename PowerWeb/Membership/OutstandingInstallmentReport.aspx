<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="OutstandingInstallmentReport" Title="<%$Resources:dictionary,Outstanding Installment Report %>"
    CodeBehind="OutstandingInstallmentReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <div style="height: 20px; width: 650px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="650px" id="FilterTable">
        <tr style="width:250px">
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:dictionary, Search %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" Width="170px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                 <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:dictionary, Show Outstanding Installment Only  %>"></asp:Literal>
            </td>
            <td>
                <asp:CheckBox ID="cbIsShowOutstandingOnly" runat="server" AutoPostBack="true" OnCheckedChanged="cbIsShowOutstandingOnly_OnCheckedChanged"/>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" CssClass="classname" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td align="right">
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvReport" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" DataKeyNames="MembershipNo" PageSize="20" SkinID="scaffold"
        OnDataBound="gvReport_DataBound" OnPageIndexChanging="gvReport_PageIndexChanging"
        OnRowDataBound="gvReport_RowDataBound" OnSorting="gvReport_Sorting" Width="800px"
        OnRowCommand="gvReport_RowCommand" OnSelectedIndexChanged="gvReport_SelectedIndexChanged">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="View" runat="server" CausesValidation="false" CommandName="Select"
                        Text="<%$Resources:dictionary, View%>"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MembershipNo" HeaderText="<%$Resources:dictionary, Membership No%>" SortExpression="MembershipNo" />
            <asp:BoundField DataField="NameToAppear" HeaderText="<%$Resources:dictionary, Name%>" SortExpression="NameToAppear" />
            <asp:BoundField DataField="Home" HeaderText="<%$Resources:dictionary, Home%>" SortExpression="Home" />
            <asp:BoundField DataField="Mobile" HeaderText="<%$Resources:dictionary, Mobile%>" SortExpression="Mobile" />
            <asp:BoundField DataField="Balance" HeaderText="<%$Resources:dictionary, Balance%>" DataFormatString="{0:N2}" SortExpression="Balance" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                    ID="lblPageCount" runat="server"></asp:Label>
                <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                    CommandArgument="Next" CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                    CommandArgument="Last" CommandName="Page" />
            </div>
        </PagerTemplate>
        <EmptyDataTemplate>
            <asp:Literal ID="literal4656" runat="server" Text="No Installment data has been created yet"></asp:Literal>
        </EmptyDataTemplate>
        <SelectedRowStyle ForeColor="#000066" />
    </asp:GridView>
</asp:Content>
