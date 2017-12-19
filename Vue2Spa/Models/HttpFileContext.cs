using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vue2Spa.Models.Identity;

namespace Vue2Spa.Models
{
    public class HttpFileContext : DbContext
    {
        public HttpFileContext(DbContextOptions<HttpFileContext> options)
           : base(options)
        {
            DbInitializer.Initialize(this);
        }

        public DbSet<HttpFileInfo> HttpFileInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.EnableAutoHistory(null);

            builder.Entity<HttpFileInfo>().ToTable("TB_File_Info").HasKey(m => m.Id);
            // shadow properties
            builder.Entity<HttpFileInfo>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<ApplicationUser>().Property<DateTime>("UpdatedTimestamp");

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
