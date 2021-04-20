using EntityModels.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityMagazine.Areas.Credential.DAO
{
    public class RoleGroupDAO
    {

        UniversityMagazineDBContext db = null;
        public RoleGroupDAO()
        {
            db = new UniversityMagazineDBContext();
        }
        public ROLEGROUP GetById(Guid nHOMQUYEN_Id)
        {
            return db.ROLEGROUPs.Find(nHOMQUYEN_Id);
        }

        public IEnumerable<CREDENTIAL> GetCredentialByRoleGroup(Guid roleGroup_Id)
        {
            return db.CREDENTIALs.Where(x => x.ROLEGROUP_Id == roleGroup_Id);
        }

        public IEnumerable<CREDENTIAL> GetCredential()
        {
            return db.CREDENTIALs;
        }

        public IEnumerable<ROLEGROUP> ListAll()
        {
            IEnumerable<ROLEGROUP> model = db.ROLEGROUPs;
            return model;
        }

        public bool Create(ROLEGROUP rOLEGROUP)
        {
            try
            {
                Guid id = Guid.NewGuid();
                rOLEGROUP.ROLEGROUP_Id = id;
                rOLEGROUP.ROLEGROUP_Code = rOLEGROUP.ROLEGROUP_Code.ToUpper();
                db.ROLEGROUPs.Add(rOLEGROUP);
                db.SaveChanges();
                AddRoletoRoleGroup(id);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        public bool Edit(ROLEGROUP rOLEGROUP)
        {
            try
            {
                var data = db.ROLEGROUPs.Find(rOLEGROUP.ROLEGROUP_Id);
                data.ROLEGROUP_Code = rOLEGROUP.ROLEGROUP_Code.ToUpper();
                data.ROLEGROUP_Name = rOLEGROUP.ROLEGROUP_Name;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool EditCredential(HttpRequestBase request)
        {
            try
            {
                Guid a = Guid.Parse(request.Form["ROLEGROUP_Id"]);
                var data = db.CREDENTIALs.Where(x => x.ROLEGROUP_Id == a).ToList();
                foreach (var item in data)
                {
                    item.CREDENTIAL_VIEW = Boolean.Parse(request.Form[item.ROLEGROUP_Id + "-" + item.ROLE_Id + "-CREDENTIAL_VIEW"] == "on" ? "True" : "False"); ;
                    item.CREDENTIAL_ADD = Boolean.Parse(request.Form[item.ROLEGROUP_Id + "-" + item.ROLE_Id + "-CREDENTIAL_ADD"] == "on" ? "True" : "False");
                    item.CREDENTIAL_EDIT = Boolean.Parse(request.Form[item.ROLEGROUP_Id + "-" + item.ROLE_Id + "-CREDENTIAL_EDIT"] == "on" ? "True" : "False");
                    item.CREDENTIAL_DELETE = Boolean.Parse(request.Form[item.ROLEGROUP_Id + "-" + item.ROLE_Id + "-CREDENTIAL_DELETE"] == "on" ? "True" : "False");
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void AddRoletoRoleGroup(Guid rOLEGROUP_Id)
        {

            foreach (var item in db.ROLEs)
            {
                CREDENTIAL cREDENTIAL = new CREDENTIAL();
                cREDENTIAL.CREDENTIAL_Id = Guid.NewGuid();
                cREDENTIAL.ROLE_Id = item.ROLE_Id;
                cREDENTIAL.ROLEGROUP_Id = rOLEGROUP_Id;
                cREDENTIAL.CREDENTIAL_VIEW = false;
                cREDENTIAL.CREDENTIAL_DELETE = false;
                cREDENTIAL.CREDENTIAL_EDIT = false;
                cREDENTIAL.CREDENTIAL_ADD = false;
                db.CREDENTIALs.Add(cREDENTIAL);
            }
            db.SaveChanges();

        }

        public bool Delete(Guid[] chkId)
        {
            try
            {
                for (int i = 0; i < chkId.Length; i++)
                {
                    Guid temp = chkId[i];
                    var article = db.ROLEGROUPs.Where(x => x.ROLEGROUP_Id == temp).SingleOrDefault();
                    db.ROLEGROUPs.Remove(article);
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