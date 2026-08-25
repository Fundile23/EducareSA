using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(200)]
        public string? SchoolName { get; set; }

        public int? Grade { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<StudentSubjectResult> SubjectResults { get; set; }
            = new List<StudentSubjectResult>();
    }
}