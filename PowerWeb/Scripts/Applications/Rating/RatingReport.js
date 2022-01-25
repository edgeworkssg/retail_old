$(document).ready(function() {
    if ($("#paramGroupBy").val() == "")
        $("#paramGroupBy").val("staff");
    if ($("#paramFilter").val() == "")
        $("#paramFilter").val("today");
    if ($("#paramStartDate").val() == "")
        $("#paramStartDate").val(moment().format('YYYY-MM-DD') + ' 00:00:00');
    if ($("#paramEndDate").val() == "")
        $("#paramEndDate").val(moment().format('YYYY-MM-DD') + ' 23:59:59');

    loadDataSource();
    SetGroupBy();
    RenderReport();
    SetFilter();

    $("#btnStaff").click(function(evt) {
        evt.preventDefault();
        $("#paramGroupBy").val("staff");
        SetGroupBy();
        RenderReport();
    });

    $("#btnOutlet").click(function(evt) {
        evt.preventDefault();
        $("#paramGroupBy").val("outlet");
        SetGroupBy();
        RenderReport();
    });

    $("#filterToday").click(function(evt) {
        evt.preventDefault();
        $("#paramFilter").val("today");
        SetFilter();
        RenderReport();
    });

    $("#filterWeek").click(function(evt) {
        evt.preventDefault();
        $("#paramFilter").val("week");
        SetFilter();
        RenderReport();
    });

    $("#filterMonth").click(function(evt) {
        evt.preventDefault();
        $("#paramFilter").val("month");
        SetFilter();
        RenderReport();
    });

    $("#filterYear").click(function(evt) {
        evt.preventDefault();
        $("#paramFilter").val("year");
        SetFilter();
        RenderReport();
    });

    $("#btnNext").click(function(evt) {
        evt.preventDefault();
        SetDate('next');
        RenderReport();
    });
    
    $("#btnPrev").click(function(evt) {
        evt.preventDefault();
        SetDate('prev');
        RenderReport();
    });
});

function SetFilter() {
    var filter = $("#paramFilter").val();

    $("#filterToday").removeClass("filter-active");
    $("#filterToday").addClass("filter-nonactive");
    $("#filterWeek").removeClass("filter-active");
    $("#filterWeek").addClass("filter-nonactive");
    $("#filterMonth").removeClass("filter-active");
    $("#filterMonth").addClass("filter-nonactive");
    $("#filterYear").removeClass("filter-active");
    $("#filterYear").addClass("filter-nonactive");

    if (filter.toLowerCase() == "year") {
        var syear = moment().startOf('year');
        var eyear = moment().endOf('year');

        $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
        $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

        $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));

        $("#filterYear").removeClass("filter-nonactive");
        $("#filterYear").addClass("filter-active");

    } else if (filter.toLowerCase() == "month") {
        var syear = moment().startOf('month');
        var eyear = moment().endOf('month');

        $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
        $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

        $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));

        $("#filterMonth").removeClass("filter-nonactive");
        $("#filterMonth").addClass("filter-active");

    } if (filter.toLowerCase() == "week") {
        var syear = moment().startOf('isoweek');
        var eyear = moment().endOf('isoweek');


        $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
        $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

        $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));

        $("#filterWeek").removeClass("filter-nonactive");
        $("#filterWeek").addClass("filter-active");

    } else if (filter.toLowerCase() == "today") {
        var sdate = moment();

        $("#paramStartDate").val(sdate.format('YYYY-MM-DD') + ' 00:00:00');
        $("#paramEndDate").val(sdate.format('YYYY-MM-DD') + ' 23:59:59');

        $("#lblDate").html(sdate.format('DD-MMM-YYYY'));

        $("#filterToday").removeClass("filter-nonactive");
        $("#filterToday").addClass("filter-active");
    }
}

function SetGroupBy() {
    var groupby = $("#paramGroupBy").val();
       
    $("#btnStaff").removeClass("button-gray");
    $("#btnOutlet").removeClass("button-gray");

    if (groupby.toLowerCase() == "outlet") {
        $("#btnStaff").addClass("button-gray");
    } else {
        $("#btnOutlet").addClass("button-gray");
    }
}

