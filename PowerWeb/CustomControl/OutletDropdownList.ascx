<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutletDropdownList.ascx.cs"
    Inherits="SRL.UserControls.OutletDropdownList" %>

<td>
    <asp:Label ID="lblPOS" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Label>
</td>
<td>
    <asp:DropDownList ID="ddlPOS" runat="server" Width="179px">
    </asp:DropDownList>
</td>
<td>
    <asp:Label ID="lblOutlet" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Label>
</td>
<td>
    <asp:DropDownList ID="ddlOutlet" runat="server" Width="179px" OnInit="ddlOutlet_OnInit" OnSelectedIndexChanged="ddlOutlet_OnSelectedIndexChanged" AutoPostBack="True" >
    </asp:DropDownList>
</td>

