<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="MillionIntegrationReport.aspx.cs" Inherits="PowerWeb.Reports.MillionIntegrationReport"
    Title="<%$Resources:dictionary, Million Integration Report%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="360000">
    </ajax:ScriptManager>
    <ajax:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
                PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
                PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
            </cc1:CalendarExtender>
            <table width="600px">
                <tr>
                    <td colspan="4" class="searchHeader">
                        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="fieldname" style="width: 102px; height: 3px">
                        <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                            Text="<%$ Resources:dictionary, Start Date %>" />
                    </td>
                    <td style="height: 3px">
                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                        <asp:DropDownList ID="ddStartHour" runat="server">
                            <asp:ListItem>00</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddStartMinute" runat="server">
                            <asp:ListItem>00</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>29</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>31</asp:ListItem>
                            <asp:ListItem>32</asp:ListItem>
                            <asp:ListItem>33</asp:ListItem>
                            <asp:ListItem>34</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>36</asp:ListItem>
                            <asp:ListItem>37</asp:ListItem>
                            <asp:ListItem>38</asp:ListItem>
                            <asp:ListItem>39</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>41</asp:ListItem>
                            <asp:ListItem>42</asp:ListItem>
                            <asp:ListItem>43</asp:ListItem>
                            <asp:ListItem>44</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>46</asp:ListItem>
                            <asp:ListItem>47</asp:ListItem>
                            <asp:ListItem>48</asp:ListItem>
                            <asp:ListItem>49</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>51</asp:ListItem>
                            <asp:ListItem>52</asp:ListItem>
                            <asp:ListItem>53</asp:ListItem>
                            <asp:ListItem>54</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                            <asp:ListItem>56</asp:ListItem>
                            <asp:ListItem>57</asp:ListItem>
                            <asp:ListItem>58</asp:ListItem>
                            <asp:ListItem>59</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="fieldname" style="height: 3px">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
                    </td>
                    <td style="height: 3px">
                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                        <br />
                        <asp:DropDownList ID="ddEndHour" runat="server">
                            <asp:ListItem>00</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddEndMinute" runat="server">
                            <asp:ListItem>00</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>29</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>31</asp:ListItem>
                            <asp:ListItem>32</asp:ListItem>
                            <asp:ListItem>33</asp:ListItem>
                            <asp:ListItem>34</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>36</asp:ListItem>
                            <asp:ListItem>37</asp:ListItem>
                            <asp:ListItem>38</asp:ListItem>
                            <asp:ListItem>39</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>41</asp:ListItem>
                            <asp:ListItem>42</asp:ListItem>
                            <asp:ListItem>43</asp:ListItem>
                            <asp:ListItem>44</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>46</asp:ListItem>
                            <asp:ListItem>47</asp:ListItem>
                            <asp:ListItem>48</asp:ListItem>
                            <asp:ListItem>49</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>51</asp:ListItem>
                            <asp:ListItem>52</asp:ListItem>
                            <asp:ListItem>53</asp:ListItem>
                            <asp:ListItem>54</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                            <asp:ListItem>56</asp:ListItem>
                            <asp:ListItem>57</asp:ListItem>
                            <asp:ListItem>58</asp:ListItem>
                            <asp:ListItem>59</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="fieldname" style="width: 102px">
                        <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                            Width="68px" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" Width="122px">
                            <asp:ListItem Value="1">January</asp:ListItem>
                            <asp:ListItem Value="2">February</asp:ListItem>
                            <asp:ListItem Value="3">March</asp:ListItem>
                            <asp:ListItem Value="4">April</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">June</asp:ListItem>
                            <asp:ListItem Value="7">July</asp:ListItem>
                            <asp:ListItem Value="8">August</asp:ListItem>
                            <asp:ListItem Value="9">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblYear" runat="server"></asp:Label>
                    </td>
                    <td class="fieldname">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="fieldname" style="width: 102px">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Item Name %>"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName" runat="server" Width="172px"></asp:TextBox>
                    </td>
                    <td class="fieldname">
                        <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Category %>"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddCategory" runat="server" Width="179px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="fieldname" style="width: 102px; height: 31px;">
                        <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal>
                    </td>
                    <td style="height: 31px">
                        <subsonic:DropDown ID="ddDept" runat="server" PromptValue="" TableName="Department"
                            TextField="DepartmentName" ValueField="DepartmentID" Width="172px" OnInit="ddDept_Init"
                            OnSelectedIndexChanged="ddDept_SelectedIndexChanged" AutoPostBack="True">
                        </subsonic:DropDown>
                    </td>
                    <td class="fieldname" style="height: 31px">
                        <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
                    </td>
                    <td style="height: 31px">
                        <asp:DropDownList ID="ddlOutlet" runat="server" Width="180px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddDept_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="fieldname" style="width: 102px">
                        <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
                    </td>
                    <td>
                        <subsonic:DropDown ID="ddPOS" runat="server" ShowPrompt="True" TableName="PointOfSale"
                            TextField="PointOfSaleName" ValueField="PointOfSaleID" Width="170px" PromptText="ALL"
                            OnInit="ddPOS_Init">
                        </subsonic:DropDown>
                    </td>
                    <td class="fieldname">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                            OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                            OnClick="btnClear_Click" />
                    </td>
                    <td colspan="2" align="right">
                        <a href="../exportToExcel.aspx"><asp:Literal ID="ltr01"  runat="server" Text="<%$Resources:dictionary, Export%>" /></a>
                    </td>
                </tr>
            </table>
            <ajax:UpdateProgress runat="server" ID="progress1" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <img src="../App_Themes/Default/image/indicator_mozilla_blu.gif" />
                    <span style="color: #0000ff"><b><asp:Literal ID="ltr02"  runat="server" Text="<%$Resources:dictionary, Update in progress%>" /></b><br />
                    </span>
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <asp:GridView ID="gvReport" ShowFooter="True" Width="100%" runat="server" AllowPaging="True"
                AllowSorting="False" OnDataBound="gvReport_DataBound" OnPageIndexChanging="gvReport_PageIndexChanging"
                DataKeyNames="ref_no" AutoGenerateColumns="False" SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound"
                PageSize="20">
                <Columns>
                    <asp:BoundField DataField="accno" HeaderText="doc_no" />
                    <asp:BoundField DataField="doc_type" HeaderText="doc_type" />
                    <asp:BoundField DataField="doc_no" HeaderText="doc_no" />
                    <asp:BoundField DataField="seq" HeaderText="seq" />
                    <asp:BoundField DataField="doc_date" HeaderText="doc_date" />
                    <asp:BoundField DataField="ref_no" HeaderText="ref_no" />
                    <asp:BoundField DataField="ref_no2" HeaderText="ref_no2" />
                    <asp:BoundField DataField="desp" HeaderText="desp" />
                    <asp:BoundField DataField="desp2" HeaderText="desp2" />
                    <asp:BoundField DataField="amount" HeaderText="amount" />
                    <asp:BoundField DataField="debit" HeaderText="debit" />
                    <asp:BoundField DataField="credit" HeaderText="credit" />
                    <asp:BoundField DataField="fx_amount" HeaderText="fx_amount" />
                    <asp:BoundField DataField="fx_debit" HeaderText="fx_debit" />
                    <asp:BoundField DataField="fx_credit" HeaderText="fx_credit" />
                    <asp:BoundField DataField="fx_rate" HeaderText="fx_rate" />
                    <asp:BoundField DataField="curr_code" HeaderText="curr_code" />
                    <asp:BoundField DataField="taxcode" HeaderText="taxcode" />
                    <asp:BoundField DataField="taxable" HeaderText="taxable" />
                    <asp:BoundField DataField="fx_taxable" HeaderText="fx_taxable" />
                    <asp:BoundField DataField="link_seq" HeaderText="link_seq" />
                    <asp:BoundField DataField="billtype" HeaderText="billtype" />
                    <asp:BoundField DataField="remark1" HeaderText="remark1" />
                    <asp:BoundField DataField="remark2" HeaderText="remark2" />
                    <asp:BoundField DataField="cheque_no" HeaderText="cheque_no" />
                    <asp:BoundField DataField="projcode" HeaderText="projcode" />
                    <asp:BoundField DataField="deptcode" HeaderText="deptcode" />
                    <asp:BoundField DataField="accmgr_id" HeaderText="accmgr_id" />
                    <asp:BoundField DataField="batchno" HeaderText="batchno" />
                </Columns>
                <PagerTemplate>
                    <div style="border-top: 1px solid #666666">
                        <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                            CommandArgument="First" CommandName="Page" />
                        <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                            CommandArgument="Prev" CommandName="Page" />
                        <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                        <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                            ID="lblPageCount" runat="server"></asp:Label>
                        <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                            CommandArgument="Next" CommandName="Page" />
                        <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                            CommandArgument="Last" CommandName="Page" />
                    </div>
                </PagerTemplate>
            </asp:GridView>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
