using EntityModels.EF;
using EntityModels.Models;
using System;
using System.Web.Mvc;
using UniversityMagazine.Common;
using UniversityMagazine.DAO;

namespace UniversityMagazine.Controllers
{
    public class LoginController : Controller
    {
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "error";
            }
        }
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            if (!new LoginDAO().CheckConnectDatabase())
            {
                SetAlert("Does not connect to database! Please try again later.", "warning");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var dao = new LoginDAO();

                var result = dao.Login(loginModel.UserName, dao.GetMD5(loginModel.Password));
                if (result == 1)
                {
                    var user = dao.GetByUsername(loginModel.UserName);
                    var userSession = new UserLogin();
                    userSession.UserID = user.ACCOUNT_Id;
                    userSession.RoleGroup = user.ROLEGROUP.ROLEGROUP_Code;
                    // Lấy danh sách quyền của người đăng nhập
                    var listCredentials = dao.GetListCredential(user.ACCOUNT_Id);
                    //Tạo session cho danh sách quyền
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    //Tạo session cho người dăng nhập.
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    SetAlert("Account does not exist!", "warning");
                }
                else if (result == -1)
                {
                    SetAlert("The account is locked!", "warning");
                }
                else if (result == -2)
                {
                    SetAlert("Incorrect password!", "warning");
                }
                else if (result == -3)
                {
                    SetAlert("Your faculty has been locked, please contact the marketing manager for more information.", "warning");
                }
                else
                {
                    SetAlert("Login information is incorrect!", "warning");
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Logout()
        {
            //Thoát session
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.SESSION_CREDENTIALS] = null;
            return RedirectToAction("index", "Login");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ForgotPassword(string ACCOUNT_Email)
        {
            if (ACCOUNT_Email != null)
            {
                var dao = new LoginDAO();
                if (dao.CheckEmail(ACCOUNT_Email))
                {
                    int tokenkey = dao.RandomNumber(100000, 999999);
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/templates/ForgotPassword.html"));

                    content = content.Replace("{{FullName}}", dao.GetByEmail(ACCOUNT_Email).ACCOUNT_Name);
                    content = content.Replace("{{tokenkey}}", Convert.ToString(tokenkey));
                    new MailHelper().SendMail(ACCOUNT_Email, "University Magazine", content, "Reset Password");

                    Session.Add(CommonConstants.SESSION_KEY, dao.GetToken(ACCOUNT_Email, dao.GetMD5(Convert.ToString(tokenkey))));
                    Session.Timeout = 5;

                    return RedirectToAction("RecoverPassword", "Login", new { @email = ACCOUNT_Email });
                }
                else
                {
                    SetAlert("Email does not exist!", "warning");

                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult RecoverPassword(string email)
        {
            var session = (TokenModels)Session[CommonConstants.SESSION_KEY];
            if (Session[CommonConstants.SESSION_KEY] == null && session.token == null && session.email != email)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ViewBag.Email = email;
                return View();
            }
        }

        [HttpPost]
        public ActionResult RecoverPassword(string ACCOUNT_Email, string token)
        {
            var dao = new LoginDAO();
            var session = (TokenModels)Session[CommonConstants.SESSION_KEY];
            if (Convert.ToString(session) != "")
            {
                if (session.email == ACCOUNT_Email && session.token == dao.GetMD5(token))
                {
                    return RedirectToAction("NewPassword", "Login", new { @email = ACCOUNT_Email, @token = dao.GetMD5(token) });
                }
                else
                {
                    SetAlert("Wrong code!", "warning");

                }
            }
            else
            {
                SetAlert("Security code does not exist!", "warning");
            }

            return RedirectToAction("RecoverPassword", "Login", new { @email = ACCOUNT_Email });

        }
        [HttpGet]
        public ActionResult NewPassword(string email, string token)
        {
            var session = (TokenModels)Session[CommonConstants.SESSION_KEY];
            if (Convert.ToString(Session[CommonConstants.SESSION_KEY]) != "")
            {
                if (session.email == email && session.token == token)
                {
                    ViewBag.Email = email;
                    return View();
                }
            }
            else
            {
                SetAlert("Security code does not exist!", "warning");
            }
            return RedirectToAction("Index", "Login");

        }
        [HttpPost]
        public ActionResult NewPassword(ACCOUNT aCCOUNT, string NewPassword)
        {
            var dao = new LoginDAO();
            if (Convert.ToString(Session[CommonConstants.SESSION_KEY]) != null)
            {
                var encryptedMd5Pas = dao.GetMD5(NewPassword);
                aCCOUNT.ACCOUNT_Id = dao.GetByEmail(aCCOUNT.ACCOUNT_Email).ACCOUNT_Id;
                aCCOUNT.ACCOUNT_Password = encryptedMd5Pas;
                var result = dao.UpdatePassword(aCCOUNT);
                if (result)
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/templates/ChangePassword.html"));
                    content = content.Replace("{{username}}", dao.GetByEmail(aCCOUNT.ACCOUNT_Email).ACCOUNT_Username);
                    content = content.Replace("{{domain}}", Request.Url.Host);
                    new MailHelper().SendMail(aCCOUNT.ACCOUNT_Email, "University Magazine", content, "Change the password");
                    Session[CommonConstants.SESSION_KEY] = null;

                    SetAlert("Password changed successfully!", "success");
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                SetAlert("Error! Please try again.", "warning");

            }
            return RedirectToAction("Index", "Login");
        }

    }
}