using Dashboard.Data;
using Dashboard.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajouter les services nécessaires
builder.Services.AddDistributedMemoryCache();  // Cache en mémoire pour la session
// Ajouter IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Durée de la session
    options.Cookie.HttpOnly = true;  // Sécuriser les cookies
    options.Cookie.IsEssential = true;  // Utilisation essentielle des cookies
});
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login"; // Redirection vers Login si non connecté
        options.AccessDeniedPath = "/Auth/AccessDenied"; // (optionnel)
        options.LogoutPath = "/Auth/Logout"; // Redirection vers Logout
    });


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<ILineService, LineService>();
builder.Services.AddScoped<IHeaderService, HeaderService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();



var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<AppDbContext>();
// Initialize the database
//await context.Database.EnsureCreatedAsync();
await context.Database.MigrateAsync();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseSession();
app.UseAuthentication();    
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
