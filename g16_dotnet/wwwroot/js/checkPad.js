var checkPadModule = (function () {
    var padId;

    function init() {
        padId = window.localStorage.getItem("padId");
        var checkPad = function () {
            console.log("Checking pad...");
            $.ajax({
                type: "GET",
                url: "/Spel/CheckPad",
                data: { "padId": parseInt(padId) },
                contentType: "application/json;",
                dataType: "json",
                success: function (response) {
                    if (!response.isGeblokkeerd && !response.isVergrendeld) {
                        $("#verdergaan").removeClass("not-active");
                    } else {
                        setTimeout(checkPad, 3000);
                    }
                }
            });
        }

        checkPad();
    }

    return {
        init: init
    }
}());

$(document).ready(function () {
    checkPadModule.init();
});