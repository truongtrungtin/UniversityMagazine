using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        #region Random
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        #endregion

        public string GetUsername(string aCCOUNT_Name)
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
        public ACCOUNT Create(ACCOUNT aCCOUNT)
        {
            aCCOUNT.ACCOUNT_Password = GetMD5(RandomPassword());
            aCCOUNT.ACCOUNT_Username = GetUsername(aCCOUNT.ACCOUNT_Name);
            aCCOUNT.ACCOUNT_CreateTime = DateTime.Now;
            aCCOUNT.ACCOUNT_Status = true;
            aCCOUNT.ACCOUNT_Avatar = "/Content/dist/img/Avatar.png";
            var user = db.ACCOUNTs.Add(aCCOUNT);
            db.SaveChanges();
            return user;


        }

        public bool Edit(ACCOUNT aCCOUNT)
        {
            try
            {
                var data = db.ACCOUNTs.SingleOrDefault(x => x.ACCOUNT_Username == aCCOUNT.ACCOUNT_Username);
                data.ACCOUNT_Name = aCCOUNT.ACCOUNT_Name;
                data.ACCOUNT_BirthDay = aCCOUNT.ACCOUNT_BirthDay;
                data.ACCOUNT_Telephone = aCCOUNT.ACCOUNT_Telephone;
                data.ACCOUNT_Email = aCCOUNT.ACCOUNT_Email;
                data.ACCOUNT_Address = aCCOUNT.ACCOUNT_Address;
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
        public bool Delete(string aCCOUNT_Username)
        {
            try
            {
                var user = db.ACCOUNTs.SingleOrDefault(x => x.ACCOUNT_Username == aCCOUNT_Username);
                db.ACCOUNTs.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<ACCOUNT> GetAccountWithCreated(Guid CREATOR)
        {
            return db.ACCOUNTs.Where(x => x.ACCOUNT_CREATOR == CREATOR).OrderByDescending(x => x.ACCOUNT_CreateTime);
        }
    }
}