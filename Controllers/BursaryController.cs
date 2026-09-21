using EducareSA.Data;
using EducareSA.Models;
using EducareSA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize]
    public class BursaryController : Controller
    {
        private readonly EducareDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentProvisioningService _provisioning;
        private readonly IEligibilityService _eligibility;
        private readonly IApsCalculator _aps;

        public BursaryController(
            EducareDbContext context,
            UserManager<IdentityUser> userManager,
            IStudentProvisioningService provisioning,
            IEligibilityService eligibility,
            IApsCalculator aps)
        {
            _context = context;
            _userManager = userManager;
            _provisioning = provisioning;
            _eligibility = eligibility;
            _aps = aps;
        }

        // GET: /Bursary
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var student = await _provisioning.GetOrCreateForUserAsync(user.Id);

            // Get the learner's captured results
            var results = await _context.StudentSubjectResults
                .Include(r => r.Subject)
                .Where(r => r.StudentId == student.StudentId)
                .AsNoTracking()
                .ToListAsync();

            // No results yet
            if (results.Count == 0)
            {
                ViewBag.HasResults = false;
                ViewBag.Aps = 0;

                return View(new List<Bursary>());
            }

            // Get active programmes
            var programmes = await _context.Programmes
                .Include(p => p.Faculty)
                    .ThenInclude(f => f.University)
                .Where(p =>
                    p.IsActive &&
                    p.Faculty.University.IsActive)
                .AsNoTracking()
                .ToListAsync();

            // Find programmes the learner qualifies for
            var qualifiedProgrammes = new List<Models.Programme>();

            foreach (var programme in programmes)
            {
                var evaluation = await _eligibility.EvaluateAsync(
                    student.StudentId,
                    programme.ProgrammeId,
                    DateTime.UtcNow.Year);

                if (evaluation.IsQualified)
                {
                    qualifiedProgrammes.Add(programme);
                }
            }

            // Get the faculties of the qualified programmes
            var facultyIds = qualifiedProgrammes
                .Select(p => p.FacultyId)
                .Distinct()
                .ToList();

            // Find active bursaries linked to those faculties
            var bursaries = await _context.Bursaries
                .Include(b => b.Faculty)
                .Where(b =>
                    b.IsActive &&
                    b.FacultyId.HasValue &&
                    facultyIds.Contains(b.FacultyId.Value))
                .OrderBy(b => b.ClosingDate)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.HasResults = true;
            ViewBag.Aps = _aps.CalculateForStudent(results);
            ViewBag.QualifiedProgrammes = qualifiedProgrammes;

            return View(bursaries);
        }

        // GET: /Bursary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var bursary = await _context.Bursaries
                .Include(b => b.Faculty)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BursaryId == id);

            if (bursary == null)
                return NotFound();

            return View(bursary);
        }
    }
}
