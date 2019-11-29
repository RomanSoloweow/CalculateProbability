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
    Post("GetData", Data, function (Result)
	{
        if (Result.ErrorMessage)
        {
            ShowError(Result.ErrorMessage);          
        }
        else {
			//ResultArr = [];
			//ResultArr.push(Result.Names);
			//$.each(Result.ParameterValues, function (index, item) {
			//	ResultArr.push([Result.ParameterValues[index], Result.P[index]]);
			//});
            showChart2(Result.Names,Result.ParameterValues, Result.P);
            sessionStorage.Names = JSON.stringify(Result.Names);
            sessionStorage.ParameterValues = JSON.stringify(Result.ParameterValues);
            sessionStorage.P = JSON.stringify(Result.P);
		}
    });
}

