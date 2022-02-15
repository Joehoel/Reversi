using System.Diagnostics;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("api/spel")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly ISpelRepository _repository;

        public SpelController(ISpelRepository repository)
        {
            _repository = repository;
        }


        // GET api/spel
        [HttpGet]
        public ActionResult<IEnumerable<Spel>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        {
            return _repository.GetSpellen().Where(spel => string.IsNullOrEmpty(spel.Speler2Token)).ToList();

        }

        [HttpGet("{token}")]
        public ActionResult<Spel> GetSpelByToken(string token)
        {
            var spel = _repository.GetSpel(token);
            if (spel == null) return NotFound();
            return Ok(spel);
        }


        [HttpGet("speler/{spelerToken}")]
        public ActionResult<Spel> GetSpelBySpelerToken(string spelerToken)
        {
            var spel = _repository.GetSpellen().Where(spel => spel.Speler1Token == spelerToken || spel.Speler2Token == spelerToken);
            if (spel == null) return NotFound();
            return Ok(spel);
        }
        // [HttpGet("{spelerToken}")]
        // public ActionResult<Spel> GetSpelBySpelerToken(string spelerToken)
        // {
        //     var spel = _repository.GetSpelFromSpeler(spelerToken);
        //     if (spel == null) return NotFound();
        //     return Ok(spel);
        // }
        public class SpelInfo
        {
            public string Omschrijving { get; set; }
            public string Token { get; set; }
        }
        [HttpPost]
        public ActionResult<SpelInfo> AddSpel([FromBody] SpelInfo spelInfo)
        {
            var spel = new Spel();

            spel.Token = Guid.NewGuid().ToString();
            spel.Speler1Token = spelInfo.Token;
            spel.Omschrijving = spelInfo.Omschrijving;

            _repository.AddSpel(spel);

            return Created(nameof(AddSpel), spel);
        }



        [HttpGet("beurt/{token}")]
        public ActionResult<int> GetAanDeBeurt(string token)
        {
            var spel = _repository.GetSpel(token);
            if (spel == null) return NotFound();

            return Ok(spel.AandeBeurt);
        }

        public class GameData
        {
            public string Token { get; set; }

            public string SpelerToken { get; set; }
        }

        public class Zet : GameData
        {
            public int Rij { get; set; }
            public int Kolom { get; set; }
            public bool Pas { get; set; } = false;


        }

        [HttpPut("zet")]
        public ActionResult<Spel> DoeZet([FromBody] Zet data)
        {
            var spel = _repository.GetSpel(data.Token);
            if (spel == null) return NotFound();

            if ((spel.AandeBeurt == Kleur.Wit ? spel.Speler1Token : spel.Speler2Token) != data.SpelerToken)
            {
                return Unauthorized("Niet jou beurt");
            }

            if (!spel.ZetMogelijk(data.Rij, data.Kolom))
            {
                return BadRequest("Niet mogelijk");
            }

            if (data.Pas)
            {
                spel.Pas();
                return Ok(spel);
            }

            spel.DoeZet(data.Rij, data.Kolom);

            return Ok(spel);
        }


        [HttpPut("opgeven")]
        public ActionResult<Spel> Opgeven([FromBody] GameData data)
        {
            var spel = _repository.GetSpel(data.Token);
            if (spel == null) return NotFound();

            if ((spel.AandeBeurt == Kleur.Wit ? spel.Speler1Token : spel.Speler2Token) != data.SpelerToken)
            {
                return Unauthorized("Niet jou beurt");
            }

            // TODO: Opgeven
            return null;
        }
    }

}
