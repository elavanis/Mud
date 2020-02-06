$(function () {
    var SetGuid = function () {
        var guid = $("#guid");
        if (guid.val() === "a") {
            guid.val(GenerateGuid());
        }
    };

    var GenerateGuid = function () {
        var d = new Date().getTime();

        var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);

            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });

        return guid;
    };

    var BindSubmit = function () {
        $("#input").keypress(function (event) {
            var keycode = event.keyCode ? event.keyCode : event.which;
            if (keycode === 13) {
                SendCommand();
            }
        });
    };

    var SendCommand = function () {
        var command = $("#input").val();
        var guid = $("#guid").val();

        $.post('Home/SendCommand', { guid: guid, command: command })
            .done(function (resp) {
                ProcessResponse(resp);
            })
            .always(function () {
                $("#input").val("");
            });
    };

    var ProcessResponse = function (resp) {
        resp.each(obj, function (tagType, message) {
            alert(tagType + ": " + message);
        });
    };

    $(document).ready(function () {
        SetGuid();
        BindSubmit();
    });
});