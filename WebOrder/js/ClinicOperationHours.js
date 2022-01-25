$(document).ready(function(evt) {
	initElementEvents();
	initDataSource();
	initElementState();

	function initElementState() {
		$('.datepicker').datepicker({
			format: 'dd MM yyyy',
			setDate: new Date()
		});
	}

	function initDataSource() {
		fnLoadDropdownAjaxDataSource({
			url: 'API/Lookup/Clinic.ashx',
			element: 'ClinicCode',
			selectOneText: '-- Select All --'
		});
		fnLoadDropdownAjaxDataSource({
			url: 'API/Lookup/Clinic.ashx',
			element: 'ModalManageDay_ClinicCode',
			selectOneText: '-- Select One --'
		});
		fnLoadDropdownAjaxDataSource({
			url: 'API/Lookup/Year.ashx',
			element: 'Year'
		});	
		fnLoadDropdownAjaxDataSource({
			url: 'API/Lookup/Year.ashx',
			element: 'ModalPublicHoliday_Year'
		});	
	}

	function initElementEvents() {
		var buttonGenerateData = $('#buttonGenerateData');
		buttonGenerateData.off();
		buttonGenerateData.on('click', function(evt) {
			evt.preventDefault();

			var data = $('#formData').serializeObject();
			if(data.Year) {
				fnAjax({
					url: '/API/OperatingHours/Generate.ashx',
					data: data,
					success: function(response) {
						if(response.Status == true) {
							loadOperationHours();
						}	
						else {
							$('#MessageBox').modal('show');
						}
					}
				});
				fnShowMessage('Information', 'Data generated successfuly!');
			}
			else {
				fnShowMessage('Information', 'Year cannot be empty!');
			}
		});

		var year = $('#Year');
		year.off();
		year.on('change', function() {
			loadOperationHours();
		});

		var clinicCode = $('#ClinicCode');
		clinicCode.off();
		clinicCode.on('change', function() {
			loadOperationHours();
		});

		var modalPublicHoliday_Year = $('#ModalPublicHoliday_Year');
		modalPublicHoliday_Year.off();
		modalPublicHoliday_Year.on('change', function(evt) {
			loadPublicHoliday();
		});

		var modalPublicHoliday_ButtonImport = $('#ModalPublicHoliday_ButtonImport');
		modalPublicHoliday_ButtonImport.off();
		modalPublicHoliday_ButtonImport.on('click', function(evt){
			$('#ModalPublicHoliday_IcalFile').click();
		}); 

		var modalPublicHoliday_IcalFile = $('#ModalPublicHoliday_IcalFile');
		modalPublicHoliday_IcalFile.off();
		modalPublicHoliday_IcalFile.on('change', function(evt) {
			var fileItem = $('#ModalPublicHoliday_IcalFile');
	        var files = fileItem[0].files[0];
	        
	        var formData = new FormData();
	        formData.append('FileItem', files);
	        formData.append('Sender', 'edgeworks');

			$.ajax({
	            url: connection.serverAddress + '/API/PublicHoliday/UploadICAL.ashx',
	            type: 'POST',
	            data: formData,
	            processData: false,
	            contentType: false,
	            beforeSend: function() {
	                $('#divLoading').fadeIn();
	            },
	            success: function(response) {
	                console.log(response);

	                if(response.Status) {
	                	loadPublicHoliday();
	                }
	            },
	            cache: false,
	            xhr: function() {
	                myXhr = $.ajaxSettings.xhr();
	                if(myXhr.upload){
	                    myXhr.upload.addEventListener('progress',function(prog) {
	                        var uploadProgress = ((prog.loaded / prog.total) * 100);
	                        $('.progress-bar-inner').css({
	                            'width': uploadProgress + '%'
	                        });
	                        
	                        console.log(uploadProgress + ' %');
	                    }, false);
	                }
	                return myXhr;
	            },
	            complete: function() {
	                $('#divLoading').fadeOut();
	            }
	        });	
		})

		var modalPublicHoliday_ButtonCancel = $('#ModalPublicHoliday_ButtonCancel');
		modalPublicHoliday_ButtonCancel.off();
		modalPublicHoliday_ButtonCancel.on('click', function(evt) {
			$('#ModalPublicHoliday_ButtonSave').hide();
			$('#ModalPublicHoliday_ButtonCancel').hide();
			$('#ModalPublicHoliday_ButtonImport').fadeIn();
			$('#ModalPublicHoliday_ButtonAdd').fadeIn();
			$('#ModalPublicHoliday_Form').fadeOut();	
		});

		var buttonManagePublicHolidays = $('#buttonManagePublicHolidays');
		buttonManagePublicHolidays.off();
		buttonManagePublicHolidays.on('click', function(evt) {
			evt.preventDefault();

			$('#ModalPublicHoliday').modal('show');
			$('#ModalPublicHoliday_Year').select2('val', (new Date()).getFullYear());
			$('#ModalPublicHoliday_ButtonSave').hide();
			$('#ModalPublicHoliday_ButtonCancel').hide();
			$('#ModalPublicHoliday_ButtonImport').fadeIn();
			$('#ModalPublicHoliday_ButtonAdd').fadeIn();
			$('#ModalPublicHoliday_Form').fadeOut();
			loadPublicHoliday();
		});

		var modalPublicHoliday_ButtonAdd = $('#ModalPublicHoliday_ButtonAdd');
		modalPublicHoliday_ButtonAdd.off();
		modalPublicHoliday_ButtonAdd.on('click', function(evt) {
			evt.preventDefault();

			$('#ModalPublicHoliday_Form').fadeIn();
			$('#ModalPublicHoliday_HolidayDate').val('');
			$('#ModalPublicHoliday_HolidayName').val('');
			$('#ModalPublicHoliday_ButtonAdd').hide();
			$('#ModalPublicHoliday_ButtonImport').hide();
			$('#ModalPublicHoliday_ButtonSave').fadeIn();
			$('#ModalPublicHoliday_ButtonCancel').fadeIn();
		});

		var modalPublicHoliday_ButtonSave = $('#ModalPublicHoliday_ButtonSave');
		modalPublicHoliday_ButtonSave.off();
		modalPublicHoliday_ButtonSave.on('click', function(evt) {
			evt.preventDefault();

			var holidayDate = $('#ModalPublicHoliday_HolidayDate').val();
			var holidayName = $('#ModalPublicHoliday_HolidayName').val();
			if(holidayDate && holidayName) {
				fnAjax({
					url: '/API/PublicHoliday/Save.ashx',
					data: {
						HolidayName:  holidayName,
						HolidayDate: holidayDate
					},
					success: function(response) {
						if(response.Status) {
							loadPublicHoliday();
							$('#ModalPublicHoliday_ButtonSave').hide();
							$('#ModalPublicHoliday_ButtonCancel').hide();
							$('#ModalPublicHoliday_ButtonAdd').fadeIn();
							$('#ModalPublicHoliday_ButtonImport').fadeIn();
							$('#ModalPublicHoliday_Form').fadeOut();
						}

						fnShowMessage('Information', response.Message);
					}
				});
			}
			else {
				fnShowMessage('Warning', 'Holiday date and Holiday name cannot be empty');
			}
		});

		var buttonManageClinicOperatingHours = $('#buttonManageClinicOperatingHours');
		buttonManageClinicOperatingHours.off();
		buttonManageClinicOperatingHours.on('click', function(evt) {
			evt.preventDefault();

			$('#ModalManageDay').modal('show');
			$('#ModalManageDay_ClinicCode').select2('val', '');
			$('#ModalManageDay_Monday').val('true');
			$('#ModalManageDay_Tuesday').val('true');
			$('#ModalManageDay_Wednesday').val('true');
			$('#ModalManageDay_Thursday').val('true');
			$('#ModalManageDay_Friday').val('true');
			$('#ModalManageDay_Saturday').val('false');
			$('#ModalManageDay_Sunday').val('false');
			$('#ModalManageDay_OpenOnPublicHoliday').val('false');
			$('#ModalManageDay_StartDate').val(moment(new Date()).format('d MMMM YYYY'));

			var buttonSave = $('#ModalManageDay_ButtonSave');
			buttonSave.off();
			buttonSave.on('click', function(evt) {
				evt.preventDefault();

				var clinicCode = $('#ModalManageDay_ClinicCode').val();
				var sunday = $('#ModalManageDay_Sunday').val();
				var monday = $('#ModalManageDay_Monday').val();
				var tuesday = $('#ModalManageDay_Tuesday').val();
				var wednesday = $('#ModalManageDay_Wednesday').val();
				var thursday = $('#ModalManageDay_Thursday').val();
				var friday = $('#ModalManageDay_Friday').val();
				var saturday = $('#ModalManageDay_Saturday').val();
				var openOnHoliday = $('#ModalManageDay_OpenOnPublicHoliday').val();
				var startDate = $('#ModalManageDay_StartDate').val();

				if(startDate && clinicCode) {
					fnAjax({
						url: '/API/OperatingHours/OverrideOperatingHours.ashx',
						data: {
							ModalManageDay_ClinicCode: clinicCode,
							ModalManageDay_Sunday: sunday,
							ModalManageDay_Monday: monday,
							ModalManageDay_Tuesday: tuesday,
							ModalManageDay_Wednesday: wednesday,
							ModalManageDay_Thursday: thursday,
							ModalManageDay_Friday: friday,
							ModalManageDay_Saturday: saturday,
							ModalManageDay_OpenOnPublicHoliday: openOnHoliday,
							ModalManageDay_StartDate: startDate
						},
						success: function(response) {
							if(response.Status) {
								$('#ClinicCode').select2('val', clinicCode);
								$('#Year').select2('val', (new Date()).getFullYear());
								loadOperationHours();
								$('#ModalManageDay').modal('hide');
							}
							else {
								fnShowMessage('Information', response.Message);
							}
						} 
					});
				}
				else {
					fnShowMessage('Information', '"Date Effective From" and Clinic cannot be empty');
				}
			});
		});
	}

	function loadOperationHours() {
		var data = $('#formData').serializeObject();

		 $('#example tfoot th').each( function () {
	        var title = $('#table-operation-hours thead tr').eq( $(this).index() ).text();
	        $(this).html( '<input type="text" placeholder="Search '+title+'" />' );
	    } );

		if(data.Year) {
			$('#table-operation-hours').dataTable({
				bFilter: true, 
				bInfo: true,
				"sDom": "<'exportOptions'T><'table-responsive't><'row'<p i>>",
	            // "sPaginationType": "bootstrap",
	            "destroy": true,
	            "scrollCollapse": true,
	            "oLanguage": {
	                "sLengthMenu": "_MENU_ ",
	                "sInfo": "Showing <b>_START_ to _END_</b> of _TOTAL_ entries"
	            },
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
	                var btnHistory = __this.children('td').children('[data-action="history"]');

	                btnEdit.off();
	                btnEdit.on('click', function () {
	                 	$('#ModalOperationHoursEdit_OperationDate').text(moment(aData.OperationDate).format('DD MMMM YYYY')); 
	                 	$('#ModalOperationHoursEdit_Status').val(aData.Status.toString()); 
	                 	$('#ModalOperationHoursEdit').modal('show'); 

	                 	var buttonSave = $('#ModalOperationHoursEdit_ButtonSave');
	                 	buttonSave.off();
	                 	buttonSave.on('click', function(evt) {
	                 		var data = {
	                 			Status: $('#ModalOperationHoursEdit_Status').val(),
	                 			OperationDate: aData.OperationDate,
	                 			ClinicCode: aData.ClinicCode
	                 		};
	                 		fnAjax({
	                 			url: 'API/OperatingHours/Edit.ashx',
	                 			data: data,
	                 			success: function(response) {
	                 				if(response.Status == true) {
	                 					loadOperationHours();	
	                 					$('#ModalOperationHoursEdit').modal('hide'); 
	                 				}
	                 			}
	                 		});
	                 	});
	                });

	                btnHistory.off();
	                btnHistory.on('click', function () {
	                	var strHtml = '<table border="1px solid black;">'
	                				+ '<thead>'
	                				+ '<tr style="font-weight: bolder">'
	                				+ '<td style="padding: 5px;">Status</td>'
	                				+ '<td style="padding: 5px;">Date Modified</td>'
	                				+ '<td style="padding: 5px;">Modified By</td>'
	                				+ '</tr>'
	                				+ '</thead>'
	                				+ '<tbody>'
	                				+ '<tr>'
	                				+ '<td style="padding: 5px;">' + ( aData.Status == true ? 'Open' : 'Closed' ) + '</td>'
	                				+ '<td style="padding: 5px;">' + moment(aData.ModifiendOn).format('DD MMMM YYYY') + '</td>'
	                				+ '<td style="padding: 5px;">' + aData.ModifiedBy + '</td>'
	                				+ '</tr>'
	                				+ '</tbody>'
	                			    + '</table>'

	                  	$('#ModalOperationHoursHistory_History').html(strHtml);
	                  	$('#ModalOperationHoursHistory_OperationDate').html('<strong style="font-size: 18px;">Date selected : ' + moment(aData.OperationDate).format('DD MMMM YYYY')+ '</strong>');
	                  	$('#ModalOperationHoursHistory').modal('show');
	                });
	            }
			});
		}
	}



	function loadPublicHoliday() {
		fnAjax({
			url: '/API/PublicHoliday/List.ashx',
			data: {
				Year: $('#ModalPublicHoliday_Year').select2('val')
			},
			success: function(response) {
				var strHtml = '';

				if(response.length > 0) {
					$.each(response || [], function(index, data) {
						strHtml += '<tr>';
						strHtml += '<td>' + moment(data.HolidayDate).format("DD MMMM YYYY") + '</td>';
						strHtml += '<td>' + data.HolidayName + '</td>';
						strHtml += '<td>' + moment(data.CreatedOn).format("DD MMMM YYYY") + '</td>';
						strHtml += '<td>' + data.CreatedBy + '</td>';
						strHtml += '<td><span style="cursor: pointer;" onclick="deletePublicHoliday(\'' + data.RecordId + '\')">Delete</span></td>';
						strHtml += '</tr>';
					});
				}
				else {
					strHtml += '<td colspan="5" style="text-align: center;"><strong>No Data Available ...</strong></td>';
				}

				$('#ModalPublicHoliday_List_Body').html(strHtml);
			}
		});
	}

	
});

