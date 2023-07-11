namespace Indextrious.Models
{
    public class FlashCard : ICard
    {
        public int Id { get; set; }
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
