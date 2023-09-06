using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eppeta.webapi.DTO
{
    public class PerformanceEvaluationRating
    {
        public string PerformanceEvaluationTitle { get; set; } = string.Empty;
        public DateTime ActualDate { get; set; }
        public string EvaluatorName { get; set; } = string.Empty;
        public string EvaluationStatus { get;set; } = string.Empty;
        public string ReviewedCandidateName { get; set; } = string.Empty;
    }
}
