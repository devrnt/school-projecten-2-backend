var pollingModule = (function () {
    function init() {

    }

    function checkSessie() {
        console.log("Checking sessie...");
        $.ajax({
            type: "GET",
            url: "/Groep/IsSessieActief",
            data: "{}",
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
    }

    return {
        init: init,
        checkSessie: checkSessie
    };

}());
