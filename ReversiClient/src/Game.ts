import Model from "./Game.Model";

interface State {
    gameState?: number;
}

interface Config {
    api?: string;
}

export default class Game {
    private static instance: Game;

    private config: Config = {};
    private state: State = {};

    public static getInstance(): Game {
        if (!Game.instance) {
            Game.instance = new Game();
        }

        return Game.instance;
    }

    public init(url: string, callback: (api: string) => void) {
        this.config.api = url;

        setInterval(() => callback(this.config.api ?? ""), 2000);

        // callback();
    }

    private async getCurrentGameState() {
        this.state.gameState = await Model.getInstance().getGameState("W1eAb01Hn0q4gQweGRFRng==");
        console.log(this.state.gameState);
    }
}
