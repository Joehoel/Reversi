using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using ReversiMvcApp.Services;

namespace ReversiMvcApp.Controllers
{
    public class GamesController : Controller
    {
        private IService<Game> _service;

        public GamesController(IService<Game> service)
        {
            _service = service;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAsync("/api/game"));
        }

        // GET: Games/Play/5
        public async Task<IActionResult> Play(string id)
        {
            var game = await _service.GetAsync(id, "/api/game");

            if(game.Token == null)
            {
                return NotFound();
            }  

            return View(game);
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var playerToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var game = await _service.GetAsync(playerToken, "/api/game/player");

            if (game.Token != null && game.Token != id)
            {
                return RedirectToAction("Details", "Games", new { id = game.Token });

            }

            if (id == null)
            {
                return NotFound();
            }

            game = await _service.GetAsync(id, "/api/game");
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public async Task<IActionResult> Create()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var game = await _service.GetAsync(currentUserID, "/api/game/player");

            if (game.Token != null)
            {
                return RedirectToAction("Details", "Games", new { id = game.Token });
            }

            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.Player1Token = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _service.AddAsync(game, "/api/game");
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var game = await _service.GetAsync(id, "/api/game");
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Token,Description,Player1Token,Player2Token,Board,TurnColor")] Game game)
        {
            if (id != game.Token)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(id, game, "/api/game");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _service.GetAsync(id, "/api/game");
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _service.DeleteAsync(id, "/api/game");

            return RedirectToAction(nameof(Index));
        }

        private Game CheckForGame(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = user;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var response = _service.GetAsync(currentUserID, "/api/game/player").Result;
                if (response.Token != null)
                {
                    return response;

                }
                return null;
            }
            return null;

        }
    }
}
