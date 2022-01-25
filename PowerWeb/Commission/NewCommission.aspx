<%@ Page Title="Commission Setup" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="NewCommission.aspx.cs" Inherits="PowerWeb.Commission.NewCommission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <asp:Panel ID="PanelList" runat="server">
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Add New%>"
            Width="130px" ID="BtnAdd" OnClick="btnAdd_Click" />
        <div class="divider">
        </div>
        <div style="height: 10px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SkinID="scaffold"
            Width="400px" EmptyDataText="Commission Setup is empty">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="CommissionHdrID" DataNavigateUrlFormatString="NewCommission.aspx?id={0}"
                    Text="Edit">
                    <HeaderStyle Width="80px" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="SchemeName" HeaderText="Scheme Name" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="PanelInput" runat="server">
        <table cellpadding="5" cellspacing="0" width="1000">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Literal ID="Literal2" runat="server" Text="Scheme Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="tbSchemeName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *Scheme Name is required"
                                    ControlToValidate="tbSchemeName"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="width: 150px">
                                <asp:CheckBox ID="cbProduct" runat="server" Text="Product" />
                            </td>
                            <td style="width: 20px">
                                <asp:TextBox ID="tbProduct" runat="server" Width="50px" style="text-align:right;"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                %
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbService" runat="server" Text="Service" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbService" runat="server" Width="50px" style="text-align:right;"></asp:TextBox>
                            </td>
                            <td>
                                %
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbPointSold" runat="server" Text="Point Sold" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbPointSold" runat="server" Width="50px" style="text-align:right;"></asp:TextBox>
                            </td>
                            <td>
                                %
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbPackageSold" runat="server" Text="Package Sold" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbPackageSold" runat="server" Width="50px" style="text-align:right;"></asp:TextBox>
                            </td>
                            <td>
                                %
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbPointRedeem" runat="server" Text="Point Redeem" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbPointRedeem" runat="server" Width="50px" style="text-align:right;"></asp:TextBox>
                            </td>
                            <td>
                                %
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbPackageRedeem" runat="server" Text="Package Redeem" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbPackageRedeem" runat="server" Width="50px" style="text-align:right;"></asp:TextBox>
                            </td>
                            <td>
                                %
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="Only for specific :"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="800px">
                        <tr>
                            <td style="vertical-align: top;">
                                <ajax:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvDetail" runat="server" Width="400px" AutoGenerateColumns="False"
                                            SkinID="scaffold" EmptyDataText="Commission Detail is empty" OnRowDeleting="gvDetail_RowDeleting">
                                            <Columns>
                                                <asp:CommandField ShowDeleteButton="True">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:CommandField>
                                                <asp:BoundField DataField="CategoryName" HeaderText="Category Name" />
                                                <asp:BoundField DataField="ItemNo" HeaderText="ItemNo" />
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <ajax:AsyncPostBackTrigger ControlID="btnAddCommissionFor" EventName="Click" />
                                    </Triggers>
                                </ajax:UpdatePanel>
                            </td>
                            <td style="vertical-align: top; padding-left: 25px;">
                                <table>
                                    <tr>
                                        <td style="width: 120px;">
                                            Category Name
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px;">
                                            Item Name
                                        </td>
                                        <td>
                                            <ajax:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlItem" runat="server">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <ajax:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </ajax:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 10px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: left;">
                                            <asp:Button ID="btnAddCommissionFor" runat="server" Text="Add" OnClick="btnAddCommissionFor_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbDeduction" runat="server" Text="Deduction for 2<sup>nd</sup> salesperson" /><br />
                    <div style="margin-left: 15px">
                        <table>
                            <tr>
                                <td style="width: 150px;">
                                    <asp:RadioButton ID="rbDeductionByPercentage" runat="server" Text="By Percentage"
                                        GroupName="DeductionBy" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbDeductionByPercentage" runat="server" Width="50px" style="text-align:right;"></asp:TextBox>
                                </td>
                                <td>
                                    %
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">
                                    <asp:RadioButton ID="rbDeductionByAmount" runat="server" Text="By Amount" GroupName="DeductionBy" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbDeductionByAmount" runat="server" Width="50px" style="text-align:right;"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <td style="width: 150px">
                                    <asp:Literal ID="Literal5" runat="server" Text="Apply to Sales Group "></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSalesGroup" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="800px">
                        <tr>
                            <td>
                                <asp:RadioButton ID="rbCommissionByPercentage" runat="server" Text="Commission By Percentage"
                                    GroupName="CommissionBy" OnCheckedChanged="rbCommissionBy_CheckedChanged" AutoPostBack="True" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbCommissionByQuantity" runat="server" Text="Commission By Quantity"
                                    GroupName="CommissionBy" OnCheckedChanged="rbCommissionBy_CheckedChanged" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ajax:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <Triggers>
                                        <ajax:AsyncPostBackTrigger ControlID="rbCommissionByPercentage" EventName="CheckedChanged" />
                                        <ajax:AsyncPostBackTrigger ControlID="rbCommissionByQuantity" EventName="CheckedChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td style="text-align: center">
                                                    Amt.From
                                                </td>
                                                <td>
                                                </td>
                                                <td style="text-align: center">
                                                    Amt.To
                                                </td>
                                                <td>
                                                </td>
                                                <td colspan="2" style="text-align: center">
                                                    Percentage
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCPFrom1" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPTo1" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPValue1" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    %
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCPFrom2" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPTo2" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPValue2" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    %
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCPFrom3" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPTo3" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPValue3" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    %
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCPFrom4" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPTo4" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPValue4" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    %
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCPFrom5" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPTo5" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCPValue5" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    %
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajax:UpdatePanel>
                            </td>
                            <td>
                                <ajax:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <Triggers>
                                        <ajax:AsyncPostBackTrigger ControlID="rbCommissionByPercentage" EventName="CheckedChanged" />
                                        <ajax:AsyncPostBackTrigger ControlID="rbCommissionByQuantity" EventName="CheckedChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td style="text-align: center">
                                                    Qty.From
                                                </td>
                                                <td>
                                                </td>
                                                <td style="text-align: center">
                                                    Qty.To
                                                </td>
                                                <td>
                                                </td>
                                                <td style="text-align: center">
                                                    Amount
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCQFrom1" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQTo1" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQValue1" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCQFrom2" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQTo2" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQValue2" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCQFrom3" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQTo3" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQValue3" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCQFrom4" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQTo4" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQValue4" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="tbCQFrom5" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    -
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQTo5" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    =
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCQValue5" runat="server" Width="60px" style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajax:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$ Resources:dictionary, Save %>" />
                    <asp:Button ID="btnSaveNew" runat="server" CssClass="classname" Text="<%$Resources:dictionary, Save and New%>"
                        OnClick="btnSaveNew_Click" />
                    <asp:Button ID="btnReturn" runat="server" CssClass="classname" Text="<%$Resources:dictionary, Return%>"
                        OnClick="btnReturn_Click" CausesValidation="false" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$ Resources:dictionary, Delete %>" OnClientClick="javascript:return confirm('Are you sure?');" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <script type="text/javascript">
        jQuery(function($) {
            var rbCommissionBy = $("#rbCommissionBy").val();
            if (rbCommissionBy == "Quantity") {
                $("#<%= rbCommissionByQuantity.ClientID %>").prop("checked", true);
            } else if (rbCommissionBy == "Percentage") {
                $("#<%= rbCommissionByPercentage.ClientID %>").prop("checked", true);
            }
        });
    </script>

</asp:Content>
