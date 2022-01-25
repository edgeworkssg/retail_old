<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="MembershipPointSetup.aspx.cs" Inherits="MembershipPointSetup" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>

    <script type="text/javascript">
        var newwindow;
        function poptastic(url)
        {
	        newwindow=window.open(url,'name','height=700,width=650,resizeable=1,scrollbars=1');
	        if (window.focus) {newwindow.focus()}
        }
    </script>
    &nbsp;&nbsp;
    <table cellpadding="5" cellspacing="0" width="100%">
        <tr style="height:100%">
            <td style="width: 104px">
                <ul id="qm0" class="qmmc" runat="server">
                    
                </ul>
                
            </td>
            <td>
<%--    	<table cellpadding="5" cellspacing="0" Width="600px">
			<tr>
			    <td  style="width: 104px">
                   <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal> 
                </td>
			    <td  style="width: 180px"><asp:TextBox ReadOnly=true ID="ctrlMembershipNo" runat="server" MaxLength="50" ></asp:TextBox></td>
			    <td  style="width: 92px"> <asp:Literal ID = "Literal28" runat="server" Text="<%$Resources:dictionary,Group %>"></asp:Literal></td>
			    <td ><asp:DropDownList ID="ctrlMembershipGroupId" runat="server" Width="153px" ></asp:DropDownList></td>
		    </tr>
            <tr>
                <td  style="width: 104px">
                    <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Expiry Date %>"></asp:Literal></td>
                <td  style="width: 180px">
                <asp:TextBox ID="ctrlExpiryDate" runat="server" Width="121px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
                <td  style="width: 92px">
                    <asp:Literal ID="Literal8" runat="server" 
                        Text="<%$Resources:dictionary,Subscription Date %>"></asp:Literal>
                   </td>
                <td >
                    <asp:TextBox ID="txtSubscriptionDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                </td>
            </tr>
        </table>
--%>
<%--    <cc1:Accordion ID="editAccordion"
        Width="100%"
        runat="server"
        SelectedIndex="0"
        HeaderCssClass="AccordHeader"
        HeaderSelectedCssClass="AccordHeader"
        AutoSize="Fill"
        FadeTransitions="true"
        TransitionDuration="250"
        FramesPerSecond="40"
        RequireOpenedPane="false"
        SuppressHeaderPostbacks="true">
            <Panes>
                <cc1:AccordionPane>
                    <Header>
                        Personal Information
                    </Header>
                    <Content>
                        <table cellpadding="5" cellspacing="0" width="100%">
		                    <tr>
			                    <td  style="width: 104px">
                                    <asp:Literal ID="Literal2" runat="server" 
                                        Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal>
                                </td>
                    			
		                        <td >
                                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
		                        <td  style="width: 92px">
		                            <asp:Literal ID = "Literal12" runat="server" Text="<%$Resources:dictionary,NRIC/FIN %>"></asp:Literal>
		                        </td>
			                    <td >
                                    <asp:TextBox ID="TextBox2" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
		                    </tr>
                    		
		                    <tr>
			                    <td  style="width: 104px"><asp:Literal ID = "Literal13" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal></td>
                    			
		                        <td ><asp:TextBox ID="TextBox3" runat="server" MaxLength="50" ></asp:TextBox>
                                </td>
		                        <td  style="width: 92px"><asp:Literal ID = "Literal14" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal></td>
			                    <td ><asp:TextBox ID="TextBox4" runat="server" MaxLength="50" ></asp:TextBox>
                                </td>
		                    </tr>
                    		
		                    <tr>
			                    <td  style="width: 104px"><asp:Literal ID = "Literal15" runat="server" Text="<%$Resources:dictionary,Gender %>"></asp:Literal></td>
                    			
		                        <td >
                                    <asp:DropDownList ID="DropDownList1" runat="server">
                                        <asp:ListItem>M</asp:ListItem>
                                        <asp:ListItem>F</asp:ListItem>
                                    </asp:DropDownList></td>
		                        <td  style="width: 92px"><asp:Literal ID = "Literal16" runat="server" Text="<%$Resources:dictionary,Date Of Birth  %>"></asp:Literal></td>
			                    <td >
                                    <asp:TextBox ID="TextBox5" runat="server" Width="118px"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
		                    </tr>
                        </table>
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane>
                    <Header>
                        AAAAAAA
                    </Header>
                    <Content>
                    
                    </Content>
                </cc1:AccordionPane>
            </Panes>
        
    </cc1:Accordion>
--%>    </td>
            </tr>
    </table>

</asp:Content>
