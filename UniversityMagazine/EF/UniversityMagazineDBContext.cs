using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace UniversityMagazine.EF
{
    public partial class UniversityMagazineDBContext : DbContext
    {
        public UniversityMagazineDBContext()
            : base("name=UniversityMagazineContext")
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; }
        public virtual DbSet<ARTICLE> ARTICLEs { get; set; }
        public virtual DbSet<ARTICLEHASHTAG> ARTICLEHASHTAGs { get; set; }
        public virtual DbSet<ARTICLEIMAGE> ARTICLEIMAGEs { get; set; }
        public virtual DbSet<CREDENTIAL> CREDENTIALs { get; set; }
        public virtual DbSet<EDITINGHISTORY> EDITINGHISTORies { get; set; }
        public virtual DbSet<FACULTY> FACULTies { get; set; }
        public virtual DbSet<HASHTAG> HASHTAGs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ROLEGROUP> ROLEGROUPs { get; set; }
        public virtual DbSet<SLIDE> SLIDEs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TERMCONDITION> TERMCONDITIONs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.ACCOUNT_Code)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.ACCOUNT_Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<ARTICLE>()
                .Property(e => e.ARTICLES_Code)
                .IsUnicode(false);

            modelBuilder.Entity<ARTICLE>()
                .Property(e => e.ARTICLES_FileUpload)
                .IsUnicode(false);

            modelBuilder.Entity<ARTICLE>()
                .HasMany(e => e.ARTICLEHASHTAGs)
                .WithOptional(e => e.ARTICLE)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ARTICLE>()
                .HasMany(e => e.ARTICLEIMAGEs)
                .WithOptional(e => e.ARTICLE)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ARTICLE>()
                .HasMany(e => e.EDITINGHISTORies)
                .WithOptional(e => e.ARTICLE)
                .HasForeignKey(e => e.ARTICLE_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ARTICLEIMAGE>()
                .Property(e => e.ARTICLEIMAGE_Url)
                .IsUnicode(false);

            modelBuilder.Entity<FACULTY>()
                .Property(e => e.FACULTY_Code)
                .IsUnicode(false);

            modelBuilder.Entity<HASHTAG>()
                .Property(e => e.HASHTAG_Code)
                .IsUnicode(false);

            modelBuilder.Entity<HASHTAG>()
                .HasMany(e => e.ARTICLEHASHTAGs)
                .WithOptional(e => e.HASHTAG)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Role>()
                .Property(e => e.ROLE_Code)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
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
