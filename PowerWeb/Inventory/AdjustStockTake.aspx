<%@ Page Title="<%$Resources:dictionary,Approve Stock Take %>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="AdjustStockTake.aspx.cs" Inherits="PowerWeb.Inventory.AdjustStockTake" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Chosen/chosen.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <style type="text/css">
        .input-disabled
        {
            background-color: #EBEBE4;
            border: 1px solid #ABADB3;
            padding: 2px 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStockDate">
    </cc1:CalendarExtender>
    <asp:Label ID="lblResult" runat="server"></asp:Label>
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
    <table cellpadding="5" cellspacing="0" width="1000" id="FieldsTable1">
        <tr>
            <td style="width: 150px">
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Inventory Location %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlInventoryLocation" runat="server" OnSelectedIndexChanged="ddlInventoryLocation_OnSelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 150px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Filter %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>&nbsp;
                <asp:Button CssClass="classname" ID="btnFilter" runat="server" OnClick="btnFilter_Click"
                    Text="<%$ Resources:dictionary, Filter %>" CausesValidation="false"></asp:Button>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button class="classname" runat="server" Text="Select All" Width="130px" ID="BtnSelectAll"
        OnClick="BtnSelectAll_Click" /><div class="divider">
        </div>
    <asp:Button class="classname" runat="server" Text="Clear Selection" ID="BtnClearSelection"
        OnClick="BtnClearSelection_Click" Width="130px" /><div class="divider">
        </div>
    <input type="button" class="classname" id="btnShowDialogDeleted" value="Delete Selection"
        style="width: 130px" /><div class="divider">
        </div>
    <input type="button" class="classname" id="btnMissOutItems" value="Miss Out Items"
        style="width: 120px" /><div class="divider">
        </div>
    <input type="button" class="classname" id="btnShowDialogAdjusted" value="Approve"
        style="width: 130px" /><div class="divider">
        </div>
    <br />
    <br />
    <input type="button" class="classname" id="btnExport" value="Download Stock Take File"
        style="width: 180px" /><div class="divider">
        </div>
    <input type="button" class="classname" id="btnExportWard" value="Download Ward Topup File"
        style="width: 190px" /><div class="divider">
        </div>
    <div id="tempPanel" runat="server" style="display: none;">
        <asp:Button ID="BtnDeleteSelection" runat="server" OnClick="BtnDeleteSelection_Click"
            CausesValidation="false" />
        <asp:HiddenField ID="UpdateStockTakeID" runat="server" />
        <asp:HiddenField ID="UpdateQty" runat="server" />
        <asp:HiddenField ID="StockDocumentNo" runat="server" />
        <asp:HiddenField ID="ExportStockTakeDocNo" runat="server" />
        <asp:HiddenField ID="ExportWardStockTakeDocNo" runat="server" />
        <asp:HiddenField ID="StockDateDate" runat="server" />
        <asp:HiddenField ID="StockDateTime" runat="server" />
        <asp:Button ID="BtnUpdateStockTake" runat="server" OnClick="BtnUpdateStockTake_Click"
            CausesValidation="false" />
        <asp:Button ID="BtnAdjust" runat="server" OnClick="BtnAdjust_Click" CausesValidation="false" />
        <asp:Button ID="BtnExportStockTake" runat="server" OnClick="BtnExport_Click" CausesValidation="false" />
        <asp:Button ID="BtnExportWardTopUp" runat="server" OnClick="BtnExportWardTopUp_Click"
            CausesValidation="false" />
        <asp:Button ID="BtnSetStockDateTime" runat="server" OnClick="BtnSetStockDateTime_Click"
            CausesValidation="false" />
        <asp:Button ID="BtnAddMissedOutItems" runat="server" OnClick="BtnAddMissedOutItems_Click"
            CausesValidation="false" />
    </div>
    <br />
    <br />
    <asp:GridView ID="gvStock" SkinID="scaffold" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" DataKeyNames="StockTakeID" Width="100%" ShowFooter="true"
        OnRowDataBound="gvStock_RowDataBound" PageSize="20" OnPageIndexChanging="gvStock_PageIndexChanging"
        OnSorting="gvStock_Sorting" OnDataBound="gvStock_DataBound">
        <Columns>
            <asp:TemplateField HeaderText="<%$Resources:dictionary,Edit%>">
                <ItemTemplate>
                    <asp:LinkButton ID="editBtn" runat="server" ForeColor="Black" data="" OnClientClick="return false;"
                        action="edit_detail">Edit</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Select">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox1_OnCheckedChanged" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>"
                SortExpression="ItemNo"></asp:BoundField>
            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Product Name %>"
                SortExpression="ItemName"></asp:BoundField>
            <asp:BoundField DataField="SystemBalQty" HeaderText="<%$Resources:dictionary,System Balance Qty %>"
                DataFormatString="{0:##}" SortExpression="SystemBalQty"></asp:BoundField>
            <asp:BoundField DataField="StockTakeQty" HeaderText="<%$Resources:dictionary,Count Qty %>"
                DataFormatString="{0:##}" SortExpression="StockTakeQty"></asp:BoundField>
            <asp:BoundField DataField="defi" HeaderText="+/-" DataFormatString="{0:##}" SortExpression="defi" />
            <asp:BoundField DataField="StockTakeDate" HeaderText="<%$Resources:dictionary,Stock Take Date %>"
                SortExpression="StockTakeDate" />
            <asp:BoundField DataField="BatchNo" HeaderText="<%$Resources:dictionary,Batch No %>"
                SortExpression="BatchNo" />
            <asp:BoundField DataField="ParValue" HeaderText="<%$Resources:dictionary,Par Value%>"
                SortExpression="ParValue" />
            <asp:BoundField DataField="TakenBy" HeaderText="<%$Resources:dictionary,Taken By%>"
                SortExpression="TakenBy" />
            <asp:BoundField DataField="VerifiedBy" HeaderText="<%$Resources:dictionary,Verified By %>"
                SortExpression="VerifiedBy" />
            <asp:BoundField DataField="StockTakeID" Visible="false" SortExpression="StockTakeID" />
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblStockTakeID" runat="server" Text='<%# Eval("StockTakeID")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <asp:Literal ID="literal5" runat="server" Text="<%$Resources:dictionary,No Stock Take data to approve %>"></asp:Literal>
        </EmptyDataTemplate>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
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
    <div id="dialog-form" title="Edit Stock Take" style="display: none;">
        <div class="panel" id="panelUpdateStockTake">
            <input type="hidden" id="editStockTakeID" value="" />
            <fieldset>
                <legend>Edit Stock Take</legend>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Item No
                    </div>
                    <input type="text" id="lblItemNo" class="input-disabled" style="width: 250px;" disabled="disabled" />
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Item Name
                    </div>
                    <input type="text" id="lblItemName" class="input-disabled" style="width: 250px;"
                        disabled="disabled" />
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Qty
                    </div>
                    <input type="text" id="txtQty" style="width: 250px;" />
                </div>
            </fieldset>
        </div>
    </div>
    <div id="dialog-stockadjust" title="Aprove Stock Take" style="display: none;">
        <div class="panel" id="panelAdjustStockTake">
            <input type="hidden" id="Hidden1" value="" />
            <fieldset>
                <legend>Stock Take</legend>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Date
                    </div>
                    <asp:TextBox ID="txtStockDate" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                </div>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Time
                    </div>
                    <cc2:TimeSelector ID="txtTimeStock" Style="float: left; vertical-align: middle" MinuteIncrement="5"
                        DisplaySeconds="true" runat="server">
                    </cc2:TimeSelector>
                </div>
                <br />
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Document No
                    </div>
                    <input type="text" id="txtStockTakeDocumentNo" style="width: 250px;" />
                </div>
            </fieldset>
        </div>
    </div>
    <div id="dialog-download" title="Download Stock Take" style="display: none;">
        <div class="panel" id="panelDownloadStocktake">
            <fieldset>
                <legend>Stock Take</legend>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Document No
                    </div>
                    <select id="ListDocumentNo" data-placeholder="Choose Stock Document No" style="width: 600px"
                        class="chosen-select">
                    </select>
                </div>
            </fieldset>
        </div>
    </div>
    <div id="dialog-downloadtopup" title="Download Ward Top Up" style="display: none;">
        <div class="panel" id="Div2">
            <fieldset>
                <legend>Stock Take</legend>
                <div class="form-group" style="padding: 5px;">
                    <div style="width: 150px; float: left;">
                        Document No
                    </div>
                    <select id="ListDocumentNoTopUp" data-placeholder="Choose Stock Document No" style="width: 600px"
                        class="chosen-select">
                    </select>
                </div>
            </fieldset>
        </div>
    </div>
    <div id="dialog-missedout" title="Missed Out Items" style="display: none">
        <div class="panel" id="Div1" style="width: 500px">
            <input type="hidden" id="selectedinvlocid" />
            <table class="grid display" id="GridProduct">
                <thead>
                    <th>
                        Item No
                    </th>
                    <th>
                        Product Name
                    </th>
                    <th>
                        Qty
                    </th>
                </thead>
            </table>
        </div>
    </div>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Chosen/chosen.jquery.min.js")%>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.min.js") %>"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            app.setting.baseUrl = $('#BaseUrl').val();

            ajaxPost({
                url: app.setting.baseUrl + 'API/Lookup/StockTakeDocument.ashx',
                success: function(result) {
                    loadDropdownDataSource('ListDocumentNo', result, false);
                }
            });

            ajaxPost({
                url: app.setting.baseUrl + 'API/Lookup/StockTakeDocument.ashx',
                success: function(result) {
                    loadDropdownDataSource('ListDocumentNoTopUp', result, false);
                }
            });

            setTimeout(function() {
                $(".chosen-select").chosen();
            }, 2000);


            $('#btnExportWard').click(function() {
                $('#dialog-downloadtopup').dialog({
                    modal: true, zIndex: 10000, autoOpen: true,
                    width: 'auto', resizable: false, height: 300,
                    buttons: {
                        Yes: function() {
                            $("[id*=ExportWardStockTakeDocNo]").val($('#ListDocumentNoTopUp').val());
                            $("[id*=BtnExportWardTopUp]").click();
                            $(this).dialog("close");
                        },
                        No: function() {
                            $(this).dialog("close");
                        }
                    },
                    close: function(event, ui) {

                    }
                });
            });


            $('#btnExport').click(function() {
                $('#dialog-download').dialog({
                    modal: true, zIndex: 10000, autoOpen: true,
                    width: 'auto', resizable: false, height: 300,
                    buttons: {
                        Yes: function() {
                            $("[id*=ExportStockTakeDocNo]").val($('#ListDocumentNo').val());
                            $("[id*=BtnExportStockTake]").click();
                            $(this).dialog("close");
                        },
                        No: function() {
                            $(this).dialog("close");
                        }
                    },
                    close: function(event, ui) {

                    }
                });
            });


            $('#btnShowDialogDeleted').click(function() {
                ConfirmDialogDelete();
            });

            $('#btnShowDialogAdjusted').click(function() {
                $('#dialog-stockadjust').dialog({
                    modal: true, zIndex: 10000, autoOpen: true,
                    width: 'auto', resizable: false, height: 350,
                    buttons: {
                        "Approve": function() {
                            var stockdocumentno = $('#txtStockTakeDocumentNo').val();
                            if (stockdocumentno != null && stockdocumentno.length > 0) {
                                var stockdate = $("[id*=txtStockDate]").val();
                                var stocktime = $("[id*=txtTimeStock_txtHour]").val() + ':' + $("[id*=txtTimeStock_txtMinute]").val() + ':' + $("[id*=txtTimeStock_txtSecond]").val() + ' ' + $("[id*=txtTimeStock_txtAmPm]").val();
                                $("[id*=StockDateDate]").val(stockdate);
                                $("[id*=StockDateTime]").val(stocktime);
                                $("[id*=StockDocumentNo]").val(stockdocumentno);
                                $("[id*=BtnAdjust]").click();
                                $(this).dialog("close");
                            } else {
                                alert("Please enter the document no for this stock take for future reference");
                            }
                        },
                        "Cancel": function() {
                            $(this).dialog("close");
                        }
                    },
                    close: function(event, ui) {

                    }
                });
            });

            $('#btnSetStockDate').click(function() {
                $('#dialog-setstakedate').dialog({
                    modal: true, zIndex: 10000, autoOpen: true,
                    width: 'auto', resizable: false, height: 350,
                    buttons: {
                        Yes: function() {
                            var stockdate = $("[id*=txtStockDate]").val();
                            var stocktime = $("[id*=txtTimeStock_txtHour]").val() + ':' + $("[id*=txtTimeStock_txtMinute]").val() + ':' + $("[id*=txtTimeStock_txtSecond]").val() + ' ' + $("[id*=txtTimeStock_txtAmPm]").val();
                            $("[id*=StockDateDate]").val(stockdate);
                            $("[id*=StockDateTime]").val(stocktime);
                            $("[id*=BtnSetStockDateTime]").click();
                            $(this).dialog("close");
                        },
                        No: function() {
                            $(this).dialog("close");
                        }
                    },
                    close: function(event, ui) {

                    }
                });
            });



            $('[action=edit_detail]').click(function(e) {
                var data = $(this).attr('data');
                var it = data.split("|");

                $('#editStockTakeID').val(it[0]);
                $('#lblItemNo').val(it[1]);
                $('#lblItemName').val(it[2]);
                $('#txtQty').val(it[3]);

                setTimeout(function() {
                    $('#dialog-form').dialog({
                        modal: true, zIndex: 10000, autoOpen: true,
                        width: 'auto', resizable: false,
                        buttons: {
                            Yes: function() {
                                $("[id*=UpdateStockTakeID]").val($('#editStockTakeID').val());
                                $("[id*=UpdateQty]").val($('#txtQty').val());
                                $("[id*=BtnUpdateStockTake]").click();
                                $(this).dialog("close");
                            },
                            No: function() {
                                $(this).dialog("close");
                            }
                        },
                        close: function(event, ui) {

                        }
                    });
                }, 500);
            });

            $('#btnMissOutItems').click(function() {
                var invlocid = $("[id*=ddlInventoryLocation]").val();
                if (invlocid == "0") {
                    alert("Please Select Inventory Location!");
                } else {
                    $('#GridProduct').dataTable().fnDestroy();
                    InitGridProduct(invlocid);

                    setTimeout(function() {
                        $('#dialog-missedout').dialog({
                            modal: true, zIndex: 10000, autoOpen: true,
                            width: 'auto', resizable: false,
                            buttons: {
                                "Add All Item with Zero Qty": function() {
                                    $("[id*=BtnAddMissedOutItems]").click();
                                    $(this).dialog("close");
                                },
                                No: function() {
                                    $(this).dialog("close");
                                }
                            },
                            close: function(event, ui) {

                            }
                        });
                    }, 1000);
                }
            });

        });

        function ConfirmDialogDelete() {
            $('<div></div>').appendTo('body')
                .html('<div><h5>Are you sure want to delete the selected stock take ?</h5></div>')
                .dialog({
                    modal: true, title: 'Delete Selected Stock Take', zIndex: 10000, autoOpen: true,
                    width: 'auto', resizable: false,
                    buttons: {
                        Yes: function() {

                            $("[id*=BtnDeleteSelection]").click();
                            $(this).dialog("close");
                        },
                        No: function() {
                            $(this).dialog("close");
                        }
                    },
                    close: function(event, ui) {
                        $(this).remove();
                    }
                });
        };

        function InitGridProduct(invlocid) {
            $('#GridProduct').dataTable().fnDestroy();
            $('#GridProduct').DataTable({
                aoColumns: [
                    { mData: 'ItemNo', width: '100px' },
                    { mData: 'ItemName', width: '200px' },
                    { mData: 'Qty', width: '100px' }
                ],
                "aLengthMenu": [
                    [5, 10, 20, 50, 100, 150, -1],
                    [5, 10, 20, 50, 100, 150, "All"]
                ],
                "fnServerParams": function(aoData) {
                    aoData.push({
                        name: "InventoryLocationID",
                        value: invlocid
                    });
                },
                "iDisplayLength": (5),
                "bServerSide": (false),
                "sServerMethod": "POST",
                "sAjaxSource": app.setting.baseUrl + 'API/Inventory/MissedOutItems.ashx',
                "sAjaxDataProp": (""),
                "aaSorting": [[2, "desc"],[1, "asc"]],
                "fnRowCallback": function(nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                },
                "bAutoWidth": false,
                "bFilter": true,
                "bProcessing": true,
                "bRedraw": true,
                "fnDrawCallback": function(oSettings) {

                }
            });
        }  
    </script>

</asp:Content>
