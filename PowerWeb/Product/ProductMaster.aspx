<%@ Page Language="C#" Theme="Default"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="ProductMaster" Title="<%$Resources:dictionary,Product Setup %>" Codebehind="ProductMaster.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:Panel id="pnlGrid" runat="server">	
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
                </ajax:ScriptManager>
    <table style="width: 650px" id="FilterTable">
        <tr><td  style="width: 160px">
            <asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal></td>
            <td style="width: 330px" >
            <asp:TextBox ID="txtItemNo" runat="server" Width="200px"></asp:TextBox><div class="divider">   </div>
            <asp:Button ID="btnSearch" runat="server" class="classname" OnClick="btnSearch_Click" Text="<%$ Resources:dictionary, Search %>" />
            </td>
        </tr>
        <tr><td style="width: 160px">
            <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Search By Item Name %>"></asp:Literal></td>
            <td  style="width: 330px" >
            <subsonic:DropDown ID="ddlItemName" runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="DropDown1_SelectedIndexChanged"
                OrderField="ItemName" ShowPrompt="True" TableName="Item" TextField="ItemName"
                ValueField="ItemNo" Width="300px">
            </subsonic:DropDown></td></tr>
        <tr>
            <td  colspan="2">
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label></td>
        </tr>
    </table>
    <table style="width: 650px">
        <tr><td align="right" ><asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td></tr>
    </table>
    <input class="classname" onclick="location.href='ProductMaster.aspx?id=0'" type="button" value="Add New"/>
   <div style="height:7px;"></div>
    <asp:GridView 
    ID="GridView1" 
    SkinID="scaffold"
    runat="server" 
    AllowPaging="True" 
    AllowSorting="True"
    AutoGenerateColumns="False"         
    DataKeyNames="ItemNo" 
    ShowFooter="True"
    PageSize="20" Width="100%" OnRowDataBound="GridView1_RowDataBound" 
    ondatabound="GridView1_DataBound" onpageindexchanging="GridView1_PageIndexChanging" 
    >
        <Columns>
	            <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="ItemNo" DataNavigateUrlFormatString="ProductMaster.aspx?id={0}" />	            
	            <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>"></asp:BoundField>	            
	            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Product Name %>"></asp:BoundField>	            	            
	            <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category %>"></asp:BoundField>	            	            	            	            
	            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:dictionary,Department %>"></asp:BoundField>	            	            	            	            
	            <asp:BoundField DataField="RetailPrice" HeaderText="<%$Resources:dictionary,Retail Price %>"></asp:BoundField>	            	            	            	            	            	            
	            <asp:BoundField DataField="FactoryPrice" HeaderText="Factory Price"></asp:BoundField>	            	            	            	            	            	            
	            <asp:BoundField DataField="IsInInventory" 
                    HeaderText="<%$Resources:dictionary,Inventory Item %>" />
	            <asp:BoundField DataField="Attributes1" HeaderText="Attributes1" />
                <asp:BoundField DataField="Attributes2" HeaderText="Attributes2" />
                <asp:BoundField DataField="Attributes3" HeaderText="Attributes3" />
                <asp:BoundField DataField="Attributes4" HeaderText="Attributes4" />
                <asp:BoundField DataField="Attributes5" HeaderText="Attributes5" />
                <asp:BoundField DataField="Attributes6" HeaderText="Attributes6" />
                <asp:BoundField DataField="Attributes7" HeaderText="Attributes7" />
                <asp:BoundField DataField="Attributes8" HeaderText="Attributes8" />
                <asp:BoundField DataField="IsNonDiscountable" HeaderText="Non Discountable" />
	            <asp:BoundField DataField="PointType" HeaderText="Point Type"></asp:BoundField>
	            <asp:BoundField DataField="PointAmount" HeaderText="Point Amount"></asp:BoundField>
	            <asp:BoundField  DataField="Barcode" HeaderText="<%$Resources:dictionary,Barcode %>"></asp:BoundField>	            	            	            
	            <asp:BoundField DataField="IsServiceItem" HeaderText="<%$Resources:dictionary,Service Item %>"></asp:BoundField>	            	            	            
	            <asp:BoundField DataField="GSTRule" HeaderText="<%$Resources:dictionary,GST Rule %>"></asp:BoundField>
	            <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary,Remark %>"></asp:BoundField>	           	            	            	            	            
	            <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary,Deleted %>"></asp:BoundField>	            
        </Columns>
        <EmptyDataTemplate>
            <asp:Literal ID = "literal5" runat="server" Text="<%$Resources:dictionary,No Product Created Yet %>"></asp:Literal>
        </EmptyDataTemplate>
       <PagerTemplate>
            <div style="border-top:1px solid #666666">
            <asp:Button ID="btnFirst" runat="server" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server"  Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server"  Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server"  Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
    </asp:GridView>	
	</asp:Panel>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete" SelectMethod="FetchAllNonDeletedItems_PlusPointInfo" TypeName="PowerPOS.ItemController">
        <DeleteParameters>
            <asp:Parameter Name="ItemNo" Type="Object" />
        </DeleteParameters>
    </asp:ObjectDataSource>	
	<asp:panel id="pnlEdit" Runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table cellpadding="5" cellspacing="0"  width="1000" id="FieldsTable" >
        <tr>
			<td  style="width: 150px">
                <asp:Literal ID="Literal2" runat="server" 
                    Text="<%$Resources:dictionary,Item No %>"></asp:Literal>
            </td>
			<td  style="width: 420px">
                <asp:TextBox ID="txtItemNoEditor" runat="server" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtItemNoEditor" 
                    ErrorMessage="<%$ Resources:dictionary, *Required! %>"></asp:RequiredFieldValidator>
            </td>
			<td  style="width: 150px"> 
                <asp:Literal ID="Literal4" runat="server" 
                    Text="<%$Resources:dictionary,Barcode %>"></asp:Literal>
            </td>
			<td >
                <asp:TextBox ID="txtBarcode" runat="server" MaxLength="90"></asp:TextBox>
            </td>
		</tr>
            <tr>
                <td  style="width: 101px; height: 19px;">
                    <asp:Literal ID="Literal3" runat="server" 
                        Text="<%$Resources:dictionary,Item Name %>"></asp:Literal>
                </td>
                <td colspan="3" style="height: 19px;">
                    <asp:TextBox ID="txtItemName" runat="server" MaxLength="290" Width="400px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ControlToValidate="txtItemName" ErrorMessage="*Please input Item Name" 
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>            
            <tr>
			<td  style="width: 101px">
                <asp:Literal ID="Literal13" runat="server" 
                    Text="<%$Resources:dictionary,Category Name %>"></asp:Literal>
            </td>
			<td colspan="3" >
                    <asp:DropDownList ID="ddlCategoryName" runat="server" Width="400px" 
                        Height="21px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="ddlCategoryName" ErrorMessage="*Please select Category Name" 
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
		    </tr>
            <tr>
                <td  style="width: 101px">
                    <asp:Literal ID="Literal7" runat="server" 
                        Text="<%$Resources:dictionary,Retail Price %>"></asp:Literal>
                </td>
                <td  style="width: 420px">
                    <asp:TextBox ID="txtRetailPrice" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" 
                        ControlToValidate="txtRetailPrice" 
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="500000" 
                        MinimumValue="0" Type="Currency"></asp:RangeValidator>
                </td>
                <td  style="width: 150px">
                    <asp:Literal ID="Literal8" runat="server" 
                        Text="<%$Resources:dictionary,Factory Price %>"></asp:Literal>
                </td>
                <td >
                    <asp:TextBox ID="txtFactoryPrice" runat="server" Width="50px" Height="24px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator2" runat="server" 
                        ControlToValidate="txtFactoryPrice" 
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5000000000" 
                        MinimumValue="0" Type="Currency"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td  style="width: 101px">
                    <asp:Literal ID="Literal12" runat="server" 
                        Text="<%$Resources:dictionary,Is Non Discountable %>"></asp:Literal>
                </td>
                <td  style="width: 180px">
                    <asp:CheckBox ID="cbIsNonDiscountable" runat="server" Checked="False" />
                </td>
                <td  style="width: 100px">
                    <asp:Literal ID="Literal23" runat="server" 
                        Text="<%$Resources:dictionary,GST %>"></asp:Literal>
                </td>
                <td >
                    <asp:DropDownList ID="ddGST" runat="server" Height="20px" Width="130px">
                        <asp:ListItem>--Please Select--</asp:ListItem>
                        <asp:ListItem>Exclusive GST</asp:ListItem>
                        <asp:ListItem>Inclusive GST</asp:ListItem>
                        <asp:ListItem>Non GST</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="ddGST" ErrorMessage="*Select GST mode" 
                        InitialValue="--Please Select--" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td  style="width: 101px">
                    <asp:Literal ID="Literal6" runat="server" 
                        Text="Give Commission"></asp:Literal>
                </td>
                <td  style="width: 410px">
                    <asp:CheckBox ID="cbGiveCommission" runat="server" Checked="False" />
                </td>
                <td  style="width: 101px">
                    &nbsp;
                    <asp:Literal ID="Literal26" runat="server" Text="Point Redeemable"></asp:Literal>
                </td>
                <td >
                    &nbsp;
                    <asp:CheckBox ID="cbPointRedeemable" runat="server" Checked="False" />
                </td>
            </tr>
            <tr>
                <td  style="width: 101px">
                    <asp:Literal ID = "Literal11" runat="server" Text="Item Type"></asp:Literal>
                </td>
                <td  colspan="3" style="padding:0px;">
                    <table cellpadding="0" cellspacing="0"  width="100%">
                        <tr style="background-color:#ebebeb;">
                            <td  style="width: 156px; text-align:left">
                                <asp:RadioButton ID="rbProduct" runat="server" Text="Product" 
                                    GroupName="ItemType" />
                            </td>
                            <td  style="width: 101px">
                                &nbsp;</td>
                            <td >
                            </td>
                        </tr>
                        <tr>
                            <td  style="width: 156px; text-align:left">
                                <asp:RadioButton ID="rbOpenPriceProduct" runat="server" GroupName="ItemType" 
                                    Text="Open Price Product" />
                            </td>
                            <td  style="width: 101px">
                                &nbsp;</td>
                            <td >
                                &nbsp;</td>
                        </tr>
                        <tr style="background-color:#ebebeb;">
                            <td  style="width: 156px; text-align:left">
                                <asp:RadioButton ID="rbService" runat="server" Text="Service" 
                                    GroupName="ItemType" />
                            </td>
                            <td  style="width: 101px">
                            </td>
                            <td >
                            </td>
                        </tr>
                        <tr>
                            <td  
                                style="width: 156px; text-align:left; height: 37px;">
                                <asp:RadioButton ID="rbPoint" runat="server" Text="Point Package" 
                                    GroupName="ItemType" />
                            </td>
                            <td  
                                style="width: 101px; font-weight:bold; height: 37px;">
                                <asp:Literal ID="Literal24" runat="server" Text="Points Get"></asp:Literal>
                                :</td>
                            <td  style="height: 37px">
                                <asp:TextBox ID="txtPointGet" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator6" runat="server" 
                                    ControlToValidate="txtPointGet" 
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" 
                                    MaximumValue="5000000000" MinimumValue="0" Type="Currency"></asp:RangeValidator>
                                <asp:Label ID="Label1" runat="server" ForeColor="#009933" 
                                    Text="* put 0 to follow Retail Price"></asp:Label>
                            </td>
                        </tr>
                        <tr style="background-color:#ebebeb;">
                            <td  rowspan="2" 
                                style="width: 156px; text-align:left">
                                <asp:RadioButton ID="rbCourse" runat="server" Text="Course Package" 
                                    GroupName="ItemType" />
                            </td>
                            <td  style="width: 101px; font-weight:bold">
                                <asp:Literal ID="Literal25" runat="server" Text="Times Get"></asp:Literal>
                            </td>
                            <td >
                                <asp:TextBox ID="txtTimesGet" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator7" runat="server" 
                                    ControlToValidate="txtTimesGet" 
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="50000" 
                                    MinimumValue="0" Type="Integer"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td  style="width: 101px; font-weight:bold">
                                <asp:Literal ID="Literal19" runat="server" Text="Breakdown Price"></asp:Literal>
                            </td>
                            <td >
                                <asp:TextBox ID="txtBreakdownPrice" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator8" runat="server" 
                                    ControlToValidate="txtBreakdownPrice" 
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" 
                                    MaximumValue="5000000000" MinimumValue="0" Type="Currency"></asp:RangeValidator>
                                <asp:Label ID="Label3" runat="server" ForeColor="#009933" 
                                    Text="* Individual price of the course"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>            
            <tr style="display: none;">
                <td  style="width: 101px; ">
                    <asp:Literal ID = "Literal9" runat="server" 
                        Text="<%$Resources:dictionary,Item Picture %>"></asp:Literal></td>
                <td  colspan="2">
                    <asp:FileUpload ID="FileUpload1" runat="server" Enabled="False" Height="21px" 
                        Width="218px" />
                </td>
                <td  rowspan="2" style="width: 287px">
                    <asp:Image ID="ItemImage" runat="server" Height="100px" Width="100px" />
                </td>
            </tr>
            <tr style="background-color:#dddbdc">
			<td  style="width: 101px; height: 50px;">                
                <asp:Literal ID="Literal10" runat="server" 
                    Text="<%$Resources:dictionary,Item Description %>"></asp:Literal>
            </td>
			<td  style="height: 50px;" colspan="3">
                <asp:TextBox ID="txtItemDesc" runat="server" MaxLength="250" Height="52px" 
                    TextMode="MultiLine" Width="344px"></asp:TextBox>
            </td>
		    </tr>        
            <tr id="attributesrow1" runat="server" style="background-color:#ebebeb;">
                <td  style="width: 101px">
                    <asp:Label ID="lblAttributes1" runat="server" Text=""></asp:Label>
                 </td>
                <td  colspan="3" style="width: 180px">
                    <asp:TextBox ID="txtAttributes1" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow2" runat="server" style="background-color:#dddbdc">
                <td  style="width: 101px">
                    <asp:Label ID="lblAttributes2" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAttributes2" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow3" runat="server" style="background-color:#ebebeb;">
                <td  style="width: 101px">
                    <asp:Label ID="lblAttributes3" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3"  style="width: 180px">
                    <asp:TextBox ID="txtAttributes3" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow4" runat="server" style="background-color:#dddbdc">
                <td  style="width: 101px">
                    <asp:Label ID="lblAttributes4" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAttributes4" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow5" runat="server" style="background-color:#ebebeb;">
                <td  style="width: 101px; ">
                    <asp:Label ID="lblAttributes5" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3" style="width: 180px;" >
                    <asp:TextBox ID="txtAttributes5" runat="server" MaxLength="50"></asp:TextBox>
                </td>
               <%-- <td  style="width: 101px">
                    &nbsp;</td>
                <td >
                    &nbsp;</td>--%>
            </tr>
            <tr id="attributesrow6" runat="server" >
                <td  style="width: 101px; ">
                    <asp:Label ID="lblAttributes6" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3" style="width: 180px;">
                    <asp:TextBox ID="txtAttributes6" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow7" runat="server">
                <td  style="width: 101px; ">
                    <asp:Label ID="lblAttributes7" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3" style="width: 180px;">
                    <asp:TextBox ID="txtAttributes7" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow8" runat="server" >
                <td  style="width: 101px; ">
                    <asp:Label ID="lblAttributes8" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3" style="width: 180px;">
                    <asp:TextBox ID="txtAttributes8" runat="server" MaxLength="50"></asp:TextBox>
                </td>            
            </tr>
            <tr style="background-color:#dddbdc">
                <td  style="width: 101px; height: 50px">
                    <asp:Literal ID="Literal17" runat="server" 
                        Text="<%$Resources:dictionary,Remark %>"></asp:Literal>
                </td>
                <td  colspan="3" style="height: 50px">
                    <asp:TextBox ID="txtRemark" runat="server" Height="133px" MaxLength="250" 
                        TextMode="MultiLine" Width="340px"></asp:TextBox>
                </td>
            </tr>
            <%--<tr  style="background-color:#ebebeb;">
                <td  style="width: 101px; height: 50px">
                    <asp:Literal ID="Literal14" runat="server" 
                        Text="Price Scheme"></asp:Literal>
                </td>
                <td  colspan="3" style="height: 50px">
                    <table  >
                        <tr style="background-color:#ebebeb;">
                            <td >
                                <!-- Grid -->
                                <asp:DataGrid ID="dgPriceScheme" runat="server" AutoGenerateColumns="false" 
                                    OnEditCommand="dgPriceScheme_EditCommand" OnItemCommand="dgPriceScheme_ItemCommand">
                                    <Columns>
                                        <asp:BoundColumn DataField="SchemeID" HeaderText="SchemeID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Price" HeaderText="Price"></asp:BoundColumn>
                                        <asp:ButtonColumn ButtonType="LinkButton" HeaderText="Edit" Text="Edit" CommandName="Edit" 
                                            CausesValidation="true">
                                        </asp:ButtonColumn>
                                        <asp:ButtonColumn ButtonType="LinkButton" HeaderText="Delete" Text="Delete" CommandName="Delete" 
                                            CausesValidation="true">
                                        </asp:ButtonColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr style="background-color:#ebebeb;">
                            <td>
                                <!-- Add Button -->
                                <asp:Button ID="btnAddPriceScheme" runat="server" 
                                    Text="Add" class="classname" OnClick="btnAddPriceScheme_Click" />
                            </td>
                        </tr>
                        <tr style="background-color:#ebebeb;">
                            <td>
                                <!-- Input / Edit Panel (Grid Item) -->
                                <asp:Panel ID="pnlPriceSchemeInput" GroupingText="PriceScheme Input" runat="server" Visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" Text="SchemeID" runat="server"></asp:Label>
                                            </td>
                                            <td> 
                                                <asp:TextBox ID="txtPriceSchemeSchemeID" runat="server"></asp:TextBox>
                                            </td>                                        
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label5" Text="Price" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPriceSchemePrice" runat="server"></asp:TextBox>
                                            </td>  
                                        </tr>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPriceSchemeOK" runat="server" Text="OK" class="classname" 
                                                    OnClick="btnPriceSchemeOK_Click" />
                                            </td>  
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr style="background-color:#ebebeb;">
                            <td>
                                <asp:Label ID="lblPriceSchemeErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <tr style="background-color:#dddbdc" >
                <td  style="width: 101px; height: 50px">
                    <asp:Literal ID="Literal15" runat="server" 
                        Text="Supplier"></asp:Literal>
                </td>
                <td  colspan="3" style="height: 50px">
                    <table style="background-color:#dddbdc">
                        <tr>
                            <td>
                                <!-- Grid -->
                                <asp:DataGrid ID="dgSupplier" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundColumn DataField="SupplierID" HeaderText="SupplierID"></asp:BoundColumn>
                                    </Columns>
                                    <Columns>
                                        <asp:BoundColumn DataField="SupplierName" HeaderText="SupplierName"></asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr  style="background-color:#ebebeb;" >
                <td align="left" colspan="4">
                    <asp:Button ID="btnSave" runat="server" class="classname"  Cssclass="classname" 
                        OnClick="btnSave_Click" Text="<%$ Resources:dictionary, Save %>" />
                    &nbsp;
                    <input class="classname" onclick="location.href='ProductMaster.aspx'" 
                        type="button" value="Return" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                        Cssclass="classname" OnClick="btnDelete_Click" 
                        Text="<%$ Resources:dictionary, Delete %>" />
                </td>
            </tr>            
        </table>	
	</asp:panel>
</asp:Content>

