// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Evaluations.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Runtime.ConstrainedExecution;

namespace eppeta.webapi.Evaluations.Data
{

    // Disabled because Entity Framework sets the DbSets collections and we don't need to guard against null as we normally would.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8603 // Possible null reference
    public class EvaluationDbContext : DbContext, IEvaluationRepository
    {

        public EvaluationDbContext(DbContextOptions<EvaluationDbContext> options)
            : base(options)
        {
        }

        public async Task<List<Status>> GetAllStatuses()
        {
            return await Statuses.ToListAsync();
        }
        public async Task<Status> GetStatusByText(string text)
        {
            return await Statuses.Where(s => s.StatusText.ToLower() == text.ToLower()).FirstOrDefaultAsync();
        }
        public async Task<Status> GetStatusById(int id)
        {
            return await Statuses.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Evaluation>> GetAllEvaluations()
        {
            return await Evaluations.ToListAsync();
        }
        public async Task<Evaluation> GetEvaluationById(int id)
        {
            return await Evaluations.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
        public Task<List<Evaluation>> GetEvaluationsByPK(object samePKObject)
        {
            var es = FilterByRequiredFields(Evaluations.ToList(), samePKObject);
            return es.Any() ? Task.FromResult(es) : (Task<List<Evaluation>>?)null;
        }
        public Task<List<PerformanceEvaluation>> GetPerformanceEvaluationByPK(object samePKObject)
        {
            var es = FilterByRequiredFields(PerformanceEvaluations.ToList(), samePKObject);
            return es.Any() ? Task.FromResult(es) : (Task<List<PerformanceEvaluation>>?)null;
        }
        public async Task UpdateEvaluations(List<Evaluation> evaluations)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var e in evaluations)
            {
                var ee = FilterByRequiredFields(Evaluations.ToList(), e).FirstOrDefault();
                if (ee != null)
                {
                    foreach (var property in typeof(Evaluation).GetProperties())
                    {
                        if (property.Name != "Id")
                        {
                            property.SetValue(ee, property.GetValue(e));
                        }
                    }

                    _ = Evaluations.Update(ee);
                    e.Id = ee.Id;
                }
                else
                {
                    // Add new records
                    _ = Evaluations.Update(e);
                }
            }
            _ = await SaveChangesAsync();
        }

        public async Task<List<EvaluationRating>> GetAllEvaluationRatings()
        {
            return await EvaluationRatings.ToListAsync();
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
        public async Task<List<Candidate>> GetAllCandidates()
        {
            return await Candidates.ToListAsync();
        }

        private static List<T> FilterByRequiredFields<T>(List<T> listToFilter, object referenceObject)
        {
            if (!listToFilter.Any())
            {
                return listToFilter;
            }

            var requiredReferenceProperties = referenceObject.GetType().GetProperties()
                .Where(p => p.IsDefined(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true)
                    && p.Name != "EdFiId").Select(p => p.Name).ToList();
            var requiredListProperties = listToFilter?.First()?.GetType().GetProperties()
                .Where(p => p.IsDefined(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true)
                    && p.Name != "EdFiId").Select(p => p.Name).ToList();

            requiredListProperties = requiredListProperties?.Intersect(requiredReferenceProperties).ToList();

            var colFilter = new List<string>();
            if (requiredListProperties != null)
            {
                foreach (var propertyName in requiredListProperties)
                {
                    var property = listToFilter?.First()?.GetType().GetProperty(propertyName);
                    var refProperty = referenceObject.GetType().GetProperty(propertyName);
                    if (property?.PropertyType == typeof(DateTime))
                    {
                        // SQL needs exact datetime format
                        // Disabled: `propertyName` cannot be null, due to filter in the foreach
#pragma warning disable CS8605 // Unboxing a possibly null value.
                        var dateTimeVal = ((DateTime)refProperty?.GetValue(referenceObject)).ToString("yyyy-MM-dd HH:mm:ss.FFF");
#pragma warning restore CS8605 // Unboxing a possibly null value.
                        colFilter.Add($"{propertyName} == \"{dateTimeVal}\"");
                    }
                    else
                    {
                        colFilter.Add($"{propertyName} == \"{refProperty?.GetValue(referenceObject)}\"");
                    }
                }
            }
            var whereClause = string.Join(" and ", colFilter);
            return listToFilter?.AsQueryable().Where(whereClause).ToList();
        }

        public async Task UpdateEvaluationObjectives(List<EvaluationObjective> evaluationObjectives)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var eo in evaluationObjectives)
            {
                var eeo = FilterByRequiredFields(EvaluationObjectives.ToList(), eo).FirstOrDefault();
                if (eeo != null)
                {
                    foreach (var property in typeof(EvaluationObjective).GetProperties())
                    {
                        if (property.Name == "LastModifiedDate")
                        {
                            property.SetValue(eeo, DateTime.Now);
                        }
                        else if (property.Name != "Id")
                        {
                            property.SetValue(eeo, property.GetValue(eo));
                        }
                    }
                    _ = EvaluationObjectives.Update(eeo);
                }
                else
                {
                    // Add new records
                    EvaluationObjectives.UpdateRange(eo);
                }
            }
            _ = await SaveChangesAsync();
        }

