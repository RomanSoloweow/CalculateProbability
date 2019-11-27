function showChart2(dataArr) {

	google.charts.load('current', { 'packages': ['corechart'] });
	google.charts.setOnLoadCallback(drawChart);
	function drawChart() {
		var data = google.visualization.arrayToDataTable(
			dataArr
		);

		var options = {
			title: 'График',
			hAxis: {
				title: dataArr[0][0], titleTextStyle: { color: '#333' },
				slantedText: true, slantedTextAngle: 80
			},
			vAxis: {
				title: "Значение",
				minValue: 0
			},
			explorer: {
				actions: ['dragToZoom', 'rightClickToReset'],
				axis: 'horizontal',
				keepInBounds: true,
				maxZoomIn: 4.0
			},
			colors: ['#D44E41'],
			width: $(window).width(),
			height: $(window).height()
		};
		
		var chart = new google.visualization.LineChart(document.getElementById('canvas'));
		chart.draw(data, options);
	}
}