using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using WebFeedReader.Models;

namespace WebFeedReader.Utils
{
    public static class FeedUtils
    {
        internal static Feed CreateFeedFromFeedData(FeedData feedData)
        {
            Feed newFeed = new Feed();

            newFeed.Title = string.IsNullOrEmpty(feedData.Data.Title?.Text) ? feedData.Url : feedData.Data.Title.Text;
            newFeed.Url = feedData.Url;

            foreach (SyndicationItem newFeedDataItem in feedData.Data.Items)
            {
                FeedItem newFeedItem = new FeedItem
                {
                    Title = newFeedDataItem.Title?.Text ?? "No Title",
                    Content = newFeedDataItem.Content?.ToString() ?? newFeedDataItem.Summary?.Text ?? "No Content"
                };
                newFeed.FeedItems.Add(newFeedItem);
            }

            return newFeed;
        }

        internal static FeedData LoadFeedDataFromUrl(string feedUrl)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(feedUrl);
                request.UserAgent = "Web Feed Reader"; // Some servers require this to be set.
                using (Stream responseStream = request.GetResponse().GetResponseStream())
                {
                    return new FeedData(feedUrl, SyndicationFeed.Load(XmlReader.Create(responseStream)));
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new FeedDataLoadException("Feed url is empty", ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new FeedDataLoadException("Could not access feed", ex);
            }
            catch (Exception ex) when (ex is UriFormatException || ex is ArgumentException)
            {
                throw new FeedDataLoadException("Feed url is malformed", ex);
            }
            catch (XmlException ex)
            {
                throw new FeedDataLoadException("Feed content is invalid", ex);
            }
            catch (Exception ex)
            {
                throw new FeedDataLoadException("An error occurred while loading the feed", ex);
            }
        }

        internal class FeedData
        {
            public readonly string Url;
            public readonly SyndicationFeed Data;

            public FeedData(string url, SyndicationFeed data)
            {
                Url = url;
                Data = data;
            }
        }
    }

    public class FeedDataLoadException : Exception
    {
        public FeedDataLoadException(string msg, Exception ex) : base(msg, ex)
        {
        }
    }
}