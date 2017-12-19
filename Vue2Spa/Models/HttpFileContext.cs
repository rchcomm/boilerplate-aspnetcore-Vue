using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EnableAutoHistory(null);

            modelBuilder.Entity<HttpFileInfo>().ToTable("TB_File_Info");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
