using EducareSA.Data;
using EducareSA.Models;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Services
{
    public enum EligibilityStatus { Qualified, NotQualified, MissingData }

    public class EligibilityCheck
    {
        public string Description { get; init; } = string.Empty;
        public bool Passed { get; init; }
    }

    public class EligibilityResult
    {
        public EligibilityStatus Status { get; init; }
        public int StudentAps { get; init; }
        public int? RequiredAps { get; init; }
        public IReadOnlyList<EligibilityCheck> Checks { get; init; } = Array.Empty<EligibilityCheck>();
        public IReadOnlyList<string> FailureReasons { get; init; } = Array.Empty<string>();

        public bool IsQualified => Status == EligibilityStatus.Qualified;
    }

    public interface IEligibilityService
    {
        Task<EligibilityResult> EvaluateAsync(int studentId, int programmeId, int academicYear);
    }

    public class EligibilityService : IEligibilityService
    {
        private readonly EducareDbContext _context;
        private readonly IApsCalculator _aps;

        public EligibilityService(EducareDbContext context, IApsCalculator aps)
        {
            _context = context;
            _aps = aps;
        }

        public async Task<EligibilityResult> EvaluateAsync(int studentId, int programmeId, int academicYear)
        {
            var student = await _context.Students
                .Include(s => s.SubjectResults)
                    .ThenInclude(r => r.Subject)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return new EligibilityResult { Status = EligibilityStatus.MissingData };

            var programme = await _context.Programmes
                .Include(p => p.SubjectRequirements).ThenInclude(r => r.Subject)
                .Include(p => p.AdmissionRequirements)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProgrammeId == programmeId);

            if (programme == null)
                return new EligibilityResult { Status = EligibilityStatus.MissingData };

            var results = student.SubjectResults
                .Where(r => r.AcademicYear == academicYear)
                .ToList();

            if (results.Count == 0)
                return new EligibilityResult { Status = EligibilityStatus.MissingData };

            var aps = _aps.CalculateForStudent(results);
            var admission = programme.AdmissionRequirements
                .FirstOrDefault(a => a.AcademicYear == academicYear)
                ?? programme.AdmissionRequirements.FirstOrDefault();

            var checks = new List<EligibilityCheck>();
            var failures = new List<string>();

            // 1. APS
            if (admission?.MinimumAPS is decimal minAps)
            {
                var passed = aps >= minAps;
                checks.Add(new EligibilityCheck
                {
                    Description = $"APS requirement: {minAps:0}",
                    Passed = passed
                });
                if (!passed)
                    failures.Add($"APS requires {minAps:0}; student has {aps}.");
            }

            // 2. Required subjects
            foreach (var req in programme.SubjectRequirements)
            {
                var match = results.FirstOrDefault(r => r.SubjectId == req.SubjectId);
                var subjectName = req.Subject?.Name ?? $"Subject {req.SubjectId}";

                if (match == null)
                {
                    checks.Add(new EligibilityCheck
                    {
                        Description = $"{subjectName}: required, not provided",
                        Passed = !req.Required
                    });
                    if (req.Required)
                        failures.Add($"{subjectName} is required but not provided.");
                    continue;
                }

                var passed = match.Percentage >= req.MinimumPercentage;
                checks.Add(new EligibilityCheck
                {
                    Description = $"{subjectName}: needs {req.MinimumPercentage:0}%, student has {match.Percentage:0}%",
                    Passed = passed
                });

                if (!passed)
                    failures.Add($"{subjectName} requires {req.MinimumPercentage:0}%; student has {match.Percentage:0}%.");
            }

            var status = failures.Count == 0
                ? EligibilityStatus.Qualified
                : EligibilityStatus.NotQualified;

            return new EligibilityResult
            {
                Status = status,
                StudentAps = aps,
                RequiredAps = admission?.MinimumAPS is decimal m ? (int)Math.Round(m) : null,
                Checks = checks,
                FailureReasons = failures
            };
        }
    }
}