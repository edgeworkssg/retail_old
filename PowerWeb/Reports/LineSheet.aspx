<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="LineSheet" Title="Line Sheet" Codebehind="LineSheet.aspx.cs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript"> 
        function toggle() {
	        var ele = document.getElementById("HelpBox");
	        if(ele.style.display == "block") {
    		        ele.style.display = "none";
  	        }
	        else {
		        ele.style.display = "block";
	        }
        } 
    </script>
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    
    
    <div id="HelpBox" style="position:absolute; display:none; left:50px; width:300px; background-color:Gray; padding:10px 10px 10px 10px; ">
        <a href="javascript:toggle();" style="position:inherit; right:10px; top:10px;" >
            <img id="btnCloseHelp" src="../Images/close.gif" />
        </a>
        
        <h1>HELP</h1>
        <p>
            Tick the selected items, scroll to the bottom of the page, and press the <b>Create Line Sheet </b> button <br />
        </p>
    </div>
    
    <table width="1024px">
        <tr><td colspan=4 class="searchHeader"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        
        <tr>
        <td class="fieldname"> Search</td><td>
                <asp:TextBox ID="txtSearch" runat="server" Width="165px"></asp:TextBox>
    
            </td><td class="fieldname">  
                                &nbsp;</td><td>
                &nbsp;</td>
        </tr>
        
        <tr><td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" />
            <button type="button" causesvalidation="false" onclick="javascript:toggle();">Help</button>
            </td>
            <td colspan=2 align=right class="ExportButton">
            <asp:LinkButton ID="lnkSelectAll" runat="server" OnClick="lnkSelectAll_Click" Text="Select All"></asp:LinkButton>
                , <asp:LinkButton ID="lnkSelectNone" runat="server" OnClick="lnkSelectNone_Click" Text="Select None"></asp:LinkButton>
                , <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
                </td>
        </tr>
        <tr>
            <td colspan=4>
                <div>
                    <asp:Label ID="lblResult" runat="server"></asp:Label>
                    <br />
                    <asp:GridView ID="gvReport" Width="100%" runat="server" SkinID="scaffold" 
                            AutoGenerateColumns="False" AllowSorting="True"
                            OnRowDataBound="gvReport_RowDataBound" 
                            OnDataBound="gvReport_DataBound" 
                            OnSorting="gvReport_Sorting">
                        <Columns>                                    
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>"></asp:BoundField>	            
	                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Product Name %>"></asp:BoundField>	            	            
	                        <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category %>"></asp:BoundField>	            	            	            	            
	                        <asp:BoundField DataField="RetailPrice" HeaderText="<%$Resources:dictionary,Retail Price %>"></asp:BoundField>	            	            	            	            	            	            
	                        <asp:BoundField DataField="FactoryPrice" HeaderText="Factory Price"></asp:BoundField>	            	            	            	            	            	            
	                        <asp:BoundField DataField="Attributes1" HeaderText="Article No" />
                            <asp:BoundField DataField="Attributes2" HeaderText="Colour Description" />
                            <asp:BoundField DataField="Attributes3" HeaderText="Size" />
                            <asp:BoundField DataField="Attributes4" HeaderText="P C N" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr><td colspan=2 >
                        <asp:Button ID="btnAdd" runat="server" onclick="btnAdd_Click" 
                Text="Add To List"  Width="110px" Height="35px"  />
                        <asp:Button ID="btnSend" runat="server" onclick="btnSend_Click" 
                Text="Create Line Sheet"  Width="110px" Height="35px"  />
</td>
            <td colspan=2 align=right class="ExportButton">
                </td>
        </tr>
    </table>    
</asp:Content>

