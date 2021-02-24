using System.IO;
using System.Web;
using System.Web.Mvc;
using UniversityMagazine.Common;
using UniversityMagazine.DAO;
using UniversityMagazine.EF;

namespace UniversityMagazine.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index(string Id)
        {
            return View(new LoginDAO().GetByUsername(Id));
        }

        [HttpPost]
        public ActionResult Edit(ACCOUNT aCCOUNT)
        {
            if (ModelState.IsValid)
            {
                if (new LoginDAO().Edit(aCCOUNT))
                {
                    SetAlert("Đã sửa thành công!", "success");
                }
            }
            else
            {
                SetAlert("Chỉnh sửa không thành công, vui lòng thử lại!", "warning");
            }
            return Redirect("/" + aCCOUNT.ACCOUNT_Username + "/");
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
                        SetAlert("Đã thay đổi ảnh đại diện thành công!", "success");
                    }
                }
                else
                {
                    SetAlert("Thay đổi ảnh đại diện không thành công, vui lòng thử lại!", "warning");

                }
            }
            return RedirectToAction("Index", ACCOUNT_Username + "/");
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
            return RedirectToAction("Index", ACCOUNT_Username + "/");
        }
    }
}