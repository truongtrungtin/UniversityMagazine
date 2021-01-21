using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityMagazine.EF;

namespace UniversityMagazine.Models
{
    public class LoginModel
    {
        public string UserName { set; get; }
        public string Password { set; get; }

    }
}