using EducareSA.Data;
using EducareSA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Services
{
    public interface IStudentProvisioningService
    {
        Task<Student> GetOrCreateForUserAsync(string applicationUserId);
    }

    public class StudentProvisioningService : IStudentProvisioningService
    {
        private readonly EducareDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentProvisioningService(
            EducareDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Student> GetOrCreateForUserAsync(string applicationUserId)
        {
            var existing = await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId);

            if (existing != null)
                return existing;

            var user = await _userManager.FindByIdAsync(applicationUserId)
                ?? throw new InvalidOperationException(
                    $"No IdentityUser found with id {applicationUserId}.");

            var email = user.Email ?? user.UserName ?? "unknown@example.com";
            var localPart = email.Split('@')[0];

            var student = new Student
            {
                ApplicationUserId = applicationUserId,
                Email = email,
                FirstName = localPart,
                LastName = string.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }
    }
}