using EducareSA.Models;
using EducareSA.Services;

namespace EducareSA.ViewModels
{
    public class ProgrammeMatchViewModel
    {
        public Programme Programme { get; set; } = null!;
        public EligibilityResult Eligibility { get; set; } = null!;
    }

    public class MatchesPageViewModel
    {
        public List<ProgrammeMatchViewModel> Qualified { get; set; } = new();
        public List<ProgrammeMatchViewModel> NotQualified { get; set; } = new();
        public int Aps { get; set; }
        public bool HasResults { get; set; }
    }
}