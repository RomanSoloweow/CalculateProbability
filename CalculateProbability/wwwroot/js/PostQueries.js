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
        Result.Data.unshift(Result.Names);
        resData = Result.Data;
        showChart2(resData);

        resDataObj = [];
        resData.forEach(function (item)
        {
            tempObj = { x: item[0], y: item[1] };
            resDataObj.push(item)
        });

        //localStorage.data = JSON.stringify(resDataObj);
    });


}