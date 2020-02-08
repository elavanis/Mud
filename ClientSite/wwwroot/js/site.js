﻿$(function () {

    setInterval(function () {
        var guid = $("#guid").val();
        $.post('Home/SendCommand', { guid: guid, command: "" })
            .done(function (resp) {
                ProcessResponse(resp);
            });
    }, 500);

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
            idPos += 1;
            var element = document.createElement("pre");
            element.setAttribute("id", idPos);
            element.setAttribute("class", value["item1"]);
            element.innerHTML = value["item2"] + " ";
            document.getElementById("display").appendChild(element);

        });

        $("#" + idPos).scrollTop($("#" + idPos)[0].scrollIntoView);

        idElement.val(idPos);
    };

    $(document).ready(function () {
        SetGuid();
        BindSubmit();
    });
});