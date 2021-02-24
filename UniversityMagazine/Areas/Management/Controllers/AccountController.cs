using System;
using System.Configuration;
using System.Web.Mvc;
using UniversityMagazine.Areas.Management.DAO;
using UniversityMagazine.Common;
using UniversityMagazine.EF;

namespace UniversityMagazine.Areas.Management.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [HasCredential(ROLE_Code = "ACCOUNT", CREDENTIAL_VIEW = true)]
        public ActionResult Index()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            return View(new AccountDAO().GetAccountsWithCreated(session.UserID));
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ACCOUNT", CREDENTIAL_ADD = true)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ACCOUNT", CREDENTIAL_ADD = true)]

        public ActionResult Create(ACCOUNT aCCOUNT)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (ModelState.IsValid)
            {
                aCCOUNT.ACCOUNT_CREATOR = session.UserID;
                var dao = new AccountDAO();
                var user = dao.Create(aCCOUNT);
                if (user != null)
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/templates/CreateAccount.html"));

                    content = content.Replace("{{FullName}}", user.ACCOUNT_Name);
                    content = content.Replace("{{Telephone}}", user.ACCOUNT_Telephone);
                    content = content.Replace("{{Email}}", user.ACCOUNT_Email);
                    content = content.Replace("{{Address}}", user.ACCOUNT_Address);
                    content = content.Replace("{{Username}}", user.ACCOUNT_Username);
                    content = content.Replace("{{Faculty}}", "null");
                    content = content.Replace("{{Password}}", aCCOUNT.ACCOUNT_Password);

                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(user.ACCOUNT_Email, "Account University Magazine Management.", content, "Account University Magazine Management.");
                    new MailHelper().SendMail(toEmail, "Account University Magazine Management.", content, "Account University Magazine Management.");
                    SetAlert("Create Account Success! Username and password sent to email.", "success");
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    SetAlert("Error!!!", "warning");
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(ROLE_Code = "ACCOUNT", CREDENTIAL_EDIT = true)]
        public ActionResult Edit(Guid aCCOUNT_Id)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            return View(new AccountDAO().GetAccountWithCreated(session.UserID, aCCOUNT_Id));
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ACCOUNT", CREDENTIAL_EDIT = true)]

        public ActionResult Edit(ACCOUNT aCCOUNT)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                if (dao.Edit(aCCOUNT))
                {
                    SetAlert("Edit Account Success!", "success");
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    SetAlert("Error!!!", "warning");
                }
            }
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        [HasCredential(ROLE_Code = "ACCOUNT", CREDENTIAL_DELETE = true)]
        public ActionResult Delete(Guid[] chkId)
        {
            var result = new AccountDAO().Delete(chkId);
            if (result)
            {
                SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index", "Account");
            }
            else
            {
                SetAlert("Xóa không thành công", "warning");
                return RedirectToAction("Index", "Account");
            }
        }

    }
}