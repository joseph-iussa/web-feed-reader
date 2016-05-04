using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFeedReader.Models;

namespace WebFeedReader.Persistence
{
    interface IPersistenceService : IDisposable
    {
        IEnumerable<Feed> getAllFeeds();

        IEnumerable<FeedItem> getAllFeedItems();

        Feed findFeedById(long id);

        FeedItem findFeedItemById(long id);

        Feed addFeed(Feed feed);

        Feed modifyFeed(Feed feed);

        Feed deleteFeed(Feed feed);

        Feed deleteFeedById(long id);

        FeedItem addFeedItem(FeedItem feedItem);

        FeedItem deleteFeedItem(FeedItem feedItem);

        FeedItem deleteFeedItemById(long id);

        int SaveChanges();
    }
}