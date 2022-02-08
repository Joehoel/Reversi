Game.Data = (function () {
    console.log("'Game.Data' loaded!");

    const config = {};

    const privateInit = function () {
        console.log("private init");
    };

    return {
        init: privateInit,
    };
})();
