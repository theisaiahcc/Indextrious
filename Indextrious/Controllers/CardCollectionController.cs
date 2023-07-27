using Indextrious.Data;
using Indextrious.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> CollectionIndex(int id)
        {
            var collection = await _context.CardCollections
                .Where(cc => cc.Id == id)
                .SingleOrDefaultAsync();

            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile(string name, int parentCollectionId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("File name cannot be empty.");
            }

            var file = new CardFile
            {
                Label = name,
                ParentCollectionId = parentCollectionId
            };

            _context.Add(file);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
