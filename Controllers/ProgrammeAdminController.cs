using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProgrammeAdminController : Controller
    {
        private readonly EducareDbContext _context;

        public ProgrammeAdminController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: ProgrammeAdmin
        public async Task<IActionResult> Index()
        {
            var programmes = await _context.Programmes
                .Include(p => p.Faculty)
                    .ThenInclude(f => f.University)
                .Include(p => p.Campus)
                .AsNoTracking()
                .OrderBy(p => p.Faculty.University.Name)
                .ThenBy(p => p.Faculty.Name)
                .ThenBy(p => p.Name)
                .ToListAsync();

            return View(programmes);
        }

        // GET: ProgrammeAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var programme = await _context.Programmes
                .Include(p => p.Faculty)
                    .ThenInclude(f => f.University)
                .Include(p => p.Campus)
                .Include(p => p.AdmissionRequirements)
                .Include(p => p.SubjectRequirements)
                    .ThenInclude(r => r.Subject)
                .Include(p => p.Fees)
                .Include(p => p.ApplicationPeriods)
                .Include(p => p.Modules)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProgrammeId == id);

            if (programme == null)
                return NotFound();

            return View(programme);
        }

        // GET: ProgrammeAdmin/Create
        public async Task<IActionResult> Create()
        {
            await LoadFormData();

            return View();
        }

        // POST: ProgrammeAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Programme programme)
        {
            if (!ModelState.IsValid)
            {
                await LoadFormData();
                return View(programme);
            }

            var facultyExists = await _context.Faculties
                .AnyAsync(f => f.FacultyId == programme.FacultyId);

            if (!facultyExists)
            {
                ModelState.AddModelError(
                    "FacultyId",
                    "The selected faculty does not exist.");

                await LoadFormData();
                return View(programme);
            }

            if (programme.CampusId.HasValue)
            {
                var campusExists = await _context.Campuses
                    .AnyAsync(c => c.CampusId == programme.CampusId.Value);

                if (!campusExists)
                {
                    ModelState.AddModelError(
                        "CampusId",
                        "The selected campus does not exist.");

                    await LoadFormData();
                    return View(programme);
                }
            }

            _context.Programmes.Add(programme);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ProgrammeAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var programme = await _context.Programmes
                .FindAsync(id);

            if (programme == null)
                return NotFound();

            await LoadFormData();

            return View(programme);
        }

        // POST: ProgrammeAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Programme programme)
        {
            if (id != programme.ProgrammeId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadFormData();
                return View(programme);
            }

            var facultyExists = await _context.Faculties
                .AnyAsync(f => f.FacultyId == programme.FacultyId);

            if (!facultyExists)
            {
                ModelState.AddModelError(
                    "FacultyId",
                    "The selected faculty does not exist.");

                await LoadFormData();
                return View(programme);
            }

            if (programme.CampusId.HasValue)
            {
                var campusExists = await _context.Campuses
                    .AnyAsync(c => c.CampusId == programme.CampusId.Value);

                if (!campusExists)
                {
                    ModelState.AddModelError(
                        "CampusId",
                        "The selected campus does not exist.");

                    await LoadFormData();
                    return View(programme);
                }
            }

            try
            {
                var existingProgramme = await _context.Programmes
                    .FindAsync(id);

                if (existingProgramme == null)
                    return NotFound();

                existingProgramme.FacultyId = programme.FacultyId;
                existingProgramme.CampusId = programme.CampusId;
                existingProgramme.Name = programme.Name;
                existingProgramme.QualificationType =
                    programme.QualificationType;
                existingProgramme.QualificationCode =
                    programme.QualificationCode;
                existingProgramme.NQFLevel = programme.NQFLevel;
                existingProgramme.DurationYears =
                    programme.DurationYears;
                existingProgramme.Description =
                    programme.Description;
                existingProgramme.CareerInformation =
                    programme.CareerInformation;
                existingProgramme.IsActive =
                    programme.IsActive;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgrammeExists(programme.ProgrammeId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ProgrammeAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var programme = await _context.Programmes
                .Include(p => p.Faculty)
                    .ThenInclude(f => f.University)
                .Include(p => p.Campus)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProgrammeId == id);

            if (programme == null)
                return NotFound();

            return View(programme);
        }

        // POST: ProgrammeAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programme = await _context.Programmes
                .FindAsync(id);

            if (programme == null)
                return NotFound();

            _context.Programmes.Remove(programme);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadFormData()
        {
            ViewBag.Faculties = await _context.Faculties
                .Include(f => f.University)
                .Where(f => f.University.IsActive)
                .OrderBy(f => f.University.Name)
                .ThenBy(f => f.Name)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Campuses = await _context.Campuses
                .Include(c => c.University)
                .Where(c => c.IsActive && c.University.IsActive)
                .OrderBy(c => c.University.Name)
                .ThenBy(c => c.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        private bool ProgrammeExists(int id)
        {
            return _context.Programmes
                .Any(p => p.ProgrammeId == id);
        }
    }
}