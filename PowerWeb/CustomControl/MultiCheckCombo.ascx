<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiCheckCombo.ascx.cs" Inherits="SRL.UserControls.MultiCheckCombo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type = "text/javascript"> 
//Script para incluir en el ComboBox1 cada item chekeado del chkListMateriales
function CheckItem(checkBoxList)
{
//        var options = checkBoxList.getElementsByTagName('input');
//        var arrayOfCheckBoxLabels = checkBoxList.getElementsByTagName("label");

    var options = document.getElementById("<%=chkList.ClientID %>").getElementsByTagName("input");
    var arrayOfCheckBoxLabels = document.getElementById("<%=chkList.ClientID %>").getElementsByTagName("label");
    
    var s = "";
    
    for(i=0;i<options.length;i++)
    {
        var opt = options[i];
        if(opt.checked)
        {
            s = s + ", " + arrayOfCheckBoxLabels[i].textContent;
        } 
    }
    if(s.length > 0) 
    {       
       s = s.substring(2, s.length); //sacar la primer 'coma'
    }
    var TxtBox = document.getElementById("<%=txtCombo.ClientID%>");       
    TxtBox.value = s;
    document.getElementById('<%=hidVal.ClientID %>').value = s;
}
</script>


<asp:TextBox ID="txtCombo" runat="server" ReadOnly="true" Width="178px" Font-Size="X-Small"></asp:TextBox>
<cc1:PopupControlExtender ID="PopupControlExtender111" runat="server" 
    TargetControlID="txtCombo" PopupControlID="Panel111" Position="Bottom" >
</cc1:PopupControlExtender>

<input type="hidden" name="hidVal" id="hidVal" runat="server" />

<asp:Panel ID="Panel111" runat="server" ScrollBars="Vertical" Width="178px" 
    Height="429px" BackColor="AliceBlue" BorderColor="Gray" BorderWidth="1">
    
    <asp:CheckBoxList ID="chkList" 
        runat="server" 
        Height="150" onclick="CheckItem(this)">                                                                                                                                                                        
    </asp:CheckBoxList>
    
</asp:Panel>


