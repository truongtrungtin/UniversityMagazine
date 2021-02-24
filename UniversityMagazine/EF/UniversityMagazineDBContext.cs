using System.Data.Entity;

namespace UniversityMagazine.EF
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
        public virtual DbSet<CREDENTIAL> CREDENTIALs { get; set; }
        public virtual DbSet<EDITINGHISTORY> EDITINGHISTORies { get; set; }
        public virtual DbSet<FACULTY> FACULTies { get; set; }
        public virtual DbSet<IMAGE> IMAGEs { get; set; }
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
                .HasMany(e => e.COMMENTARTICLEs)
                .WithOptional(e => e.ACCOUNT)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ACCOUNT>()
                .HasMany(e => e.COMMENTIMAGEs)
                .WithOptional(e => e.ACCOUNT)
                .WillCascadeOnDelete();

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

            modelBuilder.Entity<ROLE>()
                .Property(e => e.ROLE_Code)
                .IsUnicode(false);

            modelBuilder.Entity<ROLE>()
                .HasMany(e => e.CREDENTIALs)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ROLEGROUP>()
                .Property(e => e.ROLEGROUP_Code)
                .IsUnicode(false);

            modelBuilder.Entity<ROLEGROUP>()
                .HasMany(e => e.CREDENTIALs)
                .WithRequired(e => e.ROLEGROUP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SLIDE>()
                .Property(e => e.SLIDE_Image)
                .IsUnicode(false);

            modelBuilder.Entity<SLIDE>()
                .Property(e => e.SLIDE_Content)
                .IsUnicode(false);
        }
    }
}
