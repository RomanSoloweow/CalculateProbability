function resetZoom() {
    window.myLine.resetZoom();
}

let config = {
    type: 'line',
    datasets: [{
    }],

    options: {
        legend: {
            display: true,
            position: 'top',
            labels: {
                boxWidth: 80,
                fontColor: 'black'
            }
        },
        scales: {
            xAxes: [
                {
                    scaleLabel: {
                        display: true
                    }
                }
            ],
            yAxes: [
                {
                    scaleLabel: {
                        display: true,
                        labelString: "Значение"
                    }
                }
            ]
        },
        pan: {
            enabled: true,
            mode: 'xy',
            speed: 1
        },
        zoom: {
            enabled: true,
            drag: false,
            mode: 'xy',
            speed: 0.01
        }
    }

};

window.onload = function () {
    var ctx = document.getElementById("canvas");
    window.myLine = new Chart(ctx, config);
};

function ShowChart()
{
    color = "27,110,194"
    dataArr =
     [
        [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
        [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    ]
    config.data = {
        labels: dataArr[0],
        datasets: [{
            label: "График",
            data: dataArr[1],
            borderColor: "rgb( "+color+")",
            backgroundColor: "rgb( " + color + ",0.5)",
            pointBorderColor: "rgb( " + color + ")",
            pointBackgroundColor: "rgb( " + color + ")",
            pointBorderWidth: 1,
            fill: true
        }]
    }
    config.options.scales.xAxes[0].scaleLabel.labelString = "new Data"
    window.myLine.update();
}
