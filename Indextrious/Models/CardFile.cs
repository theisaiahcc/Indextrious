namespace Indextrious.Models
{
    public abstract class CardFile
    {
        /// <summary>
        /// Unique integer identifier for each file
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Label of file
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// List of files in current file
        /// </summary>
        public List<CardFile> SubFiles { get; set; } = new List<CardFile>();
        /// <summary>
        /// List of cards in current file
        /// </summary>
        public List<ICard> Cards { get; set; } = new List<ICard>();
    }   
}
