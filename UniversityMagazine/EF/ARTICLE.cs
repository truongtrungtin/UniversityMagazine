namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARTICLE")]
    public partial class ARTICLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARTICLE()
        {
            ARTICLEHASHTAGs = new HashSet<ARTICLEHASHTAG>();
            ARTICLEIMAGEs = new HashSet<ARTICLEIMAGE>();
            EDITINGHISTORies = new HashSet<EDITINGHISTORY>();
        }

        [Key]
        public Guid ARTICLES_Id { get; set; }

        [StringLength(500)]
        public string ARTICLES_Code { get; set; }

        [StringLength(500)]
        public string ARTICLES_Content { get; set; }

        [StringLength(100)]
        public string ARTICLES_Tittle { get; set; }

        [StringLength(500)]
        public string ARTICLES_FileUpload { get; set; }

        public DateTime? ARTICLES_UploadTime { get; set; }

        public bool? ARTICLES_Status { get; set; }

        public Guid? FACULTY_Id { get; set; }

        public virtual FACULTY FACULTY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARTICLEHASHTAG> ARTICLEHASHTAGs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARTICLEIMAGE> ARTICLEIMAGEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EDITINGHISTORY> EDITINGHISTORies { get; set; }
    }
}
