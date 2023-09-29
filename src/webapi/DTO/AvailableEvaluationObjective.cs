using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace eppeta.webapi.DTO
{
    public class AvailableEvaluationObjective
    {
        public string Name { get; set; } = string.Empty;
        public int EvaluationObjectiveId { get; set; }
        public List<AvailableEvaluationElement> EvaluationElements { get; set; } = new List<AvailableEvaluationElement>();

        public class AvailableEvaluationElement
        {
            public string Name { get; set; } = string.Empty;
            public int EvaluationElementId { get; set; }
        }
    }
}
