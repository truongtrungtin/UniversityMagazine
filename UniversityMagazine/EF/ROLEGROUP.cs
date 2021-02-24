namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ROLEGROUP")]
    public partial class ROLEGROUP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ROLEGROUP()
        {
            ACCOUNTs = new HashSet<ACCOUNT>();
            CREDENTIALs = new HashSet<CREDENTIAL>();
        }

        [Key]
        public Guid ROLEGROUP_Id { get; set; }

        [StringLength(500)]
        public string ROLEGROUP_Code { get; set; }

        [StringLength(500)]
        public string ROLEGROUP_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACCOUNT> ACCOUNTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CREDENTIAL> CREDENTIALs { get; set; }
    }
}
