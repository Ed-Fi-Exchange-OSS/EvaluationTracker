// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Evaluations.Models;
using eppeta.webapi.Identity.Data;
using eppeta.webapi.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eppeta.webapi.Evaluations.Data
{
    public class EvaluationDbContext : DbContext, IEvaluationRepository
    {
        public EvaluationDbContext(DbContextOptions<EvaluationDbContext> options)
            : base(options)
        {
            PerformanceEvaluationRatings = Set<PerformanceEvaluationRating>();
            EvaluationRatings = Set<EvaluationRating>();
            EvaluationObjectiveRatings = Set<EvaluationObjectiveRating>();
            EvaluationElementRatings = Set<EvaluationElementRating>();
        }

        public async Task CreatePerformanceEvaluationRating(PerformanceEvaluationRating rating)
        {
            PerformanceEvaluationRatings.Add(rating);
            await SaveChangesAsync();
        }

        public async Task<List<PerformanceEvaluationRating>> GetAllPerformanceEvaluationRatings()
        {
            return await PerformanceEvaluationRatings.ToListAsync();
        }

        public async Task<List<PerformanceEvaluationRating>> GetPerformanceEvaluationRating(string userId)
        {
            return await PerformanceEvaluationRatings.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task UpdatePerformanceEvaluationRating(PerformanceEvaluationRating rating)
        {
            PerformanceEvaluationRatings.Update(rating);
            await SaveChangesAsync();
        }

        // DbSet for ratings entities
        public DbSet<PerformanceEvaluationRating> PerformanceEvaluationRatings { get; set; }

        public DbSet<EvaluationRating> EvaluationRatings { get; set; }

        public DbSet<EvaluationObjectiveRating> EvaluationObjectiveRatings { get; set; }

        public DbSet<EvaluationElementRating> EvaluationElementRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("eppeta");
            modelBuilder.Entity<PerformanceEvaluationRating>().ToTable("PerformanceEvaluationRating");
            modelBuilder.Entity<EvaluationRating>().ToTable("EvaluationRating");
            modelBuilder.Entity<EvaluationObjectiveRating>().ToTable("EvaluationObjectiveRating");
            modelBuilder.Entity<EvaluationElementRating>().ToTable("EvaluationElementRating");

            // Configure the Many-to-one relationship between PerformanceEvaluationRating and ApplicationUser
            // Reference the ApplicationUser property in PerformanceEvaluationRating
            // Reference the PerformanceEvaluationRatings ICollection property in ApplicationUser
            // Use the UserId foreign key in PerformanceEvaluationRating
            modelBuilder.Entity<PerformanceEvaluationRating>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(u => u.PerformanceEvaluationRatings)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // EF couldn't determine the appropriate column type for the Rating property
            modelBuilder.Entity<EvaluationElementRating>()
                .Property(e => e.Rating)
                .HasColumnType("decimal(6, 3)");
        }
    }
}
