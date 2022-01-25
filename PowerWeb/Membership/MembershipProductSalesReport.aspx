<%@ Page Language="C#" Theme="Default" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="MembershipProductSalesReport" Title="<%$Resources:dictionary,Membership Product Sales Report %>"
    CodeBehind="MembershipProductSalesReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <ajax:UpdateProgress runat="server" ID="progress1" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <img src="../App_Themes/Default/image/indicator_mozilla_blu.gif" />
            <span style="color: #0000ff"><b>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Update in progress%>" /></b><br />
            </span>
        </ProgressTemplate>
    </ajax:UpdateProgress>
    <ajax:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
                PopupButtonID="IBStartDate" TargetControlID="txtStartDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
                PopupButtonID="IBEndDate" TargetControlID="txtEndDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Animated="False" Format="dd MMM yyyy"
                PopupButtonID="IBStartExpiryDate" TargetControlID="txtStartExpiryDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Animated="False" Format="dd MMM yyyy"
                PopupButtonID="IBEndExpiryDate" TargetControlID="txtEndExpiryDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Animated="False" Format="dd MMM yyyy"
                PopupButtonID="IBStartBirthDate" TargetControlID="txtStartBirthDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="CalendarExtender6" runat="server" Animated="False" Format="dd MMM yyyy"
                PopupButtonID="IBEndBirthDate" TargetControlID="txtEndBirthDate">
            </cc1:CalendarExtender>
            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="LabelMessage"></asp:Label>
            <div style="height: 20px; width: 700px;" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </div>
            <table width="700px" class="wl_bodytxt">
                <%-- <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="Filter Sales By Date"></asp:Literal></td></tr>
        --%><tr>
            <td style="width: 102px; height: 3px">
            </td>
            <td style="height: 3px">
                <asp:CheckBox ID="cbFilterByDate" runat="server" Text="<%$Resources:dictionary, Filter By Date%>" />
            </td>
            <td style="height: 3px">
            </td>
            <td style="height: 3px">
            </td>
        </tr>
                <tr>
                    <td style="width: 102px; height: 3px">
                        <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                            Text="<%$ Resources:dictionary, Start Date %>" />
                    </td>
                    <td style="height: 3px">
                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="IBStartDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                    </td>
                    <td style="height: 3px">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
                    </td>
                    <td style="height: 3px">
                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="IBEndDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 102px">
                        <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                            Width="68px" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" Width="122px">
                            <asp:ListItem Value="1">January</asp:ListItem>
                            <asp:ListItem Value="2">February</asp:ListItem>
                            <asp:ListItem Value="3">March</asp:ListItem>
                            <asp:ListItem Value="4">April</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">June</asp:ListItem>
                            <asp:ListItem Value="7">July</asp:ListItem>
                            <asp:ListItem Value="8">August</asp:ListItem>
                            <asp:ListItem Value="9">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblYear" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <table width="700px" class="wl_bodytxt">
                <tr>
                    <td colspan="4" class="wl_pageheaderSub">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Start Subscription Date %>"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStartExpiryDate" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="IBStartExpiryDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                        &nbsp;<asp:CheckBox ID="cbUseStartExpiryDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
                    </td>
                    <td>
                        <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,End Subscription Date %>"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEndExpiryDate" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="IBEndExpiryDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                        &nbsp;<asp:CheckBox ID="cbUseEndExpiryDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Start Birth Date %>"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStartBirthDate" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="IBStartBirthDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                        &nbsp;<asp:CheckBox ID="cbUseStartBirthDate" runat="server" Text="<%$ Resources:dictionary, Use Start Date %>" />
                    </td>
                    <td>
                        <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,End Birth Date %>"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEndBirthDate" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="IBEndBirthDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                        &nbsp;<asp:CheckBox ID="cbUseEndBirthDate" runat="server" Text="<%$ Resources:dictionary, Use End Date %>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary, Birthday Month %>"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server" Width="120px">
                            <asp:ListItem Value="1">January</asp:ListItem>
                            <asp:ListItem Value="2">February</asp:ListItem>
                            <asp:ListItem Value="3">March</asp:ListItem>
                            <asp:ListItem Value="4">April</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">June</asp:ListItem>
                            <asp:ListItem Value="7">July</asp:ListItem>
                            <asp:ListItem Value="8">August</asp:ListItem>
                            <asp:ListItem Value="9">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <asp:CheckBox ID="cbUseBirthDayMonth" runat="server" Text="<%$ Resources:dictionary, Use Birthday Month %>" />
                    </td>
                    <td>
                        <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Gender %>"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGender" runat="server">
                            <asp:ListItem Value="">ALL</asp:ListItem>
                            <asp:ListItem Value="M">Male</asp:ListItem>
                            <asp:ListItem Value="F">Female</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMembershipNo" runat="server" Width="172px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,NRIC %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNRIC" runat="server" Width="172px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Card Type %>"></asp:Literal>
                        </td>
                        <td>
                            <subsonic:DropDown ID="ddGroupName" runat="server" OrderField="GroupName" PromptValue="0"
                                ShowPrompt="True" TableName="MembershipGroup" TextField="GroupName" ValueField="MembershipGroupID"
                                Width="175px">
                            </subsonic:DropDown>
                        </td>
                        <td>
                            <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Name %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNameToAppear" runat="server" Width="172px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px">
                            <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Address %>"></asp:Literal>
                        </td>
                        <td style="height: 15px">
                            <asp:TextBox ID="txtStreetName" runat="server" Width="172px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
            </table>
            <table width="700px" class="wl_bodytxt">
                <tbody>
                    <tr>
                        <td class="wl_pageheaderSub" colspan="4">
                            <asp:Literal ID="AddItemstobeStockOut" runat="server" Text="<%$Resources:dictionary,Select Items %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 99px">
                            <asp:Literal ID="ItemNoLbl" runat="server" Text="<%$Resources:dictionary,Item No %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemNo" runat="server" Width="100px" ValidationGroup="AddItem"
                                CausesValidation="True"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnOkItemNo" OnClick="btnOkItemNo_Click" CssClass="classname"
                                runat="server" Text="<%$ Resources:dictionary, Ok %>" Width="41px" ValidationGroup="AddItem">
                            </asp:Button>
                        </td>
                        <td style="width: 104px">
                            <asp:Literal ID="ScanBarcodeLbl" runat="server" Text="<%$Resources:dictionary,Scan Barcode %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemBarcode" runat="server" Width="100px" ValidationGroup="AddItem"
                                CausesValidation="True"></asp:TextBox>
                            <asp:Button ID="btnOK" CssClass="classname" OnClick="btnOK_Click" runat="server"
                                Text="<%$ Resources:dictionary, Ok %>" Width="41px" ValidationGroup="AddItem">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 104px">
                            <asp:Literal ID="ItemNameLbl" runat="server" Text="<%$Resources:dictionary,Item Name %>"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <subsonic:DropDown ID="ddlName" runat="server" OnSelectedIndexChanged="ddlName_SelectedIndexChanged"
                                AutoPostBack="True" Width="443px" TableName="Item" ValueField="ItemNo" TextField="ItemName"
                                ShowPrompt="True" ValidationGroup="AddItem" CausesValidation="True">
                            </subsonic:DropDown>
                        </td>
                    </tr>
                    <tr>
                        <td class="wl_pageheaderSub" colspan="4">
                            <asp:Literal ID="InventoryDetailsLbl" runat="server" Text="<%$Resources:dictionary,Item List %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="" colspan="4">
                            <asp:GridView ID="gvDetails" runat="server" Width="100%" OnRowDataBound="gvDetails_RowDataBound"
                                OnRowDeleting="gvDetails_RowDeleting" DataKeyNames="ItemNo" AutoGenerateColumns="False"
                                SkinID="scaffold">
                                <Columns>
                                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="false" ValidationGroup="EditLine" />
                                    <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>" />
                                    <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Item Name%>" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </ajax:UpdatePanel>
    <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="classname" runat="server"
        Text="<%$ Resources:dictionary, Submit %>"></asp:Button>
    <asp:Button ID="btnClear" OnClick="btnClear_Click" CssClass="classname" runat="server"
        Text="<%$ Resources:dictionary, Clear %>"></asp:Button>
</asp:Content>
