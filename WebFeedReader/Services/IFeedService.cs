using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFeedReader.Models;

namespace WebFeedReader.Services
{
    public interface IFeedService
    {
        IList<Feed> GetAllFeeds();

        IList<FeedItem> GetAllFeedItems();

        IList<FeedItem> GetFeedItemsInFeed(Feed feed);

        Feed FindFeedById(long id);

        FeedItem FindFeedItemById(long id);

        void AddFeed(Feed feed);

        void ModifyFeed(Feed feed);

        void DeleteFeed(Feed feed);

        void DeleteFeedById(long id);

        void AddFeedItem(FeedItem feedItem);

        void DeleteFeedItem(FeedItem feedItem);

        void DeleteFeedItemById(long id);
    }
}