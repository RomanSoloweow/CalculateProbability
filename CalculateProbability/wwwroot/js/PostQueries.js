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
    var Data =
    {
        параметр: "Значение"
    };
    var Result = Post("GetData", Data);
}