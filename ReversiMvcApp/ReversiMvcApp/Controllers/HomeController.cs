using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using ReversiMvcApp.Data;
using ReversiMvcApp.Lib;
using ReversiMvcApp.Models;
using ReversiMvcApp.Services;

namespace ReversiMvcApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ReversiDbContext _context;
        private readonly IService<Game> _service;

        public HomeController(ILogger<HomeController> logger, ReversiDbContext context, IService<Game> service)
        {
            _logger = logger;
            _context = context;
            _service = service;
        }

        [Authorize]
        public IActionResult Index()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserID != null)
            {
                Player player = _context.Players.FirstOrDefault(p => p.Guid == currentUserID);

                if (player == null)
                {
                    player = new Player()
                    {
                        Guid = currentUserID,
                        Name = User.Identity.Name,
                    };
                    _context.Add(player);
                    _context.SaveChanges();
                }
            }

            //var game = await _service.GetAsync(currentUserID, "/api/game/player");

            //if (game.Token != null)
            //{
            //    return RedirectToAction("Details", "Games", new { id = game.Token });
            //}

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
