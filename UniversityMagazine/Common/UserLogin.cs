using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityMagazine.Common
{
    [Serializable]
    public class UserLogin
    {
        public Guid UserID { set; get; }
    }
}