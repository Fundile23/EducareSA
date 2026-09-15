using System.Text.Json;
using EducareSA.Models;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Data
{
    public static class UniversityJsonSeeder
    {
        private class CampusDto
        {
            public string Name { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Province { get; set; } = string.Empty;
            public string? HeroImageUrl { get; set; }
        }

        private class UniversityDto
        {
            public string Name { get; set; } = string.Empty;
            public string ShortName { get; set; } = string.Empty;
            public string Province { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string? WebsiteUrl { get; set; }
            public string? LogoUrl { get; set; }
            public string? Description { get; set; }
            public List<CampusDto> Campuses { get; set; } = new();
        }

        public static async Task SeedAsync(EducareDbContext context, string jsonPath)
        {
            if (!File.Exists(jsonPath))
                return;

            var json = await File.ReadAllTextAsync(jsonPath);
            var dtos = JsonSerializer.Deserialize<List<UniversityDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (dtos == null || dtos.Count == 0)
                return;

            foreach (var dto in dtos)
            {
                // Skip if a university with this name already exists
                var exists = await context.Universities
                    .AnyAsync(u => u.Name == dto.Name);
                if (exists)
                    continue;

                var university = new University
                {
                    Name = dto.Name,
                    ShortName = dto.ShortName,
                    Province = dto.Province,
                    City = dto.City,
                    WebsiteUrl = dto.WebsiteUrl,
                    LogoUrl = dto.LogoUrl,
                    Description = dto.Description,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.Universities.Add(university);
                await context.SaveChangesAsync();

                foreach (var campusDto in dto.Campuses)
                {
                    context.Campuses.Add(new Campus
                    {
                        UniversityId = university.UniversityId,
                        Name = campusDto.Name,
                        City = campusDto.City,
                        Province = campusDto.Province,
                        HeroImageUrl = campusDto.HeroImageUrl,
                        IsActive = true
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}