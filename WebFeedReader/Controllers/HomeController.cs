using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFeedReader.Models;
using WebFeedReader.Persistence;
using WebFeedReader.ViewModels;

namespace WebFeedReader.Controllers
{
    public class HomeController : Controller
    {
        private IPersistenceService persistenceService;

        public HomeController(IPersistenceService persistenceService)
        {
            this.persistenceService = persistenceService;
        }

        public ActionResult Index(long? selectedFeedId, long? selectedFeedItemId)
        {
            Feed selectedFeed;
            IEnumerable<FeedItem> filteredFeedItems;
            FeedItem selectedFeedItem;

            if (selectedFeedId == null)
            {
                selectedFeed = null;
                filteredFeedItems = persistenceService.GetAllFeedItems();
            }
            else
            {
                selectedFeed = persistenceService.FindFeedById((long)selectedFeedId);

                if (selectedFeed == null)
                {
                    return HttpNotFound();
                }

                filteredFeedItems = persistenceService.GetFeedItemsInFeed(selectedFeed);
            }

            if (selectedFeedItemId == null)
            {
                selectedFeedItem = filteredFeedItems.FirstOrDefault();
            }
            else
            {
                selectedFeedItem = persistenceService.FindFeedItemById((long)selectedFeedItemId);

                if (selectedFeedItem == null)
                {
                    return HttpNotFound();
                }
            }

            MainFeedDisplayViewModel vm = new MainFeedDisplayViewModel(
                persistenceService.GetAllFeeds(),
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