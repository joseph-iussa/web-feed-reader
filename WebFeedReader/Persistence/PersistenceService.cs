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

        public IEnumerable<Feed> getAllFeeds()
        {
            return context.Feeds.AsEnumerable();
        }

        public IEnumerable<FeedItem> getAllFeedItems()
        {
            return context.FeedItems.Include(feedItem => feedItem.Feed).AsEnumerable();
        }

        public Feed findFeedById(long id)
        {
            return context.Feeds.Find(id);
        }

        public FeedItem findFeedItemById(long id)
        {
            return context.FeedItems.Find(id);
        }

        public Feed addFeed(Feed feed)
        {
            context.Feeds.Add(feed);
            return feed;
        }

        public FeedItem addFeedItem(FeedItem feedItem)
        {
            context.FeedItems.Add(feedItem);
            return feedItem;
        }

        public Feed modifyFeed(Feed feed)
        {
            context.Entry(feed).State = EntityState.Modified;
            return feed;
        }

        public Feed deleteFeed(Feed feed)
        {
            context.Feeds.Remove(feed);
            return feed;
        }

        public Feed deleteFeedById(long id)
        {
            Feed feed = findFeedById(id);
            return deleteFeed(feed);
        }

        public FeedItem deleteFeedItem(FeedItem feedItem)
        {
            context.FeedItems.Remove(feedItem);
            return feedItem;
        }

        public FeedItem deleteFeedItemById(long id)
        {
            FeedItem feedItem = findFeedItemById(id);
            return deleteFeedItem(feedItem);
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