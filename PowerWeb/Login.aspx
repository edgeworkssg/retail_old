<%@ Page Language="C#" AutoEventWireup="true" Inherits="Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EQUIP WEB LOGIN</title>
    <link href="App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function FocusHandler(x)
    {
      if (x.value==x.getAttribute("title")){
        x.value="";
        x.setAttribute("class", "wl_formInput");
      }
    }    
    function BlurHandler(x)
    {
        if (x.value == "") {
            var title = x.getAttribute("title");
            x.value= title;
            x.setAttribute("class", "wl_waterMark");
        }
    }
    </script>
</head>
<body class="wl_bodytxt">
    <form id="form1" runat="server" defaultbutton="submitBtn" >
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td background="App_Themes/Default/image/New/homeBGgrey.jpg">&nbsp;</td>
    <td width="990" height="540" align="right" valign="top" background="App_Themes/Default/image/New/homeBGgrey.jpg">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="App_Themes/Default/image/New/homeBG.jpg">
      <tr>
        <td height="539">&nbsp;</td>
        <td width="262" valign="top"><table width="262" border="0" cellpadding="0" cellspacing="0" background="App_Themes/Default/image/New/boxTop.gif">
          <tr>
            <td width="47">&nbsp;</td>
            <td height="28">&nbsp;</td>
            </tr>
          <tr>
            <td>&nbsp;</td>
            <td height="60" align="left"><img src="App_Themes/Default/image/New/logoSub.gif" alt="EQuipWeb Logo" width="184" height="54" /></td>
            </tr>
          <tr>
            <td>&nbsp;</td>
            <td height="30px" align="left">
            <span class="wl_bodytxt">
                <asp:TextBox  ID="UserName" class="wl_waterMark" runat="server" Text="User Name" ToolTip="User Name" height="20px"  Width="150px" onfocus="FocusHandler(this)" onblur="BlurHandler(this)"></asp:TextBox>
                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
             <!-- <input name="textfield" type="text" class="wl_formInput" id="textfield" value="User Name" size="33"  />-->
            </span></td>
            </tr>
          <tr>
            <td>&nbsp;</td>
            <td height="30px" align="left"><span class="wl_bodytxt">
              <asp:TextBox ID="Password" class="wl_waterMark" TextMode="Password" Text="Password" runat="server" ToolTip="Password"  height="20px" Width="150px" onfocus="FocusHandler(this)" onblur="BlurHandler(this)"></asp:TextBox>
              <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
              <%--<input name="textfield2" type="text" class="wl_formInput" id="textfield2" value="Password" size="33" />--%>
            </span></td>
            </tr>
            <tr>
            <td>&nbsp;</td>
             <td align="left" colspan="2" style="color: red">
               <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
              </td>
             </tr>
             <tr ><td height="5px"/>
            </tr>
          <tr>
            <td height="30">&nbsp;</td>
            <td height="30" align="left">
               <asp:DropDownList ID="ddlLanguages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLanguages_SelectedIndexChanged">
                    </asp:DropDownList>
             <%-- <select name="select" id="select">
                <option value="English (United States)">English (United States)</option>
                <option value="中文">中文</option>
                </select>--%>
           </td>
            </tr>
          <tr>
            <td height="30px"  >&nbsp;</td>
            <td width="282" height="28" align="left">
           <asp:LinkButton ID="submitBtn" class="classLightBlue"  runat="server" onClick="LoginButton_Click"  ValidationGroup="Login1"  > <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:dictionary, Log In%>" /></asp:LinkButton></td></tr><tr>
            <td colspan="2" valign="bottom">
            <img src="App_Themes/Default/image/New/boxBottom.gif" width="262" height="10" />
            </td>
            </tr>
            </table>
            </td>
            <td width="66">&nbsp;</td>
            </tr>
            </table>
            </td>
            <td align="right" valign="top" background="App_Themes/Default/image/New/homeBGgrey.jpg">&nbsp;</td>
            </tr><tr>
            <td>&nbsp;</td>
            <td height="30"> <span class="wl_footer">Powered By <asp:HyperLink id="edgeworksLink" 
                                                                    NavigateUrl="http://www.edgeworks.com.sg/"
                                                                    Text="Edgeworks"
                                                                    Target="_blank"
                                                                    runat="server"/></span>
            </td>
            <td>&nbsp;</td>
  </tr>
</table>
</form>
</body>
</html>
