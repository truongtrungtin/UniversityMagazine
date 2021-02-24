using System;
using System.Collections.Generic;
using System.Linq;
using UniversityMagazine.EF;

namespace UniversityMagazine.DAO
{
    public class CommentArticleDAO
    {
        UniversityMagazineDBContext db = null;
        public CommentArticleDAO()
        {
            db = new UniversityMagazineDBContext();
        }
        public COMMENTARTICLE GetById(int cOMMENT_Id)
        {
            return db.COMMENTARTICLEs.Find(cOMMENT_Id);
        }

        public IEnumerable<COMMENTARTICLE> ViewComment(string aRTICLE_FileName)
        {
            IEnumerable<COMMENTARTICLE> model = db.COMMENTARTICLEs.Where(x => x.ARTICLE.ARTICLE_FileName == aRTICLE_FileName).OrderByDescending(x => x.COMMENT_Time);
            return model;
        }

        public bool Create(COMMENTARTICLE cOMMENTARTICLE)
        {
            try
            {
                cOMMENTARTICLE.COMMENT_Time = DateTime.Now;
                var model = db.COMMENTARTICLEs.Add(cOMMENTARTICLE);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        public bool Edit(COMMENTARTICLE cOMMENTARTICLE)
        {
            try
            {
                COMMENTARTICLE data = db.COMMENTARTICLEs.Find(cOMMENTARTICLE.COMMENT_Id);
                data.COMMENT_Content = cOMMENTARTICLE.COMMENT_Content;
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
                var article = db.COMMENTARTICLEs.Find(cOMMENT_Id);
                db.COMMENTARTICLEs.Remove(article);
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