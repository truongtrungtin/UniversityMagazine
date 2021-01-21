namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARTICLEIMAGE")]
    public partial class ARTICLEIMAGE
    {
        [Key]
        public Guid ARTICLEIMAGE_Id { get; set; }

        [StringLength(500)]
        public string ARTICLEIMAGE_Url { get; set; }

        public Guid? ARTICLES_Id { get; set; }

        public virtual ARTICLE ARTICLE { get; set; }
    }
}
