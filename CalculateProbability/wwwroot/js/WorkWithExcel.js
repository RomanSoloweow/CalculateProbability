function uploadFile() {
	document.getElementById("file").click();
};

$('#file').change(function (e) {
	var reader = new FileReader();
	var xlsHeader;
	var xlsRows;

	reader.readAsArrayBuffer(e.target.files[0]);
	reader.onload = function (e) {
		var data = new Uint8Array(reader.result);
		wb = XLSX.read(data, { type: 'array' });

		let first_sheet_name = wb.SheetNames[0];

		let worksheet = wb.Sheets[first_sheet_name];
		var my_data = XLSX.utils.sheet_to_json(worksheet, { raw: true })

		xlsHeader = extractHeader(worksheet);
		xlsRows = my_data;

		var resultArr = [];
		resultArr.push(xlsHeader);
		$.each(xlsRows, function (index, value) {
			var tempArr = [];
			$.each(value, function (ind, val) {
				tempArr.push(Number(val));
			});
			resultArr.push(tempArr);
		});
		showChart2(resultArr);
		console.log(resultArr);
		sessionStorage.data = JSON.stringify(resultArr);
	}
});

function extractHeader(ws) {
	const header = []
	const columnCount = XLSX.utils.decode_range(ws['!ref']).e.c + 1
	for (let i = 0; i < columnCount; ++i) {
		header[i] = ws[`${XLSX.utils.encode_col(i)}1`].v;
	}
	return header
};

function downloadFile() {
	var data = JSON.parse(sessionStorage.data);
	var xlsHeader = data[0];
	data.shift();
	var xlsRows = data;
	var createXLSLFormatObj = [];
	createXLSLFormatObj.push(xlsHeader);
	$.each(xlsRows, function (index, value) {
		var innerRowData = [];
		$.each(value, function (ind, val) {
			innerRowData.push(val);
		});
		createXLSLFormatObj.push(innerRowData);
	});
	var ws_name = "Calculations";
	var wb_new = XLSX.utils.book_new()
	var ws_new = XLSX.utils.aoa_to_sheet(createXLSLFormatObj);

	XLSX.utils.book_append_sheet(wb_new, ws_new, ws_name);
	XLSX.writeFile(wb_new, "Calculations.xlsx");
};