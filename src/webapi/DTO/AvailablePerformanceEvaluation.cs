using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace eppeta.webapi.DTO
{
    public class AvailablePerformanceEvaluation
    {
        public int PerformanceEvaluationId { get; set; }
        public List<AvailableEvaluationObjective> EvaluationObjectives { get; set; } = new List<AvailableEvaluationObjective> { };
        public List<AvailableRatingLevel> RatingLevels { get; set; } = new List<AvailableRatingLevel> { };
        public class AvailableEvaluationObjective
        {
            public string Name { get; set; } = string.Empty;
            public int EvaluationObjectiveId { get; set; }
            public List<AvailableEvaluationElement> EvaluationElements { get; set; } = new List<AvailableEvaluationElement>();
        }
        public class AvailableEvaluationElement
        {
            public string Name { get; set; } = string.Empty;
            public int EvaluationElementId { get; set; }
        }
        public class AvailableRatingLevel
        {
            public string Name { get; set; }
            public int RatingLevel { get; set;}
            public int RatingLevelId { get; set; }
        }
    }
}
