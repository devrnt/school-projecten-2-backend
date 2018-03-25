var checkSessieModule = (function () {
    var sessieId;

    function init() {
        var checkSessie = function () {
            console.log("Checking sessie...");
            sessieId = window.localStorage.getItem("sessieId");
            $.ajax({
                type: "GET",
                url: "/Sessie/IsSessieActief",
                data: { "sessieId": parseInt(sessieId) },
                contentType: "application/json;",
                dataType: "json",
                success: function (response) {
                    if (response.isActief) {
                        $("#startspel").removeClass("not-active");
                    } else {
                        setTimeout(checkSessie, 3000);
                    }
                }
            });
        };

        checkSessie();
    }

    return {
        init: init
    };

}());

$(document).ready(function () {
    checkSessieModule.init();
});