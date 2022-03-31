import FeedbackWidget from "./FeedbackWidget";
import Game from "./Game";
import Model from "./Game.Model";
import Reversi from "./Game.Reversi";

function handleClick(e: Event) {}

async function main() {
    const token = decodeURIComponent(window.location.href.split("/").at(-1)!);
    const playerToken = document.querySelector<HTMLInputElement>("#playerToken")?.value!;
    console.log(playerToken);

    const game = new Game("https://localhost:6001/api/game", token, playerToken);
}

document.addEventListener("DOMContentLoaded", () => {
    try {
        main();
    } catch (error) {
        console.error(error);
    }
});
