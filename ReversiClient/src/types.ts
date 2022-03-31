export type Turn = 0 | 1 | 2;

export type TBoard = Array<Array<Turn>>;

export type Color = "white" | "black";

export type Colors = Omit<Record<Turn, Color>, 0>;

export interface IGame {
    id: number;
    board: TBoard;
    description: string;
    token: string;
    player1Token: string;
    player2Token: string;
    turnColor: Turn;
    winner: Turn;
}

export interface Move {
    playerToken?: string;
    token?: string;
    row: number;
    column: number;
    color?: Turn;
}
