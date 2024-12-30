using Domain.Models;
using Domain.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public IHttpContextAccessor _httpContextAccessor { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region preSubmit

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applying soft delete globally
            modelBuilder.Entity<BaseEntity>().HasQueryFilter(e => !e.IsDeleted);

            // Configuring Table-Per-Hierarchy (TPH) for BaseEntity and its derived entities
            //modelBuilder.Entity<BaseEntity>()
            //    .HasDiscriminator<string>("EntityType")
            //    .HasValue<Device>("Device")
            //    .HasValue<Images>("Images")
            //    .HasValue<Treatment>("Treatment")
            //    .HasValue<DeploymentData>("DeploymentData")
            //    .HasValue<DeploymentSummary>("DeploymentSummary")
            //    .HasValue<VehicleData>("VehicleData");

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            HandleSoftDeletes();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            HandleSoftDeletes();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            var currentUser = GetCurrentUserId();
            if (!string.IsNullOrEmpty(currentUser))
            {
                var currentTime = DateTime.UtcNow;
                foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedAt = currentTime;
                        entry.Entity.CreatedBy_Id = currentUser;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastUpdatedAt = currentTime;
                        entry.Entity.UpdatedBy_Id = currentUser;
                    }
                }
            }
        }

        private void HandleSoftDeletes()
        {
            var currentUser = GetCurrentUserId();
            var currentTime = DateTime.UtcNow;

            var entries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Deleted && e.Entity is BaseEntity);

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                entity.IsDeleted = true;
                entity.DeletedAt = currentTime;
                entity.DeletedBy_Id = currentUser;

                entry.State = EntityState.Modified; // Mark as modified to reflect soft delete
            }
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User_Id")?.Value;
        }

        #endregion

        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<JMX> Jmxes { get; set; }
        public virtual DbSet<Lot> Lots { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DeploymentSummary> DeploymentSummaries { get; set; }
        public virtual DbSet<VehicleData> VehicleDatas { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
    }
}
