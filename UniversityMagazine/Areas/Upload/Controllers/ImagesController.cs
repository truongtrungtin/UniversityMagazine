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
            SetAlert("Xóa comment thành công!", "success");
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
            }
            return Json(new
            {
                VAT = result
            });
        }


    }
}