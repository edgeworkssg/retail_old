var myExampleData = {};

//pie Chart sample data and options
myExampleData.pieChartData = [{
    data: [[0, 4]],
    label: "Max"
}, {
    data: [[0, 3]],
    label: "Kurt"
}, {
    data: [[0, 1.03]],
    label: "Alvin",
    pie: {
        explode: 50
    }
}, {
    data: [[0, 3.5]],
    label: "Sophie"
}];

    myExampleData.pieChartOptions = {
        HtmlText: false,
        grid: {
            verticalLines: false,
            horizontalLines: false
        },
        xaxis: {
            showLabels: false
        },
        yaxis: {
            showLabels: false
        },
        pie: {
            show: true,
            explode: 6
        },
        mouse: {
            track: true
        },
        legend: {
            position: "se",
            backgroundColor: "#D2E8FF"
        }
    };

    //Pie chart sample data ends here

    //bar Chart sample data and options

    myExampleData.constructBubbleChartData = function() {
        var d1 = [];
        var d2 = []
        var point
        var i;

        for (i = 0; i < 10; i++) {
            point = [i, Math.ceil(Math.random() * 10), Math.ceil(Math.random() * 10)];
            d1.push(point);

            point = [i, Math.ceil(Math.random() * 10), Math.ceil(Math.random() * 10)];
            d2.push(point);
        }
        return [d1, d2];
    };
    myExampleData.bubbleChartData = myExampleData.constructBubbleChartData();

    myExampleData.bubbleChartOptions = {
        bubbles: {
            show: true,
            baseRadius: 5
        },
        xaxis: {
            min: -4,
            max: 14
        },
        yaxis: {
            min: -4,
            max: 14
        },
        mouse: {
            track: true,
            relative: true
        }
    };

    //bar chart sample data ends here

    //bar Chart sample data and options

    myExampleData.constructBarChartData = function() {
        var d1 = [];
        var d2 = []
        var point
        var i;
        for (i = 0; i < 4; i++) {
            point = [Math.ceil(Math.random() * 10), i];
            d1.push(point);
            point = [Math.ceil(Math.random() * 10), i + 0.5];

            d2.push(point);
        }
        return [d1, d2];
    };
    myExampleData.barChartData = myExampleData.constructBarChartData();

    myExampleData.barChartOptions = {
        bars: {
            show: true,
            horizontal: false,
            shadowSize: 0,
            barWidth: 0.5,
            stacked: true
        },
        mouse: {
            track: true,
            relative: true
        },
        yaxis: {
            min: 0,
            autoscaleMargin: 1
        },
        xaxis: {
            noTicks: 12,
            labelsAngle: 30,
            tickFormatter: function(x) {
                var 
x = parseInt(x),
months = ['Jan-2013', 'Apr-2013', 'Jul-2013', 'Oct-2013', 'Jan-2014', 'Apr-2014', 'Jul-2014', 'Oct-2014', 'Jan-2015', 'Apr-2015', 'Jul-2015', 'Oct-2015'];
                return months[x];
            }
        }
    };

    myExampleData.barChartOptions2 = {
        bars: {
            show: true,
            horizontal: true,
            shadowSize: 0,
            barWidth: 0.5,
            stacked: true
        },
        mouse: {
            track: true,
            relative: true
        },
        xaxis: {
            min: 0,
            autoscaleMargin: 1
        },
        yaxis: {
            noTicks: 12,
            labelsAngle: 30,
            tickFormatter: function(y) {
                var 
y = parseInt(y),
months = ['Jan-2013', 'Apr-2013', 'Jul-2013', 'Oct-2013', 'Jan-2014', 'Apr-2014', 'Jul-2014', 'Oct-2014', 'Jan-2015', 'Apr-2015', 'Jul-2015', 'Oct-2015'];
                return months[y];
            }
        }
    };

    //bar chart sample data ends here

    //line Chart sample data and options
    myExampleData.constructLineChartData = function() {
        var d1 = [[1, 3]
				, [2, 6]
				, [3, 9]
				, [4, 12]
				, [5, 15]
			 ];
        return [d1];
    };
    myExampleData.lineChartData = myExampleData.constructLineChartData();

    myExampleData.lineChartOptions = {
        mouse: {
            track: true,
            relative: true
        },
        points: { show: true },
        lines: { show: true },
        markers: {
            show: true,
            position: 'ct'
        },
        HtmlText: false,
        yaxis: {
            noTicks: 10,
            max: 130000,
            min: 0
        },
        xaxis: {
            noTicks: 12,
            labelsAngle: 30,
            tickFormatter: function(x) {
                var 
x = parseInt(x),
months = ['Jan-2013', 'Apr-2013', 'Jul-2013', 'Oct-2013', 'Jan-2014', 'Apr-2014', 'Jul-2014', 'Oct-2014', 'Jan-2015', 'Apr-2015', 'Jul-2015', 'Oct-2015'];
                return months[x];
            }
        },
        legend: {
            position: 'se',
            // Position the legend 'south-east'.
            backgroundColor: '#D2E8FF' // A light blue background color.
        },
        spreadsheet: {
            show: false,
            tickFormatter: function(e) { return e + ''; }
        }
    };

    //line chart sample data ends here

    //table Widget sample data and options

    myExampleData.constructTableWidgetData = function() {
        return ["Trident" + Math.ceil(Math.random() * 10), "IE" + Math.ceil(Math.random() * 10), "Win" + Math.ceil(Math.random() * 10)]
    };

    myExampleData.tableWidgetData = {
        "aaData": [myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData()
	],

        "aoColumns": [{
            "sTitle": "Engine"
        }, {
            "sTitle": "Browser"
        }, {
            "sTitle": "Platform"
}],
            "iDisplayLength": 25,
            "aLengthMenu": [[1, 25, 50, -1], [1, 25, 50, "All"]],
            "bPaginate": true,
            "bAutoWidth": false
        };
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //table widget sample data ends here
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
