using System.ComponentModel.DataAnnotations;

namespace EducareSA.ViewModels
{
    public class SubjectResultInputViewModel
    {
        public int? ResultId { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "Percentage")]
        public decimal Percentage { get; set; }

        [Range(1, 7)]
        [Display(Name = "NSC level (optional)")]
        public int? Level { get; set; }

        [Required]
        [Range(2000, 2100)]
        [Display(Name = "Academic year")]
        public int AcademicYear { get; set; } = DateTime.UtcNow.Year;
    }
}