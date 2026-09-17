using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class ProgrammeSubjectRequirement
    {
        [Key]
        public int RequirementId { get; set; }

        public int ProgrammeId { get; set; }

        public int? RequirementGroup { get; set; }

        public int SubjectId { get; set; }

        [Range(0, 100)]
        public decimal MinimumPercentage { get; set; }

        public bool Required { get; set; } = true;

        public int? MinimumLevel { get; set; }

        [StringLength(500)]
        public string? SourceUrl { get; set; }

        public DateTime? LastVerified { get; set; }

        public string? Notes { get; set; }

        public Programme Programme { get; set; } = null!;

        public Subject Subject { get; set; } = null!;
    }
}