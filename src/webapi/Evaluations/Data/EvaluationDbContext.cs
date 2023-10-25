// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Evaluations.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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
        public async Task<PerformanceEvaluation> GetPerformanceEvaluationById(int performanceEvaluationId)
        {
            return await PerformanceEvaluations.Where(e => e.Id == performanceEvaluationId).FirstOrDefaultAsync();
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

        private object FilterByRequiredFields<T>(List<T> listToFilter, object referenceObject)
        {
            var requiredProperties = referenceObject.GetType().GetProperties()
                .Where(p => p.IsDefined(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true)
                    && p.Name != "EdFiId").ToList();
            List<string> colFilter = new List<string>();
            foreach (var property in requiredProperties)
            {
                if (property.PropertyType == typeof(DateTime))
                {
                    // SQL needs exact datetime format
                    string dateTimeVal = ((DateTime)property.GetValue(referenceObject)).ToString("yyyy-MM-dd HH:mm:ss.FFF");
                    colFilter.Add($"{property.Name} == \"{dateTimeVal}\"");
                }
                else
                    colFilter.Add($"{property.Name} == \"{property.GetValue(referenceObject)}\"");
            }
            string whereClause = String.Join(" and ", colFilter);
            return listToFilter.AsQueryable().Where(whereClause).FirstOrDefault();
        }

        public async Task UpdateEvaluationObjectives(List<EvaluationObjective> evaluationObjectives)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var eo in evaluationObjectives)
            {
                var eeo = FilterByRequiredFields(EvaluationObjectives.ToList(), eo);
                if (eeo != null)
                {
                    foreach (var property in typeof(EvaluationElement).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eeo, property.GetValue(eo));
                    EvaluationObjectives.Update((EvaluationObjective)eeo);
                }
                else
                    // Add new records
                    EvaluationObjectives.UpdateRange(eo);
            }
            await SaveChangesAsync();
        }

        public async Task UpdateEvaluationElements(List<EvaluationElement> evaluationElements)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var ee in evaluationElements)
            {
                var eee = FilterByRequiredFields(EvaluationElements.ToList(), ee);
                if (eee != null)
                {
                    foreach (var property in typeof(EvaluationElement).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eee, property.GetValue(ee));
                    EvaluationElements.Update((EvaluationElement)eee);
                }
                else
                    // Add new records
                    EvaluationElements.Update(ee);
            }
            await SaveChangesAsync();
        }


        public async Task UpdateEvaluationRatings(List<EvaluationRating> evaluationRatings)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var er in evaluationRatings)
            {
                var eer = FilterByRequiredFields(EvaluationRatings.ToList(), er);
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationRating).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eer, property.GetValue(er));
                    EvaluationRatings.Update((EvaluationRating)eer);
                }
                else
                    // Add new records
                    EvaluationRatings.Update(er);
            }
            await SaveChangesAsync();
        }


        public async Task UpdateEvaluationObjectiveRatings(List<EvaluationObjectiveRating> evaluationObjectiveRatings)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var er in evaluationObjectiveRatings)
            {
                var eer = FilterByRequiredFields(EvaluationObjectiveRatings.ToList(), er);
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationObjectiveRating).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eer, property.GetValue(er));
                    EvaluationObjectiveRatings.Update((EvaluationObjectiveRating)eer);
                }
                else
                    // Add new records
                    EvaluationObjectiveRatings.Update(er);
            }
            await SaveChangesAsync();
        }

        public async Task UpdateEvaluationElementRatingResults(List<EvaluationElementRatingResult> evaluationElementRatingResults)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var er in evaluationElementRatingResults)
            {
                var eer = FilterByRequiredFields(EvaluationElementRatingResults.ToList(), er);
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationElementRatingResult).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(eer, property.GetValue(er));
                    EvaluationElementRatingResults.Update((EvaluationElementRatingResult)eer);
                }
                else
                    // Add new records
                    EvaluationElementRatingResults.Update(er);
            }
            await SaveChangesAsync();
        }
        public async Task UpdatePerformanceEvaluations(List<PerformanceEvaluation> performanceEvaluations)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var pe in performanceEvaluations)
            {
                var epe = FilterByRequiredFields(PerformanceEvaluations.ToList(), pe);
                if (epe != null)
                {
                    foreach (var property in typeof(PerformanceEvaluation).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(epe, property.GetValue(pe));
                    PerformanceEvaluations.Update((PerformanceEvaluation)epe);
                }
                else
                    // Add new records
                    PerformanceEvaluations.Update(pe);
            }
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
