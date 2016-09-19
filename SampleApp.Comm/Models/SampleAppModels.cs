using SampleApp.Comm.Contracts;
using SampleApp.Model.Models;
using System.Data.Entity;

namespace SampleApp.Comm.Models
{
    public partial class SampleAppModels : DbContext, ISampleAppModels
    {
        public SampleAppModels()
            : base("name=SampleAppModels")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<FailedEmail> FailedEmail { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Transection> Transection { get; set; }
        public virtual DbSet<TransectionMode> TransectionMode { get; set; }
        public virtual DbSet<TransectionType> TransectionType { get; set; }
        public virtual DbSet<Campaign> Campaign { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Account)
                .WithOptional(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);
        }
    }
}