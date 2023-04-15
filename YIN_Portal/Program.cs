using YIN_Portal.Data;
using YIN_Portal._keenthemes;
using YIN_Portal._keenthemes.libs;
using Microsoft.AspNetCore.Identity;
using YIN_Portal.Models;
using Microsoft.AspNetCore.Components.Authorization;
using YIN_Portal.Areas.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, ApplicationRoles>(options => options.SignIn.RequireConfirmedAccount = true)
       .AddUserManager<UserManager<ApplicationUser>>()
       .AddSignInManager<SignInManager<ApplicationUser>>()
       .AddRoles<ApplicationRoles>()
       .AddDefaultTokenProviders()
       .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IKTTheme, KTTheme>();
builder.Services.AddSingleton<IBootstrapBase, BootstrapBase>();

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("themesettings.json")
                            .Build();

var app = builder.Build();

KTThemeSettings.init(configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