        public async Task UpdateEvaluationElements(List<EvaluationElement> evaluationElements)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var ee in evaluationElements)
            {
                var eee = FilterByRequiredFields(EvaluationElements.ToList(), ee).FirstOrDefault();
                if (eee != null)
                {
                    foreach (var property in typeof(EvaluationElement).GetProperties())
                    {
                        if (property.Name != "Id")
                        {
                            property.SetValue(eee, property.GetValue(ee));
                        }
                    }

                    _ = EvaluationElements.Update(eee);
                }
                else
                {
                    // Add new records
                    _ = EvaluationElements.Update(ee);
                }
            }
            _ = await SaveChangesAsync();
        }


        public async Task<List<int>> UpdateEvaluationRatings(List<EvaluationRatingForUpdate> evaluationRatings)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var er in evaluationRatings)
            {
                var eer = FilterByRequiredFields(EvaluationRatings.ToList(), er).FirstOrDefault();
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationRating).GetProperties())
                    {
                        if (property.Name == "LastModifiedDate")
                        {
                            property.SetValue(eer, DateTime.Now);
                        }
                        else if (property.Name != "Id" && property.Name != "CreateDate")
                        {
                            property.SetValue(eer, property.GetValue(er));
                        }
                    }

                    if (er.NewEvaluationDate.HasValue && er.EvaluationDate != er.NewEvaluationDate)
                        eer.EvaluationDate = er.NewEvaluationDate.Value;

                    _ = EvaluationRatings.Update(eer);
                    er.Id = eer.Id;
                }
                else
                {
                    // Add new records
                    _ = EvaluationRatings.Update(er);
                }
            }
            _ = await SaveChangesAsync();
            return evaluationRatings.Select(er => er.Id).ToList();
        }


        public async Task<List<int>> UpdateEvaluationObjectiveRatings(List<EvaluationObjectiveRatingForUpdate> evaluationObjectiveRatings)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var er in evaluationObjectiveRatings)
            {
                var eer = FilterByRequiredFields(EvaluationObjectiveRatings.ToList(), er).FirstOrDefault();
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationObjectiveRating).GetProperties())
                    {
                        if (property.Name == "LastModifiedDate")
                        {
                            property.SetValue(eer, DateTime.Now);
                        }
                        else if (property.Name != "Id" && property.Name != "CreateDate")
                        {
                            property.SetValue(eer, property.GetValue(er));
                        }
                    }

                    if (er.NewEvaluationDate.HasValue  && er.EvaluationDate != er.NewEvaluationDate)
                        eer.EvaluationDate = er.NewEvaluationDate.Value;

                    _ = EvaluationObjectiveRatings.Update(eer);
                    er.Id = eer.Id;
                }
                else
                {
                    // Add new records
                    _ = EvaluationObjectiveRatings.Update(er);
                }
            }
            _ = await SaveChangesAsync();
            return evaluationObjectiveRatings.Select(eor => eor.Id).ToList();
        }

        public async Task<List<int>> UpdateEvaluationElementRatingResults(List<EvaluationElementRatingResultForUpdate> evaluationElementRatingResults)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var er in evaluationElementRatingResults)
            {
                var eer = FilterByRequiredFields(EvaluationElementRatingResults.ToList(), er).FirstOrDefault();
                if (eer != null)
                {
                    foreach (var property in typeof(EvaluationElementRatingResult).GetProperties())
                    {
                        if (property.Name != "Id" && property.Name != "CreateDate")
                        {
                            property.SetValue(eer, property.GetValue(er));
                        }
                    }
                    
                    if (er.NewEvaluationDate.HasValue  && er.EvaluationDate != er.NewEvaluationDate)
                        eer.EvaluationDate = er.NewEvaluationDate.Value;

                    _ = EvaluationElementRatingResults.Update(eer);
                    er.Id = eer.Id;
                }
                else
                {
                    // Add new records
                    _ = EvaluationElementRatingResults.Update(er);
                }
            }
            _ = await SaveChangesAsync();
            return evaluationElementRatingResults.Select(eerr => eerr.Id).ToList();
        }
        public async Task UpdatePerformanceEvaluations(List<PerformanceEvaluation> performanceEvaluations)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var pe in performanceEvaluations)
            {
                var epe = FilterByRequiredFields(PerformanceEvaluations
                    .Include(pe => pe.PerformanceEvaluationRatingLevels).ToList(), pe).FirstOrDefault();
                if (epe != null)
                {
                    foreach (var property in typeof(PerformanceEvaluation).GetProperties())
                    {
                        if (property.Name is not "Id" and not "PerformanceEvaluationRatingLevels")
                        {
                            property.SetValue(epe, property.GetValue(pe));
                        }
                    }

                    _ = PerformanceEvaluations.Update(epe);
                }
                else
                {
                    // Add new records
                    _ = PerformanceEvaluations.Update(pe);
                }
            }
            _ = await SaveChangesAsync();
        }

        public async Task UpdateCandidates(List<Candidate> candidates)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var ca in candidates)
            {
                var candidate = FilterByRequiredFields(Candidates.ToList(), ca).FirstOrDefault();
                if (candidate != null)
                {
                    foreach (var property in typeof(Candidate).GetProperties())
                    {
                        if (property.Name != "Id" && property.PropertyType != typeof(DateTime))
                        {
                            property.SetValue(candidate, property.GetValue(ca));
                        }
                    }

                    _ = Candidates.Update(candidate);
                }
                else
                {
                    // Add new records
                    _ = Candidates.Update(ca);
                }
            }
            _ = await SaveChangesAsync();
        }

        public async Task CreatePerformanceEvaluationRating(PerformanceEvaluationRating rating)
        {
            _ = PerformanceEvaluationRatings.Add(rating);
            _ = await SaveChangesAsync();
        }

        public async Task<List<PerformanceEvaluationRating>> GetAllPerformanceEvaluationRatings()
        {
            return await PerformanceEvaluationRatings.Include(m => m.RecordStatus).ToListAsync();
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
            return await PerformanceEvaluationRatings.Include(m => m.RecordStatus).Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<PerformanceEvaluationRating> GetPerformanceEvaluationRatingById(int id)
        {
            return await PerformanceEvaluationRatings.Include(m => m.RecordStatus).Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<int>> UpdatePerformanceEvaluationRatings(List<PerformanceEvaluationRatingForUpdate> performanceEvaluationRatings)
        {
            // Since the surrogate Id is Identity then match on required cols and update existing records
            foreach (var pe in performanceEvaluationRatings)
            {
                var epe = FilterByRequiredFields(PerformanceEvaluationRatings.ToList(), pe).FirstOrDefault();
                if (epe != null)
                {
                    foreach (var property in typeof(PerformanceEvaluationRating).GetProperties())
                    {
                        if (property.Name == "LastModifiedDate")
                        {
                            property.SetValue(epe, DateTime.Now);
                        }
                        else if (property.Name != "Id" && property.Name != "CreateDate")
                        {
                            property.SetValue(epe, property.GetValue(pe));
                        }
                    }

                    if (pe.NewStartTime.HasValue && pe.StartTime != pe.NewStartTime)
                        epe.StartTime = pe.NewStartTime.Value;

                    _ = PerformanceEvaluationRatings.Update(epe);
                    pe.Id = epe.Id;
                }
                else
                {
                    // Add new records
                    _ = PerformanceEvaluationRatings.Update(pe);
                }
            }
            _ = await SaveChangesAsync();
            return performanceEvaluationRatings.Select(per => per.Id).ToList();
        }

        public async Task UpdatePerformanceEvaluationRatingStatus(int id, string newStatus)
        {
            var performanceEvaluationRating = PerformanceEvaluationRatings.FirstOrDefault(p => p.Id == id);
            if (performanceEvaluationRating != null)
            {
                performanceEvaluationRating.StatusId = (await GetStatusByText(newStatus)).Id;
                _ = PerformanceEvaluationRatings.Update(performanceEvaluationRating);
                _ = await SaveChangesAsync();
            }
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

        public DbSet<Candidate> Candidates { get; set; }

        async Task<EvaluationObjective> IEvaluationRepository.GetEvaluationObjectiveById(int id)
        {
            return await EvaluationObjectives.Where(eo => eo.Id == id).FirstOrDefaultAsync();
        }
        public async Task<EvaluationObjectiveRating> GetEvaluationObjectiveRatingById(int id)
        {
            return await EvaluationObjectiveRatings.Where(eor => eor.Id == id).FirstOrDefaultAsync();
        }
        public Task<EvaluationElement> GetEvaluationElementById(int id)
        {
            return EvaluationElements.Where(ee => ee.Id == id).FirstOrDefaultAsync()!;
        }
        public Task<EvaluationElementRatingResult> GetEvaluationElementRatingResultById(int id)
        {
            return EvaluationElementRatingResults.Where(ee => ee.Id == id).FirstOrDefaultAsync()!;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);
                _ = modelBuilder.HasDefaultSchema("eppeta");
                _ = modelBuilder.Entity<Status>().ToTable(nameof(Status));

                _ = modelBuilder.Entity<Candidate>().ToTable(nameof(Candidate)).Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                _ = modelBuilder.Entity<Candidate>().Property(e => e.LastModifiedDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<Evaluation>().ToTable(nameof(Evaluation)).Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                _ = modelBuilder.Entity<Evaluation>().Property(e => e.LastModifiedDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<EvaluationObjective>().ToTable(nameof(EvaluationObjective)).Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                _ = modelBuilder.Entity<EvaluationObjective>().Property(e => e.LastModifiedDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<EvaluationElement>().ToTable(nameof(EvaluationElement)).Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
                _ = modelBuilder.Entity<EvaluationElement>().Property(x => x.LastModifiedDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<PerformanceEvaluation>().ToTable(nameof(PerformanceEvaluation)).Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
                _ = modelBuilder.Entity<PerformanceEvaluation>().Property(x => x.LastModifiedDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<PerformanceEvaluationRating>().ToTable(nameof(PerformanceEvaluationRating)).Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
                _ = modelBuilder.Entity<PerformanceEvaluationRating>().Property(x => x.LastModifiedDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<PerformanceEvaluationRatingLevel>().ToTable(nameof(PerformanceEvaluationRatingLevel));

                _ = modelBuilder.Entity<EvaluationRating>().ToTable(nameof(EvaluationRating)).Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
                _ = modelBuilder.Entity<EvaluationRating>().Property(x => x.LastModifiedDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<EvaluationObjectiveRating>().ToTable(nameof(EvaluationObjectiveRating)).Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
                _ = modelBuilder.Entity<EvaluationObjectiveRating>().Property(x => x.LastModifiedDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<EvaluationElementRating>().ToTable(nameof(EvaluationElementRating)).Property(e => e.CreateDate).HasDefaultValueSql("getdate()");

                _ = modelBuilder.Entity<EvaluationElementRatingResult>().ToTable(nameof(EvaluationElementRatingResult)).Property(e => e.CreateDate).HasDefaultValueSql("getdate()");

                // Configure the Many-to-one relationship between PerformedEvaluation and ApplicationUser
                // Reference the ApplicationUser propertyName in PerformedEvaluation
                // Reference the PerformanceEvaluationRatings ICollection propertyName in ApplicationUser
                // Use the UserId foreign key in PerformedEvaluation
                _ = modelBuilder.Entity<PerformanceEvaluationRating>()
                    .HasOne(p => p.ApplicationUser)
                    .WithMany(u => u.PerformanceEvaluationRatings)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // EF couldn't determine the appropriate column type for the Rating propertyName
                _ = modelBuilder.Entity<EvaluationElementRating>()
                    .Property(e => e.Rating)
                    .HasColumnType("decimal(6, 3)");
            }
            catch (Exception ex)
            {
                var et = ex;
            }
        }
        public Task<List<PerformanceEvaluation>> GetPerformanceEvaluationsByPK(object samePKObject)
        {
            var pes = FilterByRequiredFields(PerformanceEvaluations.ToList(), samePKObject);
            return pes.Any() ? Task.FromResult(pes) : Task.FromResult<List<PerformanceEvaluation>>(null!);
        }
        public Task<List<PerformanceEvaluationRating>> GetPerformanceEvaluationRatingsByPK(object samePKObject)
        {
            var per = FilterByRequiredFields(PerformanceEvaluationRatings.Include(m => m.RecordStatus).ToList(), samePKObject);
            return per.Any() ? Task.FromResult(per) : Task.FromResult<List<PerformanceEvaluationRating>>(null!);
        }

        public Task<List<EvaluationObjectiveRating>> GetEvaluationObjectiveRatingsByPK(object samePKObject)
        {
            var eos = FilterByRequiredFields(EvaluationObjectiveRatings.ToList(), samePKObject);
            return eos.Any() ? Task.FromResult(eos) : Task.FromResult<List<EvaluationObjectiveRating>>(null!);
        }

        public Task<List<EvaluationElementRatingResult>> GetEvaluationElementRatingResultsByPK(object samePKObject)
        {
            var es = FilterByRequiredFields(EvaluationElementRatingResults.ToList(), samePKObject);
            return es.Any() ? Task.FromResult(es) : Task.FromResult<List<EvaluationElementRatingResult>>(null!);
        }

        public Task<List<EvaluationObjective>> GetEvaluationObjectivesByPK(object samePKObject)
        {
            var eos = FilterByRequiredFields(EvaluationObjectives.ToList(), samePKObject);
            return eos.Any() ? Task.FromResult(eos) : Task.FromResult<List<EvaluationObjective>>(null!);
        }

        public Task<List<EvaluationElement>> GetEvaluationElementsByPK(object samePKObject)
        {
            var es = FilterByRequiredFields(EvaluationElements.ToList(), samePKObject);
            return es.Any() ? Task.FromResult(es) : Task.FromResult<List<EvaluationElement>>(null!);
        }

        public Task<List<EvaluationRating>> GetEvaluationRatingsByPK(object samePKObject)
        {
            var er = FilterByRequiredFields(EvaluationRatings.ToList(), samePKObject);
            return er.Any() ? Task.FromResult(er) : Task.FromResult<List<EvaluationRating>>(null!);
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
