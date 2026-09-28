using EducareSA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace EducareSA.Controllers
{
    [Authorize]
    public class CollegeController : Controller
    {
        private readonly EducareDbContext _context;

        public CollegeController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: /College
        public async Task<IActionResult> Index()
        {
            var colleges = await _context.Colleges
                .Where(c => c.IsActive)
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(colleges);
        }

        // GET: /College/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var college = await _context.Colleges
                .Include(c => c.Campuses
                    .Where(campus => campus.IsActive))
                .Include(c => c.Programmes
                    .Where(programme => programme.IsActive))
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CollegeId == id);

            if (college == null)
                return NotFound();

            return View(college);
        }

        // GET: /College/Programme/5
        public async Task<IActionResult> Programme(int? id)
        {
            if (id == null)
                return NotFound();

            var programme = await _context.CollegeProgrammes
                .Include(p => p.College)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.CollegeProgrammeId == id
                          && p.IsActive);

            if (programme == null)
                return NotFound();

            return View(programme);
        }
    }
}
