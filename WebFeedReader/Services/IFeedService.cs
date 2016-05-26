using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebFeedReader.Models;
using WebFeedReader.Persistence;

namespace WebFeedReader.Services
{
    public interface IFeedService
    {
        IList<Feed> GetAllFeeds();

        IList<FeedItem> GetAllFeedItems(params OrderByDescription<FeedItem>[] orderByParams);

        IList<FeedItem> GetFeedItemsInFeed(Feed feed, params OrderByDescription<FeedItem>[] orderByParams);

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