using System.ComponentModel.DataAnnotations;

namespace EducareSA.ViewModels
{
    public class StudentProfileViewModel
    {
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(200)]
        [Display(Name = "School")]
        public string? SchoolName { get; set; }

        [Range(8, 12)]
        [Display(Name = "Current grade")]
        public int? Grade { get; set; }

        public int Aps { get; set; }
        public int SubjectCount { get; set; }
    }
}