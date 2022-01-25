<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="CRReport.aspx.cs" Inherits="PowerWeb.CRReport.CRReport" EnableEventValidation="false" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager id="ScriptManager1" runat="server" />
    <script type="text/javascript">
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=500,width=800,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }
    </script>    
   <%-- <div style="position:relative;z-index:2;">
    <table id="FilterTable" runat="server" width="1122px">
        <tr><td colspan="4" class="wl_pageheaderSub" style="width: 100%"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Search %>"></asp:Literal></td></tr>
        <tr>
            <td colspan="2" >
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" /></td> 
            <td colspan="2" align="right" class="ExportButton">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:dictionary, Export to: %>" />
                <asp:LinkButton ID="lnkExportRaw" runat="server" ToolTip="Data Only" OnClick="lnkExportRaw_Click">
                    <img id="ImgGrid" src="../App_Themes/Default/image/CSV.jpg" width="20" height="20" /></asp:LinkButton>
                <asp:LinkButton ID="lnkExportExcel" runat="server" ToolTip="Excel" OnClick="lnkExportExcel_Click">
                    <img id="ImgExcel" src="../App_Themes/Default/image/Excel.jpg" width="20" height="20" /></asp:LinkButton>
                <asp:LinkButton ID="lnkExportPDF" runat="server" ToolTip="PDF" OnClick="lnkExportPDF_Click">
                    <img id="ImgPDF" src="../App_Themes/Default/image/PDF.jpg" width="20" height="20" /></asp:LinkButton></td>
        </tr>
    </table>
    </div>--%>
    <table style="width:1112px;" class="wl_bodytxt">
    <tr>
         <td>
            <table id="FilterTable" runat="server"  style="vertical-align:middle;width:1112px;"  border="0" cellpadding="0" cellspacing="0">
             <tr> 
                <td  colspan="4" class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Search %>"></asp:Literal></td>                
              </tr>        
            </table>
            <table id="search_ExportTable" style="vertical-align:middle;width:1112px;height:40px;" border="0" cellpadding="2" cellspacing="0">
            <tr>                
                <td style="height:30px;width:400px; background-color:#FFFFFF; left:0;vertical-align :middle ;" >
                    <asp:LinkButton  ID="LinkButton1"  class="classname"  runat="server"  OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:dictionary, Search%>" /> </asp:LinkButton><div class="divider">   </div>
                    <asp:LinkButton  ID="LinkButton4" class="classname" runat="server" ><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton></td><td align="right" style="height:30px;width:708px; background-color:#FFFFFF;padding-right:0px;vertical-align:middle;right:0px;">
                     <asp:Literal ID="Literal6" runat="server"  Text="<%$ Resources:dictionary, Export to: %>" />
                    <asp:LinkButton  ID="LinkButton3" class="classBlue"  runat="server" OnClick="lnkExportRaw_Click" >  <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary,Data Only%>" /></asp:LinkButton><div class="divider" > </div>  
                      <asp:LinkButton  ID="LinkButton2" class="classBlue"  runat="server" OnClick="lnkExportExcel_Click" >  <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:dictionary, Excel%>" /></asp:LinkButton><div class="divider" > </div>  
                    <asp:LinkButton  ID="LinkButton5" class="classBlue" runat="server" OnClick="lnkExportPDF_Click" >   <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:dictionary, PDF%>" /></asp:LinkButton></td></tr><tr>
                <td colspan="4" style="height:25px;"> <asp:Literal ID="litMessage" runat="server" Text="" /> </td>
            </tr>
            </table>
          </td>
      </tr>
      </table>
      <table id="Table1" runat="server" style="width:1118px;">
      <tr>
        <td colspan="4" class="wl_pageheaderSubReports" style="width: 100%">
        <asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary, Reports %>"></asp:Literal></td></tr></table><br/>
        <asp:Label ID="lblMsg" runat="server" /><br/>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"  
            DisplayGroupTree="False" Width="100%" Height="50px" 
            HasCrystalLogo="False" HasExportButton="False" OnNavigate="CR_Navigate" OnSearch="CR_Search" HasViewList="False"/>
            <!---->
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>

    <script src="../Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ShowLastWeekDate() {
            var d = new Date();
            d.setDate(d.getDate() - d.getDay());
            $('input#ctl00_ContentPlaceHolder1_EndDate').val(d.format("dd MMM yyyy"));
            $find('calExtEndDate').set_selectedDate(d);
            d.setDate(d.getDate() - 6);
            $('input#ctl00_ContentPlaceHolder1_StartDate').val(d.format("dd MMM yyyy"));
            $find('calExtStartDate').set_selectedDate(d);
            $('#ctl00_ContentPlaceHolder1_rdbMonthEndDate').removeAttr('checked');
            $('#ctl00_ContentPlaceHolder1_rdbStartDate').attr('checked', 'checked');
        };
    
        $(document).ready(function() {
            $('#ctl00_ContentPlaceHolder1_ddlEndDate').change(function() {
                $('#ctl00_ContentPlaceHolder1_rdbStartDate').removeAttr('checked');
                $('#ctl00_ContentPlaceHolder1_rdbMonthEndDate').attr('checked', 'checked');
            });

            $('#ctl00_ContentPlaceHolder1_ddlEndYear').change(function() {
                $('#ctl00_ContentPlaceHolder1_rdbStartDate').removeAttr('checked');
                $('#ctl00_ContentPlaceHolder1_rdbMonthEndDate').attr('checked', 'checked');
            });

            $('input#ctl00_ContentPlaceHolder1_StartDate').click(function() {
                $('#ctl00_ContentPlaceHolder1_rdbMonthEndDate').removeAttr('checked');
                $('#ctl00_ContentPlaceHolder1_rdbStartDate').attr('checked', 'checked');
            });

            $('input#ctl00_ContentPlaceHolder1_EndDate').click(function() {
                $('#ctl00_ContentPlaceHolder1_rdbMonthEndDate').removeAttr('checked');
                $('#ctl00_ContentPlaceHolder1_rdbStartDate').attr('checked', 'checked');
            });

            $('input#ctl00_ContentPlaceHolder1_btnStartDate').click(function() {
                $('#ctl00_ContentPlaceHolder1_rdbMonthEndDate').removeAttr('checked');
                $('#ctl00_ContentPlaceHolder1_rdbStartDate').attr('checked', 'checked');
            });

            $('input#ctl00_ContentPlaceHolder1_btnEndDate').click(function() {
                $('#ctl00_ContentPlaceHolder1_rdbMonthEndDate').removeAttr('checked');
                $('#ctl00_ContentPlaceHolder1_rdbStartDate').attr('checked', 'checked');
            });
            
            $('input#ctl00_ContentPlaceHolder1_btnShowLastWeek').click(function() {
                var d = new Date();
                d.setDate(d.getDate() - d.getDay());
                $('input#ctl00_ContentPlaceHolder1_EndDate').val(d.format("dd MMM yyyy"));
                $('input#ctl00_ContentPlaceHolder1_CalendarExtender2').set_selectedDate(d);
                d.setDate(d.getDate() - 6);
                $('input#ctl00_ContentPlaceHolder1_StartDate').val(d.format("dd MMM yyyy"));
                $('input#ctl00_ContentPlaceHolder1_CalendarExtender1').set_selectedDate(d);

                $('#ctl00_ContentPlaceHolder1_rdbMonthEndDate').removeAttr('checked');
                $('#ctl00_ContentPlaceHolder1_rdbStartDate').attr('checked', 'checked');
            });
            
            $('div.crystalstyle').css('z-index', '1');
        });
    </script>

</asp:Content>