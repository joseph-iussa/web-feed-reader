using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebFeedReader.Models;
using WebFeedReader.Services;

namespace WebFeedReader.Controllers
{
    public class FeedItemsController : Controller
    {
        private readonly IFeedService feedService;

        public FeedItemsController(IFeedService feedService)
        {
            this.feedService = feedService;
        }

        // GET: FeedItems
        public ActionResult Index()
        {
            return View(feedService.GetAllFeedItems());
        }

        // GET: FeedItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedItem feedItem = feedService.FindFeedItemById((long)id);
            if (feedItem == null)
            {
                return HttpNotFound();
            }
            return View(feedItem);
        }

        // GET: FeedItems/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedItem feedItem = feedService.FindFeedItemById((long)id);
            if (feedItem == null)
            {
                return HttpNotFound();
            }
            return View(feedItem);
        }

        // POST: FeedItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            feedService.DeleteFeedItemById(id);
            return RedirectToAction("Index");
        }
    }
}