using System;
using System.Collections.Generic;
using System.Linq;
using UniversityMagazine.EF;

namespace UniversityMagazine.Areas.Management.DAO
{
    public class FacultyDAO
    {
        UniversityMagazineDBContext db = null;
        public FacultyDAO()
        {
            db = new UniversityMagazineDBContext();
        }

        public FACULTY GetById(Guid fACULTY_Id)
        {
            return db.FACULTies.Find(fACULTY_Id);
        }

        public IEnumerable<FACULTY> ListAll()
        {
            IEnumerable<FACULTY> model = db.FACULTies;
            return model;
        }

        public bool Create(FACULTY fACULTy)
        {
            try
            {
                Guid id = Guid.NewGuid();
                fACULTy.FACULTY_Id = id;
                fACULTy.FACULTY_Code = GetCode(fACULTy.FACULTY_Name).ToUpper();
                db.FACULTies.Add(fACULTy);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool ChangeStatus(Guid? id)
        {
            var item = db.FACULTies.Find(id);
            item.FACULTY_Status = !item.FACULTY_Status;
            db.SaveChanges();

            return item.FACULTY_Status;
        }
        public string GetCode(string Faculty_Name)
        {
            string Code = null;
            foreach (var item in Faculty_Name.Split(' '))
            {
                Code += item.Substring(0, 1);
            }
            return Code;
        }
        public IEnumerable<ARTICLE> ArticlesApproved(string fACULTY_Code, string month = null, string year = null)
        {
            IQueryable<ARTICLE> model = db.ARTICLEs.Where(x => x.FACULTY.FACULTY_Code == fACULTY_Code && x.ARTICLE_Status == true);
            if (month != null && month != "" && year != null && year != "")
            {
                model = model.Where(x => x.ARTICLE_UploadTime.Value.Month.ToString() == month && x.ARTICLE_UploadTime.Value.Year.ToString() == year);
            }
            return model;
        } 
        public IEnumerable<IMAGE> ImagesApproved(string fACULTY_Code)
        {
            IQueryable<IMAGE> model = db.IMAGEs.Where(x => x.FACULTY.FACULTY_Code == fACULTY_Code && x.IMAGE_Status == true);
            return model;
        }
        public bool Edit(FACULTY fACULTy)
        {
            try
            {
                var data = db.FACULTies.Find(fACULTy.FACULTY_Id);
                data.FACULTY_Code = GetCode(fACULTy.FACULTY_Name).ToUpper();
                data.FACULTY_Name = fACULTy.FACULTY_Name;
                data.FACULTY_Descriptions = fACULTy.FACULTY_Descriptions;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Delete(Guid? Id)
        {
            try
            {
                var article = db.FACULTies.Where(x => x.FACULTY_Id == Id).SingleOrDefault();
                db.FACULTies.Remove(article);
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