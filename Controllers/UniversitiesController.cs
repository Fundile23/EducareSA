using EducareSA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    public class UniversitiesController : Controller
    {
        private readonly EducareDbContext _context;

        public UniversitiesController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: Universities
        public async Task<IActionResult> Index()
        {
            var universities = await _context.Universities
                .Where(u => u.IsActive)
                .OrderBy(u => u.Name)
                .AsNoTracking()
                .ToListAsync();

            return View(universities);
        }

        // GET: Universities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var university = await _context.Universities
                .Include(u => u.Campuses)
                .Include(u => u.Faculties)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UniversityId == id);

            if (university == null)
                return NotFound();

            return View(university);
        }
    }
}