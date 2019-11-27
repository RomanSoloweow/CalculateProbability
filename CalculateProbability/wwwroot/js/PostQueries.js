function Post(Handler, Data, FunctionOnSuccess, FunctionOnFailure)
{
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
            FunctionOnSuccess(response);
        },
        failure: function (response)
        {
            if (FunctionOnFailure)
                FunctionOnFailure(response);
        }
    });

}