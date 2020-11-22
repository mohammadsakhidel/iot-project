using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TrackDataAccess.Models;

namespace TrackDataAccess.Database {
    public class TrackDbContext : DbContext {

        public TrackDbContext(DbContextOptions<TrackDbContext> options) : base(options) {
        }

        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<LocationReport> LocationReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            #region Soft Delete Query Filter:
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
                var isDeletedProperty = entityType.FindProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool)) {
                    var entity = Expression.Parameter(entityType.ClrType, "entity");
                    var left = Expression.Property(entity, "IsDeleted");
                    var right = Expression.Constant(false);
                    var equal = Expression.Equal(left, right);
                    var filter = Expression.Lambda(equal, entity);
                    entityType.SetQueryFilter(filter);
                }
            }
            #endregion
        }
    }
}
