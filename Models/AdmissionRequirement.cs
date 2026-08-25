using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class AdmissionRequirement
    {
        public int AdmissionRequirementId { get; set; }

        public int ProgrammeId { get; set; }

        [Range(0, 100)]
        public decimal? MinimumAPS { get; set; }

        public decimal? MinimumPointScore { get; set; }

        public string? AdditionalRequirements { get; set; }

        public int AcademicYear { get; set; }

        public string? SourceUrl { get; set; }

        public DateTime? LastVerified { get; set; }

        public Programme Programme { get; set; } = null!;
    }
}