using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebFeedReader.Models;

namespace WebFeedReader.Persistence
{
    public class PersistenceService : IPersistenceService
    {
        private PersistenceContext context;

        public PersistenceService(PersistenceContext context)
        {
            this.context = context;
        }

        public IEnumerable<Feed> GetAllFeeds()
        {
            return context.Feeds.ToList();
        }

        public IEnumerable<FeedItem> GetAllFeedItems()
        {
            return context.FeedItems.Include(feedItem => feedItem.Feed).ToList();
        }

        public IEnumerable<FeedItem> GetFeedItemsInFeed(Feed feed)
        {
            return context.FeedItems.Include(feedItem => feedItem.Feed)
                          .Where(feedItem => feedItem.Feed.ID == feed.ID).ToList();
        }

        public Feed FindFeedById(long id)
        {
            return context.Feeds.Find(id);
        }

        public FeedItem FindFeedItemById(long id)
        {
            return context.FeedItems.Find(id);
        }

        public Feed AddFeed(Feed feed)
        {
            context.Feeds.Add(feed);
            return feed;
        }

        public FeedItem AddFeedItem(FeedItem feedItem)
        {
            context.FeedItems.Add(feedItem);
            return feedItem;
        }

        public Feed ModifyFeed(Feed feed)
        {
            context.Entry(feed).State = EntityState.Modified;
            return feed;
        }

        public Feed DeleteFeed(Feed feed)
        {
            context.Feeds.Remove(feed);
            return feed;
        }

        public Feed DeleteFeedById(long id)
        {
            Feed feed = FindFeedById(id);
            return DeleteFeed(feed);
        }

        public FeedItem DeleteFeedItem(FeedItem feedItem)
        {
            context.FeedItems.Remove(feedItem);
            return feedItem;
        }

        public FeedItem DeleteFeedItemById(long id)
        {
            FeedItem feedItem = FindFeedItemById(id);
            return DeleteFeedItem(feedItem);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}