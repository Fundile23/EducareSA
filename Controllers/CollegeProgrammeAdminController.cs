using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CollegeProgrammeAdminController : Controller
    {
        private readonly EducareDbContext _context;

        public CollegeProgrammeAdminController(
            EducareDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var programmes = await _context.CollegeProgrammes
                .Include(p => p.College)
                .AsNoTracking()
                .OrderBy(p => p.College!.Name)
                .ThenBy(p => p.Name)
                .ToListAsync();

            return View(programmes);
        }

        public async Task<IActionResult> Create()
        {
            await LoadColleges();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CollegeProgramme programme)
        {
            if (!ModelState.IsValid)
            {
                await LoadColleges();
                return View(programme);
            }

            _context.CollegeProgrammes.Add(programme);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "College programme added successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var programme =
                await _context.CollegeProgrammes.FindAsync(id);

            if (programme == null)
                return NotFound();

            await LoadColleges();

            return View(programme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            CollegeProgramme programme)
        {
            if (id != programme.CollegeProgrammeId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadColleges();
                return View(programme);
            }

            var existingProgramme =
                await _context.CollegeProgrammes.FindAsync(id);

            if (existingProgramme == null)
                return NotFound();

            existingProgramme.Name =
                programme.Name;

            existingProgramme.ProgrammeType =
                programme.ProgrammeType;

            existingProgramme.NQFLevel =
                programme.NQFLevel;

            existingProgramme.MinimumEntryRequirement =
                programme.MinimumEntryRequirement;

            existingProgramme.MinimumAPS =
                programme.MinimumAPS;

            existingProgramme.RequiredSubjects =
                programme.RequiredSubjects;

            existingProgramme.Duration =
                programme.Duration;

            existingProgramme.Description =
                programme.Description;

            existingProgramme.IsActive =
                programme.IsActive;

            existingProgramme.CollegeId =
                programme.CollegeId;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "College programme updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var programme =
                await _context.CollegeProgrammes
                    .Include(p => p.College)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        p => p.CollegeProgrammeId == id);

            if (programme == null)
                return NotFound();

            return View(programme);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var programme =
                await _context.CollegeProgrammes.FindAsync(id);

            if (programme == null)
                return NotFound();

            _context.CollegeProgrammes.Remove(programme);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "College programme deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadColleges()
        {
            ViewBag.Colleges =
                await _context.Colleges
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Name)
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}
