using System.ComponentModel.DataAnnotations;

namespace EducareSA.Models
{
    public class ApplicationPeriod
    {
        public int ApplicationPeriodId { get; set; }

        public int UniversityId { get; set; }

        public int? ProgrammeId { get; set; }

        public int AcademicYear { get; set; }

        public DateTime OpeningDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public string? ApplicationUrl { get; set; }

        public decimal? ApplicationFee { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Upcoming";

        public University University { get; set; } = null!;

        public Programme? Programme { get; set; }
    }
}