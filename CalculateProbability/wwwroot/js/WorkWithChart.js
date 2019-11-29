//функция для сброса масштабирования графика
function resetZoom()
{
    window.myLine.resetZoom();
}
//настройки для графика
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
                [{
                    scaleLabel:
                    {
                        display: true
                    }
                 }],
            yAxes:
                [{
                    scaleLabel:
                    {
                        display: true
                    }
                 }]
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

    var result = this.GetObjFromStorage();
    if (result == null)
        return;

    result = this.ObjFromJson(result);
    showChart2(result);
};
//Функция показать график
function showChart2(Result)
{

    color = "27,110,194"
    config.data = {
        labels: Result[Result.ParameterSelect],
        datasets: [{
            data: Result.P,
            borderColor: "rgb( " + color + ")",
            backgroundColor: "rgb( " + color + ",0.5)",
            pointBorderColor: "rgb( " + color + ")",
            pointBackgroundColor: "rgb( " + color + ")",
            pointBorderWidth: 1,
            fill: true
        }]
    }
    config.options.scales.yAxes[0].scaleLabel.labelString = "P";
    config.options.scales.xAxes[0].scaleLabel.labelString = Result.ParameterSelect;
    window.myLine.update();
};
