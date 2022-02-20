using System.Collections.Generic;
using API.Model;

public interface IGameRepository
{
    void AddGame(Game game);

    List<Game> GetGames();

    Game GetGame(string gameToken);
}
