using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class Campus
    {
        public int CampusId { get; set; }

        public int UniversityId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(100)]
        public string Province { get; set; } = string.Empty;

        public string? Address { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public bool IsActive { get; set; } = true;

        public University University { get; set; } = null!;

        public ICollection<Programme> Programmes { get; set; }
            = new List<Programme>();
    }
}