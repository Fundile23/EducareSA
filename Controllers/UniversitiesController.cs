using EducareSA.Data;
using EducareSA.Models;
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
                .OrderBy(u => u.Name)
                .ToListAsync();

            return View(universities);
        }

        // GET: Universities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var university = await _context.Universities
                .FirstOrDefaultAsync(u => u.UniversityId == id);

            if (university == null)
                return NotFound();

            return View(university);
        }

        // GET: Universities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Universities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(University university)
        {
            if (ModelState.IsValid)
            {
                university.CreatedAt = DateTime.UtcNow;
                university.UpdatedAt = DateTime.UtcNow;
                university.IsActive = true;

                _context.Universities.Add(university);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(university);
        }

        // GET: Universities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var university = await _context.Universities.FindAsync(id);

            if (university == null)
                return NotFound();

            return View(university);
        }

        // POST: Universities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            University university)
        {
            if (id != university.UniversityId)
                return NotFound();

            if (ModelState.IsValid)
            {
                university.UpdatedAt = DateTime.UtcNow;

                _context.Update(university);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(university);
        }

        // GET: Universities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var university = await _context.Universities
                .FirstOrDefaultAsync(u => u.UniversityId == id);

            if (university == null)
                return NotFound();

            return View(university);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var university = await _context.Universities.FindAsync(id);

            if (university != null)
            {
                _context.Universities.Remove(university);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
