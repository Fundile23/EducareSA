using System.ComponentModel.DataAnnotations;  
using System.ComponentModel.DataAnnotations.Schema;

namespace EducareSA.Models
{
    public class College
    {
        public int CollegeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Url]
        public string? WebsiteUrl { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CollegeCampus> Campuses { get; set; }
            = new List<CollegeCampus>();

        public ICollection<CollegeProgramme> Programmes { get; set; }
            = new List<CollegeProgramme>();
    }
}
