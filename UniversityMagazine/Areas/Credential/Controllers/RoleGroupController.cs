using System;
using System.Web.Mvc;
using UniversityMagazine.Areas.Credential.DAO;
using UniversityMagazine.Common;
using UniversityMagazine.EF;

namespace UniversityMagazine.Areas.Credential.Controllers
{
    public class RoleGroupController : BaseController
    {
        // GET: Admin/GroupRole
        [HttpGet]
        [HasCredential(ROLE_Code = "ROLEGROUP", CREDENTIAL_VIEW = true)]
        public ActionResult Index()
        {
            var model = new RoleGroupDAO().ListAll();
            return View(model);
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ROLEGROUP", CREDENTIAL_ADD = true)]
        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ROLEGROUP", CREDENTIAL_ADD = true)]
        public ActionResult Create(ROLEGROUP rOLEGROUP)
        {
            var result = new RoleGroupDAO().Create(rOLEGROUP);
            if (result)
            {
                SetAlert("Đã thêm thành công!", "success");
            }
            else
            {
                SetAlert("Thêm không thành công, vui lòng thử lại!", "warning");
            }
            return RedirectToAction("Index", "RoleGroup");
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ROLEGROUP", CREDENTIAL_EDIT = true)]
        public PartialViewResult Edit(Guid rOLEGROUP_Id)
        {

            return PartialView(new RoleGroupDAO().GetById(rOLEGROUP_Id));
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ROLEGROUP", CREDENTIAL_EDIT = true)]
        public ActionResult Edit(ROLEGROUP rOLEGROUP)
        {
            if (new RoleGroupDAO().Edit(rOLEGROUP))
            {
                SetAlert("Đã sửa thành công!", "success");
            }
            else
            {
                SetAlert("Chỉnh sửa không thành công, vui lòng thử lại!", "warning");
            }
            return RedirectToAction("Index", "RoleGroup");
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "CREDENTIAL", CREDENTIAL_EDIT = true)]
        public PartialViewResult Credential(Guid rOLEGROUP_Id)
        {
            var dao = new RoleGroupDAO();
            ViewBag.ROLEGROUP_Id = rOLEGROUP_Id;
            ViewBag.ROLEGROUP_Code = dao.GetById(rOLEGROUP_Id).ROLEGROUP_Code;
            return PartialView(dao.GetCredentialByRoleGroup(rOLEGROUP_Id));
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "CREDENTIAL", CREDENTIAL_EDIT = true)]
        public ActionResult Credential()
        {
            if (new RoleGroupDAO().EditCredential(HttpContext.Request))
            {
                SetAlert("Đã sửa thành công!", "success");
            }
            else
            {
                SetAlert("Chỉnh sửa không thành công, vui lòng thử lại!", "warning");
            }
            return RedirectToAction("Index", "RoleGroup");
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "CREDENTIAL", CREDENTIAL_DELETE = true)]
        public ActionResult Delete(Guid[] chkId)
        {
            var result = new RoleGroupDAO().Delete(chkId);
            if (result)
            {
                SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index", "RoleGroup");
            }
            else
            {
                SetAlert("Xóa không thành công", "warning");
                return RedirectToAction("Index", "RoleGroup");
            }
        }
    }
}