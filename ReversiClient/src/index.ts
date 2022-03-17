import FeedbackWidget from "./FeedbackWidget";
import Game from "./Game";
import Model from "./Game.Model";
import Reversi from "./Game.Reversi";
import { HubConnectionBuilder } from "@microsoft/signalr";

interface Move {
    row: number;
    column: number;
    color: "white" | "black";
}

const gridElement = document.querySelector<HTMLDivElement>(".grid")!;
const rows = gridElement.querySelectorAll<HTMLDivElement>(".grid__row")!;
const grid = new Map<string, Map<string, HTMLDivElement>>();

rows.forEach(row => {
    const map = new Map<string, HTMLDivElement>();

    const columns = row.querySelectorAll<HTMLDivElement>(".grid__column");
    columns.forEach(col => {
        map.set(col.dataset.column!, col);
    });

    grid.set(row.dataset.row!, map);
});

for (const [rowNum, row] of grid.entries()) {
    for (const [colNum, col] of row.entries()) {
        col.addEventListener("click", () => connection.invoke("Move", +rowNum, +colNum, "black"));
    }
}

const row = Array.from({ length: 8 }).map(() => 0);
const cols = Array.from({ length: 8 }).map(() => row.slice(0));

const board = cols as number[][];

const connection = new HubConnectionBuilder().withUrl("/hub").build();

type TBoard = Array<Array<0 | 1 | 2>>;

interface IGame {
    id: number;
    board: TBoard;
    description: string;
    token: string;
    player1Token: string;
    player2Token: string;
    turnColor: TBoard[0][0];
}

function showFiche(row: number, column: number, color: "white" | "black") {
    const fiche = document.createElement("span");
    fiche.classList.add(...["fiche", `fiche--${color}`]);
    grid.get(row.toString())?.get(column.toString())?.appendChild(fiche);
    const num = color === "white" ? 1 : 2;
    board[row - 1][column - 1] = num;
}

async function main() {
    await connection.start();

    const token = window.location.href.split("/").at(-1);

    connection.on("move", (row, col, color) => {
        showFiche(row, col, color);
        console.log(color);
        // Reversi.getInstance();
    });

    const response = await fetch(`https://localhost:6001/api/game/${token}`);
    const {} = (await response.json()) as IGame;

    // board.forEach((row, rowIndex) => {
    //     row.forEach((value, colIndex) => {
    //         Reversi.getInstance().showFiche(rowIndex, colIndex, value === 1 ? "white" : "black");
    //     });
    // });

    // connection.invoke("SendMessage", "Joel", "Hallo");

    // const widget = new FeedbackWidget("#widget");

    // Game.getInstance().init("/api/game", async api => {
    // Model.getInstance().getGameState("W1eAb01Hn0q4gQweGRFRng==").then(console.log);
    // const response = await fetch(`https://localhost:6001${api}/${token}`);
    // const { id } = (await response.json()) as IGame;
    // });

    // widget.history();
    // widget.danger("Lorem ipsum dolor sit amet consectetur adipisicing elit. Magnam, odio.");
    // widget.hide();

    // Game.init(() => {
    //     Game.Model.getGameState("W1eAb01Hn0q4gQweGRFRng==").then(console.log);
    // });

    // Game.Data.init({ environment: "development" });
}

document.addEventListener("DOMContentLoaded", () => {
    try {
        main();
    } catch (error) {
        console.error(error);
    }
});
