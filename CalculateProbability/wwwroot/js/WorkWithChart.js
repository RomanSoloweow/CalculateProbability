//function showChart2(dataArr) {

//	google.charts.load('current', { 'packages': ['corechart'] });
//	google.charts.setOnLoadCallback(drawChart);
//	function drawChart() {
//        var data = google.visualization.arrayToDataTable(dataArr);
//		var options = {
//            pointSize: 5,
//            chartArea: { 'width': '90%'},
//            legend: { position: 'none' },
//			hAxis: {
//                title: dataArr[0][0]
//			},
//			vAxis: {
//                title: dataArr[0][1]
//			},
//			explorer: {
//                actions: ['dragToZoom', 'rightClickToReset'],
//				axis: 'horizontal',
//				keepInBounds: true,
//                maxZoomIn:5
//			},
//            colors: ['#1b6ec2'],
//			width: $(window).width(),
//			height: $(window).height()*0.8
//		};

//        var chart = new google.visualization.AreaChart(document.getElementById('map_canvas'));
//		chart.draw(data, options);
//	}
//}

//$(window).resize(function ()
//{
//    let data = JSON.parse(sessionStorage.data);
//    if(data)
//    showChart2(data);
//});
//$(document).ready(function ()
//{
//    var data = sessionStorage.getItem('data');
//    if (data)
//     showChart2(JSON.parse(data));
//});

function resetZoom() {
    window.myLine.resetZoom();
}

let config =
{
    type: 'line',
    datasets: [{
    }],

    options: {
        legend: {
            display: false,
            position: 'top',
            labels: {
                boxWidth: 80,
                fontColor: 'black'
            }
        },
        scales: {
            xAxes:
                [
                    {
                        scaleLabel:
                        {
                            display: true
                        }
                    }
                ],
            yAxes:
                [
                    {
                        scaleLabel:
                        {
                            display: true
                        }
                    }
                ]
        },
        pan:
        {
            enabled: true,
            mode: 'x',
            speed: 1
        },
        zoom: {
            enabled: true,
            drag: false,
            mode: 'x',
            speed: 0.01
        }
    }

};

window.onload = function ()
{
    var ctx = document.getElementById("canvas");
    window.myLine = new Chart(ctx, config);

    var Names = sessionStorage.getItem('Names');
    if (Names) {
        var P = sessionStorage.getItem('P');
        var ParameterValues = sessionStorage.getItem('ParameterValues');
        showChart2(JSON.parse(Names), JSON.parse(ParameterValues), JSON.parse(P));
    }
};
function showChart2(Names, ParameterValues, P) {
    color = "27,110,194"
    config.data = {
        labels: ParameterValues,
        datasets: [{
            data: P,
            borderColor: "rgb( " + color + ")",
            backgroundColor: "rgb( " + color + ",0.5)",
            pointBorderColor: "rgb( " + color + ")",
            pointBackgroundColor: "rgb( " + color + ")",
            pointBorderWidth: 1,
            fill: true
        }]
    }
    config.options.scales.yAxes[0].scaleLabel.labelString = Names[1]
    config.options.scales.xAxes[0].scaleLabel.labelString = Names[0]
    window.myLine.update();
};
