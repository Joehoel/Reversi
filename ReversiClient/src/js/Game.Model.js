Game.Model = (function () {
    console.log("'Game.Model' loaded!");

    const config = {};

    const getGameState = async token => {
        const res = await Game.Data.get(`api/spel/beurt/${token}`);

        if (![0, 1, 2].includes(res)) {
            throw new Error(`${res} is niet gelijk aan 1, 2 of 3`);
        }

        return res;
    };

    const privateInit = function () {
        console.log("private init");
    };

    return {
        init: privateInit,
        getGameState,
    };
})();