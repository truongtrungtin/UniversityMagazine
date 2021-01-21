using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityMagazine.Common
{
    public class Credential
    {

        public Guid ACCOUNT_ID { get; set; }
        public Guid ROLEGROUP_Id { get; set; }
        public string ROLEGROUP_Name { get; set; }
        public Guid ROLE_Id { get; set; }
        public string ROLE_Code { get; set; }
        public string ROLE_Name { get; set; }
        public bool? CREDENTIAL_VIEW { get; set; }
        public bool? CREDENTIAL_ADD { get; set; }
        public bool? CREDENTIAL_EDIT { get; set; }
        public bool? CREDENTIAL_DELETE { get; set; }
    }
}