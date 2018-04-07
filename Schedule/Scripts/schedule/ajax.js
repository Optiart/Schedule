function post(url, data) {

	var jsonData = JSON.stringify(data);

	$.ajax({
		type: "POST",
		url: url,
		contentType: "application/json; charset=utf-8",
		data: jsonData,
		dataType: "json",
		success: function (content) {
			console.log(content);
		}
	});
}

function get(url, toElementById) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (content) {
            $("#" + toElementById).html(content);
        }
    });
}