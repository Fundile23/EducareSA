using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class CollegeProgramme
    {
        public int CollegeProgrammeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ProgrammeType { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NQFLevel { get; set; }

        [StringLength(500)]
        public string? MinimumEntryRequirement { get; set; }

        [Range(0, 100)]
        public decimal? MinimumAPS { get; set; }

        public string? RequiredSubjects { get; set; }

        [StringLength(100)]
        public string? Duration { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int CollegeId { get; set; }

        public College? College { get; set; }
    }
}
