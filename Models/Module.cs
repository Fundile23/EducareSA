using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class Module
    {
        public int ModuleId { get; set; }

        public int ProgrammeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int Year { get; set; }

        public int? Semester { get; set; }

        public int? Credits { get; set; }

        public Programme Programme { get; set; } = null!;
    }
}