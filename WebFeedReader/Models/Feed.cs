using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFeedReader.Models
{
    public class Feed
    {
        public long ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(4000)]
        public string Url { get; set; }

        public virtual List<FeedItem> FeedItems { get; set; }
    }
}