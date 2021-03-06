﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFeedReader.Models;
using WebFeedReader.Services;
using WebFeedReader.ViewModels;
using static WebFeedReader.Persistence.OrderByDescription<object>;

namespace WebFeedReader.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFeedService feedService;

        public HomeController(IFeedService feedService)
        {
            this.feedService = feedService;
        }

        public ActionResult Index(long? selectedFeedId, long? selectedFeedItemId)
        {
            Feed selectedFeed;
            IEnumerable<FeedItem> filteredFeedItems;
            FeedItem selectedFeedItem;

            if (selectedFeedId == null)
            {
                selectedFeed = null;
                filteredFeedItems = feedService.GetAllFeedItems(
                    Ord<FeedItem>(fi => fi.PublishedOn, ListSortDirection.Descending));
            }
            else
            {
                selectedFeed = feedService.FindFeedById((long)selectedFeedId);

                if (selectedFeed == null)
                {
                    return HttpNotFound();
                }

                filteredFeedItems = feedService.GetFeedItemsInFeed(
                    selectedFeed, Ord<FeedItem>(fi => fi.PublishedOn, ListSortDirection.Descending));
            }

            if (selectedFeedItemId == null)
            {
                selectedFeedItem = filteredFeedItems.FirstOrDefault();
            }
            else
            {
                selectedFeedItem = feedService.FindFeedItemById((long)selectedFeedItemId);

                if (selectedFeedItem == null)
                {
                    return HttpNotFound();
                }
            }

            MainFeedDisplayViewModel vm = new MainFeedDisplayViewModel(
                feedService.GetAllFeeds(),
                filteredFeedItems,
                selectedFeed,
                selectedFeedItem
            );

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}