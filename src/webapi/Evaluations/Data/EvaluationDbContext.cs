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
            Evaluations = Set<Evaluation>();
            PerformanceEvaluations = Set<PerformanceEvaluation>();
            EvaluationElements = Set<EvaluationElement>();
            EvaluationObjectives = Set<EvaluationObjective>();
            PerformanceEvaluationRatings = Set<PerformanceEvaluationRating>();
            EvaluationRatings = Set<EvaluationRating>();
            EvaluationObjectiveRatings = Set<EvaluationObjectiveRating>();
            EvaluationElementRatings = Set<EvaluationElementRating>();
        }

        public async Task<List<Evaluation>> GetAllEvaluations()
        {
            return await Evaluations.ToListAsync();
        }

        public async Task<List<EvaluationElement>> GetAllEvaluationElements()
        {
            return await EvaluationElements.ToListAsync();
        }
        public async Task<List<PerformanceEvaluation>> GetAllPerformanceEvaluations()
        {
            return await PerformanceEvaluations.ToListAsync();
        }
        public async Task<List<EvaluationObjective>> GetAllEvaluationObjectives()
        {
            return await EvaluationObjectives.ToListAsync();
        }
        public async Task UpdateEvaluationObjectives(List<EvaluationObjective> apiEvaluationObjectives)
        {
            // Since the surrogate Id is Identity then match on EdFiId and update existing records
            foreach (var eo in apiEvaluationObjectives)
            {
                var eeo = EvaluationObjectives.Where(eeo => eeo.EdFiId == eo.EdFiId).FirstOrDefault();
                if (eeo != null)
                {
                    foreach (var property in typeof(EvaluationObjective).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eeo, property.GetValue(eo));
                    EvaluationObjectives.Update(eeo);
                }
            }
            // Add new records
            EvaluationObjectives.UpdateRange(apiEvaluationObjectives.Where(eo => EvaluationObjectives.All(eo2 => eo2.EdFiId != eo.EdFiId)).ToList());
            await SaveChangesAsync();
        }

        public async Task UpdateEvaluationElements(List<EvaluationElement> apiEvaluationElements)
        {
            // Since the surrogate Id is Identity then match on EdFiId and update existing records
            foreach (var ee in apiEvaluationElements)
            {
                var eee = EvaluationElements.Where(eee => eee.EdFiId == ee.EdFiId).FirstOrDefault();
                if (eee != null)
                {
                    foreach (var property in typeof(EvaluationElement).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eee, property.GetValue(ee));
                    EvaluationElements.Update(eee);
                }
            }
            // Add new records
            EvaluationElements.UpdateRange(apiEvaluationElements.Where(ee => EvaluationElements.All(ee2 => ee2.EdFiId != ee.EdFiId)).ToList());
            await SaveChangesAsync();
        }

        public async Task UpdateEvaluationRatings(List<EvaluationRating> apiEvaluationRatings)
        {
            // Since the surrogate Id is Identity then match on EdFiId and update existing records
            foreach (var er in apiEvaluationRatings)
            {
                var eer = EvaluationRatings.Where(eer => eer.EdFiId == er.EdFiId).FirstOrDefault();
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationRating).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eer, property.GetValue(er));
                    EvaluationRatings.Update(eer);
                }
            }
            // Add new records
            EvaluationRatings.UpdateRange(apiEvaluationRatings.Where(er => EvaluationElements.All(er2 => er2.EdFiId != er.EdFiId)).ToList());
            await SaveChangesAsync();
        }

        public async Task UpdateEvaluationObjectiveRatings(List<EvaluationObjectiveRating> apiEvaluationObjectiveRatings)
        {
            // Since the surrogate Id is Identity then match on EdFiId and update existing records
            foreach (var er in apiEvaluationObjectiveRatings)
            {
                var eer = EvaluationObjectiveRatings.Where(eer => eer.EdFiId == er.EdFiId).FirstOrDefault();
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationObjectiveRating).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eer, property.GetValue(er));
                    EvaluationObjectiveRatings.Update(eer);
                }
            }
            // Add new records
            EvaluationObjectiveRatings.UpdateRange(apiEvaluationObjectiveRatings.Where(er => EvaluationElements.All(er2 => er2.EdFiId != er.EdFiId)).ToList());
            await SaveChangesAsync();
        }

        public async Task UpdateEvaluationElementRatingResults(List<EvaluationElementRatingResult> apiEvaluationElementRatingResults)
        {
            // Since the surrogate Id is Identity then match on EdFiId and update existing records
            foreach (var er in apiEvaluationElementRatingResults)
            {
                var eer = EvaluationElementRatingResults.Where(eer => eer.EdFiId == er.EdFiId).FirstOrDefault();
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationElementRatingResult).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eer, property.GetValue(er));
                    EvaluationElementRatingResults.Update(eer);
                }
            }
            // Add new records
            EvaluationElementRatingResults.UpdateRange(apiEvaluationElementRatingResults.Where(er => EvaluationElements.All(er2 => er2.EdFiId != er.EdFiId)).ToList());
            await SaveChangesAsync();
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

        public async Task<List<EvaluationRating>> GetEvaluationRatingsByUserId(string userId)
        {
            return await EvaluationRatings.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<List<PerformanceEvaluationRating>> GetPerformanceEvaluationRatingsByUserId(string userId)
        {
            return await PerformanceEvaluationRatings.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<PerformanceEvaluationRating> GetPerformanceEvaluationRatingById(int id)
        {
            return await PerformanceEvaluationRatings.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdatePerformanceEvaluationRating(PerformanceEvaluationRating rating)
        {
            PerformanceEvaluationRatings.Update(rating);
            await SaveChangesAsync();
        }

        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<PerformanceEvaluation> PerformanceEvaluations { get; set; }
        public DbSet<EvaluationElement> EvaluationElements { get; set; }
        public DbSet<EvaluationObjective> EvaluationObjectives { get; set; }
        // DbSet for ratings entities
        public DbSet<PerformanceEvaluationRating> PerformanceEvaluationRatings { get; set; }

        public DbSet<EvaluationRating> EvaluationRatings { get; set; }
        public DbSet<EvaluationElementRatingResult> EvaluationElementRatingResults { get; set; }

        public DbSet<EvaluationObjectiveRating> EvaluationObjectiveRatings { get; set; }

        public DbSet<EvaluationElementRating> EvaluationElementRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("eppeta");
            modelBuilder.Entity<Evaluation>().ToTable(nameof(Evaluation));
            modelBuilder.Entity<EvaluationObjective>().ToTable(nameof(EvaluationObjective));
            modelBuilder.Entity<EvaluationElement>().ToTable(nameof(EvaluationElement));
            modelBuilder.Entity<PerformanceEvaluation>().ToTable(nameof(PerformanceEvaluation));
            modelBuilder.Entity<PerformanceEvaluationRating>().ToTable(nameof(PerformanceEvaluationRating));
            modelBuilder.Entity<EvaluationRating>().ToTable(nameof(EvaluationRating));
            modelBuilder.Entity<EvaluationObjectiveRating>().ToTable(nameof(EvaluationObjectiveRating));
            modelBuilder.Entity<EvaluationElementRating>().ToTable(nameof(EvaluationElementRating));
            modelBuilder.Entity<EvaluationElementRatingResult>().ToTable(nameof(EvaluationElementRatingResult));

            // Configure the Many-to-one relationship between PerformedEvaluation and ApplicationUser
            // Reference the ApplicationUser property in PerformedEvaluation
            // Reference the PerformanceEvaluationRatings ICollection property in ApplicationUser
            // Use the UserId foreign key in PerformedEvaluation
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

        async Task<EvaluationObjective> IEvaluationRepository.GetEvaluationObjectiveById(int id)
        {
            return await EvaluationObjectives.Where(eo => eo.Id == id).FirstOrDefaultAsync();
        }

        async Task<List<EvaluationElement>> IEvaluationRepository.GetAllEvaluationElements()
        {
            return await EvaluationElements.ToListAsync();
        }

        Task<EvaluationElement> IEvaluationRepository.GetEvaluationElementById(int id)
        {
            return EvaluationElements.Where(ee => ee.Id == id).FirstOrDefaultAsync();
        }
    }
}
