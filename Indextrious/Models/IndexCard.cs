using System.ComponentModel.DataAnnotations.Schema;

namespace Indextrious.Models
{
    public class IndexCard : Card
    {
        /// <summary>
        /// Stores an index cards title string
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Stores an index cards body string
        /// </summary>
        public string Body { get; set; }
    }
}
