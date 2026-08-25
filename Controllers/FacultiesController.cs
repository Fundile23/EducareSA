using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FacultiesController : Controller
    {
        private readonly EducareDbContext _context;

        public FacultiesController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: Faculties
        public async Task<IActionResult> Index()
        {
            var faculties = await _context.Faculties
                .Include(f => f.University)
                .ToListAsync();

            return View(faculties);
        }

        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = await _context.Faculties
                .Include(f => f.University)
                .Include(f => f.Programmes)
                .FirstOrDefaultAsync(f => f.FacultyId == id);

            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        // GET: Faculties/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Universities = await _context.Universities
                .Where(u => u.IsActive)
                .OrderBy(u => u.Name)
                .ToListAsync();

            return View();
        }

        // POST: Faculties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faculty faculty)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Universities = await _context.Universities
                    .Where(u => u.IsActive)
                    .OrderBy(u => u.Name)
                    .ToListAsync();

                return View(faculty);
            }

            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = await _context.Faculties.FindAsync(id);

            if (faculty == null)
                return NotFound();

            ViewBag.Universities = await _context.Universities
                .Where(u => u.IsActive)
                .OrderBy(u => u.Name)
                .ToListAsync();

            return View(faculty);
        }

        // POST: Faculties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Faculty faculty)
        {
            if (id != faculty.FacultyId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Universities = await _context.Universities
                    .Where(u => u.IsActive)
                    .OrderBy(u => u.Name)
                    .ToListAsync();

                return View(faculty);
            }

            try
            {
                _context.Update(faculty);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Faculties.Any(f => f.FacultyId == id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = await _context.Faculties
                .Include(f => f.University)
                .FirstOrDefaultAsync(f => f.FacultyId == id);

            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);

            if (faculty == null)
                return NotFound();

            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}