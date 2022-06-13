import { Chart } from "chart.js";
import { TBoard } from "./types";
import "chart.js/auto";

export default class Stats {
    private ctx: CanvasRenderingContext2D;
    private chart: Chart<"doughnut", number[], string>;

    constructor(selector: string) {
        this.ctx = document.querySelector<HTMLCanvasElement>(selector)?.getContext("2d")!;
        this.chart = new Chart(this.ctx, {
            type: "doughnut",
            data: {
                labels: ["Black", "White"],
                datasets: [
                    {
                        data: [1, 1],
                        backgroundColor: ["#2A2D34", "#EFE9F4"],
                        borderColor: ["#191b1f", "#b4adba"],
                    },
                ],
            },
        });
    }

    public update(data: TBoard) {
        let white = 0;
        let black = 0;
        let i = 0;
        let j = 0;
        data.forEach(row => {
            j = 0;
            row.forEach(square => {
                switch (square) {
                    case 1:
                        white++;
                        break;
                    case 2:
                        black++;
                        break;
                }
                j++;
            });
            i++;
        });

        const dataset = this.chart.data.datasets[0];

        this.chart.data.datasets[0] = {
            ...dataset,
            data: [black, white],
        };
        this.chart.update();
    }
}
