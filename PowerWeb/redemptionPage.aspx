<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="redemptionPage.aspx.cs" Inherits="PowerWeb.redemptionPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Redeem Points</title>
    <style type="text/css">
        .style1
        {
            font-weight: bold;
        }
        .style2
        {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="600px">
        <tr>
            <td class="style1">Card No</td><td class="style2">
            <asp:Label ID="lblMembershipNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">Name</td><td class="style2">
            <asp:Label ID="lblMembershipName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">Points</td><td class="style2">
            <asp:Label ID="lblPoints" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">Contact No</td><td class="style2">
            <asp:TextBox ID="txtContactNo" runat="server" Width="319px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtContactNo" ErrorMessage="Contact No required!"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style1">Delivery Address</td><td class="style2">
                    <asp:TextBox ID="txtDeliveryAddress" runat="server" Height="91px" 
                        TextMode="MultiLine" Width="323px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtDeliveryAddress" ErrorMessage="Address Required!"></asp:RequiredFieldValidator>
                    </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns= "False"  DataKeyNames="RedemptionID"        
         SkinID=scaffold
            Width="600px" EnableModelValidation="True" onrowcommand="GridView1_RowCommand">
            <Columns>
                <asp:ButtonField ButtonType="Button" Text="Redeem" >
                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                </asp:ButtonField>                
                <asp:BoundField DataField="Description" HeaderText="<%$Resources:dictionary,Description%>" SortExpression="Description" />
                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Item Name%>" SortExpression="ItemName" />
                <asp:BoundField DataField="PointRequired" 
                    HeaderText="<%$Resources:dictionary,Points Required%>" 
                    SortExpression="PointsRequired" >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="RetailPrice" 
                    HeaderText="<%$Resources:dictionary,Retail Price%>" 
                    SortExpression="RetailPrice" >            
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>    
    </div>
    </form>
</body>
</html>
