using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class Bursary
    {
        public int BursaryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Provider { get; set; }

        public string? Description { get; set; }

        [StringLength(500)]
        public string? Coverage { get; set; }

        [Url]
        public string? WebsiteUrl { get; set; }

        public DateTime? OpeningDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        [Range(0, 100)]
        public decimal? MinimumAPS { get; set; }

        public string? EligibilityNotes { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}