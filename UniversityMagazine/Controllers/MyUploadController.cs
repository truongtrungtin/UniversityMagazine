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
            var user = new AccountDAO().GetById(session.UserID);
            Guid id = Guid.NewGuid();
            aRTICLE.ARTICLE_Id = id;
            aRTICLE.ACCOUNT_Id = session.UserID;
            aRTICLE.FACULTY_Id = user.FACULTY_Id;
            if (file != null && (Path.GetExtension(file.FileName) == ".docx" || Path.GetExtension(file.FileName) == ".doc" || Path.GetExtension(file.FileName) == ".pdf"))
            {
                string fileName = new ArticleDAO().GetCode(new StringHelper().RemoveUnicode(user.ACCOUNT_Name));
                aRTICLE.ARTICLE_FileName = fileName;
                string extension = Path.GetExtension(file.FileName);
                fileName += extension;
                string Url = "/Articles/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" + user.ACCOUNT_Username + "/" +fileName;
                fileName = Path.Combine(Server.MapPath("~/Articles/"+ DateTime.Now.ToString("yyyy") + "/"+ DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" +  user.ACCOUNT_Username + "/"), fileName);
                if (!Directory.Exists(Server.MapPath("~/Articles/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" +  user.ACCOUNT_Username + "/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Articles/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" +  user.ACCOUNT_Username + "/"));
                }
                aRTICLE.ARTICLE_FileUpload = Url;
                aRTICLE.ARTICLE_Type = extension.Replace(".", "");
                aRTICLE.ARTICLE_Size = file.ContentLength;
                file.SaveAs(fileName);
                if (new ArticleDAO().Create(aRTICLE))
                {
                    SetAlert("Đã thêm thành công!", "success");
                    return RedirectToAction("File", new { Id = aRTICLE.ARTICLE_FileName });
                }
                else
                {
                    SetAlert("Thêm không thành công!", "warning");
                }
            }
            else
            {
                SetAlert("Chưa có file hoặc Định dạng file upload không đúng!", "warning");
            }
            return RedirectToAction("Files");
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
            var user = new AccountDAO().GetById(session.UserID);
            if (file != null && (Path.GetExtension(file.FileName) == ".docx" || Path.GetExtension(file.FileName) == ".doc" || Path.GetExtension(file.FileName) == ".pdf"))
            {
                string fileName = new ArticleDAO().GetCode(new StringHelper().RemoveUnicode(user.ACCOUNT_Name));
                string extension = Path.GetExtension(file.FileName);
                fileName += extension;
                string Url = "/Articles/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" +  user.ACCOUNT_Username  +"/"+ fileName;
                fileName = Path.Combine(Server.MapPath("~/Articles/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" +  user.ACCOUNT_Username ), fileName);
                if (!Directory.Exists(Server.MapPath("~/Articles/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" +  user.ACCOUNT_Username )))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Articles/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" +  user.ACCOUNT_Username ));
                }
                aRTICLE.ARTICLE_FileUpload = Url;
                aRTICLE.ARTICLE_Type = extension.Replace(".", "");
                aRTICLE.ARTICLE_Size = file.ContentLength;
                file.SaveAs(fileName);
                if (new ArticleDAO().Edit(aRTICLE, session.UserID))
                {
                    SetAlert("Successful editing!", "success");
                    return RedirectToAction("File", new { Id = aRTICLE.ARTICLE_FileName });
                }
                else
                {
                    SetAlert("Editing failed!", "warning");
                }
            }
            else
            {
                SetAlert("Chưa có file hoặc Định dạng file upload không đúng!", "warning");
            }
            return RedirectToAction("Files");

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
                SetAlert("Comment đã chỉnh sửa thành công!", "success");
            }
            else
            {
                SetAlert("Comment chỉnh sửa không thành công!", "warning");
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
        public ActionResult Image(string id)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ImageDAO().GetByFileName(session.UserID, id);
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
            var user = new AccountDAO().GetById(session.UserID);
            Guid id = Guid.NewGuid();
            iMAGES.IMAGE_Id = id;
            iMAGES.ACCOUNT_Id = session.UserID;
            iMAGES.FACULTY_Id = user.FACULTY_Id;
            if (image != null)
            {
                string fileName = new ImageDAO().GetCode(new StringHelper().RemoveUnicode(user.ACCOUNT_Name));
                string extension = Path.GetExtension(image.FileName);
                iMAGES.IMAGE_FileName = fileName;
                fileName += extension;
                string Url = "/Images/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" + user.ACCOUNT_Username + "/"+ fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" + user.ACCOUNT_Username + "/"), fileName);
                if (!Directory.Exists(Server.MapPath("~/Images/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" + user.ACCOUNT_Username + "/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Images/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + user.FACULTY.FACULTY_Code + "/" + user.ACCOUNT_Username + "/"));
                }
                iMAGES.IMAGE_FileUpload = Url;

                iMAGES.IMAGE_Type = extension.Replace(".", "");
                iMAGES.IMAGE_Size = image.ContentLength;
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

        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSIMAGE", CREDENTIAL_ADD = true)]
        public ActionResult AddCommentImage(COMMENTIMAGE cOMMENTIMAGE)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            cOMMENTIMAGE.ACCOUNT_Id = session.UserID;
            if (new CommentImageDAO().Create(cOMMENTIMAGE))
            {
                SetAlert("Comment thành công!", "success");
            }
            else
            {
                SetAlert("Comment không thành công!", "warning");
            }

            return RedirectToAction("Image", new { id = new ImageDAO().GetById(cOMMENTIMAGE.IMAGE_Id).IMAGE_FileName });
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "COMMENTSIMAGE", CREDENTIAL_EDIT = true)]
        public PartialViewResult EditCommentImage(int cOMMENT_Id)
        {
            return PartialView(new CommentImageDAO().GetById(cOMMENT_Id));
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSIMAGE", CREDENTIAL_EDIT = true)]
        public ActionResult EditCommentImage(COMMENTIMAGE cOMMENTIMAGE)
        {
            if (new CommentImageDAO().Edit(cOMMENTIMAGE))
            {
                SetAlert("Comment đã chỉnh sửa thành công!", "success");
            }
            else
            {
                SetAlert("Comment chỉnh sửa không thành công!", "warning");
            }

            return RedirectToAction("Image", new { id = new ImageDAO().GetById(cOMMENTIMAGE.IMAGE_Id).IMAGE_FileName });
        }


        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSIMAGE", CREDENTIAL_DELETE = true)]
        public JsonResult DeleteCommentImage(int cOMMENT_Id)
        {
            return Json(new { data = new CommentImageDAO().Delete(cOMMENT_Id) });
        }
    }
}