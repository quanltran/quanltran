using System;
using System.Collections.Generic;
using KMS.Twitter.Models;
using System.Configuration;

namespace KMS.Twitter.TwitterHelper
{
    public class TwitterServices
    {
        #region Configuration String Setting
        //Get Oauth from web.config
        private string tempOauthConsumerKey = ConfigurationManager.AppSettings["oauthConsumerKey"];
        private string tempOauthConsumerKeySecret = ConfigurationManager.AppSettings["oauthConsumerKeySecret"];
        private string tempOauthAccessToken = ConfigurationManager.AppSettings["oauthAccessToken"];
        private string tempOauthAccessTokenSecret = ConfigurationManager.AppSettings["oauthAccessTokenSecret"];
        #endregion

        /// <summary>
        /// Call method get
        /// </summary>
        /// <returns></returns>
        public List<TwitterModel> GetTweets()
        {
            List<TwitterModel> lstTweets = new List<TwitterModel>();
            var twitter = new TwitterConnection(tempOauthConsumerKey, tempOauthConsumerKeySecret, tempOauthAccessToken, tempOauthAccessTokenSecret);
            var response = twitter.GetTweets(200);
            dynamic timeline = System.Web.Helpers.Json.Decode(response);

            //GetResponse then show through modelclass
            foreach (var tweet in timeline)
            {
                TwitterModel tempModel = new TwitterModel();
                tempModel.Id = ((dynamic)tweet).id.ToString();
                tempModel.AuthorName = ((dynamic)tweet).user.name;
                tempModel.AuthorUrl = ((dynamic)tweet).user.url;
                tempModel.Content = ((dynamic)tweet).Text;
                string publishedDate = ((dynamic)tweet).created_at;
                publishedDate = publishedDate.Substring(0, 19);
                tempModel.Published = DateTime.ParseExact(publishedDate, "ddd MMM dd HH:mm:ss", null);

                tempModel.ProfileImage = ((dynamic)tweet).user.profile_image_url;
                lstTweets.Add(tempModel);
            }
            return lstTweets;
        }

        /// <summary>
        /// Call method for posting a new status
        /// </summary>
        /// <param name="status">content from TextArea on Site</param>
        public void PostTweets(string status)
        {
            // New Code added for Twitter API 1.1
            var twitter = new TwitterConnection(tempOauthConsumerKey, tempOauthConsumerKeySecret, tempOauthAccessToken, tempOauthAccessTokenSecret);
            var response = twitter.PostTweets(status);
        }
    }
}