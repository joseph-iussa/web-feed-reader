using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebFeedReader.Models;
using WebFeedReader.Persistence;

namespace WebFeedReader.Services
{
    public class FeedService : IFeedService
    {
        private readonly IDataRepository<Feed> feedRepo;
        private readonly IDataRepository<FeedItem> feedItemRepo;

        public FeedService(IDataRepository<Feed> feedRepo, IDataRepository<FeedItem> feedItemRepo)
        {
            this.feedRepo = feedRepo;
            this.feedItemRepo = feedItemRepo;
        }

        public IList<Feed> GetAllFeeds()
        {
            return feedRepo.GetList();
        }

        public IList<FeedItem> GetAllFeedItems()
        {
            return feedItemRepo.GetList(navigationProperties: feedItemToFeedNavProp);
        }

        public IList<FeedItem> GetFeedItemsInFeed(Feed feed)
        {
            return feedItemRepo.GetList(
                filterBy: feedItem => feedItem.Feed.ID == feed.ID,
                navigationProperties: feedItemToFeedNavProp);
        }

        public Feed FindFeedById(long id)
        {
            return feedRepo.GetSingle(feed => feed.ID == id);
        }

        public FeedItem FindFeedItemById(long id)
        {
            return feedItemRepo.GetSingle(feedItem => feedItem.ID == id, feedItemToFeedNavProp);
        }

        public void AddFeed(Feed feed)
        {
            feedRepo.Add(feed);
        }

        public void ModifyFeed(Feed feed)
        {
            feedRepo.Update(feed);
        }

        public void DeleteFeed(Feed feed)
        {
            feedRepo.Remove(feed);
        }

        public void DeleteFeedById(long id)
        {
            Feed toDelete = FindFeedById(id);
            feedRepo.Remove(toDelete);
        }

        public void AddFeedItem(FeedItem feedItem)
        {
            feedItemRepo.Add(feedItem);
        }

        public void DeleteFeedItem(FeedItem feedItem)
        {
            feedItemRepo.Remove(feedItem);
        }

        public void DeleteFeedItemById(long id)
        {
            FeedItem toDelete = FindFeedItemById(id);
            feedItemRepo.Remove(toDelete);
        }

        private static Expression<Func<FeedItem, object>>[] feedItemToFeedNavProp =
            new Expression<Func<FeedItem, object>>[] { feedItem => feedItem.Feed };
    }
}