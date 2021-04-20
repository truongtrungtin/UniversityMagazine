using EntityModels.EF;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using UniversityMagazine.DAO;


namespace UniversityMagazine.ChatHub
{
    public class ChatHub : Hub
    {
        private int Notification;
        private int NotificationWithUser;

        public void Send(Guid? From, Guid? To, string message, string type)
        {
            var dao = new ChatDAO();
            dao.CreateContent(dao.CreateMessage(From, To).MESSAGE_Id, message, type);
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(From, To, message, type);


        }

        public void SendComment(Guid? From, Guid? Article, string type)
        {
            Guid? Id = null;
            if (type == "Article")
            {
                var dao = new ArticleDAO();
                var article = dao.GetById(Article);
                if (article.ACCOUNT_Id == From)
                {
                    var To = new UniversityMagazineDBContext().ACCOUNTs.Where(x => x.ROLEGROUP.ROLEGROUP_Code == "MARKETINGCOORDINATOR" && x.FACULTY_Id == article.FACULTY_Id).FirstOrDefault().ACCOUNT_Id;
                    if (type == "Article")
                    {
                        Id = new CommentArticleDAO().CreateNotificationComment(From, To, "/Upload/Articles/Article/" + article.ARTICLE_FileName, type);
                    }
                    LoadNotification(To);
                }
                else
                {
                    if (type == "Article")
                    {
                        Id = new CommentArticleDAO().CreateNotificationComment(From, article.ACCOUNT_Id, "/MyUpload/File/" + article.ARTICLE_FileName, type);
                    }
                    LoadNotification(article.ACCOUNT_Id);

                }
            }
            else if (type == "Image")
            {
                var dao = new ImageDAO();
                var image = dao.GetById(Article);
                if (image.ACCOUNT_Id == From)
                {
                    var To = new UniversityMagazineDBContext().ACCOUNTs.Where(x => x.ROLEGROUP.ROLEGROUP_Code == "MARKETINGCOORDINATOR" && x.FACULTY_Id == image.FACULTY_Id).FirstOrDefault().ACCOUNT_Id;
                    if (type == "Image")
                    {
                        Id = new CommentArticleDAO().CreateNotificationComment(From, To, "/Upload/Images/Image/" + image.IMAGE_FileName, type);
                    }
                    LoadNotification(To);
                }
                else
                {
                    if (type == "Image")
                    {
                        Id = new CommentArticleDAO().CreateNotificationComment(From, image.ACCOUNT_Id, "/MyUpload/Image/" + image.IMAGE_FileName, type);
                    }
                    LoadNotification(image.ACCOUNT_Id);
                }
            }
            if (Id != null)
            {
                LoadDetailNotification(Id);
            }
        }

        public void LoadNotificationMessage(Guid? To)
        {
            var dao = new ChatDAO();
            // Call the broadcastMessage method to update clients.
            Notification = dao.GetMessage().Where(x => x.MESSAGE_To == To && x.MESSAGE_Status == false).Count();
            Clients.All.broadcastNotificationMessage(Notification, To);
        }
        public void LoadNotificationMessageWithUser(Guid? From, Guid? To)
        {
            var dao = new ChatDAO();
            // Call the broadcastMessage method to update clients.
            NotificationWithUser = dao.GetMessage().Where(x => x.MESSAGE_To == To && x.MESSAGE_From == From && x.MESSAGE_Status == false).Count();
            Clients.All.broadcastNotificationMessageWithUser(NotificationWithUser, From, To);
        }
        public void RemoveNotificationMessage(Guid? To, Guid? From)
        {
            var dao = new ChatDAO();
            dao.ChangeStatus(To, From);
            NotificationWithUser = dao.GetMessage().Where(x => x.MESSAGE_To == To && x.MESSAGE_From == From && x.MESSAGE_Status == false).Count();
            Notification = dao.GetMessage().Where(x => x.MESSAGE_To == To && x.MESSAGE_Status == false).Count();
            Clients.All.broadcastNotificationMessage(Notification, To);
            Clients.All.broadcastNotificationMessageWithUser(NotificationWithUser, From, To);
        }

        public void LoadNotification(Guid? To)
        {
            var dao = new NotificationDAO();
            // Call the broadcastMessage method to update clients.
            Notification = dao.ListNotification(To).Select(x => x.NOTIFICATION_Url).Distinct().Count();
            Clients.All.broadcastNotification(Notification, To);
        }
        public void LoadDetailNotification(Guid? nOTIFICATION_Id)
        {
            var dao = new NotificationDAO();
            // Call the broadcastMessage method to update clients.
            var model = dao.GetById(nOTIFICATION_Id);
            Clients.All.broadcastNotificationDetail(model.NOTIFICATION_To, model.ACCOUNT.ACCOUNT_Name, model.ACCOUNT.ACCOUNT_Avatar, model.NOTIFICATION_Content, model.NOTIFICATION_Time.Value.ToString("dd/MM/yyyy HH:mm"), model.NOTIFICATION_Url);
        }

        public void RemoveNotification(Guid? To, Guid? From)
        {
            var dao = new ChatDAO();
            dao.ChangeStatus(To, From);
            NotificationWithUser = dao.GetMessage().Where(x => x.MESSAGE_To == To && x.MESSAGE_From == From && x.MESSAGE_Status == false).Count();
            Notification = dao.GetMessage().Where(x => x.MESSAGE_To == To && x.MESSAGE_Status == false).Count();
            Clients.All.broadcastNotification(Notification, To);
        }


    }
}