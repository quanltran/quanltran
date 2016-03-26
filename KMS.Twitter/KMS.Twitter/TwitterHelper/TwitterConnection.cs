using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using KMS.Twitter.TwitterHelper;

namespace KMS.Twitter.TwitterHelper
{
    public class TwitterConnection
    {
        /// <summary>
        /// Declare String information
        /// </summary>
        public const string OauthVersion = "1.0";
        public const string OauthSignatureMethod = "HMAC-SHA1";

        public string ConsumerKey { set; get; }
        public string ConsumerKeySecret { set; get; }
        public string AccessToken { set; get; }
        public string AccessTokenSecret { set; get; }

        private KMS.Twitter.TwitterHelper.TwitterHandler handlerString = new KMS.Twitter.TwitterHelper.TwitterHandler();

        //TODO: comment
        public TwitterConnection(string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerKeySecret = consumerKeySecret;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        /// <summary>
        /// Send request then Receive responese from server
        /// </summary>
        /// <param name="count">number of item which we want to get (max: 200 items/1 times)</param>
        /// <returns>Get data JSON from Twitter</returns>
        public string GetTweets(int count)
        {
            string resourceUrl = string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json");
            var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("count", count.ToString());
            var response = handlerString.GetResponse(resourceUrl, "GET", requestParameters);
            return response;
        }

        /// <summary>
        /// Send request then Receive responese from server
        /// </summary>
        /// <param name="status">content which we update to Twitter</param>
        /// <returns>Get data JSON from Twitter</returns>
        public string PostTweets(string status)
        {
            string resourceUrl = string.Format("https://api.twitter.com/1.1/statuses/update.json");
            var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("status", status);
            var response = handlerString.GetResponse(resourceUrl, "POST", requestParameters);
            return response;
        }

        
    }

    public static class Extensions
    {
        //TODO: Comment something
        public static string ToWebString(this SortedDictionary<string, string> source)
        {
            var body = new StringBuilder();
            foreach (var requestParameter in source)
            {
                body.Append(requestParameter.Key);
                body.Append("=");
                body.Append(Uri.EscapeDataString(requestParameter.Value));
                body.Append("&");
            }
            //remove trailing '&' 
            body.Remove(body.Length - 1, 1);

            return body.ToString();
        }
    }
}