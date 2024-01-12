using EdFi.OdsApi.Sdk.Models.All;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eppeta.webapi.Evaluations.Models
{
    [Table("Candidate", Schema = "eppeta")]
    public class Candidate
    {
        [Required]
        [StringLength(64)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(32)]
        public string PersonId { get; set; } = string.Empty;

        [Required]
        public string SourceSystemDescriptor { get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime LastModifiedDate { get; set; }

        [Column("EdFi_Id")]
        [StringLength(50)]
        public string? EdFiId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public static explicit operator Candidate(TpdmCandidate tpdmCandidate)
        {
            return new Candidate
            {
                FirstName = tpdmCandidate.FirstName,
                LastName = tpdmCandidate.LastSurname,
                PersonId = tpdmCandidate.PersonReference.PersonId,
                SourceSystemDescriptor = tpdmCandidate.PersonReference.SourceSystemDescriptor,
                EdFiId = tpdmCandidate.Id,
            };
        }

        public static explicit operator TpdmCandidate(Candidate candidate)
        {
            return new TpdmCandidate
                    (
                        firstName: candidate.FirstName,
                        lastSurname: candidate.LastName,
                        personReference: new EdFiPersonReference
                        {
                            PersonId = candidate.PersonId,
                            SourceSystemDescriptor = candidate.SourceSystemDescriptor
                        }
                    );
        }
    }
}
