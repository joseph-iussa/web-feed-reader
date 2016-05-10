using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebFeedReader.Models;
using WebFeedReader.Persistence;

namespace WebFeedReader.Controllers
{
    public class FeedItemsController : Controller
    {
        private IPersistenceService persistenceService;

        public FeedItemsController(IPersistenceService persistenceService)
        {
            this.persistenceService = persistenceService;
        }

        // GET: FeedItems
        public ActionResult Index()
        {
            return View(persistenceService.getAllFeedItems());
        }

        // GET: FeedItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedItem feedItem = persistenceService.findFeedItemById((long)id);
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
            FeedItem feedItem = persistenceService.findFeedItemById((long)id);
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
            persistenceService.deleteFeedItemById(id);
            persistenceService.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                persistenceService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}