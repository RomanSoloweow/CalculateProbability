function ShowError(ErrorMessage)
{   
    $("#Error").css('display', 'block')
    $("#ErrorText").text(ErrorMessage);
}
function CloseError()
{
    $("#Error").css('display', 'none')
}