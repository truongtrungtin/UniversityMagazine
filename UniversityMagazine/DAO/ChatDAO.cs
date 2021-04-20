using EntityModels.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityMagazine.DAO
{
    public class ChatDAO
    {
        UniversityMagazineDBContext db = null;
        public ChatDAO()
        {
            db = new UniversityMagazineDBContext();
        }

        public ACCOUNT GetById(Guid? Account_id)
        {
            return db.ACCOUNTs.Find(Account_id);
        }

        public IEnumerable<ACCOUNT> GetAccounts(Guid? Faculty_id)
        {
            IEnumerable<ACCOUNT> model = db.ACCOUNTs.Where(x => x.FACULTY_Id == Faculty_id);
            return model;
        }

        public IEnumerable<MESSAGE> GetMessage()
        {
            IQueryable<MESSAGE> model = db.MESSAGEs;
            return model;

        }

        public MESSAGE CreateMessage(Guid? From, Guid? To)
        {
            var data = new MESSAGE();
            data.MESSAGE_Id = Guid.NewGuid();
            data.MESSAGE_From = From;
            data.MESSAGE_To = To;
            data.MESSAGE_SendTime = DateTime.Now;
            db.MESSAGEs.Add(data);
            db.SaveChanges();
            return data;
        }

        public CONTENTMESSAGE CreateContent(Guid? Id, string Message, string type)
        {
            var data = new CONTENTMESSAGE();
            data.MESSAGE_Id = Id;
            data.CONTENTMESSAGE_Content = Message;
            data.CONTENT_Type = type;
            db.CONTENTMESSAGEs.Add(data);
            db.SaveChanges();
            return data;
        }

        public void ChangeStatus(Guid? To, Guid? From)
        {
            var data = db.MESSAGEs.Where(x => x.MESSAGE_To == To && x.MESSAGE_From == From && x.MESSAGE_Status == false);
            foreach (var item in data)
            {
                item.MESSAGE_Status = true;
            }
            db.SaveChanges();
        }
    }
}