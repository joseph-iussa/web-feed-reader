using System.ComponentModel.DataAnnotations;

namespace WebFeedReader.Models
{
    public class FeedItem
    {
        public long ID { get; set; }

        [Required]
        public virtual Feed Feed { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}