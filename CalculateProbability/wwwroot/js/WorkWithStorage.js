
var Parameters = ["ParameterSelect", "From", "To", "CountDotes", "Tn", "T0", "S", "F", "Fv", "Eps"];
var AllParameters = [ "P", "ParameterSelect", "From", "To", "CountDotes", "Tn", "T0", "S", "F", "Fv", "Eps"];

//Формирует объект из параметров формы
function GetParametersObjFromForm()
{
    var ParametersFromForm = new Object();
    var ParameterSelect = $("#ParameterSelect").val();
    if (ParameterSelect == null)
        return null;
    $.each(Parameters, function (index, value)
    {
        if (value != ParameterSelect)
        {
            var mass = [];
            mass.push($("#" + value).val());
            ParametersFromForm[value] = mass;
        }
    });
    return ParametersFromForm;
}

//Из полей на форме сохраняет параметры в sessionStorage
function SaveParametersToStorage()
{
    var obj = GetParametersObjFromForm();
    obj = ObjToJson(obj);
    SaveObjToStorage(obj);
}

//Из sessionStorage параметры выводит в поля на форме
function LoadParametersToStorage()
{
    var ParameterSelect = TryGet("#ParameterSelect");
    if (ParameterSelect == null)
        return;

    $.each(Parameters, function (index, value)
    {
        if (value != ParameterSelect) {
            $("#" + value).val(TryGet(value));
        }
        else
        {
            $("#" + value).val("");
        }
    });
}

function SaveObjToStorage(obj)
{
    if (obj == null)
        return null;

    for (var key in obj)
    {
        //var mass = [];
        //mass.push(obj[key]);
        //sessionStorage[key] = mass;
        sessionStorage[key] = obj[key];
    }
}

function GetObjFromStorage()
{
    var ResultObject = new Object();
    $.each(AllParameters, function (index, value)
    {
        ResultObject[value] = TryGet(value);
        if (ResultObject[value] == null)
        {
            ResultObject = null;
            return false;
        }
    });

    return ResultObject;
}

function TryGet(Name)
{
    return sessionStorage.getItem(Name);
}

function ObjToJson(obj)
{
    if (obj == null)
        return null;

    var ObjJson = new Object();
    for (var key in obj)
    {
        ObjJson[key] = JSON.stringify(obj[key]);
    }
    return ObjJson;
}

function ObjFromJson(obj)
{
    if (obj == null)
        return null;

    var ObjJson = new Object();
    for (var key in obj)
    {
        var value = obj[key];
        if (value.includes("["))
        {
            ObjJson[key] = JSON.parse(value);
        }
        else
        {
            ObjJson[key] = value;
        }
    }
    return ObjJson;
}