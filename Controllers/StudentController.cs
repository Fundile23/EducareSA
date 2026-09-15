using EducareSA.Data;
using EducareSA.Models;
using EducareSA.Services;
using EducareSA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly EducareDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentProvisioningService _provisioning;
        private readonly IApsCalculator _aps;

        public StudentController(
            EducareDbContext context,
            UserManager<IdentityUser> userManager,
            IStudentProvisioningService provisioning,
            IApsCalculator aps)
        {
            _context = context;
            _userManager = userManager;
            _provisioning = provisioning;
            _aps = aps;
        }

        // GET: Student/Profile
        public async Task<IActionResult> Profile()
        {
            var student = await GetCurrentStudentAsync();
            if (student == null) return Challenge();

            var results = await _context.StudentSubjectResults
                .Include(r => r.Subject)
                .Where(r => r.StudentId == student.StudentId)
                .AsNoTracking()
                .ToListAsync();

            var vm = new StudentProfileViewModel
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Province = student.Province,
                SchoolName = student.SchoolName,
                Grade = student.Grade,
                Aps = _aps.CalculateForStudent(results),
                SubjectCount = results.Count
            };

            return View(vm);
        }

        // POST: Student/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(StudentProfileViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var student = await GetCurrentStudentAsync();
            if (student == null) return Challenge();

            student.FirstName = vm.FirstName;
            student.LastName = vm.LastName;
            student.DateOfBirth = vm.DateOfBirth;
            student.Province = vm.Province;
            student.SchoolName = vm.SchoolName;
            student.Grade = vm.Grade;
            student.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile updated.";
            return RedirectToAction(nameof(Profile));
        }

        // GET: Student/Results
        public async Task<IActionResult> Results()
        {
            var student = await GetCurrentStudentAsync();
            if (student == null) return Challenge();

            var vm = await BuildResultsPageAsync(student.StudentId);
            return View(vm);
        }

        // POST: Student/AddResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResult(
            [Bind(Prefix = "NewResult")] SubjectResultInputViewModel input)
        {
            var student = await GetCurrentStudentAsync();
            if (student == null) return Challenge();

            if (!ModelState.IsValid)
            {
                var vm = await BuildResultsPageAsync(student.StudentId);
                vm.NewResult = input;
                return View("Results", vm);
            }

            var duplicate = await _context.StudentSubjectResults
                .AnyAsync(r => r.StudentId == student.StudentId
                            && r.SubjectId == input.SubjectId
                            && r.AcademicYear == input.AcademicYear);

            if (duplicate)
            {
                ModelState.AddModelError(string.Empty,
                    "You already captured a result for that subject in that year.");
                var vm = await BuildResultsPageAsync(student.StudentId);
                vm.NewResult = input;
                return View("Results", vm);
            }

            _context.StudentSubjectResults.Add(new StudentSubjectResult
            {
                StudentId = student.StudentId,
                SubjectId = input.SubjectId,
                Percentage = input.Percentage,
                Level = input.Level ?? _aps.PointsFor(input.Percentage),
                AcademicYear = input.AcademicYear
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Subject result added.";
            return RedirectToAction(nameof(Results));
        }

        // POST: Student/DeleteResult/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteResult(int id)
        {
            var student = await GetCurrentStudentAsync();
            if (student == null) return Challenge();

            var result = await _context.StudentSubjectResults
                .FirstOrDefaultAsync(r => r.ResultId == id
                                       && r.StudentId == student.StudentId);

            if (result == null) return NotFound();

            _context.StudentSubjectResults.Remove(result);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Result removed.";
            return RedirectToAction(nameof(Results));
        }

        private async Task<Student?> GetCurrentStudentAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;
            return await _provisioning.GetOrCreateForUserAsync(user.Id);
        }

        private async Task<SubjectResultsPageViewModel> BuildResultsPageAsync(int studentId)
        {
            var results = await _context.StudentSubjectResults
                .Include(r => r.Subject)
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.Percentage)
                .AsNoTracking()
                .ToListAsync();

            var breakdown = _aps.Breakdown(results)
                .Select(b => new ApsBreakdownRow
                {
                    SubjectName = b.SubjectName,
                    Percentage = b.Percentage,
                    Points = b.Points,
                    Counted = b.Counted
                })
                .ToList();

            return new SubjectResultsPageViewModel
            {
                Results = results,
                AvailableSubjects = await _context.Subjects
                    .OrderBy(s => s.Name)
                    .AsNoTracking()
                    .ToListAsync(),
                NewResult = new SubjectResultInputViewModel
                {
                    AcademicYear = DateTime.UtcNow.Year
                },
                Aps = _aps.CalculateForStudent(results),
                Breakdown = breakdown
            };
        }
    }
}