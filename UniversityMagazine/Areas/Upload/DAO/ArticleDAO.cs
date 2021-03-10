using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UniversityMagazine.Areas.Management.DAO;
using UniversityMagazine.EF;

namespace UniversityMagazine.Areas.Upload.DAO
{
    public class ArticleDAO
    {
        UniversityMagazineDBContext db = null;
        public ArticleDAO()
        {
            db = new UniversityMagazineDBContext();
        }
        public ARTICLE GetById(Guid? aRTICLE_Id)
        {
            return db.ARTICLEs.Find(aRTICLE_Id);
        }

        public ARTICLE GetByFileName(Guid UserID, string aRTICLE_FileName)
        {
            Guid? fACULTY_Id = new AccountDAO().GetById(UserID).FACULTY_Id;
            return db.ARTICLEs.SingleOrDefault(x => x.ARTICLE_FileName == aRTICLE_FileName && x.FACULTY_Id == fACULTY_Id);
        }
        public IEnumerable<ARTICLE> MyUpload(Guid aCCOUNT_Id, string filter = null, string ARTICLES_UploadTimeStart = null, string ARTICLES_UploadTimeFinish = null)
        {
            IEnumerable<ARTICLE> model = db.ARTICLEs.Where(x => x.ACCOUNT_Id == aCCOUNT_Id).OrderByDescending(x => x.ARTICLE_UploadTime);
            if (filter != null && filter != "")
            {
                if (ARTICLES_UploadTimeStart != null && ARTICLES_UploadTimeFinish == null)
                {
                    DateTime sdate = (ARTICLES_UploadTimeStart != "") ? Convert.ToDateTime(ARTICLES_UploadTimeStart).Date : new DateTime();
                    model = model.Where(x => x.ARTICLE_UploadTime == sdate);

                }
                else if (ARTICLES_UploadTimeStart == null && ARTICLES_UploadTimeFinish != null)
                {
                    DateTime edate = (ARTICLES_UploadTimeFinish != "") ? Convert.ToDateTime(ARTICLES_UploadTimeFinish).Date : new DateTime();
                    model = model.Where(x => x.ARTICLE_UploadTime == edate);

                }
                else if (ARTICLES_UploadTimeStart != null && ARTICLES_UploadTimeFinish != null)
                {
                    DateTime sdate = (ARTICLES_UploadTimeStart != "") ? Convert.ToDateTime(ARTICLES_UploadTimeStart).Date : new DateTime();
                    DateTime edate = (ARTICLES_UploadTimeFinish != "") ? Convert.ToDateTime(ARTICLES_UploadTimeFinish).Date : new DateTime();
                    model = model.Where(x => x.ARTICLE_UploadTime >= sdate && x.ARTICLE_UploadTime <= edate);
                }
            }
            return model;
        }

        public IEnumerable<ARTICLE> StudentsUpload(Guid aCCOUNT_Id, string filter = null, string ARTICLES_UploadTimeStart = null, string ARTICLES_UploadTimeFinish = null)
        {
            Guid? fACULTY_Id = new AccountDAO().GetById(aCCOUNT_Id).FACULTY_Id;
            IEnumerable<ARTICLE> model = db.ARTICLEs.Where(x => x.FACULTY_Id == fACULTY_Id).OrderByDescending(x => x.ARTICLE_UploadTime);
            if (filter != null && filter != "")
            {
                if (ARTICLES_UploadTimeStart != null && ARTICLES_UploadTimeFinish == null)
                {
                    DateTime sdate = (ARTICLES_UploadTimeStart != "") ? Convert.ToDateTime(ARTICLES_UploadTimeStart).Date : new DateTime();
                    model = model.Where(x => x.ARTICLE_UploadTime == sdate);

                }
                else if (ARTICLES_UploadTimeStart == null && ARTICLES_UploadTimeFinish != null)
                {
                    DateTime edate = (ARTICLES_UploadTimeFinish != "") ? Convert.ToDateTime(ARTICLES_UploadTimeFinish).Date : new DateTime();
                    model = model.Where(x => x.ARTICLE_UploadTime == edate);

                }
                else if (ARTICLES_UploadTimeStart != null && ARTICLES_UploadTimeFinish != null)
                {
                    DateTime sdate = (ARTICLES_UploadTimeStart != "") ? Convert.ToDateTime(ARTICLES_UploadTimeStart).Date : new DateTime();
                    DateTime edate = (ARTICLES_UploadTimeFinish != "") ? Convert.ToDateTime(ARTICLES_UploadTimeFinish).Date : new DateTime();
                    model = model.Where(x => x.ARTICLE_UploadTime >= sdate && x.ARTICLE_UploadTime <= edate);
                }
            }
            return model;
        }


        public string GetCode(string aRTICLES_Tittle)
        {
            string Code = null;
            foreach (var item in aRTICLES_Tittle.Split(' '))
            {
                Code += item.Substring(0, 1);
            }
            var a = DateTime.Now.Date;
            var num = db.ARTICLEs.Where(x => DbFunctions.TruncateTime(x.ARTICLE_UploadTime) == a).Count();
            if (num == 0)
            {
                Code += "001" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy");
            }
            else if (num > 0 && num < 9)
            {
                Code += "00" + (num + 1).ToString() + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy");
            }
            else if (num > 9 && num < 99)
            {
                Code += "0" + (num + 1).ToString() + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy");
            }
            else if (num > 99 && num < 999)
            {
                Code += (num + 1).ToString() + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy");
            }
            return Code;
        }

        public bool Create(ARTICLE aRTICLE)
        {
            try
            {
                aRTICLE.ARTICLE_UploadTime = DateTime.Now;
                aRTICLE.ARTICLE_Status = false;
                db.ARTICLEs.Add(aRTICLE);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public bool Edit(ARTICLE aRTICLE, Guid? aCCOUNT_Id)
        {
            try
            {
                EDITINGHISTORY log = new EDITINGHISTORY
                {
                    EDITINGHISTORY_Id = Guid.NewGuid(),
                    ARTICLE_Id = aRTICLE.ARTICLE_Id,
                    ACCOUNT_Id = aCCOUNT_Id
                };

                ARTICLE data = db.ARTICLEs.Find(aRTICLE.ARTICLE_Id);
                log.EDITINGHISTORY_Content = JsonConvert.SerializeObject(new ARTICLE(data));
                data.ARTICLE_Type = aRTICLE.ARTICLE_Type;
                data.ARTICLE_Size = aRTICLE.ARTICLE_Size;
                data.ARTICLE_EditTime = DateTime.Now;
                data.ARTICLE_Tittle = aRTICLE.ARTICLE_Tittle;
                if (aRTICLE.ARTICLE_FileUpload != null && aRTICLE.ARTICLE_FileUpload != "")
                {
                    data.ARTICLE_FileUpload = aRTICLE.ARTICLE_FileUpload;

                }
                db.EDITINGHISTORies.Add(log);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Guid aRTICLES_Id)
        {
            try
            {
                var article = db.ARTICLEs.Find(aRTICLES_Id);
                db.ARTICLEs.Remove(article);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool? ChangeStatus(Guid? id)
        {
            var item = db.ARTICLEs.Find(id);
            item.ARTICLE_Status = !item.ARTICLE_Status;
            db.SaveChanges();

            return item.ARTICLE_Status;
        }

    }
}