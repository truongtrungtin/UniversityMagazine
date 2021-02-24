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
                db.FACULTies.Add(fACULTy);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public bool Edit(FACULTY fACULTy)
        {
            try
            {
                var data = db.FACULTies.Find(fACULTy.FACULTY_Id);
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

        public bool Delete(Guid[] chkId)
        {
            try
            {
                for (int i = 0; i < chkId.Length; i++)
                {
                    Guid temp = chkId[i];
                    var article = db.FACULTies.Where(x => x.FACULTY_Id == temp).SingleOrDefault();
                    db.FACULTies.Remove(article);
                }
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