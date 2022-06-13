import { HubConnection, HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { score } from "./lib/helpers";
import Stats from "./Stats";

import type { IGame, TBoard, Move, Colors } from "./types";

export default class Game {
    private url: string;
    private token: string;
    private playerToken: string;
    private grid = new Map<string, Map<string, HTMLDivElement>>();
    public board: TBoard = [[]];
    private connection: HubConnection;
    private stats: Stats;

    public constructor(url: string, token: string, playerToken: string) {
        this.url = url;
        this.token = token;
        this.playerToken = playerToken;

        this.connection = new HubConnectionBuilder().withUrl("/hub").build();

        this.stats = new Stats("#stats");

        const gridElement = document.querySelector<HTMLDivElement>(".grid")!;
        const rows = gridElement.querySelectorAll<HTMLDivElement>(".grid__row")!;
        const concedeButton = document.querySelector("#concede");

        concedeButton?.addEventListener("click", () => this.concede());

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

    private async concede() {
        const response = await fetch(`${this.url}/concede`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ token: this.token, playerToken: this.playerToken }),
        });
        if (!response.ok) {
            const message = await response.text();

            alert(message);
        } else {
            this.redirect(`/Games/Results/${this.token}`);
        }
    }

    async init() {
        try {
            await this.connection.start();
            await this.updateBoard();

            if (this.connection.state === HubConnectionState.Connected) {
                console.log("Connected");

                this.connection.invoke("Join", this.token);
                this.connection.on("message", message => console.log(`Message: ${message}`));
                this.connection.on("move", async () => {
                    await this.updateBoard();
                });
            }
        } catch (error) {
            console.error(error);
        }
    }

    private redirect(to: string) {
        window.location.href = encodeURI(`${window.location.origin}${to}`);
    }

    async updateBoard() {
        console.log("Updating");
        const response = await fetch(`${this.url}/${this.token}`);
        const game = (await response.json()) as IGame;

        console.log(game);

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

        this.stats.update(this.board);
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

        if (response.ok) {
            this.connection.invoke("Move", this.token);
        } else {
            const message = await response.text();

            // TODO: Convert to Enum
            if (message.toLowerCase() === "game over") {
                this.redirect(`/Games/Results/${this.token}`);
            } else {
                alert(message);
            }
        }

        // switch (request.status) {
        //     case 401:
        //         alert("Niet jou beurt");
        //         break;

        //     case 400:
        //         alert("Niet mogelijk");
        //     case 200:
        //         this.connection.invoke("Move", this.token);
        //     default:
        //         break;
        // }
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
