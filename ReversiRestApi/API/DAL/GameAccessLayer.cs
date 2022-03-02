using System.Collections.Generic;
using API.Model;
using API.Repositories;
using System.Linq;
namespace API.DAL
{
    public class GameAccessLayer : IGameRepository
    {

        private DatabaseContext _context;

        public GameAccessLayer(DatabaseContext context) { _context = context; }


        public void AddGame(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public Game GetGame(string gameToken)
        {
            return _context.Games.First(game => game.Token == gameToken);
        }

        public List<Game> GetGames()
        {
            return _context.Games.ToList();
        }
    }
}
