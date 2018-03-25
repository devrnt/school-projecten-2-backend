var checkDeelnamesModule = (function () {
    var sessieId;

    function init() {
        var checkDeelnames = function () {
            console.log("Checking deelnames...");
            sessieId = window.localStorage.getItem("sessieId");
            $.ajax({
                type: "GET",
                url: "/Sessie/CheckDeelnames",
                data: { "sessieId": parseInt(sessieId) },
                success: function (partialView) {
                    $("#groepenOverzicht").html(partialView);
                    setTimeout(checkDeelnames, 3000);
                }
            });
        };
        checkDeelnames();
    }

    return {
        init: init
    };

}());

$(document).ready(function () {
    checkDeelnamesModule.init();
});