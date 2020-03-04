$(function () {

    setInterval(function () {
        SendBlankCommand();

        SetSize();

    }, 500);

    var SendBlankCommand = function () {
        var guid = $("#guid").val();
        $.post('Home/SendCommand', { guid: guid, command: "" })
            .done(function (resp) {
                ProcessResponse(resp);
            });
    };

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
        var idElement = $("#idPos");

        var idPos = parseInt(idElement.val());

        $.each(resp, function (index, value) {
            if (value["item1"] === "Map") {
                SetPosition(value["item2"]);
            }
            else {
                idPos += 1;
                var element = document.createElement("pre");
                element.setAttribute("id", idPos);
                element.setAttribute("class", value["item1"]);
                element.innerHTML = value["item2"];

                if (value["item1"] === "Health"
                    || value["item1"] === "Mana"
                    || value["item1"] === "Stamina") {
                    element.setAttribute("class", value["item1"] + " " + "Left");

                    SetStatus(value["item1"], value["item2"]);
                }
                else {
                    element.setAttribute("class", value["item1"] + " " + "Linebreak");
                }

                document.getElementById("display").appendChild(element);

                if (value["item2"].endsWith("\r\n")) {
                    idPos += 1;
                    element = document.createElement("pre");
                    element.setAttribute("class", "lineBreak");
                    element.setAttribute("id", idPos);
                    document.getElementById("display").appendChild(element);
                }
            }

        });

        $("#" + idPos).scrollTop($("#" + idPos)[0].scrollIntoView);

        idElement.val(idPos);
    };

    var SetStatus = function (status, value) {
        var element = $("#" + status + "Status");
        element.html(value);
    }

    var SetPosition = function (positionString) {
        var splitString = positionString.split("|");
        var zone = splitString[0];
        var level = splitString[1];
        var x = splitString[2];
        var y = splitString[3];

        var originalWidth = $('#mapOverlay')[0].naturalWidth;
        var originalHeight = $('#mapOverlay')[0].naturalHeight;
        var realSize = $('#mapOverlay').width();

        var sizeDiff = realSize / originalWidth;

        var centerWidth = (originalWidth - 10) / 2;
        var offset = (x - centerWidth) * sizeDiff;
        $("#pos").css("margin-left", offset + "px");

        var centerHeight = (originalHeight - 10) / 2;
        offset = (y - centerHeight) * sizeDiff * -1;
        $("#pos").css("margin-top", offset + "px");
    };

    var SetSize = function () {
        var originalSize = $('#mapOverlay')[0].naturalWidth;
        var realSize = $('#mapOverlay').width();

        var sizeDiff = realSize / originalSize;

        $("#pos").width(sizeDiff * 10);
        $("#pos").height(sizeDiff * 10);
    }

    $(document).ready(function () {
        SetGuid();
        BindSubmit();
    });
});