using System;

namespace EntityModels.Models
{
    public class NotificationModel
    {
        public string Id { set; get; }
        public string Content { set; get; }
        public DateTime? Time { set; get; }
        public string From { set; get; }
        public string To { set; get; }
        public string Url { set; get; }

    }
}