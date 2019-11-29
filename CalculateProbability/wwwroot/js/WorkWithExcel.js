

function uploadFile()
{
	document.getElementById("file").click();
};

$('#file').change(function (e) {
	var reader = new FileReader();
    var Names = [];
    var P = [];
    var ParameterValues = [];
    var file = e.target.files[0];
	reader.readAsArrayBuffer(file);
    reader.onload = function (e)
    {
		var data = new Uint8Array(reader.result);
		wb = XLSX.read(data, { type: 'array' });
		let first_sheet_name = wb.SheetNames[0];
		let worksheet = wb.Sheets[first_sheet_name];
		var my_data = XLSX.utils.sheet_to_json(worksheet, { raw: true })

        Names = extractHeader(worksheet);
        $.each(my_data, function (index, value) 
        {
            P.push(value[Names[1]]);
            ParameterValues.push(value[Names[0]]);
        });
        sessionStorage.Names = JSON.stringify(Names);
        sessionStorage.P = JSON.stringify(P);
        sessionStorage.ParameterValues = JSON.stringify(ParameterValues);
        showChart2(Names, ParameterValues,P);
     
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
    var Names = sessionStorage.getItem('Names');
    if (Names) 
    {
        var P = sessionStorage.getItem('P');
        var ParameterValues = sessionStorage.getItem('ParameterValues');

        Names = JSON.parse(Names);
        P = JSON.parse(P);
        ParameterValues = JSON.parse(ParameterValues);
    }
    else
    {
        ShowError("Данные для сохранения не найдены");
        return;
    }

    //Формируем данные
    Data = [];
    Data.push(Names);
    $.each(ParameterValues, function (index, item)
    { Data.push([ParameterValues[index], P[index]]); });

    //Создаем файл
	var ws_name = "Calculations";
	var wb_new = XLSX.utils.book_new()
    var ws_new = XLSX.utils.aoa_to_sheet(Data);

	XLSX.utils.book_append_sheet(wb_new, ws_new, ws_name);
	XLSX.writeFile(wb_new, "Calculations.xlsx");
};