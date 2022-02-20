using System;
using System.Collections.Generic;
using System.Linq;
using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/game")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly IGameRepository _repository;

        public SpelController(IGameRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        {
            return _repository.GetGames().Where(spel => string.IsNullOrEmpty(spel.Player2Token)).ToList();

        }

        [HttpGet("{token}")]
        public ActionResult<Game> GetSpelByToken(string token)
        {
            var spel = _repository.GetGame(token);
            if (spel == null) return NotFound();
            return Ok(spel);
        }


        [HttpGet("player/{playerToken}")]
        public ActionResult<Game> GetSpelBySpelerToken(string playerToken)
        {
            var game = _repository.GetGames().Where(game => game.Player1Token == playerToken || game.Player2Token == playerToken);
            if (game == null) return NotFound();
            return Ok(game);
        }
        // [HttpGet("{spelerToken}")]
        // public ActionResult<Spel> GetSpelBySpelerToken(string spelerToken)
        // {
        //     var spel = _repository.GetSpelFromSpeler(spelerToken);
        //     if (spel == null) return NotFound();
        //     return Ok(spel);
        // }
        public class GameInfo
        {
            public string Description { get; set; }
            public string Token { get; set; }
        }
        [HttpPost]
        public ActionResult<GameInfo> AddGame([FromBody] GameInfo gameInfo)
        {
            var game = new Game();

            game.Token = Guid.NewGuid().ToString();
            game.Player1Token = gameInfo.Token;
            game.Description = gameInfo.Description;

            _repository.AddGame(game);

            return Created(nameof(AddGame), game);
        }



        [HttpGet("beurt/{token}")]
        public ActionResult<int> GetTurnColor(string token)
        {
            var game = _repository.GetGame(token);
            if (game == null) return NotFound();

            return Ok(game.TurnColor);
        }

        public class GameData
        {
            public string Token { get; set; }

            public string PlayerToken { get; set; }
        }

        public class MoveData : GameData
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public bool Pass { get; set; } = false;


        }

        [HttpPut("move")]
        public ActionResult<Game> Move([FromBody] MoveData data)
        {
            var game = _repository.GetGame(data.Token);
            if (game == null) return NotFound();

            // Check if its the players turn
            if ((game.TurnColor == Color.White ? game.Player1Token : game.Player2Token) != data.PlayerToken)
            {
                return Unauthorized("Niet jouw beurt");
            }

            // Check if the turn is a valid move
            if (!game.TurnPossible(data.Row, data.Column))
            {
                return BadRequest("Niet mogelijk");
            }

            if (data.Pass)
            {
                game.Pass();
                return Ok(game);
            }

            game.Move(data.Row, data.Column);

            return Ok(game);
        }


        [HttpPut("concede")]
        public ActionResult<Game> Concede([FromBody] GameData data)
        {
            var game = _repository.GetGame(data.Token);
            if (game == null) return NotFound();

            if ((game.TurnColor == Color.White ? game.Player1Token : game.Player2Token) != data.PlayerToken)
            {
                return Unauthorized("Niet jouw beurt");
            }

            // TODO: Opgeven
            return null;
        }
    }

}
