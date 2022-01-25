<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="LineSheetResult" Title="Line Sheet Result" Codebehind="LineSheetResult.aspx.cs" %>



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
    
    
    
    
    <table width="1024px">
        
        
        <tr>            <td colspan=2 align=right class="ExportButton">
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
                            <asp:BoundField DataField="Attributes4" HeaderText="P C N" />                    
                            <asp:BoundField DataField="ItemNo" HeaderText="Item No"></asp:BoundField>	            
	                        <asp:BoundField DataField="ItemName" HeaderText="Item Name"></asp:BoundField>	 
	                        <asp:BoundField DataField="Attributes1" HeaderText="Article No" />
                            <asp:BoundField DataField="Attributes2" HeaderText="Colour Description" />
                            <asp:BoundField DataField="Attributes3" HeaderText="Size" />           	            
	                        	            	            	            	            	            	            
	                        <asp:BoundField DataField="FactoryPrice" HeaderText="STD COST"></asp:BoundField>	
	                        <asp:BoundField DataField="RetailPrice" HeaderText="RRP W GST"></asp:BoundField>            	            	            	            	            	            
	                        
                            
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
</td>
            <td colspan=2 align=right class="ExportButton">
                </td>
        </tr>
    </table>    
</asp:Content>

