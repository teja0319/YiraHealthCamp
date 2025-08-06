using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.IO;
using YiraHealthCampManagerAPI.Context;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Interfaces.ServiceInterfaces;
using YiraHealthCampManagerAPI.Models.Account;
using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Repositories;
using YiraHealthCampManagerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register database context with connection string
builder.Services.AddDbContext<YiraDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YiraSqlConn")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    // You can configure Identity options here if needed
})
.AddEntityFrameworkStores<YiraDbContext>() // <-- Correct context here
.AddDefaultTokenProviders();

#region Services
builder.Services.AddScoped<IAccountService, AccountService>(); 
builder.Services.AddScoped<IHealthCampService, HealthCampService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
#endregion

#region Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IHealthCampRepository, HealthCampRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
#endregion

var jwtCred = new JWTTokenConfigCreds();
builder.Configuration.Bind("JWTTokenConfig", jwtCred);
builder.Services.AddSingleton(jwtCred);


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//  Build the application before using 'app'
var app = builder.Build();

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("en-IN")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Ensure 'wwwroot/Content/ProfileImages/' directory exists
var profileImagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "ProfileImages");
if (!Directory.Exists(profileImagesPath))
{
    Directory.CreateDirectory(profileImagesPath);
}

// Enable serving static files from the custom directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(profileImagesPath),
    RequestPath = "/Content/ProfileImages"
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();