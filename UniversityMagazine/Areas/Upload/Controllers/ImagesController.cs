using EntityModels.EF;
using System;
using System.IO;
using System.Web.Mvc;
using UniversityMagazine.Areas.Upload.DAO;
using UniversityMagazine.Common;

namespace UniversityMagazine.Areas.Upload.Controllers
{
    public class ImagesController : BaseController
    {
        // GET: Upload/Images
        [HasCredential(ROLE_Code = "BROWSEIMAGES", CREDENTIAL_VIEW = true)]
        public ActionResult Index()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ImageDAO().StudentsUpload(session.UserID);
            return View(model);
        }
        [HttpGet]
        [HasCredential(ROLE_Code = "BROWSEIMAGES", CREDENTIAL_VIEW = true)]
        public ActionResult Image(string Id)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new ImageDAO().GetByFileName(session.UserID, Id);
            return View(model);
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSIMAGE", CREDENTIAL_ADD = true)]
        public ActionResult AddCommentImage(COMMENTIMAGE cOMMENTIMAGE)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            cOMMENTIMAGE.ACCOUNT_Id = session.UserID;
            if (new CommentImageDAO().Create(cOMMENTIMAGE))
            {
                SetAlert("Comment successfully!", "success");
            }
            else
            {
                SetAlert("Comment failed!", "warning");
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
                SetAlert("Comment successfully edited!", "success");
            }
            else
            {
                SetAlert("Comment editing was not successful!", "warning");
            }

            return RedirectToAction("Image", new { id = new ImageDAO().GetById(cOMMENTIMAGE.IMAGE_Id).IMAGE_FileName });
        }


        [HttpPost]
        [HasCredential(ROLE_Code = "COMMENTSIMAGE", CREDENTIAL_DELETE = true)]
        public JsonResult DeleteCommentImage(int cOMMENT_Id)
        {
            SetAlert("Delete Comment successfully!", "success");
            return Json(new { data = new CommentImageDAO().Delete(cOMMENT_Id) });
        }


        [HttpGet]
        public PartialViewResult ViewImage(string Id)
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult ChangeStatus(Guid? id)
        {
            var result = new ImageDAO().ChangeStatus(id);
            var model = new ImageDAO().GetById(id);
            if (result == true)
            {
                var filename = Path.GetFileName(Server.MapPath(model.IMAGE_FileUpload));
                var sourceFile = Path.Combine(Server.MapPath(@"~/Images/" + model.IMAGE_UploadTime.Value.ToString("yyyy") + "/" + model.IMAGE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/"), filename);
                var temppath = Server.MapPath(@"~/Images/" + model.IMAGE_UploadTime.Value.ToString("yyyy") + "/" + model.IMAGE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/Approved/");
                if (!Directory.Exists(temppath))
                {
                    Directory.CreateDirectory(temppath);
                }
                model.IMAGE_FileUpload = "/Images/" + model.IMAGE_UploadTime.Value.ToString("yyyy") + "/" + model.IMAGE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/Approved/" + model.IMAGE_FileName + "." + model.IMAGE_Type;
                new ImageDAO().Edit(model);
                System.IO.File.Move(sourceFile, Path.Combine(temppath, filename));
                try
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/templates/Approved.html"));

                    content = content.Replace("{{student}}", model.ACCOUNT.ACCOUNT_Name);
                    content = content.Replace("{{domain}}", Request.Url.Host);
                    content = content.Replace("{{name}}", model.IMAGE_FileName);
                    content = content.Replace("{{type}}", "image");
                    content = content.Replace("{{Url}}", "MyUpload/Image/" + model.IMAGE_FileName + "/");
                    new MailHelper().SendMail(model.ACCOUNT.ACCOUNT_Email, "University Magazine", content, "Approved");

                }
                catch (Exception)
                {
                    SetAlert("Email sending failed!", "warning");
                }
            }
            else
            {
                var filename = Path.GetFileName(Server.MapPath(model.IMAGE_FileUpload));
                var sourceFile = Path.Combine(Server.MapPath(@"~/Images/" + model.IMAGE_UploadTime.Value.ToString("yyyy") + "/" + model.IMAGE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/Approved/"), filename);
                var temppath = Server.MapPath(@"~/Images/" + model.IMAGE_UploadTime.Value.ToString("yyyy") + "/" + model.IMAGE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/");
                if (!Directory.Exists(temppath))
                {
                    Directory.CreateDirectory(temppath);
                }
                model.IMAGE_FileUpload = "/Images/" + model.IMAGE_UploadTime.Value.ToString("yyyy") + "/" + model.IMAGE_UploadTime.Value.ToString("MM") + "/" + model.FACULTY.FACULTY_Code + "/" + model.ACCOUNT.ACCOUNT_Username + "/" + model.IMAGE_FileName + "." + model.IMAGE_Type;
                new ImageDAO().Edit(model);
                System.IO.File.Move(sourceFile, Path.Combine(temppath, filename));
                try
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/templates/Unapproved.html"));

                    content = content.Replace("{{student}}", model.ACCOUNT.ACCOUNT_Name);
                    content = content.Replace("{{domain}}", Request.Url.Host);
                    content = content.Replace("{{name}}", model.IMAGE_FileName);
                    content = content.Replace("{{type}}", "image");
                    content = content.Replace("{{Url}}", "MyUpload/Image/" + model.IMAGE_FileName + "/");
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