using System.Collections.Generic;
using API.Model;

namespace API.Repositories
{
    public class GameRepository : IGameRepository
    {
        // Lijst met tijdelijke spellen
        public List<Game> Games { get; set; }

        public GameRepository()
        {
            Game game1 = new Game();
            Game game2 = new Game();
            Game game3 = new Game();

            game1.Player1Token = "abcdef";
            game1.Description = "Potje snel reversi, dus niet lang nadenken";
            game2.Player1Token = "ghijkl";
            game2.Player2Token = "mnopqr";
            game2.Description = "Ik zoek een gevorderde tegenspeler!";
            game3.Player1Token = "stuvwx";
            game3.Description = "Na dit spel wil ik er nog een paar spelen tegen zelfde tegenstander";


            Games = new List<Game> { game1, game2, game3 };
        }

        public void AddGame(Game spel)
        {
            Games.Add(spel);
        }

        public List<Game> GetGames()
        {
            return Games;
        }

        public Game GetGame(string gameToken)
        {
            return Games.Find(game => game.Token == gameToken);
        }

        public bool DeleteGame(string gameToken)
        {
            var game = GetGame(gameToken);
            if (game != null)
            {
                Games.Remove(game);
                return true;
            }
            return false;
        }

        public void UpdateGame(Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}
