using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eppeta.webapi.DTO
{
    public class Candidate
    {
        public string CandidateName { get; set; } = string.Empty;
        public string PersonId { get; set; } = string.Empty;
        public string SourceSystemDescriptor { get; set; } = string.Empty;
    }
}
