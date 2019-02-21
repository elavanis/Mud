function getValues(url) {
    var result = [];
    $.ajax({
        url: url,
        type: 'get',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            result = data;
        }
    });
    return result;
}