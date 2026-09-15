using EducareSA.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<EducareDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EducareConnection")));

// Identity
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<EducareDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// MVC
builder.Services.AddControllersWithViews();

// App services
builder.Services.AddScoped<EducareSA.Services.IApsCalculator, EducareSA.Services.ApsCalculator>();
builder.Services.AddScoped<EducareSA.Services.IEligibilityService, EducareSA.Services.EligibilityService>();
builder.Services.AddScoped<EducareSA.Services.IStudentProvisioningService,
                           EducareSA.Services.StudentProvisioningService>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Seed database (catalogue + identity)
// Seed database (catalogue + identity + university JSON)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var env = services.GetRequiredService<IWebHostEnvironment>();

    var db = services.GetRequiredService<EducareDbContext>();
    await SeedData.SeedAsync(db);

    var jsonPath = Path.Combine(env.ContentRootPath, "Data", "Seed", "universities.json");
    await UniversityJsonSeeder.SeedAsync(db, jsonPath);

    await IdentitySeeder.SeedAsync(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();