namespace EntityModels.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    [Table("TERMCONDITION")]
    public partial class TERMCONDITION
    {
        [Key]
        public Guid TERMCONDITION_Id { get; set; }
        [AllowHtml]
        public string TERMCONDITION_Content { get; set; }
    }
}
