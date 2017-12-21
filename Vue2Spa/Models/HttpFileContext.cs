using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vue2Spa.Models.Identity;

namespace Vue2Spa.Models
{
    public class HttpFileContext :  IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public HttpFileContext(DbContextOptions<HttpFileContext> options)
           : base(options)
        {
            DbInitializer.Initialize(this);
        }

        public DbSet<HttpFileInfo> HttpFileInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.EnableAutoHistory(null);

            builder.Entity<HttpFileInfo>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd();
                entity
                    .ToTable(name: "TB_File_Info");
                entity.Property<DateTime>("UpdatedTimestamp");
            });

            #region identity db context
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "TB_WF_Users");
                entity.Property(e => e.Id).HasColumnName("UserId");

            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "TB_WF_Roles");
                entity.Property(e => e.Id).HasColumnName("RoleId");
            });

            builder.Entity<ApplicationUserClaim>(entity =>
            {
                entity.ToTable("TB_WF_UserClaims");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Id).HasColumnName("UserClaimId");
            });

            builder.Entity<ApplicationUserLogin>(entity =>
            {
                entity.ToTable("TB_WF_UserLogins");
                entity.Property(e => e.UserId).HasColumnName("UserId");
            });

            builder.Entity<ApplicationRoleClaim>(entity =>
            {
                entity.ToTable("TB_WF_RoleClaims");
                entity.Property(e => e.Id).HasColumnName("RoleClaimId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable("TB_WF_UserRoles");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<ApplicationUserToken>(entity =>
            {
                entity.ToTable("TB_WF_UserTokens");
                entity.Property(e => e.UserId).HasColumnName("UserId");
            });

            #region
            builder.Entity<IdentityUser<string>>(entity =>
            {
                entity.ToTable(name: "TB_WF_Users");
                entity.Property(e => e.Id).HasColumnName("UserId");

            });

            builder.Entity<IdentityRole<string>>(entity =>
            {
                entity.ToTable(name: "TB_WF_Roles");
                entity.Property(e => e.Id).HasColumnName("RoleId");

            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("TB_WF_UserClaims");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Id).HasColumnName("UserClaimId");

            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("TB_WF_UserLogins");
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("TB_WF_RoleClaims");
                entity.Property(e => e.Id).HasColumnName("RoleClaimId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("TB_WF_UserRoles");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");

            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("TB_WF_UserTokens");
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });
            #endregion
            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }

            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<HttpFileInfo>();
            updateUpdatedProperty<ApplicationUser>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
