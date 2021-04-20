namespace EntityModels.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EDITINGHISTORY")]
    public partial class EDITINGHISTORY
    {
        [Key]
        public Guid EDITINGHISTORY_Id { get; set; }

        public DateTime? EDITINGHISTORY_Time { get; set; }

        public string EDITINGHISTORY_Content { get; set; }

        public Guid? ACCOUNT_Id { get; set; }

        public Guid? ARTICLE_Id { get; set; }

        public virtual ACCOUNT ACCOUNT { get; set; }

        public virtual ARTICLE ARTICLE { get; set; }
    }
}
