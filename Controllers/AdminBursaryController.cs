using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBursaryController : Controller
    {
        private readonly EducareDbContext _context;

        public AdminBursaryController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: AdminBursary
        public async Task<IActionResult> Index()
        {
            var bursaries = await _context.Bursaries
                .AsNoTracking()
                .OrderBy(b => b.Name)
                .ToListAsync();

            return View(bursaries);
        }

        // GET: AdminBursary/Details/5
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

        // GET: AdminBursary/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminBursary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bursary bursary)
        {
            if (!ModelState.IsValid)
                return View(bursary);

            bursary.CreatedAt = DateTime.UtcNow;
            bursary.UpdatedAt = DateTime.UtcNow;

            _context.Bursaries.Add(bursary);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminBursary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var bursary = await _context.Bursaries.FindAsync(id);

            if (bursary == null)
                return NotFound();

            return View(bursary);
        }

        // POST: AdminBursary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bursary bursary)
        {
            if (id != bursary.BursaryId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(bursary);

            var existingBursary = await _context.Bursaries.FindAsync(id);

            if (existingBursary == null)
                return NotFound();

            existingBursary.Name = bursary.Name;
            existingBursary.Provider = bursary.Provider;
            existingBursary.Description = bursary.Description;
            existingBursary.Coverage = bursary.Coverage;
            existingBursary.WebsiteUrl = bursary.WebsiteUrl;
            existingBursary.OpeningDate = bursary.OpeningDate;
            existingBursary.ClosingDate = bursary.ClosingDate;
            existingBursary.MinimumAPS = bursary.MinimumAPS;
            existingBursary.EligibilityNotes = bursary.EligibilityNotes;
            existingBursary.IsActive = bursary.IsActive;
            existingBursary.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminBursary/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: AdminBursary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bursary = await _context.Bursaries.FindAsync(id);

            if (bursary == null)
                return NotFound();

            _context.Bursaries.Remove(bursary);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}