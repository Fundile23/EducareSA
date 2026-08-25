using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class Faculty
    {
        public int FacultyId { get; set; }

        public int UniversityId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public University University { get; set; } = null!;

        public ICollection<Programme> Programmes { get; set; }
            = new List<Programme>();
    }
}