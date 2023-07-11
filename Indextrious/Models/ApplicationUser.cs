using Microsoft.AspNetCore.Identity;

namespace Indextrious.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Navigation property to the CardCollections owned by this user
        /// </summary>
        public List<CardCollection> UserCollections { get; set; } = new List<CardCollection>();
    }
}
