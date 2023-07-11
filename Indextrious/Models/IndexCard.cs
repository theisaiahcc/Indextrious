namespace Indextrious.Models
{
    public class IndexCard : ICard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
