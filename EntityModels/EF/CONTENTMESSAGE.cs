namespace EntityModels.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTENTMESSAGE")]
    public partial class CONTENTMESSAGE
    {
        [Key]
        public Guid? MESSAGE_Id { get; set; }

        [StringLength(500)]
        public string CONTENTMESSAGE_Content { get; set; }

        [StringLength(50)]
        public string CONTENT_Type { get; set; }

        public virtual MESSAGE MESSAGE { get; set; }
    }
}
