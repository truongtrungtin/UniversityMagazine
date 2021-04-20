using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UniversityMagazine.Common;
using UniversityMagazine.DAO;

namespace UniversityMagazine.Controllers
{
    public class ChatController : BaseController
    {
        // GET: Chat
        public PartialViewResult Index()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ChatDAO().GetMessage();
            return PartialView(model);
        }

        public PartialViewResult GroupChat()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ChatDAO().GetAccounts(new ChatDAO().GetById(session.UserID).FACULTY_Id);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GetName(Guid? id)
        {
            return Json(new { data = new ChatDAO().GetById(id).ACCOUNT_Name });
        }

        [HttpPost]
        public JsonResult GetNotificationMessageWithUser(Guid? id)
        {
            return Json(new { data = new ChatDAO().GetMessage().Where(x => x.MESSAGE_To == id && x.MESSAGE_Status == false).Select(x => x.MESSAGE_From).Distinct() });
        }

        [HttpGet]
        public JsonResult GetNotification(Guid? id)
        {
            var model = new NotificationDAO().ListNotification(id).OrderByDescending(x => x.NOTIFICATION_Time).ToList();
            List<Guid> data = new List<Guid>();
            string url = null;
            foreach (var item in model)
            {
                if (url != item.NOTIFICATION_Url)
                {
                    url = item.NOTIFICATION_Url;
                    data.Add(item.NOTIFICATION_Id);

                }
            }
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }


    }


}