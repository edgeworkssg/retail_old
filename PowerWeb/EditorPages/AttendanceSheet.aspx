<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceSheet.aspx.cs"
    Inherits="WebSample.EditorPages.AttendanceSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Attendance</title>
    
</head>
<body>
    <form id="form1" runat="server">
     <script type="text/javascript">
        function SelectAll(id)
        {
            //get reference of GridView control
            var grid = document.getElementById("<%= this.grdAttendance.ClientID %>");
            //var grid = $("<%= this.grdAttendance.ClientID %>");
            //variable to contain the cell of the grid
            var cell;
            
            if (grid.rows.length > 0)
            {
                //loop starts from 1. rows[0] points to the header.
                for (i=1; i<grid.rows.length; i++)
                {
                    //get the reference of first column
                    cell = grid.rows[i].cells[2];
                    
                    //loop according to the number of childNodes in the cell
                    for (j=0; j<cell.childNodes.length; j++)
                    {           
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type =="checkbox")
                        {
                        //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
            <asp:Button runat="server" ID="btnSave" Text="Save" onclick="btnSave_Click" />
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
                                <b>Course Name</b>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="lblCourseName" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Membership ID</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbAddrLine1" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                               <%-- <cc1:CalendarExtender ID="tbAddrLine1_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="tbAddrLine1">
                                </cc1:CalendarExtender>--%>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <b>NRIC Number</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbAddrLine2" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px" Visible="False"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <b>Date of Attendance</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAttendDate" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                                <cc1:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" 
                                    Enabled="True" Format="dd/MMM/yyyy" TargetControlID="txtAttendDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <asp:PlaceHolder runat="server" ID="gridplaceholder" Visible="False" >
                    <asp:GridView ID="grdAttendance" runat="server" PageSize="5" ShowFooter="True" Width="393px"
                        AutoGenerateColumns="true" OnRowDataBound="grdAttendance_RowDataBound">
                        <%--<Columns>
                            <asp:BoundField DataField="MemberId" HeaderText="MemberId" />
                            <asp:BoundField DataField="FirstName" HeaderText="FirstName" />
                        </Columns>--%>
                    </asp:GridView>
                    </asp:PlaceHolder>
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
