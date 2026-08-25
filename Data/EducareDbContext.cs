using EducareSA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Data
{
    public class EducareDbContext : IdentityDbContext
    {
        public EducareDbContext(
            DbContextOptions<EducareDbContext> options)
            : base(options)
        {
        }

        public DbSet<University> Universities { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ProgrammeSubjectRequirement>
            ProgrammeSubjectRequirements
        { get; set; }

        public DbSet<AdmissionRequirement>
            AdmissionRequirements
        { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentSubjectResult>
            StudentSubjectResults
        { get; set; }

        public DbSet<ProgrammeFee> ProgrammeFees { get; set; }

        public DbSet<ApplicationPeriod>
            ApplicationPeriods
        { get; set; }

        public DbSet<Module> Modules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Decimal precision
            modelBuilder.Entity<AdmissionRequirement>()
                .Property(a => a.MinimumAPS)
                .HasPrecision(5, 2);

            modelBuilder.Entity<AdmissionRequirement>()
                .Property(a => a.MinimumPointScore)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ApplicationPeriod>()
                .Property(a => a.ApplicationFee)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Programme>()
                .Property(p => p.DurationYears)
                .HasPrecision(4, 2);

            modelBuilder.Entity<ProgrammeFee>()
                .Property(f => f.Amount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<ProgrammeSubjectRequirement>()
                .Property(r => r.MinimumPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<StudentSubjectResult>()
                .Property(r => r.Percentage)
                .HasPrecision(5, 2);

            // University → Campus
            modelBuilder.Entity<Campus>()
                .HasOne(c => c.University)
                .WithMany(u => u.Campuses)
                .HasForeignKey(c => c.UniversityId)
                 .OnDelete(DeleteBehavior.Restrict);

            // University → Faculty
            modelBuilder.Entity<Faculty>()
                .HasOne(f => f.University)
                .WithMany(u => u.Faculties)
                .HasForeignKey(f => f.UniversityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Faculty → Programme
            modelBuilder.Entity<Programme>()
                .HasOne(p => p.Faculty)
                .WithMany(f => f.Programmes)
                .HasForeignKey(p => p.FacultyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Campus → Programme
            modelBuilder.Entity<Programme>()
                .HasOne(p => p.Campus)
                .WithMany(c => c.Programmes)
                .HasForeignKey(p => p.CampusId)
                .OnDelete(DeleteBehavior.SetNull);

            // Programme → Subject Requirements
            modelBuilder.Entity<ProgrammeSubjectRequirement>()
                .HasOne(r => r.Programme)
                .WithMany(p => p.SubjectRequirements)
                .HasForeignKey(r => r.ProgrammeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subject → Subject Requirements
            modelBuilder.Entity<ProgrammeSubjectRequirement>()
                .HasOne(r => r.Subject)
                .WithMany(s => s.ProgrammeRequirements)
                .HasForeignKey(r => r.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Programme → Admission Requirements
            modelBuilder.Entity<AdmissionRequirement>()
                .HasOne(r => r.Programme)
                .WithMany(p => p.AdmissionRequirements)
                .HasForeignKey(r => r.ProgrammeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Student → Results
            modelBuilder.Entity<StudentSubjectResult>()
                .HasOne(r => r.Student)
                .WithMany(s => s.SubjectResults)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subject → Results
            modelBuilder.Entity<StudentSubjectResult>()
                .HasOne(r => r.Subject)
                .WithMany(s => s.StudentResults)
                .HasForeignKey(r => r.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Programme → Fees
            modelBuilder.Entity<ProgrammeFee>()
                .HasOne(f => f.Programme)
                .WithMany(p => p.Fees)
                .HasForeignKey(f => f.ProgrammeId)
                .OnDelete(DeleteBehavior.Cascade);

            // University → Application Period
            modelBuilder.Entity<ApplicationPeriod>()
                .HasOne(a => a.University)
                .WithMany(u => u.ApplicationPeriods)
                .HasForeignKey(a => a.UniversityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Programme → Application Period
            modelBuilder.Entity<ApplicationPeriod>()
                .HasOne(a => a.Programme)
                .WithMany(p => p.ApplicationPeriods)
                .HasForeignKey(a => a.ProgrammeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Programme → Modules
            modelBuilder.Entity<Module>()
                .HasOne(m => m.Programme)
                .WithMany(p => p.Modules)
                .HasForeignKey(m => m.ProgrammeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate programme requirements
            modelBuilder.Entity<ProgrammeSubjectRequirement>()
                .HasIndex(r => new
                {
                    r.ProgrammeId,
                    r.SubjectId
                })
                .IsUnique();

            // Prevent duplicate student subject results
            modelBuilder.Entity<StudentSubjectResult>()
                .HasIndex(r => new
                {
                    r.StudentId,
                    r.SubjectId,
                    r.AcademicYear
                })
                .IsUnique();
        }
    }
}