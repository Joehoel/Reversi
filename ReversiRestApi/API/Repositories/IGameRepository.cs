using System.Collections.Generic;
using API.Model;

namespace API.Repositories
{
    public interface IGameRepository
    {
        void AddGame(Game game);

        public List<Game> GetGames();

        Game GetGame(string gameToken);
    }
}
