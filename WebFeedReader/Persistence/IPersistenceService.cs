using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFeedReader.Models;

namespace WebFeedReader.Persistence
{
    public interface IPersistenceService : IDisposable
    {
        IEnumerable<Feed> GetAllFeeds();

        IEnumerable<FeedItem> GetAllFeedItems();

        IEnumerable<FeedItem> GetFeedItemsInFeed(Feed feed);

        Feed FindFeedById(long id);

        FeedItem FindFeedItemById(long id);

        Feed AddFeed(Feed feed);

        Feed ModifyFeed(Feed feed);

        Feed DeleteFeed(Feed feed);

        Feed DeleteFeedById(long id);

        FeedItem AddFeedItem(FeedItem feedItem);

        FeedItem DeleteFeedItem(FeedItem feedItem);

        FeedItem DeleteFeedItemById(long id);

        int SaveChanges();
    }
}