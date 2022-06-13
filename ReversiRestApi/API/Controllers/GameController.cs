using System;
using System.Collections.Generic;
using System.Linq;
using API.Model;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _repository;

        public GameController(IGameRepository repository)
        {
            _repository = repository;
        }


        // GET /api/game
        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetWaitingGames()
        {
            return _repository.GetGames().Where(spel => string.IsNullOrEmpty(spel.Player2Token)).ToList();

        }

        // GET /api/game/{token}
        [HttpGet("{token}")]
        public ActionResult<Game> GetGameByToken(string token)
        {
            var game = _repository.GetGame(token);
            if (game == null) return NotFound();

            return Ok(game);
        }

        // GET /api/game/player/{playerToken}
        [HttpGet("player/{playerToken}")]
        public ActionResult<Game> GameGameByPlayerToken(string playerToken)
        {
            var game = _repository.GetGames().Where(game => game.Player1Token == playerToken || game.Player2Token == playerToken).FirstOrDefault();
            if (game == null) return NotFound();
            return Ok(game);
        }

        public class GameInfo
        {
            public string Description { get; set; }
            public string Player1Token { get; set; }
        }

        // POST /api/game
        [HttpPost]
        public ActionResult<GameInfo> AddGame([FromBody] GameInfo gameInfo)
        {
            var game = new Game();

            game.Token = Guid.NewGuid().ToString();
            game.Player1Token = gameInfo.Player1Token;
            game.Description = gameInfo.Description;

            _repository.AddGame(game);

            return Created(nameof(AddGame), game);
        }

        // DELETE /api/game/{token}
        [HttpDelete("{token}")]
        public ActionResult DeleteGame(string token)
        {
            var game = _repository.DeleteGame(token);
            if (!game) return NotFound();
            return Ok();
        }



        // GET /api/game/turn/{token}
        [HttpGet("turn/{token}")]
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

        public class JoinData
        {
            public string Player2Token { get; set; }
        }


        // PUT /api/game/join
        [HttpPut("join/{token}")]
        public ActionResult<Game> Join(string token, [FromBody] JoinData data)
        {
            var game = _repository.GetGame(token);

            if (game == null) return NotFound();

            if (data.Player2Token == null)
            {
                return BadRequest($"{nameof(data.Player2Token)} is not defined.");
            }
            else if (game.Player2Token != null)
            {
                return BadRequest("Game is full.");
            }
            else if (token == null)
            {
                return BadRequest($"{nameof(token)} is not defined.");
            }

            game.Player2Token = data.Player2Token;
            game.TurnColor = Color.Black;

            _repository.UpdateGame(game);
            return Ok(game);
        }
        public class LeaveData
        {
            public string Player1Token { get; set; }
            public string Player2Token { get; set; }
        }

        // PUT /api/game/leave
        [HttpPut("leave/{token}")]
        public ActionResult<Game> Leave(string token, [FromBody] LeaveData data)
        {
            var game = _repository.GetGame(token);

            if (game == null) return NotFound();

            if (data.Player2Token == null)
            {
                return BadRequest($"{nameof(data.Player2Token)} is not defined.");
            }
            else if (game.isFull())
            {
                return BadRequest("Game is full.");
            }
            else if (token == null)
            {
                return BadRequest($"{nameof(token)} is not defined.");
            }

            if (data.Player1Token == game.Player1Token) game.Player1Token = null;
            if (data.Player2Token == game.Player2Token) game.Player2Token = null;

            if (game.Player1Token != null && game.Player2Token != null)
            {
                _repository.DeleteGame(game.Token);
            }
            else
            {
                _repository.UpdateGame(game);
            }

            return Ok(game);
        }

        // PUT /api/game/move
        [HttpPut("move")]
        public ActionResult<Game> Move([FromBody] MoveData data)
        {
            var game = _repository.GetGame(data.Token);
            if (game == null) return NotFound();

            if (!game.HasPlayer(data.PlayerToken)) return BadRequest("Invalid player");
            //if (game.HasEnded()) return BadRequest("Game over");

            // Check if its the players turn
            if ((game.TurnColor == Color.White ? game.Player1Token : game.Player2Token) != data.PlayerToken)
            {
                return Unauthorized("Niet jouw beurt");
            }

            try
            {
                game.Move(data.Row, data.Column);

                if (game.HasEnded())
                {
                    game.Winner = game.DominantColor();
                }

                _repository.UpdateGame(game);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(game);
        }


        // PUT /api/game/concede
        [HttpPut("concede")]
        public ActionResult<Game> Concede(string token, [FromBody] GameData data)
        {
            var game = _repository.GetGame(token);
            if (game == null) return NotFound();

            if ((game.TurnColor == Color.White ? game.Player1Token : game.Player2Token) != data.PlayerToken)
            {
                return Unauthorized("Niet jouw beurt");
            }

            game.Winner = Game.GetOpponentColor(game.TurnColor);
            _repository.UpdateGame(game);
            return Ok(game);
        }
    }

}
