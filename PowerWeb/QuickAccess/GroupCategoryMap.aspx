<%@ Page Language="C#" Theme="Default" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="GroupCategoryMap" Title="<%$Resources:dictionary,Group Category Map Setup%>"
    CodeBehind="GroupCategoryMap.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <ajax:UpdateProgress runat="server" ID="progress1" AssociatedUpdatePanelID="abc"
        DisplayAfter="1">
        <ProgressTemplate>
            <img src="../App_Themes/Default/image/indicator_mozilla_blu.gif" />
            <span style="color: #0000ff"><b>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Update in progress%>" /></b><br />
            </span>
        </ProgressTemplate>
    </ajax:UpdateProgress>
    <ajax:UpdatePanel runat="server" ID="abc">
        <ContentTemplate>
            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="LabelMessage"></asp:Label>
            <table width="600">
                <tbody>
                    <tr>
                        <td class="wl_pageheaderSub" colspan="2">
                            <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Select Event %>">
                            </asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 104px">
                            <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Quick Access Group %>"></asp:Literal>
                        </td>
                        <td>
                            <subsonic:DropDown ID="ddlQuickAccessGroup" runat="server" CausesValidation="True"
                                OnSelectedIndexChanged="ddlName0_SelectedIndexChanged" ShowPrompt="True" TableName="QuickAccessGroup"
                                TextField="QuickAccessGroupName" ValidationGroup="AddItem" ValueField="QuickAccessGroupID"
                                WhereField="Deleted" WhereValue="false" Width="457px" AutoPostBack="True">
                            </subsonic:DropDown>
                        </td>
                    </tr>
                    <tr>
                        <td class="wl_pageheaderSub" colspan="2">
                            <asp:Literal ID="AddItemtobeStockInLbl" runat="server" Text="<%$Resources:dictionary,Add Category %>">
                            </asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 104px">
                            <asp:Literal ID="ItemNameLbl" runat="server" Text="<%$Resources:dictionary,Quick Access Category %>"></asp:Literal>
                        </td>
                        <td>
                            <subsonic:DropDown ID="ddlCategory" runat="server" CausesValidation="True" ShowPrompt="True"
                                TableName="QuickAccessCategory" TextField="QuickAccessCatName" ValidationGroup="AddItem"
                                ValueField="QuickAccessCategoryID" Width="457px" WhereField="Deleted" WhereValue="false">
                            </subsonic:DropDown>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 104px">
                        </td>
                        <td>
                            <asp:Button ID="btnOk" runat="server" CssClass="classname" OnClick="btnOk_Click"
                                Text="<%$ Resources:dictionary, Ok %>" ValidationGroup="AddItem" Width="41px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="wl_pageheaderSub" colspan="2">
                            <asp:Literal ID="InventoryDetailsLbl" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="" colspan="2">
                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" DataKeyNames="QuickAccessGroupMapID"
                                SkinID="scaffold" Width="100%" OnRowDeleting="gvDetails_RowDeleting" OnSelectedIndexChanged="gvDetails_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$Resources:dictionary, Date%>" SortExpression="PLDate">
                                        <ItemTemplate>
                                            <a id="HyperLink1" target="_blank" href="QuickAccessButtonSetup.aspx?GroupId=<%# Eval("QuickAccessGroupID").ToString()%>&CatID=<%# Eval("QuickAccessCategoryID").ToString()%>">
                                                <asp:Literal ID="ltr01"  runat="server" Text="<%$Resources:dictionary, Edit Buttons%>" /> </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowDeleteButton="True" ValidationGroup="EditLine"></asp:CommandField>
                                    <asp:BoundField DataField="QuickAccessCatName" HeaderText="<%$Resources:dictionary, Quick Access Category%>" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>
            &nbsp;
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
