export default class Reversi {
    private static instance: Reversi;

    private config = {};
    private grid = new Map<string, Map<string, HTMLDivElement>>();
    // private board: number[][];

    private constructor() {
        console.log("'Game.Reversi' loaded!");
    }

    public static getInstance(): Reversi {
        if (!Reversi.instance) {
            Reversi.instance = new Reversi();
        }

        return Reversi.instance;
    }

    public showFiche(row: number, column: number, color: "white" | "black") {
        const fiche = document.createElement("span");
        fiche.classList.add(...["fiche", `fiche--${color}`]);
        this.grid.get(row.toString())?.get(column.toString())?.appendChild(fiche);
        const num = color === "white" ? 1 : 2;
        this.board[row - 1][column - 1] = num;
    }
}
