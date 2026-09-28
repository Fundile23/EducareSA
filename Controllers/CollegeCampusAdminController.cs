using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CollegeCampusAdminController : Controller
    {
        private readonly EducareDbContext _context;

        public CollegeCampusAdminController(EducareDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var campuses = await _context.CollegeCampuses
                .Include(c => c.College)
                .AsNoTracking()
                .OrderBy(c => c.College!.Name)
                .ThenBy(c => c.Name)
                .ToListAsync();

            return View(campuses);
        }

        public async Task<IActionResult> Create()
        {
            await LoadColleges();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollegeCampus campus)
        {
            if (!ModelState.IsValid)
            {
                await LoadColleges();
                return View(campus);
            }

            _context.CollegeCampuses.Add(campus);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Campus added successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var campus = await _context.CollegeCampuses
                .FindAsync(id);

            if (campus == null)
                return NotFound();

            await LoadColleges();

            return View(campus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            CollegeCampus campus)
        {
            if (id != campus.CollegeCampusId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadColleges();
                return View(campus);
            }

            var existingCampus =
                await _context.CollegeCampuses.FindAsync(id);

            if (existingCampus == null)
                return NotFound();

            existingCampus.Name = campus.Name;
            existingCampus.Address = campus.Address;
            existingCampus.City = campus.City;
            existingCampus.Province = campus.Province;
            existingCampus.WebsiteUrl = campus.WebsiteUrl;
            existingCampus.IsActive = campus.IsActive;
            existingCampus.CollegeId = campus.CollegeId;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Campus updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var campus = await _context.CollegeCampuses
                .Include(c => c.College)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    c => c.CollegeCampusId == id);

            if (campus == null)
                return NotFound();

            return View(campus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campus =
                await _context.CollegeCampuses.FindAsync(id);

            if (campus == null)
                return NotFound();

            _context.CollegeCampuses.Remove(campus);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Campus deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadColleges()
        {
            ViewBag.Colleges = await _context.Colleges
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
