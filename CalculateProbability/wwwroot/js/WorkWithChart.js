function showChart2() {
	google.charts.load('current', { 'packages': ['corechart'] });
	google.charts.setOnLoadCallback(drawChart);
	function drawChart() {
		var data = google.visualization.arrayToDataTable([
			['Year', 'Sales'],
			[1, 1],
			[2, 2],
			[3, 4],
			[4, 2]
		]);

		var options = {
			title: 'График',
			hAxis: {
				title: 'Year', titleTextStyle: { color: '#333' },
				slantedText: true, slantedTextAngle: 80
			},
			vAxis: {
				title: "Month",
				minValue: 0
			},
			explorer: {
				actions: ['dragToZoom', 'rightClickToReset'],
				axis: 'horizontal',
				keepInBounds: true,
				maxZoomIn: 4.0
			},
			colors: ['#D44E41'],
			width: $(window).width() * 0.8,
			height: $(window).height() * 0.8
		};

		var chart = new google.visualization.LineChart(document.getElementById('canvas'));
		chart.draw(data, options);
	}
}