function deletePublicHoliday(recordId) {
	fnAjax({
		url: '/API/PublicHoliday/Delete.ashx',
		data: {
			RecordId: recordId
		},
		success: function(response) {
			if(response.Status == true) {
				loadPublicHoliday();
				fnShowMessage('Information', response.Message);
			}
			else {
				fnShowMessage('Warning', 'Cannot delete public holiday!');
			}
		}
	});		
}

function loadPublicHoliday() {
	fnAjax({
		url: '/API/PublicHoliday/List.ashx',
		data: {
			Year: $('#ModalPublicHoliday_Year').select2('val')
		},
		success: function(response) {
			var strHtml = '';

			if(response.length > 0) {
				$.each(response || [], function(index, data) {
					strHtml += '<tr>';
					strHtml += '<td>' + moment(data.HolidayDate).format("DD MMMM YYYY") + '</td>';
					strHtml += '<td>' + data.HolidayName + '</td>';
					strHtml += '<td>' + moment(data.CreatedOn).format("DD MMMM YYYY") + '</td>';
					strHtml += '<td>' + data.CreatedBy + '</td>';
					strHtml += '<td><span style="cursor: pointer;" onclick="deletePublicHoliday(\'' + data.RecordId + '\')">Delete</span></td>';
					strHtml += '</tr>';
				});
			}
			else {
				strHtml += '<td colspan="5" style="text-align: center;"><strong>No Data Available ...</strong></td>';
			}

			$('#ModalPublicHoliday_List_Body').html(strHtml);
		}
	});
}