import Data from "./Game.Data";

export default class Model {
    private static instance: Model;

    private config = {};

    private constructor() {
        console.log("'Game.Model' loaded!");
    }

    public static getInstance(): Model {
        if (!Model.instance) {
            Model.instance = new Model();
        }

        return Model.instance;
    }

    public async getGameState(token: string) {
        Data.getInstance().init({ environment: "production" });
        // const res = Data.getInstance().get();
        const res = 1;

        if (![0, 1, 2].includes(res)) {
            throw new Error(`${res} is niet gelijk aan 1, 2 of 3`);
        }

        return res;
    }
}

// Game.Model = (function () {
//     console.log("'Game.Model' loaded!");

//     const config = {};

//     const getGameState = async token => {
//         const res = await Game.Data.get(`api/spel/beurt/${token}`);

//         if (![0, 1, 2].includes(res)) {
//             throw new Error(`${res} is niet gelijk aan 1, 2 of 3`);
//         }

//         return res;
//     };

//     const privateInit = function () {
//         console.log("private init");
//     };

//     return {
//         init: privateInit,
//         getGameState,
//     };
// })();
