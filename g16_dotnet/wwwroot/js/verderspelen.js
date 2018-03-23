var verderSpelenModule = (function () {
    var padId;

    function createInputAndButton() {
        $("#indexWindow").append(
            $("<input>").attr({
                class: "box-input",
                name: "code",
                id: "sessieCode",
                placeholder: "Sessie code"
            })
        ).append(
            $("<button>").attr({
                class: "box-button",
                type: "submit",
                style: "width: 100%"
            }).text("Bevestigen")
            );
    }

    function ajaxCall() {
        $.ajax({
            type: "GET",
            url: "Sessie/VerderSpelen",
            data: { padId: parseInt(padId) },
            success: function (partialView) {
                $("#indexWindow").empty();
                $("#indexWindow").html(partialView);
            }
        });
    }

    function init() {
        $(document).ajaxStart(function () {
            $("#ajaxLoading").removeAttr("hidden");
        });

        $(document).ajaxStop(function () {
            $("#ajaxLoading").attr("hidden");
        });

        padId = window.localStorage.getItem("padId");
        if (padId !== null) {
            ajaxCall();
        } else {
            createInputAndButton();
        }
    }

    return {
        init: init
    };
}());

$(document).ready(function () {
    verderSpelenModule.init();
});