export default class Game {
    private config: Record<string, any> = {};
    private state: Record<string, any> = { gameState: undefined };

    constructor(url: string, callback: () => void) {
        this.config.api = url;
        callback();
    }
}

const OldGame = (function (url) {
    const config = {
        api: url,
    };

    const state = {
        gameState: undefined,
    };

    /**
     * Callback that gets executed after game initialization
     * @param {() => void} afterInit
     */
    const privateInit = function (afterInit) {
        setInterval(getCurrentGameState, 2000);

        afterInit();
    };

    const getCurrentGameState = async () => {
        // TODO: Wat moet hier?
        state.gameState = await Game.Model.getGameState("W1eAb01Hn0q4gQweGRFRng==");
        console.log(state.gameState);
    };

    return {
        init: privateInit,
    };
})("/api/cute-cats");
