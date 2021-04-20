using EntityModels.EF;
using EntityModels.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using UniversityMagazine.Common;
using UniversityMagazine.DAO;

namespace UniversityMagazine.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetDataChart()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            UniversityMagazineDBContext db = new UniversityMagazineDBContext();
            if (session.RoleGroup == "MARKETINGMANAGER")
            {
                List<string> faculty = new List<string>();
                List<int> article = new List<int>();
                List<int> articleApprove = new List<int>();
                List<int> image = new List<int>();
                List<int> imageApprove = new List<int>();
                foreach (var item in db.FACULTies)
                {
                    faculty.Add(item.FACULTY_Code);
                    article.Add(db.ARTICLEs.Where(x => x.FACULTY_Id == item.FACULTY_Id).Count());
                    articleApprove.Add(db.ARTICLEs.Where(x => x.FACULTY_Id == item.FACULTY_Id && x.ARTICLE_Status == true).Count());
                    image.Add(db.IMAGEs.Where(x => x.FACULTY_Id == item.FACULTY_Id).Count());
                    imageApprove.Add(db.IMAGEs.Where(x => x.FACULTY_Id == item.FACULTY_Id && x.IMAGE_Status == true).Count());
                }
                return Json(new { faculty = faculty, article = article, articleApprove = articleApprove, image = image, imageApprove = imageApprove }, JsonRequestBehavior.AllowGet);
            }
            else if (session.RoleGroup == "MARKETINGCOORDINATOR")
            {
                var Article = db.ARTICLEs.Where(x => x.FACULTY_Id == db.ACCOUNTs.FirstOrDefault(y => y.ACCOUNT_Id == session.UserID).FACULTY_Id).Count();
                var ArticleApproved = db.ARTICLEs.Where(x => x.FACULTY_Id == db.ACCOUNTs.FirstOrDefault(y => y.ACCOUNT_Id == session.UserID).FACULTY_Id && x.ARTICLE_Status == true).Count();
                var ArticleUnapproved = db.ARTICLEs.Where(x => x.FACULTY_Id == db.ACCOUNTs.FirstOrDefault(y => y.ACCOUNT_Id == session.UserID).FACULTY_Id && x.ARTICLE_Status == false).Count();
                var Image = db.IMAGEs.Where(x => x.FACULTY_Id == db.ACCOUNTs.FirstOrDefault(y => y.ACCOUNT_Id == session.UserID).FACULTY_Id).Count();
                var ImageApproved = db.IMAGEs.Where(x => x.FACULTY_Id == db.ACCOUNTs.FirstOrDefault(y => y.ACCOUNT_Id == session.UserID).FACULTY_Id && x.IMAGE_Status == true).Count();
                var ImageUnapproved = db.IMAGEs.Where(x => x.FACULTY_Id == db.ACCOUNTs.FirstOrDefault(y => y.ACCOUNT_Id == session.UserID).FACULTY_Id && x.IMAGE_Status == false).Count();
                object[] parameters =
            {
                new SqlParameter("@faculty", db.ACCOUNTs.FirstOrDefault(y => y.ACCOUNT_Id == session.UserID).FACULTY_Id),
            };
                var res = db.Database.SqlQuery<Top3AccountUploadModel>("Top3AccountUpload @faculty", parameters).ToList();
                List<Top3AccountUploadModel> topUploadArticle = new List<Top3AccountUploadModel>();
                foreach (var item in res)
                {
                    topUploadArticle.Add(item);
                }
                return Json(new { Article = Article, ArticleApprove = ArticleApproved, ArticleUnapproved = ArticleUnapproved, Image = Image, ImageApproved = ImageApproved, ImageUnapproved = ImageUnapproved, topUploadArticle = topUploadArticle }, JsonRequestBehavior.AllowGet);
            }
            else if (session.RoleGroup == "STUDENT")
            {
                var Article = db.ARTICLEs.Where(x=>x.ACCOUNT_Id == session.UserID).Count();
                var ArticleApproved = db.ARTICLEs.Where(x => x.ACCOUNT_Id == session.UserID && x.ARTICLE_Status == true).Count();
                var ArticleUnapproved = db.ARTICLEs.Where(x => x.ACCOUNT_Id == session.UserID && x.ARTICLE_Status == false).Count();
                var Image = db.IMAGEs.Where(x => x.ACCOUNT_Id == session.UserID).Count();
                var ImageApproved = db.IMAGEs.Where(x => x.ACCOUNT_Id == session.UserID && x.IMAGE_Status == true).Count();
                var ImageUnapproved = db.IMAGEs.Where(x => x.ACCOUNT_Id == session.UserID && x.IMAGE_Status == false).Count();
                return Json(new { Article = Article, ArticleApprove = ArticleApproved, ArticleUnapproved = ArticleUnapproved, Image = Image, ImageApproved = ImageApproved, ImageUnapproved = ImageUnapproved}, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);

            }
        }

    }
}