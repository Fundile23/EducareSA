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
        private readonly IApsCalculator _aps;

        public BursaryController(
            EducareDbContext context,
            UserManager<IdentityUser> userManager,
            IStudentProvisioningService provisioning,
            IApsCalculator aps)
        {
            _context = context;
            _userManager = userManager;
            _provisioning = provisioning;
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

            // No results yet — still show all bursaries, but flag that APS isn't known
            if (results.Count == 0)
            {
                ViewBag.HasResults = false;
                ViewBag.Aps = 0;
            }
            else
            {
                ViewBag.HasResults = true;
                ViewBag.Aps = _aps.CalculateForStudent(results);
            }

            // Get all active bursaries
            var bursaries = await _context.Bursaries
                .Where(b => b.IsActive)
                .OrderBy(b => b.ClosingDate)
                .ThenBy(b => b.Name)
                .AsNoTracking()
                .ToListAsync();

            return View(bursaries);
        }

        // GET: /Bursary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var bursary = await _context.Bursaries
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BursaryId == id);

            if (bursary == null)
                return NotFound();

            return View(bursary);
        }
    }
}