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
            pointSize: 5,
            hAxis:
            {
				title: 'Year'
			},
            vAxis:
            {
				title: "Month"
			},
			explorer: {
				actions: ['dragToZoom', 'rightClickToReset'],
				axis: 'horizontal',
				keepInBounds: true,
				maxZoomIn: 4.0
			},
            colors: ['#1b6ec2'],
			width: $(window).width(),
			height: $(window).height() * 0.7
		};

        var chart = new google.visualization.AreaChart(document.getElementById('map_canvas'));
		chart.draw(data, options);
	}
}

//$(window).resize(function () {
//    showChart2();
//});
