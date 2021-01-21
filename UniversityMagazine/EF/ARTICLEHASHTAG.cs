namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARTICLEHASHTAG")]
    public partial class ARTICLEHASHTAG
    {
        [Key]
        public Guid ARTICLEHASHTAG_Id { get; set; }

        public Guid? ARTICLES_Id { get; set; }

        public Guid? HASHTAG_Id { get; set; }

        public virtual ARTICLE ARTICLE { get; set; }

        public virtual HASHTAG HASHTAG { get; set; }
    }
}
