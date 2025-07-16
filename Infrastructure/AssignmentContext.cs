using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AssignmentContext : DbContext
    {
        public virtual DbSet<AssignmentDataModel> Assignments { get; set; }
        public virtual DbSet<AssignmentTempDataModel> AssignmentsTemp { get; set; }
        public virtual DbSet<CollaboratorDataModel> Collaborators { get; set; }
        public virtual DbSet<DeviceDataModel> Devices { get; set; }


        public AssignmentContext(DbContextOptions<AssignmentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssignmentDataModel>().OwnsOne(a => a.PeriodDate);
            modelBuilder.Entity<AssignmentTempDataModel>().OwnsOne(a => a.PeriodDate);
            modelBuilder.Entity<CollaboratorDataModel>().OwnsOne(a => a.PeriodDateTime);

            base.OnModelCreating(modelBuilder);
        }
    }
}