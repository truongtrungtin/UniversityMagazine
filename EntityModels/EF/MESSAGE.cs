namespace EntityModels.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MESSAGE")]
    public partial class MESSAGE
    {
        [Key]
        public Guid MESSAGE_Id { get; set; }

        public Guid? MESSAGE_From { get; set; }

        public Guid? MESSAGE_To { get; set; }

        public DateTime? MESSAGE_SendTime { get; set; }

        public bool MESSAGE_Status { get; set; }

        public virtual ACCOUNT ACCOUNT { get; set; }

        public virtual ACCOUNT ACCOUNT1 { get; set; }

        public virtual CONTENTMESSAGE CONTENTMESSAGE { get; set; }
    }
}
