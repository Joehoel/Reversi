using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReversiMvcApp.Data;
using ReversiMvcApp.Lib;
using ReversiMvcApp.Models;
using ReversiMvcApp.Services;

namespace ReversiMvcApp.Areas.Identity.Pages.Admin
{
    [Authorize("Moderator")]
    public class UsersModel : PageModel
    {
        private ApplicationDbContext _application { get; set; }

        private ReversiDbContext _reversi { get; set; }

        private IService<Game> _api { get; set; }

        public IEnumerable<IdentityUser> Users { get; set; }
            = Enumerable.Empty<IdentityUser>();

        public UsersModel(ApplicationDbContext application, ReversiDbContext reversi, IService<Game> api)
        {
            _application = application;
            _reversi = reversi;
            _api = api;
        }

        public void OnGet()
        {
            Users = _application.Users.ToList();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync([Required] string id)
        {
            // TODO: prevent from user deleting themselves or someone with a higher or same role

            if (ModelState.IsValid)
            {
                var game = await _api.GetAsync("/api/game/player", id);

                if (game != null)
                {
                    await _api.UpdateSpecialAsync(game.Token, new { Player1Token = game.Player1Token, Player2Token = game.Player2Token }, "/api/game/leave");
                }

                var user = await _application.Users.FindAsync(id);
                var player = await _reversi.Players.FindAsync(id);
                _application.Remove(user);

                if (player != null)
                {
                    _reversi.Players.Remove(player);
                    await _reversi.SaveChangesAsync();
                }

                await _reversi.SaveChangesAsync();
                await _application.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
