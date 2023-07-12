using System.ComponentModel.DataAnnotations.Schema;

namespace Indextrious.Models
{
    public class FlashCard : Card
    {
        /// <summary>
        /// Stores a flash cards prompt string
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// Stores a flash cards answer string
        /// </summary>
        public string Answer { get; set; }
    }
}
