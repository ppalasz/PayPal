using Microsoft.EntityFrameworkCore;
using PayPal.Business.BLL;
using PayPal.Business.DAL.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace PayPal.Business.DAL
{
    public class ProjectContext : DbContext
    {
        private string _connectionString;

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
            _connectionString = "";
        }

        public DbSet<VwProject> VwProject { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VwProject>().ToTable("vwProjects");
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<Vendor>().ToTable("Vendors");
            modelBuilder.Entity<Status>().ToTable("Statuses");
            modelBuilder.Entity<ProjectType>().ToTable("ProjectTypes");

            //modelBuilder.Entity<Project>().HasKey(c => c.ProjectId);
            //modelBuilder.Entity<VwProject>().HasKey(c => c.ProjectId);
            //modelBuilder.Entity<Vendor>().HasKey(c => c.VendorId);
            //modelBuilder.Entity<Status>().HasKey(c => c.StatusId);
            //modelBuilder.Entity<ProjectType>().HasKey(c => c.ProjectTypeId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}
