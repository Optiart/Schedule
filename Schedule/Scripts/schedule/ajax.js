function post(url, data) {
	$.ajax({
		type: "POST",
		url: url,
        //contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        contentType: 'application/json; charset=utf-8',
        data: data,
        dataType: "JSON",
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