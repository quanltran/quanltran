using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace KMS.Twitter.TwitterHelper
{
    public class TwitterHandler
    {
        private KMS.Twitter.TwitterHelper.TwitterHeader headerString = new KMS.Twitter.TwitterHelper.TwitterHeader();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceUrl"></param>
        /// <param name="methodName"></param>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public string GetResponse(string resourceUrl, string methodName, SortedDictionary<string, string> requestParameters)
        {
            ServicePointManager.Expect100Continue = false;
            WebRequest request = null;
            string resultString = string.Empty;

            if (requestParameters.Count > 0)
            {
                request = (HttpWebRequest)WebRequest.Create(resourceUrl + "?" + requestParameters.ToWebString());
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(resourceUrl);
            }

            request.Method = methodName;
            request.ContentType = "application/x-www-form-urlencoded";

            if (request != null)
            {
                var authHeader = headerString.CreateHeader(resourceUrl, methodName, requestParameters);
                request.Headers.Add("Authorization", authHeader);

                var response = (HttpWebResponse)request.GetResponse();
                using (var sd = new StreamReader(response.GetResponseStream()))
                {
                    resultString = sd.ReadToEnd();
                    response.Close();
                }
            }
            return resultString;
        }

    }
}