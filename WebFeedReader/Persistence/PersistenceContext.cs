using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebFeedReader.Models;

namespace WebFeedReader.Persistence
{
    public class PersistenceContext : DbContext
    {
        public PersistenceContext()
        {
#if DEBUG
            Database.Log = (string s) => System.Diagnostics.Trace.TraceInformation(s);
#endif
        }

        public DbSet<Feed> Feeds { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
    }
}