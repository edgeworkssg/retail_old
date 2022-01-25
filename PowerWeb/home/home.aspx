<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="home" Title="<%$Resources:dictionary,Welcome %>" Codebehind="home.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<table height=100% width=100%>
<tr><td align=center valign=middle width=100% height=100%>
    <img src="../App_Themes/Default/image/home.jpg" /></td></tr>
</table>

    <asp:Label ID="lblErrorList" runat="server" Text="Notice:" Font-Size="Large" Font-Bold="true" Font-Underline="true" />
    <asp:BulletedList ID="bltErrorList" runat="server" ForeColor="Red" Font-Size="Large" Font-Bold="true" />--%>
    
	<div id="webFrame" style="width:1305px; height:500px; background-color: transparent; margin: 5px auto; position: relative;">

	</div>
 <%--width="100%" height="100%" style="border: 1px solid #e8e8e8;"--%>
    <script src="../Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
	<script language="javascript" type="text/javascript" >
	    $(document).ready(function() {
	        $.ajax({
	            type: "POST",
	            url: 'home.aspx/GetIframeLink',
	            cache: false,
	            dataType: 'json',
	            contentType: "application/json; charset=utf-8",
	            async: true,
	            success: function(result) {
	            $('#webFrame').html('<iframe id="iframeContent" src="' + result.d + '"width="100%" height="100%" style="border: 0px solid #e8e8e8;></iframe>');
	            },
	            error: function(e) {
	                $('#webFrame').html('<p style="margin: 5px;">Sorry, web page can\'t be loaded.</p><p style="margin: 5px;">Error Message [' + e.status + '] : ' + e.statusText + '</p>');
	            }
	        });
	    });
	</script>
</asp:Content>

