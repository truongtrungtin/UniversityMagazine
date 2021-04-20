namespace EntityModels.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ACCOUNT")]
    public partial class ACCOUNT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACCOUNT()
        {
            ARTICLEs = new HashSet<ARTICLE>();
            COMMENTARTICLEs = new HashSet<COMMENTARTICLE>();
            COMMENTIMAGEs = new HashSet<COMMENTIMAGE>();
            EDITINGHISTORies = new HashSet<EDITINGHISTORY>();
            IMAGEs = new HashSet<IMAGE>();
            MESSAGEs = new HashSet<MESSAGE>();
            MESSAGEs1 = new HashSet<MESSAGE>();
            NOTIFICATIONs = new HashSet<NOTIFICATION>();
            NOTIFICATIONs1 = new HashSet<NOTIFICATION>();
        }

        [Key]
        public Guid ACCOUNT_Id { get; set; }

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

        public DateTime? ACCOUNT_CreateTime { get; set; }

        public DateTime? ACCOUNT_EditTime { get; set; }

        public bool ACCOUNT_Status { get; set; }

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

        public Guid? ACCOUNT_CREATOR { get; set; }

        public virtual FACULTY FACULTY { get; set; }

        public virtual ROLEGROUP ROLEGROUP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARTICLE> ARTICLEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENTARTICLE> COMMENTARTICLEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENTIMAGE> COMMENTIMAGEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EDITINGHISTORY> EDITINGHISTORies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IMAGE> IMAGEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MESSAGE> MESSAGEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MESSAGE> MESSAGEs1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICATION> NOTIFICATIONs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICATION> NOTIFICATIONs1 { get; set; }
    }
}
