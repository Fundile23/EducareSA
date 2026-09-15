using EducareSA.Data;
using EducareSA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    public class ProgrammesController : Controller
    {
        private readonly EducareDbContext _context;

        public ProgrammesController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: Programmes
        public async Task<IActionResult> Index(
            string? q,
            int? universityId,
            int? facultyId,
            string? qualificationType)
        {
            var query = _context.Programmes
                .Include(p => p.Faculty).ThenInclude(f => f.University)
                .Include(p => p.Campus)
                .Where(p => p.IsActive && p.Faculty.University.IsActive)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(p =>
                    p.Name.Contains(term) ||
                    (p.QualificationCode != null && p.QualificationCode.Contains(term)));
            }

            if (universityId.HasValue)
                query = query.Where(p => p.Faculty.UniversityId == universityId.Value);

            if (facultyId.HasValue)
                query = query.Where(p => p.FacultyId == facultyId.Value);

            if (!string.IsNullOrWhiteSpace(qualificationType))
                query = query.Where(p => p.QualificationType == qualificationType);

            var results = await query
                .OrderBy(p => p.Faculty.University.Name)
                .ThenBy(p => p.Faculty.Name)
                .ThenBy(p => p.Name)
                .Take(200)
                .ToListAsync();

            var vm = new ProgrammeSearchViewModel
            {
                Query = q,
                UniversityId = universityId,
                FacultyId = facultyId,
                QualificationType = qualificationType,
                Universities = await _context.Universities
                    .Where(u => u.IsActive).OrderBy(u => u.Name)
                    .AsNoTracking().ToListAsync(),
                Faculties = await _context.Faculties
                    .OrderBy(f => f.Name)
                    .AsNoTracking().ToListAsync(),
                Results = results,
                TotalCount = results.Count
            };

            return View(vm);
        }

        // GET: Programmes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var programme = await _context.Programmes
                .Include(p => p.Faculty).ThenInclude(f => f.University)
                .Include(p => p.Campus)
                .Include(p => p.SubjectRequirements).ThenInclude(r => r.Subject)
                .Include(p => p.AdmissionRequirements)
                .Include(p => p.Fees)
                .Include(p => p.ApplicationPeriods)
                .Include(p => p.Modules)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProgrammeId == id);

            if (programme == null) return NotFound();

            return View(programme);
        }
    }
}