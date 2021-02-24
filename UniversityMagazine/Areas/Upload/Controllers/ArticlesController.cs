using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityMagazine.Areas.Upload.DAO;
using UniversityMagazine.Common;
using UniversityMagazine.EF;

namespace UniversityMagazine.Areas.Upload.Controllers
{
    public class ArticlesController : BaseController
    {
        // GET: Upload/Articles
        [HasCredential(ROLE_Code = "BROWSEARTICLES", CREDENTIAL_VIEW = true)]
        public ActionResult Index(string filter = null, string ARTICLES_UploadTimeStart = null, string ARTICLES_UploadTimeFinish = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ArticleDAO().StudentsUpload(session.UserID, filter, ARTICLES_UploadTimeStart, ARTICLES_UploadTimeFinish);
            return View(model);

        }

        [HttpGet]
        [HasCredential(ROLE_Code = "BROWSEARTICLES", CREDENTIAL_VIEW = true)]
        public ActionResult Article(string Id)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ArticleDAO().GetByFileName(session.UserID, Id);
            return View(model);
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSARTICLE", CREDENTIAL_ADD = true)]
        public ActionResult AddCommentArticle(COMMENTARTICLE cOMMENTARTICLE)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            cOMMENTARTICLE.ACCOUNT_Id = session.UserID;
            if (new CommentArticleDAO().Create(cOMMENTARTICLE))
            {
                SetAlert("Comment thành công!", "success");
            }
            else
            {
                SetAlert("Comment không thành công!", "warning");
            }

            return RedirectToAction("Article", new { id = new ArticleDAO().GetById(cOMMENTARTICLE.ARTICLE_Id).ARTICLE_FileName });
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "COMMENTSARTICLE", CREDENTIAL_EDIT = true)]
        public PartialViewResult EditCommentArticle(int cOMMENT_Id)
        {
            return PartialView(new CommentArticleDAO().GetById(cOMMENT_Id));
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSARTICLE", CREDENTIAL_EDIT = true)]
        public ActionResult EditCommentArticle(COMMENTARTICLE cOMMENTARTICLE)
        {
            if (new CommentArticleDAO().Edit(cOMMENTARTICLE))
            {
                SetAlert("Comment successfully edited!", "success");
            }
            else
            {
                SetAlert("Comment editing failed!", "warning");
            }

            return RedirectToAction("Article", new { id = new ArticleDAO().GetById(cOMMENTARTICLE.ARTICLE_Id).ARTICLE_FileName });
        }


        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSARTICLE", CREDENTIAL_DELETE = true)]
        public JsonResult DeleteCommentArticle(int cOMMENT_Id)
        {
            SetAlert("Xóa comment thành công!", "success");
            return Json(new { data = new CommentArticleDAO().Delete(cOMMENT_Id) });
        }


        [HttpGet]
        public PartialViewResult ViewArticle(string Id)
        {
            return PartialView();
        }

    }
}