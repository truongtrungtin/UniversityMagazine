using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using UniversityMagazine.Areas.Management.DAO;
using UniversityMagazine.Common;
using UniversityMagazine.DAO;
using UniversityMagazine.EF;

namespace UniversityMagazine.Controllers
{
    public class MyUploadController : BaseController
    {
        // GET: Article
        [HasCredential(ROLE_Code = "ARTICLE", CREDENTIAL_VIEW = true)]
        public ActionResult Files(string filter = null, string ARTICLES_UploadTimeStart = null, string ARTICLES_UploadTimeFinish = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ArticleDAO().MyUpload(session.UserID, filter, ARTICLES_UploadTimeStart, ARTICLES_UploadTimeFinish);
            return View(model);

        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ARTICLE", CREDENTIAL_VIEW = true)]
        public ActionResult File(string Id)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ArticleDAO().GetByFileName(session.UserID, Id);
            return View(model);
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ARTICLE", CREDENTIAL_ADD = true)]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ARTICLE", CREDENTIAL_ADD = true)]
        public ActionResult UploadFile(ARTICLE aRTICLE, HttpPostedFileBase file)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            Guid id = Guid.NewGuid();
            aRTICLE.ARTICLE_Id = id;
            aRTICLE.ACCOUNT_Id = session.UserID;
            aRTICLE.FACULTY_Id = new AccountDAO().GetById(session.UserID).FACULTY_Id;
            if (file != null)
            {
                string fileName = new ArticleDAO().GetCode(new AccountDAO().GetById(session.UserID).ACCOUNT_Name);
                aRTICLE.ARTICLE_FileName = fileName;
                string extension = Path.GetExtension(file.FileName);
                fileName += extension;
                string Url = "/Articles/" + session.UserID + "/" + id + "/Files/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Articles/" + session.UserID + "/" + id + "/Files/"), fileName);
                if (!Directory.Exists(Server.MapPath("~/Articles/" + session.UserID + "/" + id + "/Files/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Articles/" + session.UserID + "/" + id + "/Files/"));
                }
                aRTICLE.ARTICLE_FileUpload = Url;
                aRTICLE.ARTICLE_Type = extension.Replace(".", "");
                aRTICLE.ARTICLE_Size = file.ContentLength;
                file.SaveAs(fileName);
            }
            if (new ArticleDAO().Create(aRTICLE))
            {
                SetAlert("Đã thêm thành công!", "success");
            }
            else
            {
                SetAlert("Thêm không thành công!", "warning");
            }
            return RedirectToAction("File", new { Id = aRTICLE.ARTICLE_FileName });
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ARTICLE", CREDENTIAL_EDIT = true)]
        public PartialViewResult EditFileUpload(string aRTICLE_FileName)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            return PartialView(new ArticleDAO().GetByFileName(session.UserID, aRTICLE_FileName));
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ARTICLE", CREDENTIAL_EDIT = true)]
        public ActionResult EditFileUpload(ARTICLE aRTICLE, HttpPostedFileBase file)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (file != null)
            {
                string fileName = aRTICLE.ARTICLE_FileName;
                string extension = Path.GetExtension(file.FileName);
                fileName += extension;
                string Url = "/Articles/" + session.UserID + "/" + aRTICLE.ARTICLE_Id + "/Files/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Articles/" + session.UserID + "/" + aRTICLE.ARTICLE_Id + "/Files/"), fileName);
                if (!Directory.Exists(Server.MapPath("~/Articles/" + session.UserID + "/" + aRTICLE.ARTICLE_Id + "/Files/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Articles/" + session.UserID + "/" + aRTICLE.ARTICLE_Id + "/Files/"));
                }
                aRTICLE.ARTICLE_FileUpload = Url;
                aRTICLE.ARTICLE_Type = extension.Replace(".", "");
                aRTICLE.ARTICLE_Size = file.ContentLength;
                file.SaveAs(fileName);
            }
            if (new ArticleDAO().Edit(aRTICLE, session.UserID))
            {
                SetAlert("Successful editing!", "success");
            }
            else
            {
                SetAlert("Editing failed!", "warning");
            }
            return RedirectToAction("File", new { Id = aRTICLE.ARTICLE_FileName });
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ARTICLE", CREDENTIAL_DELETE = true)]
        public JsonResult DeleteFileJson(Guid ARTICLE_Id)
        {
            SetAlert("Xóa thành công!", "success");
            return Json(new { data = new ArticleDAO().Delete(ARTICLE_Id) });
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

            return RedirectToAction("File", new { id = new ArticleDAO().GetById(cOMMENTARTICLE.ARTICLE_Id).ARTICLE_FileName });
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

            return RedirectToAction("File", new { id = new ArticleDAO().GetById(cOMMENTARTICLE.ARTICLE_Id).ARTICLE_FileName });
        }


        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSARTICLE", CREDENTIAL_DELETE = true)]
        public JsonResult DeleteCommentArticle(int cOMMENT_Id)
        {
            return Json(new { data = new CommentArticleDAO().Delete(cOMMENT_Id) });
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "IMAGES", CREDENTIAL_VIEW = true)]
        public ActionResult Image(string iMAGE_FileName)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ImageDAO().GetByFileName(session.UserID, iMAGE_FileName);
            return View(model);
        }
        [HasCredential(ROLE_Code = "IMAGES", CREDENTIAL_VIEW = true)]
        public ActionResult Images()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ImageDAO().MyUpload(session.UserID);
            return View(model);
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "IMAGES", CREDENTIAL_ADD = true)]
        public ActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "IMAGES", CREDENTIAL_ADD = true)]
        public ActionResult UploadImage(IMAGE iMAGES, HttpPostedFileBase image)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            Guid id = Guid.NewGuid();
            iMAGES.IMAGE_Id = id;
            iMAGES.ACCOUNT_Id = session.UserID;
            iMAGES.FACULTY_Id = new AccountDAO().GetById(session.UserID).FACULTY_Id;
            if (image != null)
            {
                string fileName = new ArticleDAO().GetCode(new AccountDAO().GetById(session.UserID).ACCOUNT_Name);
                iMAGES.IMAGE_FileName = fileName;
                string extension = Path.GetExtension(image.FileName);
                fileName += extension;
                string Url = "/Articles/" + session.UserID + "/" + id + "/Files/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Articles/" + session.UserID + "/" + id + "/Files/"), fileName);
                if (!Directory.Exists(Server.MapPath("~/Articles/" + session.UserID + "/" + id + "/Files/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Articles/" + session.UserID + "/" + id + "/Files/"));
                }
                iMAGES.IMAGE_FileUpload = Url;

                iMAGES.IMAGE_Type = extension.Replace(".", "");
                iMAGES.Image_Size = image.ContentLength;
                image.SaveAs(fileName);
            }
            if (new ImageDAO().Create(iMAGES))
            {
                SetAlert("Đã thêm thành công!", "success");
            }
            else
            {
                SetAlert("Thêm không thành công!", "warning");
            }
            return RedirectToAction("UploadImage");
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "IMAGES", CREDENTIAL_DELETE = true)]
        public JsonResult DeleteImage(Guid IMAGE_Id)
        {
            SetAlert("Xóa thành công!", "success");
            return Json(new { data = new ImageDAO().Delete(IMAGE_Id) });
        }

    }
}