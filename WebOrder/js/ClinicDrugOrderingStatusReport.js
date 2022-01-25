$(document).ready(function(evt) {
	initElementEvents();
	initDataSource();

	function initDataSource() {
		fnLoadDropdownAjaxDataSource({
			url: 'API/Lookup/Clinic.ashx',
			element: 'ClinicCode',
			selectOneText: '-- Select All --'
		});
		fnLoadDropdownAjaxDataSource({
			url: 'API/Lookup/Year.ashx',
			element: 'Year'
		});	
		fnLoadDropdownAjaxDataSource({
			url: 'API/Lookup/Month.ashx',
			element: 'Month'
		});	
	}

	function initElementEvents() {
		var buttonSearch = $('#buttonSearch');
		buttonSearch.off();
		buttonSearch.on('click', function(evt) {
			evt.preventDefault();

			var data = $('#formData').serializeObject();
			if(data.Year &&  data.Month) {
				fnAjax({
					url: 'API/OperatingHours/Report.ashx',
					data: data,
					success: function(response) {
						var strHtml = '';

						if(response.length > 0) {
							strHtml += '';

							var headers = response[0];
							var columnsCount = Object.keys(headers).length;
							var i =0;

							strHtml += '<thead>';
							strHtml += '<tr style="background-color: #428BCA; color: white;">';
							$.each(headers || [], function(index, data) {

								if(i == 0) {
									strHtml += '<td style="width: 150px;"><strong>Clinic</strong></td>';
								}
								else {
									strHtml += '<td style="width: 150px;"><strong>' + index + '</strong></td>';
								}
								i++;
							});
							strHtml += '</tr>';
							strHtml += '</thead>';

							strHtml += '<tbody>';

							$.each(response || [], function(index, data) {
								strHtml += '<tr>';
								
								var dataCount = Object.keys(headers).length;
								i=0;

								$.each(data || [], function(key, value) {
									if(i == 0) {
										strHtml += '<td>' + value + '</td>';
									}
									else {
										if(value == 0) {
											strHtml += '<td style="background-color: white; text-align: center;"><strong>PH</strong></td>';		
										}
										else if(value == 1) {
											strHtml += '<td style="background-color: green; text-align: center;">&nbsp;</td>';		
										}
										else if(value == 2) {
											strHtml += '<td style="background-color: red; text-align: center;">&nbsp;</td>';		
										}
										else if(value == 3) {
											strHtml += '<td style="background-color: yellow; text-align: center;">&nbsp;</td>';		
										}
										else if(value == 4) {
											strHtml += '<td style="background-color: black; text-align: center;">&nbsp;</td>';		
										}
										else {
											strHtml += '<td style="background-color: white; text-align: center;"><strong>&nbsp;</strong></td>';		
										}
									}

									i++;
								});


								strHtml += '</tr>';
							});

							strHtml += '</tbody>';


							strHtml += '';
						}

						$('#table-report').html('');
						$('#table-report').html(strHtml);
					}
				});
			}
			else {
				fnShowMessage('Information', 'Year and Month cannot be empty!');
			}
		});
	}

	function loadOperationHours() {
		var data = $('#formData').serializeObject();

		if(data.Year && data.ClinicCode) {
			$('#table-operation-hours').dataTable({
				bFilter: true, 
				bInfo: true,
				"sDom": "<'exportOptions'T><'table-responsive't><'row'<p i>>",
	            // "sPaginationType": "bootstrap",
	            "destroy": true,
	            "scrollCollapse": true,
	            // "oLanguage": {
	            //     "sLengthMenu": "_MENU_ ",
	            //     "sInfo": "Showing <b>_START_ to _END_</b> of _TOTAL_ entries"
	            // },
	            "iDisplayLength": 10,
	            "fnDrawCallback": function (oSettings) {
	                $('.export-options-container').append($('.exportOptions'));

	                $('#ToolTables_tableWithExportOptions_0').tooltip({
	                    title: 'Export as CSV',
	                    container: 'body'
	                });

	                $('#ToolTables_tableWithExportOptions_3').tooltip({
	                    title: 'Copy data',
	                    container: 'body'
	                });
	            },
	            aoColumns: [
	            	{ mData: 'OperationDate', mRender: function(a) { return moment(a).format('DD MMM YYYY'); }  },
	            	{ mData: 'DayName' },
	            	{ mData: 'Status' , sWidth: '200px', mRender: function(a) { if(a == true) { return 'Open'; } else { return 'Closed'; } } },
	            	{ mData: null, sWidth: '200px', mRender: function() {  
	            		return '<span data-action="edit" style="cursor: pointer;">Edit</span> &nbsp; <span data-action="history" style="cursor: pointer;">History</span>';
	            	} }
	            ],
	            "bProcessing": true,
	            "bServerSide": false,
	            "sAjaxDataProp": "",
	            "sAjaxSource": connection.serverAddress + '/API/OperatingHours/List.ashx',
	            "sServerMethod": "POST",
	            "fnServerParams": function (aoData) {
	                $.each(data || [], function (indexParam, paramOption) {
	                    aoData.push({
	                        name: indexParam,
	                        value: paramOption
	                    });
	                });
	            },
	            "scrollX": true,
	            "aoColumnDefs": [
	                { "sWidth": "10%", "aTargets": [-1] }
	            ],
	            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
	                var __this = $(nRow);
	                var btnEdit = __this.children('td').children('[data-action="edit"]');
	            }
			});
		}
	}
});