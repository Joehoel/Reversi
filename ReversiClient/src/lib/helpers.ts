import { Color, TBoard, Turn } from "../types";

export function arraysEqual(a: unknown[][], b: unknown[][]) {
    if (a === b) return true;
    if (a == null || b == null) return false;
    if (a.length !== b.length) return false;

    for (var i = 0; i < a.length; ++i) {
        for (var j = 0; j < a[i].length; j++) {
            if (a[i][j] !== b[i][j]) return false;
        }
    }
    return true;
}

export function count(board: TBoard, item: Turn) {
    return board.reduce((acc, row) => {
        return acc + row.filter(col => col === item).length;
    }, 0);
}

export function score(board: TBoard): Record<Color, number> {
    const black = count(board, 2);
    const white = count(board, 1);

    return {
        black,
        white,
    };
}
