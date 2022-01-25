<%@ Page Title="<%$Resources:dictionary,Company Setup %>" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="CompanyScaffold.aspx.cs" Inherits="PowerWeb.Scaffolds.CompanyScaffold" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" language="javascript">
        function previewFile() {
            var preview = document.querySelector('#<%=Image2.ClientID %>');
            var file = document.querySelector('#<%=fuItemPicture.ClientID %>').files[0];
            var reader = new FileReader();

            reader.onloadend = function() {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        }

        function removeFile() {
            var preview = document.querySelector('#<%=Image2.ClientID %>');
            preview.src = "";
            return false;
        }
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=600,width=1000,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td>
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Company Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlCompanyName" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="CompanyID" runat="server" Value="" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Receipt Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlReceiptName" runat="server"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Address %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlStreetName" runat="server" MaxLength="200" Width="431px" Style="margin-bottom: 0px"></asp:TextBox>
                    <br />
                   <asp:TextBox ID="ctrlStreetName2" runat="server" MaxLength="200" Width="431px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Zip Code %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlZipCode" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal16" runat="server" Text="<%$Resources:dictionary,City %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlCity" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Country %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlCountry" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:Literal ID="Literal18" runat="server" Text="<%$Resources:dictionary,Mobile %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlMobile" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal19" runat="server" Text="<%$Resources:dictionary,Fax %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlOffice" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 101px;">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Item Picture %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:FileUpload ID="fuItemPicture" runat="server" Height="21px" Width="218px" onchange="previewFile()" />
                    <br />
                    <asp:Image ID="Image2" runat="server" Width="155px" />
                    <br />
                    <asp:Button ID="btnRemoveImage" runat="server" Text="Remove Image" OnClientClick="javascript:return removeFile();"
                        Visible="False" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$Resources:dictionary, Save%>" />                    
                </td>
            </tr>
        </table>
</asp:Content>
