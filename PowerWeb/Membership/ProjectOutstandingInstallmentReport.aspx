<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="ProjectOutstandingInstallmentReport" Title="<%$Resources:dictionary,Project Outstanding Installment Report %>" Codebehind="ProjectOutstandingInstallmentReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <table style="width: 797px">
        <tr><td colspan=3 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        <tr><td >
            <asp:Literal ID = "Literal5" runat="server" 
                Text="<%$ Resources:dictionary, Search %>"></asp:Literal></td><td>
                <asp:TextBox ID="txtSearch" runat="server" Width="172px"></asp:TextBox></td>
        <td  style="width: 345px">
            &nbsp;</td></tr>        
        <tr><td colspan=2 style="height: 15px" >
            &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="classname"  Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" CssClass="classname"  Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td align="right"  style="width: 345px">
                <asp:LinkButton ID="lnkExport" runat="server" class="classBlue" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>
    </table>    
    <asp:GridView ID="gvReport" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" DataKeyNames="MembershipNo" 
        PageSize="20" SkinID="scaffold" OnDataBound="gvReport_DataBound" OnPageIndexChanging="gvReport_PageIndexChanging" OnRowDataBound="gvReport_RowDataBound" OnSorting="gvReport_Sorting" Width="800px">
        <Columns>
            <asp:BoundField DataField="MembershipNo" HeaderText="Membership No" 
                SortExpression="MembershipNo" />
            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" 
                SortExpression="ProjectName" />
            <asp:BoundField DataField="NameToAppear" HeaderText="Name" 
                SortExpression="NameToAppear" />                                                            
            <asp:BoundField DataField="Home" HeaderText="Home" SortExpression="Home" />
            <asp:BoundField DataField="Mobile" HeaderText="Mobile" 
                SortExpression="Mobile" />                                    
            <asp:BoundField DataField="Credit" HeaderText="Credit" 
                SortExpression="Credit" />
            <asp:BoundField DataField="Debit" HeaderText="Debit" SortExpression="Debit" />
            <asp:BoundField DataField="Balance" HeaderText="Balance" 
                SortExpression="Balance" />
        </Columns>
         <PagerTemplate>
                        <div style="border-top:1px solid #666666">            
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
        <EmptyDataTemplate>
            <asp:Literal ID = "literal4656" runat="server" Text="<%$Resources:dictionary,No Membership %>"></asp:Literal>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>

