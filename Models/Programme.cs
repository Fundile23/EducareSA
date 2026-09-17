using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EducareSA.Models
{
    public class Programme
    {
        public int ProgrammeId { get; set; }

        public int FacultyId { get; set; }

        public int? CampusId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string QualificationType { get; set; } = string.Empty;

        [StringLength(100)]
        public string? QualificationCode { get; set; }

        public int? NQFLevel { get; set; }

        public decimal? DurationYears { get; set; }

        public string? Description { get; set; }

        public string? CareerInformation { get; set; }

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// True when requirements, fees, and APS numbers for this programme
        /// have been verified against the university's official prospectus.
        /// Drives the "unverified data" badge on the public page.
        /// </summary>
        public bool DataVerified { get; set; } = false;

        /// <summary>
        /// Link to the official prospectus page where the data was confirmed.
        /// </summary>
        [StringLength(500)]
        public string? SourceUrl { get; set; }

        /// <summary>
        /// When the data was last checked against the source. Null = never.
        /// </summary>
        public DateTime? LastVerified { get; set; }

        public Faculty Faculty { get; set; } = null!;

        public Campus? Campus { get; set; }

        public ICollection<ProgrammeSubjectRequirement> SubjectRequirements { get; set; }
            = new List<ProgrammeSubjectRequirement>();

        public ICollection<AdmissionRequirement> AdmissionRequirements { get; set; }
            = new List<AdmissionRequirement>();

        public ICollection<ProgrammeFee> Fees { get; set; }
            = new List<ProgrammeFee>();

        public ICollection<ApplicationPeriod> ApplicationPeriods { get; set; }
            = new List<ApplicationPeriod>();

        public ICollection<Module> Modules { get; set; }
            = new List<Module>();
    }
}
