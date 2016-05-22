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
using WebFeedReader.Services;
using WebFeedReader.Utils;
using WebFeedReader.ViewModels;

namespace WebFeedReader.Controllers
{
    public class FeedsController : Controller
    {
        private readonly IFeedService feedService;

        public FeedsController(IFeedService feedService)
        {
            this.feedService = feedService;
        }

        // GET: Feeds
        public ActionResult Index()
        {
            return View(feedService.GetAllFeeds());
        }

        // GET: Feeds/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = feedService.FindFeedById((long)id);
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

            feedService.AddFeed(newFeed);

            return RedirectToAction("Index");
        }

        // GET: Feeds/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = feedService.FindFeedById((long)id);
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
                feedService.ModifyFeed(feed);
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
            Feed feed = feedService.FindFeedById((long)id);
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
            feedService.DeleteFeedById(id);
            return RedirectToAction("Index");
        }
    }
}