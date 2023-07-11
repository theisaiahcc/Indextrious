namespace Indextrious.Models
{
    public class CardFile
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
        List<CardFile> Files { get; set; }
        /// <summary>
        /// List of cards in current file
        /// </summary>
        List<ICard> Cards { get; set; }
    }
}
