using Dashboard.Data;
using Dashboard.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajouter les services n�cessaires
builder.Services.AddDistributedMemoryCache();  // Cache en m�moire pour la session
// Ajouter IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Dur�e de la session
    options.Cookie.HttpOnly = true;  // S�curiser les cookies
    options.Cookie.IsEssential = true;  // Utilisation essentielle des cookies
});
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login"; // Redirection vers Login si non connect�
        options.AccessDeniedPath = "/Auth/AccessDenied"; // (optionnel)
        options.LogoutPath = "/Auth/Logout"; // Redirection vers Logout
    });


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IDashboardParamService, DashboardParamService>();

builder.Services.AddScoped<IDashboardService, DashboardService>();



var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<AppDbContext>();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseSession();
app.UseStaticFiles();
app.UseAuthentication();    
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
    


app.Run();
