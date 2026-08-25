using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class ProgrammeFee
    {
        public int ProgrammeFeeId { get; set; }

        public int ProgrammeId { get; set; }

        [Required]
        [StringLength(100)]
        public string FeeType { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public int AcademicYear { get; set; }

        public string? Description { get; set; }

        public string? SourceUrl { get; set; }

        public DateTime? LastVerified { get; set; }

        public Programme Programme { get; set; } = null!;
    }
}