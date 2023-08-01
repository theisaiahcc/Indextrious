using Indextrious.Data;
using Indextrious.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Indextrious.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            // Get the collections owned by the current user
            var collections = await _context.CardCollections
                .Where(cc => cc.OwnerId == currentUser.Id)
                .ToListAsync();

            return View(collections);
        }

        public async Task<IActionResult> GetCollectionsPartial()
        {
            var currentUser = await _userManager.GetUserAsync(User);


            var collections = await _context.CardCollections
                .Where(cc => cc.OwnerId == currentUser.Id)
                .ToListAsync();

            return PartialView("_CollectionsPartial", collections);
        }

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