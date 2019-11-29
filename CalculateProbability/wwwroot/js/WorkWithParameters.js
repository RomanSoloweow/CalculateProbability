function ChangeSelect(element)
{
    $("#Tn").removeAttr('disabled');
    $("#T0").removeAttr('disabled');
    $("#S").removeAttr('disabled');
    $("#F").removeAttr('disabled');
    $("#Fv").removeAttr('disabled');
    var value = $(element).val();
    if (value)
    {
        $("#" + value).attr('disabled', 'disabled').val("");

    }
}