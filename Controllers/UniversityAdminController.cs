using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UniversityAdminController : Controller
    {
        private readonly EducareDbContext _context;

        public UniversityAdminController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: UniversityAdmin
        public async Task<IActionResult> Index()
        {
            var universities = await _context.Universities
                .AsNoTracking()
                .OrderBy(u => u.Name)
                .ToListAsync();

            return View(universities);
        }

        // GET: UniversityAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var university = await _context.Universities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UniversityId == id);

            if (university == null)
                return NotFound();

            return View(university);
        }

        // GET: UniversityAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UniversityAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(University university)
        {
            if (!ModelState.IsValid)
                return View(university);

            university.CreatedAt = DateTime.UtcNow;
            university.UpdatedAt = DateTime.UtcNow;

            _context.Universities.Add(university);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: UniversityAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var university = await _context.Universities
                .FindAsync(id);

            if (university == null)
                return NotFound();

            return View(university);
        }

        // POST: UniversityAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            University university)
        {
            if (id != university.UniversityId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(university);

            try
            {
                var existingUniversity = await _context.Universities
                    .FindAsync(id);

                if (existingUniversity == null)
                    return NotFound();

                existingUniversity.Name = university.Name;
                existingUniversity.ShortName = university.ShortName;
                existingUniversity.Description = university.Description;
                existingUniversity.WebsiteUrl = university.WebsiteUrl;
                existingUniversity.LogoUrl = university.LogoUrl;
                existingUniversity.Province = university.Province;
                existingUniversity.City = university.City;
                existingUniversity.IsActive = university.IsActive;
                existingUniversity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UniversityExists(university.UniversityId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UniversityAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var university = await _context.Universities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UniversityId == id);

            if (university == null)
                return NotFound();

            return View(university);
        }

        // POST: UniversityAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var university = await _context.Universities
                .FindAsync(id);

            if (university == null)
                return NotFound();

            _context.Universities.Remove(university);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UniversityExists(int id)
        {
            return _context.Universities
                .Any(e => e.UniversityId == id);
        }
    }
}