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

        public async Task<Status> GetStatusByText(string text)
        {
            return await Statuses.Where(s => s.StatusText.ToLower() == text.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<List<Evaluation>> GetAllEvaluations()
        {
            return await Evaluations.ToListAsync();
        }
        public async Task<PerformanceEvaluation> GetPerformanceEvaluationById(int performanceEvaluationId)
        {
            return await PerformanceEvaluations
                .Include(pe => pe.PerformanceEvaluationRatingLevels)
                .Where(e => e.Id == performanceEvaluationId).FirstOrDefaultAsync();
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
                    foreach (var property in typeof(EvaluationObjective).GetProperties())
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


        public async Task<List<int>> UpdateEvaluationRatings(List<EvaluationRating> evaluationRatings)
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
                    er.Id = ((EvaluationRating)eer).Id;
                }
                else
                    // Add new records
                    EvaluationRatings.Update(er);
            }
            await SaveChangesAsync();
            return evaluationRatings.Select(er => er.Id).ToList();
        }


        public async Task<List<int>> UpdateEvaluationObjectiveRatings(List<EvaluationObjectiveRating> evaluationObjectiveRatings)
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
                    er.Id = (((EvaluationObjectiveRating)eer).Id);
                }
                else
                    // Add new records
                    EvaluationObjectiveRatings.Update(er);
            }
            await SaveChangesAsync();
            return evaluationObjectiveRatings.Select(eor => eor.Id).ToList();
        }

        public async Task<List<int>> UpdateEvaluationElementRatingResults(List<EvaluationElementRatingResult> evaluationElementRatingResults)
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
                    er.Id = ((EvaluationElementRatingResult)eer).Id;
                }
                else
                    // Add new records
                    EvaluationElementRatingResults.Update(er);
            }
            await SaveChangesAsync();
            return evaluationElementRatingResults.Select(eerr => eerr.Id).ToList();
        }
        public async Task UpdatePerformanceEvaluations(List<PerformanceEvaluation> performanceEvaluations)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var pe in performanceEvaluations)
            {
                var epe = FilterByRequiredFields(PerformanceEvaluations
                    .Include(pe => pe.PerformanceEvaluationRatingLevels).ToList(), pe);
                if (epe != null)
                {
                    foreach (var property in typeof(PerformanceEvaluation).GetProperties())
                        if (property.Name != "Id" && property.Name != "PerformanceEvaluationRatingLevels")
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
        public async Task<EvaluationRating> GetEvaluationRatingById(int id)
        {
            return await EvaluationRatings.Where(e => e.Id == id).FirstOrDefaultAsync();
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

        public async Task<List<int>> UpdatePerformanceEvaluationRatings(List<PerformanceEvaluationRating> performanceEvaluationRatings)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var pe in performanceEvaluationRatings)
            {
                var epe = FilterByRequiredFields(PerformanceEvaluationRatings.ToList(), pe);
                if (epe != null)
                {
                    foreach (var property in typeof(PerformanceEvaluationRating).GetProperties())
                        if (property.Name != "Id")
                            property.SetValue(epe, property.GetValue(pe));
                    PerformanceEvaluationRatings.Update((PerformanceEvaluationRating)epe);
                    pe.Id = ((PerformanceEvaluationRating)epe).Id;
                }
                else
                    // Add new records
                    PerformanceEvaluationRatings.Update(pe);
            }
            await SaveChangesAsync();
            return performanceEvaluationRatings.Select(per => per.Id).ToList();
        }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<PerformanceEvaluation> PerformanceEvaluations { get; set; }
        public DbSet<EvaluationElement> EvaluationElements { get; set; }
        public DbSet<EvaluationObjective> EvaluationObjectives { get; set; }
        // DbSet for ratings entities
        public DbSet<PerformanceEvaluationRating> PerformanceEvaluationRatings { get; set; }
        public DbSet<PerformanceEvaluationRatingLevel> PerformanceEvaluationRatingLevels { get; set; }
        public DbSet<EvaluationRating> EvaluationRatings { get; set; }
        public DbSet<EvaluationElementRatingResult> EvaluationElementRatingResults { get; set; }

        public DbSet<EvaluationObjectiveRating> EvaluationObjectiveRatings { get; set; }

        public DbSet<EvaluationElementRating> EvaluationElementRatings { get; set; }

        async Task<EvaluationObjective> IEvaluationRepository.GetEvaluationObjectiveById(int id)
        {
            return await EvaluationObjectives.Where(eo => eo.Id == id).FirstOrDefaultAsync();
        }
        async Task<EvaluationObjectiveRating> IEvaluationRepository.GetEvaluationObjectiveRatingById(int id)
        {
            return await EvaluationObjectiveRatings.Where(eor => eor.Id == id).FirstOrDefaultAsync();
        }
        async Task<List<EvaluationElement>> IEvaluationRepository.GetAllEvaluationElements()
        {
            return await EvaluationElements.ToListAsync();
        }

        Task<EvaluationElement> IEvaluationRepository.GetEvaluationElementById(int id)
        {
            return EvaluationElements.Where(ee => ee.Id == id).FirstOrDefaultAsync();
        }
        Task<EvaluationElementRatingResult> IEvaluationRepository.GetEvaluationElementRatingResultById(int id)
        {
            return EvaluationElementRatingResults.Where(ee => ee.Id == id).FirstOrDefaultAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("eppeta");
            modelBuilder.Entity<Status>().ToTable(nameof(Status));
            modelBuilder.Entity<Evaluation>().ToTable(nameof(Evaluation));
            modelBuilder.Entity<EvaluationObjective>().ToTable(nameof(EvaluationObjective));
            modelBuilder.Entity<EvaluationElement>().ToTable(nameof(EvaluationElement));
            modelBuilder.Entity<PerformanceEvaluation>().ToTable(nameof(PerformanceEvaluation));
            modelBuilder.Entity<PerformanceEvaluationRating>().ToTable(nameof(PerformanceEvaluationRating));
            modelBuilder.Entity<PerformanceEvaluationRatingLevel>().ToTable(nameof(PerformanceEvaluationRatingLevel));
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


    }
}
