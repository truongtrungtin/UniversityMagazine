using System.Web.Mvc;
using UniversityMagazine.Common;
using UniversityMagazine.DAO;
using UniversityMagazine.Models;

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
                }else if(result == -3)
                {
                    SetAlert("Your faculty has been locked, please contact the marketing manager for more information.","warning");
                }
                else
                {
                    SetAlert("Login information is incorrect!", "warning");
                }
            }
            return RedirectToAction("/");
        }

        public ActionResult Logout()
        {
            //Thoát session
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.SESSION_CREDENTIALS] = null;
            return RedirectToAction("index", "Login");
        }
    }
}