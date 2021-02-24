namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ARTICLE")]
    public partial class ARTICLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARTICLE()
        {
            COMMENTARTICLEs = new HashSet<COMMENTARTICLE>();
            EDITINGHISTORies = new HashSet<EDITINGHISTORY>();
        }

        [Key]
        public Guid ARTICLE_Id { get; set; }

        [StringLength(50)]
        public string ARTICLE_FileName { get; set; }

        [StringLength(50)]
        public string ARTICLE_Type { get; set; }

        public long? ARTICLE_Size { get; set; }

        [StringLength(500)]
        public string ARTICLE_FileUpload { get; set; }

        public DateTime? ARTICLE_EditTime { get; set; }

        public DateTime? ARTICLE_UploadTime { get; set; }

        [StringLength(500)]
        public string ARTICLE_Tittle { get; set; }

        public bool ARTICLE_Status { get; set; }

        public Guid? FACULTY_Id { get; set; }

        public Guid? ACCOUNT_Id { get; set; }

        public virtual ACCOUNT ACCOUNT { get; set; }

        public virtual FACULTY FACULTY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENTARTICLE> COMMENTARTICLEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EDITINGHISTORY> EDITINGHISTORies { get; set; }

        public ARTICLE(ARTICLE aRTICLE)
        {
            ARTICLE_Type = aRTICLE.ARTICLE_Type;
            ARTICLE_Size = aRTICLE.ARTICLE_Size;
            ARTICLE_FileUpload = aRTICLE.ARTICLE_FileUpload;
            ARTICLE_EditTime = aRTICLE.ARTICLE_EditTime;
            ARTICLE_Status = aRTICLE.ARTICLE_Status;
            ARTICLE_Tittle = aRTICLE.ARTICLE_Tittle;
            FACULTY_Id = aRTICLE.FACULTY_Id;
            ACCOUNT_Id = aRTICLE.ACCOUNT_Id;
        }
    }
}
