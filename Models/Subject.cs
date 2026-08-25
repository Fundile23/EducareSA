using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Code { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        public ICollection<ProgrammeSubjectRequirement> ProgrammeRequirements { get; set; }
            = new List<ProgrammeSubjectRequirement>();

        public ICollection<StudentSubjectResult> StudentResults { get; set; }
            = new List<StudentSubjectResult>();
    }
}
