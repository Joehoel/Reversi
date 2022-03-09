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
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReversiDbContext _context;

        public HomeController(ILogger<HomeController> logger, ReversiDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Debug.WriteLine(currentUserID);
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
                    _context.Add<Player>(player);
                    _context.SaveChanges();
                }
            }

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
