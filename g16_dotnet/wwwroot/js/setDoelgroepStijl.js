var setDoelgroepStijlModule = (function () {
    function init() {
        var doelgroep = window.localStorage.getItem("doelgroep");
        var sheet = "main.css";
        if (doelgroep !== null)
            sheet = doelgroep === "0" ? "main.css" : "main2.css";
        $("#doelgroepStijl").attr({href : `/css/${sheet}`})
    }

    return {
        init: init
    };
}())

$(document).ready(function () {
    setDoelgroepStijlModule.init();
});