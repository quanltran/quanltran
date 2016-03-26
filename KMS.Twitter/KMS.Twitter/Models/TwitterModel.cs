using System;

namespace KMS.Twitter.Models
{
    //Model for using GetResponse
    public class TwitterModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public string AuthorUrl { get; set; }
        public string ProfileImage { get; set; }
        public DateTime Published { get; set; }
    }
}