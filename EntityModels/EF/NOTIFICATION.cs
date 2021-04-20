namespace EntityModels.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("NOTIFICATION")]
    public partial class NOTIFICATION
    {
        [Key]
        public Guid NOTIFICATION_Id { get; set; }

        [Column(TypeName = "text")]
        public string NOTIFICATION_Content { get; set; }

        public DateTime? NOTIFICATION_Time { get; set; }

        public Guid? NOTIFICATION_From { get; set; }

        public Guid? NOTIFICATION_To { get; set; }

        public bool? NOTIFICATION_Status { get; set; }

        public string NOTIFICATION_Url { get; set; }

        [StringLength(50)]
        public string NOTIFICATION_Type { get; set; }

        public virtual ACCOUNT ACCOUNT { get; set; }

        public virtual ACCOUNT ACCOUNT1 { get; set; }
    }
}
