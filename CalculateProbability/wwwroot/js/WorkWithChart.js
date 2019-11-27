function showChart2(dataArr) {

	google.charts.load('current', { 'packages': ['corechart'] });
	google.charts.setOnLoadCallback(drawChart);
	function drawChart() {
		var data = google.visualization.arrayToDataTable(dataArr);

		var options = {
            pointSize: 5,
            chartArea: { 'width': '90%'},
            legend: { position: 'none' },
			hAxis: {
				title: dataArr[0][0]
			},
			vAxis: {
                title: dataArr[0][1]
			},
			explorer: {
                actions: ['dragToZoom', 'rightClickToReset'],
				axis: 'horizontal',
				keepInBounds: true,
                maxZoomIn: 5,
                maxZoomOut: 5,
			},
            colors: ['#1b6ec2'],
			width: $(window).width(),
			height: $(window).height()*0.8
		};

        var chart = new google.visualization.AreaChart(document.getElementById('map_canvas'));
		chart.draw(data, options);
	}
}

$(window).resize(function () {
    showChart2();
});
