using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace KMS.Twitter.TwitterHelper
{
    public class TwitterHeader
    {
        /// <summary>
        /// Set value create header url
        /// </summary>
        public const string OauthVersion         = "1.0";
        public const string OauthSignatureMethod = "HMAC-SHA1";

        private string oauthConsumerKey       = ConfigurationManager.AppSettings["oauthConsumerKey"];
        private string oauthConsumerKeySecret = ConfigurationManager.AppSettings["oauthConsumerKeySecret"];
        private string oauthAccessToken       = ConfigurationManager.AppSettings["oauthAccessToken"];
        private string oauthAccessTokenSecret = ConfigurationManager.AppSettings["oauthAccessTokenSecret"];

        /// <summary>
        /// Create header for URL
        /// </summary>
        /// <param name="resourceUrl">Template URL</param>
        /// <param name="methodName">method URL (POST, GET, ...)</param>
        /// <param name="requestParameters">parameter follow Twitter's template</param>
        /// <returns></returns>
        public string CreateHeader(string resourceUrl, string methodName, SortedDictionary<string, string> requestParameters)
        {
            // ConvertString to Base64 
            var oauthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

            var oauthTimestamp = CreateOAuthTimestamp();
            var oauthSignature = CreateOauthSignature(resourceUrl, methodName, oauthNonce, oauthTimestamp, requestParameters);

            //The oAuth signature is then used to generate the Authentication header. 
            const string HeaderFormat =
                "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                "oauth_token=\"{4}\", oauth_signature=\"{5}\", " + "oauth_version=\"{6}\"";

            var authHeader = string.Format(HeaderFormat, Uri.EscapeDataString(oauthNonce), Uri.EscapeDataString(OauthSignatureMethod),
                                                         Uri.EscapeDataString(oauthTimestamp), Uri.EscapeDataString(oauthConsumerKey),
                                                         Uri.EscapeDataString(oauthAccessToken), Uri.EscapeDataString(oauthSignature), Uri.EscapeDataString(OauthVersion));

            return authHeader;
        }

        /// <summary>
        /// Authorization: contain an additional HTTP header with enough information
        /// </summary>
        /// <param name="resourceUrl">Template URL</param>
        /// <param name="method">method URL (POST, GET, ...)</param>
        /// <param name="oauthNonce">(1 time 1 value) Parameter let Twitter know spam or not</param>
        /// <param name="oauthTimestamp">TODO</param>
        /// <param name="requestParameters">parameter follow Twitter's template</param>
        /// <returns></returns>
        private string CreateOauthSignature(string resourceUrl, string method, string oauthNonce, string oauthTimestamp, SortedDictionary<string, string> requestParameters)
        {
            //firstly we need to add the standard oauth parameters to the sorted list 
            requestParameters.Add("oauth_consumer_key", oauthConsumerKey);
            requestParameters.Add("oauth_nonce", oauthNonce);
            requestParameters.Add("oauth_signature_method", OauthSignatureMethod);
            requestParameters.Add("oauth_timestamp", oauthTimestamp);
            requestParameters.Add("oauth_token", oauthAccessToken);
            requestParameters.Add("oauth_version", OauthVersion);

            var sigBaseString = requestParameters.ToWebString();
            var signatureBaseString = string.Concat(method, "&", Uri.EscapeDataString(resourceUrl), "&", Uri.EscapeDataString(sigBaseString.ToString()));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauthConsumerKeySecret), "&", Uri.EscapeDataString(oauthAccessTokenSecret));
            string oauthSignature;
            using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(compositeKey)))
            {
                oauthSignature = Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString)));
            }
            return oauthSignature;
        }

        /// <summary>
        /// TODO: comment
        /// </summary>
        /// <returns></returns>
        private static string CreateOAuthTimestamp()
        {
            var nowUtc = DateTime.UtcNow;
            var timeSpan = nowUtc - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
            return timestamp;
        }
    }
}