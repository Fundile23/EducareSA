using EducareSA.Models;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(EducareDbContext context)
        {
            await SeedSubjectsAsync(context);
            await SeedDemoUniversityAsync(context);
        }

        private static async Task SeedSubjectsAsync(EducareDbContext context)
        {
            if (await context.Subjects.AnyAsync()) return;

            var subjects = new[]
            {
                "English Home Language",
                "English First Additional Language",
                "Afrikaans Home Language",
                "Afrikaans First Additional Language",
                "isiZulu Home Language",
                "isiXhosa Home Language",
                "Mathematics",
                "Mathematical Literacy",
                "Technical Mathematics",
                "Physical Sciences",
                "Life Sciences",
                "Agricultural Sciences",
                "Accounting",
                "Business Studies",
                "Economics",
                "Geography",
                "History",
                "Life Orientation",
                "Information Technology",
                "Computer Applications Technology",
                "Engineering Graphics and Design",
                "Visual Arts",
                "Music",
                "Tourism",
                "Hospitality Studies",
                "Consumer Studies",
                "Religion Studies",
                "Dramatic Arts",
                "Design",
                "Electrical Technology",
                "Civil Technology",
                "Mechanical Technology"
            };

            context.Subjects.AddRange(subjects.Select(s => new Subject
            {
                Name = s,
                Category = "NSC"
            }));

            await context.SaveChangesAsync();
        }

        private static async Task SeedDemoUniversityAsync(EducareDbContext context)
        {
            // Only seed one demo university + a couple of programmes if the
            // database is completely empty. Admin can add the rest via the UI.
            if (await context.Universities.AnyAsync()) return;

            var uct = new University
            {
                Name = "University of Cape Town",
                ShortName = "UCT",
                Description = "South Africa's oldest university, located in Cape Town.",
                WebsiteUrl = "https://www.uct.ac.za",
                Province = "Western Cape",
                City = "Cape Town",
                IsActive = true
            };

            context.Universities.Add(uct);
            await context.SaveChangesAsync();

            var campus = new Campus
            {
                UniversityId = uct.UniversityId,
                Name = "Rondebosch Campus",
                City = "Cape Town",
                Province = "Western Cape"
            };

            context.Campuses.Add(campus);
            await context.SaveChangesAsync();

            var scienceFaculty = new Faculty
            {
                UniversityId = uct.UniversityId,
                Name = "Faculty of Science",
                Description = "Science, engineering and computing programmes."
            };

            context.Faculties.Add(scienceFaculty);
            await context.SaveChangesAsync();

            // We deliberately do NOT hard-code exact APS/admission numbers,
            // because they change every year. Admins should enter these from
            // the official prospectus. We seed placeholders so the UI has
            // something to render.
            var bsc = new Programme
            {
                FacultyId = scienceFaculty.FacultyId,
                CampusId = campus.CampusId,
                Name = "Bachelor of Science",
                QualificationType = "Degree",
                QualificationCode = "BSC",
                NQFLevel = 7,
                DurationYears = 3,
                Description = "General BSc. Confirm subject requirements from UCT's official prospectus.",
                IsActive = true
            };

            var bcom = new Programme
            {
                FacultyId = scienceFaculty.FacultyId,
                CampusId = campus.CampusId,
                Name = "Bachelor of Commerce",
                QualificationType = "Degree",
                QualificationCode = "BCOM",
                NQFLevel = 7,
                DurationYears = 3,
                Description = "General BCom. Confirm subject requirements from UCT's official prospectus.",
                IsActive = true
            };

            context.Programmes.AddRange(bsc, bcom);
            await context.SaveChangesAsync();

            var math = await context.Subjects.FirstAsync(s => s.Name == "Mathematics");
            var english = await context.Subjects.FirstAsync(s => s.Name == "English Home Language");

            context.ProgrammeSubjectRequirements.AddRange(
                new ProgrammeSubjectRequirement
                {
                    ProgrammeId = bsc.ProgrammeId,
                    SubjectId = math.SubjectId,
                    MinimumPercentage = 60,
                    Required = true
                },
                new ProgrammeSubjectRequirement
                {
                    ProgrammeId = bsc.ProgrammeId,
                    SubjectId = english.SubjectId,
                    MinimumPercentage = 50,
                    Required = true
                },
                new ProgrammeSubjectRequirement
                {
                    ProgrammeId = bcom.ProgrammeId,
                    SubjectId = math.SubjectId,
                    MinimumPercentage = 60,
                    Required = true
                },
                new ProgrammeSubjectRequirement
                {
                    ProgrammeId = bcom.ProgrammeId,
                    SubjectId = english.SubjectId,
                    MinimumPercentage = 50,
                    Required = true
                });

            context.AdmissionRequirements.AddRange(
                new AdmissionRequirement
                {
                    ProgrammeId = bsc.ProgrammeId,
                    MinimumAPS = 42,
                    AcademicYear = DateTime.UtcNow.Year,
                    AdditionalRequirements =
                        "Placeholder APS. Update from UCT prospectus."
                },
                new AdmissionRequirement
                {
                    ProgrammeId = bcom.ProgrammeId,
                    MinimumAPS = 42,
                    AcademicYear = DateTime.UtcNow.Year,
                    AdditionalRequirements =
                        "Placeholder APS. Update from UCT prospectus."
                });

            await context.SaveChangesAsync();
        }
    }
}