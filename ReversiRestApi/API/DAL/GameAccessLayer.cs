using System.Collections.Generic;
using System.Linq;
using API.Model;
using API.Repositories;

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

        public bool DeleteGame(string gameToken)
        {
            var game = GetGame(gameToken);

            if (game != null)
            {
                _context.Games.Remove(game);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public Game GetGame(string gameToken)
        {
            return _context.Games.FirstOrDefault(game => game.Token == gameToken);
        }

        public List<Game> GetGames()
        {
            return _context.Games.ToList();
        }

        public void UpdateGame(Game game)
        {
            _context.Games.Update(game);
            _context.SaveChanges();
        }
    }
}
