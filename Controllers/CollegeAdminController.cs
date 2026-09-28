using EducareSA.Data;
using Microsoft.EntityFrameworkCore;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CollegeAdminController : Controller
    {
        private readonly EducareDbContext _context;

        public CollegeAdminController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: /CollegeAdmin
        public async Task<IActionResult> Index()
        {
            var colleges = await _context.Colleges
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(colleges);
        }

        // GET: /CollegeAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /CollegeAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(College college)
        {
            if (!ModelState.IsValid)
                return View(college);

            college.CreatedAt = DateTime.UtcNow;
            college.UpdatedAt = DateTime.UtcNow;

            _context.Colleges.Add(college);
            await _context.SaveChangesAsync();

            TempData["Success"] = "College added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /CollegeAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var college = await _context.Colleges.FindAsync(id);

            if (college == null)
                return NotFound();

            return View(college);
        }

        // POST: /CollegeAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, College college)
        {
            if (id != college.CollegeId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(college);

            var existingCollege =
                await _context.Colleges.FindAsync(id);

            if (existingCollege == null)
                return NotFound();

            existingCollege.Name = college.Name;
            existingCollege.Province = college.Province;
            existingCollege.City = college.City;
            existingCollege.Address = college.Address;
            existingCollege.WebsiteUrl = college.WebsiteUrl;
            existingCollege.Description = college.Description;
            existingCollege.IsActive = college.IsActive;
            existingCollege.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = "College updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /CollegeAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var college = await _context.Colleges
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CollegeId == id);

            if (college == null)
                return NotFound();

            return View(college);
        }

        // POST: /CollegeAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var college = await _context.Colleges
                .FindAsync(id);

            if (college == null)
                return NotFound();

            _context.Colleges.Remove(college);
            await _context.SaveChangesAsync();

            TempData["Success"] = "College deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
