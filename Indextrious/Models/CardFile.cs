using System.ComponentModel.DataAnnotations.Schema;

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
        public List<CardFile> SubFiles { get; set; } = new List<CardFile>();
        /// <summary>
        /// List of cards in current file
        /// </summary>
        public List<Card> Cards { get; set; } = new List<Card>();

        /// <summary>
        /// Reference to parent CardCollection
        /// </summary>
        public CardCollection ParentCollection { get; set; }
        [ForeignKey("ParentCollection")]
        public int ParentCollectionId { get; set; }
    }   
}
