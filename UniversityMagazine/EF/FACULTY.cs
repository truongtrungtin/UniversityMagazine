namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FACULTY")]
    public partial class FACULTY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FACULTY()
        {
            ACCOUNTs = new HashSet<ACCOUNT>();
            ARTICLEs = new HashSet<ARTICLE>();
            IMAGEs = new HashSet<IMAGE>();
        }

        [Key]
        public Guid FACULTY_Id { get; set; }

        [StringLength(200)]
        public string FACULTY_Name { get; set; }

        [StringLength(500)]
        public string FACULTY_Descriptions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACCOUNT> ACCOUNTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARTICLE> ARTICLEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IMAGE> IMAGEs { get; set; }
    }
}
