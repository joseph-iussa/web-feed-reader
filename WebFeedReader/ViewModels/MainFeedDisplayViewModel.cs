using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFeedReader.Models;

namespace WebFeedReader.ViewModels
{
    public class MainFeedDisplayViewModel
    {
        public readonly IEnumerable<Feed> Feeds;
        public readonly IEnumerable<FeedItem> FilteredFeedItems;
        public readonly Feed SelectedFeed;
        public readonly FeedItem SelectedFeedItem;

        public MainFeedDisplayViewModel(IEnumerable<Feed> Feeds, IEnumerable<FeedItem> FilteredFeedItems,
                                        Feed SelectedFeed, FeedItem SelectedFeedItem)
        {
            this.Feeds = Feeds;
            this.FilteredFeedItems = FilteredFeedItems;
            this.SelectedFeed = SelectedFeed;
            this.SelectedFeedItem = SelectedFeedItem;
        }
    }
}