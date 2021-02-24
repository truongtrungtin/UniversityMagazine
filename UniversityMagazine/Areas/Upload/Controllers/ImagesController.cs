using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityMagazine.Common;
using UniversityMagazine.DAO;

namespace UniversityMagazine.Areas.Upload.Controllers
{
    public class ImagesController : BaseController
    {
        // GET: Upload/Images
        public ActionResult Index()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ImageDAO().MyUpload(session.UserID);
            return View(model);
        }
        [HttpGet]
        public ActionResult Image(string iMAGE_FileName)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ImageDAO().GetByFileName(session.UserID, iMAGE_FileName);
            return View(model);
        }

        
    }
}