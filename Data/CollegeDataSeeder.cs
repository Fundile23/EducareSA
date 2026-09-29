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

            var collegeData = new List<(string Name, string Province, string City, string Website, string Description)>
            {
                ("Coastal KZN TVET College", "KwaZulu-Natal", "Durban", "https://www.coastalkzn.co.za/",
                    "A public Technical and Vocational Education and Training college offering NCV, NATED and other vocational programmes."),
                ("Elangeni TVET College", "KwaZulu-Natal", "Pinetown", "https://www.efet.co.za/",
                    "A public TVET college serving learners across KwaZulu-Natal."),
                ("Esayidi TVET College", "KwaZulu-Natal", "Port Shepstone", "https://www.esayidifet.co.za/",
                    "A public TVET college serving communities in southern KwaZulu-Natal."),
                ("Majuba TVET College", "KwaZulu-Natal", "Newcastle", "https://www.majuba.edu.za/",
                    "A public TVET college offering vocational, engineering and business-related education and training."),
                ("Mnambithi TVET College", "KwaZulu-Natal", "Ladysmith", "https://www.mnambithicollege.co.za/",
                    "A public TVET college serving Ladysmith, Estcourt and surrounding communities."),
                ("Mthashana TVET College", "KwaZulu-Natal", "Vryheid", "https://www.mthashanafet.co.za/",
                    "A public TVET college serving communities in northern KwaZulu-Natal."),
                ("Thekwini TVET College", "KwaZulu-Natal", "Durban", "https://www.thekwinicollege.co.za/",
                    "A public TVET college serving learners in the Durban metropolitan area."),
                ("uMfolozi TVET College", "KwaZulu-Natal", "Richards Bay", "https://www.umfolozicollege.co.za/",
                    "A public TVET college serving northern KwaZulu-Natal."),
                ("Umgungundlovu TVET College", "KwaZulu-Natal", "Pietermaritzburg", "https://www.ufetc.edu.za/",
                    "A public TVET college serving the Midlands and surrounding areas of KwaZulu-Natal.")
            };

            foreach (var (name, province, city, website, description) in collegeData)
            {
                if (!await context.Colleges.AnyAsync(c => c.Name == name))
                {
                    context.Colleges.Add(new College
                    {
                        Name = name,
                        Province = province,
                        City = city,
                        WebsiteUrl = website,
                        Description = description,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            await context.SaveChangesAsync();

            // Look up college IDs by name (now populated by the DB)
            var coastal = await context.Colleges.FirstAsync(c => c.Name == "Coastal KZN TVET College");
            var mnambithi = await context.Colleges.FirstAsync(c => c.Name == "Mnambithi TVET College");
            var majuba = await context.Colleges.FirstAsync(c => c.Name == "Majuba TVET College");
            var umfolozi = await context.Colleges.FirstAsync(c => c.Name == "uMfolozi TVET College");

            // -----------------------------------------
            // PROGRAMMES
            // -----------------------------------------

            var programmes = new List<CollegeProgramme>
            {
                // =====================================
                // COASTAL KZN
                // =====================================

                new CollegeProgramme
                {
                    CollegeId = coastal.CollegeId,
                    Name = "Information Technology and Computer Science",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9 or equivalent",
                    MinimumAPS = null,
                    RequiredSubjects = "Language; Life Orientation; Mathematics or Mathematical Literacy",
                    Duration = "3 years",
                    Description = "Vocational programme covering information technology and computer science."
                },
                new CollegeProgramme
                {
                    CollegeId = coastal.CollegeId,
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
                    CollegeId = coastal.CollegeId,
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
                    CollegeId = coastal.CollegeId,
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
                    CollegeId = coastal.CollegeId,
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
                    CollegeId = coastal.CollegeId,
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
                    CollegeId = coastal.CollegeId,
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
                    CollegeId = mnambithi.CollegeId,
                    Name = "Information Technology and Computer Science",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9-12 school report or approved PLP bridging programme",
                    RequiredSubjects = "English; Mathematics; Life Orientation",
                    Duration = "3 years",
                    Description = "NC(V) programme covering systems development, networking, programming and computer technology."
                },
                new CollegeProgramme
                {
                    CollegeId = mnambithi.CollegeId,
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
                    CollegeId = mnambithi.CollegeId,
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
                    CollegeId = mnambithi.CollegeId,
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
                    CollegeId = majuba.CollegeId,
                    Name = "Engineering and Related Design",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 9, 10, 11 or 12 certificate or NQF Level 1 qualification",
                    RequiredSubjects = "English; Mathematics or Mathematical Literacy; Life Orientation",
                    Duration = "3 years",
                    Description = "Engineering and related design with specialisation options including fitting and turning, boilermaking, motor mechanics and welding."
                },
                new CollegeProgramme
                {
                    CollegeId = majuba.CollegeId,
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
                    CollegeId = majuba.CollegeId,
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
                    CollegeId = majuba.CollegeId,
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
                    CollegeId = umfolozi.CollegeId,
                    Name = "Engineering and Related Design",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "3 years",
                    Description = "NC(V) Engineering and Related Design programme."
                },
                new CollegeProgramme
                {
                    CollegeId = umfolozi.CollegeId,
                    Name = "Office Administration",
                    ProgrammeType = "NC(V)",
                    NQFLevel = "NQF Level 2-4",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "3 years",
                    Description = "NC(V) Office Administration programme."
                },
                new CollegeProgramme
                {
                    CollegeId = umfolozi.CollegeId,
                    Name = "Public Management",
                    ProgrammeType = "NATED",
                    NQFLevel = "N4-N6",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "18 months",
                    Description = "NATED Business Studies programme in Public Management."
                },
                new CollegeProgramme
                {
                    CollegeId = umfolozi.CollegeId,
                    Name = "Financial Management",
                    ProgrammeType = "NATED",
                    NQFLevel = "N4-N6",
                    MinimumEntryRequirement = "Grade 12",
                    Duration = "18 months",
                    Description = "NATED Business Studies programme in Financial Management."
                },
                new CollegeProgramme
                {
                    CollegeId = umfolozi.CollegeId,
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
                bool exists = await context.CollegeProgrammes
                    .AnyAsync(p => p.CollegeId == programme.CollegeId && p.Name == programme.Name);

                if (!exists)
                {
                    programme.IsActive = true;
                    context.CollegeProgrammes.Add(programme);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}