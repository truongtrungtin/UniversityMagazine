using EntityModels.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityMagazine.DAO
{
    public class NotificationDAO
    {
        UniversityMagazineDBContext db = null;
        public NotificationDAO()
        {
            db = new UniversityMagazineDBContext();
        }

        public NOTIFICATION GetById(Guid? nOTIFICATION_Id)
        {
            return db.NOTIFICATIONs.Find(nOTIFICATION_Id);
        }

        public IEnumerable<NOTIFICATION> ListNotification(Guid? To)
        {
            IQueryable<NOTIFICATION> model = db.NOTIFICATIONs.Where(x => x.NOTIFICATION_To == To && x.NOTIFICATION_Status == false);
            return model;
        }

    }
}