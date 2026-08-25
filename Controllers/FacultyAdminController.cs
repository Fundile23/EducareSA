using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FacultyAdminController : Controller
    {
        private readonly EducareDbContext _context;

        public FacultyAdminController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: FacultyAdmin
        public async Task<IActionResult> Index()
        {
            var faculties = await _context.Faculties
                .Include(f => f.University)
                .AsNoTracking()
                .OrderBy(f => f.University.Name)
                .ThenBy(f => f.Name)
                .ToListAsync();

            return View(faculties);
        }

        // GET: FacultyAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = await _context.Faculties
                .Include(f => f.University)
                .Include(f => f.Programmes)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FacultyId == id);

            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        // GET: FacultyAdmin/Create
        public async Task<IActionResult> Create()
        {
            await LoadUniversities();

            return View();
        }

        // POST: FacultyAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faculty faculty)
        {
            if (!ModelState.IsValid)
            {
                await LoadUniversities();
                return View(faculty);
            }

            var universityExists = await _context.Universities
                .AnyAsync(u => u.UniversityId == faculty.UniversityId);

            if (!universityExists)
            {
                ModelState.AddModelError(
                    "UniversityId",
                    "The selected university does not exist.");

                await LoadUniversities();
                return View(faculty);
            }

            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: FacultyAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = await _context.Faculties
                .FindAsync(id);

            if (faculty == null)
                return NotFound();

            await LoadUniversities();

            return View(faculty);
        }

        // POST: FacultyAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Faculty faculty)
        {
            if (id != faculty.FacultyId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadUniversities();
                return View(faculty);
            }

            var universityExists = await _context.Universities
                .AnyAsync(u => u.UniversityId == faculty.UniversityId);

            if (!universityExists)
            {
                ModelState.AddModelError(
                    "UniversityId",
                    "The selected university does not exist.");

                await LoadUniversities();
                return View(faculty);
            }

            try
            {
                var existingFaculty = await _context.Faculties
                    .FindAsync(id);

                if (existingFaculty == null)
                    return NotFound();

                existingFaculty.UniversityId = faculty.UniversityId;
                existingFaculty.Name = faculty.Name;
                existingFaculty.Description = faculty.Description;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacultyExists(faculty.FacultyId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: FacultyAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = await _context.Faculties
                .Include(f => f.University)
                .Include(f => f.Programmes)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FacultyId == id);

            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        // POST: FacultyAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _context.Faculties
                .FindAsync(id);

            if (faculty == null)
                return NotFound();

            var hasProgrammes = await _context.Programmes
                .AnyAsync(p => p.FacultyId == id);

            if (hasProgrammes)
            {
                TempData["Error"] =
                    "This faculty cannot be deleted because it has programmes associated with it.";

                return RedirectToAction(nameof(Index));
            }

            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadUniversities()
        {
            ViewBag.Universities = await _context.Universities
                .Where(u => u.IsActive)
                .OrderBy(u => u.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        private bool FacultyExists(int id)
        {
            return _context.Faculties
                .Any(f => f.FacultyId == id);
        }
    }
}