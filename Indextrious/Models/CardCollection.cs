namespace Indextrious.Models
{
    public class CardCollection : CardFile
    {
        /// <summary>
        /// Reference to ApplicationUser who owns the CardCollection
        /// </summary>
        public ApplicationUser Owner { get; set; }
    }
}