namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            CREDENTIALs = new HashSet<CREDENTIAL>();
        }

        [Key]
        public Guid ROLE_Id { get; set; }

        [StringLength(50)]
        public string ROLE_Code { get; set; }

        [StringLength(100)]
        public string ROLE_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CREDENTIAL> CREDENTIALs { get; set; }
    }
}
