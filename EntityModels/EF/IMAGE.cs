namespace EntityModels.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("IMAGE")]
    public partial class IMAGE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IMAGE()
        {
            COMMENTIMAGEs = new HashSet<COMMENTIMAGE>();
        }

        [Key]
        public Guid IMAGE_Id { get; set; }

        [StringLength(50)]
        public string IMAGE_FileName { get; set; }

        [StringLength(50)]
        public string IMAGE_Type { get; set; }

        public long? IMAGE_Size { get; set; }

        [StringLength(500)]
        public string IMAGE_FileUpload { get; set; }

        public DateTime? IMAGE_UploadTime { get; set; }

        [StringLength(500)]
        public string IMAGE_Tittle { get; set; }

        public bool IMAGE_Status { get; set; }

        public Guid? FACULTY_Id { get; set; }

        public Guid? ACCOUNT_Id { get; set; }

        public virtual ACCOUNT ACCOUNT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENTIMAGE> COMMENTIMAGEs { get; set; }

        public virtual FACULTY FACULTY { get; set; }
    }
}
