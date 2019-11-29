

function uploadFile()
{
	document.getElementById("file").click();
};

$('#file').change(function (e)
{
    var reader = new FileReader();
    var file = e.target.files[0];

    var Result = new Object;
    Result.Names = [];

	reader.readAsArrayBuffer(file);
    reader.onload = function (e)
    {
		var data = new Uint8Array(reader.result);
		wb = XLSX.read(data, { type: 'array' });
		let first_sheet_name = wb.SheetNames[0];
		let worksheet = wb.Sheets[first_sheet_name];
		var my_data = XLSX.utils.sheet_to_json(worksheet, { raw: true })

        Result.Names = extractHeader(worksheet);
        $.each(Result.Names, function (index, value)
        {  
            Result[value] = [];
        });
        $.each(my_data, function (index, value) 
        {
            $.each(Result.Names, function (index, name)
            {
                var fild = value[name];
                if (fild!=null)
                    Result[name].push(fild);
            });
        });


        var objJson = ObjToJson(Result);
        SaveObjToStorage(objJson);
        
        showChart2(Result);
     
	}
});

function extractHeader(ws)
{
	const header = []
	const columnCount = XLSX.utils.decode_range(ws['!ref']).e.c + 1
    for (let i = 0; i < columnCount; ++i)
    {
		header[i] = ws[`${XLSX.utils.encode_col(i)}1`].v;
	}
	return header
};

function downloadFile()
{
   //Считываем данные
    var Result = GetObjFromStorage();
    if (Result == null) 
    {
        ShowError("Данные для сохранения не найдены");
        return;   
    }
    else
    {
        Result = ObjFromJson(Result);
    }
    var Data = new Array(Result.P.length+1);
    var size = Parameters.length+1;
    $.each(Data, function (index, value)
    {
        Data[index] = new Array(size);
    });


    Data[0][0] = Result.ParameterSelect[0];
    Data[0][1] = "P";
    var i = 2;
    $.each(Parameters, function (index, value)
    {
        if (Result.ParameterSelect[0] != value)
        {
            Data[0][i] = value;
            i++;
        }
        
    });

    $.each(Result, function (indexes, values)
    {
        var i = Data[0].indexOf(indexes);
        $.each(values, function (index, value)
        {
            Data[index + 1][i]=value;
        });
    });


    //Создаем файл
	var ws_name = "Calculations";
	var wb_new = XLSX.utils.book_new()
    var ws_new = XLSX.utils.aoa_to_sheet(Data);

	XLSX.utils.book_append_sheet(wb_new, ws_new, ws_name);
	XLSX.writeFile(wb_new, "Calculations.xlsx");
};