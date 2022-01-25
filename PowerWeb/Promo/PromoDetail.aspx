<%@ Page Language="C#" AutoEventWireup="true" Inherits="PromoDetail" Codebehind="PromoDetail.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:Literal ID = "OrderDetailTitle" runat="server" Text="<%$Resources:dictionary,Order Detail %>"></asp:Literal></title>
</head>
<body>
    <form id="form1" runat="server">
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>    
    <div>
    <table width=350px>
        <tr>
            <td  ><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Campaign Name %>"></asp:Literal></td>                       
            <td style="width: 182px">            
                <asp:Label ID="lblCampaignName" runat="server" Text="-" Width="113px"></asp:Label></td>
        </tr>
        <tr>
            <td  ><asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal></td>
            <td style="width: 182px" >
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                </td>
        </tr>
        <tr>
            <td  style="height: 18px;"><asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal></td><td style="width: 182px" >
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                </td>
        </tr>
        <tr>
            <td  style="height: 18px; ">
                <asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Campaign Type %>"></asp:Literal></td>
            <td style="width: 182px" >
                <asp:Label ID="lblCampaignType" runat="server" Text="-" Width="113px"></asp:Label></td>
        </tr>
        <tr>
            <td  style="height: 18px;">
                <asp:Literal ID = "Literal8" runat="server" Text="<%$Resources:dictionary,For Members Only? %>"></asp:Literal></td>
            <td style="height: 18px; width: 182px;" >
                <asp:RadioButton GroupName="a" ID="rbYes" runat="server" Text="Yes" />
                <asp:RadioButton GroupName="a" ID="rbNo" runat="server" Text="No" /></td>
        </tr>
        <tr>
            <td colspan=2>
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label></td>            
        </tr>
        <tr>
            <td colspan=2>
                <asp:Button ID="btnSave" runat="server" Enabled="False" OnClick="btnSave_Click" Text="Save" /></td>            
        </tr>
    </table>
    </div>
        <asp:HiddenField ID="hdfID" runat="server" />
    </form>
</body>
</html>
