using System;
using System.Collections.Generic;
using System.Linq;
using UniversityMagazine.EF;

namespace UniversityMagazine.DAO
{
    public class CommentImageDAO
    {
        UniversityMagazineDBContext db = null;
        public CommentImageDAO()
        {
            db = new UniversityMagazineDBContext();
        }
        public COMMENTIMAGE GetById(int cOMMENT_Id)
        {
            return db.COMMENTIMAGEs.Find(cOMMENT_Id);
        }

        public IEnumerable<COMMENTIMAGE> ViewComment(string iMAGE_FileName)
        {
            IEnumerable<COMMENTIMAGE> model = db.COMMENTIMAGEs.Where(x => x.IMAGE.IMAGE_FileName == iMAGE_FileName).OrderByDescending(x => x.COMMENT_Time);
            return model;
        }

        public bool Create(COMMENTIMAGE cOMMENTIMAGE)
        {
            try
            {
                cOMMENTIMAGE.COMMENT_Time = DateTime.Now;
                var model = db.COMMENTIMAGEs.Add(cOMMENTIMAGE);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        public bool Edit(COMMENTIMAGE cOMMENTIMAGE)
        {
            try
            {
                COMMENTIMAGE data = db.COMMENTIMAGEs.Find(cOMMENTIMAGE.COMMENT_Id);
                data.COMMENT_Content = cOMMENTIMAGE.COMMENT_Content;
                data.COMMENT_Time = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int cOMMENT_Id)
        {
            try
            {
                var image = db.COMMENTIMAGEs.Find(cOMMENT_Id);
                db.COMMENTIMAGEs.Remove(image);
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