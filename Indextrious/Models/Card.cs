using System.ComponentModel.DataAnnotations.Schema;

namespace Indextrious.Models
{
    public class Card
    {
        /// <summary>
        /// Unique Integer identifier for ICard
        /// </summary>
        public int Id { get; set; }

        public int CardFileId { get; set; } // Foreign key for CardFile
        [ForeignKey("CardFileId")]
        public CardFile CardFile { get; set; } // Navigation property
    }
}
