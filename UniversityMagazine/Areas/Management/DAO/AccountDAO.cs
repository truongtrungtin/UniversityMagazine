using EntityModels.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using UniversityMagazine.Common;

namespace UniversityMagazine.Areas.Management.DAO
{
    public class AccountDAO
    {
        UniversityMagazineDBContext db = null;
        public AccountDAO()
        {
            db = new UniversityMagazineDBContext();
        }
        public ACCOUNT GetById(Guid? aCCOUNT_Id)
        {
            return db.ACCOUNTs.Find(aCCOUNT_Id);
        }
        public ACCOUNT GetByEmail(string email)
        {
            return db.ACCOUNTs.SingleOrDefault(x => x.ACCOUNT_Email == email);
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


        public List<Credentials> GetListCredential(Guid aCCOUNT_Id)
        {
            var data = (from a in db.ROLEs
                        join b in db.CREDENTIALs on a.ROLE_Id equals b.ROLE_Id
                        join c in db.ROLEGROUPs on b.ROLEGROUP_Id equals c.ROLEGROUP_Id
                        join d in db.ACCOUNTs on c.ROLEGROUP_Id equals d.ROLEGROUP_Id
                        where d.ACCOUNT_Id == aCCOUNT_Id
                        select new Credentials()
                        {
                            ACCOUNT_ID = d.ACCOUNT_Id,
                            ROLEGROUP_Id = c.ROLEGROUP_Id,
                            ROLE_Id = a.ROLE_Id,
                            ROLEGROUP_Code = c.ROLEGROUP_Code,
                            ROLE_Code = a.ROLE_Code,
                            ROLE_Name = a.ROLE_Name,
                            CREDENTIAL_VIEW = b.CREDENTIAL_VIEW,
                            CREDENTIAL_ADD = b.CREDENTIAL_ADD,
                            CREDENTIAL_EDIT = b.CREDENTIAL_EDIT,
                            CREDENTIAL_DELETE = b.CREDENTIAL_DELETE,
                        });
            return data.ToList();

        }

        public bool CheckPassword(string aCCOUNT_Username, string aCCOUNT_Password)
        {

            if (db.ACCOUNTs.Where(x => x.ACCOUNT_Username == aCCOUNT_Username && x.ACCOUNT_Password == aCCOUNT_Password).Count() > 0)
            {
                return true;
            }
            return false;

        }



        public string GetUsername(string aCCOUNT_Name)
        {
            string Code = null;
            foreach (var item in aCCOUNT_Name.Split(' '))
            {
                Code += item.Substring(0, 1);
            }
            var a = DateTime.Now.Date;
            var num = db.ACCOUNTs.Where(x => DbFunctions.TruncateTime(x.ACCOUNT_CreateTime) == a).Count();
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
        public ACCOUNT Create(ACCOUNT aCCOUNT)
        {
            aCCOUNT.ACCOUNT_Id = Guid.NewGuid();
            aCCOUNT.ACCOUNT_Password = GetMD5(new StringHelper().RandomPassword());
            aCCOUNT.ACCOUNT_Username = GetUsername(new StringHelper().RemoveUnicode(aCCOUNT.ACCOUNT_Name));
            aCCOUNT.ACCOUNT_CreateTime = DateTime.Now;
            aCCOUNT.ACCOUNT_Avatar = "/Content/dist/img/Avatar.png";
            var user = db.ACCOUNTs.Add(aCCOUNT);
            db.SaveChanges();
            return user;


        }

        public bool Edit(ACCOUNT aCCOUNT)
        {
            try
            {
                var data = db.ACCOUNTs.Find(aCCOUNT.ACCOUNT_Id);
                data.ACCOUNT_Name = aCCOUNT.ACCOUNT_Name;
                data.ACCOUNT_BirthDay = aCCOUNT.ACCOUNT_BirthDay;
                data.ACCOUNT_Telephone = aCCOUNT.ACCOUNT_Telephone;
                data.ACCOUNT_Email = aCCOUNT.ACCOUNT_Email;
                data.ACCOUNT_Address = aCCOUNT.ACCOUNT_Address;
                data.ACCOUNT_Status = aCCOUNT.ACCOUNT_Status;
                data.ACCOUNT_EditTime = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Avatar(ACCOUNT aCCOUNT)
        {
            try
            {
                var data = db.ACCOUNTs.SingleOrDefault(x => x.ACCOUNT_Username == aCCOUNT.ACCOUNT_Username);
                data.ACCOUNT_Avatar = aCCOUNT.ACCOUNT_Avatar;
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
                    var article = db.ACCOUNTs.Find(temp);
                    db.ACCOUNTs.Remove(article);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public ACCOUNT GetAccountCoordinator(Guid? Faculty)
        {
            return db.ACCOUNTs.FirstOrDefault(x => x.FACULTY_Id == Faculty && x.ROLEGROUP.ROLEGROUP_Code == "MARKETINGCOORDINATOR");
        }

        public ACCOUNT GetAccountWithCreated(Guid? CREATOR, Guid? aCCOUNT_Id)
        {
            return db.ACCOUNTs.SingleOrDefault(x => x.ACCOUNT_CREATOR == CREATOR && x.ACCOUNT_Id == aCCOUNT_Id);
        }
        public IEnumerable<ACCOUNT> GetAccountsWithCreated(Guid? CREATOR)
        {
            return db.ACCOUNTs.Where(x => x.ACCOUNT_CREATOR == CREATOR).OrderByDescending(x => x.ACCOUNT_CreateTime);
        }
    }
}