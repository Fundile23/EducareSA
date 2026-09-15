using EducareSA.Data;
using EducareSA.Services;
using EducareSA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize]
    public class EligibilityController : Controller
    {
        private readonly EducareDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentProvisioningService _provisioning;
        private readonly IApsCalculator _aps;
        private readonly IEligibilityService _eligibility;

        public EligibilityController(
            EducareDbContext context,
            UserManager<IdentityUser> userManager,
            IStudentProvisioningService provisioning,
            IApsCalculator aps,
            IEligibilityService eligibility)
        {
            _context = context;
            _userManager = userManager;
            _provisioning = provisioning;
            _aps = aps;
            _eligibility = eligibility;
        }

        // GET: Eligibility/Matches
        public async Task<IActionResult> Matches(int? universityId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var student = await _provisioning.GetOrCreateForUserAsync(user.Id);

            var results = await _context.StudentSubjectResults
                .Include(r => r.Subject)
                .Where(r => r.StudentId == student.StudentId)
                .AsNoTracking()
                .ToListAsync();

            if (results.Count == 0)
            {
                return View(new MatchesPageViewModel { HasResults = false });
            }

            var programmesQuery = _context.Programmes
                .Include(p => p.Faculty).ThenInclude(f => f.University)
                .Include(p => p.Campus)
                .Where(p => p.IsActive && p.Faculty.University.IsActive)
                .AsNoTracking();

            if (universityId.HasValue)
                programmesQuery = programmesQuery.Where(p => p.Faculty.UniversityId == universityId.Value);

            var programmes = await programmesQuery.ToListAsync();

            var qualified = new List<ProgrammeMatchViewModel>();
            var notQualified = new List<ProgrammeMatchViewModel>();

            foreach (var p in programmes)
            {
                var evaluation = await _eligibility.EvaluateAsync(
                    student.StudentId, p.ProgrammeId, DateTime.UtcNow.Year);

                var match = new ProgrammeMatchViewModel
                {
                    Programme = p,
                    Eligibility = evaluation
                };

                if (evaluation.IsQualified)
                    qualified.Add(match);
                else
                    notQualified.Add(match);
            }

            var vm = new MatchesPageViewModel
            {
                Qualified = qualified
                    .OrderBy(m => m.Programme.Faculty.University.Name)
                    .ThenBy(m => m.Programme.Name)
                    .ToList(),
                NotQualified = notQualified
                    .OrderBy(m => m.Programme.Faculty.University.Name)
                    .ThenBy(m => m.Programme.Name)
                    .ToList(),
                Aps = _aps.CalculateForStudent(results),
                HasResults = true
            };

            return View(vm);
        }

        // GET: Eligibility/Check/5
        public async Task<IActionResult> Check(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var student = await _provisioning.GetOrCreateForUserAsync(user.Id);

            var programme = await _context.Programmes
                .Include(p => p.Faculty).ThenInclude(f => f.University)
                .Include(p => p.Campus)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProgrammeId == id);

            if (programme == null) return NotFound();

            var evaluation = await _eligibility.EvaluateAsync(
                student.StudentId, programme.ProgrammeId, DateTime.UtcNow.Year);

            return View(new ProgrammeMatchViewModel
            {
                Programme = programme,
                Eligibility = evaluation
            });
        }
    }
}