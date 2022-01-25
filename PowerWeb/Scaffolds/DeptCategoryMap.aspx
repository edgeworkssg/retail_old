<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="DeptCategoryMap" Title="Assign Category to Department" Codebehind="DeptCategoryMap.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
   <div>Group Name: <subsonic:DropDown ID="DropDown1" runat="server" 
   TableName="Department" TextField="DepartmentName" ValueField="DepartmentID" OnSelectedIndexChanged="DropDown1_SelectedIndexChanged" ShowPrompt=true AutoPostBack="True">
   </subsonic:DropDown></div>
   
   <subsonic:ManyManyList ID="ManyManyList1" runat="server" 
   MapTableName="DeptCategoryMap" ForeignTableName="Category" PrimaryTableName="Department" ProviderName="PowerPOS" Height="126px" RepeatColumns="3" Width="652px" ForeignTextField="CategoryName">
   </subsonic:ManyManyList><br />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
</asp:Content>

