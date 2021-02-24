namespace UniversityMagazine.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TERMCONDITION")]
    public partial class TERMCONDITION
    {
        [Key]
        public Guid TERMCONDITION_Id { get; set; }

        public string TERMCONDITION_Content { get; set; }
    }
}
