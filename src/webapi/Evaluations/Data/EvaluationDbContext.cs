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
        public EvaluationDbContext(DbContextOptions options)
            : base(options)
        {
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

        public async Task<PerformanceEvaluationRating?> GetPerformanceEvaluationRating(int id)
        {
            return await PerformanceEvaluationRatings.FindAsync(id);
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
            modelBuilder.Entity<PerformanceEvaluationRating>().ToTable("PerformanceEvaluationRatings");
        }
    }
}
