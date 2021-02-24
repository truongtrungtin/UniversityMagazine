using System;

namespace UniversityMagazine.Common
{
    [Serializable]
    public class UserLogin
    {
        public Guid UserID { set; get; }
        public string RoleGroup { set; get; }
    }
}