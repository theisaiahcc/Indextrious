using Indextrious.Data;
using Indextrious.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Indextrious.Controllers
{
    public class CardCollectionController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CardCollectionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Collection name cannot be empty.");
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            var collection = new CardCollection
            {
                Name = name,
                Owner = user
            };

            _context.Add(collection);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
