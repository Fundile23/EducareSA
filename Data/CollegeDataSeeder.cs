using EducareSA.Models;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Data
{
    public class CollegeDataSeeder
    {
        public static async Task SeedAsync(EducareDbContext context)
        {
            // -----------------------------------------
            // COLLEGES
            // -----------------------------------------

            var colleges = new List<College>
            {
                new College
                {
                    CollegeId = 1,
                    Name = "Coastal KZN TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Durban",
                    WebsiteUrl = "https://www.coastalkzn.co.za/",
                    Description =
                        "A public Technical and Vocational Education and Training college offering NCV, NATED and other vocational programmes.",
                    IsActive = true
                },

                new College
                {
                    CollegeId = 2,
                    Name = "Elangeni TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Pinetown",
                    WebsiteUrl = "https://www.efet.co.za/",
                    Description =
                        "A public TVET college serving learners across KwaZulu-Natal.",
                    IsActive = true
                },

                new College
                {
                    CollegeId = 3,
                    Name = "Esayidi TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Port Shepstone",
                    WebsiteUrl = "https://www.esayidifet.co.za/",
                    Description =
                        "A public TVET college serving communities in southern KwaZulu-Natal.",
                    IsActive = true
                },

                new College
                {
                    CollegeId = 4,
                    Name = "Majuba TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Newcastle",
                    WebsiteUrl = "https://www.majuba.edu.za/",
                    Description =
                        "A public TVET college offering vocational, engineering and business-related education and training.",
                    IsActive = true
                },

                new College
                {
                    CollegeId = 5,
                    Name = "Mnambithi TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Ladysmith",
                    WebsiteUrl = "https://www.mnambithicollege.co.za/",
                    Description =
                        "A public TVET college serving Ladysmith, Estcourt and surrounding communities.",
                    IsActive = true
                },

                new College
                {
                    CollegeId = 6,
                    Name = "Mthashana TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Vryheid",
                    WebsiteUrl = "https://www.mthashanafet.co.za/",
                    Description =
                        "A public TVET college serving communities in northern KwaZulu-Natal.",
                    IsActive = true
                },

                new College
                {
                    CollegeId = 7,
                    Name = "Thekwini TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Durban",
                    WebsiteUrl = "https://www.thekwinicollege.co.za/",
                    Description =
                        "A public TVET college serving learners in the Durban metropolitan area.",
                    IsActive = true
                },

                new College
                {
                    CollegeId = 8,
                    Name = "uMfolozi TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Richards Bay",
                    WebsiteUrl = "https://www.umfolozicollege.co.za/",
                    Description =
                        "A public TVET college serving northern KwaZulu-Natal.",
                    IsActive = true
                },

                new College
                {
                    CollegeId = 9,
                    Name = "Umgungundlovu TVET College",
                    Province = "KwaZulu-Natal",
                    City = "Pietermaritzburg",
                    WebsiteUrl = "https://www.ufetc.edu.za/",
                    Description =
                        "A public TVET college serving the Midlands and surrounding areas of KwaZulu-Natal.",
                    IsActive = true
                }
            };

            foreach (var college in colleges)
            {
                if (!await context.Colleges.AnyAsync(c => c.CollegeId == college.CollegeId))
                {
                    college.CreatedAt = DateTime.UtcNow;
                    college.UpdatedAt = DateTime.UtcNow;

                    context.Colleges.Add(college);
                }
            }

            await context.SaveChangesAsync();


            // -----------------------------------------
            // PROGRAMMES
            // -----------------------------------------

            var programmes = new List<CollegeProgramme>
            {
                // =====================================
                // COASTAL
                // =====================================

                new CollegeProgramme
                {
                    CollegeProgrammeId = 1,
                    CollegeId = 1,
                    Name = "Information Technology and Computer Science",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9 or equivalent",
                    MinimumAPS = null,
                    RequiredSubjects = "Language; Life Orientation; Mathematics or Mathematical Literacy",
                    Duration = "3 years",
                    Description =
                        "Vocational programme covering information technology and computer science."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 2,
                    CollegeId = 1,
                    Name = "Finance, Economics and Accounting",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9 or equivalent",
                    RequiredSubjects = "Language; Life Orientation; Mathematics or Mathematical Literacy",
                    Duration = "3 years",
                    Description = "Vocational business and accounting programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 3,
                    CollegeId = 1,
                    Name = "Office Administration",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9 or equivalent",
                    RequiredSubjects = "Language; Life Orientation; Mathematics or Mathematical Literacy",
                    Duration = "3 years",
                    Description = "Vocational programme focused on office administration."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 4,
                    CollegeId = 1,
                    Name = "Hospitality",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9 or equivalent",
                    RequiredSubjects = "Language; Life Orientation; Mathematics or Mathematical Literacy",
                    Duration = "3 years",
                    Description = "Vocational hospitality programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 5,
                    CollegeId = 1,
                    Name = "Tourism",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9 or equivalent",
                    RequiredSubjects = "Language; Life Orientation; Mathematics or Mathematical Literacy",
                    Duration = "3 years",
                    Description = "Vocational tourism programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 6,
                    CollegeId = 1,
                    Name = "Civil Engineering",
                    ProgrammeType = "NATED",
                    NQFLevel = "N1-N6",
                    MinimumEntryRequirement = "N3 or Grade 12 with Mathematics and Physical Science as passed subjects",
                    RequiredSubjects = "Mathematics; Physical Science",
                    Duration = "N1-N6",
                    Description = "NATED engineering programme in civil engineering."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 7,
                    CollegeId = 1,
                    Name = "Electrical Engineering",
                    ProgrammeType = "NATED",
                    NQFLevel = "N1-N6",
                    MinimumEntryRequirement = "N3 or Grade 12 with Mathematics and Physical Science as passed subjects",
                    RequiredSubjects = "Mathematics; Physical Science",
                    Duration = "N1-N6",
                    Description = "NATED engineering programme in electrical engineering."
                },


                // =====================================
                // MNAMBITHI
                // =====================================

                new CollegeProgramme
                {
                    CollegeProgrammeId = 8,
                    CollegeId = 5,
                    Name = "Information Technology and Computer Science",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9-12 school report or approved PLP bridging programme",
                    RequiredSubjects = "English; Mathematics; Life Orientation",
                    Duration = "3 years",
                    Description =
                        "NC(V) programme covering systems development, networking, programming and computer technology."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 9,
                    CollegeId = 5,
                    Name = "Finance, Economics and Accounting",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9-12 school report or approved PLP bridging programme",
                    RequiredSubjects = "Fundamental NC(V) subjects",
                    Duration = "3 years",
                    Description = "NC(V) business and accounting programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 10,
                    CollegeId = 5,
                    Name = "Office Administration",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9-12 school report or approved PLP bridging programme",
                    RequiredSubjects = "Fundamental NC(V) subjects",
                    Duration = "3 years",
                    Description = "NC(V) office administration programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 11,
                    CollegeId = 5,
                    Name = "Education and Development",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9-12 school report or approved PLP bridging programme",
                    RequiredSubjects = "English; Mathematical Literacy; Life Orientation",
                    Duration = "3 years",
                    Description = "NC(V) programme in education and development."
                },


                // =====================================
                // MAJUBA
                // =====================================

                new CollegeProgramme
                {
                    CollegeProgrammeId = 12,
                    CollegeId = 4,
                    Name = "Engineering and Related Design",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9, 10, 11 or 12 certificate or NQF Level 1 qualification",
                    RequiredSubjects = "English; Mathematics or Mathematical Literacy; Life Orientation",
                    Duration = "3 years",
                    Description =
                        "Engineering and related design with specialisation options including fitting and turning, boilermaking, motor mechanics and welding."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 13,
                    CollegeId = 4,
                    Name = "Electrical Infrastructure Construction",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9, 10, 11 or 12 certificate or NQF Level 1 qualification",
                    RequiredSubjects = "English; Mathematics or Mathematical Literacy; Life Orientation",
                    Duration = "3 years",
                    Description = "Vocational electrical infrastructure construction programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 14,
                    CollegeId = 4,
                    Name = "Civil Engineering and Building Construction",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9, 10, 11 or 12 certificate or NQF Level 1 qualification",
                    RequiredSubjects = "English; Mathematics or Mathematical Literacy; Life Orientation",
                    Duration = "3 years",
                    Description = "Vocational civil engineering and building construction programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 15,
                    CollegeId = 4,
                    Name = "Hospitality",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9, 10, 11 or 12 certificate or NQF Level 1 qualification",
                    RequiredSubjects = "English; Mathematics or Mathematical Literacy; Life Orientation",
                    Duration = "3 years",
                    Description = "Vocational hospitality programme."
                },


                // =====================================
                // UMFOLOZI
                // =====================================

                new CollegeProgramme
                {
                    CollegeProgrammeId = 16,
                    CollegeId = 8,
                    Name = "Engineering and Related Design",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "3 years",
                    Description = "NC(V) Engineering and Related Design programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 17,
                    CollegeId = 8,
                    Name = "Office Administration",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "3 years",
                    Description = "NC(V) Office Administration programme."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 18,
                    CollegeId = 8,
                    Name = "Public Management",
                    ProgrammeType = "NATED",
                    NQFLevel = "N4-N6",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "18 months",
                    Description = "NATED Business Studies programme in Public Management."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 19,
                    CollegeId = 8,
                    Name = "Financial Management",
                    ProgrammeType = "NATED",
                    NQFLevel = "N4-N6",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "18 months",
                    Description = "NATED Business Studies programme in Financial Management."
                },

                new CollegeProgramme
                {
                    CollegeProgrammeId = 20,
                    CollegeId = 8,
                    Name = "Legal Secretary",
                    ProgrammeType = "NATED",
                    NQFLevel = "N4-N6",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "18 months",
                    Description = "NATED Business Studies programme in Legal Secretary."
                }
            };

            foreach (var programme in programmes)
            {
                if (!await context.CollegeProgrammes
                    .AnyAsync(p => p.CollegeProgrammeId == programme.CollegeProgrammeId))
                {
                    programme.IsActive = true;
                    context.CollegeProgrammes.Add(programme);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
