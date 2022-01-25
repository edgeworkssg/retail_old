<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="ViewReport" Title="<%$Resources:dictionary,View Report %>" Codebehind="ViewReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <table width="600px">
        <tr><td style="height: 15px" >            
                <a href="../Membership/MembershipProductSalesReport.aspx">Back</a>&nbsp;</td>
            <td align="right" >
                <asp:LinkButton width="70px" ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>
        <tr>
            <td align="left" colspan=2>
                <asp:Label ID="lblRemark" runat="server" CssClass="LabelMessage" Width="100%"></asp:Label>
            </td>
        </tr>
    </table>    
    <asp:GridView ID="gvReport" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="true" DataKeyNames="MembershipNo" 
        PageSize="20" SkinID="scaffold" OnDataBound="gvReport_DataBound" 
        OnPageIndexChanging="gvReport_PageIndexChanging" 
        OnRowDataBound="gvReport_RowDataBound" OnSorting="gvReport_Sorting" 
        Width="800px" onrowcreated="gvReport_RowCreated">
        <Columns>   
            <asp:HyperLinkField />
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
            <asp:Literal ID = "literal4656" runat="server" Text="No Data Found"></asp:Literal>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>

