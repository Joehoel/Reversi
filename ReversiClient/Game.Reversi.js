Game.Reversi = (function () {
    console.log("'Game.Reversi' loaded!");

    const config = {};

    const privateInit = function () {
        console.log("private init");
    };

    return {
        init: privateInit,
    };
})();
