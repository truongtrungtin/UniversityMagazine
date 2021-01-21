namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CREDENTIAL")]
    public partial class CREDENTIAL
    {
        [Key]
        public Guid CREDENTIAL_Id { get; set; }

        public bool? CREDENTIAL_VIEW { get; set; }

        public bool? CREDENTIAL_EDIT { get; set; }

        public bool? CREDENTIAL_DELETE { get; set; }

        public bool? CREDENTIAL_ADD { get; set; }

        public Guid ROLEGROUP_Id { get; set; }

        public Guid ROLE_Id { get; set; }

        public virtual Role Role { get; set; }

        public virtual ROLEGROUP ROLEGROUP { get; set; }
    }
}
