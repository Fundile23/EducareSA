using EducareSA.Models;

namespace EducareSA.Services
{
    /// <summary>
    /// South African APS (Admission Point Score) calculation.
    ///
    /// Rules implemented:
    ///  - NSC 7-point scale: 80-100% = 7, 70-79% = 6, 60-69% = 5,
    ///    50-59% = 4, 40-49% = 3, 30-39% = 2, 0-29% = 1.
    ///  - The best six subjects count toward APS.
    ///  - Life Orientation is capped at 4 points (common SA convention),
    ///    but only when it is one of the counting subjects.
    ///  - If a student has fewer than six subjects, all are counted.
    ///
    /// NOTE: some universities use different rules (e.g. exclude LO entirely,
    /// count only 6 subjects, apply different LO caps). The interface is
    /// deliberately small so a per-university strategy can be added later.
    /// </summary>
    public interface IApsCalculator
    {
        int PointsFor(decimal percentage);
        int CalculateForStudent(IEnumerable<StudentSubjectResult> results);
        IReadOnlyList<SubjectPoints> Breakdown(IEnumerable<StudentSubjectResult> results);
    }

    public record SubjectPoints(string SubjectName, decimal Percentage, int Points, bool Counted);

    public class ApsCalculator : IApsCalculator
    {
        private const int MaxSubjectsCounted = 6;
        private const int LifeOrientationCap = 4;

        public int PointsFor(decimal percentage)
        {
            if (percentage >= 80) return 7;
            if (percentage >= 70) return 6;
            if (percentage >= 60) return 5;
            if (percentage >= 50) return 4;
            if (percentage >= 40) return 3;
            if (percentage >= 30) return 2;
            return 1;
        }

        public int CalculateForStudent(IEnumerable<StudentSubjectResult> results)
            => Breakdown(results).Where(b => b.Counted).Sum(b => b.Points);

        public IReadOnlyList<SubjectPoints> Breakdown(IEnumerable<StudentSubjectResult> results)
        {
            var list = results.ToList();
            var scored = list
                .Select(r => new
                {
                    Name = r.Subject?.Name ?? $"Subject {r.SubjectId}",
                    r.Percentage,
                    IsLo = string.Equals(r.Subject?.Name, "Life Orientation",
                                         StringComparison.OrdinalIgnoreCase),
                    RawPoints = PointsFor(r.Percentage)
                })
                .Select(x => new
                {
                    x.Name,
                    x.Percentage,
                    x.IsLo,
                    Points = x.IsLo ? Math.Min(x.RawPoints, LifeOrientationCap) : x.RawPoints
                })
                .OrderByDescending(x => x.Points)
                .ToList();

            var topIds = scored.Take(MaxSubjectsCounted).ToHashSet();

            return scored
                .Select(x => new SubjectPoints(
                    x.Name,
                    x.Percentage,
                    x.Points,
                    Counted: topIds.Contains(x)))
                .ToList();
        }
    }
}