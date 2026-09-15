using EducareSA.Models;

namespace EducareSA.ViewModels
{
    public class ProgrammeSearchViewModel
    {
        public string? Query { get; set; }
        public int? UniversityId { get; set; }
        public int? FacultyId { get; set; }
        public string? QualificationType { get; set; }

        public List<University> Universities { get; set; } = new();
        public List<Faculty> Faculties { get; set; } = new();
        public List<Programme> Results { get; set; } = new();
        public int TotalCount { get; set; }
    }
}