<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="DashboardSetup.aspx.cs" Inherits="PowerWeb.Support.DashboardSetup"
    Title="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .style2
        {
            width: 150px;
            height: 26px;
        }
        .style3
        {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <asp:Button ID="btnAddNew" CssClass="classname" runat="server" Text="Add New" 
            onclick="btnAddNew_Click" />&nbsp;
        <div style="height: 5px;">
        </div>
        <asp:GridView ID="gvDashboard" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="ID" PageSize="50" 
            SkinID="scaffold" ondatabound="gvDashboard_DataBound" 
            onpageindexchanging="gvDashboard_PageIndexChanging" 
            onrowcommand="gvDashboard_RowCommand">
            <Columns>
                <asp:BoundField Visible="false" DataField="ID" HeaderText="ID" />
                <asp:BoundField DataField="Title" HeaderText="Title" />
                <asp:BoundField DataField="SubTitle" HeaderText="Sub Title" />
                <asp:BoundField DataField="PlotType" HeaderText="PlotType" />
                <asp:BoundField DataField="Width" HeaderText="Width" />
                <asp:BoundField DataField="Height" HeaderText="Height" />
                <asp:CheckBoxField DataField="IsEnable" HeaderText="IsEnable" />
                <asp:BoundField DataField="DisplayOrder" HeaderText="DisplayOrder" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDisable" runat="server" Text='<%# Bind("Disabled") %>' CommandName="Disable"
                            CommandArgument='<%# Bind("ID") %>' />
                        &nbsp;
                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditData" CommandArgument='<%# Bind("ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No Dashboard
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<< First"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="< Previous"
                        CommandArgument="Prev" CommandName="Page" />
                    Page
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    of
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="Next >" CommandArgument="Next"
                        CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="Last >>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlForm" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td style="width: 150px">
                    Title
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:HiddenField ID="hdID" runat="server" />
                    <asp:TextBox ID="txtTitle" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Sub Title
                </td>
                <td class="style3">
                    :
                </td>
                <td class="style3">
                    <asp:TextBox ID="txtSubTitle" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Plot Type
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:DropDownList ID="ddlPlotType" Width="200px" runat="server">
                        <asp:ListItem Text="Area Chart" Value="AreaChart" />
                        <asp:ListItem Text="Bar Chart" Value="BarChart" />
                        <asp:ListItem Text="Column Chart" Value="ColumnChart" />
                        <asp:ListItem Text="Lines" Value="Lines" />
                        <asp:ListItem Text="Pie Chart" Value="PieChart" />
                        <asp:ListItem Text="Stepped Area Chart" Value="SteppedAreaChart" />
                        <asp:ListItem Text="Table " Value="Table " />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Plot Option
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtPlotOption" runat="server" Width="200px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Width
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtWidth" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Height
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtHeight" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    SQL String
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtSQLString" runat="server" Width="200px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Is In Line
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:CheckBox ID="chkIsInLine" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Break After
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:CheckBox ID="chkBreakAfter" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Break Before
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:CheckBox ID="chkBreakBefore" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Display Order
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtDisplayOrder" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    Is Enable
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:CheckBox ID="chkEnable" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="Save" 
                        onclick="btnSave_Click" />&nbsp;
                    <asp:Button ID="btnReturn" CssClass="classname" runat="server" Text="Return" 
                        onclick="btnReturn_Click" />&nbsp;
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" Text="Delete" 
                        onclick="btnDelete_Click" />&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
