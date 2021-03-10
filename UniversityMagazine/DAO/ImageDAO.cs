using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using UniversityMagazine.EF;

namespace UniversityMagazine.DAO
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

        public IMAGE GetByFileName(Guid aCCOUNT_Id, string IMAGE_FileName)
        {
            return db.IMAGEs.SingleOrDefault(x => x.IMAGE_FileName == IMAGE_FileName && x.ACCOUNT_Id == aCCOUNT_Id);
        }
        public IEnumerable<IMAGE> MyUpload(Guid aCCOUNT_Id, string filter = null, string IMAGE_UploadTimeStart = null, string CONGVIEC_NgayKetThucStart = null)
        {
            IEnumerable<IMAGE> model = db.IMAGEs.Where(x => x.ACCOUNT_Id == aCCOUNT_Id).OrderByDescending(x => x.IMAGE_UploadTime);
            if (filter != null && filter != "")
            {
                if (IMAGE_UploadTimeStart != null && CONGVIEC_NgayKetThucStart == null)
                {
                    DateTime sdate = (IMAGE_UploadTimeStart != "") ? Convert.ToDateTime(IMAGE_UploadTimeStart).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime == sdate);

                }
                else if (IMAGE_UploadTimeStart == null && CONGVIEC_NgayKetThucStart != null)
                {
                    DateTime edate = (CONGVIEC_NgayKetThucStart != "") ? Convert.ToDateTime(CONGVIEC_NgayKetThucStart).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime == edate);

                }
                else if (IMAGE_UploadTimeStart != null && CONGVIEC_NgayKetThucStart != null)
                {
                    DateTime sdate = (IMAGE_UploadTimeStart != "") ? Convert.ToDateTime(IMAGE_UploadTimeStart).Date : new DateTime();
                    DateTime edate = (CONGVIEC_NgayKetThucStart != "") ? Convert.ToDateTime(CONGVIEC_NgayKetThucStart).Date : new DateTime();
                    model = model.Where(x => x.IMAGE_UploadTime >= sdate && x.IMAGE_UploadTime <= edate);
                }
            }
            else
            {
                model = model.Take(50);
            }
            return model;
        }


        public string GetCode(string aCCOUNT_Name)
        {
            string Code = null;
            foreach (var item in aCCOUNT_Name.Split(' '))
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

        public bool Create(IMAGE iMAGES)
        {
            try
            {
                iMAGES.IMAGE_UploadTime = DateTime.Now;
                iMAGES.IMAGE_Status = false;
                db.IMAGEs.Add(iMAGES);
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


        }

        public bool Delete(Guid iMage_Id)
        {
            try
            {
                var image = db.IMAGEs.Find(iMage_Id);
                db.IMAGEs.Remove(image);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}