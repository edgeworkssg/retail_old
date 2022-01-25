<%@ Page Title="<%$Resources:dictionary,Customer Rating Settings %>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="CustomerRatingSettings.aspx.cs" Inherits="PowerWeb.Support.CustomerRatingSetting" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Head" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .accordionHeader
        {
            font-family: Arial,Helvetica,sans-serif;
            color: #FFF;
            font-size: 16px;
            width: 100%;
            padding: 3px 3px 3px 5px;
            height: 23px;
            background: #51A2E4;
            border: solid 1px #87B3D1;
            cursor: pointer;
            background: #4d9ee0; /* Old browsers */
            background: -moz-linear-gradient(top,  #4d9ee0 0%, #69b8f3 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#4d9ee0), color-stop(100%,#69b8f3)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* IE10+ */
            background: linear-gradient(to bottom,  #4d9ee0 0%,#69b8f3 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#4d9ee0', endColorstr='#69b8f3',GradientType=0 ); /* IE6-9 */
        }
        .accordionHeaderSelected
        {
            font-family: Arial,Helvetica,sans-serif;
            color: #FFF;
            font-size: 16px;
            width: 100%;
            padding: 3px 3px 3px 5px;
            height: 23px;
            background: #4A9BDD;
            border: solid 1px #357FB7;
            cursor: pointer;
        }
        .accordionContent
        {
            width: 100%;
            background: #E1E1E1;
            border: solid 1px #D1D1D1;
            padding: 5px;
            width: 788px !important;
            overflow: hidden !important;
            min-height: 200px;
        }
        .ctl00_ContentPlaceHolder1_MyAccordion
        {
            overflow: hidden !important;
        }
    </style>

    <script type="text/javascript">
        function previewFile() {
            var preview = document.querySelector('#<%=Image2.ClientID %>');
            var file = document.querySelector('#<%=fuRatingSystem.ClientID %>').files[0];
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

        function previewFileFeedback() {
            var preview = document.querySelector('#<%=ImageFeedback.ClientID %>');
            var file = document.querySelector('#<%=FileUploadFeedback.ClientID %>').files[0];
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
            newwindow = window.open(url, 'name', 'height=500,width=620,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Panel ID="PanelSetting" runat="server">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="classname" OnClick="btnSave_Click" />
        <br />
        <cc1:Accordion ID="MyAccordion" runat="Server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            AutoSize="Limit" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="800px">
            <Panes>
                <cc1:AccordionPane ID="AccordionPane1" runat="server" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                    <Header>
                        Rating System</Header>
                    <Content>
                        <asp:GridView ID="gvRatingSystem" Width="100%" runat="server" AllowPaging="False"
                            AllowSorting="False" AutoGenerateColumns="False"
                            SkinID="scaffold" PageSize="5">
                            <Columns>
                                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="Rating"
                                    DataNavigateUrlFormatString="CustomerRatingSettings.aspx?rating={0}" />
                                <asp:BoundField DataField="Rating" HeaderText="<%$Resources:dictionary,No %>"></asp:BoundField>
                                <asp:BoundField DataField="RatingName" HeaderText="<%$Resources:dictionary,Rating Name%>">
                                </asp:BoundField>
                                 <asp:ImageField DataImageUrlField="ImageUrl" ControlStyle-Height="100" ControlStyle-Width="100" />
                                <%--<asp:TemplateField HeaderText="<%$Resources:dictionary,Picture%>" ControlStyle-Width="100px"
                                    ControlStyle-Height="100px">
                                    <ItemTemplate>
                                        <asp:Image ID="Image" runat="server" ImageUrl='<%# "../../API/Lookup/RatingSystemPicture.ashx?id=" + Eval("Rating")  %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="RatingType" HeaderText="<%$Resources:dictionary, Type%>">
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="<%$Resources:dictionary, Weight%>" >
                                    <ItemTemplate><%# Eval("Weight").ToString() %> %</ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:dictionary, Enabled%>" >
                                    <ItemTemplate><%# Boolean.Parse(Eval("Deleted").ToString()) == true ? "False" : "True" %></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, (No Data Found)%>" /></EmptyDataTemplate>
                        </asp:GridView>
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane2" runat="server" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                    <Header>
                        Main Page</Header>
                    <Content>
                        <asp:Literal ID="Literal2" runat="server" Text="Greeting Text : " />
                        <asp:TextBox ID="Rating_GreetingText" runat="server" Text="" Width="450px" /><br />
                        <asp:Literal ID="Literal1" runat="server" Text="Footer Text : " />
                        <asp:TextBox ID="Rating_FooterText" runat="server" Text="" Width="450px" /><br />
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane3" runat="server" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                    <Header>
                        Thank You Page</Header>
                    <Content>
                        <asp:Literal ID="Literal3" runat="server" Text=" Good Rating (Good and Very Good) : " />
                        <asp:TextBox ID="Rating_ThankYouGoodRating" runat="server" Text="" Width="450px" /><br />
                        <asp:Literal ID="Literal4" runat="server" Text="Bad Rating (Average, Poor, Very Poor) : " />
                        <asp:TextBox ID="Rating_ThankYouBadRating" runat="server" Text="" Width="450px" /><br />
                        <asp:Literal ID="Literal5" runat="server" Text="Show Thank you Page for " />
                        <asp:TextBox ID="Rating_ThankYouInterval" runat="server" Text="" Width="15px" />
                        <asp:Literal ID="Literal6" runat="server" Text="seconds" />
                        <br />
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane4" runat="server" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                    <Header>
                        Good Rating Feedback selection</Header>
                    <Content>
                        <asp:CheckBox ID="Rating_AllowGoodRatingFeedback" runat="server" Text="Allow to select reasons for good rating"
                            Checked="true" /><br />
                        <asp:Literal ID="Literal7" runat="server" Text="Good Rating Feedback Selection Greeting Text : " />
                        <asp:TextBox ID="Rating_GoodFeedbackGreeting" runat="server" Text="" Width="450px" /><br />
                        <br />
                        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Add New%>"
                            Width="130px" ID="BtnAddNewGoodFeedback" OnClick="BtnAddNewGoodFeedback_Click" />
                        <br />
                        <br />
                        <asp:GridView ID="gvGoodFeedback" Width="100%" runat="server" AllowPaging="False"
                            AllowSorting="False" AutoGenerateColumns="False"
                            SkinID="scaffold" PageSize="20">
                            <Columns>
                                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="RatingFeedbackID"
                                    DataNavigateUrlFormatString="CustomerRatingSettings.aspx?feedback={0}" />
                                <asp:HyperLinkField Text="<%$Resources:dictionary, Delete%>" DataNavigateUrlFields="RatingFeedbackID"
                                    DataNavigateUrlFormatString="CustomerRatingSettings.aspx?feedback={0}&&delete=true" />
                                <asp:BoundField DataField="SelectionText" HeaderText="<%$Resources:dictionary,Selection Text%>">
                                </asp:BoundField>
                                  <asp:ImageField DataImageUrlField="ImageUrl" ControlStyle-Height="100" ControlStyle-Width="100" />
                                <%--<asp:TemplateField HeaderText="<%$Resources:dictionary,Selection Picture%>" ControlStyle-Width="100px"
                                    ControlStyle-Height="100px">
                                    <ItemTemplate>
                                        <asp:Image ID="Image" runat="server" ImageUrl='<%# "../../API/Lookup/RatingFeedbackPicture.ashx?id=" + Eval("RatingFeedbackID")  %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, (No Data Found)%>" /></EmptyDataTemplate>
                        </asp:GridView>
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane5" runat="server" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                    <Header>
                        Bad Rating Feedback selection</Header>
                    <Content>
                        <asp:CheckBox ID="Rating_AllowBadRatingFeedback" runat="server" Text="Allow to select reasons for bad rating"
                            Checked="true" /><br />
                        <asp:Literal ID="Literal8" runat="server" Text="Bad Rating Feedback Selection Greeting Text : " />
                        <asp:TextBox ID="Rating_BadFeedbackGreeting" runat="server" Text="" Width="450px" />
                        <br />
                        <br />
                        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Add New%>"
                            Width="130px" ID="BtnAddNewBadFeedback" OnClick="BtnAddNewBadFeedback_Click" />
                        <br />
                        <br />
                        <asp:GridView ID="gvBadFeedback" Width="100%" runat="server" AllowPaging="False"
                            AllowSorting="False" AutoGenerateColumns="False"
                            SkinID="scaffold" PageSize="20">
                            <Columns>
                                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="RatingFeedbackID"
                                    DataNavigateUrlFormatString="CustomerRatingSettings.aspx?feedback={0}" />
                                <asp:HyperLinkField Text="<%$Resources:dictionary, Delete%>" DataNavigateUrlFields="RatingFeedbackID"
                                    DataNavigateUrlFormatString="CustomerRatingSettings.aspx?feedback={0}&&delete=true" />
                                <asp:BoundField DataField="SelectionText" HeaderText="<%$Resources:dictionary,Selection Text%>">
                                </asp:BoundField>
                                  <asp:ImageField DataImageUrlField="ImageUrl" ControlStyle-Height="100" ControlStyle-Width="100" />
                                <%--<asp:TemplateField HeaderText="<%$Resources:dictionary,Selection Picture%>" ControlStyle-Width="100px"
                                    ControlStyle-Height="100px">
                                    <ItemTemplate>
                                        <asp:Image ID="Image" runat="server" ImageUrl='<%# "../../API/Lookup/RatingFeedbackPicture.ashx?id=" + Eval("RatingFeedbackID")  %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, (No Data Found)%>" /></EmptyDataTemplate>
                        </asp:GridView>
                    </Content>
                </cc1:AccordionPane>
            </Panes>
        </cc1:Accordion>
    </asp:Panel>
    <asp:Panel ID="PanelRatingSystem" runat="server">
        <asp:Label ID="lblResultRatingSystem" runat="server"></asp:Label>
        <table cellpadding="5" cellspacing="0" width="1000" id="FieldsTable1">
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Rating %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:Literal ID="lblRatingID" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Rating Name %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtRatingName" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td style="width: 101px;">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Picture %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:FileUpload ID="fuRatingSystem" runat="server" Height="21px" Width="218px" onchange="previewFile()" />
                    <br />
                    <asp:Image ID="Image2" runat="server" Width="155px" />
                    <br />
                    <asp:Button ID="btnRemoveImage" runat="server" Text="Remove Image" OnClientClick="javascript:return removeFile();"
                        Visible="False" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary, Type %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlRatingType" runat="server">
                        <asp:ListItem Text="Good Rating" Value="Good Rating"></asp:ListItem>
                        <asp:ListItem Text="Bad Rating" Value="Bad Rating"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary,Weight %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:Literal ID="lblWeight" runat="server" ></asp:Literal> &nbsp;%
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Enabled %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="cbEnabled" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    <asp:Button ID="btnRatingSystemSave" runat="server" CssClass="classname" OnClick="btnRatingSystemSave_Click"
                        Text="<%$ Resources:dictionary, Save %>" />
                    <asp:Button ID="btnRatingSystemReturn" runat="server" CssClass="classname" Text="<%$Resources:dictionary, Return%>"
                        OnClick="btnRatingSystemReturn_Click" />
                   <%-- <asp:Button ID="btnRatingSystemDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnRatingSystemDelete_Click" Text="<%$ Resources:dictionary, Delete %>" />--%>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PanelRatingFeedback" runat="server">
        <asp:Label ID="lblResultRatingFeedback" runat="server"></asp:Label>
        <table cellpadding="5" cellspacing="0" width="1000" id="FieldsTable2">
            <asp:HiddenField ID="hFeedbackID" runat="server"></asp:HiddenField>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$Resources:dictionary, Type %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:Literal ID="lblRatingType" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal16" runat="server" Text="<%$Resources:dictionary,Selection Text %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSelectionText" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td style="width: 101px;">
                    <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:dictionary,Selection Picture %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:FileUpload ID="FileUploadFeedback" runat="server" Height="21px" Width="218px"
                        onchange="previewFileFeedback()" />
                    <br />
                    <asp:Image ID="ImageFeedback" runat="server" Width="155px" />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    <asp:Button ID="btnRatingFeedbackSave" runat="server" CssClass="classname" OnClick="btnRatingFeedbackSave_Click"
                        Text="<%$ Resources:dictionary, Save %>" />
                    <asp:Button ID="btnRatingFeedbackReturn" runat="server" CssClass="classname" Text="<%$Resources:dictionary, Return%>"
                        OnClick="btnRatingFeedbackReturn_Click" />
                    <asp:Button ID="btnRatingFeedbackDelete" runat="server" CausesValidation="False"
                        CssClass="classname" OnClick="btnRatingFeedbackDelete_Click" Text="<%$ Resources:dictionary, Delete %>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
