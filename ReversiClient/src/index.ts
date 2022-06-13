import Game from "./Game";

async function main() {
    const token = decodeURIComponent(window.location.href.split("/").at(-1)!);
    const playerToken = document.querySelector<HTMLInputElement>("#playerToken")?.value!;
    console.log(playerToken);

    const game = new Game("https://localhost:6001/api/game", token, playerToken);

    await game.init();
}

document.addEventListener("DOMContentLoaded", () => {
    try {
        main();
    } catch (error) {
        console.error(error);
    }
});
