using Indextrious.Data;
using Indextrious.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
        public async Task<IActionResult> Create(string name, string color)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Collection name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(color) || !Regex.IsMatch(color, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")) //ChatGPT generated this to check if the hex code is in the proper format
                                                                                                                 // So, the entire expression ^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$ matches strings that: Start with "#",
                                                                                                             // Followed by either: 6 hexadecimal characters(e.g., #1A2B3C), OR 3 hexadecimal characters(e.g., #ABC).
            {
                return BadRequest("Invalid color format.");
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            var collection = new CardCollection
            {
                Name = name,
                Color = color,  // Set the color here
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

        [HttpPost]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            if (id == 0)
            {
                return BadRequest("Collection with associated Id does not exist.");
            }

            var collection = await _context.CardCollections
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();

            if (collection == null)
            {
                return NotFound("The associated collection was not found.");
            }

            _context.Remove(collection);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Collection deleted successfully" });
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
        public async Task<IActionResult> UpdateFile(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("File Title cannot be empty");
            }

            var file = await _context.CardFiles
                .Where(cf => cf.Id == id)
                .SingleOrDefaultAsync();

            if (file == null)
            {
                return NotFound();
            }

            file.Label = name;
            _context.Update(file);
            await _context.SaveChangesAsync();

            return Ok(new { message = "File updated successfully", file });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile(int id)
        {
            if (id == 0)
            {
                return BadRequest("Card with associated Id does not exist.");
            }

            var file = await _context.CardFiles
                .Where(f => f.Id == id)
                .SingleOrDefaultAsync();

            if (file == null)
            {
                return NotFound("The associated card was not found.");
            }

            _context.Remove(file);
            await _context.SaveChangesAsync();
            return Ok(new { message = "File deleted successfully" });
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

        [HttpPost]
        public async Task<IActionResult>UpdateCard(int id, string title, string body, int fileId)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            {
                return BadRequest("Card title or body cannot be empty.");
            }

            var card = await _context.Cards
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();

            if (card == null)
            {
                return NotFound("The associated card was not found.");
            }

            if (card is IndexCard indexCard)
            {
                indexCard.Title = title;
                indexCard.Body = body;
                _context.Update(indexCard);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Card updated successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCard(int id)
        {
            if (id == 0)
            {
                return BadRequest("Card with associated Id does not exist.");
            }

            var card = await _context.Cards
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();

            if (card == null)
            {
                return NotFound("The associated card was not found.");
            }

            _context.Remove(card);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Card deleted successfully" });
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

        public async Task<IActionResult> GetTabsPartial(int collectionId)
        {
            var files = await _context.CardFiles
                .Where(f => f.ParentCollectionId == collectionId)
                .ToListAsync();

            return PartialView("_TabsPartial", files);
        }
    }
}
