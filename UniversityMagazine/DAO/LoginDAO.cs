using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using UniversityMagazine.Common;
using UniversityMagazine.EF;

namespace UniversityMagazine.DAO
{
    public class LoginDAO
    {
        UniversityMagazineDBContext db = null;
        public LoginDAO()
        {
            db = new UniversityMagazineDBContext();
        }
        public ACCOUNT GetById(Guid aCCOUNT_Id)
        {
            return db.ACCOUNTs.Find(aCCOUNT_Id);
        }

        public string GetMD5(string password)
        {
            string password_md5 = "";
            byte[] array = System.Text.Encoding.UTF8.GetBytes(password + "hn");

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            array = my_md5.ComputeHash(array);

            foreach (byte b in array)
            {
                password_md5 += b.ToString("X2");
            }

            return password_md5;
        }

        public bool CheckConnectDatabase()
        {
            try
            {
                db.Database.Exists();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdatePassword(ACCOUNT aCCOUNT)
        {
            try
            {
                var data = db.ACCOUNTs.Find(aCCOUNT.ACCOUNT_Id);
                data.ACCOUNT_Password = aCCOUNT.ACCOUNT_Password;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public ACCOUNT GetByUsername(string aCCOUNT_Username)
        {
            return db.ACCOUNTs.SingleOrDefault(x => x.ACCOUNT_Username == aCCOUNT_Username);
        }


        public List<Credential> GetListCredential(Guid aCCOUNT_Id)
        {
            var data = (from a in db.Roles
                        join b in db.CREDENTIALs on a.ROLE_Id equals b.ROLE_Id
                        join c in db.ROLEGROUPs on b.ROLEGROUP_Id equals c.ROLEGROUP_Id
                        join d in db.ACCOUNTs on c.ROLEGROUP_Id equals d.ROLEGROUP_Id
                        where d.ACCOUNT_Id == aCCOUNT_Id
                        select new Credential()
                        {
                            ACCOUNT_ID = d.ACCOUNT_Id,
                            ROLEGROUP_Id = c.ROLEGROUP_Id,
                            ROLE_Id = a.ROLE_Id,
                            ROLEGROUP_Name = c.ROLEGROUP_Name,
                            ROLE_Code = a.ROLE_Code,
                            ROLE_Name = a.ROLE_Name,
                            CREDENTIAL_VIEW = b.CREDENTIAL_VIEW,
                            CREDENTIAL_ADD = b.CREDENTIAL_ADD,
                            CREDENTIAL_EDIT = b.CREDENTIAL_EDIT,
                            CREDENTIAL_DELETE = b.CREDENTIAL_DELETE,
                        });
            return data.ToList();

        }

        public bool CheckPassword(Guid aCCOUNT_Id, string aCCOUNT_Password)
        {

            if (db.ACCOUNTs.Where(x => x.ACCOUNT_Id == aCCOUNT_Id && x.ACCOUNT_Password == aCCOUNT_Password).Count() > 0)
            {
                return true;
            }
            return false;

        }

        public int Login(string aCCOUNT_Username, string aCCOUNT_Password)
        {
            var result = db.ACCOUNTs.SingleOrDefault(x => x.ACCOUNT_Username == aCCOUNT_Username);
            if (result == null)
            {
                return 0;
            }
            else
            {

                if (result.ACCOUNT_Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.ACCOUNT_Password == aCCOUNT_Password)
                        return 1;
                    else
                        return -2;
                }
            }
        }
    }
}