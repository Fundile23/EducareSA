using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class CollegeCampus
    {
        public int CollegeCampusId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [Url]
        public string? WebsiteUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int CollegeId { get; set; }

        public College? College { get; set; }
    }
}
