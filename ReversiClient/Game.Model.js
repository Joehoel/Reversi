Game.Model = (function () {
    console.log("'Game.Model' loaded!");

    const config = {};

    const privateInit = function () {
        console.log("private init");
    };

    return {
        init: privateInit,
    };
})();
