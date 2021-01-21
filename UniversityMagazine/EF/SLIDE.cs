namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SLIDE")]
    public partial class SLIDE
    {
        [Key]
        public Guid SLIDE_Id { get; set; }

        public string SLIDE_Image { get; set; }

        [StringLength(500)]
        public string SLIDE_Content { get; set; }

        public DateTime? SLIDE_UploadTime { get; set; }

        public DateTime? SLIDE_EditTime { get; set; }
    }
}