function SetDate(type) {
    var filter = $("#paramFilter").val();
    
    if (type == 'next') {
        if (filter.toLowerCase() == "year") {
            var syear = moment($("#paramStartDate").val(), 'YYYY-MM-DD HH:mm:ss').add(1, 'years');
            var eyear = moment($("#paramEndDate").val(), 'YYYY-MM-DD HH:mm:ss').add(1, 'years');

            $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
            $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

            $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));
        } else if (filter.toLowerCase() == "month") {
            var syear = moment($("#paramStartDate").val(), 'YYYY-MM-DD HH:mm:ss').add(1, 'months');
            var eyear = moment($("#paramEndDate").val(), 'YYYY-MM-DD HH:mm:ss').add(1, 'months');
        
            $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
            $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

            $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));
        } if (filter.toLowerCase() == "week") {
            var syear = moment($("#paramStartDate").val(), 'YYYY-MM-DD HH:mm:ss').add(7, 'days');
            var eyear = moment($("#paramEndDate").val(), 'YYYY-MM-DD HH:mm:ss').add(7, 'days');
    
            $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
            $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

            $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));
        } else if (filter.toLowerCase() == "today") {
            var param = $("#paramStartDate").val();
            var sdate = moment(param, 'YYYY-MM-DD HH:mm:ss', true);
            var ndate = sdate.add(1, 'days');

            $("#paramStartDate").val(ndate.format('YYYY-MM-DD') + ' 00:00:00');
            $("#paramEndDate").val(ndate.format('YYYY-MM-DD') + ' 23:59:59');

            $("#lblDate").html(ndate.format('DD-MMM-YYYY'));
        }
    } else if(type == 'prev'){
    if (filter.toLowerCase() == "year") {
        var syear = moment($("#paramStartDate").val(), 'YYYY-MM-DD HH:mm:ss').add(-1, 'years');
        var eyear = moment($("#paramEndDate").val(), 'YYYY-MM-DD HH:mm:ss').add(-1, 'years');

            $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
            $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

            $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));
        } else if (filter.toLowerCase() == "month") {
            var syear = moment($("#paramStartDate").val(), 'YYYY-MM-DD HH:mm:ss').add(-1, 'months');
            var eyear = moment($("#paramEndDate").val(), 'YYYY-MM-DD HH:mm:ss').add(-1, 'months');

            $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
            $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

            $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));
        } if (filter.toLowerCase() == "week") {
            var syear = moment($("#paramStartDate").val(), 'YYYY-MM-DD HH:mm:ss').add(-7, 'days');
            var eyear = moment($("#paramEndDate").val(), 'YYYY-MM-DD HH:mm:ss').add(-7, 'days');

            $("#paramStartDate").val(syear.format('YYYY-MM-DD') + ' 00:00:00');
            $("#paramEndDate").val(eyear.format('YYYY-MM-DD') + ' 23:59:59');

            $("#lblDate").html(syear.format('DD-MMM-YYYY') + ' - ' + eyear.format('DD-MMM-YYYY'));
        } else if (filter.toLowerCase() == "today") {
            var param = $("#paramStartDate").val();
            var sdate = moment(param, 'YYYY-MM-DD HH:mm:ss', true);
            var ndate = sdate.add(-1, 'days');

            $("#paramStartDate").val(ndate.format('YYYY-MM-DD') + ' 00:00:00');
            $("#paramEndDate").val(ndate.format('YYYY-MM-DD') + ' 23:59:59');

            $("#lblDate").html(ndate.format('DD-MMM-YYYY'));
        }
    }
}

