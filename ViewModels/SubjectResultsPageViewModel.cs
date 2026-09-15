using EducareSA.Models;

namespace EducareSA.ViewModels
{
    public class SubjectResultsPageViewModel
    {
        public List<StudentSubjectResult> Results { get; set; } = new();
        public List<Subject> AvailableSubjects { get; set; } = new();
        public SubjectResultInputViewModel NewResult { get; set; } = new();
        public int Aps { get; set; }
        public List<ApsBreakdownRow> Breakdown { get; set; } = new();
    }

    public class ApsBreakdownRow
    {
        public string SubjectName { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
        public int Points { get; set; }
        public bool Counted { get; set; }
    }
}