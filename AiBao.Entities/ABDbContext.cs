using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AiBao.Entities
{
    public partial class ABDbContext : DbContext
    {
        public ABDbContext()
        {
        }

        public ABDbContext(DbContextOptions<ABDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Sys_ActivityLog> Sys_ActivityLog { get; set; }
        public virtual DbSet<Sys_ActivityLogComment> Sys_ActivityLogComment { get; set; }
        public virtual DbSet<Sys_Category> Sys_Category { get; set; }
        public virtual DbSet<Sys_NLog> Sys_NLog { get; set; }
        public virtual DbSet<Sys_Permission> Sys_Permission { get; set; }
        public virtual DbSet<Sys_Role> Sys_Role { get; set; }
        public virtual DbSet<Sys_Setting> Sys_Setting { get; set; }
        public virtual DbSet<Sys_User> Sys_User { get; set; }
        public virtual DbSet<Sys_UserJwt> Sys_UserJwt { get; set; }
        public virtual DbSet<Sys_UserLogin> Sys_UserLogin { get; set; }
        public virtual DbSet<Sys_UserR> Sys_UserR { get; set; }
        public virtual DbSet<Sys_UserRole> Sys_UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=182.48.115.232,18833;initial catalog=AiBao;user id=fww;password=DY@fww2018;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Sys_ActivityLog>(entity =>
            {
                entity.HasIndex(e => e.CreationTime);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_ActivityLogComment>(entity =>
            {
                entity.Property(e => e.EntityName).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_Category>(entity =>
            {
                entity.HasIndex(e => e.UID)
                    .HasName("IX_Sys_Category")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsMenu).IsUnicode(false);
            });

            modelBuilder.Entity<Sys_NLog>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_Permission>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_Setting>(entity =>
            {
                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_User>(entity =>
            {
                entity.HasIndex(e => e.Account);

                entity.HasIndex(e => e.CreationTime);

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationTime).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Sys_UserJwt>(entity =>
            {
                entity.HasKey(e => e.Jti)
                    .HasName("PK_SysUserJWTToken");

                entity.HasIndex(e => e.Expiration);

                entity.Property(e => e.Jti).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_UserLogin>(entity =>
            {
                entity.HasIndex(e => e.LoginTime);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_UserR>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_UserRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
