using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class University
    {
        public int UniversityId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string ShortName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Url]
        public string? WebsiteUrl { get; set; }

        public string? LogoUrl { get; set; }

        [StringLength(100)]
        public string Province { get; set; } = string.Empty;

        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Campus> Campuses { get; set; } = new List<Campus>();

        public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

        public ICollection<ApplicationPeriod> ApplicationPeriods { get; set; }
            = new List<ApplicationPeriod>();
    }
}