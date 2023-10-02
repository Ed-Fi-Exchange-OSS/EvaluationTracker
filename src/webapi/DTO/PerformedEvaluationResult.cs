using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace eppeta.webapi.DTO
{
    public class PerformedEvaluationResult
    {
        public string ReviewedPersonId { get; set; } = string.Empty;
        public string ReviewedPersonIdSourceSystemDescriptor { get; set; } = string.Empty;
        public int PerformanceEvaluationId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; } 
        public List<PerformedEvaluationResultObjective> ObjectiveResults { get; set; } = new List<PerformedEvaluationResultObjective>();
        public class PerformedEvaluationResultObjective
        {
            public int Id { get; set; }
            public string Comment { get; set; } = string.Empty;
            public List<PerformedEvaluationResultElement> Elements { get; set; } = new List<PerformedEvaluationResultElement>();
        }
        public class PerformedEvaluationResultElement
        {
            public int Id { get; set; }
            public int Score { get; set; }
        }
    }
}
