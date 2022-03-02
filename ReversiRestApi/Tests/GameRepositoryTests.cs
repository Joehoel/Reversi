using System.Collections.Generic;
using API.Model;
using API.Repositories;
using NUnit.Framework;


[TestFixture]
public class GameRepositoryTest
{
    [Test]
    public void AddSpel_Spel_SpelAdded()
    {
        Game game = new Game();
        GameRepository gameRepository = new GameRepository();
        gameRepository.AddGame(game);
        Assert.Contains(game, gameRepository.GetGames());
    }

    [Test]
    public void GetSpellen_Spellen_List()
    {
        Game game0 = new Game();
        Game game1 = new Game();
        Game game3 = new Game();
        GameRepository gameRepository = new GameRepository();
        gameRepository.AddGame(game0);
        gameRepository.AddGame(game1);
        gameRepository.AddGame(game3);
        Assert.Contains(game0, gameRepository.GetGames());
        Assert.Contains(game1, gameRepository.GetGames());
        Assert.Contains(game3, gameRepository.GetGames());
    }

    [Test]
    public void GetSpellen_noSpellen_EmptyList()
    {
        GameRepository gameRepository = new GameRepository();
        Assert.AreEqual(new List<Game>(), gameRepository.GetGames());
    }

    [Test]
    public void GetSpel_Spel_Spel()
    {
        Game game = new Game();
        GameRepository gameRepository = new GameRepository();
        gameRepository.AddGame(game);
        Assert.AreEqual(game, gameRepository.GetGame(game.Token));
    }

    [Test]
    public void GetSpel_EmptyString_Null()
    {
        GameRepository gameRepository = new GameRepository();
        Assert.AreEqual(null, gameRepository.GetGame(""));
    }
}
