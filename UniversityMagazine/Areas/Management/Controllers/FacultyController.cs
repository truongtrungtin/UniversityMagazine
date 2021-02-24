using System;
using System.Web.Mvc;
using UniversityMagazine.Areas.Management.DAO;
using UniversityMagazine.Common;
using UniversityMagazine.EF;

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
                SetAlert("Đã thêm thành công!", "success");
            }
            else
            {
                SetAlert("Thêm không thành công, vui lòng thử lại!", "warning");
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
                SetAlert("Đã sửa thành công!", "success");
            }
            else
            {
                SetAlert("Chỉnh sửa không thành công, vui lòng thử lại!", "warning");
            }
            return RedirectToAction("Index", "Faculty");
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "FACULTY", CREDENTIAL_DELETE = true)]
        public ActionResult Delete(Guid[] chkId)
        {
            var result = new FacultyDAO().Delete(chkId);
            if (result)
            {
                SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index", "Faculty");
            }
            else
            {
                SetAlert("Xóa không thành công", "warning");
                return RedirectToAction("Index", "Faculty");
            }
        }
    }
}