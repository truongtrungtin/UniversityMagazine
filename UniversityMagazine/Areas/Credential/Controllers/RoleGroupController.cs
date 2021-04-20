using EntityModels.EF;
using System;
using System.Web.Mvc;
using UniversityMagazine.Areas.Credential.DAO;
using UniversityMagazine.Common;

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
                SetAlert("Added successfully!", "success");
            }
            else
            {
                SetAlert("Add failed, please try again!", "warning");
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
                SetAlert("Successfully edited!", "success");
            }
            else
            {
                SetAlert("Editing failed, please try again!", "warning");
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
                SetAlert("Successfully edited!", "success");
            }
            else
            {
                SetAlert("Editing failed, please try again!", "warning");
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
                SetAlert("Deleted successfully", "success");
                return RedirectToAction("Index", "RoleGroup");
            }
            else
            {
                SetAlert("Deletion failed!", "warning");
                return RedirectToAction("Index", "RoleGroup");
            }
        }
    }
}