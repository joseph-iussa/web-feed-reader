using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using WebFeedReader.Models;
using WebFeedReader.Utils;
using WebFeedReader.ViewModels;

namespace WebFeedReader.Controllers
{
    public class FeedsController : Controller
    {
        private Context db = new Context();

        // GET: Feeds
        public ActionResult Index()
        {
            return View(db.Feeds.ToList());
        }

        // GET: Feeds/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = db.Feeds.Find(id);
            if (feed == null)
            {
                return HttpNotFound();
            }
            return View(feed);
        }

        // GET: Feeds/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeedCreateViewModel feedViewModel)
        {
            if (!ModelState.IsValid) return View(feedViewModel);

            Feed newFeed;
            try
            {
                newFeed = Feed.CreateFeed(feedViewModel.Url);
            }
            catch (FeedDataLoadException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(feedViewModel);
            }

            db.Feeds.Add(newFeed);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Feeds/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = db.Feeds.Find(id);
            if (feed == null)
            {
                return HttpNotFound();
            }
            return View(feed);
        }

        // POST: Feeds/Edit/5 To protect from overposting attacks, please enable the specific
        //       properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Url")] Feed feed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feed);
        }

        // GET: Feeds/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = db.Feeds.Find(id);
            if (feed == null)
            {
                return HttpNotFound();
            }
            return View(feed);
        }

        // POST: Feeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Feed feed = db.Feeds.Find(id);
            db.Feeds.Remove(feed);
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