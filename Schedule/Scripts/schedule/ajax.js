function post(url, data, successCallBack, errorCallBack) {

    var json = JSON.stringify(data);
    console.log(json);
    $.ajax({
        type: "POST",
        url: url,
        //contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        contentType: 'application/json; charset=utf-8',
        data: json,
        dataType: "JSON",
        success: function (content) {
            successCallBack(content);
        },
        error: function (content) {
            errorCallBack(content);
        }
    });
}

function deleteVerb(url, successCallBack, errorCallBack) {
    $.ajax({
        type: "DELETE",
        url: url,
        success: function (content) {
            successCallBack(content);
        },
        error: function (content) {
            errorCallBack(content);
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