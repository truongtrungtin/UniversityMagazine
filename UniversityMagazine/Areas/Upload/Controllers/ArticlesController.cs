using EntityModels.EF;
using System;
using System.IO;
using System.Web.Mvc;
using UniversityMagazine.Areas.Upload.DAO;
using UniversityMagazine.Common;

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
                SetAlert("Comment successfully!", "success");
            }
            else
            {
                SetAlert("Comment failed!", "warning");
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
                SetAlert("Comment editing was not successful!", "warning");
            }

            return RedirectToAction("Article", new { id = new ArticleDAO().GetById(cOMMENTARTICLE.ARTICLE_Id).ARTICLE_FileName });
        }


        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSARTICLE", CREDENTIAL_DELETE = true)]
        public JsonResult DeleteCommentArticle(int cOMMENT_Id)
        {
            SetAlert("Delete Comment successfully!", "success");
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
                var sourceFile = Path.Combine(Server.MapPath(@"~/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/"), filename);
                var temppath = Server.MapPath(@"~/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/Approved/");
                if (!Directory.Exists(temppath))
                {
                    Directory.CreateDirectory(temppath);
                }
                model.ARTICLE_FileUpload = "/Articles/" + model.ARTICLE_UploadTime.Value.ToString("yyyy") + "/" + model.ARTICLE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/Approved/" + model.ARTICLE_FileName + "." + model.ARTICLE_Type;
                new ArticleDAO().Edit(model, model.ACCOUNT_Id);
                System.IO.File.Move(sourceFile, Path.Combine(temppath, filename));
                try
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/templates/Approved.html"));

                    content = content.Replace("{{student}}", model.ACCOUNT.ACCOUNT_Name);
                    content = content.Replace("{{domain}}", Request.Url.Host);
                    content = content.Replace("{{name}}", model.ARTICLE_FileName);
                    content = content.Replace("{{type}}", "article");
                    content = content.Replace("{{Url}}", "MyUpload/File/" + model.ARTICLE_FileName + "/");
                    new MailHelper().SendMail(model.ACCOUNT.ACCOUNT_Email, "University Magazine", content, "Approved");

                }
                catch (Exception)
                {
                    SetAlert("Email sending failed!", "warning");
                }
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
                try
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/templates/Unapproved.html"));

                    content = content.Replace("{{student}}", model.ACCOUNT.ACCOUNT_Name);
                    content = content.Replace("{{domain}}", Request.Url.Host);
                    content = content.Replace("{{name}}", model.ARTICLE_FileName);
                    content = content.Replace("{{type}}", "article");
                    content = content.Replace("{{Url}}", "MyUpload/File/" + model.ARTICLE_FileName + "/");
                    new MailHelper().SendMail(model.ACCOUNT.ACCOUNT_Email, "University Magazine", content, "Unapproved");

                }
                catch (Exception)
                {
                    SetAlert("Email sending failed!", "warning");
                }
            }
            return Json(new
            {
                VAT = result
            });
        }

    }
}