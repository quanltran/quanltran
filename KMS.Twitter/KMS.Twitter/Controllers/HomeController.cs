using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KMS.Twitter.Models;

namespace KMS.Twitter.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Using method in another (TwitterServices)
        /// </summary>
        private KMS.Twitter.TwitterHelper.TwitterServices twitterServices = new KMS.Twitter.TwitterHelper.TwitterServices();

        /// <summary>
        /// Action Get all Tweets on Twitter
        /// </summary>
        /// <returns>Get list from response & show on Index page</returns>
        public ActionResult Index()
        {
            List<TwitterModel> tweets = twitterServices.GetTweets();
            return View(tweets);
        }

        /// <summary>
        /// Action Post a new Tweet
        /// </summary>
        /// <param name="status">Status which you wanna update to Twitter</param>
        /// <returns>Back to Index page</returns>
        public ActionResult PostStatus(string status)
        {
            twitterServices.PostTweets(status);
            return RedirectToAction("Index");
        }
    }
}