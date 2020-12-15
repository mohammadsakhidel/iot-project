﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TrackDataAccess.Models;
using TrackDataAccess.Models.Identity;

namespace TrackDataAccess.Database {
    public class TrackDbContext : IdentityDbContext<AppUser> {

        public TrackDbContext(DbContextOptions<TrackDbContext> options) : base(options) {
        }

        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<CommandLog> CommandLogs { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);

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

            #region Resolving MySQL issue with key length
            modelBuilder.Entity<AppUser>().Property(user => user.Id).HasMaxLength(256);
            modelBuilder.Entity<IdentityRole>().Property(r => r.Id).HasMaxLength(256);
            modelBuilder.Entity<IdentityUserLogin<string>>().Property(ul => ul.LoginProvider).HasMaxLength(256);
            modelBuilder.Entity<IdentityUserLogin<string>>().Property(ul => ul.ProviderKey).HasMaxLength(256);
            modelBuilder.Entity<IdentityUserToken<string>>().Property(ul => ul.LoginProvider).HasMaxLength(256);
            modelBuilder.Entity<IdentityUserToken<string>>().Property(ul => ul.Name).HasMaxLength(256);
            #endregion

            #region Identity Customization:
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Claims)
                .WithOne()
                .HasForeignKey(c => c.UserId);
            #endregion

        }
    }
}
