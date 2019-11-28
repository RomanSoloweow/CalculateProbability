function Post(Handler, Data, functionOnSuccess, functionOnFailure)
{
    var result;
    $.ajax({
        type: "Post",
        url: "?handler=" + Handler,
        data: Data,
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        dataType: "json",
        success: function (response)
        {
            functionOnSuccess(JSON.parse(response))
        },
        failure: function (response)
        {
            if (functionOnFailure)
                functionOnFailure(JSON.parse(response))
        }
    });
}

function GetData()
{
	let Data =
	{
        ParameterName: $("#ParameterSelect").val(),
		From: $("#From").val(),
		To: $("#To").val(),
        CountDote: $("#CountDots").val(),
		Tn: $("#Tn").val(),
		T0: $("#T0").val(),
		S: $("#S").val(),
		F: $("#F").val(),
		Fv: $("#Fv").val(),
		Eps: $("#Eps").val()
	};
	console.log(Data);
    Post("GetData", Data, function (Result)
	{
		if (Result.ErrorMessage)
			alert(Result.ErrorMessage)
		else {
			ResultArr = [];
			ResultArr.push(Result.Names);
			$.each(Result.ParameterValues, function (index, item) {
				ResultArr.push([Result.ParameterValues[index], Result.P[index]]);
			});
			showChart2(ResultArr);
			sessionStorage.data = JSON.stringify(ResultArr);
		}
    });
}

$(document).ready(function () {
	if (sessionStorage.getItem('data'))
		showChart2(JSON.pasre(sessionStorage.getItem('data')));
});