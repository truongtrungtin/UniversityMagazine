namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HASHTAG")]
    public partial class HASHTAG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HASHTAG()
        {
            ARTICLEHASHTAGs = new HashSet<ARTICLEHASHTAG>();
        }

        [Key]
        public Guid HASHTAG_Id { get; set; }

        [StringLength(50)]
        public string HASHTAG_Code { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARTICLEHASHTAG> ARTICLEHASHTAGs { get; set; }
    }
}
