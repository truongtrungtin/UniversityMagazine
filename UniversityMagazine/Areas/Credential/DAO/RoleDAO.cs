using System;
using System.Collections.Generic;
using System.Linq;
using UniversityMagazine.EF;

namespace UniversityMagazine.Areas.Credential.DAO
{
    public class RoleDAO
    {
        UniversityMagazineDBContext db = null;
        public RoleDAO()
        {
            db = new UniversityMagazineDBContext();
        }

        public ROLE GetById(Guid fORMNAME_Id)
        {
            return db.ROLEs.Find(fORMNAME_Id);
        }

        public IEnumerable<ROLE> ListAll()
        {
            IEnumerable<ROLE> model = db.ROLEs;
            return model;
        }

        public bool Create(ROLE rOLE)
        {
            try
            {
                Guid id = Guid.NewGuid();
                rOLE.ROLE_Id = id;
                rOLE.ROLE_Code = rOLE.ROLE_Code.ToUpper();
                db.ROLEs.Add(rOLE);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public bool Edit(ROLE rOLE)
        {
            try
            {
                var data = db.ROLEs.Find(rOLE.ROLE_Id);
                data.ROLE_Code = rOLE.ROLE_Code.ToUpper();
                data.ROLE_Name = rOLE.ROLE_Name;
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
                    var article = db.ROLEs.Where(x => x.ROLE_Id == temp).SingleOrDefault();
                    db.ROLEs.Remove(article);
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