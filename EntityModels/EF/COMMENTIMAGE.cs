namespace EntityModels.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("COMMENTIMAGE")]
    public partial class COMMENTIMAGE
    {
        [Key]
        public int COMMENT_Id { get; set; }

        public Guid? ACCOUNT_Id { get; set; }

        public string COMMENT_Content { get; set; }

        public Guid? IMAGE_Id { get; set; }

        public DateTime? COMMENT_Time { get; set; }

        public virtual ACCOUNT ACCOUNT { get; set; }

        public virtual IMAGE IMAGE { get; set; }
    }
}
