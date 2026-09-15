using EducareSA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly EducareDbContext _context;

        public DashboardController(EducareDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Dashboard
        public async Task<IActionResult> Index()
        {
            ViewBag.UniversityCount = await _context.Universities.CountAsync();
            ViewBag.CampusCount = await _context.Campuses.CountAsync();
            ViewBag.FacultyCount = await _context.Faculties.CountAsync();
            ViewBag.ProgrammeCount = await _context.Programmes.CountAsync();
            ViewBag.SubjectCount = await _context.Subjects.CountAsync();
            ViewBag.BursaryCount = await _context.Bursaries.CountAsync();
            ViewBag.StudentCount = await _context.Students.CountAsync();

            ViewBag.ActiveProgrammes = await _context.Programmes
                .CountAsync(p => p.IsActive);

            ViewBag.InactiveUniversities = await _context.Universities
                .CountAsync(u => !u.IsActive);

            return View();
        }
    }
}