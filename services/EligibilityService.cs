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

            // ---- 1. APS check ----
            if (admission?.MinimumAPS is decimal minAps)
            {
                var passed = aps >= minAps;
                checks.Add(new EligibilityCheck
                {
                    Description = $"APS requirement: {minAps:0} (you have {aps})",
                    Passed = passed
                });
                if (!passed)
                    failures.Add($"APS requires {minAps:0}; student has {aps}.");
            }

            // ---- 2. Subject requirements, grouped ----
            var ungrouped = programme.SubjectRequirements
                .Where(r => r.RequirementGroup == null)
                .ToList();

            var grouped = programme.SubjectRequirements
                .Where(r => r.RequirementGroup != null)
                .GroupBy(r => r.RequirementGroup!.Value)
                .ToList();

            // 2a. Ungrouped: each must be satisfied individually
            foreach (var req in ungrouped)
            {
                var check = EvaluateSingle(req, results);
                checks.Add(check.Check);
                if (!check.Passed)
                    failures.Add(check.FailureReason!);
            }

            // 2b. Grouped: any one member satisfies the whole group
            foreach (var group in grouped)
            {
                var groupChecks = group.Select(r => EvaluateSingle(r, results)).ToList();
                var anyPassed = groupChecks.Any(c => c.Passed);

                if (anyPassed)
                {
                    // Report the satisfied option(s)
                    var satisfied = groupChecks.First(c => c.Passed);
                    checks.Add(new EligibilityCheck
                    {
                        Description = satisfied.Check.Description,
                        Passed = true
                    });
                }
                else
                {
                    // Report all options — none met
                    foreach (var c in groupChecks)
                    {
                        checks.Add(new EligibilityCheck
                        {
                            Description = c.Check.Description,
                            Passed = false
                        });
                    }

                    // Build a human-readable failure using the requirement names
                    var subjectNames = group
                        .Select(r => r.Subject?.Name ?? $"Subject {r.SubjectId}")
                        .ToList();

                    var readable = string.Join(" or ", subjectNames);
                    failures.Add($"None of the following met: {readable}.");
                }
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

        // Helper: evaluates one requirement against the student's results.
        private (EligibilityCheck Check, bool Passed, string? FailureReason)
            EvaluateSingle(ProgrammeSubjectRequirement req, List<StudentSubjectResult> results)
        {
            var subjectName = req.Subject?.Name ?? $"Subject {req.SubjectId}";
            var match = results.FirstOrDefault(r => r.SubjectId == req.SubjectId);

            if (match == null)
            {
                return (
                    new EligibilityCheck
                    {
                        Description = $"{subjectName}: required, not provided",
                        Passed = !req.Required
                    },
                    !req.Required,
                    $"{subjectName} is required but not provided."
                );
            }

            var passed = match.Percentage >= req.MinimumPercentage;

            return (
                new EligibilityCheck
                {
                    Description = $"{subjectName}: needs {req.MinimumPercentage:0}%, you have {match.Percentage:0}%",
                    Passed = passed
                },
                passed,
                passed ? null : $"{subjectName} requires {req.MinimumPercentage:0}%; student has {match.Percentage:0}%."
            );
        }
    }
}