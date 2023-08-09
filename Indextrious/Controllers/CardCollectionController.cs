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

        [HttpPost]
        public async Task<IActionResult> UpdateCollection(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Collection Title cannot be empty");
            }

            var collection = await _context.CardCollections
                .Where(cc => cc.Id == id)
                .SingleOrDefaultAsync();

            if (collection == null)
            {
                return NotFound();
            }

            collection.Name = name;
            _context.Update(collection);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Collection updated successfully", collection });
        }

        public async Task<IActionResult> CollectionIndex(int id)
        {
            var collection = await _context.CardCollections
                .Include(cc => cc.CardFiles)
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

            return RedirectToAction("CollectionIndex", new { id = parentCollectionId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard(string title, string body, int fileId)
        {
            // Validation checks
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            {
                return BadRequest("Card title or body cannot be empty.");
            }

            var file = await _context.CardFiles.FindAsync(fileId);
            if (file == null)
            {
                return NotFound("The associated file was not found.");
            }

            // Creating a new IndexCard
            var card = new IndexCard
            {
                Title = title,
                Body = body
            };

            file.Cards.Add(card);

            await _context.SaveChangesAsync();

            return Ok();  // You can also return any other status or data that you find relevant
        }

        public async Task<IActionResult> GetCardsByFileId(int fileId)
        {
            var cards = await _context.Cards.Where(c => c.CardFileId == fileId).ToListAsync();

            if (!cards.Any())
            {
                return NotFound("No cards found for the specified file.");
            }

            return Ok(cards);
        }

        public async Task<IActionResult> GetFilePartial(int fileId)
        {
            var cards = await _context.Cards
               .Include(c => c.CardFile) // example, if CardFile is a navigation property
               .Where(c => c.CardFileId == fileId)
               .ToListAsync();


            return PartialView("_FilePartial", cards);
        }
    }
}
