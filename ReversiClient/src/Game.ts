import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { score } from "./lib/helpers";

import type { IGame, TBoard, Move, Colors } from "./types";

export default class Game {
    private url: string;
    private token: string;
    private playerToken: string;
    private grid = new Map<string, Map<string, HTMLDivElement>>();
    private board?: TBoard = [[]];
    private connection: HubConnection;

    public constructor(url: string, token: string, playerToken: string) {
        this.url = url;
        this.token = token;
        this.playerToken = playerToken;

        this.connection = new HubConnectionBuilder().withUrl("/hub").build();

        this.init();

        const gridElement = document.querySelector<HTMLDivElement>(".grid")!;
        const rows = gridElement.querySelectorAll<HTMLDivElement>(".grid__row")!;

        rows.forEach(row => {
            const map = new Map<string, HTMLDivElement>();

            const columns = row.querySelectorAll<HTMLDivElement>(".grid__column");
            columns.forEach(col => {
                map.set(col.dataset.column!, col);
            });

            this.grid.set(row.dataset.row!, map);
        });

        for (const [rowNum, row] of this.grid.entries()) {
            for (const [colNum, col] of row.entries()) {
                // TODO: Make this work
                col.addEventListener("click", () => {
                    this.move({ column: parseInt(colNum!), row: parseInt(rowNum!) });
                });
            }
        }
    }

    async init() {
        try {
            await this.connection.start();
            await this.updateBoard();

            this.connection.invoke("Join", this.token);
            this.connection.on("message", message => console.log(`Message: ${message}`));
            this.connection.on("move", async () => {
                await this.updateBoard();
            });
        } catch (error) {
            console.error(error);
        }
    }

    async updateBoard() {
        console.log("Updating");
        const response = await fetch(`${this.url}/${this.token}`);

        if (response.status === 502) {
            await this.updateBoard();
        } else if (response.status != 200) {
            console.error(`Something went wrong: ${response.statusText}`);
            await new Promise(resolve => setTimeout(resolve, 10000));
            await this.updateBoard();
        } else {
            const game = (await response.json()) as IGame;

            if (game.winner !== 0) {
                // window.location.href = `${
                //     window.location.origin
                // }/Games/Results/${encodeURIComponent(this.token)}`;
                // return;
            }

            this.board = game.board;

            this.board.forEach((row, rowIndex) => {
                row.forEach((value, colIndex) => {
                    this.showFiche({
                        color: value,
                        column: colIndex,
                        row: rowIndex,
                    });
                });
            });
        }
    }

    public async move({ column, row }: Move) {
        const move: Move = {
            playerToken: this.playerToken,
            token: this.token,
            row,
            column,
        };

        const response = await fetch(`${this.url}/move`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(move),
        });

        switch (response.status) {
            case 401:
                alert("Niet jou beurt");
                break;

            case 400:
                alert("Niet mogelijk");
            case 200:
                this.connection.invoke("Move", this.token);
            default:
                break;
        }
    }

    public showFiche({ color, column, row }: Move) {
        if (color === 0 || typeof color === "undefined") {
            return;
        }

        const colors: Colors = {
            "1": "white",
            "2": "black",
        };

        const fiche = document.createElement("span");
        fiche.classList.add(...["fiche", `fiche--${colors[color]}`]);
        const parent = this.grid.get(row.toString())?.get(column.toString());

        if (parent) {
            parent.childNodes.forEach(node => node.remove());
            parent.appendChild(fiche);
        }

        if (this.board) {
            this.board[row][column] = color;
        }
    }
}
