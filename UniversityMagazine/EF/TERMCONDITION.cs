namespace UniversityMagazine.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TERMCONDITION")]
    public partial class TERMCONDITION
    {
        [Key]
        public Guid TERMCONDITION_Id { get; set; }

        public string TERMCONDITION_Content { get; set; }
    }
}
