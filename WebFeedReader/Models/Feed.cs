using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebFeedReader.Utils.FeedUtils;

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

        public Feed()
        {
            FeedItems = new List<FeedItem>();
        }

        public static Feed CreateFeed(string feedUrl)
        {
            return CreateFeedFromFeedData(LoadFeedDataFromUrl(feedUrl));
        }
    }
}