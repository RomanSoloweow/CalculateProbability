function Post(Handler, Data)
{
    var result;
    $.ajax({
        type: "Post",
        url: "?handler=" + Handler,
        async: false,
        data: Data,
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        dataType: "json",
        success: function (response)
        {
            result =  response;
        },
        failure: function (response)
        {
            return response;
        }
    });
    return JSON.parse(result);
}

function GetData()
{
	let Data =
	{
		paramName: $("#paramName option:selected").text(),
		from: $("#from").val(),
		to: $("#to").val(),
		countDote: $("#from").val(),
		Tn: $("#Tn").val(),
		T0: $("#T0").val(),
		S: $("#S").val(),
		F: $("#F").val(),
		Fv: $("#Fv").val(),
		Eps: $("#Eps").val()
	};
	var Result = Post("GetData", Data);
	Result.Data.unshift(Result.Names);
	resData = Result.Data;
	showChart2(resData);
	
	resDataObj = [];
	resData.forEach(function (item) {
		tempObj = { x: item[0], y: item[1]};
		resDataObj.push(item)
	});

	localStorage.data = JSON.stringify(resDataObj);

}

$(window).resize(function () {
	let data = JSON.parse(localStorage.data);
	showChart2(data);
});