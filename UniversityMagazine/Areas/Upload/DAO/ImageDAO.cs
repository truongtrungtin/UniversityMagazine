using EntityModels.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UniversityMagazine.Areas.Management.DAO;

namespace UniversityMagazine.Areas.Upload.DAO
{
    public class ImageDAO
    {
        UniversityMagazineDBContext db = null;
        public ImageDAO()
        {
            db = new UniversityMagazineDBContext();
        }
        public IMAGE GetById(Guid? iMAGE_Id)
        {
            return db.IMAGEs.Find(iMAGE_Id);
        }

        public IMAGE GetByFileName(Guid UserID, string iMAGE_FileName)
        {
            Guid? fACULTY_Id = new AccountDAO().GetById(UserID).FACULTY_Id;
            return db.IMAGEs.SingleOrDefault(x => x.IMAGE_FileName == iMAGE_FileName && x.FACULTY_Id == fACULTY_Id);
        }
        public IEnumerable<IMAGE> MyUpload(Guid aCCOUNT_Id, string filter = null, string IMAGES_UploadTimeStart = null, string IMAGES_UploadTimeFinish = null)
        {
            IEnumerable<IMAGE> model = db.IMAGEs.Where(x => x.ACCOUNT_Id == aCCOUNT_Id).OrderByDescending(x => x.IMAGE_UploadTime);
            if (filter != null && filter != "")
            {
                if (IMAGES_UploadTimeStart != null && IMAGES_UploadTimeFinish == null)
                {
                    DateTime sdate = (IMAGES_UploadTimeStart != "") ? Convert.ToDateTime(IMAGES_UploadTimeStart).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime == sdate);

                }
                else if (IMAGES_UploadTimeStart == null && IMAGES_UploadTimeFinish != null)
                {
                    DateTime edate = (IMAGES_UploadTimeFinish != "") ? Convert.ToDateTime(IMAGES_UploadTimeFinish).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime == edate);

                }
                else if (IMAGES_UploadTimeStart != null && IMAGES_UploadTimeFinish != null)
                {
                    DateTime sdate = (IMAGES_UploadTimeStart != "") ? Convert.ToDateTime(IMAGES_UploadTimeStart).Date : new DateTime();
                    DateTime edate = (IMAGES_UploadTimeFinish != "") ? Convert.ToDateTime(IMAGES_UploadTimeFinish).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime >= sdate && x.IMAGE_UploadTime <= edate);
                }
            }
            return model;
        }

        public IEnumerable<IMAGE> StudentsUpload(Guid aCCOUNT_Id, string filter = null, string IMAGES_UploadTimeStart = null, string IMAGES_UploadTimeFinish = null)
        {
            Guid? fACULTY_Id = new AccountDAO().GetById(aCCOUNT_Id).FACULTY_Id;
            IEnumerable<IMAGE> model = db.IMAGEs.Where(x => x.FACULTY_Id == fACULTY_Id).OrderByDescending(x => x.IMAGE_UploadTime);
            if (filter != null && filter != "")
            {
                if (IMAGES_UploadTimeStart != null && IMAGES_UploadTimeFinish == null)
                {
                    DateTime sdate = (IMAGES_UploadTimeStart != "") ? Convert.ToDateTime(IMAGES_UploadTimeStart).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime == sdate);

                }
                else if (IMAGES_UploadTimeStart == null && IMAGES_UploadTimeFinish != null)
                {
                    DateTime edate = (IMAGES_UploadTimeFinish != "") ? Convert.ToDateTime(IMAGES_UploadTimeFinish).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime == edate);

                }
                else if (IMAGES_UploadTimeStart != null && IMAGES_UploadTimeFinish != null)
                {
                    DateTime sdate = (IMAGES_UploadTimeStart != "") ? Convert.ToDateTime(IMAGES_UploadTimeStart).Date : new DateTime();
                    DateTime edate = (IMAGES_UploadTimeFinish != "") ? Convert.ToDateTime(IMAGES_UploadTimeFinish).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime >= sdate && x.IMAGE_UploadTime <= edate);
                }
            }
            return model;
        }


        public string GetCode(string iMAGES_Tittle)
        {
            string Code = null;
            foreach (var item in iMAGES_Tittle.Split(' '))
            {
                Code += item.Substring(0, 1);
            }
            var a = DateTime.Now.Date;
            var num = db.IMAGEs.Where(x => DbFunctions.TruncateTime(x.IMAGE_UploadTime) == a).Count();
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

        public bool Create(IMAGE iMAGE)
        {
            try
            {
                iMAGE.IMAGE_UploadTime = DateTime.Now;
                iMAGE.IMAGE_Status = false;
                db.IMAGEs.Add(iMAGE);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public bool Edit(IMAGE iMAGE)
        {
            try
            {

                IMAGE data = db.IMAGEs.Find(iMAGE.IMAGE_Id);
                data.IMAGE_Type = iMAGE.IMAGE_Type;
                data.IMAGE_Size = iMAGE.IMAGE_Size;
                data.IMAGE_Tittle = iMAGE.IMAGE_Tittle;
                if (iMAGE.IMAGE_FileUpload != null && iMAGE.IMAGE_FileUpload != "")
                {
                    data.IMAGE_FileUpload = iMAGE.IMAGE_FileUpload;

                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Guid iMAGES_Id)
        {
            try
            {
                var article = db.IMAGEs.Find(iMAGES_Id);
                db.IMAGEs.Remove(article);
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
            var item = db.IMAGEs.Find(id);
            item.IMAGE_Status = !item.IMAGE_Status;
            db.SaveChanges();

            return item.IMAGE_Status;
        }

    }
}