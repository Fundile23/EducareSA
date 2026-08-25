using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CampusAdminController : Controller
    {
        private readonly EducareDbContext _context;

        public CampusAdminController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: CampusAdmin
        public async Task<IActionResult> Index()
        {
            var campuses = await _context.Campuses
                .Include(c => c.University)
                .AsNoTracking()
                .OrderBy(c => c.University.Name)
                .ThenBy(c => c.Name)
                .ToListAsync();

            return View(campuses);
        }

        // GET: CampusAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var campus = await _context.Campuses
                .Include(c => c.University)
                .Include(c => c.Programmes)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CampusId == id);

            if (campus == null)
                return NotFound();

            return View(campus);
        }

        // GET: CampusAdmin/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Universities = await _context.Universities
                .Where(u => u.IsActive)
                .OrderBy(u => u.Name)
                .AsNoTracking()
                .ToListAsync();

            return View();
        }

        // POST: CampusAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Campus campus)
        {
            if (!ModelState.IsValid)
            {
                await LoadUniversities();
                return View(campus);
            }

            var universityExists = await _context.Universities
                .AnyAsync(u => u.UniversityId == campus.UniversityId);

            if (!universityExists)
            {
                ModelState.AddModelError(
                    "UniversityId",
                    "The selected university does not exist.");

                await LoadUniversities();
                return View(campus);
            }

            _context.Campuses.Add(campus);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: CampusAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var campus = await _context.Campuses
                .FindAsync(id);

            if (campus == null)
                return NotFound();

            await LoadUniversities();

            return View(campus);
        }

        // POST: CampusAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Campus campus)
        {
            if (id != campus.CampusId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadUniversities();
                return View(campus);
            }

            var universityExists = await _context.Universities
                .AnyAsync(u => u.UniversityId == campus.UniversityId);

            if (!universityExists)
            {
                ModelState.AddModelError(
                    "UniversityId",
                    "The selected university does not exist.");

                await LoadUniversities();
                return View(campus);
            }

            try
            {
                var existingCampus = await _context.Campuses
                    .FindAsync(id);

                if (existingCampus == null)
                    return NotFound();

                existingCampus.UniversityId = campus.UniversityId;
                existingCampus.Name = campus.Name;
                existingCampus.City = campus.City;
                existingCampus.Province = campus.Province;
                existingCampus.Address = campus.Address;
                existingCampus.Latitude = campus.Latitude;
                existingCampus.Longitude = campus.Longitude;
                existingCampus.IsActive = campus.IsActive;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampusExists(campus.CampusId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CampusAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var campus = await _context.Campuses
                .Include(c => c.University)
                .Include(c => c.Programmes)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CampusId == id);

            if (campus == null)
                return NotFound();

            return View(campus);
        }

        // POST: CampusAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campus = await _context.Campuses
                .FindAsync(id);

            if (campus == null)
                return NotFound();

            var hasProgrammes = await _context.Programmes
                .AnyAsync(p => p.CampusId == id);

            if (hasProgrammes)
            {
                TempData["Error"] =
                    "This campus cannot be deleted because it has programmes associated with it.";

                return RedirectToAction(nameof(Index));
            }

            _context.Campuses.Remove(campus);
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

        private bool CampusExists(int id)
        {
            return _context.Campuses
                .Any(c => c.CampusId == id);
        }
    }
}