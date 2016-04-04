using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebFeedReader.Models;

namespace WebFeedReader.Controllers
{
    public class FeedItemsController : Controller
    {
        private Context db = new Context();

        // GET: FeedItems
        public ActionResult Index()
        {
            return View(db.FeedItems.ToList());
        }

        // GET: FeedItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedItem feedItem = db.FeedItems.Find(id);
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
            FeedItem feedItem = db.FeedItems.Find(id);
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
            FeedItem feedItem = db.FeedItems.Find(id);
            db.FeedItems.Remove(feedItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}