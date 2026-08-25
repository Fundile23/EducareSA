using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class StudentSubjectResult
    {
        [Key]
        public int ResultId { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }

        [Range(0, 100)]
        public decimal Percentage { get; set; }

        [Range(1, 7)]
        public int? Level { get; set; }

        public int AcademicYear { get; set; }

        public Student Student { get; set; } = null!;

        public Subject Subject { get; set; } = null!;
    }
}