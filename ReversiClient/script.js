const widget = new FeedbackWidget("#widget");

Game.init(() => {
    Game.Model.getGameState("W1eAb01Hn0q4gQweGRFRng==").then(console.log);
});

Game.Data.init({ environment: "development" });