using EntityModels.EF;
using System;
using System.Web.Mvc;
using UniversityMagazine.Areas.Credential.DAO;
using UniversityMagazine.Common;

namespace UniversityMagazine.Areas.Credential.Controllers
{
    public class RoleController : BaseController
    {
        // GET: Admin/Role
        [HasCredential(ROLE_Code = "ROLE", CREDENTIAL_VIEW = true)]
        public ActionResult Index()
        {
            var model = new RoleDAO().ListAll();
            return View(model);
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ROLE", CREDENTIAL_ADD = true)]
        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ROLE", CREDENTIAL_ADD = true)]
        public ActionResult Create(ROLE rOLE)
        {
            if (new RoleDAO().Create(rOLE))
            {
                SetAlert("Added successfully!", "success");
            }
            else
            {
                SetAlert("Add failed, please try again!", "warning");
            }
            return RedirectToAction("Index", "Role");
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ROLE", CREDENTIAL_EDIT = true)]
        public PartialViewResult Edit(Guid rOLE_Id)
        {

            return PartialView(new RoleDAO().GetById(rOLE_Id));
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ROLE", CREDENTIAL_EDIT = true)]
        public ActionResult Edit(ROLE rOLE)
        {
            if (new RoleDAO().Edit(rOLE))
            {
                SetAlert("Successfully edited!", "success");
            }
            else
            {
                SetAlert("Editing failed, please try again!", "warning");
            }
            return RedirectToAction("Index", "Role");
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ROLE", CREDENTIAL_DELETE = true)]
        public ActionResult Delete(Guid[] chkId)
        {
            var result = new RoleDAO().Delete(chkId);
            if (result)
            {
                SetAlert("Deleted successfully", "success");
                return RedirectToAction("Index", "Role");
            }
            else
            {
                SetAlert("Deletion failed!", "warning");
                return RedirectToAction("Index", "Role");
            }
        }
    }
}