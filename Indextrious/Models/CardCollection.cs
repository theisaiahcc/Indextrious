namespace Indextrious.Models
{
    /// <summary>
    /// CardCollection is a container for CardFiles
    /// </summary>
    public class CardCollection
    {
        /// <summary>
        /// Unique integer identifier for CardCollection
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of CardCollection
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Reference to Owner of CardCollection
        /// </summary>
        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        /// <summary>
        /// CardFiles in Collection
        /// </summary>
        public List<CardFile> CardFiles { get; set; } = new List<CardFile>();
    }
}