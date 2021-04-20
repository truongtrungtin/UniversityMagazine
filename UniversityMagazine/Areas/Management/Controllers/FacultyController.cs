using EntityModels.EF;
using System;
using System.IO;
using System.IO.Compression;
using System.Web.Mvc;
using UniversityMagazine.Areas.Management.DAO;
using UniversityMagazine.Common;

namespace UniversityMagazine.Areas.Management.Controllers
{
    public class FacultyController : BaseController
    {
        // GET: Faculty
        [HasCredential(ROLE_Code = "FACULTY", CREDENTIAL_VIEW = true)]
        public ActionResult Index()
        {
            var model = new FacultyDAO().ListAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult ArticlesApproved(string fACULTY_Code)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session.RoleGroup == "MARKETINGMANAGER")
            {
                var model = new FacultyDAO().ArticlesApproved(fACULTY_Code);
                ViewBag.fACULTY_Code = fACULTY_Code;
                return View(model);
            }
            else
            {
                return RedirectToAction("Error403", "Error", new { area = "" });
            }
        }
        public FileResult DownloadZipArticles(string fACULTY_Code, string month, string year, string username)
        {
            var fileName = string.Format("{0}_articles.zip", DateTime.Today.Date.ToString("MM-yyyy") + "_" + fACULTY_Code + "_" + username);
            var temppath = Server.MapPath("~/Articles/" + year + "/" + month + "/" + fACULTY_Code + "/" + username + "/Approved/");
            var tempOut = Server.MapPath("~/Articles/" + year + "/" + month + "/" + fACULTY_Code + "/" + username + "/");
            if (!Directory.Exists(temppath))
            {
                Directory.CreateDirectory(temppath);
            }
            var tempOutPutPath = Path.Combine(tempOut, fileName);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);
            // Tạo ra file zip bằng cách nén một thư mục.
            ZipFile.CreateFromDirectory(temppath, tempOutPutPath);
            byte[] bytes = System.IO.File.ReadAllBytes(tempOutPutPath);
            return File(bytes, "application/octet-stream", fileName);
        }

        public FileResult DownloadZipImages(string fACULTY_Code, string month, string year, string username)
        {
            var fileName = string.Format("{0}_images.zip", DateTime.Today.Date.ToString("MM-yyyy") + "_" + fACULTY_Code + "_" + username);
            var temppath = Server.MapPath("~/Images/" + year + "/" + month + "/" + fACULTY_Code + "/" + username + "/Approved/");
            var tempOut = Server.MapPath("~/Images/" + year + "/" + month + "/" + fACULTY_Code + "/" + username + "/");
            if (!Directory.Exists(temppath))
            {
                Directory.CreateDirectory(temppath);
            }
            var tempOutPutPath = Path.Combine(tempOut, fileName);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);
            // Tạo ra file zip bằng cách nén một thư mục.
            ZipFile.CreateFromDirectory(temppath, tempOutPutPath);
            byte[] bytes = System.IO.File.ReadAllBytes(tempOutPutPath);
            return File(bytes, "application/octet-stream", fileName);
        }

        [HttpGet]
        public ActionResult ImagesApproved(string fACULTY_Code)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session.RoleGroup == "MARKETINGMANAGER")
            {
                var model = new FacultyDAO().ImagesApproved(fACULTY_Code);
                ViewBag.fACULTY_Code = fACULTY_Code;
                return View(model);
            }
            else
            {
                return RedirectToAction("Error403", "Error", new { area = "" });
            }
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "FACULTY", CREDENTIAL_ADD = true)]
        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "FACULTY", CREDENTIAL_ADD = true)]
        public ActionResult Create(FACULTY fACULTY)
        {
            if (new FacultyDAO().Create(fACULTY))
            {
                SetAlert("Added successfully!", "success");
            }
            else
            {
                SetAlert("Add failed, please try again!", "warning");
            }
            return RedirectToAction("Index", "Faculty");
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "FACULTY", CREDENTIAL_EDIT = true)]
        public PartialViewResult Edit(Guid fACULTY_Id)
        {

            return PartialView(new FacultyDAO().GetById(fACULTY_Id));
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "FACULTY", CREDENTIAL_EDIT = true)]
        public ActionResult Edit(FACULTY fACULTY)
        {
            if (new FacultyDAO().Edit(fACULTY))
            {
                SetAlert("Successfully edited!", "success");
            }
            else
            {
                SetAlert("Editing failed, please try again!", "warning");
            }
            return RedirectToAction("Index", "Faculty");
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "FACULTY", CREDENTIAL_DELETE = true)]
        public ActionResult Delete(Guid? Id)
        {
            var result = new FacultyDAO().Delete(Id);
            if (result)
            {
                SetAlert("Deleted successfully", "success");
                return RedirectToAction("Index", "Faculty");
            }
            else
            {
                SetAlert("Deletion failed!", "warning");
                return RedirectToAction("Index", "Faculty");
            }
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "FACULTY", CREDENTIAL_EDIT = true)]
        public JsonResult ChangeStatus(Guid? Id)
        {
            var result = new FacultyDAO().ChangeStatus(Id);
            return Json(new
            {
                Status = result
            });
        }
    }
}