function RenderReport() {
    var startdate = $("#paramStartDate").val();
    var enddate = $("#paramEndDate").val();
    var groupby = $("#paramGroupBy").val();
    var filter = $("#paramFilter").val();
    var strdate = $("#lblDate").html();
    var filteroutlet = $("#ddlOutlet").val();
    var filterstaff = $("#txtStaff").val();

    ajaxPost({
        url: app.setting.baseUrl + "API/Lookup/RatingReport.ashx?groupby=" + groupby + "&&filter=" + filter + "&&startdate=" + startdate + "&&enddate=" + enddate + "&&filteroutlet=" + filteroutlet + "&&filterstaff=" + filterstaff,
        success: function(result) {

            if (result.msg == "") {
                $("#rating").empty();

                var strhtml = "";

                if (result.header.length > 0) {
                    for (var i = 0; i < result.header.length; i++) {
                        if (groupby == "outlet") {
                            strhtml += [
                            "<div class='red-border'><div class='subhdr'>",
                            "<div class='alignleft'>OUTLET : " + result.header[i].OutletName + "</div>",
                            "<div class='aligncenter'>POS : " + result.header[i].PointOfSaleName + "</div>",
                            "<div class='alignright'>Date: " + strdate + "</div>",
                            "<div class='clear'></div></div>",
                            "<div class='subhdr'><div class='col-xs-2'><span class='current'>SCORE : " + result.header[i].CurrentScore + " %&nbsp; (" + result.header[i].SumCurrentRating + ")</span></div>",
                            "<div class='col-xs-8'><span class='last'>LAST SCORE : " + result.header[i].LastScore + " %&nbsp;(" + result.header[i].SumLastRating + ")</span></div>",
                            "<div class='clear'></div></div>"
                            ].join("");

                            var posid = result.header[i].PointOfSaleID;
                            var sumcurrent = parseInt(result.header[i].SumCurrentRating);
                            var sumlast = parseInt(result.header[i].SumLastRating);

                            for (var j = 0; j < result.detail.length; j++) {
                                if (result.detail[j].PointOfSaleID == posid) {

                                    var procurrent = 0;
                                    var prolast = 0;

                                    if (sumcurrent > 0)
                                        procurrent = Math.floor((parseInt(result.detail[j].CurrentRating) / sumcurrent) * 100);

                                    if (sumlast > 0)
                                        prolast = Math.floor((parseInt(result.detail[j].LastRating) / sumlast) * 100);

                                    strhtml += [
                                        "<div class='black-border'><div class='col-xs-2'>",
                                        "<img class='img-responsive' src='../Rating/" + result.detail[j].RatingImageUrl + "' style='width: 45px; margin: 0 auto;'></img></div>",
                                        "<div class='col-xs-2'>" + result.detail[j].RatingName + "</div>",
                                        "<div class='col-xs-1'><div class='col-compact current'>" + result.detail[j].CurrentRating + "</div>",
                                        "<div class='col-compact last'>" + result.detail[j].LastRating + "</div>",
                                        "</div>",
                                        "<div class='col-xs-6'>",
                                        "<div class='col-compact' style='height: 10px; background-color: #EFF7EA'>",
                                        "<div class='grating bg-current' style='width: " + procurrent + "%'></div>",
                                        "</div>",
                                        "<div class='col-compact' style='height: 10px; background-color: #FFF9E4'>",
                                        "<div class='grating bg-last' style='width: " + prolast + "%'></div>",
                                        "</div>",
                                        "</div>",
                                        "<div class='col-xs-1'>",
                                        "<div class='col-compact current'>" + procurrent + " %</div>",
                                        "<div class='col-compact last'>" + prolast + "%</div>",
                                        "</div>",
                                        "<div class='clear'></div></div>"
                                ].join("");
                                }
                            }

                            strhtml += "<div class='black-border'></div></div>";
                        }
                        else {
                            strhtml += [
                            "<div class='red-border'><div class='subhdr'>",
                            "<div class='alignleft'>Staff : " + result.header[i].DisplayName + "</div>",
                            "<div class='aligncenter'>&nbsp;</div>",
                            "<div class='alignright'>Date: " + strdate + "</div>",
                            "<div class='clear'></div></div>",
                            "<div class='subhdr'><div class='col-xs-2'><span class='current'>SCORE : " + result.header[i].CurrentScore + "  %&nbsp;(" + result.header[i].SumCurrentRating + ")</span></div>",
                            "<div class='col-xs-8'><span class='last'>LAST SCORE : " + result.header[i].LastScore + " %&nbsp;(" + result.header[i].SumLastRating + ")</span></div>",
                            "<div class='clear'></div></div>",
                            ].join("");

                            var username = result.header[i].UserName;
                            var sumcurrent = parseInt(result.header[i].SumCurrentRating);
                            var sumlast = parseInt(result.header[i].SumLastRating);

                            for (var j = 0; j < result.detail.length; j++) {
                                if (result.detail[j].UserName == username) {

                                    var procurrent = 0;
                                    var prolast = 0;

                                    if (sumcurrent > 0)
                                        procurrent = Math.floor((parseInt(result.detail[j].CurrentRating) / sumcurrent) * 100);

                                    if (sumlast > 0)
                                        prolast = Math.floor((parseInt(result.detail[j].LastRating) / sumlast) * 100);

                                    strhtml += [
                                       "<div class='black-border'><div class='col-xs-2'>",
                                        "<img class='img-responsive' src='../Rating/" + result.detail[j].RatingImageUrl + "' style='width: 45px; margin: 0 auto;'></img></div>",
                                        "<div class='col-xs-2'>" + result.detail[j].RatingName + "</div>",
                                        "<div class='col-xs-1'><div class='col-compact current'>" + result.detail[j].CurrentRating + "</div>",
                                        "<div class='col-compact last'>" + result.detail[j].LastRating + "</div>",
                                        "</div>",
                                        "<div class='col-xs-6'>",
                                        "<div class='col-compact' style='height: 10px; background-color: #EFF7EA'>",
                                        "<div class='grating bg-current' style='width: " + procurrent + "%'></div>",
                                        "</div>",
                                        "<div class='col-compact' style='height: 10px; background-color: #FFF9E4'>",
                                        "<div class='grating bg-last' style='width: " + prolast + "%'></div>",
                                        "</div>",
                                        "</div>",
                                        "<div class='col-xs-1'>",
                                        "<div class='col-compact current'>" + procurrent + " %</div>",
                                        "<div class='col-compact last'>" + prolast + "%</div>",
                                        "</div>",
                                        "<div class='clear'></div></div>"
                                    ].join("");
                                }
                            }
                            strhtml += "<div class='black-border'></div></div>";
                        }
                    }
                    strhtml += "<div class='red-border'></div></div>";
                }
                else {
                    strhtml += "<div class='red-border'><h2 style='text-align:center'>NO DATA<h2></div><div class='red-border'></div>";
                }

                $("#rating").html(strhtml);
            }
            else {
                alert(result.msg);
            }
        }
    });
}

function loadDataSource() {
    ajaxPost({
        url: app.setting.baseUrl + 'API/Lookup/Outlet.ashx',
        success: function(result) {
            loadDropdownDataSource('ddlOutlet', result, '-- Select One --');
        }
    });
}
