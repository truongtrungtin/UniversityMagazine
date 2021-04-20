using EntityModels.EF;
using System.IO;
using System.Web;
using System.Web.Mvc;
using UniversityMagazine.Common;
using UniversityMagazine.DAO;

namespace UniversityMagazine.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            return View(new LoginDAO().GetById(session.UserID));
        }

        [HttpPost]
        public ActionResult Edit(ACCOUNT aCCOUNT)

        {
            if (ModelState.IsValid)
            {
                if (new LoginDAO().Edit(aCCOUNT))
                {
                    SetAlert("Successfully edited!", "success");
                }
            }
            else
            {
                SetAlert("Editing failed, please try again!", "warning");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Avatar(string ACCOUNT_Username, HttpPostedFileBase ACCOUNT_Avatar)
        {
            if (ModelState.IsValid)
            {
                ACCOUNT aCCOUNT = new ACCOUNT();
                aCCOUNT.ACCOUNT_Username = ACCOUNT_Username;
                var dao = new LoginDAO().GetByUsername(ACCOUNT_Username);
                if (ACCOUNT_Avatar != null)
                {
                    string fileName = new ArticleDAO().GetCode(dao.ACCOUNT_Name);
                    string extension = Path.GetExtension(ACCOUNT_Avatar.FileName);
                    fileName += extension;
                    string Url = "/Content/dist/img/" + dao.ACCOUNT_Id + "/Avatar/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/dist/img/" + dao.ACCOUNT_Id + "/Avatar/"), fileName);
                    if (!Directory.Exists(Server.MapPath("~/Content/dist/img/" + dao.ACCOUNT_Id + "/Avatar/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/dist/img/" + dao.ACCOUNT_Id + "/Avatar/"));
                    }
                    ACCOUNT_Avatar.SaveAs(fileName);
                    aCCOUNT.ACCOUNT_Avatar = Url;
                    if (new LoginDAO().Avatar(aCCOUNT))
                    {
                        SetAlert("Profile has been changed successfully!", "success");
                    }
                }
                else
                {
                    SetAlert("Profile change failed, please try again!", "warning");

                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChangePassword(string ACCOUNT_Username, string OldPassword, string NewPassword, string ConfirmNewPassword)
        {
            if (ModelState.IsValid)
            {
                ACCOUNT aCCOUNT = new ACCOUNT();
                var dao = new LoginDAO();
                if (dao.CheckPassword(ACCOUNT_Username, dao.GetMD5(OldPassword)))
                {
                    if (OldPassword != NewPassword)
                    {
                        if (NewPassword == ConfirmNewPassword)
                        {
                            aCCOUNT.ACCOUNT_Username = ACCOUNT_Username;
                            aCCOUNT.ACCOUNT_Password = dao.GetMD5(NewPassword);
                            var result = dao.UpdatePassword(aCCOUNT);
                            if (result)
                            {
                                string content = System.IO.File.ReadAllText(Server.MapPath("~/templates/ChangePassword.html"));
                                content = content.Replace("{{username}}", ACCOUNT_Username);
                                new MailHelper().SendMail(dao.GetByUsername(ACCOUNT_Username).ACCOUNT_Email, "University Magazine", content, "Authenticate information");
                                SetAlert("Password changed successfully!", "success");
                            }
                            else
                            {
                                SetAlert("Password change failed!", "warning");

                            }
                        }
                        else
                        {
                            SetAlert("New passwords are not the same!", "warning");
                        }
                    }
                    else
                    {
                        SetAlert("Old password cannot match new password!", "warning");
                    }

                }
                else
                {
                    SetAlert("Wrong password!", "warning");
                }
            }
            return RedirectToAction("Index");
        }
    }
}