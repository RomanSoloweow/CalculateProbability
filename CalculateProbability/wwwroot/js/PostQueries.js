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
    var Data = GetParametersObjFromForm();
    Post("GetData", Data, function (Result)
	{
        if (Result.ErrorMessage)
        {
            ShowError(Result.ErrorMessage);          
        }
        else
        {
            var ObjJson = ObjToJson(Result);
            SaveObjToStorage(ObjJson);
            SaveParametersToStorage();

            showChart2(Result);
		}
    });
}

