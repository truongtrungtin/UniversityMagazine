namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ACCOUNT")]
    public partial class ACCOUNT
    {
        [Key]
        public Guid ACCOUNT_Id { get; set; }

        [StringLength(100)]
        public string ACCOUNT_Code { get; set; }

        [StringLength(250)]
        public string ACCOUNT_Name { get; set; }

        [StringLength(500)]
        public string ACCOUNT_Address { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ACCOUNT_BirthDay { get; set; }

        [StringLength(500)]
        public string ACCOUNT_Email { get; set; }

        [StringLength(15)]
        public string ACCOUNT_Telephone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ACCOUNT_CreateTime { get; set; }

        public DateTime? ACCOUNT_EditTime { get; set; }

        public bool? ACCOUNT_Status { get; set; }
        [StringLength(100)]
        public string ACCOUNT_Username { get; set; }
        [StringLength(100)]
        public string ACCOUNT_Password { get; set; }
        [StringLength(8)]
        public string ACCOUNT_Key { get; set; }
        [StringLength(500)]
        public string ACCOUNT_Avatar { get; set; }

        public Guid? FACULTY_Id { get; set; }

        public Guid? ROLEGROUP_Id { get; set; }

        public virtual FACULTY FACULTY { get; set; }

        public virtual ROLEGROUP ROLEGROUP { get; set; }
    }
}
