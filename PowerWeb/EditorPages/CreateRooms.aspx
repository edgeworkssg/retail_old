<%@ Page Language="C#" AutoEventWireup="true" Inherits="WebSample.CreateRooms" CodeBehind="CreateRooms.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Create Rooms</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 600px;">
        <div style="font-size: 1px; line-height: 1px; background-color: #C3D9FF; height: 1px;
            margin-left: 2px; margin-right: 2px;">
        </div>
        <div style="font-size: 1px; line-height: 1px; background-color: #C3D9FF; height: 1px;
            margin-left: 1px; margin-right: 1px;">
        </div>
        <div style="background-color: #C3D9FF; padding: 10px; font-family: Arial, Sans-Serif;
            font-size: 12px;">
            <asp:LinkButton runat="server" ID="lbBack" Text="<< Back to calendar"></asp:LinkButton>
            <asp:Button runat="server" ID="btnSave" Text="Save" />
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" />
            <asp:Button runat="server" ID="btnDelete" Text="Delete" />
            <br />
            <asp:Label runat="server" ID="lbError" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
            <br />
            <div style="background-color: White; padding: 10px;">
                <div style="font-size: 1px; line-height: 1px; background-color: #D2E6D2; height: 1px;
                    margin-left: 2px; margin-right: 2px;">
                </div>
                <div style="font-size: 1px; line-height: 1px; background-color: #D2E6D2; height: 1px;
                    margin-left: 1px; margin-right: 1px;">
                </div>
                <div style="background-color: #D2E6D2;">
                    <table cellpadding="5" cellspacing="0" border="0">
                    <tr>
                            <td>
                                <b>Room ID</b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblRoomID" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Room Name</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbRoomName" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Floor</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbFloor" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Building </b>
                            </td>
                            <td>
                                <%--<mc:gRecEditor runat="server" ID="recEditor"  />--%>
                                <table border="0" cellpadding="0" cellspacing="5" width="100%">
                                    <tr>
                                        <td style="border-bottom: solid 2px #E0F1E0; padding: 3px;">
                                            <asp:DropDownList runat="server" ID="ddBuilding" AutoPostBack="true" Font-Names="Arial, Sans-Serif"
                                                Font-Size="12px">
                                                
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                       
                            
                                
                            
                       
                    </table>
                   <asp:GridView ID="grdRooms" runat="server" AutoGenerateEditButton="True" 
                        onrowediting="grdRooms_RowEditing" PageSize="5" ShowFooter="True" Width="393px">
                                </asp:GridView>
                    <tr runat="server" id="trRecEndType">
                        <td style="border-bottom: solid 2px #E0F1E0; padding: 3px;">
                        </td>
                    </tr>
                   
                </div>
                <div style="font-size: 1px; line-height: 1px; background-color: #D2E6D2; height: 1px;
                    margin-left: 1px; margin-right: 1px;">
                </div>
                <div style="font-size: 1px; line-height: 1px; background-color: #D2E6D2; height: 1px;
                    margin-left: 2px; margin-right: 2px;">
                </div>
            </div>
        </div>
        <div style="font-size: 1px; line-height: 1px; background-color: #C3D9FF; height: 1px;
            margin-left: 1px; margin-right: 1px;">
        </div>
        <div style="font-size: 1px; line-height: 1px; background-color: #C3D9FF; height: 1px;
            margin-left: 2px; margin-right: 2px;">
        </div>
    </div>
    </form>
</body>
</html>
