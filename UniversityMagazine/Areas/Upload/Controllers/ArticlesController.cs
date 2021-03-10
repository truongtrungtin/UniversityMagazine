using System;
using System.Collections.Generic;
using System.IO;
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
                SetAlert("Comment đã chỉnh sửa thành công!", "success");
            }
            else
            {
                SetAlert("Comment chỉnh sửa không thành công!", "warning");
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

        [HttpPost]
        public JsonResult ChangeStatus(Guid? id)
        {
            var result = new ArticleDAO().ChangeStatus(id);
            var model = new ArticleDAO().GetById(id);
            if (result == true)
            {
                var filename = Path.GetFileName(Server.MapPath(model.ARTICLE_FileUpload));
                var sourceFile = Path.Combine(Server.MapPath(@"~/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/"),filename);
                var temppath = Server.MapPath(@"~/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/Approved/");
                if (!Directory.Exists(temppath))
                {
                    Directory.CreateDirectory(temppath);
                }
                model.ARTICLE_FileUpload = "/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/Approved/" + model.ARTICLE_FileName + "." + model.ARTICLE_Type;
                new ArticleDAO().Edit(model, model.ACCOUNT_Id);
                System.IO.File.Move(sourceFile, Path.Combine(temppath,filename));
            }
            else
            {
                var filename = Path.GetFileName(Server.MapPath(model.ARTICLE_FileUpload));
                var sourceFile = Path.Combine(Server.MapPath(@"~/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/Approved/"), filename);
                var temppath = Server.MapPath(@"~/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/");
                if (!Directory.Exists(temppath))
                {
                    Directory.CreateDirectory(temppath);
                }
                model.ARTICLE_FileUpload = "/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/" + model.ARTICLE_FileName + "." + model.ARTICLE_Type;
                new ArticleDAO().Edit(model, model.ACCOUNT_Id);
                System.IO.File.Move(sourceFile, Path.Combine(temppath, filename));
            }
            return Json(new
            {
                VAT = result
            });
        }

    }
}