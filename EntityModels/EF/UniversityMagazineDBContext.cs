using System.Data.Entity;

namespace EntityModels.EF
{
    public partial class UniversityMagazineDBContext : DbContext
    {
        public UniversityMagazineDBContext()
            : base("name=UniversityMagazineDBContext")
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; }
        public virtual DbSet<ARTICLE> ARTICLEs { get; set; }
        public virtual DbSet<COMMENTARTICLE> COMMENTARTICLEs { get; set; }
        public virtual DbSet<COMMENTIMAGE> COMMENTIMAGEs { get; set; }
        public virtual DbSet<CONTENTMESSAGE> CONTENTMESSAGEs { get; set; }
        public virtual DbSet<CREDENTIAL> CREDENTIALs { get; set; }
        public virtual DbSet<EDITINGHISTORY> EDITINGHISTORies { get; set; }
        public virtual DbSet<FACULTY> FACULTies { get; set; }
        public virtual DbSet<IMAGE> IMAGEs { get; set; }
        public virtual DbSet<MESSAGE> MESSAGEs { get; set; }
        public virtual DbSet<NOTIFICATION> NOTIFICATIONs { get; set; }
        public virtual DbSet<ROLE> ROLEs { get; set; }
        public virtual DbSet<ROLEGROUP> ROLEGROUPs { get; set; }
        public virtual DbSet<SLIDE> SLIDEs { get; set; }
        public virtual DbSet<TERMCONDITION> TERMCONDITIONs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.ACCOUNT_Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.ACCOUNT_Username)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.ACCOUNT_Password)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.ACCOUNT_Key)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.ACCOUNT_Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .HasMany(e => e.MESSAGEs)
                .WithOptional(e => e.ACCOUNT)
                .HasForeignKey(e => e.MESSAGE_From);

            modelBuilder.Entity<ACCOUNT>()
                .HasMany(e => e.MESSAGEs1)
                .WithOptional(e => e.ACCOUNT1)
                .HasForeignKey(e => e.MESSAGE_To);

            modelBuilder.Entity<ACCOUNT>()
                .HasMany(e => e.NOTIFICATIONs)
                .WithOptional(e => e.ACCOUNT)
                .HasForeignKey(e => e.NOTIFICATION_From);

            modelBuilder.Entity<ACCOUNT>()
                .HasMany(e => e.NOTIFICATIONs1)
                .WithOptional(e => e.ACCOUNT1)
                .HasForeignKey(e => e.NOTIFICATION_To);

            modelBuilder.Entity<ARTICLE>()
                .Property(e => e.ARTICLE_FileName)
                .IsUnicode(false);

            modelBuilder.Entity<ARTICLE>()
                .Property(e => e.ARTICLE_Type)
                .IsUnicode(false);

            modelBuilder.Entity<ARTICLE>()
                .Property(e => e.ARTICLE_FileUpload)
                .IsUnicode(false);

            modelBuilder.Entity<ARTICLE>()
                .HasMany(e => e.COMMENTARTICLEs)
                .WithOptional(e => e.ARTICLE)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ARTICLE>()
                .HasMany(e => e.EDITINGHISTORies)
                .WithOptional(e => e.ARTICLE)
                .WillCascadeOnDelete();

            modelBuilder.Entity<FACULTY>()
                .Property(e => e.FACULTY_Code)
                .IsUnicode(false);

            modelBuilder.Entity<IMAGE>()
                .Property(e => e.IMAGE_FileName)
                .IsUnicode(false);

            modelBuilder.Entity<IMAGE>()
                .Property(e => e.IMAGE_Type)
                .IsUnicode(false);

            modelBuilder.Entity<IMAGE>()
                .Property(e => e.IMAGE_FileUpload)
                .IsUnicode(false);

            modelBuilder.Entity<IMAGE>()
                .HasMany(e => e.COMMENTIMAGEs)
                .WithOptional(e => e.IMAGE)
                .WillCascadeOnDelete();

            modelBuilder.Entity<MESSAGE>()
                .HasOptional(e => e.CONTENTMESSAGE)
                .WithRequired(e => e.MESSAGE);

            modelBuilder.Entity<NOTIFICATION>()
                .Property(e => e.NOTIFICATION_Content)
                .IsUnicode(false);

            modelBuilder.Entity<ROLE>()
                .Property(e => e.ROLE_Code)
                .IsUnicode(false);

            modelBuilder.Entity<ROLEGROUP>()
                .Property(e => e.ROLEGROUP_Code)
                .IsUnicode(false);

            modelBuilder.Entity<SLIDE>()
                .Property(e => e.SLIDE_Image)
                .IsUnicode(false);

            modelBuilder.Entity<SLIDE>()
                .Property(e => e.SLIDE_Content)
                .IsUnicode(false);
        }
    }
}
